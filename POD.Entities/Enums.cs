using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Entities.Enums
{

    /// <summary>
    /// Supported DB engines
    /// </summary>
    public enum DBEngine
    {
        PostgreSQL,
        SQLServer
    }
    public enum OrganizationType
    {
        None = 0,
        System = 1,
        Company = 2,
        Customer = 3
    }

    public enum AccountStatus : short
    {
        None = 0,
        Inactive = 1,
        Active = 2,
        Disabled = 3,
        Locked = 4
    }

}
