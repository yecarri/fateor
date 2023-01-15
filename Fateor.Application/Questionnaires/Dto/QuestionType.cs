using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_questiontype")]
    public class QuestionType
    {
        [Key]
        public int QuestionTypeId { get; set; }
        public string? QuestionTypeName { get; set; }
    }
}
