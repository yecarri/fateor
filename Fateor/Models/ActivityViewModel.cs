using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class ActivityViewModel : ViewModelBase
    {
        public int ActivityId { get; set; }
        [DisplayName("Tipo de Actividad")]
        [Required(ErrorMessage = "El tipo de actividad es obligatorio")]
        public int ActivityTypeId { get; set; }
        [DisplayName("Fecha de la Actividad")]
        [Required(ErrorMessage = "La fecha de la actividad es obligatoria")]
        public DateTime ActivityDate { get; set; }
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; } = "";
        [DisplayName("Duración (minutos)")]
        [Required(ErrorMessage = "La duración es obligatoria")]
        public int DurationInMinutes { get; set; }
        [DisplayName("Participantes")]
        public IList<MemberActivityViewModel> Participants { get; set; }
        // public IEnumerable<ActivityType> ActivityTypes { get; set; }
        // public List<System.Web.Mvc.SelectListItem> Participation { get; } = new List<System.Web.Mvc.SelectListItem>()
        // {
        //     new System.Web.Mvc.SelectListItem() { Text="Activa", Value="1"},
        //     new System.Web.Mvc.SelectListItem() { Text="Pasiva", Value="2"},
        // };

        public bool New { get; set; }
    }
}