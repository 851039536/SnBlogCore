﻿using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnippetTag
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
}
