using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Repeat.Models
{
    internal class App
    {
        #region Property

        private readonly CancellationTokenSource _source;   // 多執行緒的取消

        #endregion

        #region Singleton Pattern

        // https://csharpindepth.com/Articles/Singleton
        private static readonly Lazy<App> lazy = new Lazy<App>(() => new App());

        public static App Instance { get { return lazy.Value; } }

        private App()
        {
            // Create a cancellation token and cancel it.
            _source = new CancellationTokenSource();
            var token = _source.Token;

            // https://stackoverflow.com/questions/177856/how-do-i-trap-ctrlc-sigint-in-a-c-sharp-console-app
            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e) {
                _source.Cancel();
                e.Cancel = true;
            };
        }

        #endregion

        internal void Run(string[] args)
        {
            var builder = new SqlConnectionStringBuilder();
            //builder["Data Source"] = "140.116.234.15";
            //builder["integrated Security"] = true;
            //builder["Initial Catalog"] = "CHP_CDB_AVMii_TEST";
            builder.DataSource = "140.116.234.15";
            builder.IntegratedSecurity = false;
            builder.InitialCatalog = "CHP_CDB_AVMii_TEST";
            builder.UserID = "CHP_CDB_AVMii_TEST";
            builder.Password = "CHP_CDB_AVMii_TEST";
            builder.PersistSecurityInfo = true;

            string connectionString = builder.ConnectionString;

            int count = 0;
            int limit = 100;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //while (count < limit)
                while (true)
                {
                    if (_source.IsCancellationRequested)
                    {
                        break;
                    }

                    UpdateRule(connection);
                    count++;

                    Thread.Sleep(25);
                }
            }
        }

        private static void UpdateRule(SqlConnection connection)
        {
            string queryString = "UPDATE [RULE] SET UPDATE_TIME = GETDATE() WHERE RULE_ID = 'rule1';";
            using (var command = new SqlCommand(queryString, connection))
            {
                int count = command.ExecuteNonQuery();

                //using (SqlDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        Console.WriteLine(String.Format("{0}, {1}",
                //            reader[0], reader[1]));
                //    }
                //}
            }
        }
    }
}
