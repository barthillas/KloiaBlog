using System.ComponentModel.DataAnnotations;
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

        public void Create(int articleId, string reviewer, string reviewContent)
        {
            ArticleId = articleId;
            Reviewer = reviewer;
            ReviewContent = reviewContent;
        }

        public void Update(int articleId, string reviewer, string reviewContent)
        {
            ArticleId = articleId;
            Reviewer = reviewer;
            ReviewContent = reviewContent;
        }
    }
}