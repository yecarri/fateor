using Fateor.Application.Organization.Entities;
using System.ComponentModel;

namespace Fateor.Models
{
    public class MemberFeeBatchListViewModel : ViewModelBase
    {
        [DisplayName("Lote")]
        public string? BatchTitle { get; set; }
        [DisplayName("Fecha Desde")]
        public DateOnly DateFrom { get; set; } = DateOnly.MinValue;
        [DisplayName("Fecha Hasta")]
        public DateOnly DateTo { get; set; } = DateOnly.MaxValue;
        [DisplayName("Estados")]
        public StatusSelectViewModel[]? Statuses{ get; set; }
        public MemberFeeBatch[]? Batches { get; set; }

    }
}
