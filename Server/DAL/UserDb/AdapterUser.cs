using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.UserDb
{
    public class AdapterUser
    {
        public static void InsertUserToDB(User user)
        {
            DbUser dbuser = new DbUser(user.getUserName(), user.isguest(), user.isSystemAdmin(), user.LoggedStatus());
            List<CandidateToOwnership> userOwnershipRequests = ConvertAllMasterAppointers(user);
            List<NeedToApprove> UsersNeedToBeApprovedByuser = ConvertUsersThatINeedToApprove(user);
            List<NeedToApprove> UsersNeedToApproveTheUser = ConvertUsersThatTheyNeedToApprove(user);
            List<StoreManagersAppoint> StoresUserIsManaging = ConvertStoresUserManaging(user);
            List<StoreOwnershipAppoint> StoresUserIsOwner = ConvertStoresUserOwnes(user);
            List<StoreOwnertshipApprovalStatus> StoreOwnershipWatingForApproval = ConvertUsersOwnershipApprovalStatuses(user);
            List<UserStorePermissions> StorePermissionsSet = COnvertUsersPermissionsStores(user);
        }
        public static List<DbUser> GetAllUsersFlat()
        {
            return DbManager.Instance.getAllDBUsers();
        }
        public static DbUser ConvertDBUser(User user)
        {
            return new DbUser(user.getUserName(), user.isguest(), user.isSystemAdmin(), user.LoggedStatus());
        }
        public static List<CandidateToOwnership> ConvertAllMasterAppointers(User user)
        {
            List<CandidateToOwnership> userOwnershipRequests = new List<CandidateToOwnership>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.MasterAppointers))
            {
                Dictionary<int, string> AllMastersAppointers = user.MasterAppointer;
                foreach (int storeNum in AllMastersAppointers.Keys)
                {
                    string userName = AllMastersAppointers[storeNum];
                    CandidateToOwnership appoitment = new CandidateToOwnership(userName, user.getUserName(), storeNum);
                    userOwnershipRequests.Add(appoitment);

                }
            }
            return userOwnershipRequests;

        }
        public static List<NeedToApprove> ConvertUsersThatINeedToApprove(User user)
        {
            List<NeedToApprove> UsersNeedToBeApprovedByuser = new List<NeedToApprove>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.IsNeedToApprove))
            {
                Dictionary<int, List<string>> AllUsersWaitingForApproval = user.GetAllWaitingForApproval();
                foreach (int storeNum in AllUsersWaitingForApproval.Keys)
                {
                    List<string> users = AllUsersWaitingForApproval[storeNum];
                    foreach (string uname in users)
                    {
                        NeedToApprove appoitment = new NeedToApprove(user.getUserName(), uname, storeNum);
                        UsersNeedToBeApprovedByuser.Add(appoitment);

                    }
                }
            }
            return UsersNeedToBeApprovedByuser;
        }
        public static List<NeedToApprove> ConvertUsersThatTheyNeedToApprove(User user)
        {
            List<NeedToApprove> UsersNeedToApprovedThsUser = new List<NeedToApprove>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.TheyNeedApprove))
            {
                Dictionary<int, List<string>> AllUsersNeedToApprove = user.NeedToApprove;
                foreach (int storeNum in AllUsersNeedToApprove.Keys)
                {
                    List<string> users = AllUsersNeedToApprove[storeNum];
                    foreach (string uname in users)
                    {
                        NeedToApprove appoitment = new NeedToApprove(uname, user.getUserName(), storeNum);
                        UsersNeedToApprovedThsUser.Add(appoitment);

                    }
                }
            }
            return UsersNeedToApprovedThsUser;
        }
        public static List<StoreManagersAppoint> ConvertStoresUserManaging(User user)
        {
            List<StoreManagersAppoint> StoresUserIsManaging = new List<StoreManagersAppoint>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.ManageStores))
            {
                Dictionary<int, string> AllManagingStores = user.Store_Managment;
                //Dictionary<int, User> AllManagerAppointers = user.AppointedByManager;
                foreach (int storeNum in AllManagingStores.Keys)
                {
                    StoreManagersAppoint managingData = new StoreManagersAppoint(AllManagingStores[storeNum], user.getUserName(), storeNum);
                    StoresUserIsManaging.Add(managingData);
                }
            }
            return StoresUserIsManaging;
        }
        public static List<StoreOwnershipAppoint> ConvertStoresUserOwnes(User user)
        {
            List<StoreOwnershipAppoint> StoresUserIsOwner = new List<StoreOwnershipAppoint>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.OwnStores))
            {
                Dictionary<int, string> AllOwnerStores = user.Store_Ownership;
                //Dictionary<int, User> AllOwnerAppointers = user.AppointedByOwner;
                foreach (int storeNum in AllOwnerStores.Keys)
                {
                    StoreOwnershipAppoint Ownersdata = new StoreOwnershipAppoint(AllOwnerStores[storeNum], user.getUserName(), storeNum);
                    StoresUserIsOwner.Add(Ownersdata);
                }
            }
            return StoresUserIsOwner;
        }
        public static List<StoreOwnertshipApprovalStatus> ConvertUsersOwnershipApprovalStatuses(User user)
        {
            List<StoreOwnertshipApprovalStatus> StoreOwnershipWatingForApproval = new List<StoreOwnertshipApprovalStatus>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.StoreApprovalStatus))
            {
                Dictionary<int, bool> AllStoreOwnershipApplications = user.IsApproved;
                foreach (int storeNum in AllStoreOwnershipApplications.Keys)
                {
                    StoreOwnertshipApprovalStatus ApprovalStatus = new StoreOwnertshipApprovalStatus(storeNum, AllStoreOwnershipApplications[storeNum], user.getUserName());
                    StoreOwnershipWatingForApproval.Add(ApprovalStatus);
                }
            }
            return StoreOwnershipWatingForApproval;
        }
        public static List<UserStorePermissions> COnvertUsersPermissionsStores(User user)
        {
            List<UserStorePermissions> StorePermissionsSet = new List<UserStorePermissions>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.Permmisions))
            {
                Dictionary<int, int[]> StorePermisions = user.Store_options;
                foreach (int storeNum in StorePermisions.Keys)
                {
                    List<string> PermissionSet = user.getPermissionsStringSet(storeNum);
                    foreach (string perm in PermissionSet)
                    {
                        UserStorePermissions Permission = new UserStorePermissions(user.getUserName(), storeNum, perm);
                        StorePermissionsSet.Add(Permission);
                    }
                }
            }
            return StorePermissionsSet;
        }
        public static DbUser CreateDBUser(string userName, bool isGuest, bool isAdmin, bool logStatus)
        {
            return new DbUser(userName, isGuest, isAdmin, logStatus);
        }
        public static CandidateToOwnership CreateCandidate(string owner, string candidtaeName, int storeId)
        {
            return new CandidateToOwnership(owner, candidtaeName, storeId);
        }
        public static NeedToApprove CreateNewApprovalNote(string approvaerName, string needtobeapproved, int storeId)
        {
            return new NeedToApprove(approvaerName, needtobeapproved, storeId);
        }
        public static DbPassword CreateNewPasswordEntry(string userName, string sha5)
        {
            return new DbPassword(userName, sha5);
        }
        public static StoreManagersAppoint CreateNewManagerAppoitment(string appoiter, string appointed , int storeId)
        {
            return new StoreManagersAppoint(appoiter, appointed, storeId);
        }
        public static StoreOwnershipAppoint CreateNewOwnerAppoitment(string appointer, string appointed, int storeId)
        {
            return new StoreOwnershipAppoint(appointer, appointed, storeId);
        }
        public static StoreOwnertshipApprovalStatus CreateNewStoreAppoitmentApprovalStatus(int storeId, bool candidtaeStatus, string candidateName)
        {
            return new StoreOwnertshipApprovalStatus(storeId, candidtaeStatus, candidateName);
        }
        public static UserStorePermissions CreateNewPermission(string userName, int StoreId, string PermissionName)
        {
            return new UserStorePermissions(userName, StoreId, PermissionName);
        }
        public static List<UserStorePermissions> CreateNewPermissionSet(string userName, int StoreId, int[] permissionsNumbers)
        {
            List<UserStorePermissions> results = new List<UserStorePermissions>();
            if(permissionsNumbers[0] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Comments));
            }
            if (permissionsNumbers[1] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Purchase));
            }
            if (permissionsNumbers[2] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Product));
            }
            if (permissionsNumbers[3] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.PurachsePolicy));
            }
            if (permissionsNumbers[4] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.DiscountPolicy));
            }
            return results;
        }

        public static List<UserStorePermissions> GetPermissionSetForDelete(string userName, int StoreId, int[] permissionsNumbers)
        {
            List<UserStorePermissions> results = new List<UserStorePermissions>();
            if (permissionsNumbers[0] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Comments));
            }
            if (permissionsNumbers[1] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Purchase));
            }
            if (permissionsNumbers[2] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.Product));
            }
            if (permissionsNumbers[3] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.PurachsePolicy));
            }
            if (permissionsNumbers[4] == 1)
            {
                results.Add(new UserStorePermissions(userName, StoreId, CommonStr.MangerPermission.DiscountPolicy));
            }
            return results;
        }
        public static StoreManagersAppoint GetManagerNote(string appointer,string appointed,int storeID)
        {
            StoreManagersAppoint s = new StoreManagersAppoint(appointer, appointed, storeID);
            return DbManager.Instance.GetManagerAppoint(s);
        }
        public static StoreOwnershipAppoint GetOwnerNote(string appointer, string appointed, int storeID)
        {
            StoreOwnershipAppoint s = new StoreOwnershipAppoint(appointer, appointed, storeID);
            return DbManager.Instance.GetOwnerAppoint(s);
        }


    }
}
