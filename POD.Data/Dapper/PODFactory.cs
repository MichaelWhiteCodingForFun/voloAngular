using POD.Entities.Enums;
using POD.Utilities;
using System;

namespace POD.Data.Dapper
{
    public class PODFactory
    {
        public static PODContext GetContext(bool withTransaction, Type mapper = null)
        {
            #region Verify state
            if (_connectionString == null)
            {
                Initialize();
            }
            #endregion

            return new PODContext(_connectionString, _dbEngine, withTransaction, mapper);
        }

        #region Initialize
        /// <summary>
        /// Default initialization.
        /// Reads configuration from environment (cloud) or configuration file.
        /// </summary>
        public static void Initialize()
        {
                // If another thread already initialized connection string while we where locked => return.
                if (_connectionString != null) return;

                /* 
                 * 
                 * The ReadDatabaseConfiguration method reads the configuration setting value from the appropriate configuration store. 
                 * If the application is running as a .NET Web application, the GetSetting method will return the setting 
                 * value from the Web.config or app.config file. If the application is running in Windows Azure Cloud Service 
                 * or in a Windows Azure Website, the GetSetting will return the setting value from the ServiceConfiguration.cscfg.
                 * See:  PODEnvironment class for more information.
                 * 
                 * */
                ReadDatabaseConfiguration();
          
        }
        #endregion Initialize

        #region Private helper     

        private static void ReadDatabaseConfiguration()
        {           
            _connectionString = PODEnvironment.GetConnnectionString("PODDatabase");
            if (_connectionString == null || _connectionString == "")
            {
                throw new ApplicationException("Connection string not found in configurations.");
            }
            string db = PODEnvironment.DbEngine.ToString();
            if (db == null || db == "")
            {
                throw new ApplicationException("DbEngine not found in configurations.");
            }
            if (!Enum.TryParse<DBEngine>(db, out _dbEngine))
            {
                throw new ApplicationException("Invalid DbEngine value in configurations: " + db);
            }
        }
      
        #endregion

        #region Private variables   
           
        static string _connectionString;
        static DBEngine _dbEngine;

        #endregion
    }
}
