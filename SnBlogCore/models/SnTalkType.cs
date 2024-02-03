using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class SnTalkType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SnTalk> SnTalks { get; set; } = new List<SnTalk>();
}
