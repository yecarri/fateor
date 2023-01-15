using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Questionnaires.Entities
{
    [Serializable]
    public class ResponseKey
    {
        public ResponseKey(int memberId, int questionnaireId)
        {
            MemberId = memberId;
            QuestionnaireId = questionnaireId;
        }

        public int MemberId { get; set; }

        public int QuestionnaireId { get; set; }
    }
}