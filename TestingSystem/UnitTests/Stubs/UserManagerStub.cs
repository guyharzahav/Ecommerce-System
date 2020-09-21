using eCommerce_14a.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests.Stubs
{
    public class UserManagerStub : UserManager
    {
        private static string adminName = "Admin";
        private static User admin = new User(1, adminName);

        public override User GetAtiveUser(string username)
        {
            if (username.Equals(adminName))
            {
                return admin;
            }

            return null;
        }
    }
}
