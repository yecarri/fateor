using Fateor.Application.Questionnaires.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class QuestionnaireViewModel : ViewModelBase
    {
        public string? Tags { get; set; }

        public int QuestionnaireId { get; set; }

        [DisplayName("Título del Cuestionario")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? QuestionnaireName { get; set; }

        [DisplayName("URL al finalizar el questionario")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? RedirectUrl { get; set; }

        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();

        public IEnumerable<QuestionType>? QuestionTypes { get; set; }
    }
}