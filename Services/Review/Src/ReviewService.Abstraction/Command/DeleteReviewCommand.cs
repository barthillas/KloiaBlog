using Abstraction.Command;
using MediatR;

namespace ReviewService.Abstraction.Command
{
    public class DeleteReviewCommand : CommandBase<Unit>
    {
        public int ReviewId { get; set; }
    }
}