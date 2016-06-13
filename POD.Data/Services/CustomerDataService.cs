using POD.Data.Dapper;
using POD.Entities;
using POD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using POD.Entities.Enums;

namespace POD.Data.Services
{
    public class CustomerDataService : BaseDataService, ICustomerDataService
    {
        static CustomerDataService _instance;
        public CustomerDataService()
        {
            _instance = this;
        }
        public static CustomerDataService Instance
        {
            get
            {
                return _instance == null ? new CustomerDataService() : _instance;
            }
        }
        public bool Create(Organization customer)
        {
            bool tof = false;
            try
            {
                Organization getCustomerByName = this.GetCustomerByName(customer.OrganizationName);
                if (getCustomerByName != null)
                {
                    customer.ID = getCustomerByName.ID;
                    if (getCustomerByName.IsActive == false)
                    {
                        bool b = base.Update<Organization>(customer);
                        tof = true;
                    }

                }
                else if (getCustomerByName == null)
                {
                    base.Insert<Organization>(customer);
                    tof = true;
                }
                return tof;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Delete(Guid customerId)
        {
            Organization customer = GetOrganizationWithAdminUser(customerId);
            if (customer != null)
            {
                base.Delete<Organization>(customer);
            }
        }
        public void Edit(Organization customer)
        {
            string adminUserName = GetByID<User>(customer.AdminUser.ID).UserName;

            //admmin hasnt been changed
            if (String.Compare(customer.AdminUser.UserName, adminUserName, true) == 0)
            {
                string sql = @"UPDATE ""Organization"" SET OrganizationName = @OrganizationName WHERE Id = @Id";
                base.Execute(sql, new { OrganizationName = customer.OrganizationName, ID = customer.ID });
            }
            //admin user has been changed
            else
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@CustomerID", customer.ID, DbType.Guid, direction: ParameterDirection.Input);
                    param.Add("@CustomerName", customer.OrganizationName, DbType.String, direction: ParameterDirection.Input);
                    param.Add("@AdminID", customer.AdminUser.ID, DbType.Guid, direction: ParameterDirection.Input);
                    param.Add("@RoleID", customer.AdminUser.Role.ID, DbType.Guid, direction: ParameterDirection.Input);
                    using (var context = PODFactory.GetContext(true))
                    {
                        var multi = context.Database.Connection.QueryMultiple("[EditOrganization]", param, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }
        public Organization GetOrganizationWithAdminUser(Guid ID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrganizationID", ID, dbType: DbType.Guid, direction: ParameterDirection.Input);

                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetOrganizationByID]", param, commandType: CommandType.StoredProcedure);

                    Organization org = multi.Read<Organization>().FirstOrDefault();
                    if (org != null)
                    {
                        User user = multi.Read<User>().FirstOrDefault();
                        org.AdminUser = user;
                    }
                    return org;
                }
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
        public List<Organization> GetCustomers(Guid currentOrganizationID, string accountNumber, string accountName, string adminUser, string userName, string statusName, DateTime? lastLoginDate,
                                                 int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows)
        {
            totalRows = 0;
            try
            {
                List<Organization> customers = new List<Organization>();

                var offset = (currentPageNumber - 1) * pageSize;
                var param = new DynamicParameters();
              
                param.Add("@ParentOrganizationID", currentOrganizationID, dbType: DbType.Guid, direction: ParameterDirection.Input);
                param.Add("@AccountNumber", accountNumber, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@AccountName", accountName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@AdminUser", adminUser, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserName", userName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@StatusName", statusName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@LastLoginDate", lastLoginDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                param.Add("@Offset", offset, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@PageSize", pageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@SortExpression", sortExpression, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@SortDirection", sortDirection, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetOrganizations]", param, commandType: CommandType.StoredProcedure);

                    customers = multi.Read<Organization, User, Organization>((a, g) =>
                    { a.AdminUser = g; return a; }, splitOn: "UserName").ToList();

                    totalRows = param.Get<int>("RowCount");
                }
                if (customers == null || customers.Count == 0)
                {
                    return customers;
                }



                //foreach (Organization organization in customers)
                //{
                //    var customerRole = customerRoles.FirstOrDefault(cr => cr.OrganizationID == organization.ID);
                //    if (customerRole != null)
                //    {
                //        organization.AdminUser = users.FirstOrDefault(u => u.ID == customerRole.UserID);
                //    };
                //}
                return customers;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Organization GetCustomerByName(string name)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrganizationName", name, DbType.String, direction: ParameterDirection.Input);
                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetOrganizationByName]", param, commandType: CommandType.StoredProcedure);

                    Organization org = multi.Read<Organization, User, Organization>((o, u) =>
                    { o.AdminUser = u; return o; }, splitOn: "FullName").FirstOrDefault();

                    return org;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Organization> GetOrganizationsByParentID(Guid parentOrganization)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ParentOrganization", parentOrganization, DbType.Guid, direction: ParameterDirection.Input);
                using (var context = PODFactory.GetContext(true))
                {
                    var multi = context.Database.Connection.QueryMultiple("[GetOrganizationsByParentID]", param, commandType: CommandType.StoredProcedure);
                    List<Organization> orgs = multi.Read<Organization>().ToList();
                    //if (orgs != null)
                    //{

                    //}
                    return orgs;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #region Dapper Extensions

        //public Organization GetCustomerByID(Guid id)
                //{
        //    Organization customer = null;
        //    try
        //    {
        //        // Basic DapperExtensions read
        //        //  customer = get.<Organization>(new { ID = id });
        //        //if (res.Count() == 0)
        //        //{
        //        //    // Not found - return null
        //        //    return null;
        //        //}
        //        //else if (res.Count() > 1)
        //        //{
        //        //    throw new InvalidOperationException("Get by username returned more than 1 user for username: " + username);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return null;

        //}

        public Organization GetOrganization(string accountNumber)
        {
            string sql = "SELECT * from [Organization] Where AccountNumber='" + accountNumber + "'";
            using (var context = PODFactory.GetContext(true))
            {
                var result = context.Database.Connection.Query<Organization>(sql).FirstOrDefault();             
                              
                if (result != null)
                {                    
                    return result;
                }
            }
            return null;
        }


        public Guid CreateOrganization(Ticket ticket)
        {
            Organization organization = new Organization()
            {
                OrganizationName=string.Empty,
                CreatedByID = new Guid("839375C9-BB28-4256-AB58-A5FE0109BE99"),
                CreatedDateUtc = DateTime.Now,
                IsActive = false,
                AccountName = "AccountName" + ticket.AccountName,
                AccountNumber = ticket.AccountNumber,
                OrganizationTypeID = (int)OrganizationType.Customer,
                ParentOrganizationID = Guid.Empty // new Guid("52954A6A-6ED5-42F6-A25F-5F637F458002")
            };
            return base.Insert<Organization>(organization);
        }
        public Organization GetOrganizationByID(Guid OrganizationID)
        {
            try
            {
                Organization organization = base.GetByID<Organization>(OrganizationID);
                return organization;
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }


        #endregion


    }
}
