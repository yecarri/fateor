using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_option")]
    public class Option
    {
        [Key]
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string? OptionText { get; set; }
        public int Ord { get; set; }
        public decimal? Points { get; set; }
        public int LastModificationUserId { get; set; }
        public DateTime LastModificationDate { get; set; }

    }
}
