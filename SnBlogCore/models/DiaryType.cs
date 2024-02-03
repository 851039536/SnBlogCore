using System;
using System.Collections.Generic;

namespace SnBlogCore.models;

/// <summary>
/// 日记分类
/// </summary>
public partial class DiaryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Diary> Diaries { get; set; } = new List<Diary>();
}
