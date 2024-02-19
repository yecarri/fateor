using DocumentFormat.OpenXml.EMMA;
using System.Security.Policy;

namespace Fateor
{
    public class Settings
    {

        protected Dictionary<string, string> _Settings = new Dictionary<string, string>();

        public Settings(Dictionary<string, string> settings)
        {
            _Settings=settings;
        }

        public string Url { get => _Settings["Url"]; }
        public string Unit_Selector_Type { get => _Settings["Unit_Selector_Type"]; }
        public string Unit_Selector_Assembly { get => _Settings["Unit_Selector_Assembly"]; }
        public string Site { get => _Settings["Site"]; }
        public string Site_Name { get => _Settings["Site_Name"]; }
        public bool ForumSystem_CreateUser { get => int.Parse(_Settings["ForumSystem_CreateUser"]) != 0; }
        public string Email_Service_Provider { get => _Settings["Email_Service_Provider"]; }
        public string Email_Service_Server { get => _Settings["Email_Service_Server"]; }
        public int Email_Service_Port { get => int.Parse(_Settings["Email_Service_Port"]); }
        public string Email_Service_User { get => _Settings["Email_Service_User"]; }
        public string Email_Service_Password { get => _Settings["Email_Service_Password"]; }
        public string Email_Service_Key { get => _Settings["Email_Service_Key"]; }
        public string Email_Service_Domain { get => _Settings["Email_Service_Domain"]; }
        public string Email_Sender_Name { get => _Settings["Email_Sender_Name"]; }
        public string Email_Sender_Address { get => _Settings["Email_Sender_Address"]; }
        public bool Email_DisabledAll { get => int.Parse(_Settings["Email_DisabledAll"]) != 0; }
        public int Default_UserId { get => int.Parse(_Settings["Default_UserId"]); }
        public int Default_UnitId { get => int.Parse(_Settings["Default_UnitId"]); }
        public int Default_StatusId { get => int.Parse(_Settings["Default_StatusId"]); }
        public string Default_InitialPoll { get => _Settings["Default_InitialPoll"]; }
        public string Credentials_Generator_Type { get => _Settings["Credentials_Generator_Type"]; }
        public string Credentials_Generator_Assembly { get => _Settings["Credentials_Generator_Assembly"]; }
        public string Credentials_Enabled { get => _Settings["Credentials_Enabled"]; }
        public string ID_Validator_Type { get => _Settings["ID_Validator_Type"]; }
    }
}
