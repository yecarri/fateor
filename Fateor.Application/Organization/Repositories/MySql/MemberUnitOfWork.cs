using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberUnitOfWork : IDisposable, IMemberUnitOfWork
    {
        private bool disposedValue;

        public MemberRepository MemberRepository { get; private set; }

        public MemberStatusRepository MemberStatusRepository { get; private set; }

        private readonly UnitOfWork _uow;

        public MemberUnitOfWork(string connectionString)
        {
            _uow = new UnitOfWork(connectionString);
            MemberRepository = new MemberRepository(_uow);
            MemberStatusRepository = new MemberStatusRepository(_uow);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    MemberStatusRepository.Dispose();
                    MemberRepository.Dispose();
                    _uow.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            _uow.Commit();
        }

        public void Rollback()
        {
            _uow.Rollback();
        }

        public void BeginTransaction()
        {
            _uow.BeginTransaction();
        }
    }
}
