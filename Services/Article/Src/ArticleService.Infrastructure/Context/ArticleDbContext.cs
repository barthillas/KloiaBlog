using System;
using ArticleService.Domain.Entities;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Infrastructure.Context
{
    public class ArticleDbContext : DbContextBase
    {
        public ArticleDbContext() { }
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        { }
        
        public virtual DbSet<Article> Articles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=localhost;Database=Articles;User Id=sa;Password=kloia12345!@#$%;"; 
            var envConnectionString = Environment.GetEnvironmentVariable("envConnectionString");
            if (!string.IsNullOrEmpty(envConnectionString))
            {
                connectionString = envConnectionString;
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Article>().HasData(
                new Article{
                    ArticleId = 1,
                    Title = "Totem und Tabu",
                    Author = "Sigmund Freud",
                    ArticleContent = "Totem und Tabu",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 2,
                    Title = "Madde ve Bellek",
                    Author = "Henri Bergson",
                    ArticleContent = "Madde ve Bellek",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 3,
                    Title = "Moses and Monotheism",
                    Author = "Sigmund Freud",
                    ArticleContent = "Moses and Monotheism",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 4,
                    Title = "Little Prince",
                    Author = "Antoine de saint exupery",
                    ArticleContent = "Little Prince",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 5,
                    Title = "Lage de raison",
                    Author = "Jean-Paul Sartre",
                    ArticleContent = "Lage de raison",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 6,
                    Title = "ostre sledovane vlaky rozbor",
                    Author = "Bohumil Hrabal",
                    ArticleContent = "ostre sledovane vlaky rozbor",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 7, 
                    Title = "Ningen Shikkaku",
                    Author = "Osamu Dazai",
                    ArticleContent = "Ningen Shikkaku",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                });
             
            base.OnModelCreating(builder);
        }
    }
}