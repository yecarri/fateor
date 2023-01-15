using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class Questionnaire
    {
        public int QuestionnaireId { get; set; }
        public string? QuestionnaireName { get; set; }
        public string? RedirectUrl { get; set; }
        public IList<Question>? Questions { get; set; }
    }
}
