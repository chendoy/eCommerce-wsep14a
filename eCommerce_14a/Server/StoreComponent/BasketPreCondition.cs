using System;
using System.Collections.Generic;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.StoreComponent
{
    class BasketPreCondition
    {
        private int preCondNumber;
        Dictionary<int, Func<Product, bool>> validateFunctions;
        public BasketPreCondition(int preCondNumber, Dictionary<int, Func<PurchaseBasket, bool>> validateFunctions)
        {
            this.preCondNumber = preCondNumber;
            this.validateFunctions = validateFunctions;
        }

        public void AddCondition(int conditionNum, Func<Product, bool> condFunc)
        {
            if (!validateFunctions.ContainsKey(conditionNum))
                validateFunctions.Add(conditionNum, condFunc);
        }


        public bool IsFulfilled(int cond, Product p)
        {
            if (validateFunctions.ContainsKey(cond))
                return false;
            return validateFunctions[cond].Invoke(p);
        }
    }
}
