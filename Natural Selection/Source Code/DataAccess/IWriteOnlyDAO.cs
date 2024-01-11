namespace DataAccessLibraryCraftVerify
{
    public interface IWriteOnlyDAO
    {

        int InsertAttribute(string connString, string sqlcommand);
    }
}
