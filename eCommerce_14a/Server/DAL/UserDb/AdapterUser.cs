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
        public void InsertUserToDB(User user)
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
        public DbUser ConvertDBUser(User user)
        {
            return new DbUser(user.getUserName(), user.isguest(), user.isSystemAdmin(), user.LoggedStatus());
        }
        public List<CandidateToOwnership> ConvertAllMasterAppointers(User user)
        {
            List<CandidateToOwnership> userOwnershipRequests = new List<CandidateToOwnership>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.MasterAppointers))
            {
                Dictionary<int, User> AllMastersAppointers = user.MasterAppointer;
                foreach (int storeNum in AllMastersAppointers.Keys)
                {
                    string userName = AllMastersAppointers[storeNum].getUserName();
                    CandidateToOwnership appoitment = new CandidateToOwnership(userName, user.getUserName(), storeNum);
                    userOwnershipRequests.Add(appoitment);

                }
            }
            return userOwnershipRequests;

        }
        public List<NeedToApprove> ConvertUsersThatINeedToApprove(User user)
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
        public List<NeedToApprove> ConvertUsersThatTheyNeedToApprove(User user)
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
        public List<StoreManagersAppoint> ConvertStoresUserManaging(User user)
        {
            List<StoreManagersAppoint> StoresUserIsManaging = new List<StoreManagersAppoint>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.ManageStores))
            {
                Dictionary<int, Store> AllManagingStores = user.Store_Managment;
                Dictionary<int, User> AllManagerAppointers = user.AppointedByManager;
                foreach (int storeNum in AllManagingStores.Keys)
                {
                    StoreManagersAppoint managingData = new StoreManagersAppoint(AllManagerAppointers[storeNum].getUserName(), user.getUserName(), storeNum);
                    StoresUserIsManaging.Add(managingData);
                }
            }
            return StoresUserIsManaging;
        }
        public List<StoreOwnershipAppoint> ConvertStoresUserOwnes(User user)
        {
            List<StoreOwnershipAppoint> StoresUserIsOwner = new List<StoreOwnershipAppoint>();
            if (user.IsListNotEmpty(CommonStr.UserListsOptions.OwnStores))
            {
                Dictionary<int, Store> AllOwnerStores = user.Store_Managment;
                Dictionary<int, User> AllOwnerAppointers = user.AppointedByOwner;
                foreach (int storeNum in AllOwnerStores.Keys)
                {
                    if (AllOwnerAppointers.ContainsKey(storeNum))
                    {
                        StoreOwnershipAppoint Ownersdata = new StoreOwnershipAppoint(AllOwnerAppointers[storeNum].getUserName(), user.getUserName(), storeNum);
                        StoresUserIsOwner.Add(Ownersdata);
                    }
                    else
                    {
                        StoreOwnershipAppoint Ownersdata = new StoreOwnershipAppoint(user.getUserName(), user.getUserName(), storeNum);
                        StoresUserIsOwner.Add(Ownersdata);
                    }
                }
            }
            return StoresUserIsOwner;
        }
        public List<StoreOwnertshipApprovalStatus> ConvertUsersOwnershipApprovalStatuses(User user)
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
        public List<UserStorePermissions> COnvertUsersPermissionsStores(User user)
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

    }
}
