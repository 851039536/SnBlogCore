using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class PhotoType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
