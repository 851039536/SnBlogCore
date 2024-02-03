using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnVideoType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
