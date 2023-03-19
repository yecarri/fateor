using Fateor.Application.Gdpr.Repositories.Interfaces;
using Fateor.Application.Questionnaires.Repositories.MySql;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Gdpr.Repositories.MySql
{
    public class GdprUnitOfWork : IGdprUnitOfWork, IDisposable
    {
        private bool disposedValue;
        private IUnitOfWork _uow;
        public IGdprRepository GdprRepository { get; private set; }
        public IGdprMemberRepository GdprMemberRepository { get; private set; }

        public IDbConnection Connection => _uow.Connection;

        public IDbTransaction Transaction => _uow.Transaction;

        public GdprUnitOfWork(string connectionString)
        {
            _uow= new oSoft.Data.MySql.UnitOfWork(connectionString);
            if (_uow is not null)
            {
                GdprRepository= new GdprRepository((oSoft.Data.UnitOfWork)_uow);
                GdprMemberRepository= new GdprMemberRepository((oSoft.Data.UnitOfWork)_uow);
            }
            else
                throw new NullReferenceException($"Cannot create UnitOfWork for {connectionString}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GdprRepository.Dispose();
                    GdprMemberRepository.Dispose();
                }
                disposedValue=true;
            }
        }

        void IDisposable.Dispose()
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
