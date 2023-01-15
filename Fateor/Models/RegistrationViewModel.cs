using Org.BouncyCastle.Asn1.Esf;
using oSoft.Data.Annotation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class RegistrationViewModel : MemberViewModel
    {
        [MaxLength(80)]
        public string Login { get; set; } = string.Empty;

        [MaxLength(80)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(80)]
        [DisplayName("Repite el password")]
        [Compare("Password", ErrorMessage = "El password no coincide")]
        public string Password2 { get; set; } = string.Empty;

        [MustBeTrue("Es necesario que aceptes cumplir los Estatutos y Reglamentos")]
        public bool AcceptedMembershipConditions { get; set; }

        [MustBeTrue("Es necesario que aceptes la política de privacidad de datos")]
        public bool AcceptedGDPR { get; set; }

        [DisplayName("Ya estoy registrado en la intranet")]
        public bool AlreadyInIntranet { get; set; }

        public bool CreateExternalUser { get; set; }
    }
}