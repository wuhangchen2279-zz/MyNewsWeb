using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNewsWeb.Models
{
    public class ManageUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] UserRoles { get; set; }
    }


}