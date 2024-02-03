using Microsoft.AspNetCore.Mvc;
using SnBlogCore.models;

namespace SnBlogCore.Controllers.Uploading;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("uoload")]
public class UploadController : ControllerBase
{
    private readonly SnblogContext _coreDbContext;
    private IWebHostEnvironment _env; // 注入环境实例以便获取应用程序目录
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="coreDbContext"></param>
    public UploadController(SnblogContext coreDbContext, IWebHostEnvironment env)
    {
        _coreDbContext = coreDbContext;
        _env = env;
    }
}