using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class PhotoGalleryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PhotoGallery> PhotoGalleries { get; set; } = new List<PhotoGallery>();
}
