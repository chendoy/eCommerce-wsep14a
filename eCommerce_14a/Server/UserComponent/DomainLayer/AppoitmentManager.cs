using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.DAL.UserDb;
using Server.UserComponent.Communication;

namespace eCommerce_14a.UserComponent.DomainLayer
{
    public class AppoitmentManager
    {
        //All store appointer
        StoreManagment storeManagment;
        //Publisher publisher;
        UserManager UM;
        AppoitmentManager()
        {
            UM = UserManager.Instance;
            storeManagment = StoreManagment.Instance;
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
                    DbManager.Instance.DeleteSingleApproval(AdapterUser.CreateNewApprovalNote(owner, Appointed, storeID));
                }
            }
            //Set to false if False and the operation will fail.
            if(!approval)
            {
                appointed.SetApprovalStatus(storeID, approval);
                //Update The Approval Status in the DB
                StoreOwnertshipApprovalStatus status = DbManager.Instance.getApprovalStat(Appointed, storeID);
                DbManager.Instance.UpdateApprovalStatus(status, approval);
                //Remove MasterAppointer - Candidtae Table from DB
                string masterNmae = appointed.MasterAppointer[storeID];
                appointed.RemoveMasterAppointer(storeID);
                DbManager.Instance.DeleteSingleCandidate(AdapterUser.CreateCandidate(masterNmae, Appointed, storeID));
                return new Tuple<bool, string>(true, "User failed to become an owner");
            }
            if(appointed.CheckSApprovalStatus(storeID))
            {
                //User can be assigned to Store owner
                Store store = storeManagment.getStore(storeID);
                if (store is null)
                {
                    return new Tuple<bool, string>(false, "Store Does not Exist");
                }
                appointed.RemoveApprovalStatus(storeID);
                //Delete Approval Status from DB
                StoreOwnertshipApprovalStatus status = DbManager.Instance.getApprovalStat(Appointed, storeID);
                DbManager.Instance.DeleteSingleApprovalStatus(status);
                //Add Store Ownership in store Liav is incharge of this
                store.AddStoreOwner(appointed);
                string Mappointer = appointed.MasterAppointer[storeID];
                //Remove Candidation From DB
                CandidateToOwnership cand = AdapterUser.CreateCandidate(Mappointer, Appointed, storeID);
                DbManager.Instance.DeleteSingleCandidate(cand);
                appointed.AppointerMasterAppointer(storeID);
                appointed.addStoreOwnership(storeID, Mappointer);
                //Add Store Ownership to DB
                StoreOwnershipAppoint appoitment = AdapterUser.CreateNewOwnerAppoitment(Mappointer, Appointed, storeID);
                DbManager.Instance.InsertStoreOwnershipAppoint(appoitment);
                int[] p = { 1, 1, 1, 1, 1 };
                appointed.setPermmisions(store.GetStoreId(), p);
                //Add StorePermissions to DB
                List<UserStorePermissions> permissions = AdapterUser.CreateNewPermissionSet(Appointed, storeID, p);
                DbManager.Instance.InsertUserStorePermissionSet(permissions);
                //Adapter Will Insert EveryThing Needed
                Publisher.Instance.Notify(Appointed, new NotifyData("Your request to be an Owner to Store - " + storeID + " is Approved"));
                Tuple<bool, string> ans = Publisher.Instance.subscribe(Appointed, storeID);
                return ans;
            }
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
            List<string> owners = store.getOwners();
            owners.Remove(owner);
            if(owners.Count == 0)
            {
                //Is ready to become Owner.
                store.AddStoreOwner(appointed);
                appointed.AppointerMasterAppointer(storeId);
                //Remove Candidtation from DataBase
                CandidateToOwnership cand = AdapterUser.CreateCandidate(owner, addto, storeId);
                DbManager.Instance.DeleteSingleCandidate(cand);
                appointed.addStoreOwnership(storeId, appointer.getUserName());
                StoreOwnershipAppoint appoitment = AdapterUser.CreateNewOwnerAppoitment(owner, addto, storeId);
                DbManager.Instance.InsertStoreOwnershipAppoint(appoitment);
                int[] p = { 1, 1, 1, 1, 1 };
                appointed.setPermmisions(store.GetStoreId(), p);
                //Insert Permissions Into DB
                List<UserStorePermissions> permissions = AdapterUser.CreateNewPermissionSet(addto, storeId, p);
                DbManager.Instance.InsertUserStorePermissionSet(permissions);
                Publisher.Instance.Notify(addto, new NotifyData("Your request to be an Owner to Store - " + storeId + " is Approved"));
                Tuple<bool, string> ansSuccess = Publisher.Instance.subscribe(addto, storeId);
                return ansSuccess;
            }
            appointed.SetApprovalStatus(storeId, true);
            //Add AptovmentStatus to DB
            DbManager.Instance.InsertStoreOwnerShipApprovalStatus(AdapterUser.CreateNewStoreAppoitmentApprovalStatus(storeId, true, addto));
            //No need to Inser Here Approvals we will Insert in the Inner Loop by the Owners.
            appointed.InsertOtherApprovalRequest(storeId, owners);
            foreach(string storeOwner in owners)
            {
                User tmpOwner = UM.GetUser(storeOwner);
                tmpOwner.INeedToApproveInsert(storeId, addto);
                //Insert Approval To DB
                DbManager.Instance.InsertNeedToApprove(AdapterUser.CreateNewApprovalNote(storeOwner, addto, storeId));
                Publisher.Instance.Notify(storeOwner, new NotifyData("User: " + addto + " Is want to Be store:" + storeId + " Owner Let him know what you think"));

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
            if (!store.IsStoreOwner(appointer))
                return new Tuple<bool, string>(false, owner + "Is not a store Owner\n");
            //Liav Will Insert In the correct Table in the DB here
            store.AddStoreManager(appointed);
            //appointed.addManagerAppointment(appointer, store.GetStoreId());
            Tuple<bool, string> res = appointed.addStoreManagment(store,owner);
            //Insert StoreManager Appoint Into DB
            DbManager.Instance.InsertStoreManagerAppoint(AdapterUser.CreateNewManagerAppoitment(owner, addto, storeId));
            int[] p = {1, 1, 0, 0, 0};
            appointed.setPermmisions(store.GetStoreId(), p);
            //Insert Permissions into DB
            List<UserStorePermissions> permissions = AdapterUser.CreateNewPermissionSet(addto, storeId, p);
            DbManager.Instance.InsertUserStorePermissionSet(permissions);
            //Version 2 Addition
            Tuple<bool, string> ans = Publisher.Instance.subscribe(addto, storeId);
            if (!ans.Item1)
                return ans;
            return res;
        }
        //Remove appoitment only if owner gave the permissions to the Appointed user
        public Tuple<bool, string> RemoveAppStoreManager(string o, string m, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (o == null || m == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (o == "" || m == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            User owner = UM.GetAtiveUser(o);
            User manager = UM.GetUser(m);
            if (owner is null || manager is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (owner.isguest() || manager.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(manager))
                return new Tuple<bool, string>(false, m + " Is already Store Owner\n");
            if (!store.IsStoreOwner(owner))
                return new Tuple<bool, string>(false, o + "Is not a store Owner\n");
            if (!manager.isAppointedByManager(owner, store.GetStoreId()) && !(manager.isAppointedByOwner(o, store.GetStoreId())))
                return new Tuple<bool, string>(false, m + "Is not appointed by " + o + "to be store manager\n");
            //Liav will delete from DB Here
            store.RemoveManager(manager);
            manager.RemoveStoreManagment(store.GetStoreId());
            //Remove Store Manager Appoint From here
            DbManager.Instance.DeleteSingleManager(AdapterUser.GetManagerNote(o, m, storeId));
            int[] p = manager.Store_options[storeId];
            manager.RemovePermission(store.GetStoreId());
            //Remove Permissions From DB
            List<UserStorePermissions> permissions = AdapterUser.CreateNewPermissionSet(m, storeId, p);
            DbManager.Instance.DeletePermission(permissions);
            //Version 2 Addition
            Tuple<bool, string> message = Publisher.Instance.Notify(storeId, new NotifyData(m + "is not a StoreID: "+storeId +" StoreName: "+store.GetName() +" Manager any More"));
            if (!message.Item1)
                return message;
            Tuple<bool, string> ans = Publisher.Instance.Unsubscribe(m, storeId);
            return ans;
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
            foreach(User Prevowner in OwnersToRemove)
            {
                //Liav Remove from DB Here
                store.RemoveOwner(Prevowner);
            }
            return new Tuple<bool, string>(true, PrevOwner + " Removed from store: - " + store.StoreName + "\n");

        }
        private List<User> RemoveOwnerLoop(User appointer,User DemoteOwner,Store store)
        {
            List<User> OwnersToRemove = new List<User>();
            OwnersToRemove.Add(DemoteOwner);
            string OwnerRemovalMessage = "You have been Removed From Owner position in the Store " + store.StoreName +"By you appointer - "+appointer.getUserName()+"\n";
            //store.RemoveOwner(DemoteOwner);
            DemoteOwner.RemoveStoreOwner(store.GetStoreId());
            //Remove Store Ownership from DB here
            StoreOwnershipAppoint s = AdapterUser.GetOwnerNote(appointer.getUserName(), DemoteOwner.getUserName(), store.Id);
            DbManager.Instance.DeleteSingleOwnership(s);
            int[] p = DemoteOwner.Store_options[store.Id];
            DemoteOwner.RemovePermission(store.GetStoreId());
            //Remove Owners Store Permissions from DB here
            List<UserStorePermissions> permissions = AdapterUser.CreateNewPermissionSet(DemoteOwner.getUserName(), store.Id, p);
            DbManager.Instance.DeletePermission(permissions);
            Publisher.Instance.Notify(DemoteOwner.getUserName(), new NotifyData(OwnerRemovalMessage));
            Publisher.Instance.Unsubscribe(DemoteOwner.getUserName(), store.GetStoreId());
            //DemoteOwner.RemoveAppoitmentOwner(appointer, store.GetStoreId());
            string Message = "You have been Removed From Manager position in the Store " + store.StoreName + " Due to the fact that you appointer " + DemoteOwner.getUserName() + "Was fired now\n";
            List<string> Managers = store.managers;
            List<User> ManagersToRemove = new List<User>();
            List<string> Owners = store.owners;
            foreach (string managerName in Managers)
            {
                User manager = UserManager.Instance.GetUser(managerName);
                if (manager.isAppointedByManager(DemoteOwner, store.GetStoreId()))
                {
                    ManagersToRemove.Add(manager);
                    manager.RemoveStoreManagment(store.GetStoreId());
                    //Remove Managment From DB
                    DbManager.Instance.DeleteSingleManager(AdapterUser.GetManagerNote(DemoteOwner.getUserName(), managerName, store.Id));
                    int[] pm = manager.Store_options[store.Id];
                    manager.RemovePermission(store.GetStoreId());
                    //Remove Permissions From DB
                    List<UserStorePermissions> perms = AdapterUser.CreateNewPermissionSet(managerName, store.GetStoreId(), pm);
                    DbManager.Instance.DeletePermission(perms);
                    Publisher.Instance.Notify(manager.getUserName(), new NotifyData(Message));
                    Publisher.Instance.Unsubscribe(manager.getUserName(), store.GetStoreId());
                    //manager.RemoveStoreManagment(store.GetStoreId());
                }
            }
            foreach(User manager in ManagersToRemove)
            {
                //Liav Will Delete From DB Here
                store.RemoveManager(manager);
            }
            foreach (string ownername in Owners)
            {
                User owner = UserManager.Instance.GetUser(ownername);
                if(owner.isAppointedByOwner(ownername,store.GetStoreId()))
                {
                    OwnersToRemove.AddRange(RemoveOwnerLoop(DemoteOwner,owner, store));
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

            return manager.setPermmisions(store.GetStoreId(), permissions);
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
