using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class NavigationType
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<Navigation> Navigations { get; set; } = new List<Navigation>();
}
