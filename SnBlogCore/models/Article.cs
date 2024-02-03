namespace SnBlogCore.models;

public partial class Article
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 标题 
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 内容简述
    /// </summary>
    public string Sketch { get; set; } = null!;

    /// <summary>
    /// 博客内容
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// 阅读次数
    /// </summary>
    public short Read { get; set; }

    /// <summary>
    /// 点赞
    /// </summary>
    public short Give { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string Img { get; set; } = null!;

    /// <summary>
    /// 评论
    /// </summary>
    public short CommentId { get; set; }

    /// <summary>
    /// 标签外键
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// 分类外键
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// 用户外键id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 发表时间
    /// </summary>
    public DateTime TimeCreate { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime TimeModified { get; set; }

    public virtual ArticleTag Tag { get; set; } = null!;

    public virtual ArticleType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
