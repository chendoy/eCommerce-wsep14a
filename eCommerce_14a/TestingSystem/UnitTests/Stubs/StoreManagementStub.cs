using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.UnitTests.Stubs
{
    public class StoreManagementStub : StoreManagment
    {
        private int storeId;
        public StoreManagementStub(int storeId)
        {
            this.storeId = storeId; 
        }
        public override Store getStore(int storeId)
        {
            if (storeId == this.storeId)
            {
                return null;
            }

            return null;
        }
    }
}
