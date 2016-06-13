using POD.Data.Dapper;
using POD.Entities;
using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions.Mapper;
using System.Data;

namespace POD.Data.Services
{
    public class UserDataService : BaseDataService, IUserDataService
    {
        static UserDataService _instance;
        //public UserDataService(PODContext context) : base(context)
        //{
        //    _instance = this;
        //}

        public UserDataService()
        {
            _instance = this;
        }

        public static UserDataService Instance
        {
            get { return _instance == null ? new UserDataService() : _instance; }
        }


        public User Create(User user)
        {
            try
            {
                user.ID = base.Insert(user);

                UserRole userRole = new UserRole
                {
                    UserID = user.ID,
                    RoleID = user.Role.RoleID,
                    IsPrimary = user.Role.IsPrimary,
                    IsActive = true

                };
                base.Insert(userRole);

                UserOrganization userOrganization = new UserOrganization()
                {
                    UserID = user.ID,
                    OrganizationID = user.Organization.OrganizationID,
                    IsActive = true
                };

                base.Insert(userOrganization);

                return user;
            }

            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
        public int Delete(Guid userId)  //
        {           
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserID", userId, dbType: DbType.Guid, direction: ParameterDirection.Input);

                using (var context = PODFactory.GetContext(true))
                {
                    int result = context.Database.Connection.Execute("[DeleteUser]", param, commandType: CommandType.StoredProcedure);
                    return result;
                }        
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }

        public void Update(User user) // void
        {
            try
            {
                using (var context = PODFactory.GetContext(true))
                {
                    var param = new DynamicParameters();
                    param.Add("@UserID", user.ID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                    param.Add("@FullName", user.FullName, dbType: DbType.String, direction: ParameterDirection.Input);
                    param.Add("@RoleID", user.Role.RoleID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                    param.Add("@IsPrimary", user.Role.IsPrimary, dbType: DbType.Boolean, direction: ParameterDirection.Input);
                    param.Add("@StatusID", user.StatusID, dbType: DbType.Int32, direction: ParameterDirection.Input);

                    context.Database.Connection.Execute("[dbo].[UpdateUser]", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
        public User GetUserByID(Guid ID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserID", ID, dbType: DbType.Guid, direction: ParameterDirection.Input);

                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetUserByID]", param, commandType: CommandType.StoredProcedure);

                    User user = multi.Read<User, UserRole, UserOrganization, User>((u, r, o) =>
                    { u.Role = r; u.Organization = o; return u; }, splitOn: "RoleID, OrganizationID").FirstOrDefault();

                    return user;
                }

            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Username", username, dbType: DbType.String, direction: ParameterDirection.Input);

                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetUserByUsername]", param, commandType: CommandType.StoredProcedure);

                    User user = multi.Read<User, UserRole, UserOrganization, User>((u, r, o) =>
                    { u.Role = r; u.Organization = o; return u; }, splitOn: "RoleID, OrganizationID").FirstOrDefault();
                  
                    return user;
                }               
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }


        public IEnumerable<User> GetUsers(Guid organizationID, string organizationName, string fullName, string userName, string roleName, string statusName, DateTime? lastLoginDate, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows)
        {
            try
            {
                List<User> users = new List<User>();

                var offset = (currentPageNumber - 1) * pageSize;
                var param = new DynamicParameters();
                param.Add("@OrganizationID", organizationID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                param.Add("@OrganizationName", organizationName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@FullName", fullName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserName", userName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RoleName", roleName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@StatusName", statusName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@LastLoginDate", lastLoginDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                param.Add("@Offset", offset, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@PageSize", pageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@SortExpression", sortExpression, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@SortDirection", sortDirection, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetUsers]", param, commandType: CommandType.StoredProcedure);

                    users = multi.Read<User, UserRole, UserOrganization, User>((u, r, o) =>
                    { u.Role = r; u.Organization = o; return u; }, splitOn: "IsPrimary, OrganizationName").ToList();

                    totalRows = param.Get<int>("RowCount");
                }

                if (users == null || users.Count == 0)
                {
                    // Not found
                    return users;
                }

                return users;

            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }

        }

        public IEnumerable<Role> Roles()
        {
            try
            {
                List<Role> roles = base.GetList<Role>(null).ToList(); 
                return roles;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
        public Role GetRole(Guid RoleID)
        {
            try
            {
                Role role = base.GetByID<Role>(RoleID);
                return role;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
      
    }
}
