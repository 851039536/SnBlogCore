using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnPicture
{
    public int Id { get; set; }

    /// <summary>
    /// 图床名
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 图片地址
    /// </summary>
    public string ImgUrl { get; set; } = null!;

    /// <summary>
    /// 分类
    /// </summary>
    public int TypeId { get; set; }

    public int UserId { get; set; }

    public virtual SnPictureType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
