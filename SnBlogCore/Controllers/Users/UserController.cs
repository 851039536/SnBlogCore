using Microsoft.AspNetCore.Mvc;
using SnBlogCore.models;
using AuthenticationTest;
using SnBlogCore.Swagger;

namespace Snblog.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly SnblogContext _coreDbContext;
        private readonly JwtHelper _jwtModel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="coreDbContext"></param>
        /// <param name="jwtModel"></param>
        public UserController(SnblogContext coreDbContext,JwtHelper jwtModel)
        {
            _coreDbContext = coreDbContext;
            _jwtModel = jwtModel;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="pwd">密码</param>
        /// <returns>Nickname,token,id,name</returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ApiGroup(ApiGroupNames.Login)]
        public IActionResult Login( string user,string pwd)
        {

            // 如果用户名和密码为空，则返回错误信息
            if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(pwd)) return Ok("用户密码不能为空");
            // 查询用户信息
            var ret = _coreDbContext.Users.FirstOrDefault(u => u.Name == user && u.Pwd == pwd);
            if (ret == null) return BadRequest("用户或密码错误");

            // 生成token
            string token = _jwtModel.CreateToken(user);
            // 返回用户信息和token
            return Ok(new { ret.Nickname,Token = token,ret.Id,ret.Name });
        }
    }
}
