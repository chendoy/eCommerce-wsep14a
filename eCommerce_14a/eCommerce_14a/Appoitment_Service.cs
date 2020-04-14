﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eCommerce_14a
{
    public class Appoitment_Service
    {
        AppoitmentManager AM;
        public Appoitment_Service()
        {
            AM = AppoitmentManager.Instance;
        }
        //Apoint user to be store owner by store owner
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-store-owner-43 </req>
        //Need to be store number but i need Liav
        public Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return AppoitmentManager.Instance.AppointStoreOwner(owner, appoint, store);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-a-store-manager-45 </req>        
        public Tuple<bool, string> AppointStoreManage(string owner, string appoint, int storeId)
        {
            return AppoitmentManager.Instance.AppointStoreManager(owner, appoint, storeId);
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-demote-store-manager-47 </req>
        public Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int storeId)
        {
            return AppoitmentManager.Instance.RemoveAppStoreManager(owner, appoint, storeId);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-change-store-managers-permissions-46- </req>
        public Tuple<bool, string> ChangePermissions(string owner, string appoint, int storeId, int[] permissions)
        {
            return AppoitmentManager.Instance.ChangePermissions(owner, appoint, storeId, permissions);
        }
        //For Adim Uses
    }
}
