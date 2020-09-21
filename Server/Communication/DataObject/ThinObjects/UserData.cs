using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class UserData
    {
        public string Username { get; set; }
        public string Password { get; set; }

    public UserData() { }

    public UserData(string Username) {
            this.Username = Username;
    }
    public UserData(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public void clear()
        {
            this.Username = "";
            this.Password = "";
        }
    }

}