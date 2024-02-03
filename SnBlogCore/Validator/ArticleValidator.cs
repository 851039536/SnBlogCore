using FluentValidation;
using SnBlogCore.models;

namespace SnBlogCore.Validator
{
    public class ArticleValidator : AbstractValidator<Article> //验证实体
    {
        public ArticleValidator()
        {   //规则
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Sketch).NotEmpty();
            RuleFor(x => x.Text).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Img).NotEmpty();
        }
    }
}
