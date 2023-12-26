using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNatural.CraftVerify.UserManager;

public interface IDBInserter
{
    Task<bool> InsertUserIntoTwoTablesAsync(string userAccountTable, string userProfileTable, AccountCreation account);
}

public class DBInserter : IDBInserter
{
    private readonly string _connectionString;

    public DBInserter(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> InsertUserIntoTwoTablesAsync(string UserAccount, string UserProfile, AccountCreation account)
    {
        var commandText1 = $"INSERT INTO {UserAccount} (userID) (email) (dateCreate) (userHash) VALUES (@userID) (@email) (@dateCreate) (@userHash)";
        var commandText2 = $"INSERT INTO {UserProfile} (userID) (DOB) (userRole) (userHash) VALUES (@userID) (@dob) (@userRole) (@userHash)";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Insert into the first table
                    var command1 = new SqlCommand(commandText1, connection, transaction);
                    // Add parameters to command1
                    await command1.ExecuteNonQueryAsync();

                    // Insert into the second table
                    var command2 = new SqlCommand(commandText2, connection, transaction);
                    // Add parameters to command2
                    await command2.ExecuteNonQueryAsync();

                    // Commit the transaction
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    // If an error occurs, roll back the transaction
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}