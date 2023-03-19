using oSoft.Data;

namespace Fateor.Application.Gdpr.Repositories.Interfaces
{
    public interface IGdprUnitOfWork : IUnitOfWork, IDisposable
    {
        public IGdprRepository GdprRepository { get;  }
        public IGdprMemberRepository GdprMemberRepository { get; }
    }
}
