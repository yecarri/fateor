using System.ComponentModel.DataAnnotations;

namespace Fateor.Models
{
    public class RequestPasswordResetViewModel : ViewModelBase, IValidatableObject
    {
        public string? Login { get; set; } 

        [EmailAddress]
        public string? Email { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Login) && string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult("Por favor, indique su login o su email", new List<string> { nameof(Login), nameof(Email) });
            }
        }
    }
}
