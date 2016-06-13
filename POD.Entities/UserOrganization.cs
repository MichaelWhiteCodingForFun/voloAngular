using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Entities
{
    public class UserOrganization : Organization
    {
        public new Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid OrganizationID { get { return base.ID; } set { base.ID = value; } }
        public new bool IsActive { get; set; }
    }

    public class UserOrganizationMapper : ClassMapper<UserOrganization>
    {
        public UserOrganizationMapper()
        {
            base.Map(u => u.OrganizationName).Ignore();
            base.Map(u => u.AccountNumber).Ignore();
            base.Map(u => u.AccountName).Ignore();
            base.Map(u => u.OrganizationTypeID).Ignore();
            base.Map(u => u.CreatedByID).Ignore();
            base.Map(u => u.CreatedDateUtc).Ignore();
            base.Map(u => u.AdminUser).Ignore();
            base.Map(u => u.ParentOrganizationID).Ignore();
            base.AutoMap();
        }
    }
}
