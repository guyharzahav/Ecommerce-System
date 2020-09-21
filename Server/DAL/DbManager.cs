using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.Communication.DataObject;
using Server.DAL.CommunicationDb;
using Server.DAL.PurchaseDb;
using Server.DAL.StoreDb;
using Server.DAL.UserDb;
using Server.StoreComponent.DomainLayer;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Reflection.PortableExecutable;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using System.ComponentModel;
using eCommerce_14a;
using Server.DAL.StatisticsDb;

namespace Server.DAL
{
    public class DbManager
    {

        private EcommerceContext dbConn;
        private static readonly object padlock = new object();
        private static DbManager instance = null;
        private bool testingmode;
        List<Tuple<object,string>> insertData;
        List<Tuple<object,string>> deleteData;
        private DbManager()
        {
            dbConn = new EcommerceContext();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String Root = Directory.GetCurrentDirectory();
            JObject data =  JObject.Parse(File.ReadAllText(Path.Combine(new string[] { Root, "configFile.json" })));
            testingmode = data.GetValue("mode").ToString().Equals("testing");
            insertData = new List<Tuple<object, string>>();
            deleteData = new List<Tuple<object, string>>();
        }

        public static DbManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DbManager();
                        }
                    }
                }
                return instance;
            }
        }




        public int GetNextProductId()
        {
            if(testingmode)
            {
                return 1;
            }
            if (dbConn.Products.Any())
            {
                return dbConn.Products.Max(product => product.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
        public void SaveChanges(bool working = true)
        {
            if (testingmode)
            {
                return;
            }
            if (!working)
            {
                Logger.logError("Save changes To Db Failed", this, System.Reflection.MethodBase.GetCurrentMethod());
                return;
            }
            if(dbConn.SaveChanges() <= 0)
            {
                Logger.logError("Save changes To Db Failed", this, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        public int GetNextPurchBasketId()
        {
            if (testingmode)
            {
                return 100;
            }

            if (dbConn.Baskets.Any())
            {
                return dbConn.Baskets.Max(b => b.Id) + 1;
            }
            else
            {
                return 1;
            }
        }

        public void DeApprovalTransaction(StoreOwnertshipApprovalStatus status, bool astatus, CandidateToOwnership cand, bool savechanges)
        {
            UpdateApprovalStatus(status, astatus);
            DeleteSingleCandidate(cand);
            if (savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteSingleCandidate(CandidateToOwnership candidateToOwnership, bool savechanges = false)
        {
            if (testingmode)
            {
                return ;
            }
            dbConn.CandidateToOwnerships.Remove(candidateToOwnership);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
            return;
        }

        public void AppendProductTransaction(Product product, int amount, int storeid, bool saveChanges)
        {
            InsertProduct(StoreAdapter.Instance.ToDbProduct(product), false);
            InsertInventoryItem(StoreAdapter.Instance.ToDbInventoryItem(product.Id, amount, storeid), false);
           if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public CandidateToOwnership GetCandidateToOwnership(string cand, string owner, int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.CandidateToOwnerships.Where(o => o.CandidateName == cand && o.AppointerName == owner && o.StoreId == storeId).FirstOrDefault();
        }

        public void RemoveProductTransaction(Product product)
        {
            DeleteInventoryItem(GetDbInventoryItem(product.Id, product.StoreId), false);
            DeleteProduct(Instance.GetDbProductItem(product.Id), false);
            dbConn.SaveChanges();
        }

        public void DeleteUser(DbUser user, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.Users.Remove(user);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
            return;
        }
        public void DeletePass(DbPassword pass,bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.Passwords.Remove(pass);
            if (savechanges)
            {
                dbConn.SaveChanges();
            }
            return;
        }

 
        public void DeleteSingleMessage(DbNotifyData msg, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.Notifies.Remove(msg);
            if(savechanges)
            {
               dbConn.SaveChanges();
            }
            
        }

        public DbNotifyData GetDbNotification(string username, string context) 
        {
            if (testingmode)
            {
                return null;
            }
            DbNotifyData msg = dbConn.Notifies.Where(m => m.Context == context && m.UserName == username).FirstOrDefault();
            return msg;
        }

        public void DeleteMessages(List<DbNotifyData> lmessage, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (DbNotifyData data in lmessage)
            {
                DeleteSingleMessage(data);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteCandidates(List<CandidateToOwnership> candidates, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (CandidateToOwnership can in candidates)
            {
                DeleteSingleCandidate(can);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

       

        public void DeleteSingleApproval(NeedToApprove msg, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.NeedToApproves.Remove(msg);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
            
        }
        public NeedToApprove GetNeedToApprove(string aprover, string cand, int storeid)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.NeedToApproves.Where(o => o.ApproverName == aprover && o.CandiateName == cand && o.StoreId == storeid).FirstOrDefault();
        }

        public void UpdateProductTransaction(Product product)
        {
            DbProduct dbProd = GetDbProductItem(product.Id);
            //DB update 
            UpdatePrdouct(dbProd, product, false);
            dbConn.SaveChanges();
        }



        public void DeleteAprovals(List<NeedToApprove> list, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }

            foreach (NeedToApprove single in list)
            {
                DeleteSingleApproval(single);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteSingleOwnership(StoreOwnershipAppoint msg, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreOwnershipAppoints.Remove(msg);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteOwnership(List<StoreOwnershipAppoint> list, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (StoreOwnershipAppoint single in list)
            {
                DeleteSingleOwnership(single);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteSingleManager(StoreManagersAppoint msg, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreManagersAppoints.Remove(msg);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public StoreManagersAppoint GetSingleManagerAppoints(string apr, string apo, int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreManagersAppoints.Where(ap => ap.AppointerName == apr && ap.AppointedName == apo && ap.StoreId == storeId).FirstOrDefault();
        }
        public StoreOwnershipAppoint GetSingleOwnesAppoints(string apr, string apo, int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreOwnershipAppoints.Where(ap => ap.AppointerName == apr && ap.AppointedName == apo && ap.StoreId == storeId).FirstOrDefault();
        }
        public void DeleteManagers(List<StoreManagersAppoint> list, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (StoreManagersAppoint single in list)
            {
                DeleteSingleManager(single);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteSinglePermission(UserStorePermissions msg, bool saveChanges=false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.UserStorePermissions.Remove(msg);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeletePermission(List<UserStorePermissions> list, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (UserStorePermissions single in list)
            {
                DeleteSinglePermission(single, saveChanges);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public Tuple<bool, string> PerformPurchaseTransaction(Cart userCart, PaymentHandler paymentHandler, string paymentDetails, string deliveryDetails, DeliveryHandler devHandler, Dictionary<Store, List<PurchaseBasket>> purchasesHistoryByStore, Dictionary<string, List<Purchase>> purchasesHistoryByUser, bool Failed = false)
        {
            string user = userCart.user;
            Tuple<bool, string> updatePriceRes = userCart.UpdateCartPrice(false);
            if (!updatePriceRes.Item1)
            {
                return new Tuple<bool, string>(false, "there was err in PerformPurchase when Calling to UpdatePrice with Db Update " + updatePriceRes.Item2);
            }

            int payRes = paymentHandler.pay(paymentDetails,Failed);
            if (payRes == -1)
            {
                return new Tuple<bool, string>(false, "payment faield");
            }

            Tuple<bool, string> removeFromStockRes = userCart.RemoveFromStoresStock(false);

            if (!removeFromStockRes.Item1)
            {
                Tuple<bool, string> refund_res = paymentHandler.refund(payRes);
                if (!refund_res.Item1)
                    return refund_res;
                return removeFromStockRes;
            }

            Tuple<bool, string> delvRes = devHandler.ProvideDeliveryForUser(deliveryDetails);
            if (!delvRes.Item1)
            {
                userCart.RestoreItemsToStores(false);
                Tuple<bool, string> refund_res = paymentHandler.refund(payRes);
                if (!refund_res.Item1)
                    return refund_res;
                return delvRes;
            }

            Tuple<bool, string> setPurchaseTimeRes =  userCart.SetPurchaseTime(DateTime.Now, false);
            if(!setPurchaseTimeRes.Item1)
            {
                userCart.RestoreItemsToStores(false);
                Tuple<bool, string> refund_res = paymentHandler.refund(payRes);
                if (!refund_res.Item1)
                    return refund_res;

                return setPurchaseTimeRes;
            }

            foreach (Store store in userCart.GetBaskets().Keys)
            {
                if (!purchasesHistoryByStore.TryGetValue(store, out List<PurchaseBasket> currHistory))
                {
                    currHistory = new List<PurchaseBasket>();
                }

                currHistory.Add(userCart.GetBaskets()[store]);
                purchasesHistoryByStore[store] = currHistory;
                //Version 2 Addition
                string message_data = "Purchase was made from - " + user + ",StoreId - " + store.GetStoreId() + " ,StoreName -" + store.GetName() + ", Products: " + (userCart.GetBaskets()[store]).ToString();
                Tuple<bool, string> ans = Publisher.Instance.Notify(store.GetStoreId(), new NotifyData(message_data));
                if (!ans.Item1)
                    return ans;
            }
            Purchase newPurchase = new Purchase(user, userCart);
            if (!purchasesHistoryByUser.TryGetValue(user, out List<Purchase> userHistory))
            {
                userHistory = new List<Purchase>();
            }

            userHistory.Add(newPurchase);
            // DB Insert New Purchase
            if (!UserManager.Instance.GetAtiveUser(user).IsGuest)
            {
                InsertPurchase(StoreAdapter.Instance.ToDbPurchase(newPurchase), false);
            }
            purchasesHistoryByUser[user] = userHistory;
            //DB Updating cart status to purchased
            if (!UserManager.Instance.GetAtiveUser(user).IsGuest)
            {

               UpdateDbCart(GetDbCart(userCart.Id), userCart, true, false);
            }

            dbConn.SaveChanges();

            return new Tuple<bool, string>(true, "");

        }

        public List<UserStorePermissions> GetUserStorePermissionSet(int storeId, string username)
        {
            if (testingmode)
            {
                return null;
            }
            List<UserStorePermissions> res = new List<UserStorePermissions>();
            res = dbConn.UserStorePermissions.Where(pe => pe.StoreId == storeId && pe.UserName == username).ToList();
            return res;
        }
        public void DeleteSingleApprovalStatus(StoreOwnertshipApprovalStatus msg, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreOwnertshipApprovalStatuses.Remove(msg);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void DeleteAprovalsStatuses(List<StoreOwnertshipApprovalStatus> list, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (StoreOwnertshipApprovalStatus single in list)
            {
                DeleteSingleApprovalStatus(single);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

     
        public void DeleteUserFullCascase(User user)
        {
            if (testingmode)
            {
                return;
            }
            string name = user.getUserName();
            //Delete the user
            DbUser usr = dbConn.Users.Where(buyer => buyer.Name == name).FirstOrDefault();
            DeleteUser(usr);
            //Delete the users hash
            DbPassword pass = dbConn.Passwords.Where(buyer => buyer.UserName == name).FirstOrDefault();
            DeletePass(pass);
            //Delete Messages
            List<DbNotifyData> messages = dbConn.Notifies.Where(m => m.UserName == name).ToList();
            DeleteMessages(messages);
            //Delete Candidations
            List<CandidateToOwnership> candidations = dbConn.CandidateToOwnerships.Where(c => c.CandidateName == name).ToList();
            DeleteCandidates(candidations);
            //Delete all aprovvals requests
            //INeedToApprove
            List<NeedToApprove> ineed = dbConn.NeedToApproves.Where(n => n.ApproverName == name).ToList();
            List<NeedToApprove> theyNeed = dbConn.NeedToApproves.Where(n => n.CandiateName == name).ToList();
            ineed.AddRange(theyNeed);
            DeleteAprovals(ineed);
            //Ownership
            List<StoreOwnershipAppoint> owner = dbConn.StoreOwnershipAppoints.Where(a => a.AppointerName == name).ToList();
            List<StoreOwnershipAppoint> Maxowner = dbConn.StoreOwnershipAppoints.Where(a => a.AppointedName == name).ToList();
            owner.AddRange(Maxowner);
            DeleteOwnership(owner);
            //Managers
            List<StoreManagersAppoint> manager = dbConn.StoreManagersAppoints.Where(a => a.AppointerName == name).ToList();
            List<StoreManagersAppoint> MaxManager = dbConn.StoreManagersAppoints.Where(a => a.AppointedName == name).ToList();
            manager.AddRange(MaxManager);
            DeleteManagers(manager);
            //Permissions
            List<UserStorePermissions> perm = dbConn.UserStorePermissions.Where(p => p.UserName == name).ToList();
            DeletePermission(perm);
            //Approvals
            List<StoreOwnertshipApprovalStatus> apprvs = dbConn.StoreOwnertshipApprovalStatuses.Where(c => c.CandidateName == name).ToList();
            DeleteAprovalsStatuses(apprvs);
        }

        internal DbUser GetUser(string username)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.Users.Where(u => u.Name == username).FirstOrDefault();
        }

        public List<DbUser> getAllDBUsers()
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.Users.ToList();
        }
        public List<User> LoadAllUsers()
        {
            if (testingmode)
            {
                return null;
            }
            List<string> usernames = dbConn.Users.Select(user => user.Name).Distinct().ToList();
            List<DbPassword> passes = dbConn.Passwords.ToList();
            //Dictionary<string, string> name_and_hashes = new Dictionary<string, string>();
            foreach (DbPassword pass in passes)
            {
                if (!UserManager.Instance.Users_And_Hashes.ContainsKey(pass.UserName))
                {
                    UserManager.Instance.Users_And_Hashes.Add(pass.UserName, pass.PwdHash);
                }
            }
            List<User> users = new List<User>();
            foreach (string user in usernames)
            {
                User usr = BuildUser(user);
                usr.IsLoggedIn = false;
                if(usr.LoggedStatus())
                {
                    usr.IsLoggedIn = false;
                    //if (!UserManager.Instance.Active_users.ContainsKey(user))
                    //{
                    //    UserManager.Instance.Active_users.Add(user, usr);
                    //}
                }
                if (!UserManager.Instance.users.ContainsKey(user))
                {
                    UserManager.Instance.users.Add(user, usr);
                }
                users.Add(usr);
            }
            return users;
        }

        public User BuildUser(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            DbUser dbUser = dbConn.Users.Where(dbuser => dbuser.Name == userName).FirstOrDefault();
            User user = new User(1, dbUser.Name, dbUser.IsGuest, dbUser.IsAdmin);
            user.IsLoggedIn = dbUser.IsLoggedIn;

            user.Store_Ownership = GetStoreOwnershipUser(userName);
            user.unreadMessages = GetUnreadMessages(userName);
            user.Store_Managment = GetStoreManagmentUser(userName);
            user.Store_options = GetStorePermissionsUser(userName);
            user.MasterAppointer = GetMasterAppointersUsers(userName);
            user.NeedToApprove = GetTheyNeedToApproveUsers(userName);
            user.WaitingForApproval = GeTINeedToApproveUsers(userName);
            user.IsApproved = GetIsApprovedStoreUsers(userName);

            return user;
        }

        public Tuple<int,string> InsertStoreTranscation(Store store, User user, string userName, int next_id)
        {
            try
            {
               InsertStore(store, false);
            }
            catch
            {
                return new Tuple<int, string>(-1, CommonStr.GeneralErrMessage.DbErrorMessage);
            }

            Tuple<bool, string> ownershipAdded = user.addStoreOwnership(store.Id, userName, false);

            if (!ownershipAdded.Item1)
            {
                return new Tuple<int, string>(-1, ownershipAdded.Item2);
            }
            else
            {
                //Version 2 Addition
                Tuple<bool, string> ans = Publisher.Instance.subscribe(userName, next_id);
                if (!ans.Item1)
                    return new Tuple<int, string>(-1, ans.Item2);

                DbManager.Instance.SaveChanges();
                return new Tuple<int, string>(next_id, "");
            }
        }
        public void InsertStatisticRecord(string userName, DateTime date, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            try
            {
                dbConn.Statistics.Add(new DbStatistics(userName, date));
            }
            catch(Exception ex)
            {
                return;
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public List<Tuple<string,DateTime>> GetStatisticsRecords()
        {
            if(testingmode)
            {
                return null;
            }
            List<Tuple<string, DateTime>> str = new List<Tuple<string, DateTime>>();
            List<DbStatistics> allstatistics = dbConn.Statistics.ToList();
            foreach(DbStatistics dbsts in allstatistics)
            {
                str.Add(new Tuple<string,DateTime>(dbsts.Name, dbsts.DateTime));
            }
            return str;
        }
        public Dictionary<int,bool> GetIsApprovedStoreUsers(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            Dictionary<int, bool> status = new Dictionary<int, bool>();
            List<StoreOwnertshipApprovalStatus> statuses = dbConn.StoreOwnertshipApprovalStatuses.Where(cand => cand.CandidateName == userName).ToList();
            foreach(StoreOwnertshipApprovalStatus stat in statuses)
            {
                status.Add(stat.StoreId, stat.Status);
            }
            return status;

        }
        public Dictionary<int, List<string>> GeTINeedToApproveUsers(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            Dictionary<int, List<string>> INeedToApproveUser = new Dictionary<int, List<string>>();
            List<NeedToApprove> listodusers = dbConn.NeedToApproves.Where(cand => cand.ApproverName == userName).ToList();
            foreach (NeedToApprove rshuma in listodusers)
            {
                List<string> userslist;
                if (!INeedToApproveUser.TryGetValue(rshuma.StoreId, out userslist))
                {
                    userslist = new List<string>();
                    userslist.Add(rshuma.CandiateName);
                    INeedToApproveUser.Add(rshuma.StoreId, userslist);
                }
                else
                {
                    if (!userslist.Contains(rshuma.CandiateName))
                    {
                        userslist.Add(rshuma.CandiateName);
                        INeedToApproveUser[rshuma.StoreId] = userslist;
                    }
                }
            }
            return INeedToApproveUser;
        }
        public Dictionary<int,List<string>> GetTheyNeedToApproveUsers(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            Dictionary<int, List<string>> theyneedToApproveUser = new Dictionary<int, List<string>>();
            List<NeedToApprove> listodusers = dbConn.NeedToApproves.Where(cand => cand.CandiateName == userName).ToList();
            foreach(NeedToApprove rshuma in listodusers)
            {
                List<string> userslist;
                if(!theyneedToApproveUser.TryGetValue(rshuma.StoreId,out userslist))
                {
                    userslist = new List<string>();
                    userslist.Add(rshuma.ApproverName);
                    theyneedToApproveUser.Add(rshuma.StoreId, userslist);
                }
                else
                {
                    if(!userslist.Contains(rshuma.ApproverName))
                    {
                        userslist.Add(rshuma.ApproverName);
                        theyneedToApproveUser[rshuma.StoreId] = userslist;
                    }
                }
            }
            return theyneedToApproveUser;
        }

        public List<DbPurchaseBasket> GetCartBaskets(int cartid)
        {
  
            return dbConn.Baskets.Where(b => b.CartId == cartid).ToList();
        }

        public List<ProductAtBasket> GetAllProductAtBasket(int basketid)
        {
           
            return dbConn.ProductsAtBaskets.Where(pab => pab.BasketId == basketid).ToList();
        }

        public Dictionary<int,string> GetMasterAppointersUsers(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            List<CandidateToOwnership> canidadtions = dbConn.CandidateToOwnerships.Where(cand => cand.CandidateName == userName).ToList();
            Dictionary<int, string> candidtaes = new Dictionary<int, string>();
            foreach(CandidateToOwnership can in canidadtions)
            {
                candidtaes.Add(can.StoreId, can.AppointerName);
            }
            return candidtaes;
        }
        public Dictionary<int, int[]> GetStorePermissionsUser(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            Dictionary<int,int[]> perms  = new Dictionary<int, int[]>();
            List<UserStorePermissions> AllPerms = dbConn.UserStorePermissions.Where(storeP => storeP.UserName == userName).ToList();
            foreach(UserStorePermissions permmission in AllPerms)
            {
                int[] p;
                string permmisionName = permmission.Permission;
                if(permmisionName.Equals(CommonStr.MangerPermission.Comments))
                {
                    if (!perms.TryGetValue(permmission.StoreId, out p))
                    {
                        p = new int[] { 1,0,0,0,0};
                        perms.Add(permmission.StoreId, p);
                    }
                    else
                    {
                        p[0] = 1;
                        perms[permmission.StoreId] = p;
                    }

                }
                else if (permmisionName.Equals(CommonStr.MangerPermission.Purchase))
                {
                    if (!perms.TryGetValue(permmission.StoreId, out p))
                    {
                        p = new int[] { 0, 1, 0, 0, 0 };
                        perms.Add(permmission.StoreId, p);
                    }
                    else
                    {
                        p[1] = 1;
                        perms[permmission.StoreId] = p;
                    }

                }
                else if (permmisionName.Equals(CommonStr.MangerPermission.Product))
                {
                    if (!perms.TryGetValue(permmission.StoreId, out p))
                    {
                        p = new int[] { 0, 0, 1, 0, 0 };
                        perms.Add(permmission.StoreId, p);
                    }
                    else
                    {
                        p[2] = 1;
                        perms[permmission.StoreId] = p;
                    }

                }
                else if (permmisionName.Equals(CommonStr.MangerPermission.PurachsePolicy))
                {
                    if (!perms.TryGetValue(permmission.StoreId, out p))
                    {
                        p = new int[] { 0, 0, 0, 1, 0 };
                        perms.Add(permmission.StoreId, p);
                    }
                    else
                    {
                        p[3] = 1;
                        perms[permmission.StoreId] = p;
                    }

                }
                else if (permmisionName.Equals(CommonStr.MangerPermission.DiscountPolicy))
                {
                    if (!perms.TryGetValue(permmission.StoreId, out p))
                    {
                        p = new int[] { 0, 0, 0, 0, 1 };
                        perms.Add(permmission.StoreId, p);
                    }
                    else
                    {
                        p[4] = 1;
                        perms[permmission.StoreId] = p;
                    }

                }
            }
            return perms;
        }
        public Dictionary<int, string> GetStoreManagmentUser(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            List<StoreManagersAppoint> storeManagingAppoints = dbConn.StoreManagersAppoints.Where(soa => soa.AppointedName == userName).ToList();
            Dictionary<int, string> ManagersDict = new Dictionary<int, string>();
            foreach (StoreManagersAppoint soa in storeManagingAppoints)
            {
                ManagersDict.Add(soa.StoreId, soa.AppointerName);
            }
            return ManagersDict;
        }
        public List<NotifyData> GetUnreadMessages(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            List<NotifyData> messages = new List<NotifyData>();
            List<DbNotifyData> DBMessages = dbConn.Notifies.Where(message => message.UserName == userName).ToList();
            foreach(DbNotifyData message in DBMessages)
            {
                NotifyData ndata = new NotifyData(message.Context, message.UserName);
                messages.Add(ndata);
            }
            return messages;
        }
        public Dictionary<int, string> GetStoreOwnershipUser(string userName)
        {
            if (testingmode)
            {
                return null;
            }
            List<StoreOwnershipAppoint> storeOwnershipAppoints = dbConn.StoreOwnershipAppoints.Where(soa => soa.AppointedName == userName).ToList();
            Dictionary<int, string> ownershipsDict = new Dictionary<int, string>();
            foreach (StoreOwnershipAppoint soa in storeOwnershipAppoints)
            {
                ownershipsDict.Add(soa.StoreId, soa.AppointerName);
            }
            return ownershipsDict;
        }

        public void InsertStore(Store store, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            InsertDbstore(StoreAdapter.Instance.ToDbStore(store), false);
            //InsertInventory(store.Inventory, store.Id);
            InsertDiscountPolicy(store.DiscountPolicy, store.Id, parentId: null, false);
            InsertPurchasePolicy(store.PurchasePolicy, store.Id, parentId: null, false);
            InsertOwners(store.owners, store.Id, false);
            InsertManagers(store.managers, store.Id, false);
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertPurchaseBasket(PurchaseBasket basket, int cartId, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            DbPurchaseBasket dbPurchaseBasket = StoreAdapter.Instance.ToDbPurchseBasket(basket, cartId);
            if(GetDbPurchaseBasket(dbPurchaseBasket.Id) == null)
            {
                dbConn.Baskets.Add(dbPurchaseBasket);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

     
        public ProductAtBasket GetProductAtBasket(int basketId, int productId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.ProductsAtBaskets.Where(pab => pab.BasketId == basketId && pab.ProductId == productId).FirstOrDefault();
        }

        public void UpdateProductAtBasket(ProductAtBasket productAtBasket, int wantedAmount, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            productAtBasket.ProductAmount = wantedAmount;
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        private void InsertDbstore(DbStore dbStore, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbStore(dbStore.Id) == null)
            {
                dbConn.Stores.Add(dbStore);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void UpdateDbCart(DbCart dbCart, Cart cart, bool isPurchased, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbCart.Price = cart.Price;
            dbCart.IsPurchased = isPurchased;
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertProductAtBasket(ProductAtBasket productAtBasket, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetProductAtBasket(productAtBasket.BasketId, productAtBasket.ProductId) == null)
            {
                dbConn.ProductsAtBaskets.Add(productAtBasket);
            }
            dbConn.SaveChanges();
        }

        public void InsertPurchasePolicy(PurchasePolicy policyData, int storeId, int? parentId, bool saveChanges)
        {
            saveChanges = true;
            if (testingmode)
            {
                return;
            }
            if (policyData.GetType() == typeof(ProductPurchasePolicy))
            {  
                int policyProdutId = ((ProductPurchasePolicy)policyData).ProductId;
                int preCondition = ((ProductPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                ProductPurchasePolicy policy = ((ProductPurchasePolicy)policyData);
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionnumber: preCondition,
                                                                 policyproductid: policyProdutId,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.ProductPurchasePolicy,
                                                                 maxproductidunits: policy.MaxAmount,
                                                                 minproductidsunits: policy.MinAmount,
                                                                 maxitemsatbasket: null,
                                                                 minitemsatbasket: null,
                                                                 minbasketprice: null,
                                                                 maxbaskeptrice: null));
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (policyData.GetType() == typeof(BasketPurchasePolicy))
            {
                BasketPurchasePolicy policy = ((BasketPurchasePolicy)policyData);
                int preCondition = ((BasketPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionnumber: preCondition,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy,
                                                                 maxproductidunits:null,
                                                                 minproductidsunits:null,
                                                                 maxitemsatbasket: policy.MaxItems,
                                                                 minitemsatbasket: policy.MinItems,
                                                                 minbasketprice:   policy.MinBasketPrice,
                                                                 maxbaskeptrice:   policy.MaxBasketPrice));;
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (policyData.GetType() == typeof(SystemPurchasePolicy))
            {

                int preCondition = ((SystemPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionnumber: preCondition,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.SystemPurchasePolicy,
                                                                 maxproductidunits:null,
                                                                 minproductidsunits:null,
                                                                 maxitemsatbasket:null,
                                                                 minitemsatbasket:null,
                                                                 minbasketprice:null,
                                                                 maxbaskeptrice:null));
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (policyData.GetType() == typeof(UserPurchasePolicy))
            {
                int preCondition = ((UserPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                mergetype: null,
                                                                parentid: parentId,
                                                                preconditionnumber: preCondition,
                                                                policyproductid: null,
                                                                buyerusername: null,
                                                                purchasepolictype: CommonStr.PurchasePolicyTypes.UserPurchasePolicy,
                                                                maxproductidunits:null,
                                                                minproductidsunits:null,
                                                                maxitemsatbasket:null,
                                                                minitemsatbasket:null,
                                                                minbasketprice:null,
                                                                maxbaskeptrice:null));
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (policyData.GetType() == typeof(CompundPurchasePolicy))
            {
                int mergetype = ((CompundPurchasePolicy)policyData).mergeType;
                DbPurchasePolicy dbPurchase = new DbPurchasePolicy(storeId: storeId,
                                                                   mergetype: mergetype,
                                                                   parentid: parentId,
                                                                   preconditionnumber: null,
                                                                   policyproductid: null,
                                                                   buyerusername: null,
                                                                   purchasepolictype: CommonStr.PurchasePolicyTypes.CompundPurchasePolicy,
                                                                   maxproductidunits:null,
                                                                   minproductidsunits:null,
                                                                   maxitemsatbasket:null,
                                                                   minitemsatbasket:null,
                                                                   minbasketprice:null,
                                                                   maxbaskeptrice:null);
                dbConn.PurchasePolicies.Add(dbPurchase);
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }

                int dbPolicyId = GetDbPurchsePolicyId(dbPurchase, storeId, null);
                List<PurchasePolicy> policies = ((CompundPurchasePolicy)policyData).getChildren();

                foreach (PurchasePolicy policy in policies)
                {
                    InsertPurchasePolicy(policy, storeId, dbPolicyId, saveChanges);
                }

            }

        }


        public DbPurchaseBasket GetDbPurchaseBasket(int id)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.Baskets.Where(b => b.Id == id).FirstOrDefault();
        }

        public DbCart GetDbCart(int id)
        {
            if(testingmode)
            {
                return null;
            }
            return dbConn.Carts.Where(c => c.Id == id).FirstOrDefault();
        }

        public void UpdatePrdouct(DbProduct dbProd, Product product, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbProd.Category = product.Category;
            dbProd.Details = product.Details;
            dbProd.ImgUrl = product.ImgUrl;
            dbProd.Name = product.Name;
            dbProd.Price = product.Price;
            dbProd.Rank = product.Rank;
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void UpdateInventoryItem(DbInventoryItem dbInventoryItem, int amount, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbInventoryItem.Amount = amount;
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public StoreManager getStoreManager(string name)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreManagers.Where(m => m.ManagerName.Equals(name)).FirstOrDefault();
        }

        public StoreOwner getStoreOwner(string name)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreOwners.Where(o => o.OwnerName.Equals(name)).FirstOrDefault();
        }
        public StoreOwner getStoreOwnerbyStore(string name, int storeID)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreOwners.Where(o => o.OwnerName.Equals(name) && o.StoreId == storeID).FirstOrDefault();
        }
        public StoreManager getStoreManagerbyStore(string name, int storeID)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreManagers.Where(o => o.ManagerName.Equals(name) && o.StoreId == storeID).FirstOrDefault();
        }
        public void UpdateDiscountPolicy(DiscountPolicy newPolicy,Store s, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }

            DeleteAllStoreDiscountPolicy(s, false);
            InsertDiscountPolicy(newPolicy, s.Id, null, false);
            if (saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void UpdatePurchasePolicy(PurchasePolicy newPolicy, Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            
            DeleteAllStorePurchasePolicy(store, false);
            InsertPurchasePolicy(newPolicy, store.Id, null, false);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void UpdateStore(DbStore dbstore, Store store, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            dbstore.ActiveStore = store.ActiveStore;
            dbstore.Rank = store.Rank;
            dbstore.StoreName = store.StoreName;
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        public void UpdatePurchaseBasket(DbPurchaseBasket dbPurchaseBasket, PurchaseBasket basket, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbPurchaseBasket.Price = basket.Price;
            dbPurchaseBasket.PurchaseTime = basket.PurchaseTime;
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public DbProduct GetDbProductItem(int productId)
        {
            if (testingmode)
            {
                return null;
            }
            return  dbConn.Products.Where(p => p.Id == productId).FirstOrDefault();
        }

        internal DbInventoryItem GetDbInventoryItem(int productId, int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.InventoriesItmes.Where(invItem => invItem.ProductId == productId  && invItem.StoreId == storeId).FirstOrDefault();
        }

        internal void InsertInventoryItem(DbInventoryItem dbInventoryItem, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbInventoryItem(dbInventoryItem.ProductId, dbInventoryItem.StoreId) == null)
            {
                dbConn.InventoriesItmes.Add(dbInventoryItem);
            }
            if(saveCahnges)
            { 
                dbConn.SaveChanges();
            }

        }

        public void InsertCart(DbCart dbCart, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbCart(dbCart.Id) == null)
            {
                dbConn.Carts.Add(dbCart);
            }
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        public int GetNextStoreId()
        {
            if (testingmode)
            {
                List<int> storesids = StoreManagment.Instance.GetAllStores().Select(store => store.Id).Distinct().ToList();
                if (storesids.Count > 0)
                {
                    int max = storesids.Max(id => id);
                    if (max > 0)
                    {
                        return max + 1;
                    }
                }
                else
                {
                    return 100;
                }
            }
            if (!dbConn.Stores.Any())
            {
                return 1;
            }

            int max_storeId = dbConn.Stores.Max(store => store.Id);
            return max_storeId + 1;
        }

        public void InsertDiscountPolicy(DiscountPolicy discountPolicy, int storeId, int? parentId, bool saveChanges)
        {
            saveChanges = true;
            if (testingmode)
            {
                return;
            }
            if (discountPolicy.GetType() == typeof(ConditionalProductDiscount))
            {
                int discountProdutId = ((ConditionalProductDiscount)discountPolicy).discountProdutId;
                int preCondition_num = ((ConditionalProductDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalProductDiscount)discountPolicy).Discount;
                ConditionalProductDiscount policy = ((ConditionalProductDiscount)discountPolicy);
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition_num, CommonStr.PreConditionType.DiscountPreCondition);
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preConditionnumber: preCondition_num,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount,
                                                                 minproductunits: policy.MinUnits,
                                                                 minbaskeptice: null,
                                                                 minproductprice: null,
                                                                 minunitsatbasket: null));
                if(saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (discountPolicy.GetType() == typeof(ConditionalBasketDiscount))
            {
                int preCondition = ((ConditionalBasketDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalBasketDiscount)discountPolicy).Discount;
                ConditionalBasketDiscount policy = ((ConditionalBasketDiscount)discountPolicy);
                //DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.DiscountPreCondition);
                DbDiscountPolicy dbDiscount = new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preConditionnumber: preCondition,
                                                                 discountproductid: null,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount,
                                                                 minproductunits: null,
                                                                 minbaskeptice: policy.MinBasketPrice,
                                                                 minproductprice: policy.MinProductPrice,
                                                                 minunitsatbasket: policy.MinUnitsAtBasket);
                dbConn.DiscountPolicies.Add(dbDiscount);
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (discountPolicy.GetType() == typeof(RevealdDiscount))
            {


                int discountProdutId = ((RevealdDiscount)discountPolicy).discountProdutId;
                double discountPrecentage = ((RevealdDiscount)discountPolicy).discount;
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preConditionnumber: null,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.RevealdDiscount,
                                                                 minproductunits:null,
                                                                 minbaskeptice:null,
                                                                 minproductprice:null,
                                                                 minunitsatbasket:null));
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
            }

            else if (discountPolicy.GetType() == typeof(CompundDiscount))
            {
                int mergetype = ((CompundDiscount)discountPolicy).GetMergeType();
                DbDiscountPolicy dbDiscount = new DbDiscountPolicy(storeid: storeId,
                                                              mergetype: mergetype,
                                                              parentId: parentId,
                                                              preConditionnumber: null,
                                                              discountproductid: null,
                                                              discount: null,
                                                              discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount,
                                                              minproductunits:null,
                                                              minbaskeptice:null,
                                                              minproductprice:null,
                                                              minunitsatbasket:null);
                dbConn.DiscountPolicies.Add(dbDiscount);
                if (saveChanges)
                {
                    dbConn.SaveChanges();
                }
                int dbPolicyId = GetDbDiscountPolicyId(dbDiscount, storeId, null);
                List<DiscountPolicy> policies = ((CompundDiscount)discountPolicy).getChildren();
                foreach (DiscountPolicy policy in policies)
                {
                    InsertDiscountPolicy(discountPolicy: policy, storeId: storeId, parentId: dbPolicyId, saveChanges);
                }
            }
        }

     

        public DbStore GetDbStore(int id)
        {
            return dbConn.Stores.Where(s => s.Id == id).FirstOrDefault();
        }

        private int GetDbDiscountPolicyId(DbDiscountPolicy dbDiscountPolicy, int storeId, int? precondition)
        {

            DbDiscountPolicy dbFromDb;
            if (precondition == null)
            {
               dbFromDb = dbConn.DiscountPolicies.Where(dbDiscount => dbDiscountPolicy.Discount == dbDiscount.Discount &
                                                                        dbDiscountPolicy.DiscountProductId == dbDiscount.DiscountProductId &
                                                                        dbDiscountPolicy.MergeType == dbDiscount.MergeType &
                                                                        dbDiscountPolicy.StoreId == storeId &
                                                                        dbDiscountPolicy.ParentId == dbDiscount.ParentId).FirstOrDefault();
            }
            else
            {
                dbFromDb = dbConn.DiscountPolicies.Where(dbDiscount => dbDiscountPolicy.Discount == dbDiscount.Discount &
                                                 dbDiscountPolicy.DiscountProductId == dbDiscount.DiscountProductId &
                                                 dbDiscountPolicy.MergeType == dbDiscount.MergeType &
                                                 dbDiscountPolicy.StoreId == storeId &
                                                 dbDiscountPolicy.ParentId == dbDiscount.ParentId &
                                                 dbDiscountPolicy.PreConditionNumber == precondition).FirstOrDefault();
            }

            return dbFromDb.Id;
                                                        
        }


        private int GetDbPurchsePolicyId(DbPurchasePolicy dbPurchasePolicy, int storeId, int? precondition)
        {
            DbPurchasePolicy dbFromDb;
            if (precondition == null)
            {
                dbFromDb = dbConn.PurchasePolicies.Where(dbPurchase => dbPurchasePolicy.BuyerUserName == dbPurchase.BuyerUserName &
                                                                 dbPurchasePolicy.MergeType == dbPurchase.MergeType &
                                                                 dbPurchasePolicy.PolicyProductId == dbPurchase.PolicyProductId &
                                                                 dbPurchasePolicy.StoreId == storeId &
                                                                 dbPurchasePolicy.ParentId == dbPurchase.ParentId).FirstOrDefault();
            }
            else
            {
              dbFromDb = dbConn.PurchasePolicies.Where(dbPurchase => dbPurchasePolicy.BuyerUserName == dbPurchase.BuyerUserName &
                                                                                dbPurchasePolicy.MergeType == dbPurchase.MergeType &
                                                                                dbPurchasePolicy.PolicyProductId == dbPurchase.PolicyProductId &
                                                                                dbPurchasePolicy.StoreId == storeId &
                                                                                dbPurchasePolicy.ParentId == dbPurchase.ParentId &
                                                                                dbPurchasePolicy.PreConditionNumber == precondition).FirstOrDefault();
            }
         
            return dbFromDb.Id;

        }

 


        private void InsertOwners(List<string> owners, int storeId, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            foreach (string owner in owners)
            {
                InsertStoreOwner(new StoreOwner(storeId, owner), false);
            }

            if(saveChanges)
            {
                dbConn.SaveChanges();
            }

        }

        private void InsertManagers(List<string> managers, int storeId, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            foreach (string manager in managers)
            {
                InsertStoreManager(new StoreManager(storeId, manager), false);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertStoreOwner(StoreOwner owner, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(getStoreOwner(owner.OwnerName) == null)
            {
                dbConn.StoreOwners.Add(owner);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertStoreManager(StoreManager manager, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(getStoreManagerbyStore(manager.ManagerName,manager.StoreId) == null)
            {
                dbConn.StoreManagers.Add(manager);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsretInventoryItem(DbInventoryItem invItem)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbInventoryItem(invItem.ProductId, invItem.StoreId) == null)
            {
                dbConn.InventoriesItmes.Add(invItem);
            }
            dbConn.SaveChanges();
        }




        public void InsertProduct(DbProduct product, bool savechanges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbProductItem(product.Id) == null)
            {
                dbConn.Products.Add(product);
            }
            if (savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertPurchase(DbPurchase dbPurchase, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            if(GetPurchase(dbPurchase.CartId) == null)
            {
                dbConn.Purchases.Add(dbPurchase);
            }
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        private DbPurchase GetPurchase(int cartId)
        {
            return dbConn.Purchases.Where(s => s.CartId == cartId).FirstOrDefault();

        }

        public List<Store> LoadAllStores()
        {
          
            List<int> storesIds = dbConn.Stores.Select(store => store.Id).Distinct().ToList();
            List<Store> stores = new List<Store>();
            foreach(int storeId in storesIds)
            {
                stores.Add(LoadStore(storeId));
            }
            return stores;
        }

        public List<Purchase> LoadPurchases()
        {
         
            List<Purchase> purchases = new List<Purchase>();
            foreach (DbPurchase dbPurchase in dbConn.Purchases)
            {
                purchases.Add(StoreAdapter.Instance.ComposePurchse(dbPurchase));
            }
            return purchases;
        }

        public List<Cart> LoadNotPurchasedCarts()
        {
 
            List<Cart> carts = new List<Cart>();
            List<DbCart> dbcarts = dbConn.Carts.Where(c => c.IsPurchased == false).ToList();
            foreach(DbCart dbCart in dbcarts)
            {
                carts.Add(StoreAdapter.Instance.ComposeCart(dbCart.Id, dbCart.UserName));
            }
            return carts;
        }

      

        //GET Store Component Functions
        public Store LoadStore(int StoreId)
        {
 
            DbStore dbstore = dbConn.Stores.Where(store => store.Id == StoreId).FirstOrDefault();
            if(dbstore == null)
            {
                return null;
            }
            DiscountPolicy discountPolicy = LoadDiscountPolicy(StoreId);
            PurchasePolicy purchasePolicy = LoadPurchasePolicy(StoreId);
            Inventory inventory = LoadInventory(StoreId);
            Dictionary<string, object> store_params = new Dictionary<string, object>();
            store_params.Add(CommonStr.StoreParams.StoreId, StoreId);
            store_params.Add(CommonStr.StoreParams.StoreName, dbstore.StoreName);
            store_params.Add(CommonStr.StoreParams.IsActiveStore, dbstore.ActiveStore);
            store_params.Add(CommonStr.StoreParams.StoreRank, dbstore.Rank);
            store_params.Add(CommonStr.StoreParams.StorePuarchsePolicy, purchasePolicy);
            store_params.Add(CommonStr.StoreParams.StoreDiscountPolicy, discountPolicy);
            store_params.Add(CommonStr.StoreParams.StoreInventory, inventory);
            List<string> owners = LoadStoreOwners(StoreId);
            store_params.Add(CommonStr.StoreParams.Owners, owners);
            List<string> managers= LoadStoreManagers(StoreId);
            store_params.Add(CommonStr.StoreParams.Managers, managers);
            return new Store(store_params);
        }

        private DiscountPolicy LoadDiscountPolicy(int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            List<DbDiscountPolicy> storeDiscountPolicies = dbConn.DiscountPolicies.Where(policy => policy.StoreId == storeId).ToList();
            return StoreAdapter.Instance.ComposeDiscountPolicy(storeDiscountPolicies);
        }

        private Inventory LoadInventory(int StoreId)
        {
            if (testingmode)
            {
                return null;
            }
            List<DbInventoryItem> invItems = dbConn.InventoriesItmes.Where(item => item.StoreId == StoreId).ToList();
            if(invItems.Count == 0)
            {
                return new Inventory();
            }
            else
            {
                Dictionary<int, Tuple<Product, int>> inventoryProducts = new Dictionary<int, Tuple<Product, int>>();
                foreach (DbInventoryItem item in invItems)
                {
                    DbProduct dbProduct = dbConn.Products.Where(prod => prod.Id == item.ProductId).FirstOrDefault();
                    Product product = new Product(dbProduct.Id, dbProduct.StoreId, dbProduct.Details, dbProduct.Price, dbProduct.Name, dbProduct.Rank, dbProduct.Category, dbProduct.ImgUrl);
                    inventoryProducts.Add(item.ProductId, new Tuple<Product, int>(product, item.Amount));
                }
                return new Inventory(inventoryProducts);
            }
          
            
        }

        private List<string> LoadStoreOwners(int StoreId)
        {
 
            List<StoreOwner> owners = dbConn.StoreOwners.Where(owner => owner.StoreId == StoreId).ToList();
            if(owners.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                List<string> userOwners = new List<string>();
                foreach (StoreOwner owner in owners)
                {
                    userOwners.Add(owner.OwnerName);
                }
                return userOwners;
            }
        }

        private List<string> LoadStoreManagers(int StoreId)
        {
         
            List<StoreManager> managers = dbConn.StoreManagers.Where(manager => manager.StoreId == StoreId).ToList();
            if(managers.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                List<string> userManagers = new List<string>();
                foreach (StoreManager manager in managers)
                {
                    userManagers.Add(manager.ManagerName);
                }
                return userManagers;
            }
     
        }

        //public PreCondition GetPreCondition(int PreConditionId)
        //{
        //    DbPreCondition pre = dbConn.PreConditions.Where(precon => precon.Id == PreConditionId).FirstOrDefault();
        //    if(pre.PreConditionType == CommonStr.PreConditionType.DiscountPreCondition)
        //    {
        //        return new DiscountPreCondition(pre.PreConditionNum);
        //    }
        //    else
        //    {
        //        return new PurchasePreCondition(pre.PreConditionNum);
        //    }
        //}

        private PurchasePolicy LoadPurchasePolicy(int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            List<DbPurchasePolicy> storePurchasePolicies = dbConn.PurchasePolicies.Where(policy => policy.StoreId == storeId).ToList();
            return StoreAdapter.Instance.ComposePurchasePolicy(storePurchasePolicies);
        }
        public void InsertUserNotification(DbNotifyData notification,bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetDbNotification(notification.UserName, notification.Context) == null)
            {
                dbConn.Notifies.Add(notification);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public void UpdateUserLogInStatus(string user,bool status, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            DbUser usr = dbConn.Users.Where(u => u.Name == user).SingleOrDefault();
            if(usr != null)
            {
                usr.IsLoggedIn = status;
                if(savechanges)
                {
                    dbConn.SaveChanges();
                }

            }

        }

        public void UpdateUserAdminStatus(string user, bool status, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            DbUser usr = dbConn.Users.Where(u => u.Name == user).SingleOrDefault();
            if (usr != null)
            {
                usr.IsAdmin = status;
                if (savechanges)
                {
                    dbConn.SaveChanges();
                }

            }
        }
        public StoreOwnertshipApprovalStatus getApprovalStat(string cand, int storeID)
        {
            if (testingmode)
            {
                return null;
            }
            StoreOwnertshipApprovalStatus s = dbConn.StoreOwnertshipApprovalStatuses.Where(u => u.CandidateName == cand && u.StoreId == storeID).SingleOrDefault();
            return s;
        }
        public void UpdateApprovalStatus(StoreOwnertshipApprovalStatus aps, bool status, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            StoreOwnertshipApprovalStatus s = dbConn.StoreOwnertshipApprovalStatuses.Where(u => u.CandidateName == aps.CandidateName && u.StoreId == aps.StoreId).SingleOrDefault();
            if (s != null)
            {
                s.Status = status;
                if(savechanges)
                {
                    dbConn.SaveChanges();
                }
            }

        }
        public  DbDiscountPolicy getParentDiscount(DbDiscountPolicy dbDiscountPolicy)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.DiscountPolicies.Where(policy => policy.Id == dbDiscountPolicy.ParentId).FirstOrDefault();
        }

        public DbPurchasePolicy getParentPurchasePolicy(DbPurchasePolicy dbPurchasePolicy)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.PurchasePolicies.Where(policy => policy.Id == dbPurchasePolicy.ParentId).FirstOrDefault();
        }

        //Store Componenet Delete Functions


        public void DeleteProduct(DbProduct dbProd, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.Products.Remove(dbProd);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void DeleteInventoryItem(DbInventoryItem invItem, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.InventoriesItmes.Remove(invItem);
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }


        public void DeletePurchasePolicy(DbPurchasePolicy purchasePolicy)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.PurchasePolicies.Remove(purchasePolicy);
            dbConn.SaveChanges();
        }

        public void DeleteDiscountPolicy(DbDiscountPolicy discountPolicy)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.DiscountPolicies.Remove(discountPolicy);
            dbConn.SaveChanges();
        }

        public void DeleteStoreManager(StoreManager manager, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(manager is null)
            {
                return;
            }
            dbConn.StoreManagers.Remove(manager);
            if (saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void DeleteStoreOwner(StoreOwner owner, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreOwners.Remove(owner);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }


        public void DeleteStoreOwnerShipAppoint(StoreOwnershipAppoint soaItem, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreOwnershipAppoints.Remove(soaItem);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void DeleteStoreManagerAppoint(StoreManagersAppoint soaItem, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.StoreManagersAppoints.Remove(soaItem);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }


        public void DeletePurchaseBasket(DbPurchaseBasket purchasebasket, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.Baskets.Remove(purchasebasket);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void DeletePrdocutAtBasket(ProductAtBasket productab, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.ProductsAtBaskets.Remove(productab);
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void DeleteUserStorePermission(UserStorePermissions permission, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.UserStorePermissions.Remove(permission);
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }


        public void DeleteFullStoreTransaction(Store store)
        {
            if (testingmode)
            {
                return;
            }
            DeleteAllStoreDiscountPolicy(store, false);
            DeleteAllStorePurchasePolicy(store, false);
            DeleteAllStoreInventoryItems(store, false);
            DeleteAllStoreProductAtBasket(store, false);
            DeleteAllStoreOwners(store, false);
            DeleteAllStoreManagers(store, false);
            DeleteAllStoreOwnersAppoint(store, false);
            DeleteAllStoreManagersAppoint(store, false);
            DeleteAllStorePurchaseBasket(store, false);
            DeleteAllUserStorePermissions(store, false);
            DeleteAllStoresProducts(store, false);
            DeleteDbStore(store, false);
            dbConn.SaveChanges();
        }

        private void DeleteAllUserStorePermissions(Store store, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            List<UserStorePermissions> userStorePermissions = dbConn.UserStorePermissions.Where(p => p.StoreId == store.Id).ToList();
            foreach(UserStorePermissions permission in userStorePermissions)
            {
                DeleteUserStorePermission(permission, false);
            }
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

     

        private void DeleteAllStorePurchaseBasket(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<DbPurchaseBasket> lstBaskets = dbConn.Baskets.Where(basket => basket.StoreId == store.Id).ToList();
            foreach(DbPurchaseBasket purchasebasket in lstBaskets)
            {
                DeletePurchaseBasket(purchasebasket, false);
            }

            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }


        private void DeleteAllStoreProductAtBasket(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<ProductAtBasket> pabLst = dbConn.ProductsAtBaskets.Where(pab => pab.StoreId == store.Id).ToList();
            
            foreach(ProductAtBasket productab in pabLst)
            {
                DeletePrdocutAtBasket(productab, false);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

      

        private void DeleteAllStoreOwnersAppoint(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<StoreOwnershipAppoint> soaLst =  dbConn.StoreOwnershipAppoints.Where(soa => soa.StoreId == store.Id).ToList();
            foreach(StoreOwnershipAppoint soaItem in soaLst)
            {
                DeleteStoreOwnerShipAppoint(soaItem, false);
            }

            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
            
        }

        private void DeleteAllStoreManagersAppoint(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<StoreManagersAppoint> soaLst = dbConn.StoreManagersAppoints.Where(soa => soa.StoreId == store.Id).ToList();
            foreach (StoreManagersAppoint soaItem in soaLst)
            {
                DeleteStoreManagerAppoint(soaItem, false);
            }

            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }


        private void DeleteDbStore(Store store, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            DbStore delStore = dbConn.Stores.Where(s => s.Id == store.Id).FirstOrDefault();
            if (delStore != null)
            {
                dbConn.Stores.Remove(delStore);
            }
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        private void DeleteAllStoreManagers(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            foreach (string name in store.managers)
            {
                StoreManager manager = dbConn.StoreManagers.Where(storeman => storeman.ManagerName == name).FirstOrDefault();
                if(manager != null)
                {
                    DeleteStoreManager(manager, false);
                }
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        private void DeleteAllStoreOwners(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            foreach (string name in store.owners)
            {
                StoreOwner owner = dbConn.StoreOwners.Where(o => o.OwnerName == name).FirstOrDefault();
                if (owner != null)
                {
                    DeleteStoreOwner(owner, false);
                }
            }

            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }




        private void DeleteAllStoresProducts(Store store, bool saveCahnges)
        {
            if (testingmode)
            {
                return;
            }
            List<DbProduct> prods = dbConn.Products.Where(p => p.StoreId == store.Id).ToList();
            foreach (DbProduct product in prods)
            {
                DeleteProduct(product, false);
            }
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }

        }

        private void DeleteAllStoreInventoryItems(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<DbInventoryItem> invItems = dbConn.InventoriesItmes.Where(item => item.StoreId == store.Id).ToList();
            foreach(DbInventoryItem invItem in invItems)
            {
                DeleteInventoryItem(invItem, false);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }



        private void DeleteAllStorePurchasePolicy(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<DbPurchasePolicy> storePolicies = dbConn.PurchasePolicies.Where(policy => policy.StoreId == store.Id).ToList();
            foreach (DbPurchasePolicy purchasePolicy in storePolicies)
            {
                DeletePurchasePolicy(purchasePolicy);
            }
            if(saveChanges)
            { 
                dbConn.SaveChanges();
            }
        }


        private void DeleteAllStoreDiscountPolicy(Store store, bool saveChanges)
        {
            if (testingmode)
            {
                return;
            }
            List<DbDiscountPolicy> storePolicies = dbConn.DiscountPolicies.Where(policy => policy.StoreId == store.Id).ToList();
            foreach(DbDiscountPolicy discountPolicy in storePolicies)
            {
                DeleteDiscountPolicy(discountPolicy);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

      



        // User Componnent Insert Functions
        public void InsertUser(DbUser user, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetUser(user.Name) == null)
            {
                dbConn.Users.Add(user);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertCandidateToOwnerShip(CandidateToOwnership candidate, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetCandidateToOwnership(candidate.CandidateName, candidate.AppointerName, candidate.StoreId) == null)
            {
                dbConn.CandidateToOwnerships.Add(candidate);
            }
            if (savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertNeedToApprove(NeedToApprove nta, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetNeedToApprove(nta.ApproverName, nta.CandiateName, nta.StoreId) == null)
            {
                dbConn.NeedToApproves.Add(nta);
            }
            if (savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertPassword(DbPassword password, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetdbPassword(password.UserName) == null)
            {
                dbConn.Passwords.Add(password);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        private DbPassword GetdbPassword(string userName)
        {
            return dbConn.Passwords.Where(s => s.UserName == userName).FirstOrDefault();

        }

        public void InsertStoreManagerAppoint(StoreManagersAppoint sma, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetSingleManagerAppoints(sma.AppointerName, sma.AppointedName, sma.StoreId) == null)
            {
                dbConn.StoreManagersAppoints.Add(sma);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertStoreOwnershipAppoint(StoreOwnershipAppoint soa, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(GetSingleOwnesAppoints(soa.AppointerName, soa.AppointedName, soa.StoreId) == null)
            {
                dbConn.StoreOwnershipAppoints.Add(soa);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertStoreOwnerShipApprovalStatus(StoreOwnertshipApprovalStatus soas, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            if(getApprovalStat(soas.CandidateName, soas.StoreId) == null)
            {
                dbConn.StoreOwnertshipApprovalStatuses.Add(soas);
            }
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertUserStorePermission(UserStorePermissions usp, bool saveCahnges=false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.UserStorePermissions.Add(usp);
            if(saveCahnges)
            {
                dbConn.SaveChanges();
            }
        }

        public void InsertUserStorePermissionSet(List<UserStorePermissions> usps, bool saveChanges = false)
        {
            if (testingmode)
            {
                return;
            }
            foreach (UserStorePermissions usp in usps)
            {
                InsertUserStorePermission(usp, saveChanges);
            }
            if(saveChanges)
            {
                dbConn.SaveChanges();
            }
        }



        public CandidateToOwnership GetCandidate(string userName, int storeId)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.CandidateToOwnerships.Where(candidate => candidate.CandidateName == userName && candidate.StoreId == storeId).FirstOrDefault();
        }

        public void DeleteCandidate(CandidateToOwnership candidateToOwnership, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            dbConn.CandidateToOwnerships.Remove(candidateToOwnership);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }



        public int GetNotifyWithMaxId()
        {
            if (testingmode)
            {
                return 100;
            }
            if (dbConn.Notifies.Any())
            {
                return dbConn.Notifies.Max(n => n.Id) + 1;
            }
            else
            {
                return 1;
            }
          
        }

        public void InsertUserUnreadMessages(DbNotifyData ntfd, bool savechanges = false)
        {
            if (testingmode)
            {
                return;
            }
            
            dbConn.Notifies.Add(ntfd);
            if(savechanges)
            {
                dbConn.SaveChanges();
            }
        }
        public Dictionary<int,LinkedList<string>> GetAllsubsribers()
        {
            if (testingmode)
            {
                return new Dictionary<int, LinkedList<string>>();
            }
            Dictionary<int, LinkedList<string>> subscruberList = new Dictionary<int, LinkedList<string>>();
            List<StoreOwnershipAppoint> owners = dbConn.StoreOwnershipAppoints.ToList();
            List<StoreManagersAppoint> managers = dbConn.StoreManagersAppoints.ToList();
            foreach(StoreOwnershipAppoint on in owners)
            {
                if(!subscruberList.ContainsKey(on.StoreId))
                {
                    subscruberList.Add(on.StoreId, new LinkedList<string>());
                    subscruberList[on.StoreId].AddFirst(on.AppointedName);
                }
                else
                {
                    if(!subscruberList[on.StoreId].Contains(on.AppointedName))
                    {
                        subscruberList[on.StoreId].AddFirst(on.AppointedName);
                    }
                }
            }
            foreach (StoreManagersAppoint on in managers)
            {
                if (!subscruberList.ContainsKey(on.StoreId))
                {
                    subscruberList.Add(on.StoreId, new LinkedList<string>());
                    subscruberList[on.StoreId].AddFirst(on.AppointedName);
                }
                else
                {
                    if (!subscruberList[on.StoreId].Contains(on.AppointedName))
                    {
                        subscruberList[on.StoreId].AddFirst(on.AppointedName);
                    }
                }
            }
            return subscruberList;
        }
        public StoreManagersAppoint GetManagerAppoint(StoreManagersAppoint s)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreManagersAppoints.Find(s);
        }
        public StoreOwnershipAppoint GetOwnerAppoint(StoreOwnershipAppoint s)
        {
            if (testingmode)
            {
                return null;
            }
            return dbConn.StoreOwnershipAppoints.Find(s);
        }

        public int GetnextCartId()
        {
            if (testingmode)
            {
                return 100;
            }
            if (dbConn.Carts.Any())
            {
                return dbConn.Carts.Max(c => c.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
    }
}
