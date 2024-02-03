using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

/// <summary>
/// 片段的历史版本表
/// </summary>
public partial class SnippetVersion
{
    public int Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 内容
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// 关联片段id
    /// </summary>
    public int SnippetId { get; set; }

    /// <summary>
    /// 版本变更次数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime TimeCreate { get; set; }
}
