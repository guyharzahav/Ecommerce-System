using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.UserDb
{
    public class DbUser
    {
        [Key]
        public string Name { set; get; }
        public bool IsGuest { set; get; }
        public bool IsAdmin { set; get; }
        public bool IsLoggedIn { set; get; }

        public DbUser(string uname, bool isguest, bool isadmin, bool isloggedin)
        {
            Name = uname;
            IsGuest = isguest;
            IsAdmin = isadmin;
            IsLoggedIn = isloggedin;
        }

        public DbUser ()
        {

        }
    }
}
