namespace eCommerce_14a
{
    public class Product
    {
        private int id;
        private string details;
        
        public Product(int product_id, string details)
        {
            this.id = product_id;
            this.details = details;
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
    }
}