using MySql.Data.MySqlClient;
using System.Text;

namespace Fateor.Database.MySql
{
    public class DBUpdates: IDBUpdates
    {
        public readonly string _connectionString;
        public readonly string _scriptsDirectory;

        public DBUpdates(string connectionString, string scriptsDirectory)
        {
            _connectionString=connectionString;
            _scriptsDirectory=scriptsDirectory; 
            InitializeScriptLogTable();            
        }

        public string Update()
        {
            List<string> executedScrips = new List<string>();
            StringBuilder sb = new StringBuilder();

            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cm = cn.CreateCommand())
                {
                    cn.Open();
                    cm.CommandText="select ScriptName from db_Scripts";
                    using(var reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            executedScrips.Add((string)reader[0]);
                        }
                    }
                }

                DirectoryInfo di = new DirectoryInfo(_scriptsDirectory);
                
                sb.AppendLine($"Starting DB Update at {DateTime.Now}");
                using (var insert = new MySqlCommand("INSERT INTO db_Scripts values(@ScriptName, @DateExecuted)"))
                {
                    insert.CommandType=System.Data.CommandType.Text;
                    insert.Connection = cn;
                    insert.Parameters.Add("ScriptName", MySqlDbType.VarString);
                    insert.Parameters.Add("DateExecuted", MySqlDbType.DateTime);

                    foreach (FileInfo fi in di.GetFiles("*.sql").OrderBy(fi => fi.Name))
                    {

                        if (executedScrips.Contains(fi.Name))
                        {
                            sb.AppendLine($"{fi.Name} already executed - Skipping.");
                        }
                        else
                        {
                            sb.AppendLine($"{fi.Name} will execute...");
                            var textreader = new StreamReader(fi.FullName);
                            var script = textreader.ReadToEnd();
                            using (var cm = new MySqlCommand(script))
                            {
                                cm.Connection = cn;
                                try
                                {
                                    cm.ExecuteNonQuery();
                                    sb.AppendLine($"{fi.Name} executed");
                                    insert.Parameters["ScriptName"].Value = fi.Name;
                                    insert.Parameters["DateExecuted"].Value = DateTime.Now;
                                    insert.ExecuteNonQuery();
                                }
                                catch (MySqlException ex)
                                {
                                    sb.AppendLine($"{fi.Name} failed:");
                                    sb.AppendLine(new string('-', 80));
                                    sb.AppendLine($"-- Code: {ex.Code}");
                                    sb.AppendLine("-- Message:");
                                    sb.AppendLine(ex.Message);
                                    sb.AppendLine("-- Source:");
                                    sb.AppendLine(ex.Source);
                                    sb.AppendLine(new string('-', 80));
                                }
                            }
                        }
                        sb.AppendLine($"Finished DB Update at {DateTime.Now}");
                    }
                }
            }

            return sb.ToString();
        }

        private void InitializeScriptLogTable()
        {
            using (MySqlConnection cn = new MySqlConnection(_connectionString))
            using (MySqlCommand cm = cn.CreateCommand())
            {
                cn.Open();
                cm.CommandText="select count(*) from information_schema.tables where table_name='db_Scripts'";
                var exists = (long)cm.ExecuteScalar();
                if (exists==0)
                {
                    cm.CommandText="create table db_Scripts(ScriptName varchar(768) not null primary key, DateExecuted DateTime)";
                    cm.ExecuteNonQuery();
                }
            }
        }
    }
}