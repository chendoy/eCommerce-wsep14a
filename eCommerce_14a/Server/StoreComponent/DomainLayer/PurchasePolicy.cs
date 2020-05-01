namespace eCommerce_14a.StoreComponent.DomainLayer
{
    public class PurchasePolicy
    {
        private int type;
        
        public PurchasePolicy(int type)
        {
            this.type = type;
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}