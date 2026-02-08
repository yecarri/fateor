using Fateor.Application.Organization.Repositories.MySql;
using oSoft.Data;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberUnitOfWork: IUnitOfWork
    {
        IMemberRepository MemberRepository { get; }
        IMemberStatusRepository MemberStatusRepository { get; }        
    }
}