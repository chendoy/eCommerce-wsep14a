using Client.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Client.Service
{
    public class ECommerce14AService
    {
        private Communication comm;
        public ECommerce14AService()
        {
            comm = new Communication();
            Time = DateTime.Now;
        }

        public DateTime Time { get; private set; }

        public List<Store> GetAllActiveStores()
        {
            comm.SendRequest("GetAllActiveStores");
            List<Store> stores = (List<Store>)comm.Get();
            string json = System.IO.File.ReadAllText("wwwroot/resources/stores.json");
            stores = JsonSerializer.Deserialize<List<Store>>(json);
            return stores;
        }
    }
}
