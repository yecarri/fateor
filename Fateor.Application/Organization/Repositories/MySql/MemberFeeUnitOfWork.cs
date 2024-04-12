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
    public class MemberFeeUnitOfWork : IDisposable, IMemberFeeUnitOfWork
    {
        private bool disposedValue;

        private readonly UnitOfWork _uow;

        public IMemberFeeBatchRepository MemberFeeBatchRepository { get; private set; }
        public IMemberFeeRepository MemberFeeRepository { get; private set; }
        public IMemberRepository MemberRepository { get; private set; }
        public IStatusRepository StatusRepository { get; private set; }

        public IDbConnection Connection => _uow.Connection;
        public IDbTransaction Transaction => _uow.Transaction;

        public MemberFeeUnitOfWork(string connectionString)
        {
            _uow=new UnitOfWork(connectionString);
            MemberFeeBatchRepository = new MemberFeeBatchRepository(_uow);
            MemberFeeRepository= new MemberFeeRepository(_uow);
            MemberRepository = new MemberRepository(_uow);
            StatusRepository = new StatusRepository(_uow);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    MemberFeeBatchRepository.Dispose();
                    MemberFeeRepository.Dispose();
                    MemberRepository.Dispose();
                    StatusRepository.Dispose();
                    _uow.Dispose();
                }

                disposedValue=true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _uow.BeginTransaction();
        }

        public void Commit()
        {
            _uow.Commit();
        }

        public void Rollback()
        {
            _uow.Rollback();
        }
    }
}
