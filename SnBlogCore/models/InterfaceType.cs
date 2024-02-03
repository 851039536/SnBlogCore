using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class InterfaceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Interface> Interfaces { get; set; } = new List<Interface>();
}
