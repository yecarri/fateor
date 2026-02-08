using System.ComponentModel.DataAnnotations;

namespace Fateor.Models
{
    public class MemberLoginViewModel: ViewModelBase
    {
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
