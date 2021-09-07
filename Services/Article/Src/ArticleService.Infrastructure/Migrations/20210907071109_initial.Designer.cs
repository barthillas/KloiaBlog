﻿// <auto-generated />
using System;
using ArticleService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArticleService.Infrastructure.Migrations
{
    [DbContext(typeof(ArticleDbContext))]
    [Migration("20210907071109_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ArticleService.Domain.Entities.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArticleContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<short?>("StarCount")
                        .HasColumnType("smallint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArticleId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            ArticleId = 1,
                            ArticleContent = "Totem und Tabu",
                            Author = "Sigmund Freud",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Totem und Tabu"
                        },
                        new
                        {
                            ArticleId = 2,
                            ArticleContent = "Madde ve Bellek",
                            Author = "Henri Bergson",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Madde ve Bellek"
                        },
                        new
                        {
                            ArticleId = 3,
                            ArticleContent = "Moses and Monotheism",
                            Author = "Sigmund Freud",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Moses and Monotheism"
                        },
                        new
                        {
                            ArticleId = 4,
                            ArticleContent = "Little Prince",
                            Author = "Antoine de saint exupery",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Little Prince"
                        },
                        new
                        {
                            ArticleId = 5,
                            ArticleContent = "Lage de raison",
                            Author = "Jean-Paul Sartre",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Lage de raison"
                        },
                        new
                        {
                            ArticleId = 6,
                            ArticleContent = "ostre sledovane vlaky rozbor",
                            Author = "Bohumil Hrabal",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "ostre sledovane vlaky rozbor"
                        },
                        new
                        {
                            ArticleId = 7,
                            ArticleContent = "Ningen Shikkaku",
                            Author = "Osamu Dazai",
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StarCount = (short)5,
                            Title = "Ningen Shikkaku"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
