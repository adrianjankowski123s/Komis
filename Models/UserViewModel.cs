using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Komis.Models
{
    public class GroupedUserViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> Admins { get; set; }
    }

    public class UserViewModel
    {
        public AspNetUsers currentUser { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}