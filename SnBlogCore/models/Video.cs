using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class Video
{
    public int Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 图片
    /// </summary>
    public string Img { get; set; } = null!;

    /// <summary>
    /// 链接路径
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// 分类
    /// </summary>
    public int TypeId { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime TimeCreate { get; set; }

    public DateTime TimeModified { get; set; }

    public virtual SnVideoType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
