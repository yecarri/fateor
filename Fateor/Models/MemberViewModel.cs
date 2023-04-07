using Fateor.Application.Location.Entities;
using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class MemberViewModel : ViewModelBase
    {
        [DisplayName("Id")]
        public int MemberId { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(240)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string GivenName { get; set; } = "";

        [DisplayName("Apellido")]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string Surname { get; set; } = "";

        [DisplayName("Tipo de Documento")]
        [Required(ErrorMessage = "{0} es requerido")]
        public int IdentityDocumentTypeId { get; set; }

        [DisplayName("Número de Documento")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string IdentityDocumentNumber { get; set; } = "";

        [DisplayName("Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "{0} es requerido")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(15)]
        [RegularExpression(@"^(\+(?:1|1\s?\d{3}|\d{2,3}))\s?(\(\d{1,4}\))?\s?(?=.{6,13})(\d{6,13}|\d{1,4}\s\d{4,7})$", ErrorMessage = "Incluye el prefijo del país, por ejemplo: +58 (212) 678456123, +58 414 7891234 o +1 (123) 4567890")]
        public string Phone { get; set; } = "";

        [DisplayName("Dirección")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string Address1 { get; set; } = "";

        [DisplayName("Dirección (adicional)")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string? Address2 { get; set; }

        [DisplayName("Ciudad")]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string City { get; set; } = "";

        [DisplayName("Código Postal")]
        [MaxLength(20)]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string? PostCode { get; set; }

        [DisplayName("Estado/Región")]
        [Required(ErrorMessage = "{0} es requerido")]
        public int? RegionId { get; set; }

        [DisplayName("País")]
        [Required(ErrorMessage = "{0} es requerido")]
        public int? CountryId { get; set; }

        [DisplayName("Credencial")]
        [RegularExpression(@"^[^\s].*[^\s]$")]
        public string? MemberCode { get; set; }

        [DisplayName("Miembro Borrado")]
        public bool IsDeleted { get; set; }

        [DisplayName("Historial")]
        public IList<MemberStatus>? MemberStatuses { get; set; }

        [DisplayName("Respuestas de los Cuestionarios")]
        public IList<ResponseViewModel>? Responses { get; set; }

        public IList<Region>? Regions { get; set; }

        public IList<Country>? Countries { get; set; }

        public IList<IdentityDocumentType>? DocumentTypes { get; set; }

        public bool New { get; set; } = false;
    }
}