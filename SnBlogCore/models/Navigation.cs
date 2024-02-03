using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class Navigation
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 导航标题
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 标题描述
    /// </summary>
    public string Describe { get; set; } = null!;

    /// <summary>
    /// 图片路径
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

    /// <summary>
    /// 用户
    /// </summary>
    public int UserId { get; set; }

    public DateTime? TimeCreate { get; set; }

    public DateTime? TimeModified { get; set; }

    public virtual NavigationType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
