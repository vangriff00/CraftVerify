//dotnet add package MySql.Data

namespace DataAccessLibraryCraftVerify
{
    public interface IReadOnlyDAO
    {
        string GetAttribute(string connString, string sqlcommand);
    }
}
