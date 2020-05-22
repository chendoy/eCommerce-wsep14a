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
    public class INeedToApprove
    {

        [Key, ForeignKey("Approver")]
        [Column(Order = 1)]
        public string ApproverName { set; get; }
        public virtual DbUser Approver { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public Store Store { set; get; }


        [Key, ForeignKey("Candidate")]
        [Column(Order = 3)]
        public string CandiateName { set; get; }
        public virtual DbUser Candidate { set; get; }




        public INeedToApprove(string approver, string candiate, int storeid)
        {
            CandiateName = candiate;
            ApproverName = approver;
            StoreId = storeid;
        }

    }
}
