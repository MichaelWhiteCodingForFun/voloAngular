using DapperExtensions.Mapper;
using POD.Entities.Enums;
using System;


namespace POD.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public Guid CreatedByID { get; set; }
        public bool IsActive { get; set; }
        public short StatusID { get; set; }
        public string SecurityStamp { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public string StatusDisplayName { get { return ((AccountStatus)StatusID).ToString(); } }

        public UserRole Role { get; set; }
        public UserOrganization Organization { get; set; }

    }

    public class UserMapper : ClassMapper<User>
    {
        public UserMapper()
        {
            base.Map(u => u.Role).Ignore();
            base.Map(u => u.Organization).Ignore();
            base.Map(u => u.StatusDisplayName).Ignore();
            base.AutoMap();
        }
    }


}
