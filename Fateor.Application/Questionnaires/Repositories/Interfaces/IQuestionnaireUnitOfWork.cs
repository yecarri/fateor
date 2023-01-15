namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IQuestionnaireUnitOfWork
    {
        IOptionRepository OptionRepository { get; }
        IQuestionnaireRepository QuestionnaireRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IQuestionTypeRepository QuestionTypeRepository { get; }
        void Commit();
        void Rollback();
    }
}