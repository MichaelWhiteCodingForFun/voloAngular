using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class UserIdentityInfo
    {
        public Guid ID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public Guid RoleID { get; set; }
        public Guid OrganizationID { get; set; }        
        public int OrganizationTypeID { get; set; }
        public string OrganizationName { get; set; }
        public bool IsAdmin { get; set; }
    }
}