using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    public class Product
    {
        public int Id { set; get; }
        
        public int StoreId { set; get; }

        public double Price { set; get; }

        public string Details { set; get; }

        public int Rank { set; get; }

        public string Name { set; get; }

        public string Category { set; get; }

        public string ImgUrl { set; get; }
        
        public Product(int product_id, int store_id, string details="this is product", double price=100, string name="", int rank=3, string category="Electricity", string imgUrl="")
        {
            Id = product_id;
            Details = details;
            Price = price;
            Name = name;
            Rank = rank;
            Category = category;
            ImgUrl = imgUrl;
            StoreId = store_id;
        }

 



    }
}