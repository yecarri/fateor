using Fateor.Application.Questionnaires.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class ResponseListViewModel : ViewModelBase
    {
        [DisplayName("Cuestionario")]
        public int? QuestionnaireId { get; set; }
        [DisplayName("Miembro")]
        public int? MemberId { get; set; }
        [DisplayName("Miembro")]
        public string Member { get; set; }
        [DisplayName("Fecha Desde")]
        public DateTime? DateFrom { get; set; }
        [DisplayName("Fecha Hasta")]
        public DateTime? DateTo { get; set; }
        public int? CurrentPage { get; set; }
        public List<Response> Responses { get; set; }
        public List<Questionnaire> Questionnaires { get; set; }
    }
}