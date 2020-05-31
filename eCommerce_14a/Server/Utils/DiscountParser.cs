using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        // CompoundDiscount: (AND (XOR (OR r1 cb1) r2) (cp1 OR cb2)) *** NOTICE: prefix operator ***

        // ---------------------- ERROR CODES -----------------------------------------------------
        // RevealedDiscount(-1, -1) -> "parenthesis are not balanced in outer expression"
        // RevealedDiscount(-2, -2) -> "parenthesis are not balanced in one if the inner expressions"
        // RevealedDiscount(-3, -3) -> "invalid operator: must be one of {XOR, OR, AND}
        // RevealdDiscount(-4, -4); -> "unknown discount type"
        // ---------------------------------------------------------------------------------------

        static Regex simpleDiscountRegex = new Regex(@"\b(?<word>\w+):\d*$");
        static Regex conditionalBasketDiscountRegex = new Regex(@"\bcb:\d*:\d*\.?\d*$");
        static Regex conditionalProductDiscountRegex = new Regex(@"\bcp:\d*:\d:\d*\.?\d*$");
        static Regex revealdDiscountRegex = new Regex(@"r:\d*\.?\d*:\d*");
        static string[] operators = new string[3] { "XOR", "OR", "AND" };

        public DiscountPolicy Parse(string text)
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
                int counter = 0;
                string curr = "";
                string opStr = Regex.Split(text, "(XOR|OR|AND)")[1];
                int op = opStr == "XOR" ? 0 : opStr == "OR" ? 1 : opStr == "AND" ? 2 : -1;
                if(op == -1)
                    return new RevealdDiscount(-3, -3);
                string restText = text.Substring(opStr.Length+2, text.Length - 1 - opStr.Length - 2);
                List<string> discounts = new List<string>();
                for (int i = 0; i < restText.Length; i++)
                {
                    if (restText[i] == '(')
                    {
                        if(counter == 0) // shift from 0 to 1 -> new element
                        {
                            curr = "";
                        }
                        counter++;
                    }
                    else if(restText[i] == ')')
                    {
                        if(counter == 1) // shift from 1 to 0 -> finish the current element
                        {
                            curr += restText[i];
                            discounts.Add(curr);
                            curr = "";
                        }
                        counter--;
                    }
                    else if (restText[i] == ' ' && counter == 0)
                    {
                        if(!discounts.Contains(curr) && curr != "")
                        {
                            discounts.Add(curr);
                            curr = "";
                        }
                    }
                    if(!(counter == 0 || curr == ")"))
                        curr += restText[i];
                    if (counter == 0 && restText[i] != ')' && restText[i] != '(' && restText[i] != ' ')
                        curr += restText[i];
                }
                if (curr != "")
                    discounts.Add(curr);
                if (counter != 0)
                    return new RevealdDiscount(-1, -1);

                List<DiscountPolicy> children = new List<DiscountPolicy>();
                
                foreach (string discount in discounts)
                {
                    DiscountPolicy discountPolicy = Parse(discount);
                    if (checkDiscount(discountPolicy) == true) // this indicates an error!
                        return new RevealdDiscount(-2, -2);
                    else
                        children.Add(discountPolicy);
                }
                DiscountPolicy compoundDiscount = new CompundDiscount(op, children);
                return compoundDiscount;
            }
            return new RevealdDiscount(-4, -4);
        }
        // will return true iff <param> discountPolicy is a malformed discount, i.e failed
        // to parse, i.e if it is instance of RevealdDiscount with negative product id.
        static bool checkDiscount(DiscountPolicy discountPolicy)
        {
            try
            {
                RevealdDiscount revealedDiscount = ((RevealdDiscount)discountPolicy);
                return revealedDiscount.discountProdutId < 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
