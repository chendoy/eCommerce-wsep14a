using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbDiscountPolicy
    {
        [Key]
        public int Id { set; get; }

        [ForeignKey("Store")]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }

        public int MergeType { set; get; }
        
        public int ParentId { set; get; }

        [ForeignKey("PreCondition")]
        public int PreConditionId { set; get; }
        public DbPreCondition PreCondition { set; get; }

        public int DiscountProductId { set; get; }

        public double Discount { set; get; }


        public DbDiscountPolicy (int storeid, int mergetype, int parentId, int preconditionid, int discountproductid, int discount)
        {
            StoreId = storeid;
            MergeType = mergetype;
            ParentId = parentId;
            PreConditionId = preconditionid;
            DiscountProductId = discountproductid;
            Discount = discount;
        }

    }
}
