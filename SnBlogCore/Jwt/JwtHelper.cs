using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationTest;


/// <summary>
/// 生成 JWT 的 Token
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration"></param>
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 根据用户名称生成token
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string CreateToken(string name)
    {
        // 1. 定义需要使用到的Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, name), //HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, "r_admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Username", name),
            //new Claim("Name", "超级管理员")
        };

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey,algorithm);
        // 获取当前时间
        DateTime now = DateTime.UtcNow;
        var expiration = _configuration["Jwt:Expiration"];
        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],     //签发者
            _configuration["Jwt:Audience"],   //生成token
            claims,                          //jwt令牌数据体
            DateTime.Now,                    //notBefore
            now.Add(TimeSpan.FromMinutes(int.Parse(expiration))),    ////令牌过期时间 Expiration
            signingCredentials               ////为数字签名定义SecurityKey    
        );

        // 6. 将token变为string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}
