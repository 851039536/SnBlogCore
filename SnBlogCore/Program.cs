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

//ע��Controller��ط���
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var configuration = builder.Configuration;

#region ����

string corsUrls = configuration["CorsUrls"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(corsUrls.Split(","))
                //���Ԥ���������ʱ��
                .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                //�������Ҫ������ע�͵�.AllowCredentials()�������ӿ������
                .AllowCredentials()
                .AllowAnyHeader().AllowAnyMethod();
        });
});

#endregion

#region ע��jwt��֤����

builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, //�Ƿ���֤Issuer
            ValidIssuer = configuration["Jwt:Issuer"], //������Issuer
            ValidateAudience = true, //�Ƿ���֤Audience
            ValidAudience = configuration["Jwt:Audience"], //������Audience
            ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
            ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
            ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
            RequireExpirationTime = true,
        };
    });


//�� JwtHelper ע��Ϊ����ģʽ
builder.Services.AddSingleton(new JwtHelper(configuration));

#endregion

#region Swagger����

builder.Services.AddSwaggerGen(c =>
{
    // ����ApiGroupNames����ö��ֵ���ɽӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
    typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
    {
        //��ȡö��ֵ�ϵ�����
        var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
        c.SwaggerDoc(f.Name, new OpenApiInfo
        {
            Title = info?.Title,
            Version = info?.Version,
            Description = info?.Description
        });
    });
    // û�����ԵĽӿڷֵ�NoGroup��
    c.SwaggerDoc("NoGroup", new OpenApiInfo
    {
        Title = "�޷���"
    });
    // �жϽӿڹ����ĸ�����
    c.DocInclusionPredicate((docName, apiDescription) =>
    {
        if (docName == "NoGroup")
        {
            // ������ΪNoGroupʱ��ֻҪû�����ԵĽӿڶ����������
            return string.IsNullOrEmpty(apiDescription.GroupName);
        }
        else
        {
            return apiDescription.GroupName == docName;
        }
    });


    #region ����Authorization

    //Bearer ��scheme����
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) �����ṹ: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        //���������ͷ��
        In = ParameterLocation.Header,
        //ʹ��Authorizeͷ��
        Type = SecuritySchemeType.Http,
        //����Ϊ�� bearer��ͷ
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    //�����з�������Ϊ����bearerͷ����Ϣ
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

    //ע�ᵽswagger��
    c.AddSecurityDefinition("Authorization", securityScheme);
    c.AddSecurityRequirement(securityRequirement);

    #endregion


    var file = Path.Combine(AppContext.BaseDirectory, "SnBlogCore.xml"); // xml�ĵ�����·��
    var path = Path.Combine(AppContext.BaseDirectory, file); // xml�ĵ�����·��
    c.IncludeXmlComments(path, true); // true : ��ʾ��������ע��
    c.OrderActionsBy(o => o.RelativePath); // ��action�����ƽ�����������ж�����Ϳ��Կ���Ч���ˡ�
});

#endregion

#region ��������



builder.Services.AddDbContext<SnblogContext>(
    options => options
        .UseMySql(configuration["ConnectionStrings:MysqlConnection"], ServerVersion.Parse("8.0.33-mysql")));

#endregion

#region DI����ע������

// ��ASP.NET Core�������õ�EF��Service ����Ҫע���Scoped
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddTransient<IValidator<Article>, ArticleValidator>();
 
new CommonLibrary.ModulInit().Init(builder.Services);

#endregion

#region ʵ��ӳ���Զ���ע��

builder.Services.AddAutoMapper(
    Assembly.Load("SnBlogCore").GetTypes()
        .Where(t => t.FullName != null && t.FullName.EndsWith("Mapper"))
        .ToArray()
);

#endregion

#region ����

builder.Services.AddRateLimiter(_ => _
    //.AddFixedWindowLimiter(policyName: "fixed",options =>
    //{
    //    options.PermitLimit = 4; //������ֵ����ÿ������ʱ�䷶Χ�ڣ����������������������ָ��Ϊ�������4�����󡣸�ֵ���� > 0
    //    options.Window = TimeSpan.FromSeconds(5); // ���ڴ�С����ʱ�䳤�ȡ���ֵ���� > TimeSpan.Zero
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //�Ŷ�����Ĵ���˳����������Ϊ���ȴ�������������
    //    options.QueueLimit = 3; //�������������ﵽ���ʱ���������������Ŷӣ��������ö��еĴ�С���������������������Ŷӵȴ���
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
            options.TokenLimit = 10; //����Ͱ���� ����ᱻ���� 
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(5); //����೤ʱ���һ������
            options.TokensPerPeriod = 2; //ÿ��������Ͱ�������������
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
        // ����ApiGroupNames����ö��ֵ���ɽӿ��ĵ�
        typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
        {
            //��ȡö��ֵ�ϵ�����
            var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
            c.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
        });
        c.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "�޷���");
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