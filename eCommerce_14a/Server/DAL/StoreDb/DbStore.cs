using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbStore
    {

        [Key]
        public int Id { set; get; }

        [ForeignKey("DiscountPolicy")]
        public int RootDiscoutnPolicyId { set; get; }
        public DbDiscountPolicy DiscountPolicy { set; get; }

        [ForeignKey("PurchasePolicy")]
        public int RootPurchasePolicyId { set; get; }
        public DbPurchasePolicy PurchasePolicy { set; get; }
        
        public int Rank { set; get; }
        public string StoreName { set; get; }
        public bool ActiveStore { set; get; }
        
        public DbStore(int id, int rootdiscountpolicyid, int rootpurchasepolicyid, int rank, string storename, bool activestore)
        {
            Id = id;
            RootDiscoutnPolicyId = rootdiscountpolicyid;
            RootPurchasePolicyId = rootpurchasepolicyid;
            Rank = rank;
            StoreName = storename;
            ActiveStore = activestore;
        }

    }
}
