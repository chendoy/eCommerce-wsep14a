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

namespace Server.DAL
{
    public class DbManager
    {

        private EcommerceContext dbConn;
        private static readonly object padlock = new object();
        private static DbManager instance = null;

        private DbManager()
        {
            dbConn = new EcommerceContext();
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
            if (dbConn.Products.Any())
            {
                return dbConn.Products.Max(product => product.Id) + 1;
            }
            else
            {
                return 1;
            }
        }

        public int GetNextPurchBasketId()
        {
            if (dbConn.Baskets.Any())
            {
                return dbConn.Baskets.Max(b => b.Id) + 1;
            }
            else
            {
                return 1;
            }
        }


        public void DeleteSingleCandidate(CandidateToOwnership candidateToOwnership)
        {
            dbConn.CandidateToOwnerships.Remove(candidateToOwnership);
            dbConn.SaveChanges();
        }
        public void DeleteUser(DbUser user)
        {
            dbConn.Users.Remove(user);
            dbConn.SaveChanges();
        }
        public void DeletePass(DbPassword pass)
        {
            dbConn.Passwords.Remove(pass);
            dbConn.SaveChanges();
        }
        public void DeleteSingleMessage(DbNotifyData msg)
        {
            dbConn.Notifies.Remove(msg);
            dbConn.SaveChanges();
        }

        public DbNotifyData GetDbNotification(string username, string context) 
        {
            DbNotifyData msg = dbConn.Notifies.Where(m => m.Context == context && m.UserName == username).FirstOrDefault();
            return msg;
        }

        public void DeleteMessages(List<DbNotifyData> lmessage)
        {
            foreach(DbNotifyData data in lmessage)
            {
                DeleteSingleMessage(data);
            }
        }
        public void DeleteCandidates(List<CandidateToOwnership> candidates)
        {
            foreach (CandidateToOwnership can in candidates)
            {
                DeleteSingleCandidate(can);
            }
        }

       

        public void DeleteSingleApproval(NeedToApprove msg)
        {
            dbConn.NeedToApproves.Remove(msg);
            dbConn.SaveChanges();
        }
        public void DeleteAprovals(List<NeedToApprove> list)
        {
            foreach (NeedToApprove single in list)
            {
                DeleteSingleApproval(single);
            }
        }
        public void DeleteSingleOwnership(StoreOwnershipAppoint msg)
        {
            dbConn.StoreOwnershipAppoints.Remove(msg);
            dbConn.SaveChanges();
        }
        public void DeleteOwnership(List<StoreOwnershipAppoint> list)
        {
            foreach (StoreOwnershipAppoint single in list)
            {
                DeleteSingleOwnership(single);
            }
        }
        public void DeleteSingleManager(StoreManagersAppoint msg)
        {
            dbConn.StoreManagersAppoints.Remove(msg);
            dbConn.SaveChanges();
        }
        public void DeleteManagers(List<StoreManagersAppoint> list)
        {
            foreach (StoreManagersAppoint single in list)
            {
                DeleteSingleManager(single);
            }
        }
        public void DeleteSinglePermission(UserStorePermissions msg)
        {
            dbConn.UserStorePermissions.Remove(msg);
            dbConn.SaveChanges();
        }
        public void DeletePermission(List<UserStorePermissions> list)
        {
            foreach (UserStorePermissions single in list)
            {
                DeleteSinglePermission(single);
            }
        }
        public void DeleteSingleApprovalStatus(StoreOwnertshipApprovalStatus msg)
        {
            dbConn.StoreOwnertshipApprovalStatuses.Remove(msg);
            dbConn.SaveChanges();
        }
        public void DeleteAprovalsStatuses(List<StoreOwnertshipApprovalStatus> list)
        {
            foreach (StoreOwnertshipApprovalStatus single in list)
            {
                DeleteSingleApprovalStatus(single);
            }
        }

     
        public void DeleteUserFullCascase(User user)
        {
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
            return dbConn.Users.Where(u => u.Name == username).FirstOrDefault();
        }

        public List<DbUser> getAllDBUsers()
        {
            return dbConn.Users.ToList();
        }
        public List<User> LoadAllUsers()
        {
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
                if(usr.LoggedStatus())
                {
                    if (!UserManager.Instance.Active_users.ContainsKey(user))
                    {
                        UserManager.Instance.Active_users.Add(user, usr);
                    }
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
        public Dictionary<int,bool> GetIsApprovedStoreUsers(string userName)
        {
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

            List<StoreOwnershipAppoint> storeOwnershipAppoints = dbConn.StoreOwnershipAppoints.Where(soa => soa.AppointedName == userName).ToList();
            Dictionary<int, string> ownershipsDict = new Dictionary<int, string>();
            foreach (StoreOwnershipAppoint soa in storeOwnershipAppoints)
            {
                ownershipsDict.Add(soa.StoreId, soa.AppointerName);
            }
            return ownershipsDict;
        }

        public void InsertStore(Store store)
        {
            InsertDbstore(StoreAdapter.Instance.ToDbStore(store));
            //InsertInventory(store.Inventory, store.Id);
            InsertDiscountPolicy(store.DiscountPolicy, store.Id, parentId: null);
            InsertPurchasePolicy(store.PurchasePolicy, store.Id, parentId: null);
            InsertOwners(store.owners, store.Id);
            InsertManagers(store.managers, store.Id);
        }

        public void InsertPurchaseBasket(DbPurchaseBasket dbPurchaseBasket)
        {
            dbConn.Baskets.Add(dbPurchaseBasket);
            dbConn.SaveChanges();
        }

     
        public ProductAtBasket GetProductAtBasket(int basketId, int productId)
        {
            return dbConn.ProductsAtBaskets.Where(pab => pab.BasketId == basketId && pab.ProductId == productId).FirstOrDefault();
        }

        public void UpdateProductAtBasket(ProductAtBasket productAtBasket, int wantedAmount)
        {
            productAtBasket.ProductAmount = wantedAmount;
            dbConn.SaveChanges();
        }

        private void InsertDbstore(DbStore dbStore)
        {
            dbConn.Stores.Add(dbStore);
            dbConn.SaveChanges();
        }

        public void UpdateDbCart(DbCart dbCart, Cart cart)
        {
            dbCart.Price = cart.Price;
            dbConn.SaveChanges();
        }

        public void InsertProductAtBasket(ProductAtBasket productAtBasket)
        {
            dbConn.ProductsAtBaskets.Add(productAtBasket);
            dbConn.SaveChanges();
        }

        public void InsertPurchasePolicy(PurchasePolicy policyData, int storeId, int? parentId)
        {
            if (policyData.GetType() == typeof(ProductPurchasePolicy))
            {
                int policyProdutId = ((ProductPurchasePolicy)policyData).policyProductId;
                int preCondition = ((ProductPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: policyProdutId,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.ProductPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(BasketPurchasePolicy))
            {
                int preCondition = ((BasketPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(SystemPurchasePolicy))
            {

                int preCondition = ((SystemPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.PurchasePreCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.SystemPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(UserPurchasePolicy))
            {
                string username = ((UserPurchasePolicy)policyData).UserName;
                int preCondition = ((UserPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                mergetype: null,
                                                                parentid: parentId,
                                                                preconditionid: null,
                                                                policyproductid: null,
                                                                buyerusername: username,
                                                                purchasepolictype: CommonStr.PurchasePolicyTypes.UserPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(CompundPurchasePolicy))
            {
                int mergetype = ((CompundPurchasePolicy)policyData).mergeType;
                DbPurchasePolicy dbPurchase = new DbPurchasePolicy(storeId: storeId,
                                                                   mergetype: mergetype,
                                                                   parentid: parentId,
                                                                   preconditionid: null,
                                                                   policyproductid: null,
                                                                   buyerusername: null,
                                                                   purchasepolictype: CommonStr.PurchasePolicyTypes.CompundPurchasePolicy);
                dbConn.PurchasePolicies.Add(dbPurchase);
                dbConn.SaveChanges();

                int dbPolicyId = GetDbPurchsePolicyId(dbPurchase, storeId, null);
                List<PurchasePolicy> policies = ((CompundPurchasePolicy)policyData).getChildren();

                foreach (PurchasePolicy policy in policies)
                {
                    InsertPurchasePolicy(policy, storeId, dbPolicyId);
                }

            }

        }


        public DbPurchaseBasket GetDbPurchaseBasket(int id)
        {
            return dbConn.Baskets.Where(b => b.Id == id).FirstOrDefault();
        }

        public DbCart GetDbCart(int id)
        {
            return dbConn.Carts.Where(c => c.Id == id).FirstOrDefault();
        }

        public void UpdatePrdouct(DbProduct dbProd, Product product)
        {
            dbProd.Category = product.Category;
            dbProd.Details = product.Details;
            dbProd.ImgUrl = product.ImgUrl;
            dbProd.Name = product.Name;
            dbProd.Price = product.Price;
            dbProd.Rank = product.Rank;
            dbConn.SaveChanges();
        }

        public void UpdateInventoryItem(DbInventoryItem dbInventoryItem, int amount)
        {
            dbInventoryItem.Amount = amount;
            dbConn.SaveChanges();
        }

        public void UpdateDiscountPolicy(DiscountPolicy newPolicy,Store s)
        {
            DeleteAllStoreDiscountPolicy(s);
            InsertDiscountPolicy(newPolicy, s.Id, null);
        }

        public void UpdatePurchasePolicy(PurchasePolicy newPolicy, Store store)
        {
            DeleteAllStorePurchasePolicy(store);
            InsertPurchasePolicy(newPolicy, store.Id, null);
        }

        public void UpdateStore(DbStore dbstore, Store store)
        {
            dbstore.ActiveStore = store.ActiveStore;
            dbstore.Rank = store.Rank;
            dbstore.StoreName = store.StoreName;
            dbConn.SaveChanges();
        }

        public void UpdatePurchaseBasket(DbPurchaseBasket dbPurchaseBasket, PurchaseBasket basket)
        {
            dbPurchaseBasket.Price = basket.Price;
            dbPurchaseBasket.PurchaseTime = basket.PurchaseTime;
            dbConn.SaveChanges();
        }

        public DbProduct GetDbProductItem(int productId)
        {
           return  dbConn.Products.Where(p => p.Id == productId).FirstOrDefault();
        }

        internal DbInventoryItem GetDbInventoryItem(int productId, int storeId)
        {
            return dbConn.InventoriesItmes.Where(invItem => invItem.ProductId == productId  && invItem.StoreId == storeId).FirstOrDefault();
        }

        internal void InsertInventoryItem(DbInventoryItem dbInventoryItem)
        {
            dbConn.InventoriesItmes.Add(dbInventoryItem);
            dbConn.SaveChanges();
        }

        public void InsertDbCart(DbCart dbCart)
        {
            dbConn.Carts.Add(dbCart);
            dbConn.SaveChanges();
        }

        public int GetNextStoreId()
        {
            if(!dbConn.Stores.Any())
            {
                return 1;
            }

            int max_storeId= dbConn.Stores.Max(store => store.Id);
            return max_storeId + 1;
        }

        public void InsertDiscountPolicy(DiscountPolicy discountPolicy, int storeId, int? parentId)
        {

            if (discountPolicy.GetType() == typeof(ConditionalProductDiscount))
            {
                int discountProdutId = ((ConditionalProductDiscount)discountPolicy).discountProdutId;
                int preCondition_num = ((ConditionalProductDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalProductDiscount)discountPolicy).Discount;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition_num, CommonStr.PreConditionType.DiscountPreCondition);
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount));
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(ConditionalBasketDiscount))
            {
                int preCondition = ((ConditionalBasketDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalBasketDiscount)discountPolicy).Discount;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition, CommonStr.PreConditionType.DiscountPreCondition);
                DbDiscountPolicy dbDiscount = new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 discountproductid: null,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount);
                dbConn.DiscountPolicies.Add(dbDiscount);
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(RevealdDiscount))
            {
                int discountProdutId = ((RevealdDiscount)discountPolicy).discountProdutId;
                double discountPrecentage = ((RevealdDiscount)discountPolicy).discount;
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: null,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.RevealdDiscount));
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(CompundDiscount))
            {
                int mergetype = ((CompundDiscount)discountPolicy).GetMergeType();
                DbDiscountPolicy dbDiscount = new DbDiscountPolicy(storeid: storeId,
                                                              mergetype: mergetype,
                                                              parentId: parentId,
                                                              preconditionid: null,
                                                              discountproductid: null,
                                                              discount: null,
                                                              discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount);
                dbConn.DiscountPolicies.Add(dbDiscount);
                dbConn.SaveChanges();
                int dbPolicyId = GetDbDiscountPolicyId(dbDiscount, storeId, null);
                List<DiscountPolicy> policies = ((CompundDiscount)discountPolicy).getChildren();
                foreach (DiscountPolicy policy in policies)
                {
                    InsertDiscountPolicy(discountPolicy: policy, storeId: storeId, parentId: dbPolicyId);
                }
            }
        }

     

        public DbStore GetDbStore(int id)
        {
            return dbConn.Stores.Where(s => s.Id == id).FirstOrDefault();
        }

        private int GetDbDiscountPolicyId(DbDiscountPolicy dbDiscountPolicy, int storeId, int? precondition)
        {
            DbDiscountPolicy dbFromDb = dbConn.DiscountPolicies.Where(dbDiscount => dbDiscountPolicy.Discount == dbDiscount.Discount &
                                                        dbDiscountPolicy.DiscountProductId == dbDiscount.DiscountProductId &
                                                        dbDiscountPolicy.MergeType == dbDiscount.MergeType &
                                                        dbDiscountPolicy.StoreId == storeId &
                                                        dbDiscountPolicy.ParentId == dbDiscount.ParentId &
                                                        GetDbPreConditionNumberById((int)dbDiscountPolicy.PreConditionId) == precondition).FirstOrDefault();
            return dbFromDb.Id;
                                                        
        }


        private int GetDbPurchsePolicyId(DbPurchasePolicy dbPurchasePolicy, int storeId, int? precondition)
        {
            DbPurchasePolicy dbFromDb = dbConn.PurchasePolicies.Where(dbPurchase => dbPurchasePolicy.BuyerUserName == dbPurchase.BuyerUserName &
                                                                    dbPurchasePolicy.MergeType == dbPurchase.MergeType&
                                                                    dbPurchasePolicy.PolicyProductId == dbPurchase.PolicyProductId &
                                                                    dbPurchasePolicy.StoreId == storeId &
                                                                    dbPurchasePolicy.ParentId == dbPurchase.ParentId &
                                                                    GetDbPreConditionNumberById((int)dbPurchasePolicy.PreConditionId) == precondition
                                                                    ).FirstOrDefault();
            return dbFromDb.Id;

        }

        private int GetDbPreConditionNumberById(int Id)
        {
            return dbConn.PreConditions.Where(pre => pre.Id == Id).FirstOrDefault().PreConditionNum;
        }

        private DbPreCondition GetDbPreCondition(int preCondition_num, int preType)
        {
            return dbConn.PreConditions.Where(pre => pre.PreConditionNum == preCondition_num && pre.PreConditionType == preType).FirstOrDefault();
        }


        private void InsertOwners(List<string> owners, int storeId)
        {
            foreach(string owner in owners)
            {
                InsertOwner(new StoreOwner(storeId, owner));
            }

        }

        private void InsertManagers(List<string> managers, int storeId)
        {
            foreach (string manager in managers)
            {
                InsertManager(new StoreManager(storeId, manager));
            }

        }

        public void InsertOwner(StoreOwner owner)
        {
            dbConn.StoreOwners.Add(owner);
            dbConn.SaveChanges();
        }

        private void InsertManager(StoreManager manager)
        {
            dbConn.StoreManagers.Add(manager);
            dbConn.SaveChanges();
        }

        public void InsretInventoryItem(DbInventoryItem invItem)
        {
            dbConn.InventoriesItmes.Add(invItem);
            dbConn.SaveChanges();
        }


        public void InsertPreCondition(DbPreCondition preCoondition)
        {
            dbConn.PreConditions.Add(preCoondition);
            dbConn.SaveChanges();
        }

        public void InsertProduct(DbProduct product)
        {
            dbConn.Products.Add(product);
            dbConn.SaveChanges();
        }

        public void InsertPurchase(DbPurchase dbPurchase)
        {
            dbConn.Purchases.Add(dbPurchase);
            dbConn.SaveChanges();
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
            List<DbDiscountPolicy> storeDiscountPolicies = dbConn.DiscountPolicies.Where(policy => policy.StoreId == storeId).ToList();
            return StoreAdapter.Instance.ComposeDiscountPolicy(storeDiscountPolicies);
        }

        private Inventory LoadInventory(int StoreId)
        {
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

        public PreCondition GetPreCondition(int PreConditionId)
        {
            DbPreCondition pre = dbConn.PreConditions.Where(precon => precon.Id == PreConditionId).FirstOrDefault();
            if(pre.PreConditionType == CommonStr.PreConditionType.DiscountPreCondition)
            {
                return new DiscountPreCondition(pre.PreConditionNum);
            }
            else
            {
                return new PurchasePreCondition(pre.PreConditionNum);
            }
        }

        private PurchasePolicy LoadPurchasePolicy(int storeId)
        {
            List<DbPurchasePolicy> storePurchasePolicies = dbConn.PurchasePolicies.Where(policy => policy.StoreId == storeId).ToList();
            return StoreAdapter.Instance.ComposePurchasePolicy(storePurchasePolicies);
        }
        public void InsertUserNotification(DbNotifyData notification)
        {
            dbConn.Notifies.Add(notification);
            dbConn.SaveChanges();
        }
        public void UpdateUserLogInStatus(string user,bool status)
        {
            DbUser usr = dbConn.Users.Where(u => u.Name == user).SingleOrDefault();
            if(usr != null)
            {
                usr.IsLoggedIn = status;
                dbConn.SaveChanges();
            }

        }
        public StoreOwnertshipApprovalStatus getApprovalStat(string cand, int storeID)
        {
            StoreOwnertshipApprovalStatus s = dbConn.StoreOwnertshipApprovalStatuses.Where(u => u.CandidateName == cand && u.StoreId == storeID).SingleOrDefault();
            return s;
        }
        public void UpdateApprovalStatus(StoreOwnertshipApprovalStatus aps, bool status)
        {
            StoreOwnertshipApprovalStatus s = dbConn.StoreOwnertshipApprovalStatuses.Where(u => u.CandidateName == aps.CandidateName && u.StoreId == aps.StoreId).SingleOrDefault();
            if (s != null)
            {
                s.Status = status;
                dbConn.SaveChanges();
            }

        }
        public  DbDiscountPolicy getParentDiscount(DbDiscountPolicy dbDiscountPolicy)
        {
            return dbConn.DiscountPolicies.Where(policy => policy.Id == dbDiscountPolicy.ParentId).FirstOrDefault();
        }

        public DbPurchasePolicy getParentPurchasePolicy(DbPurchasePolicy dbPurchasePolicy)
        {
            return dbConn.PurchasePolicies.Where(policy => policy.Id == dbPurchasePolicy.ParentId).FirstOrDefault();
        }

        //Store Componenet Delete Functions


        public void DeleteProduct(DbProduct dbProd)
        {
            dbConn.Products.Remove(dbProd);
            dbConn.SaveChanges();
        }

        public void DeleteInventoryItem(DbInventoryItem invItem)
        {
            dbConn.InventoriesItmes.Remove(invItem);
            dbConn.SaveChanges();

        }


        public void DeletePurchasePolicy(DbPurchasePolicy purchasePolicy)
        {
            dbConn.PurchasePolicies.Remove(purchasePolicy);
            dbConn.SaveChanges();
        }

        public void DeleteDiscountPolicy(DbDiscountPolicy discountPolicy)
        {
            dbConn.DiscountPolicies.Remove(discountPolicy);
            dbConn.SaveChanges();
        }

        public void DeleteStoreManager(StoreManager manager)
        {
            dbConn.StoreManagers.Remove(manager);
            dbConn.SaveChanges();
        }

        public void DeleteStoreOwner(StoreOwner owner)
        {
            dbConn.StoreOwners.Remove(owner);
            dbConn.SaveChanges();
        }


        public void DeleteStoreOwnerShipAppoint(StoreOwnershipAppoint soaItem)
        {
            dbConn.StoreOwnershipAppoints.Remove(soaItem);
            dbConn.SaveChanges();
        }

        public void DeleteStoreManagerAppoint(StoreManagersAppoint soaItem)
        {
            dbConn.StoreManagersAppoints.Remove(soaItem);
            dbConn.SaveChanges();
        }


        public void DeletePurchaseBasket(DbPurchaseBasket purchasebasket)
        {
            dbConn.Baskets.Remove(purchasebasket);
            dbConn.SaveChanges();
        }

        public void DeletePrdocutAtBasket(ProductAtBasket productab)
        {
            dbConn.ProductsAtBaskets.Remove(productab);
            dbConn.SaveChanges();
        }

        public void DeleteUserStorePermission(UserStorePermissions permission)
        {
            dbConn.UserStorePermissions.Remove(permission);
            dbConn.SaveChanges();
        }


        public void DeleteFullStore(Store store)
        {
            DeleteAllStoreDiscountPolicy(store);
            DeleteAllStorePurchasePolicy(store);
            DeleteAllStoreInventoryItems(store);
            DeleteAllStoreProductAtBasket(store);
            DeleteAllStoreOwners(store);
            DeleteAllStoreManagers(store);
            DeleteAllStoreOwnersAppoint(store);
            DeleteAllStoreManagersAppoint(store);
            DeleteAllStorePurchaseBasket(store);
            DeleteAllUserStorePermissions(store);
            DeleteAllStoresProducts(store);
            DeleteDbStore(store);
        }

        private void DeleteAllUserStorePermissions(Store store)
        {
            List<UserStorePermissions> userStorePermissions = dbConn.UserStorePermissions.Where(p => p.StoreId == store.Id).ToList();
            foreach(UserStorePermissions permission in userStorePermissions)
            {
                DeleteUserStorePermission(permission);
            }
        }

     

        private void DeleteAllStorePurchaseBasket(Store store)
        {
            List<DbPurchaseBasket> lstBaskets = dbConn.Baskets.Where(basket => basket.StoreId == store.Id).ToList();
            foreach(DbPurchaseBasket purchasebasket in lstBaskets)
            {
                DeletePurchaseBasket(purchasebasket);
            }
        }


        private void DeleteAllStoreProductAtBasket(Store store)
        {
            List<ProductAtBasket> pabLst = dbConn.ProductsAtBaskets.Where(pab => pab.StoreId == store.Id).ToList();
            foreach(ProductAtBasket productab in pabLst)
            {
                DeletePrdocutAtBasket(productab);
            }
        }

      

        private void DeleteAllStoreOwnersAppoint(Store store)
        {
            List<StoreOwnershipAppoint> soaLst =  dbConn.StoreOwnershipAppoints.Where(soa => soa.StoreId == store.Id).ToList();
            foreach(StoreOwnershipAppoint soaItem in soaLst)
            {
                DeleteStoreOwnerShipAppoint(soaItem);
            }
            
        }

        private void DeleteAllStoreManagersAppoint(Store store)
        {
            List<StoreManagersAppoint> soaLst = dbConn.StoreManagersAppoints.Where(soa => soa.StoreId == store.Id).ToList();
            foreach (StoreManagersAppoint soaItem in soaLst)
            {
                DeleteStoreManagerAppoint(soaItem);
            }

        }


        private void DeleteDbStore(Store store)
        {
            DbStore delStore = dbConn.Stores.Where(s => s.Id == store.Id).FirstOrDefault();
            if (delStore != null)
            {
                dbConn.Stores.Remove(delStore);
            }
            dbConn.SaveChanges();
        }

        private void DeleteAllStoreManagers(Store store)
        {
            foreach(string name in store.managers)
            {
                StoreManager manager = dbConn.StoreManagers.Where(storeman => storeman.ManagerName == name).FirstOrDefault();
                if(manager != null)
                {
                    DeleteStoreManager(manager);
                }
            }
        }

        private void DeleteAllStoreOwners(Store store)
        {
            foreach (string name in store.owners)
            {
                StoreOwner owner = dbConn.StoreOwners.Where(o => o.OwnerName == name).FirstOrDefault();
                if (owner != null)
                {
                    DeleteStoreOwner(owner);
                }
            }
        }




        private void DeleteAllStoresProducts(Store store)
        {
            List<DbProduct> prods = dbConn.Products.Where(p => p.StoreId == store.Id).ToList();
            foreach (DbProduct product in prods)
            {
                DeleteProduct(product);
            }

        }

        private void DeleteAllStoreInventoryItems(Store store)
        {
            List<DbInventoryItem> invItems = dbConn.InventoriesItmes.Where(item => item.StoreId == store.Id).ToList();
            foreach(DbInventoryItem invItem in invItems)
            {
                DeleteInventoryItem(invItem);
            }
            dbConn.SaveChanges();
        }



        private void DeleteAllStorePurchasePolicy(Store store)
        {
            List<DbPurchasePolicy> storePolicies = dbConn.PurchasePolicies.Where(policy => policy.StoreId == store.Id).ToList();
            foreach (DbPurchasePolicy purchasePolicy in storePolicies)
            {
                DeletePurchasePolicy(purchasePolicy);
            }
            dbConn.SaveChanges();
        }


        private void DeleteAllStoreDiscountPolicy(Store store)
        {
            List<DbDiscountPolicy> storePolicies = dbConn.DiscountPolicies.Where(policy => policy.StoreId == store.Id).ToList();
            foreach(DbDiscountPolicy discountPolicy in storePolicies)
            {
                DeleteDiscountPolicy(discountPolicy);
            }
            dbConn.SaveChanges();
        }

      



        // User Componnent Insert Functions
        public void InsertUser(DbUser user)
        {
            dbConn.Users.Add(user);
            dbConn.SaveChanges();
        }

        public void InsertCandidateToOwnerShip(CandidateToOwnership candidate)
        {
            dbConn.CandidateToOwnerships.Add(candidate);
            dbConn.SaveChanges();
        }

        public void InsertNeedToApprove(NeedToApprove nta)
        {
            dbConn.NeedToApproves.Add(nta);
            dbConn.SaveChanges();
        }

        public void InsertPassword(DbPassword password)
        {
            dbConn.Passwords.Add(password);
            dbConn.SaveChanges();
        }

        public void InsertStoreManagerAppoint(StoreManagersAppoint sma)
        {
            dbConn.StoreManagersAppoints.Add(sma);
            dbConn.SaveChanges();
        }

        public void InsertStoreOwnershipAppoint(StoreOwnershipAppoint soa)
        {
            dbConn.StoreOwnershipAppoints.Add(soa);
            dbConn.SaveChanges();
        }

        public void InsertStoreOwnerShipApprovalStatus(StoreOwnertshipApprovalStatus soas)
        {
            dbConn.StoreOwnertshipApprovalStatuses.Add(soas);
            dbConn.SaveChanges();
        }

        public void InsertUserStorePermission(UserStorePermissions usp)
        {
            dbConn.UserStorePermissions.Add(usp);
            dbConn.SaveChanges();
        }

        public void InsertUserStorePermissionSet(List<UserStorePermissions> usps)
        {
            foreach(UserStorePermissions usp in usps)
            {
                InsertUserStorePermission(usp);
            }
        }



        public CandidateToOwnership GetCandidate(string userName, int storeId)
        {
            return dbConn.CandidateToOwnerships.Where(candidate => candidate.CandidateName == userName && candidate.StoreId == storeId).FirstOrDefault();
        }

        public void DeleteCandidate(CandidateToOwnership candidateToOwnership)
        {
            dbConn.CandidateToOwnerships.Remove(candidateToOwnership);
            dbConn.SaveChanges();
        }



        public NotifyData GetNotifyWithMaxId()
        {

            /*if (!dbConn.Notifies.Any())
            {
                // The table is empty
                return null;
            }
            return dbConn.Notifies.OrderByDescending(n => n.Id).FirstOrDefault();*/
            return null;
        }
        public void InsertUserUnreadMessages(DbNotifyData ntfd)
        {
            dbConn.Notifies.Add(ntfd);
            dbConn.SaveChanges();
        }
        public Dictionary<int,LinkedList<string>> GetAllsubsribers()
        {
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
            return dbConn.StoreManagersAppoints.Find(s);
        }
        public StoreOwnershipAppoint GetOwnerAppoint(StoreOwnershipAppoint s)
        {
            return dbConn.StoreOwnershipAppoints.Find(s);
        }

        public int GetnextCartId()
        {
            if(dbConn.Carts.Any())
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
