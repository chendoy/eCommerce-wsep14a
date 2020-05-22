using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;

namespace Server.DAL.UserDb
{
    public class UserStorePermissions
    {

        [Key, ForeignKey("User")]
        [Column(Order = 1)]
        public string UserName { set; get; }
        public DbUser User { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public Store Store { set; get; }


        [Key]
        [Column(Order = 3)]
        public string Permission { set; get; }

        public UserStorePermissions(string user, Store store, string permission)
        {
            UserName = user;
            Store = store;
            Permission = permission;
        }
    }
}
