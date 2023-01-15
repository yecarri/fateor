using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class QuestionViewModel
    {
        public int? QuestionId { get; set; }

        [DisplayName("Tipo de Pregunta")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int QuestionTypeId { get; set; }

        public int? QuestionnaireId { get; set; }

        [DisplayName("Pregunta")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? QuestionText { get; set; }

        public List<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();

        public int Ord { get; set; }
    }
}