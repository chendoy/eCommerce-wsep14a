using Client.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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

        public Store GetStoreById(int storeId)
        {
            comm.SendRequest("GetStoreById");
            List<Store> stores = (List<Store>)comm.Get();
            string json = System.IO.File.ReadAllText("wwwroot/resources/stores.json");
            stores = JsonSerializer.Deserialize<List<Store>>(json);
            foreach (Store store in stores)
            {
                if (store.StoreId == storeId)
                {
                    return store;
                }
            }

            return null;
        }

        async public Task<User> Login(User _user)
        {
            string username = _user.Username;
            string password = _user.Password;

            comm.SendRequest(_user);
            //comm.SendRequest("ValidateUser");
            List<User> users = (List<User>)comm.Get();
            string json = System.IO.File.ReadAllText("wwwroot/resources/users.json");
            users = JsonSerializer.Deserialize<List<User>>(json);
            foreach(User user in users)
            {
                if (user.Username == username && user.Password == password)
                    return await Task.FromResult(user);
            }

            return await Task.FromResult(new User("null", "null", new[]{""}));
            
        }
    }
}
