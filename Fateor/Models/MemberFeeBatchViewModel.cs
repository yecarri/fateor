using Fateor.Application.Organization.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fateor.Models
{
    public class MemberFeeBatchViewModel : ViewModelBase
    {
        [DisplayName("Título")]
        public string BatchTitle { get; set; } = "";

        [DisplayName("Desde")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Hasta")]
        public DateTime DateTo { get; set; }

        [DisplayName("Referecia")]
        public string ReferencePattern { get; set; } = "";

        [DisplayName("Periodicidad")]
        public int Periodicity { get; set; } = 1;

        [DisplayName("Importe")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        [DisplayName("Estatus")]
        public Status[] Statuses { get; set; } = new Status[0];

        [DisplayName("Estatus")]
        public int[] SelectedStatuses { get; set; } = new int[0];

        [DisplayName("Cuotas")]
        public IEnumerable<MemberFee> MemberFees { get; set; } = new List<MemberFee>();
    }
}
