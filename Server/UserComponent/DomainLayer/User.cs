using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.DAL.UserDb;
using Server.UserComponent.Communication;

namespace eCommerce_14a.UserComponent.DomainLayer

{

    public class User
    {
        public string Name { set; get; }
        public int Id { set; get; }
        public bool IsGuest { set; get; }
        public bool IsAdmin { set; get; }
        public bool IsLoggedIn { set; get; }



        public Dictionary<int, string> Store_Ownership { get; set; }
        public List<NotifyData> unreadMessages { set; get; }
        public Dictionary<int, string> Store_Managment { get; set; }
        public Dictionary<int, int[]> Store_options { get; set; }
        //Contains the list of who appointed you to which store! not who you appointed to which store!
        //public Dictionary<int, User> AppointedByOwner { get; }
        //public Dictionary<int, User> AppointedByManager { get; }
        //Version 3 Use case - 4.3 Addings.
        public Dictionary<int, string> MasterAppointer { get; set; }
        //Contains the list of who need to Approve his Ownership
        public Dictionary<int, List<string>> NeedToApprove { set;  get; }
        //Contains the list of who waiting for his approval
        public Dictionary<int, List<string>> WaitingForApproval { set;  get; } //the user I need to approve
        //Contains the status of the Appoitment
        public Dictionary<int, bool> IsApproved { get; set; }


        public User(int id, string name, bool isGuest = true, bool isAdmin = false)
        {
            Id = id;
            Name = name;
            IsGuest = isGuest;
            IsAdmin = isAdmin;
            IsLoggedIn = false;
            Store_Ownership = new Dictionary<int, string>();
            Store_Managment = new Dictionary<int, string>();
            //AppointedByOwner = new Dictionary<int, User>();
            //AppointedByManager = new Dictionary<int, User>();
            Store_options = new Dictionary<int, int[]>();
            //Version 3 Addings use casr 4.3
            MasterAppointer = new Dictionary<int, string>();
            NeedToApprove = new Dictionary<int, List<string>>();
            WaitingForApproval = new Dictionary<int, List<string>>();
            unreadMessages = new List<NotifyData>();
            IsApproved = new Dictionary<int, bool>();
        }
        //Get Function that Added
        public List<string> getAllThatNeedToApprove(int storeID)
        {
            List<string> users;
            if (!NeedToApprove.TryGetValue(storeID, out users))
            {
                users = new List<string>();
            }
            return users;
        }
        public bool IsListNotEmpty(string Op)
        {
            switch ((Op)
)
            {
                case "MasterAppointer":
                    return MasterAppointer.Count != 0;
                case "IsNeedToApproveSomeone":
                    return WaitingForApproval.Count != 0;
                case "IsOwnsStores":
                    return Store_Ownership.Count != 0;
                case "IsManagesStores":
                    return Store_Managment.Count != 0;
                case "StoreApprovalStatus":
                    return IsApproved.Count != 0;
                case "TheyNeedToApproveUser":
                    return NeedToApprove.Count != 0;
                case "PermissionSets":
                    return Store_options.Count != 0;
                default:
                    return false; ;
            }
        }
        public List<string> getPermissionsStringSet(int storeId)
        {
            List<string> output = new List<string>();
            int[] store_options = Store_options[storeId];
            if (store_options[0] == 1)
                output.Add(CommonStr.MangerPermission.Comments);
            if (store_options[1] == 1)
                output.Add(CommonStr.MangerPermission.Purchase);
            if (store_options[2] == 1)
                output.Add(CommonStr.MangerPermission.Product);
            if (store_options[3] == 1)
                output.Add(CommonStr.MangerPermission.PurachsePolicy);
            if (store_options[4] == 1)
                output.Add(CommonStr.MangerPermission.DiscountPolicy);
            return output;
        }
        public bool AppointerMasterAppointer(int storeID)
        {
            string masterA;
            if (!MasterAppointer.TryGetValue(storeID, out masterA))
            {
                return false;
            }
            MasterAppointer.Remove(storeID);
            if (Store_Ownership.ContainsKey(storeID))
            {
                return false;
            }
            Store_Ownership.Add(storeID, masterA);
            return true;
        }
        public bool RemoveMasterAppointer(int storeID)
        {
            return MasterAppointer.Remove(storeID);
        }
        public Tuple<bool, string> SetMasterAppointer(int storeID, User masterA)
        {
            string master;
            if (MasterAppointer.TryGetValue(storeID, out master))
            {
                return new Tuple<bool, string>(false, "Already has a master Appointer to this storeID");
            }
            MasterAppointer.Add(storeID, masterA.getUserName());
            return new Tuple<bool, string>(true, "Appointer Master Added");
        }
        public bool GetApprovalStatus(int storeID)
        {
            bool ans;
            if (!IsApproved.TryGetValue(storeID, out ans))
            {
                ans = false;
            }
            return ans;
        }
        public Dictionary<int, List<string>> GetAllWaitingForApproval()
        {
            return this.WaitingForApproval;
        }
        public List<string> GetAllWaitingForApproval(int storeID)
        {
            List<string> users;
            if (!WaitingForApproval.TryGetValue(storeID, out users))
            {
                users = new List<string>();
            }
            return users;
        }
        //IsApprovedStatuse - Approved to become Store Owner.
        public void SetApprovalStatus(int storeID, bool status)
        {
            bool ans;
            if (!IsApproved.TryGetValue(storeID, out ans))
            {
                IsApproved.Add(storeID, status);
            }
            IsApproved[storeID] = status;
        }
        public bool RemoveApprovalStatus(int storeID)
        {
            bool ans;
            if (!IsApproved.TryGetValue(storeID, out ans))
            {
                return false;
            }
            return IsApproved.Remove(storeID);
        }
        //Waiting for Approval List functions to become Store Owner
        public void InsertOtherApprovalRequest(int storeID, List<string> user)
        {
            List<string> users;
            if (NeedToApprove.TryGetValue(storeID, out users))
            {
                return;
            }
            NeedToApprove.Add(storeID, user);
        }
        public bool RemoveOtherApprovalRequest(int storeID, string user)
        {
            List<string> users;
            if (NeedToApprove.TryGetValue(storeID, out users))
            {
                return NeedToApprove[storeID].Remove(user);
            }
            return false;
        }
        public bool RemoveOtherApprovalList(int storeID)
        {
            List<string> users;
            if (NeedToApprove.TryGetValue(storeID, out users))
            {
                return NeedToApprove.Remove(storeID);
            }
            return false;
        }
        //Need to Approve other users as Current Store owner.
        public void INeedToApproveInsert(int storeID, string user)
        {
            List<string> users;
            if (WaitingForApproval.TryGetValue(storeID, out users))
            {
                WaitingForApproval[storeID].Add(user);
                return;
            }
            users = new List<string>();
            users.Add(user);
            WaitingForApproval.Add(storeID, users);
        }
        public bool INeedToApproveRemove(int storeID, string user)
        {
            List<string> users;
            if (WaitingForApproval.TryGetValue(storeID, out users))
            {
                return WaitingForApproval[storeID].Remove(user);
            }
            return false;
        }
        public bool INeedToApproveRemoveAllList(int storeID)
        {
            List<string> users;
            if (WaitingForApproval.TryGetValue(storeID, out users))
            {
                return WaitingForApproval.Remove(storeID);
            }
            return false;
        }
        public bool CheckSApprovalStatus(int storeId)
        {
            bool ans = GetApprovalStatus(storeId);
            List<string> needtoApprove = getAllThatNeedToApprove(storeId);
            if (needtoApprove.Count == 0)
            {
                NeedToApprove[storeId] = new List<string>();
                return ans;
            }
            return false;
        }
        //End of Adding to Use-case 4.3 version 3 addings.
        public Dictionary<int, int[]> GetUserPermissions()
        {
            return Store_options;
        }
        public void LogIn()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.IsLoggedIn = true;
        }

        public List<NotifyData> GetPendingMessages()
        {
            return this.unreadMessages;
        }

        public bool HasPendingMessages()
        {
            return this.unreadMessages.Count != 0;
        }

        public void RemovePendingMessage(NotifyData msg)
        {
            this.unreadMessages.Remove(msg);
        }
        public void RemoveAllPendingMessages()
        {
            this.unreadMessages = new List<NotifyData>();
        }

        public void Logout()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.IsLoggedIn = false;
        }
        public string getUserName()
        {
            return this.Name;
        }
        public int getUserID()
        {
            return this.Id;
        }
        public bool LoggedStatus()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.IsLoggedIn;
        }
        public bool isguest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.IsGuest;
        }

        public bool getUserPermission(int storeid, string permission)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (permission is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return false;
            }
            int[] perms;
            if (!Store_options.TryGetValue(storeid, out perms))
                return false;
            if (permission.Equals(CommonStr.MangerPermission.Comments))
            {
                return perms[0] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.Purchase))
            {
                return perms[1] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.Product))
            {
                return perms[2] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.DiscountPolicy))
            {
                return perms[3] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.PurachsePolicy))
            {
                return perms[4] == 1;
            }
            return false;
        }

        //This user will be store Owner 
        public Tuple<bool, string> addStoreOwnership(int storeId, string appointer, bool saveCahnges=false)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
            {
                return new Tuple<bool, string>(false, "Guest user cannot be store Owner\n");

            }
            if (Store_Ownership.ContainsKey(storeId))
            {
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            }
            Store_Ownership.Add(storeId, appointer);
            
            //DB Insert//
            StoreOwnershipAppoint soa = new StoreOwnershipAppoint(appointer, this.Name, storeId);
            try
            {
                DbManager.Instance.InsertStoreOwnershipAppoint(soa, saveCahnges);

            }
            catch(Exception ex)
            {
                Logger.logError("Add Store Owner db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Insert Store Owner Operation to DB Failed cannot proceed");
            }
            Tuple<bool,string> ans =  setPermmisions(storeId, CommonStr.StorePermissions.FullPermissions, saveCahnges);
            if(!ans.Item1)
            {
                DbManager.Instance.DeleteStoreOwnerShipAppoint(soa);
                return new Tuple<bool, string>(false, "Insert Store Permissions Operation to DB Failed cannot proceed");
            }
            return new Tuple<bool, string>(true, "Insert Permissions to DB is OK");
        }
        //Version 2 changes
        public void AddMessage(NotifyData notification)
        {
            this.unreadMessages.Add(notification);
        }

        //Add a user to be store Manager
        public Tuple<bool, string> addStoreManagment(Store store, string appointer)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Manager\n");
            if (Store_Managment.ContainsKey(store.Id))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Managment.Add(store.GetStoreId(), appointer);
            return new Tuple<bool, string>(true, "");
        }
        //Checks if User user appointed this current user to be owner or manager to this store
        public bool isAppointedByOwner(string user, int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            string owner;
            if (!Store_Ownership.TryGetValue(store_id, out owner))
                return false;
            return owner.Equals(user);
        }
        public bool isAppointedByManager(User user, int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            string owner;
            if (!Store_Managment.TryGetValue(store_id, out owner))
                return false;
            return owner.Equals(user.getUserName());
        }
        //Add appointment from user to store
        //owner appointed current to be store manager\owner
        //If this apppointment exist do not add.
        //public void addManagerAppointment(User owner, int id)
        //{
        //    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
        //    if (!isAppointedByManager(owner, id))
        //        AppointedByManager.Add(id, owner);
        //}
        //public void addOwnerAppointment(User owner, int id)
        //{
        //    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
        //    if (!isAppointedByOwner(owner., id))
        //        AppointedByOwner.Add(id, owner);
        //}
        //Remove Manager
        public bool RemoveStoreManagment(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Managment.Remove(store_id);
        }
        public bool RemoveStoreOwner(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Ownership.Remove(store_id);
        }
        //Remove Appoitment
        //public bool RemoveAppoitmentOwner(User owner, int id)
        //{
        //    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
        //    return AppointedByOwner.Remove(id);
        //}
        //public bool RemoveAppoitmentManager(User owner, int id)
        //{
        //    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
        //    return AppointedByManager.Remove(id);
        //}
        //Check if the user is Currently Store Owner
        public bool isStoreOwner(int store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Ownership.ContainsKey(store);
        }
        //Check if the user is Currently Store Manager
        public bool isStorManager(int store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Managment.ContainsKey(store);
        }
        public bool isSystemAdmin()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.IsAdmin;
        }
        //Set User permission over spesific store
        public Tuple<bool, string> setPermmisions(int store_id, int[] permission_set, bool saveChanges=false)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store_id < 1)
                return new Tuple<bool, string>(false, "No such Store id\n");
            if (permission_set == null)
                return new Tuple<bool, string>(false, "Null Argument\n");
            if (!isStorManager(store_id) && !isStoreOwner(store_id))
                return new Tuple<bool, string>(false, "The user is not Store Manager or owner\n");
            
            if (Store_options.ContainsKey(store_id))
            {
                int[] oldp = Store_options[store_id];

                List<UserStorePermissions> perms = DbManager.Instance.GetUserStorePermissionSet(store_id, this.Name);
                Store_options.Remove(store_id);
                try
                {
                    DbManager.Instance.DeletePermission(perms);
                }
                catch(Exception ex)
                {
                    Logger.logError("Delete Permissions error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "Delete Permissions DB Failed cannot proceed");
                }
                
            }
            Store_options.Add(store_id, permission_set);
            List<UserStorePermissions> permsN = AdapterUser.CreateNewPermissionSet(Name, store_id, permission_set);
            try
            {
                DbManager.Instance.InsertUserStorePermissionSet(permsN, saveChanges);
            }
            catch (Exception ex)
            {
                Logger.logError("Insert Permissions error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Insert Permissions DB Failed cannot proceed");
            }
            return new Tuple<bool, string>(true, "");
        }
        public bool RemovePermission(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_options.Remove(store_id);
        }
    }


}