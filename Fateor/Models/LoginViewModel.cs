namespace Fateor.Models
{
    public class LoginViewModel : ViewModelBase
    {
        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public bool LoginFailed { get; set; } = false;

        public string LoginMessage { get; set; } = "";

    }
}
