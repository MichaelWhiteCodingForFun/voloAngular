using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POD.Portal.Models
{

    //public class AppUserState
    //{
    //    public string UserName = string.Empty;
    //    public string Email = string.Empty;
    //    public int OrganizationID = 0;
    //    public Guid UserID = 0;
    //    public Guid RoleID = 0;


    //    /// <summary>
    //    /// Exports a short string list of Id, Email, Name separated by |
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return string.Join("|", new string[] { this.UserID.ToString(), this.UserName, this.RoleID.ToString(), this.OrganizationID.ToString() });
    //    }


    //    /// <summary>
    //    /// Imports Id, Email and Name from a | separated string
    //    /// </summary>
    //    /// <param name="itemString"></param>
    //    public bool FromString(string itemString)
    //    {
    //        if (string.IsNullOrEmpty(itemString))
    //            return false;

    //        string[] strings = itemString.Split('|');
    //        if (strings.Length < 3)
    //            return false;

    //        UserID = Int32.Parse(strings[0]);
    //        UserName = strings[1];
    //        RoleID = Int32.Parse(strings[2]);
    //        OrganizationID = Int32.Parse(strings[3]);

    //        return true;
    //    }

    //    /// <summary>
    //    /// Populates the AppUserState properties from a
    //    /// User instance
    //    /// </summary>
    //    /// <param name="user"></param>
    //    public void FromUser(UserViewModel user)
    //    {
    //        UserID = user.UserID;
    //        UserName = user.UserName;
    //        //Email = user.Email;
    //        //IsAdmin = user.IsAdmin;
    //        //Theme = user.Theme;
    //    }

    //    /// <summary>
    //    /// Determines if the user is logged in
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsEmpty()
    //    {
    //        if (string.IsNullOrEmpty(this.UserID.ToString()) || string.IsNullOrEmpty(this.UserName))
    //            return true;

    //        return false;
    //    }
    //}
}
