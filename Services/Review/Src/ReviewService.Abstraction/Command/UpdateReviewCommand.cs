using Abstraction.Command;
using MediatR;

namespace ReviewService.Abstraction.Command
{
    public class UpdateReviewCommand : CommandBase<Unit>
    {
        public int ReviewId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
    }
}