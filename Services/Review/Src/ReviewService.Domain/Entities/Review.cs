using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.DDD;

namespace ReviewService.Domain.Entities
{
    public class Review : Entity
    {
        [Key]
        public int ReviewId { get; set; }
        public int ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }

        public void Add(int articleId, string reviewer, string reviewContent)
        {
            ArticleId = articleId;
            Reviewer = reviewer;
            ReviewContent = reviewContent;
        }

        public void Update(int reviewId, string reviewer, string reviewContent)
        {
            ReviewId = reviewId;
            Reviewer = reviewer;
            ReviewContent = reviewContent;
        }
    }
}