using Abstraction.Command;
using MediatR;

namespace ArticleService.Abstraction.Command
{
    public class UpdateArticleCommand : CommandBase<Unit>
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
        public string PublishDate { get; set; }
        public short? StarCount { get; set; }
    }
}