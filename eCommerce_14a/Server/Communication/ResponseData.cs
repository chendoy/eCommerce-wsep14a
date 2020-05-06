using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Utils
{
    class ResponseData
    {
        bool ans;
        string error;

        public ResponseData(bool ans, string error)
        {
            this.ans = ans;
            this.error = error;
        }

        public bool Ans { get => ans; set => ans = value; }
        public string Error { get => error; set => error = value; }
    }
}
