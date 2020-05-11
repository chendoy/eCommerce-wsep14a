using Server.Communication.DataObject.ThinObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    class UpdatePurchasePolicyRequest : Message
    {
        public int storeId { get; set; }
        public string userName { get; set; }
        public PurchasePolicyData purchasePolicy { get; set; }

        public UpdatePurchasePolicyRequest(int storeId, string userName, PurchasePolicyData purchasePolicy) : base(Opcode.UPDATE_PURCHASE_POLICY)
        {
            this.storeId = storeId;
            this.userName = userName;
            this.purchasePolicy = purchasePolicy;
        }

        public UpdatePurchasePolicyRequest() : base() { }
    }
}
