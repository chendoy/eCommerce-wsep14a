﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    class AddProductToStoreRequest : Message
    {
        public int StoreId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string ProductDetails { get; set; }
        public double ProductPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int Pamount { get; set; }
        public string ImgUrl { get; set; }

        public AddProductToStoreRequest(int storeId, string userName, int productId, string productDetails, double productPrice, 
            string productName, string productCategory, int pamount, string imgUrl = @"Image/bana.png") : base(Opcode.ADD_PRODUCT_TO_STORE)
        {
            StoreId = storeId;
            UserName = userName;
            ProductId = productId;
            ProductDetails = productDetails;
            ProductPrice = productPrice;
            ProductName = productName;
            ProductCategory = productCategory;
            Pamount = pamount;
            ImgUrl = imgUrl;
        }

    }
}
