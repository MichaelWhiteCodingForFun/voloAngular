using POD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class CustomerViewModel : BaseViewModel
    {
        public Guid ID { get; set; }
        public string OrganizationName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int TypeID { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedByID { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public User AdminUser { get; set; }
        public Guid ParentOrganizationID { get; set; }

        public static implicit operator Organization(CustomerViewModel customer)
        {
            return new Organization
            {
                ID = customer.ID,
                OrganizationName = customer.OrganizationName,
                AccountNumber = customer.AccountNumber,
                AccountName = customer.AccountName,
                OrganizationTypeID = customer.TypeID,
                IsActive = customer.IsActive,
                CreatedByID = customer.CreatedByID,
                CreatedDateUtc = customer.CreatedDateUtc,
                AdminUser = customer.AdminUser,
                ParentOrganizationID = customer.ParentOrganizationID
            };
        }

        public static implicit operator CustomerViewModel(Organization customer)
        {
            return new CustomerViewModel
            {
                ID = customer.ID,
                OrganizationName = customer.OrganizationName,
                AccountNumber = customer.AccountNumber,
                AccountName = customer.AccountName,
                TypeID = customer.OrganizationTypeID,
                IsActive = customer.IsActive,
                CreatedByID = customer.CreatedByID,
                CreatedDateUtc = customer.CreatedDateUtc,
                AdminUser = customer.AdminUser,
                ParentOrganizationID = customer.ParentOrganizationID
            };
        }

    }

    public class CustomersViewModel : BaseViewModel
    {
        public List<CustomerViewModel> Customers { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string UserName { get; set; }
        public string AdminUser { get; set; }
        public string StatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

}