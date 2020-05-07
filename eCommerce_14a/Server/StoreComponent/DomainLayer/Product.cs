namespace eCommerce_14a.StoreComponent.DomainLayer
{
    public class Product
    {
        private int id;
        private double price;
        private string details;
        private int rank;
        private string name;
        private string category;
        private string imgUrl;
        public Product(int product_id, string details="this is product", double price=100, string name="", int rank=3, string category="Electricity", string imgUrl="")
        {
            this.id = product_id;
            this.details = details;
            this.price = price;
            this.name = name;
            this.rank = rank;
            this.category = category;
            this.imgUrl = imgUrl;
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public int  ProductID
        {
            get { return id; }
            set { id = value; }
        }
        public string Details
        {
            get { return details; }
            set { details = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string ImgUrl
        {
            get { return imgUrl; }
            set { imgUrl = value; }
        }
    }
}