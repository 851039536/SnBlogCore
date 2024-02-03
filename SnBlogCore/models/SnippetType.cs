﻿using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnippetType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SnippetTypeSub> SnippetTypeSubs { get; set; } = new List<SnippetTypeSub>();

    public virtual ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
}
