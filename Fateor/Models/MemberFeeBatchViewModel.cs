using Fateor.Application;
using Fateor.Application.Organization.Entities;
using HtmlAgilityPack;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fateor.Models
{
    public class MemberFeeBatchViewModel : ViewModelBase
    {
        public int? MemberFeeBatchId { get; set; }

        [DisplayName("Título")]
        public string BatchTitle { get; set; } = "";

        [DisplayName("Desde")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Hasta")]
        public DateTime DateTo { get; set; }

        [DisplayName("Vencimiento")]
        public DateTime DateDue { get; set; }

        [DisplayName("Referecia")]
        public string ReferencePattern { get; set; } = "";

        [DisplayName("Periodicidad")]
        public Application.Organization.Periodicity Periodicity { get; set; } = Application.Organization.Periodicity.None;

        [DisplayName("Importe")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        [DisplayName("Estatus")]
        public Status[] Statuses { get; set; } = new Status[0];

        [DisplayName("Estatus")]
        public int[] SelectedStatuses { get; set; } = new int[0];

        [DisplayName("Cuotas")]
        public IList<MemberFee> MemberFees { get; set; } = new List<MemberFee>();

        public DateRange DateRange
        {
            get => new DateRange(DateFrom, DateTo);
            set
            {
                DateFrom =value.Item1;
                DateTo =value.Item2;
            }
        }
    }
}
