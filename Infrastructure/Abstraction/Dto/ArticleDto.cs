using System;
using System.Collections.Generic;

namespace Abstraction.Dto
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
        
        public short StarCount { get; set; }
        public DateTime PublishDate { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new ();
    }
}