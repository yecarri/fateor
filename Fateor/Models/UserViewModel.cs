using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Fateor.Models
{
    public class UserViewModel : ViewModelBase
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string Login { get; set; } = "";

        [Required(ErrorMessage = "{0} es requerido")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "{0} es requerido")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "{0} es requerido")]
        [DisplayName("Login Externo")]
        public bool IsExternal { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DisplayName("Usuario Eliminado")]
        public bool IsDeleted { get; set; }
    }
}
