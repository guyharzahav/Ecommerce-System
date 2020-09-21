using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.UserDb
{
    public class DbPassword
    {
        [Key, ForeignKey("User")]
        [Column(Order = 1)]
        public string UserName { set; get; }
        public virtual DbUser User { set; get; }

        public string PwdHash { set; get; }

        public DbPassword(string username, string pwdhash)
        {
            UserName = username;
            PwdHash = pwdhash;
        }

        public DbPassword()
        {

        }

    }
}
