using System;
using System.Collections.Generic;
using Abstraction.Dto;
using ArticleService.Domain.Entities;

namespace ArticleService.UnitTest.Tests
{
    public class ArticleTestBase : TestBase.TestBase
    {
        public List<Article> FakeArticles = new()
        {
            new()
            {
                ArticleId = 1,
                Title = "Totem und Tabu",
                Author = "Sigmund Freud",
                ArticleContent = "Totem und Tabu",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 2,
                Title = "Madde ve Bellek",
                Author = "Henri Bergson",
                ArticleContent = "Madde ve Bellek",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 3,
                Title = "Moses and Monotheism",
                Author = "Sigmund Freud",
                ArticleContent = "Moses and Monotheism",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 4,
                Title = "Little Prince",
                Author = "Antoine de saint exupery",
                ArticleContent = "Little Prince",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 5,
                Title = "Lage de raison",
                Author = "Jean-Paul Sartre",
                ArticleContent = "Lage de raison",
                PublishDate = new DateTime(),
                StarCount = 5
            }
        };

        public List<ArticleDto> FakeArticleDtoList = new()
        {
            new()
            {
                ArticleId = 1,
                Title = "Totem und Tabu",
                Author = "Sigmund Freud",
                ArticleContent = "Totem und Tabu",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 2,
                Title = "Madde ve Bellek",
                Author = "Henri Bergson",
                ArticleContent = "Madde ve Bellek",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 3,
                Title = "Moses and Monotheism",
                Author = "Sigmund Freud",
                ArticleContent = "Moses and Monotheism",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 4,
                Title = "Little Prince",
                Author = "Antoine de saint exupery",
                ArticleContent = "Little Prince",
                PublishDate = new DateTime(),
                StarCount = 5
            },
            new()
            {
                ArticleId = 5,
                Title = "Lage de raison",
                Author = "Jean-Paul Sartre",
                ArticleContent = "Lage de raison",
                PublishDate = new DateTime(),
                StarCount = 5
            }
        };
    }
}