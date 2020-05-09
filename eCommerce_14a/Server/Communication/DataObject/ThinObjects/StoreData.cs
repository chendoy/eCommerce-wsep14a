using eCommerce_14a.StoreComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainLayer.ThinObjects
{
    public class StoreData
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<UserData> Owners { get; set; }
        public List<UserData> Mangers { get; set; }
        public InventoryData Products { get; set; }
        public string StoreThumbnail { get; set; }


        public StoreData() { }

        public StoreData(int storeId, List<UserData> owners, List<UserData> mangers, InventoryData products, string storeThumbnail = "", string storeName = "")
        {
            StoreId = storeId;
            StoreName = storeName;
            Owners = owners;
            Mangers = mangers;
            Products = products;
            StoreThumbnail = storeThumbnail;
        }
    }
}
