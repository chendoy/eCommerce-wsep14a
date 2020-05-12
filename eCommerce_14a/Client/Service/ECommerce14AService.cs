using Client.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Server.Communication.DataObject;
using Server.Communication.DataObject.ThinObjects;
using Server.Communication.DataObject.Requests;
using Server.Communication.DataObject.Responses;

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
        
        public NotifierService NotifierService
        {
            get { return comm.NotifierService; }
        }


        async public Task<List<StoreData>> GetAllActiveStores()
        {
            GetAllStoresRequest getAllStoresRequest = new GetAllStoresRequest();
            comm.SendRequest(getAllStoresRequest);
            GetStoresResponse getStoresResponse = await comm.Get<GetStoresResponse>(); 
            return getStoresResponse.Stores;
        }

        //public Get GetStoreById(int storeId)
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

        async public Task<GetUsersCartResponse> GetCart(string username)
        {
            CartRequest cartRequest = new CartRequest(username);
            comm.SendRequest(cartRequest);
            GetUsersCartResponse getCartResponse = await comm.Get<GetUsersCartResponse>();
            return getCartResponse;
        }

        async public Task<TResponse> GetResponse<TRequest, TResponse>(TRequest request)
        {
            comm.SendRequest(request);
            TResponse response = await comm.Get<TResponse>();
            return response;
        }



        async public Task<LoginResponse> Login(UserData _user)
        {
            LoginRequest loginRequest = new LoginRequest(_user.Username, _user.Password);
            comm.SendRequest(loginRequest);
            LoginResponse response = await comm.Get<LoginResponse>();
            return response;


        }

        async public Task<bool> Register(UserData _user)
        {
            RegisterRequest registerRequest = new RegisterRequest(_user.Username, _user.Password);
            comm.SendRequest(registerRequest);
            RegisterResponse response = await comm.Get<RegisterResponse>();
            return response.Success;
        }

        async public Task<LogoutResponse> Logout(string username)
        {
            LogoutRequest request = new LogoutRequest(username);
            comm.SendRequest(request);
            LogoutResponse response = await comm.Get<LogoutResponse>();
            return response;
        }

        async public Task<LoginAsGuestResponse> LoginAsaGuest()
        {
            LoginAsGuestRequest request = new LoginAsGuestRequest();
            comm.SendRequest(request);
            LoginAsGuestResponse loginAsGuestResponse = await comm.Get<LoginAsGuestResponse>();
            return loginAsGuestResponse;
        }

        async public Task<GetStoresOwnedByResponse> GetStoresOwnedBy(string username)
        {
            GetStoresOwnedByRequest request = new GetStoresOwnedByRequest(username);
            comm.SendRequest(request);
            GetStoresOwnedByResponse response = await comm.Get<GetStoresOwnedByResponse>();
            return response;
        }

        async public Task<GetAllRegisteredUsersResponse> GetAllActiveUsers()
        {
            GetAllRegisteredUsersRequest request = new GetAllRegisteredUsersRequest();
            comm.SendRequest(request);
            GetAllRegisteredUsersResponse response = await comm.Get<GetAllRegisteredUsersResponse>();
            return response;
        }
    }
}
