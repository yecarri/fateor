using Dapper;
using MySql.Data.MySqlClient;

namespace Fateor.Setup
{
    public static class SetupControllers
    {
        public static void AddSettings(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            string connectionString = builder.Configuration.GetConnectionString("fateor");

            MySqlConnection cn = new MySqlConnection(connectionString);

            cn.Open();

            var data = cn.Query("SELECT * FROM app_config").Select(r => new KeyValuePair<string, string>(r.ConfigCode, r.ConfigValue));

            var settings = new Dictionary<string, string>(data);

            services.AddSingleton<Settings, Settings>(s => new Settings(settings));
        }
    }
}
