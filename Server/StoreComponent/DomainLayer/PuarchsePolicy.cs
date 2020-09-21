namespace eCommerce_14a.StoreComponent.DomainLayer
{
    public class PuarchsePolicy
    {
        private int type;
        
        public PuarchsePolicy(int type)
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