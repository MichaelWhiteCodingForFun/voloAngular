using POD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.Interfaces;

namespace POD.Interfaces
{
    public interface IUserDataService
    {
        User Create(User user);
        int Delete(Guid userId);
        void Update(User user);
        User GetUserByID(Guid ID);
        User GetUserByUsername(string userName);
        // List<User> GetUsers(int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows);

        IEnumerable<User> GetUsers(Guid organizationID, string organizationName, string fullName, string userName, string roleName, string statusName, DateTime? lastLoginDate, int currentPageNumber, int pageSize, string sortExpression, string sortDirection, out int totalRows);
        IEnumerable<Role> Roles();
    }
}
