namespace Fateor.Application.Organization.Repositories.MySql
{
    public interface IMemberUnitOfWork
    {
        MemberRepository MemberRepository { get; }
        MemberStatusRepository MemberStatusRepository { get; }
        void BeginTransaction();
        void Commit();
        void Dispose();
        void Rollback();
    }
}