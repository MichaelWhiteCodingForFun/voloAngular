using System;
using POD.Entities;
using System.Collections.Generic;

namespace POD.Interfaces 
{
    public interface ICustomerDataService
    {
        bool Create(Organization customer);       
        Guid CreateOrganization(Ticket ticket);
        void Delete(Guid customerId);
        void Edit(Organization customer);      
     
        Organization GetCustomerByName(string name);
        List<Organization> GetCustomers(Guid currentOrganizationID, string accountNumber, string accountName, string adminUser, string userName, string statusName, DateTime? lastLoginDate, 
            int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows);
        Organization GetOrganization(string accountNumber);
        Organization GetOrganizationByID(Guid OrganizationID);
        Organization GetOrganizationWithAdminUser(Guid ID);
        List<Organization> GetOrganizationsByParentID(Guid parentOrganization);   

    }
}
