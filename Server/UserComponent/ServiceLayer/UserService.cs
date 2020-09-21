using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.ServiceLayer
{
    public class UserService
    {
        UserManager UM;
        public UserService()
        {
            UM = UserManager.Instance;
        }

        public Tuple<bool, string> MakeAdmin(string username) 
        {
            return UM.MakeAdmin(username);
        }
        public Dictionary<int, int[]> GetUserPermissions(string username) 
        {
            return UM.GetUserPermissions(username);
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
        public Tuple<bool,string> Registration(string username, string password)
        {
            //Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return  UM.Register(username, password);

        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
        public Tuple<bool,string> Login(string username, string password)
        {
            //Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Login(username, password);

        }
        public Tuple<bool,string> LoginAsGuest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Login("", "", true);

        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer-logout-31</req>
        public Tuple<bool,string> Logout(string user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.Logout(user);
        
        }

        public bool isAdmin(string username) 
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return UM.isAdmin(username);
        }

        public List<string> GetApprovalListByStoreAndUser(string username, int storeID) 
        {
           return UM.GetApprovalListByStoreAndUser(username, storeID);
        }

        public List<User> GetAllRegisteredUsers()
        {
            return UM.GetAllRegisteredUsers();
        }

        //For Admin Usage
        public void cleanup()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            UM.cleanup();
        }

        public List<Tuple<string, Permission>> GetStoreManagersPermissions(string appointer, int storeId)
        {
            return UM.GetStoreManagersPermissions(appointer, storeId);
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-admin-views-statistics-65</req>
        internal Statistic_View GetStatistics(string username, DateTime? startTime, DateTime? endTime)
        {
            return UM.GetStatistics(username, startTime, endTime);
        }
    }
}