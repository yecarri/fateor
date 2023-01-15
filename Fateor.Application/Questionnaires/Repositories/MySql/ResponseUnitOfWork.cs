using Fateor.Application.Questionnaires.Repositories.Interfaces;
using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Repositories.MySql
{
    public class ResponseUnitOfWork : IDisposable, IResponseUnitOfWork
    {
        private readonly IUnitOfWork _uow;
        public IResponseRepository ResponseRepository { get; set; }
        public IAnswerRepository AnswerRepository { get; set; }
        public IAnswerOptionRepository AnswerOptionRepository { get; set; }
        public IQuestionnaireRepository QuestionnaireRepository { get; set; }
        public ResponseUnitOfWork(string connectionString)
        {
            _uow= new oSoft.Data.MySql.UnitOfWork(connectionString);
            if (_uow is not null)
            {
                ResponseRepository = new ResponseRepository((oSoft.Data.UnitOfWork)_uow);
                AnswerOptionRepository = new AnswerOptionRepository((oSoft.Data.UnitOfWork)_uow);
                AnswerRepository = new AnswerRepository((oSoft.Data.UnitOfWork)_uow);
                QuestionnaireRepository=new QuestionnaireRepository((oSoft.Data.UnitOfWork)_uow);
            }
            else
                throw new NullReferenceException($"Cannot create UnitOfWork for {connectionString}");
            
        }
        public void Commit()
        {
            _uow.Commit();
        }
        public void Rollback()
        {
            _uow.Rollback();
        }
        public void Dispose()
        {
            ResponseRepository.Dispose();
            AnswerRepository.Dispose();
            AnswerOptionRepository.Dispose();
            _uow.Dispose();
        }
    }
}
