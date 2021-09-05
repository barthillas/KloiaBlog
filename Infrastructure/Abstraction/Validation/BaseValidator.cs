using FluentValidation;

namespace Abstraction.Validation
{
    public abstract class BaseValidator<T> : AbstractValidator<T>, IBaseValidator<T>
    {
        protected BaseValidator()
        {
            CascadeMode = CascadeMode.Continue;
        }
    }
}
