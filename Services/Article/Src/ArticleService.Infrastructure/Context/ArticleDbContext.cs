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
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=Articles;User Id=sa;Password=kloia12345!@#$%;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Article>().HasData(
                new Article{
                    ArticleId = 1, 
                    Author = "Freud",
                    ArticleContent = "Totem und Tabu",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                },
                new Article{
                    ArticleId = 2, 
                    Author = "Moses and Monotheism",
                    ArticleContent = "Freud",
                    PublishDate = new DateTime(),
                    StarCount = 5,
                });
             
            base.OnModelCreating(builder);
        }
    }
}