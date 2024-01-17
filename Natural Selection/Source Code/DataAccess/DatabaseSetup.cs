using Microsoft.Data.SqlClient;

namespace DataAccessLibraryCraftVerify
{
    public class DatabaseSetup
    {
        public void Main()
        {
            string connString = "Server=localhost;Database=CraftVerify;User Id = admin; Password = admin;Trusted_Connection=True;";
            string sqlcommand1 = "@CREATE TABLE[dbo].[ClaimPrinciple] ([claim] VARCHAR (64) NOT NULL,[identity]      VARCHAR(64) NULL,[hashPrinciple] CHAR(64)    NULL,PRIMARY KEY CLUSTERED([claim] ASC));";
            string sqlcommand2 = "CREATE TABLE[dbo].[LogTable]([logID] BIGINT        NOT NULL,[userHash] CHAR(64)     NULL,[actionType] VARCHAR(50)  NULL,[logTime]DATETIME NULL,[logStatus]  VARCHAR(50)  NULL,[logDetail] VARCHAR(200) NULL,PRIMARY KEY CLUSTERED([logID] ASC));";
            string sqlcommand3 = "CREATE TABLE[dbo].[UserAccount]([userID] CHAR (10)    NOT NULL,email]                            VARCHAR(64) NULL,[userHash] CHAR(64)    NOT NULL,[hashedOTP]                        CHAR(64)    NULL,[otpSalt] CHAR(10)    NULL,[userStatus]BIT NULL,[dateCreate]                       DATETIME NULL,[secureAnswer1]                    VARCHAR(50) NULL,[secureAnswer2] VARCHAR(50) NULL,[secureAnswer3] VARCHAR(50) NULL,[firstAuthenticationFailTimestamp]DATETIME NULL,[failedAuthenticationAttempts]     INT NULL,PRIMARY KEY CLUSTERED([userHash] ASC));";
            string sqlcommand4 = "CREATE TABLE[dbo].[UserProfile]([userID] CHAR (10)      NOT NULL,[userHash]        CHAR(64)      NOT NULL,[profileUserRole] VARCHAR(10)   NULL,[profileUsername] VARCHAR(64)   NULL,[profileDOB]DATETIME NULL,[profileBio]      NVARCHAR(500) NULL,[profilePhoto]IMAGE NULL,PRIMARY KEY CLUSTERED([userHash] ASC));";

            InsertAttribute(connString, sqlcommand1);
            InsertAttribute(connString, sqlcommand2);
            InsertAttribute(connString, sqlcommand3);
            InsertAttribute(connString, sqlcommand4);
        }
        public int InsertAttribute(string connString, string sqlcommand)
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
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    using (SqlCommand command = new SqlCommand(sqlcommand, connection, transaction))
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
            return rowsaffected;
        }
    }
}