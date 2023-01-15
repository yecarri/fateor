namespace Fateor.Setup
{
    public static class SetupControllers
    {
        public static void AddSettings(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddSingleton<Settings, Settings>(s =>
                new Settings()
                {
                    SystemUrl = builder.Configuration.GetValue<string>("System:SustemUrl"),
                    Site = builder.Configuration.GetValue<string>("System:Site"),
                    InitialQuestionnaire = builder.Configuration.GetValue<string>("Fateor:InitialPoll"),
                    SystemUserId = builder.Configuration.GetValue<int>("Fateor:SystemUserId"),
                    InitialStatusId = builder.Configuration.GetValue<int>("Fateor:InitialStatusId"),
                    CreateExternalUsers = builder.Configuration.GetValue<bool>("Fateor:ExternalUsersSystem:CreateUser")
                });
        }
    }
}
