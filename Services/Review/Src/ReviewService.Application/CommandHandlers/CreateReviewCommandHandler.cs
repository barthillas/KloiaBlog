using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using Abstraction.Exceptions;
using Abstraction.Handler;
using Data.UnitOfWork;
using ReviewService.Abstraction.Command;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;

namespace ReviewService.Application.CommandHandlers
{
    public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;
        private readonly ODataClient _oDataClient;


        public CreateReviewCommandHandler(IUnitOfWork<ReviewDbContext> unitOfWork, ODataClient oDataClient)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review();
            ArticleDto article;
            try
            {
                var articleResponse = await _oDataClient
                    .For<ArticleDto>("Article")
                    .Filter(x => x.ArticleId == request.ArticleId).FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
                var articleDtos = articleResponse as ArticleDto[] ?? articleResponse.ToArray();
                if (!articleDtos.Any())
                    throw new BusinessException($"Article does not exist. ArticleId: {request.ArticleId}");
                article = articleDtos[0];
            }
            catch (BusinessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not access Review target machine");
            }

            review.Create(request.ArticleId, request.Reviewer, request.ReviewContent);
            await _unitOfWork.GetRepository<Review>().AddAsync(review, cancellationToken).ConfigureAwait(false);

            var result = new ReviewDto
            {
                Reviewer = review.Reviewer,
                ArticleId = review.ArticleId,
                ReviewContent = review.ReviewContent
            };
            if (article != null)
            {
                result.Article = new ArticleDto
                {
                    ArticleId = article.ArticleId,
                    Author = article.Author,
                    ArticleContent = article.ArticleContent,
                    Title = article.Title
                };
            }
            return result;
        }
    }
}