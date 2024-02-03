using AuthenticationTest;
using FluentValidation;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SnBlogCore.Controllers.Articles;
using SnBlogCore.models;
using SnBlogCore.Swagger;
using SnBlogCore.Validator;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

//注册Controller相关服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var configuration = builder.Configuration;

#region 跨域

string corsUrls = configuration["CorsUrls"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(corsUrls.Split(","))
                //添加预检请求过期时间
                .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                //如果不需要跨域请注释掉.AllowCredentials()或者增加跨域策略
                .AllowCredentials()
                .AllowAnyHeader().AllowAnyMethod();
        });
});

#endregion

#region 注册jwt认证服务

builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, //是否验证Issuer
            ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
            ValidateAudience = true, //是否验证Audience
            ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
            RequireExpirationTime = true,
        };
    });


//将 JwtHelper 注册为单例模式
builder.Services.AddSingleton(new JwtHelper(configuration));

#endregion

#region Swagger配置

builder.Services.AddSwaggerGen(c =>
{
    // 遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
    typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
    {
        //获取枚举值上的特性
        var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
        c.SwaggerDoc(f.Name, new OpenApiInfo
        {
            Title = info?.Title,
            Version = info?.Version,
            Description = info?.Description
        });
    });
    // 没有特性的接口分到NoGroup上
    c.SwaggerDoc("NoGroup", new OpenApiInfo
    {
        Title = "无分组"
    });
    // 判断接口归于哪个分组
    c.DocInclusionPredicate((docName, apiDescription) =>
    {
        if (docName == "NoGroup")
        {
            // 当分组为NoGroup时，只要没加特性的接口都属于这个组
            return string.IsNullOrEmpty(apiDescription.GroupName);
        }
        else
        {
            return apiDescription.GroupName == docName;
        }
    });


    #region 配置Authorization

    //Bearer 的scheme定义
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        //参数添加在头部
        In = ParameterLocation.Header,
        //使用Authorize头部
        Type = SecuritySchemeType.Http,
        //内容为以 bearer开头
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    //把所有方法配置为增加bearer头部信息
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                }
            },
            Array.Empty<string>()
        }
    };

    //注册到swagger中
    c.AddSecurityDefinition("Authorization", securityScheme);
    c.AddSecurityRequirement(securityRequirement);

    #endregion


    var file = Path.Combine(AppContext.BaseDirectory, "SnBlogCore.xml"); // xml文档绝对路径
    var path = Path.Combine(AppContext.BaseDirectory, file); // xml文档绝对路径
    c.IncludeXmlComments(path, true); // true : 显示控制器层注释
    c.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序，如果有多个，就可以看见效果了。
});

#endregion

#region 数据连接



builder.Services.AddDbContext<SnblogContext>(
    options => options
        .UseMySql(configuration["ConnectionStrings:MysqlConnection"], ServerVersion.Parse("8.0.33-mysql")));

#endregion

#region DI依赖注入配置

// 在ASP.NET Core中所有用到EF的Service 都需要注册成Scoped
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddTransient<IValidator<Article>, ArticleValidator>();
 
new CommonLibrary.ModulInit().Init(builder.Services);

#endregion

#region 实体映射自动化注册

builder.Services.AddAutoMapper(
    Assembly.Load("SnBlogCore").GetTypes()
        .Where(t => t.FullName != null && t.FullName.EndsWith("Mapper"))
        .ToArray()
);

#endregion

#region 限流

builder.Services.AddRateLimiter(_ => _
    //.AddFixedWindowLimiter(policyName: "fixed",options =>
    //{
    //    options.PermitLimit = 4; //窗口阈值，即每个窗口时间范围内，最多允许的请求个数。这里指定为最多允许4个请求。该值必须 > 0
    //    options.Window = TimeSpan.FromSeconds(5); // 窗口大小，即时间长度。该值必须 > TimeSpan.Zero
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //排队请求的处理顺序。这里设置为优先处理先来的请求
    //    options.QueueLimit = 3; //当窗口请求数达到最大时，后续请求会进入排队，用于设置队列的大小（即允许几个请求在里面排队等待）
    //}
    //.AddSlidingWindowLimiter("SlidingWindows",options =>
    //{
    //    options.QueueLimit = 2;
    //    options.PermitLimit = 4;
    //    options.Window = TimeSpan.FromSeconds(5);
    //    options.SegmentsPerWindow = 2;
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    //}
    .AddTokenBucketLimiter("TokenLimit", options =>
        {
            options.QueueLimit = 2;
            options.TokenLimit = 10; //令牌桶容量 多余会被丢弃 
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(5); //间隔多长时间放一次令牌
            options.TokensPerPeriod = 2; //每次向令牌桶放入的令牌数量
            options.AutoReplenishment = true;
        }
    ));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    #region UseSwaggerUI

    app.UseSwagger(); // `UseSwaggerUI` UseKnife4UI
    app.UseSwaggerUI(c =>
    {
        // 遍历ApiGroupNames所有枚举值生成接口文档
        typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
        {
            //获取枚举值上的特性
            var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
            c.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
        });
        c.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "无分组");
        c.RoutePrefix = string.Empty;
    });

    #endregion
}

app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();