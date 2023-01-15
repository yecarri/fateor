namespace Fateor
{
    public class Settings
    {
        public string SystemUrl { get; set; } = "";

        public int SystemUserId { get; set; } = 0;

        public string InitialQuestionnaire { get; set; } = "";

        public string Site { get; set; } = "";

        public int InitialStatusId { get; set; } = 0;

        public bool CreateExternalUsers { get; set; } = false;
    }
}
