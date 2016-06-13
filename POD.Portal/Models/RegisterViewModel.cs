using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{
    public class RegisterViewModel
    {
        public Guid RoleID { get; set; }

        [Required(ErrorMessageResourceName = "EmailFieldIsRequired", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "FullNameIsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FullName { get; set; }


    }
}