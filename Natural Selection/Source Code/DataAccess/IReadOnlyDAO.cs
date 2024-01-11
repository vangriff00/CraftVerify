using System;
//dotnet add package MySql.Data
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataAccessLibraryCraftVerify
{
    public class IReadOnlyDAO
    {
        public string GetAttribute(string connString, string sqlcommand)
        {
            if (connString == null)
            {
                throw new ArgumentNullException();
            }
            if (sqlcommand == null)
            {
                throw new ArgumentNullException();
            }

            int rowsaffected = 0;
            if (connString.Contains("SqlServer"))
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        using (SqlCommand command = new SqlCommand(sqlcommand, connection, transaction))
                        {
                            try
                            {
                                var read = command.ExecuteReader();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                }
            }
            else if (connString.Contains("MySql"))
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        using (MySqlCommand command = new MySqlCommand(sqlcommand, connection, transaction))
                        {
                            try
                            {
                                rowsaffected += command.ExecuteNonQuery();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                }
            }
            else
            {
                Console.WriteLine("Unsupported database type.");
            }



            return rowsaffected;
        }
    }
}
