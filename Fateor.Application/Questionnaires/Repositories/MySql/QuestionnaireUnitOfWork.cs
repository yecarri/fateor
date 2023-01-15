using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using oSoft.Data;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Repositories.MySql
{
    public class QuestionnaireUnitOfWork : IDisposable, IQuestionnaireUnitOfWork
    {
        public IQuestionnaireRepository QuestionnaireRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IQuestionTypeRepository QuestionTypeRepository { get; private set; }
        public IOptionRepository OptionRepository { get; private set; }

        private IUnitOfWork _uow;

        public QuestionnaireUnitOfWork(string connectionString)
        {
            _uow= new oSoft.Data.MySql.UnitOfWork(connectionString);
            if (_uow is not null)
            {
                OptionRepository = new OptionRepository((oSoft.Data.UnitOfWork)_uow);
                QuestionnaireRepository = new QuestionnaireRepository((oSoft.Data.UnitOfWork)_uow);
                QuestionRepository = new QuestionRepository((oSoft.Data.UnitOfWork)_uow);
                QuestionTypeRepository = new QuestionTypeRepository((oSoft.Data.UnitOfWork)_uow);
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
            QuestionnaireRepository.Dispose();
            QuestionRepository.Dispose();
            QuestionTypeRepository.Dispose();
            OptionRepository.Dispose();
            _uow.Dispose();
        }
    }
}
