using POD.Entities.Enums;
using System;
using System.Configuration;

namespace POD.Data.Dapper
{

    public static class PODEnvironment
    {

        /// <summary>
        /// Database connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return GetConnnectionString("PODDatabase");
            }
        }

        /// <summary>
        /// DbEngine as nullable DbEngine enum.
        /// null if missing from configuration.
        /// </summary>
        public static DBEngine? DbEngine
        {
            get
            {
                string dbEngine = GetSetting("DbEngine");
                if (string.IsNullOrEmpty(dbEngine))
                {
                    return null;
                }
                else
                {
                    return (DBEngine)Enum.Parse(typeof(DBEngine), dbEngine);
                }

            }
        } 

        /// <summary>
        /// TO be modified to support reading configuration from cloud
        /// </summary>
        /// <param name="name">Configuration setting name.</param>
        /// <returns>Configuration setting value.</returns>
        public static string GetConnnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string GetSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

    }
}
