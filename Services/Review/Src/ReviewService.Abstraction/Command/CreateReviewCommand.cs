using Abstraction.Command;
using Abstraction.Dto;
using MediatR;

namespace ReviewService.Abstraction.Command
{
    public class CreateReviewCommand : CommandBase<ReviewDto>
    {
        public int ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
    }
}