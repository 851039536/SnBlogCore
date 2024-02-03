using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class PhotoGallery
{
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = null!;

    public string Img { get; set; } = null!;

    public int TypeId { get; set; }

    public int UserId { get; set; }

    public string Tag { get; set; } = null!;

    public short Give { get; set; }

    public DateTime TimeCreate { get; set; }

    public DateTime TimeModified { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual PhotoGalleryType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
