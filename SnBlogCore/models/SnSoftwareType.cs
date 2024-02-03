﻿using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnSoftwareType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SnSoftware> SnSoftwares { get; set; } = new List<SnSoftware>();
}
