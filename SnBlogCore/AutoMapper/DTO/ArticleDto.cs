﻿#nullable disable

using SnBlogCore.models;

namespace SnBlogCore.AutoMapper.DTO;

public partial class ArticleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sketch { get; set; }
    public string Text { get; set; }
    public short Read { get; set; }
    public short Give { get; set; }
    public string Img { get; set; }
    public short CommentId { get; set; }
    public int TagId { get; set; }
    public int TypeId { get; set; }
    public int UserId { get; set; }
    public DateTime TimeCreate { get; set; }
    public DateTime TimeModified { get; set; }

    public virtual ArticleTag Tag { get; set; }
    public virtual ArticleType Type { get; set; }
    public virtual User User { get; set; }
}
