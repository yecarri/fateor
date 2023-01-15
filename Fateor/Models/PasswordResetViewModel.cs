using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class PasswordResetViewModel : ViewModelBase
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(80)]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(80)]
        [DisplayName("Repite el password")]
        [Compare("Password", ErrorMessage = "El password no coincide")]
        public string Password2 { get; set; }

    }
}