using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class QuestionnaireListViewModel : ViewModelBase
    {
        public List<Application.Questionnaires.Entities.Questionnaire> Questionnaires { get; set; } = new List<Application.Questionnaires.Entities.Questionnaire>();
    }
}