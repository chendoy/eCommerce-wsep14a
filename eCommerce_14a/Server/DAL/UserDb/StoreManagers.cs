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
    public class StoreManager
    {


        [Key, ForeignKey("Appointer")]
        [Column(Order = 1)]
        public string AppointerName { set; get; }
        public virtual DbUser Appointer { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public Store Store { set; get; }


        [Key, ForeignKey("Appointed")]
        [Column(Order = 3)]
        public string AppointedName { set; get; }
        public virtual DbUser Appointed { set; get; }




        public StoreManager(string appointer, string appointed, int storeid)
        {
            AppointedName = appointed;
            AppointerName = appointer;
            StoreId = storeid;
        }

    }
}
