using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Entities
{
    public class UserRole : Role
    {
        public new Guid ID { get; set; }
        public Guid UserID { get; set; }

        public Guid RoleID { get { return base.ID; } set { base.ID = value; } }     

        public bool IsPrimary { get; set; }

        public bool IsActive { get; set; }        
    }

    public class UserRoleMapper : ClassMapper<UserRole>
    {
        public UserRoleMapper()
        {
            base.Map(u => u.RoleName).Ignore();
            base.Map(u => u.IsAdmin).Ignore();          
            base.AutoMap();
        }
    }
}
