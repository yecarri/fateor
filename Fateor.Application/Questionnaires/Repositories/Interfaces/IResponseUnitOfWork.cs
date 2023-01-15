namespace Fateor.Application.Questionnaires.Repositories.Interfaces
{
    public interface IResponseUnitOfWork
    {
        IAnswerOptionRepository AnswerOptionRepository { get; set; }
        IAnswerRepository AnswerRepository { get; set; }
        IResponseRepository ResponseRepository { get; set; }
        IQuestionnaireRepository QuestionnaireRepository { get; set; }

        void Commit();
        void Dispose();
        void Rollback();
    }
}