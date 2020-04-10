using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text;

namespace eCommerce_14a
{
    public class Store
    {
        private int id;
        private List<User> owners;
        private List<User> managers;
        private Inventory inv;
        private DiscountPolicy discountPolicy;
        private PuarchsePolicy puarchsePolicy;
        private bool isActive;
        public Store(Dictionary<string, object> store_params)
        {
            this.id = (int)store_params["Id"];
            this.owners = new List<User>();
            User storeOwner = (User)store_params["Owner"];
            this.owners.Add(storeOwner);
            this.managers = new List<User>();
            this.inv = (Inventory)store_params["Inventory"];
            this.discountPolicy = (DiscountPolicy)store_params["DiscountPolicy"];
            this.puarchsePolicy = (PuarchsePolicy)store_params["PuarchasePolicy"];
            this.isActive = true;
        }

        public bool ActiveStore
        {
            get { return isActive; }
        }

        public Tuple<bool,Exception> changeStoreStatus(bool newStatus)
        {
            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    return new Tuple<bool, Exception>(false, new Exception("Cann't change store status to active until store has at least one owner"));
                }
            }
            isActive = newStatus;
            return new Tuple<bool, Exception>(true, null);
        }

        public Tuple<bool, Exception> UpdatePrdocutDetails(int userId, int productId, string newDetails)
        {
            if (!hasUser(owners, userId))
            {
                return new Tuple<bool, Exception>(false, new Exception("this user isn't a store owner, thus he can't update inventory products details"));
            }
            Tuple<bool, Exception> res = inv.UpdateProductDetails(productId, newDetails);
            bool updateSucess = res.Item1;
            if (updateSucess)
            {
                return new Tuple<bool, Exception>(true, null);

            }
            else
            {
                return new Tuple<bool, Exception>(false, res.Item2);
            }


        }

        public Tuple<bool,Exception> addProductAmount(int userId, Product product, int amount)
        {

            if (!hasUser(owners, userId))
                return new Tuple<bool, Exception>(false, new Exception("this user isn't a store owner, thus he can't update inventory"));
            Tuple<bool,Exception> res = inv.addProductAmount(product, amount);
            bool addSucess = res.Item1;
            if (addSucess)
            {
                return new Tuple<bool, Exception>(true, null);

            }
            else
            {
                return new Tuple<bool, Exception>(false, res.Item2);
            }

        }
        public Tuple<bool, Exception> decrasePrdouct(int userId, Product p, int amount)
        {
            if (!hasUser(owners, userId))
                return new Tuple<bool, Exception>(false, new Exception("this user isn't a store owner, thus he can't update inventory"));
            Tuple<bool, Exception> res = inv.DecraseProductAmount(p, amount);
            bool decraseSucess = res.Item1;
            if (decraseSucess)
            {
                return new Tuple<bool, Exception>(true, null);

            }
            else
            {
                return new Tuple<bool, Exception>(false, res.Item2);
            }

        }

        private bool hasUser(List<User> users, int userId)
        {
            foreach(User user in users)
            {
                if (user.Id == userId)
                    return true;
            }
            return false;
        }
        public List<User> Owners
        {
            // we dont check for correctn's because it's appoitnment responsibility
            set { owners = value; }
            get { return owners; }
        }
        
        public List<User> Managers
        {
            // we dont check for correctn's because it's appoitnment responsibility
            set { managers = value; }
            get { return managers; }
        }


    }
}
