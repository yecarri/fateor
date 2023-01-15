using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class GetResponseTokenViewModel : ViewModelBase
    {
        [DisplayName("Miembro")]
        public int MemberId { get; set; }
        [DisplayName("Miembro")]
        public string Member { get; set; } = string.Empty;
        [DisplayName("Token")]
        public string Token { get; set; } = string.Empty;
        [DisplayName("Cuestionario")]
        public int QuestionnaireId { get; set; }
        public IEnumerable<Application.Questionnaires.Entities.Questionnaire> Questionnaires { get; set; } = Enumerable.Empty<Application.Questionnaires.Entities.Questionnaire>();
    }
}