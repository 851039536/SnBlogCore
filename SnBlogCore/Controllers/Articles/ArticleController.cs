using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SnBlogCore.models;
using SnBlogCore.Swagger;

namespace SnBlogCore.Controllers.Articles;

/// <summary>
/// 文档
/// </summary>
[ApiGroup(ApiGroupNames.v1)]
[ApiController] //控制路由
[Route("article")]
public class ArticleController : ControllerBase
{
    //服务
    private readonly IArticleService _service;

    private readonly IValidator<Article> _validator;
    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service"></param>
    public ArticleController(IServiceProvider service,IValidator<Article> validator)
    {
        _service = service.GetRequiredService<IArticleService>();
        _validator = validator;
    }

    #endregion

    #region 查询总数

    /// <summary>
    /// 查询总数
    /// </summary>
    /// <returns></returns>
    [HttpGet("sum")]
    // [EnableRateLimiting("TokenLimit")]
    [ResponseCache(Duration = 10)]
    public async Task<IActionResult> GetSumAsync()
    {
        int data = await _service.GetSumAsync();
        return Ok(data);
    }

    /// <summary>
    /// 查询所有 
    /// </summary>
    /// <param name="cache">是否开启缓存</param>
    /// <returns>list-entity</returns>
    [Authorize]
    //[ApiGroup(ApiGroupNames.v1)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync(bool cache = false)
    {

        var data = await _service.GetAllAsync(cache);
        return Ok(data);
    }
    #endregion
}
