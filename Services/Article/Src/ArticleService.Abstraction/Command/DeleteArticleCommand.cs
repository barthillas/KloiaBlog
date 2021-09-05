using Abstraction.Command;
using MediatR;

namespace ArticleService.Abstraction.Command
{
    public class DeleteArticleCommand : CommandBase<Unit>
    {
        public int ArticleId { get; set; }
    }
}