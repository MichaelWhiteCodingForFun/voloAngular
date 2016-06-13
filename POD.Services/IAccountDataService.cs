using POD.Entities;

namespace POD.Interfaces
{
    public interface IAccountDataService
    {
       

        //void VerifyUserEmail(Guid userID, string email);

        void UpdateUserSecurity(User user);

    }
}
