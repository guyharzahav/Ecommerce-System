using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using log4net.Appender;
using Server.DAL;
using Server.DAL.PurchaseDb;
using Server.DAL.StoreDb;
using Server.DAL.UserDb;
using Server.UserComponent.Communication;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.DomainLayer
{
    public class AppoitmentManager
    {
        //All store appointer
        StoreManagment storeManagment;
        //Publisher publisher;
        UserManager UM;
        private List<Appoitment> AllAppoitments;
        private List<Candidation> AllCandidates;
        AppoitmentManager()
        {
            UM = UserManager.Instance;
            storeManagment = StoreManagment.Instance;
            AllAppoitments = new List<Appoitment>();
            AllCandidates = new List<Candidation>();
        }
        private static readonly object padlock = new object();  
        private static AppoitmentManager instance = null;  
        public static AppoitmentManager Instance  
        {  
            get  
            {  
                if (instance == null)  
                {  
                    lock (padlock)  
                    {  
                        if (instance == null)  
                        {  
                            instance = new AppoitmentManager();  
                        }  
                    }  
                }  
                return instance;  
            }  
        }
        public void insertCandidate(string appointer,string appointed,int storeId)
        {
            foreach(Candidation candidation in AllCandidates)
            {
                if(candidation.Appointer == appointer && candidation.Candidate == appointed && candidation.storeId == storeId)
                {
                    return;
                }
            }
            AllCandidates.Add(new Candidation(appointer, appointed, storeId));
        }
        public void insertAppointment(string appointer, string appointed, int storeId)
        {
            foreach (Appoitment appo in AllAppoitments)
            {
                if (appo.Appointer == appointer && appo.Appointed == appointed && appo.storeId == storeId)
                {
                    return;
                }
            }
            AllAppoitments.Add(new Appoitment(appointer, appointed, storeId));
        }
        public bool RemoveCnadidate(string appointer, string appointed, int storeId)
        {
            foreach (Candidation cand in AllCandidates)
            {
                if (cand.Appointer == appointer && cand.Candidate == appointed && cand.storeId == storeId)
                {
                    return AllCandidates.Remove(cand);
                }
            }
            return false;
        }
        public bool RemoveAppoitment(string appointer, string appointed, int storeId)
        {
            foreach (Appoitment appo in AllAppoitments)
            {
                if (appo.Appointer == appointer && appo.Appointed == appointed && appo.storeId == storeId)
                {
                    return AllAppoitments.Remove(appo);
                }
            }
            return false;
        }
        public void LoadAppointments()
        {
            AppointStoreManager("user4", "user1", 1);
            AppointStoreManager("user5", "user4", 2);
            AppointStoreManager("user5", "user3", 2);
            AppointStoreOwner("user4", "user2", 1);
            UM.Login("user2", "Test2");
            AppointStoreOwner("user4", "user6", 1);
            ApproveAppoitment("user2", "user6", 1, true);
            AppointStoreOwner("user5", "user1", 2);
            int[] perms = { 1, 1, 1, 1, 1 };
            ChangePermissions("user5", "user3", 2, perms);
        }
        public Tuple<bool, string> ApproveAppoitment(string owner, string Appointed, int storeID, bool approval)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (owner == null || Appointed == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (owner == "" || Appointed == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            Store store = storeManagment.getStore(storeID);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetUser(Appointed);
            if (appointer is null || appointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (appointer.isguest() || appointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            //Remove this approvalRequest
            if(appointer.INeedToApproveRemove(storeID, Appointed))
            {
                //Remove The Pending for the user
                if (appointed.RemoveOtherApprovalRequest(storeID, owner))
                {
                    //Remove Need to Approve From DB  
                    try
                    {
                        NeedToApprove ndap = DbManager.Instance.GetNeedToApprove(owner, Appointed, storeID);
                        DbManager.Instance.DeleteSingleApproval(ndap);
                    }
                    catch(Exception ex)
                    {
                        Logger.logError("DeleteSingleApproval error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, "Delete Operation from DB Failed cannot proceed");
                    }
                }
            }
            //Set to false if False and the operation will fail.
            if(!approval)
            {
                RemoveCnadidate(owner, Appointed, storeID);
                appointed.SetApprovalStatus(storeID, approval);
                //Update The Approval Status in the DB
                //Remove MasterAppointer - Candidtae Table from DB
                string masterNmae = appointed.MasterAppointer[storeID];
                appointed.RemoveMasterAppointer(storeID);
                try
                {
                    StoreOwnertshipApprovalStatus status = DbManager.Instance.getApprovalStat(Appointed, storeID);
                    CandidateToOwnership cand = DbManager.Instance.GetCandidateToOwnership(Appointed, masterNmae, storeID);
                    Publisher.Instance.Notify(Appointed, new NotifyData("Your request to be an Owner to Store - " + storeID + " Didn't Approved"));
                    DbManager.Instance.DeApprovalTransaction(status,approval,cand,true);
                    //DbManager.Instance.DeleteSingleCandidate(cand);
                    //DbManager.Instance.UpdateApprovalStatus(status, approval);
                }
                catch (Exception ex)
                {
                    Logger.logError("De-Approval db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "De-Approval Operation from DB Failed cannot proceed");
                }
                return new Tuple<bool, string>(true, "User failed to become an owner");
            }
            if(appointed.CheckSApprovalStatus(storeID))
            {
                RemoveCnadidate(owner, Appointed, storeID);
                //User can be assigned to Store owner
                appointed.RemoveApprovalStatus(storeID);
                string Mappointer = appointed.MasterAppointer[storeID];
                //Add Store Ownership in store Liav is incharge of this
                if (!appointed.addStoreOwnership(storeID, Mappointer).Item1)
                {
                    StoreOwner so = DbManager.Instance.getStoreOwnerbyStore(appointed.getUserName(), store.Id);
                    DbManager.Instance.DeleteStoreOwner(so,true);
                    return new Tuple<bool, string>(false, "Failed to insert store owner to DB memory");
                }
                appointed.AppointerMasterAppointer(storeID);
                if (!store.AddStoreOwner(appointed))
                {
                    return new Tuple<bool, string>(false, "Failed to insert store owner to DB memory");
                }
                insertAppointment(owner, Appointed, storeID);
                if (store.IsStoreManager(appointed))
                {
                    try
                    {
                        StoreManagersAppoint Sma = DbManager.Instance.GetSingleManagerAppoints(appointed.Store_Managment[storeID], appointed.Name, storeID);
                        DbManager.Instance.DeleteSingleManager(Sma);
                    }
                    catch (Exception ex)
                    {
                        Logger.logError("Remove Store Manager db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, "Could not Remove Manager store from  DB");
                    }
                    store.RemoveManager(appointed);
                    appointed.RemoveStoreManagment(storeID);
                }
                Publisher.Instance.Notify(Appointed, new NotifyData("Your request to be an Owner to Store - " + storeID + " is Approved"));
                Tuple<bool, string> ans = Publisher.Instance.subscribe(Appointed, storeID);
                try
                {
                    CandidateToOwnership cand = DbManager.Instance.GetCandidateToOwnership(Appointed, Mappointer, storeID);
                    DbManager.Instance.DeleteSingleCandidate(cand);
                    //Delete Approval Status from DB
                    StoreOwnertshipApprovalStatus status = DbManager.Instance.getApprovalStat(Appointed, storeID);
                    DbManager.Instance.DeleteSingleApprovalStatus(status);
                    DbManager.Instance.SaveChanges();
                }
                catch(Exception ex)
                {
                    Logger.logError("Inser Store Owner db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "Inser Store Owner Operation from DB Failed cannot proceed");
                }
                return ans;
            }
            DbManager.Instance.SaveChanges();
            return new Tuple<bool, string>(true, "User Still has some Work to do before he can become an Owner of this Store.");

        }
        //Owner appoints addto to be Store Owner.
        public Tuple<bool, string> AppointStoreOwner(string owner, string addto, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if(store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (owner == null || addto == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (owner == "" || addto == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetUser(addto);
            if (appointer is null || appointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (appointer.isguest() || appointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(appointed))
                return new Tuple<bool, string>(false, addto + " Is already Store Owner\n");
            if (!store.IsStoreOwner(appointer))
                return new Tuple<bool, string>(false, owner + "Is not a store Owner\n");

            appointed.SetMasterAppointer(storeId, appointer);
            List<string> owners = new List<string>();
            foreach(string ow in store.getOwners())
            {
                owners.Add(ow);
            }
            owners.Remove(owner);
            if(owners.Count == 0)
            {
                //Is ready to become Owner.
                insertAppointment(owner, addto, storeId);
                string maserAppointer = appointed.MasterAppointer[storeId];
                appointed.MasterAppointer.Remove(storeId);
                if(!appointed.addStoreOwnership(storeId, appointer.getUserName()).Item1)
                {
                    return new Tuple<bool, string>(false, "Could not add store Owner to DB");
                }
                if (!store.AddStoreOwner(appointed))
                {
                    return new Tuple<bool, string>(false, "Could not add store Owner to DB");
                }
                if(store.IsStoreManager(appointed))
                {
                    try
                    {
                        StoreManagersAppoint Sma = DbManager.Instance.GetSingleManagerAppoints(appointed.Store_Managment[storeId], appointed.Name, storeId);
                        DbManager.Instance.DeleteSingleManager(Sma);
                    }
                    catch(Exception ex)
                    {
                        Logger.logError("Remove Store Manager db error : " + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                        return new Tuple<bool, string>(false, "Could not Remove Manager store from  DB");
                    }
                    store.RemoveManager(appointed);
                    appointed.RemoveStoreManagment(storeId);
                }
                Publisher.Instance.Notify(addto, new NotifyData("Your request to be an Owner to Store - " + storeId + " is Approved"));
                Tuple<bool, string> ansSuccess = Publisher.Instance.subscribe(addto, storeId);
                DbManager.Instance.SaveChanges();
                return ansSuccess;
            }
            //Add Candidate TO ownership
            appointed.SetApprovalStatus(storeId, true);
            //No need to Inser Here Approvals we will Insert in the Inner Loop by the Owners.
            appointed.InsertOtherApprovalRequest(storeId, owners);
            foreach(string storeOwner in owners)
            {
                User tmpOwner = UM.GetUser(storeOwner);
                try
                {
                    DbManager.Instance.InsertNeedToApprove(AdapterUser.CreateNewApprovalNote(storeOwner, addto, storeId));
                }
                catch(Exception ex)
                {
                    appointed.RemoveApprovalStatus(storeId);
                    appointed.RemoveOtherApprovalList(storeId);
                    Logger.logError("Need to approve isnert Failed"+ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, "Insert Failed");
                }
            }
            foreach(string storeOwner in owners)
            {
                User tmpOwner = UM.GetUser(storeOwner);
                tmpOwner.INeedToApproveInsert(storeId, addto);
                Publisher.Instance.Notify(storeOwner, new NotifyData("User: " + addto + " Is want to Be store:" + storeId + " Owner Let him know what you think"));
            }
            //Add AptovmentStatus to DB
            insertCandidate(owner, addto, storeId);
            try
            {
                DbManager.Instance.InsertStoreOwnerShipApprovalStatus(AdapterUser.CreateNewStoreAppoitmentApprovalStatus(storeId, true, addto));
                DbManager.Instance.InsertCandidateToOwnerShip(AdapterUser.CreateCandidate(owner, addto, storeId), true);
            }
            catch (Exception ex)
            {
                Logger.logError("Ownership status to approve isnert Failed" + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                appointed.RemoveApprovalStatus(storeId);
                appointed.RemoveOtherApprovalList(storeId);
                foreach (string storeOwner in owners)
                {
                    User tmpOwner = UM.GetUser(storeOwner);
                    NeedToApprove nta = DbManager.Instance.GetNeedToApprove(storeOwner, addto, storeId);
                    tmpOwner.INeedToApproveRemove(storeId, addto);
                }
                return new Tuple<bool, string>(false, "Ownership status Failed");
            }
            return new Tuple<bool, string>(true, "Waiting For Approal By store Owners");
        }

        public void cleanup()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            storeManagment = StoreManagment.Instance;
            UM = UserManager.Instance;
            //publisher = Publisher.Instance;
        }

        //Owner appoints addto to be Store Manager.
        //Set his permissions to the store to be [1,1,0] only read and view
        public Tuple<bool, string> AppointStoreManager(string owner, string addto, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (owner == null || addto == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }
                
            if (owner == "" || addto == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            } 
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetUser(addto);
            if (appointer is null || appointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (appointer.isguest() || appointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(appointed) || store.IsStoreManager(appointed))
                return new Tuple<bool, string>(false, addto + " Is already Store Owner or Manager\n");
            if (!store.IsStoreOwner(appointer) && !store.IsStoreManager(appointer))
                return new Tuple<bool, string>(false, owner + "Is not a store Owner or Manager\n");
            //appointed.addManagerAppointment(appointer, store.GetStoreId());
            Tuple<bool, string> res = appointed.addStoreManagment(store,owner);
            if(!res.Item1)
            {
                return new Tuple<bool, string>(false, "Insert to store Managmnet Failed to DB");
            }
            if(!store.AddStoreManager(appointed))
            {
                return new Tuple<bool, string>(false, "Insert to store Managmnet Failed to DB");
            }
            //Insert StoreManager Appoint Into DB
            int[] p = {1, 1, 0, 0, 0};
            appointed.setPermmisions(store.GetStoreId(), p);
            try
            {
                DbManager.Instance.InsertStoreManagerAppoint(AdapterUser.CreateNewManagerAppoitment(owner, addto, storeId),true);
            }
            catch(Exception ex)
            {
                Logger.logError("Managment status to approve isnert Failed" + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
            }
            //Version 2 Addition
            Tuple<bool, string> ans = Publisher.Instance.subscribe(addto, storeId);
            if (!ans.Item1)
                return ans;
            return res;
        }
        //Remove appoitment only if owner gave the permissions to the Appointed user
        public Tuple<bool, string> RemoveAppStoreManager(string appointer, string appointed, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (appointer == null || appointed == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (appointer == "" || appointed == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            User userAppointer = UM.GetUser(appointer);
            User userAppointed = UM.GetUser(appointed);
            if (userAppointer is null || userAppointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (userAppointer.isguest() || userAppointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (!userAppointed.isAppointedByManager(userAppointer, store.GetStoreId()) && !(userAppointed.isAppointedByOwner(appointer, store.GetStoreId())))
                return new Tuple<bool, string>(false, appointed + "Is not appointed by " + appointer + "to be store manager\n");
            List<User> ManagersToRemove = RemoveManagerLoop(userAppointer, userAppointed, store);
            foreach (User manager in ManagersToRemove)
            {
                //Liav Will Delete From DB Here
                store.RemoveManager(manager);
            }
            DbManager.Instance.SaveChanges();
            return new Tuple<bool, string>(true, appointed + " Removed from store: - " + store.StoreName + "\n");
        }

        //Remove Store Owner New Function Vers.3
        public Tuple<bool,string> RemoveStoreOwner(string owner, string PrevOwner, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (owner == null || PrevOwner == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (owner == "" || PrevOwner == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            User appointer = UM.GetAtiveUser(owner);
            User DemotedOwner = UM.GetUser(PrevOwner);
            if (appointer is null || DemotedOwner is null)
                return new Tuple<bool, string>(false, "One of the users is not Exist\n");
            if (appointer.isguest() || DemotedOwner.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (!store.IsStoreOwner(appointer))
                return new Tuple<bool, string>(false, owner + " Is Not a Store Owner\n");
            if (!store.IsStoreOwner(DemotedOwner))
                return new Tuple<bool, string>(false, PrevOwner + " Is Not a Store Owner\n");
            if (!DemotedOwner.isAppointedByOwner(owner,storeId))
                return new Tuple<bool, string>(false, PrevOwner + " Did Not Appointed by:" +owner+"\n");
            List<User> OwnersToRemove = RemoveOwnerLoop(appointer, DemotedOwner, store);
            foreach (User Prevowner in OwnersToRemove)
            {
                //Liav Remove from DB Here
                store.RemoveOwner(Prevowner);
            }
            DbManager.Instance.SaveChanges();
            return new Tuple<bool, string>(true, PrevOwner + " Removed from store: - " + store.StoreName + "\n");
        }
        public List<User> RemoveManagerLoop(User appointer, User DemoteOwner, Store store)
        {
            List<User> ManagersToRemove = new List<User>();
            RemoveAppoitment(appointer.Name, DemoteOwner.Name, store.Id);
            ManagersToRemove.Add(DemoteOwner);
            string Message = "You have been Removed From Manager position in the Store " + store.StoreName + " Due to the fact that you appointer " + DemoteOwner.getUserName() + "Was fired now\n";
            DemoteOwner.RemoveStoreManagment(store.GetStoreId());
            StoreManagersAppoint stap = DbManager.Instance.GetSingleManagerAppoints(appointer.getUserName(), DemoteOwner.getUserName(), store.Id);
            DbManager.Instance.DeleteSingleManager(stap);
            int[] pm = DemoteOwner.Store_options[store.Id];
            DemoteOwner.RemovePermission(store.GetStoreId());
            //Remove Permissions From DB
            List<UserStorePermissions> perms = DbManager.Instance.GetUserStorePermissionSet(store.Id, DemoteOwner.getUserName());
            DbManager.Instance.DeletePermission(perms);
            Publisher.Instance.Notify(DemoteOwner.getUserName(), new NotifyData(Message));
            Publisher.Instance.Unsubscribe(DemoteOwner.getUserName(), store.GetStoreId());
            List<string> Managers = store.managers;
            foreach (string managerName in Managers)
            {
                User manager = UserManager.Instance.GetUser(managerName);
                if (manager.isAppointedByManager(DemoteOwner, store.GetStoreId()))
                {
                    ManagersToRemove.AddRange(RemoveManagerLoop(DemoteOwner, manager, store));
                }
            }
            return ManagersToRemove;
        }
        private List<User> RemoveOwnerLoop(User appointer,User DemoteOwner,Store store)
        {
            List<User> OwnersToRemove = new List<User>();
            OwnersToRemove.Add(DemoteOwner);
            RemoveAppoitment(appointer.Name, DemoteOwner.Name, store.Id);
            string OwnerRemovalMessage = "You have been Removed From Owner position in the Store " + store.StoreName +"By you appointer - "+appointer.getUserName()+"\n";
            DemoteOwner.RemoveStoreOwner(store.GetStoreId());
            //Remove Store Ownership from DB here
            StoreOwnershipAppoint s = DbManager.Instance.GetSingleOwnesAppoints(appointer.getUserName(), DemoteOwner.getUserName(), store.Id);
            DbManager.Instance.DeleteSingleOwnership(s);
            int[] p = DemoteOwner.Store_options[store.Id];
            DemoteOwner.RemovePermission(store.GetStoreId());
            //Remove Owners Store Permissions from DB here
            List<UserStorePermissions> permissions = DbManager.Instance.GetUserStorePermissionSet(store.Id,DemoteOwner.getUserName());
            DbManager.Instance.DeletePermission(permissions);
            Publisher.Instance.Notify(DemoteOwner.getUserName(), new NotifyData(OwnerRemovalMessage));
            Publisher.Instance.Unsubscribe(DemoteOwner.getUserName(), store.GetStoreId());
            List<string> Owners = store.owners;
            List<string> Managers = new List<string>();
            foreach(string mg in store.managers)
            {
                Managers.Add(mg);
            }
            foreach (string ownername in Owners)
            {
                User owner = UserManager.Instance.GetUser(ownername);
                if(owner.isAppointedByOwner(DemoteOwner.getUserName(),store.GetStoreId()))
                {
                    OwnersToRemove.AddRange(RemoveOwnerLoop(DemoteOwner,owner, store));
                }
            }
            foreach(string m in Managers)
            {
                User manage = UserManager.Instance.GetUser(m);
                if (manage.isAppointedByManager(DemoteOwner, store.GetStoreId()))
                {
                    RemoveAppStoreManager(DemoteOwner.getUserName(), m, store.Id);
                }
            }
            return OwnersToRemove;
        }
        public Tuple<bool, string> ChangePermissions(string ownerS, string worker, int storeId, int[] permissions)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (ownerS == null || worker == null || permissions == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (ownerS == "" || worker == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            //What about chheck permission length.
            User owner = UM.GetAtiveUser(ownerS);
            User manager = UM.GetUser(worker);
            if (owner is null || manager is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (owner.isguest() || manager.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(manager))
                return new Tuple<bool, string>(false, worker + " Is already Store Owner\n");
            if (!store.IsStoreOwner(owner))
                return new Tuple<bool, string>(false, ownerS + "Is not a store Owner\n");
            if (!manager.isAppointedByManager(owner, store.GetStoreId()) && !(manager.isAppointedByOwner(ownerS, store.GetStoreId())))
                return new Tuple<bool, string>(false, worker + "Is not appointed by " + ownerS + "to be store manager\n");
            //Insert New Permissions TO DB will happen in SetPermissions func
            return manager.setPermmisions(store.GetStoreId(), permissions,true);
        }
        //Temp function for tests
        //Add user to logged in list and Remove user from logged in lists.
        public void addactive(User a)
        {
            UM.addtoactive(a);
        }
        public void Rtoactive(User u)
        {
            UM.Rtoactive(u);
        }
    }
}
