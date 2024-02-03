using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class ArticleTag
{
    public int Id { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 标签描述
    /// </summary>
    public string Description { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
