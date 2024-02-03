using SnBlogCore.AutoMapper.DTO;

namespace SnBlogCore.Controllers.Articles
{
    /// <summary>
    /// 文章接口
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// 查询总数 
        /// </summary>
        /// <param name="identity">所有:0|分类:1|标签:2|用户3</param>
        /// <param name="type">条件</param>
        /// <param name="cache">缓存</param>
        /// <returns>int</returns>
        Task<int> GetSumAsync();


        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cache">缓存</param>
        Task<List<ArticleDto>> GetAllAsync(bool cache);

    }
}
