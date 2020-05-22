using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.PurchaseDb
{
    public class DbPurchase
    {
        [Key, ForeignKey("User"), Column(Order = 1)]
        public string UserName { get; set; }
        public DbUser User { get; set; }

        [Key, ForeignKey("Cart"), Column(Order = 2)]
        public int CartID { get; set; }
        public DbCart Cart { get; set; }

        public DbPurchase(string username, int cartid)
        {
            UserName = username;
            CartID = cartid;
        }
    }
}
