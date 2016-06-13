using DapperExtensions.Mapper;
using System;

namespace POD.Entities
{
    public class Organization
    {
        public Guid ID { get; set; }
        public string OrganizationName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int OrganizationTypeID { get; set; }
        public bool IsActive { get; set; }       
        public Guid CreatedByID { get; set; }
        public DateTime CreatedDateUtc { get; set; } 
        public User AdminUser { get; set; }
        public Guid ParentOrganizationID { get; set; } 

    }

    public class OrganizationMapper : ClassMapper<Organization>
    {
        public OrganizationMapper()
        {
            base.Map(u => u.AdminUser).Ignore();
            base.AutoMap();
        }
    }
}
