using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SnBlogCore.Swagger
{
    /// <summary>
    /// 分组接口特性
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ApiGroupAttribute(ApiGroupNames name)
        {
            GroupName = name.ToString();
        }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
    }
}
