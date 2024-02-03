using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnPictureType
{
    public int Id { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<SnPicture> SnPictures { get; set; } = new List<SnPicture>();
}
