using Abstraction.Command;
using Abstraction.Dto;
using MediatR;

namespace ArticleService.Abstraction.Command
{
    public class CreateArticleCommand : CommandBase<ArticleDto>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
        public string PublishDate { get; set; }
        public short? StarCount { get; set; }
    }
}