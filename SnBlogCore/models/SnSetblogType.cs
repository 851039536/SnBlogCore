using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnSetblogType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SnSetblog> SnSetblogs { get; set; } = new List<SnSetblog>();
}
