﻿using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
   public class DbPurchasePolicy
    {
        [Key]
        public int Id { set; get; }

        [ForeignKey("Store")]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }

        public int? MergeType { set; get; }

        public int? ParentId { set; get; }

        public int? PreConditionId { set; get; }

        [ForeignKey("Product")]
        public int? PolicyProductId { set; get; }
        public DbProduct Product { set; get; }

        [ForeignKey("Buyer")]
        public string BuyerUserName { set; get; }
        public virtual DbUser Buyer { set; get; }

        public int PurchasePolicyType { set; get; }

        public int? MaxProductUnits { set; get; }
        
        public int? MinProductUnits { set; get; }
        public int? MaxItemsAtBasket { set; get; }
        public int? MinItemsAtBasket { set; get; }
        public double? MinBasketPrice { set; get; }

        public double? MaxBasketPrice { set; get; }


        public DbPurchasePolicy(int storeId, int? mergetype, int? parentid, int? preconditionid, int? policyproductid, string buyerusername, int purchasepolictype, int? maxproductsunits, int ? minproductsunits, int? maxitemsatbasket, int? minitemsatbasket, double? minbasketprice, double? maxbaskeptrice)
        {
            StoreId = storeId;
            MergeType = mergetype;
            ParentId = parentid;
            PreConditionId = preconditionid;
            PolicyProductId = policyproductid;
            BuyerUserName = buyerusername;
            PurchasePolicyType = purchasepolictype;
            MaxProductUnits = maxproductsunits;
            MinProductUnits = minproductsunits;
            MaxItemsAtBasket = maxitemsatbasket;
            MinItemsAtBasket = minitemsatbasket;
            MinBasketPrice = minbasketprice;
            MaxBasketPrice = maxbaskeptrice;
        }

        public DbPurchasePolicy()
        {

        }

    }
}
