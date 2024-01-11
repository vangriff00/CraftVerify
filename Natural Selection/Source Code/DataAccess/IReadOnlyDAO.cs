namespace DataAccessLibraryCraftVerify
{
    public interface IReadOnlyDAO
    {
        public string GetAttribute(string connString, string sqlcommand);
    }
}
