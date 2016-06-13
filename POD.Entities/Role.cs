using DapperExtensions.Mapper;
using System;

namespace POD.Entities
{
    public class Role
    {
        public Guid ID { get; set; }
        public string RoleName { get; set; }        
        public bool IsAdmin { get; set; }     
    }

    //public class RoleMapper : ClassMapper<Role>
    //{
    //    public RoleMapper()
    //    {
    //        base.Map(u => u.UserID).Ignore();
    //        base.AutoMap();
    //    }
    //}
}
