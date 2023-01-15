using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class Response
    {
        public int ResponseId { get; set; }

        public int QuestionnaireId { get; set; }

        public string? Questionnaire { get; set; }

        public int MemberId { get; set; }

        public string? Member { get; set; }

        public DateTime LastModificationDate { get; set; }

        public int LastModificationUserId { get; set; }

        public string? LastModificationUser { get; set; }

        public IEnumerable<Answer>? Answers { get; set; }
    }
}
