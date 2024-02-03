using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using SnBlogCore.models;

namespace SnBlogCore.Controllers.Uploading;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly SnblogContext _coreDbContext;
    private IWebHostEnvironment _env; // 注入环境实例以便获取应用程序目录

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="coreDbContext"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    public TestController(SnblogContext coreDbContext,IWebHostEnvironment env)
    {
        _coreDbContext = coreDbContext;
        _env = env;
    }

    /// <summary>
    /// 测试
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public string TestAppsettings([FromServices] IConfiguration configuration)
    {
        string t = configuration["test"];
        return $"test:{t}";
    }


    [HttpPost("TestModulInit")]
    public string TestModulInit([FromServices] Class1 class1)
    {
        return class1.test();
    }
}