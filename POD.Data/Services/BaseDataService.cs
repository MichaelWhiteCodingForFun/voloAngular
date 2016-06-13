using DapperExtensions;
using POD.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;
using System.Data;
using DapperExtensions.Mapper;
using POD.Entities;
using POD.Utilities;

namespace POD.Data.Services
{
    public class BaseDataService
    {
        #region Private Members

        protected PODContext _context;
        protected LoggingManager _loggingManager = new LoggingManager();
        protected IDatabase _db
        {
            get
            {
                return _context.Database;
            }
        }

        #endregion

        #region ctor

        public BaseDataService()
        {
                 
        }

        //public BaseDataService(PODContext ctx)
        //{
        //    this._context = ctx;
        //}

        #endregion

        #region Properties

        public PODContext Context
        {
            get
            {
                return this._context;
            }
        }

        #endregion

        protected Guid Insert<T>(T obj) where T : class
        {
            using (var context = PODFactory.GetContext(true))
            {
                return context.Database.Connection.Insert<T>(obj);
            }
        }
        protected bool Delete<T>(T obj) where T : class
        {
            using (var context = PODFactory.GetContext(true))
            {
                return context.Database.Connection.Delete<T>(obj);
            }
        }
        protected bool Update<T>(T obj) where T : class
        {
            using (var context = PODFactory.GetContext(true))
            {
                return context.Database.Connection.Update<T>(obj);
            }
        }
        protected T GetByID<T>(Guid id) where T : class
        {
            try
            {
                using (var context = PODFactory.GetContext(true))
                {
                    return context.Database.Connection.Get<T>(id);
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        protected IEnumerable<T> GetList<T>(object predicate) where T : class
        {
            using (var context = PODFactory.GetContext(true))
            {
                return context.Database.Connection.GetList<T>(predicate);
            }
        }


        protected int Execute<T>(string sql, T obj) where T : class
        {
            using (var context = PODFactory.GetContext(true))
            {
                return context.Database.Connection.Execute(sql, obj);
            }
        }
    
    }

    //public class UserModelMapper<T> : ClassMapper<T>
    //{
    //    public UserModelMapper()
    //    {
    //        if (typeof(T) == typeof(User))
    //        {

    //            //Ignore this property entirely
    //            Map(x => (x).Role).Ignore();
    //        }

    //        //optional, map all other columns
    //        AutoMap();
    //    }
    //}

  
}
