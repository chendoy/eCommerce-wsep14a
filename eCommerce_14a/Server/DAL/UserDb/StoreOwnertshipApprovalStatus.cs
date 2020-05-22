using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.UserDb
{
    public class StoreOwnertshipApprovalStatus
    {

        [Key, ForeignKey("Store")]
        public int StoreId { set; get; }
        public Store Store { set; get; }

        public bool Status { set; get; }

        public StoreOwnertshipApprovalStatus(int storeId, bool status)
        {
            StoreId= storeId;
            Status = status;
        }


    }
}
