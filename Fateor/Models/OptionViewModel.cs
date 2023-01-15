using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class OptionViewModel
    {
        public int? OptionId { get; set; }

        public int? QuestionId { get; set; }

        [DisplayName("Opción")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? OptionText { get; set; }

        [DisplayName("Puntaje")]
        public float? Points { get; set; }
        public int Ord { get; set; }
    }
}