using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Exceptions;
using FluentValidation;
using MediatR.Pipeline;
using ValidationException = Abstraction.Exceptions.ValidationException;

namespace ReviewService.Application.Processors
{
    public class ValidationRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationRequestPreProcessor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (!errors.Any()) return Task.CompletedTask;
            {
                var errorBuilder = new StringBuilder();
                var errorDetails = new List<ErrorDetail>();

                errorBuilder.AppendLine("Invalid request, reason: ");

                foreach (var error in errors)
                {
                    errorBuilder.AppendLine(error.ErrorMessage);
                    errorDetails.Add(new ErrorDetail(error.ErrorCode, error.ErrorMessage, error.PropertyName));
                }

                throw new ValidationException("Invalid request", errorDetails);
            }
        }
    }
}