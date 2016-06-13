namespace POD.Data.Dapper
{
    public class Enums
    {
        /// <summary>
        /// Supported DB engines
        /// </summary>
        public enum DbEngine
        {
            PostgreSQL,
            SQLServer
        }
        public enum OrganizationType
        {
            System = 1,
            Company = 2,
            Customer = 3
            
        }
    }
}
