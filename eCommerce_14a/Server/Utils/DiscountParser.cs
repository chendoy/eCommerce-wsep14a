using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.Utils
{
    class DiscountParser
    {
        // RevealdDiscount: (r:discount:productId)
        // ConditionalBasketDiscount: (cb:pre_condition:discount)
        // ConditionalProductDiscount: (cp:ProdutId:preCondition:discount)
        // CompoundDiscount: (((r1 OR cb1) XOR r2) AND (cp1 OR cb2))

        static Regex simpleDiscountRegex = new Regex(@"\b(?<word>\w+):\d*$");
        static Regex conditionalBasketDiscountRegex = new Regex(@"\bcb:\d*:\d*\.?\d*$");
        static Regex conditionalProductDiscountRegex = new Regex(@"\bcb:\d*:\d:\d*\.?\d*$");
        static Regex revealdDiscountRegex = new Regex(@"r:\d*\.?\d*:\d*");
        static string[] operators = new string[3] { "XOR", "OR", "AND" };

        public DiscountPolicy Parse(string text = "(r:1:2 OR cb:2:3)")
        {
            if (simpleDiscountRegex.IsMatch(text))
            {
                if (conditionalBasketDiscountRegex.IsMatch(text))
                {
                    string[] constructs = text.Split(':');
                    int precondition = Convert.ToInt32(constructs[1]);
                    double discount = Convert.ToDouble(constructs[2]);
                    return new ConditionalBasketDiscount(new PreCondition(precondition), discount);
                }
                else if (conditionalProductDiscountRegex.IsMatch(text))
                {
                    string[] constructs = text.Split(':');
                    int productId = Convert.ToInt32(constructs[1]);
                    int precondition = Convert.ToInt32(constructs[2]);
                    double discount = Convert.ToDouble(constructs[3]);
                    return new ConditionalProductDiscount(productId, new PreCondition(precondition), discount);
                }
                else if (revealdDiscountRegex.IsMatch(text))
                {
                    string[] constructs = text.Split(':');
                    double discount = Convert.ToDouble(constructs[1]);
                    int productId = Convert.ToInt32(constructs[2]);
                    return new RevealdDiscount(productId, discount);
                }
            }
            else // compount discount
            {
                string[] constructs = Regex.Split(text, "(XOR|OR|AND)");
                //string discountPolicyText1 = constructs[0].Substring(1);
                //string discountPolicyText2 = constructs[2].Substring(1, constructs[2].Length - 1);
                // string opStr = constructs[1];

                // int op = opStr == "XOR" ? 0 : opStr == "OR" ? 1 : opStr == "AND" ? 2 : -1;
                DiscountPolicy leftDiscount = Parse(discountPolicyText1);
                DiscountPolicy rightDiscount = Parse(discountPolicyText2);
                return new 
            }

            return null;
        }
    }
}
