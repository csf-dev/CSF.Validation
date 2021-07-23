using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    public interface IValidator
    {
        Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }

    public interface IValidator<TValidated>
    {
        Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }
}