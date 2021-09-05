using Abstraction.Command;
using MediatR;

namespace ReviewService.Abstraction.Command
{
    public class CreateReviewCommand : CommandBase<Unit>
    {
        public int ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
    }
}