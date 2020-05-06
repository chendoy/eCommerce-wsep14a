using Client.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Server.Communication.DataObject;
using Server.Communication.DataObject.Requests;
using Server.UserComponent.Communication;

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

        public async Task GetNotifications(NotifierService notifier)
        {
            NotifyData response = await comm.Get<NotifyData>();
            await notifier.Update(response.Context);
        }


        //public List<Store> GetAllActiveStores()
        //{
        //    comm.SendRequest("GetAllActiveStores");
        //    List<Store> stores = (List<Store>)comm.GetAsync();
        //    string json = System.IO.File.ReadAllText("wwwroot/resources/stores.json");
        //    stores = JsonSerializer.Deserialize<List<Store>>(json);
        //    return stores;
        //}

        //public Store GetStoreById(int storeId)
        //{
        //    comm.SendRequest("GetStoreById");
        //    //List<Store> stores = (List<Store>)comm.Get();
        //    string json = System.IO.File.ReadAllText("wwwroot/resources/stores.json");
        //    stores = JsonSerializer.Deserialize<List<Store>>(json);
        //    foreach (Store store in stores)
        //    {
        //        if (store.StoreId == storeId)
        //        {
        //            return store;
        //        }
        //    }

        //    return null;
        //}

        async public Task<bool> Login(User _user)
        {
            LoginRequest loginRequest = new LoginRequest(_user.Username, _user.Password);
            comm.SendRequest(loginRequest);
            LoginResponse response = await comm.Get<LoginResponse>();
            return response.Success;
            
        }

        async public Task<bool> Register(User _user)
        {
            RegisterRequest registerRequest = new RegisterRequest(_user.Username, _user.Password);
            comm.SendRequest(registerRequest);
            RegisterResponse response = await comm.Get<RegisterResponse>();
            return response.Success;
        }
    }
}
