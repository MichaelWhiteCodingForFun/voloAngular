using POD.Entities;
using POD.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class UserViewModel
    {
        public Guid ID { get; set; }
        
        [Required(ErrorMessageResourceName = "EmailFieldIsRequired", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "FullNameIsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FullName { get; set; }
        public short StatusID { get; set; }
        public UserRole Role { get; set; }
        public UserOrganization Organization { get; set; }
        public DateTime? LastLoginDate { get; set; }       

        public string StatusDisplayName { get; set; }

        public bool IsEditable { get; set; }

        public static implicit operator UserViewModel(User userEntity)
        {
            return new UserViewModel
            {
                ID = userEntity.ID,
                FullName = userEntity.FullName,
                UserName = userEntity.UserName,
                Role = userEntity.Role,
                Organization = userEntity.Organization,
                LastLoginDate = userEntity.LastLoginDate,
                StatusID = userEntity.StatusID,
                StatusDisplayName = userEntity.StatusDisplayName               
            };
        }

        public static implicit operator User(UserViewModel userViewModel)
        {
            return new User
            {
                ID = userViewModel.ID,
                FullName = userViewModel.FullName,
                UserName = userViewModel.UserName,
                Role = userViewModel.Role,
                Organization = userViewModel.Organization,
                LastLoginDate = userViewModel.LastLoginDate,
                StatusID = userViewModel.StatusID
            };
        }      
    }

    public class UsersViewModel : BaseViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public Guid? OrganizationID { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string OrganizationName { get; set; }
        public string StatusName { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}