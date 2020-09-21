using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;
using Server.DAL;
using Server.DAL.UserDb;
using Server.DAL.CommunicationDb;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.DomainLayer
{


    public class UserManager
    {
        public static int usingResource = 0 ;
        public Dictionary<string, string> Users_And_Hashes {get; set;}
        public Dictionary<string, User> users { get; set; }
        public Dictionary<string, User> Active_users { get; set; }
        private int Available_ID;
        private Security SB;
        private static readonly object padlock = new object();  
        private static UserManager instance = null;  
        public static UserManager Instance  
        {  
            get  
            {  
                if (instance == null)  
                {  
                    lock (padlock)  
                    {  
                        if (instance == null)  
                        {  
                            instance = new UserManager();  
                        }  
                    }  
                }  
                return instance;  
            }  
        } 

        /// <summary>
        /// Public ONLY For generatin Stubs
        /// </summary>
        public UserManager()
        {
            //Console.WriteLine("UserManager Created\n");
            Users_And_Hashes = new Dictionary<string, string>();
            users = new Dictionary<string, User>();
            Active_users = new Dictionary<string, User>();
            SB = new Security();
            Available_ID = 1;
        }


        internal Tuple<bool, string> MakeAdmin(string username)
        {
            User user;
            if (!users.TryGetValue(username, out user))
                return new Tuple<bool, string>(false, "user not found in register users!");
            user.IsAdmin = true;
            try
            {
                DbManager.Instance.UpdateUserAdminStatus(user.getUserName(), true, true);
            }
            catch (Exception ex)
            {
                Logger.logError("couldn't save admin status to DB: " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Could not save to DB");
            }
            return new Tuple<bool, string>(true, "Appoint to admin succeed!");
        }

        internal bool isAdmin(string username)
        {
            User user;
            if (!users.TryGetValue(username, out user))
                return false;
            return user.IsAdmin == true;
        }

        public List<User> GetAllRegisteredUsers() 
        {
            return users.Values.ToList();
        }
        public Dictionary<int, int[]> GetUserPermissions(string username) 
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User user;
            if (!users.TryGetValue(username, out user))
                return null;
            return user.GetUserPermissions();
        }

        public Statistic_View GetStatistics(string username, DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null && endTime == null)
                return Statistics.Instance.getViewDataAll(username);
            if (startTime != null && endTime == null)
                return Statistics.Instance.getViewDataStart(username, startTime);
            if (startTime == null && endTime != null)
                return Statistics.Instance.getViewDataEnd(username, endTime);
            else
                return Statistics.Instance.getViewData(username, startTime, endTime);


        }

        //Checks if user name and password are legit and not exsist
        private Tuple<bool, string> name_and_pass_check(string u, string p)
        {
            if (u is null || p is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments\n");
            }
            if (u == "" || p == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguments\n");
            }
            if (isUserExist(u))
                return new Tuple<bool, string>(false, "User name Exsist\n");
            if (u.Length < 3 || u.Length > 14)
                return new Tuple<bool, string>(false, "User name Don't meet the length Requirements\n");
            if(!u.All(char.IsLetterOrDigit))
            {
                return new Tuple<bool, string>(false, "User name Don't meet the Requirements chars\n");
            }
            return new Tuple<bool, string>(true, "");
        }
        //Check 1 or 2 arguments a=if input is valid.
        private Tuple<bool, string> check_args(string u, string p = "A")
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (u is null || p is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments\n");
            }
            if (u == "" || p == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguments\n");
            }
            return new Tuple<bool, string>(true, "");
        }
        //CHecks if the user is registered somewhere 
        //Means the user entered username and password
        public bool isUserExist(string username)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            //if user name exist return false
            return Users_And_Hashes.ContainsKey(username) || users.ContainsKey(username);
        }
        //Register the system admin
        public Tuple<bool, string> RegisterMaster(string username, string pass)
        {
            Logger.logSensitive(this, System.Reflection.MethodBase.GetCurrentMethod());
            Tuple<bool, string> ans = name_and_pass_check(username, pass);
            if (!ans.Item1)
                return ans;
        
            User System_Admin = new User(0, username, false, true);  
            users.Add(username, System_Admin);
            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username, sha1);
            DbUser dbadmin = DbManager.Instance.GetUser(username);
            if (dbadmin == null)
            {
                DbManager.Instance.InsertUser(AdapterUser.CreateDBUser(username, false, true, false));
                DbManager.Instance.InsertPassword(AdapterUser.CreateNewPasswordEntry(username, sha1), true);
            }
            return new Tuple<bool, string>(true, "");
        }
        //Register regular user to the system 
        //User name must be unique
        public Tuple<bool, string> Register(string username, string pass)
        {
            Logger.logSensitive(this, System.Reflection.MethodBase.GetCurrentMethod());
            Tuple<bool, string> ans = name_and_pass_check(username, pass);
            if (!ans.Item1)
                return ans;

            User nUser = new User(Available_ID, username, false);
            users.Add(username, nUser);
            //insert to user to db:
            DbManager.Instance.InsertUser(AdapterUser.CreateDBUser(username, false, false, false));
            Available_ID++;

            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username, sha1);
            DbManager.Instance.InsertPassword(AdapterUser.CreateNewPasswordEntry(username, sha1),true);
            return new Tuple<bool, string>(true, "");
        }
        //Login to Unlogged Register User with valid user name and pass.
        public Tuple<bool, string> Login(string username, string pass, bool isGuest = false)
        {
            Logger.logSensitive(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isGuest)
            {
                string Uname = addGuest();
                return new Tuple<bool, string>(true, Uname);
                //No need to Insert Guest to DB
            }
            Tuple<bool, string> ans = check_args(username, pass);
            if (!ans.Item1)
                return ans;
            string sha1 = SB.CalcSha1(pass);
            string temp_pass_hash;
            if (!Users_And_Hashes.TryGetValue(username, out temp_pass_hash))
                return new Tuple<bool, string>(false, "No such User: " + username + "\n");
            if (Users_And_Hashes.ContainsKey(username) && temp_pass_hash == sha1)
            {
                User tUser;
                if (!users.TryGetValue(username, out tUser))
                    return new Tuple<bool, string>(false, "Error occured User is not in the users_DB but is registered: " + username + " \n");
                if (tUser.LoggedStatus())
                    return new Tuple<bool, string>(false, "The user: " + username + " is already logged in\n");
                tUser.LogIn();
                //Update LogginStatus
                Statistics.Instance.InserRecord(username, DateTime.Now);
                DbManager.Instance.UpdateUserLogInStatus(tUser.getUserName(), false);
                Active_users.Add(tUser.getUserName(), tUser);
                if (tUser.HasPendingMessages()) 
                {
                    List<NotifyData> messages = tUser.GetPendingMessages();
                    foreach (NotifyData msg in messages) 
                    {
                        //Remove From DB Message
                        DbNotifyData dbMsg = DbManager.Instance.GetDbNotification(username, msg.Context); 
                        DbManager.Instance.DeleteSingleMessage(dbMsg);
                        //Try to send the message and if not recieved it will be enetred to DB Again inside Notify
                        Publisher.Instance.Notify(tUser.getUserName(), msg);
                        //tUser.RemovePendingMessage(msg);
                    }
                    tUser.RemoveAllPendingMessages();
                }
                DbManager.Instance.SaveChanges();
                return new Tuple<bool, string>(true, username + " Logged int\n");
            }
            DbManager.Instance.SaveChanges();
            return new Tuple<bool, string>(false, "Wrong Credentials\n");

        }
        public Tuple<bool, string> Logout(string sname, User user = null)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Tuple<bool, string> ans = check_args(sname);
            if (!ans.Item1)
                return ans;
            if (user is null)
                user = GetAtiveUser(sname);
            if (user == null)
                return new Tuple<bool, string>(false, sname + "is not Logged in\n");
            if (!user.LoggedStatus())
                return new Tuple<bool, string>(false, sname + "is not Logged in\n");
            if (user.isguest())
                return new Tuple<bool, string>(false, "Guest cannot Log out.\n");
            user.Logout();
            Active_users.Remove(user.getUserName());
            //Change LogInStatus at DB
            DbManager.Instance.UpdateUserLogInStatus(user.getUserName(), true);
            addGuest(false);
            return new Tuple<bool, string>(true, sname + " Logged out succesuffly\n");
        }
        //Add Guest user to the system and to the relevant lists.
        private string addGuest(bool islogout=true)
        {
            lock (this)
            {
                    //Add Guest DO not Require DB Changes
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
                string tName = "Guest" + Available_ID;
                User nUser = new User(Available_ID, tName);
                Console.WriteLine(tName);
                nUser.LogIn();
                Active_users.Add(tName, nUser);
                if (islogout)
                {
                    Statistics.Instance.InserRecord(tName, DateTime.Now);
                }
                Available_ID++;

                return tName;
            }
        }
        //Tries to get User from users list
        public User GetUser(string username)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (username is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
            User tUser;
            if (users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }
        //Tries to get user from logged in users.
        public virtual User GetAtiveUser(string username)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (username is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
            User tUser;
            if (Active_users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }
        //Function for Tests only add to active user (simulate login) 
        //And Remove from active users (Simulate logout).
        public void addtoactive(User u)
        {
            Active_users.Add(u.getUserName(), u);
            users.Add(u.getUserName(), u);
        }
        public void Rtoactive(User u)
        {
            Active_users.Remove(u.getUserName());
            users.Add(u.getUserName(), u);
        }


        public Tuple<bool, string> removeAllFromStore(int storeId)
        {
            //function should remove all store owners and store mangers from this store
            throw new NotImplementedException();
        }
        public Tuple<bool, string> removeOwner(Store store ,User user)
        {
            //when you delete a store owner of store you should also delete it from the store owner list and use the function below
            throw new NotImplementedException();
            // the function is here ...
            //store.RemoveOwner(user);
        }
        //For test clean the DB
        public void cleanup()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Console.WriteLine("UserManager Created\n");
            this.Users_And_Hashes = new Dictionary<string, string>();
            this.users = new Dictionary<string, User>();
            this.Active_users = new Dictionary<string, User>();
            this.SB = new Security();
            Available_ID = 1;
        }

        public List<string> GetApprovalListByStoreAndUser(string username, int storeID) 
        {
            List<string> appList;
            User appUser;
            if (!users.TryGetValue(username, out appUser) || !appUser.WaitingForApproval.TryGetValue(storeID, out appList))
                return new List<string>();
            return appList;
        }


        public List<Tuple<string, Permission>> GetStoreManagersPermissions(string appointer, int storeId)
        {
            User owner = GetUser(appointer);
            if (owner is null)
            {
                return null;
            }

            List<Tuple<string, Permission>> permissionsSet = new List<Tuple<string, Permission>>();

            foreach (User manager in users.Values)
            {
                if (manager.isStorManager(storeId) && manager.isAppointedByManager(owner, storeId))
                {
                    permissionsSet.Add(new Tuple<string, Permission>(manager.getUserName(), new Permission(manager.GetUserPermissions()[storeId])));
                }
            }

            return permissionsSet;
        }
        public List<string> GetAllAdmins()
        {
            List<string> admins = new List<string>();
            foreach(User usr in users.Values)
            {
                if (usr.IsAdmin && usr.LoggedStatus())
                    admins.Add(usr.getUserName());
            }
            return admins;
        }
    }
    


}