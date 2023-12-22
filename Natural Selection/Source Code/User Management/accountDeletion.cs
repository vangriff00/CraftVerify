using System;
using System.Data.SqlClient; //gets stuff for sql connection, SqlTransaction, SqlCommand classes


namespace UserManagementLibrary2;


public class DataAccessObject{
    private string databaseConnectionString;

    //instantiate here for dao
    //passes in connection here?
    public DataAccessObject(string databaseConnectionString)
    {
        this.databaseConnectionString = databaseConnectionString;

    }

    public void AccountDeletion(string userHash){
        
        //put database connectino here and wrap it with try except for catching connection error
        try{
            using (SqlConnection dbConnection = new SqlConnection(databaseConnectionString))
                {//open connection here
                    dbConnection.Open();

                    //use system sqltransaction class 
                    SqlTransaction transact = dbConnection.BeginTransaction();

                    try
                    {

                        using (SqlCommand comm = connection.CreateCommand())
                        
                        {

                            comm.Transaction = transact;

                            comm.CommandText = "DELETE userProfile, userAccount FROM userProfile INNER JOIN userAccount ON userProfile.userHash = userAccount.userHash WHERE userProfile.userHash = userHash";

                            transact.Commit();


                        }

                    } catch (Exception ex){
                        Console.WriteLine("Error: Need rollback of transaction")
                        //figure out how to go back on the transaction here by looking through transaction sqlclient class 
                        transact.RollBack();
                    }

                }

        } catch (Exception ex){//database connection error 
            Console.WriteLine("Error: Connection to database failure");
            
        }

    }


}