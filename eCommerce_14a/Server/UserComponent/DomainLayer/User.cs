using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using eCommerce_14a.UserComponent;
using Server.UserComponent.Communication;

namespace eCommerce_14a.UserComponent.DomainLayer

{

    public class User
    {
        private string name;
        private int id;
        private bool isGuest;
        private bool isAdmin, isLoggedIn;
        private Dictionary<int, Store> Store_Ownership;
        private LinkedList<NotifyData> unreadMessages;
        private Dictionary<int, Store> Store_Managment;
        private Dictionary<int, int[]> Store_options;
        //Contains the list of who appointed you to which store! not who you appointed to which store!
        private Dictionary<int, User> AppointedBy;
        //Version 3 Use case - 4.3 Addings.
        private Dictionary<int, User> MasterAppointer;
        //Contains the list of who need to Approve his Ownership
        private Dictionary<int, List<string>> NeedToApprove;
        //Contains the list of who need to Approve his Ownership
        private Dictionary<int, List<string>> WaitingForApproval;
        //Contains the status of the Appoitment
        private Dictionary<int, bool> IsApproved;


        public User(int id, string name, bool isGuest = true, bool isAdmin = false)
        {
            this.id = id;
            this.name = name;
            this.isGuest = isGuest;
            this.isAdmin = isAdmin;
            this.isLoggedIn = false;
            Store_Ownership = new Dictionary<int, Store>();
            Store_Managment = new Dictionary<int, Store>();
            AppointedBy = new Dictionary<int, User>();
            Store_options = new Dictionary<int, int[]>();
            //Version 3 Addings use casr 4.3
            this.MasterAppointer = new Dictionary<int, User>();
            this.NeedToApprove = new Dictionary<int, List<string>>();
            this.WaitingForApproval = new Dictionary<int, List<string>>();
            unreadMessages = new LinkedList<NotifyData>();
            this.IsApproved = new Dictionary<int, bool>();
        }
        //Get Function that Added
        public List<string> getAllThatNeedToApprove(int storeID)
        {
            List<string> users;
            if(!NeedToApprove.TryGetValue(storeID,out users))
            {
                users = new List<string>();
            }
            return users;
        }
        public bool AppointerMasterAppointer(int storeID)
        {
            User masterA;
            if (!MasterAppointer.TryGetValue(storeID, out masterA))
            {
                return false;
            }
            MasterAppointer.Remove(storeID);
            AppointedBy.Add(storeID, masterA);
            return true;
        }
        public bool RemoveMasterAppointer(int storeID)
        {
            return MasterAppointer.Remove(storeID);
        }
        public Tuple<bool,string> SetMasterAppointer(int storeID,User masterA)
        {
            User master;
            if (MasterAppointer.TryGetValue(storeID, out master))
            {
                return new Tuple<bool, string>(false,"Already has a master Appointer to this storeID");
            }
            MasterAppointer.Add(storeID,masterA);
            return new Tuple<bool, string>(true,"Appointer Master Added");
        }
        public bool GetApprovalStatus(int storeID)
        {
            bool ans;
            if (!IsApproved.TryGetValue(storeID, out ans))
            {
                ans =  false;
            }
            return ans;
        }
        public Dictionary<int,List<string>> GetAllWaitingForApproval()
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
        public void SetApprovalStatus(int storeID,bool status)
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
        public void InsertOtherApprovalRequest(int storeID,List<string> user)
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
            if(needtoApprove.Count == 0)
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
            this.isLoggedIn = true;
        }

        public LinkedList<NotifyData> GetPendingMessages() 
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
            this.unreadMessages = new LinkedList<NotifyData>();
        }

        public void Logout()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.isLoggedIn = false;
        }
        public string getUserName()
        {
            return this.name;
        }
        public int getUserID()
        {
            return this.id;
        }
        public bool LoggedStatus()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.isLoggedIn;
        }
        public bool isguest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.isGuest;
        }

        public bool getUserPermission(int storeid,string permission)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if(permission is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return false;
            }
            int[] perms;
            if (!Store_options.TryGetValue(storeid, out perms))
                return false;
            if(permission.Equals(CommonStr.MangerPermission.Comments))
            {
                return perms[0] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.Puarchse))
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
        public Tuple<bool, string> addStoreOwnership(Store store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Owner\n");
            if (Store_Ownership.ContainsValue(store))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Ownership.Add(store.getStoreId(), store);
            return setPermmisions(store.getStoreId(), CommonStr.StorePermissions.FullPermissions);
        }
        //Version 2 changes
        public void AddMessage(NotifyData notification)
        {
            this.unreadMessages.AddLast(notification);
        }

        //Add a user to be store Manager
        public Tuple<bool, string> addStoreManagment(Store store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Manager\n");
            if (Store_Managment.ContainsValue(store))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Managment.Add(store.getStoreId(), store);
            return new Tuple<bool, string>(true, "");
        }
        //Checks if User user appointed this current user to be owner or manager to this store
        public bool isAppointedBy(User user, int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User owner;
            if (!AppointedBy.TryGetValue(store_id, out owner))
                return false;
            return owner == user;
        }
        //Add appointment from user to store
        //owner appointed current to be store manager\owner
        //If this apppointment exist do not add.
        public void addAppointment(User owner, int id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!isAppointedBy(owner, id))
                AppointedBy.Add(id, owner);
        }
        //Remove Manager
        public bool RemoveStoreManagment(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Managment.Remove(store_id);
        }
        //Remove Appoitment
        public bool RemoveAppoitment(User owner, int id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppointedBy.Remove(id);
        }
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
            return this.isAdmin;
        }
        //Set User permission over spesific store
        public Tuple<bool, string> setPermmisions(int store_id, int[] permission_set)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store_id < 1)
                return new Tuple<bool, string>(false, "No such Store id\n");
            if (permission_set == null)
                return new Tuple<bool, string>(false, "Null Argument\n");
            if (!isStorManager(store_id) && !isStoreOwner(store_id))
                return new Tuple<bool, string>(false, "The user is not Store Manager or owner\n");
            if (Store_options.ContainsKey(store_id))
                Store_options.Remove(store_id);
            Store_options.Add(store_id, permission_set);
            return new Tuple<bool, string>(true, "");
        }
        public bool RemovePermission(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_options.Remove(store_id);
        }

  
    }
}