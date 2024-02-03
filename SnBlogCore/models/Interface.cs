using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

public partial class Interface
{
    public int Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 类别
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 显示隐藏
    /// </summary>
    public bool Identity { get; set; }

    public virtual InterfaceType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
