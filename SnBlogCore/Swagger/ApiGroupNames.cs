namespace SnBlogCore.Swagger
{
    /// <summary>
    /// 接口分组枚举
    /// </summary>
    public enum ApiGroupNames
    {
        /// <summary>
        /// 登录相关接口
        /// </summary>
        [GroupInfo(Title = "登录认证",Description = "登录相关接口",Version = "v1")]
        Login, 
        /// <summary>
        /// v1版本
        /// </summary>
        [GroupInfo(Title = "v1",Description = "v1版本接口")]
        v1
    }
}
