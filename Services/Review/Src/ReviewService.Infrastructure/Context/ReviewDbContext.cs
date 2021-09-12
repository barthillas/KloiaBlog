using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;

namespace ReviewService.Infrastructure.Context
{
    public class ReviewDbContext : DbContextBase
    {
        public ReviewDbContext() : base() { }
        public ReviewDbContext(DbContextOptions opts) : base(opts) { }
        
        public virtual DbSet<Review> Reviews { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=localhost;Database=Reviews;User Id=sa;Password=kloia12345!@#$%;"; 
            var isDockerEvn = Environment.GetEnvironmentVariable("ISDOCKERENV");
            if (isDockerEvn != null)
            {
                connectionString = @"Server=mssql;Database=Reviews;User Id=sa;Password=kloia12345!@#$%;";
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Review>().HasData(
                new Review{
                    ReviewId = 1,
                    ArticleId = 1, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 2,
                    ArticleId = 2, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 3,
                    ArticleId = 1, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 4,
                    ArticleId = 4, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 5,
                    ArticleId = 5, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 6,
                    ArticleId = 2, 
                    Reviewer = "John Doe",
                    ReviewContent = "OMG perfect!!",
                },
                new Review{
                    ReviewId = 7,
                    ArticleId = 4, 
                    Reviewer = "Jane Doe",
                    ReviewContent = "OMG perfect!!",
                });
            base.OnModelCreating(builder);
        }
        
    }
}