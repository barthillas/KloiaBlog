namespace Abstraction.Dto
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }
        public virtual ArticleDto Article { get; set; }
    }
}