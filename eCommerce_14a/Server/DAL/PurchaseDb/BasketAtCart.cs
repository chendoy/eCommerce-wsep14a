using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.PurchaseDb
{
    public class BasketAtCart
    {
        [Key, ForeignKey("Cart"), Column(Order = 1)]
        public string CartId { set; get; }
        public DbCart Cart { set; get; }

        [Key, ForeignKey("Basket"), Column(Order = 2)]
        public int BasketId { set; get; }
        public DbPurchaseBasket Basket { set; get; }
        
        public BasketAtCart(string cartid, int basketid)
        {
            CartId = cartid;
            BasketId = basketid;
        }

    }
}
