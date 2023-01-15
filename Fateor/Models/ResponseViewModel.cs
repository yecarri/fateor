using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class ResponseViewModel : ViewModelBase
    {
        [DisplayName("Respuesta")]
        public int? ResponseId { get; set; }

        [Required]
        [DisplayName("Cuestionario")]
        public int? QuestionnaireId { get; set; }

        [DisplayName("Cuestionario")]
        public string? Questionnaire { get; set; }

        [Required, Range(1, int.MaxValue)]
        [DisplayName("Miembro")]
        public int? MemberId { get; set; }

        [DisplayName("Miembro")]
        public string Member { get; set; } = string.Empty;

        [DisplayName("Fecha")]
        public DateTime LastModificationDate { get; set; }

        [DisplayName("Id de Usuario")]
        public int LastModificationUserId { get; set; }

        [DisplayName("Completado por")]
        public string LastModificationUser { get; set; } = string.Empty;

        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();

        public List<Application.Questionnaires.Entities.Questionnaire> Questionnaires { get; set; } = new List<Application.Questionnaires.Entities.Questionnaire>();
        public bool Locked { get; set; }
    }
}