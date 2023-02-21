using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberFeeUnitOfWork: IUnitOfWork
    {
        IMemberFeeBatchRepository MemberFeeBatchRepository { get; }
        IMemberFeeRepository MemberFeeRepository { get; }
        IMemberRepository MemberRepository { get; }
    }
}