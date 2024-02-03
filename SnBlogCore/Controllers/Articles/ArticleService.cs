using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnBlogCore.AutoMapper.DTO;
using SnBlogCore.models;
using System.Linq.Expressions;

namespace SnBlogCore.Controllers.Articles
{
    public class ArticleService : IArticleService
    {

        //服务
        private readonly SnblogContext _service;
        // 创建一个字段来存储mapper对象
        private readonly IMapper _mapper;

        /// <summary>
        /// 使用了IServiceProvider接口来获取所需的服务
        ///它定义了一个方法GetRequiredService，该方法可以用于获取指定类型的服务
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ArticleService(IServiceProvider serviceProvider,IMapper mapper)
        {
            // 获取服务提供程序中的实例
            _service = serviceProvider.GetRequiredService<SnblogContext>();
            _mapper = mapper;
        }
        public async Task<List<ArticleDto>> GetAllAsync(bool cache)
        {
            var data = await _service.Articles.ToListAsync();
            return _mapper.Map<List<ArticleDto>>(data);
        }

        public async Task<int> GetSumAsync()
        {

            return await GetArticleCountAsync();
        }

        /// <summary>
        /// 获取文章的数量
        /// </summary>
        /// <param name="predicate">筛选文章的条件</param>
        /// <returns>返回文章的数量</returns>
        private async Task<int> GetArticleCountAsync(Expression<Func<Article,bool>> predicate = null)
        {
            var query = _service.Articles.AsNoTracking();
            //如果有筛选条件
            if (predicate != null) query = query.Where(predicate);

            try
            {
                int count = await query.CountAsync();
                return count;
            } catch
            {
                return -1;
            }
        }

    }
}