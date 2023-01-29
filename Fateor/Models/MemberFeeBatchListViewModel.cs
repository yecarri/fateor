using Fateor.Application.Organization.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fateor.Models
{
    public class MemberFeeBatchListViewModel : ViewModelBase
    {
        [DisplayName("Lote")]
        public string? BatchTitle { get; set; }
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;
        [DisplayName("Estados")]
        public StatusSelectViewModel[]? Statuses{ get; set; }
        public MemberFeeBatch[]? Batches { get; set; }

    }
}
