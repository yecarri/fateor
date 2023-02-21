using Fateor.Application.Organization.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.MySql
{
    public class MemberUnitOfWork : IDisposable, IMemberUnitOfWork
    {
        private bool disposedValue;
        private readonly UnitOfWork _uow;

        public MemberRepository MemberRepository { get; private set; }
        public MemberStatusRepository MemberStatusRepository { get; private set; }        

        public IDbConnection Connection => _uow.Connection;
        public IDbTransaction Transaction => _uow.Transaction;

        public MemberUnitOfWork(string connectionString)
        {
            _uow = new UnitOfWork(connectionString);
            MemberRepository = new MemberRepository(_uow);
            MemberStatusRepository = new MemberStatusRepository(_uow);
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
    }
}
