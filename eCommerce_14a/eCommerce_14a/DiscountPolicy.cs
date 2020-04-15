using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class DiscountPolicy
    {
        private int type;
        public DiscountPolicy(int type)
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
