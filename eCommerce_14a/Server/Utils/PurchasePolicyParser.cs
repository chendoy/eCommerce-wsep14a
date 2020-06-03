using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.Utils
{
    class PurchasePolicyParser
    {
        // ProductPurchasePolicy: (p:precondition:productId)
        // BasketPurchasePolicy: (b:precondition)
        // SystemPurchasePolicy: (s:preCondition:storeID)
        // UserPurchasePolicy: (u:precondition)
        // CompundPurchasePolicy: (AND (XOR (OR b:1 p:2:3) s:1:1) (OR u:3 u:2)) *** NOTICE: prefix operator ***

        // ---------------------- ERROR CODES -----------------------------------------------------
        // ProductPurchasePolicy(-1, -1) -> "parenthesis are not balanced in outer expression"
        // ProductPurchasePolicy(-2, -2) -> "parenthesis are not balanced in one if the inner expressions"
        // ProductPurchasePolicy(-3, -3) -> "invalid operator: must be one of {XOR, OR, AND}
        // ProductPurchasePolicy(-4, -4); -> "unknown discount type"
        // ---------------------------------------------------------------------------------------

        static string[] prefixes = { "b", "p", "s", "u" };
        static Regex productPurchasePolicyRegex = new Regex(@"\bp:\d*:\d*$");
        static Regex basketPurchasePolicyRegex = new Regex(@"\bb:\d*$");
        static Regex systemPurchasePolicyRegex = new Regex(@"s:\d*:\d*$");
        static Regex userPurchasePolicyRegex = new Regex(@"u:\d*");
        static string[] operators = new string[3] { "XOR", "OR", "AND" };

        public static PurchasePolicy Parse(string text)
        {
            bool simplePolicy = prefixes.Any(prefix => text.StartsWith(prefix));
            if (simplePolicy)
            {
                if (productPurchasePolicyRegex.IsMatch(text)) // (p:precondition:productId)
                {
                    string[] constructs = text.Split(':');
                    int precondition = Convert.ToInt32(constructs[1]);
                    int productId = Convert.ToInt32(constructs[2]);
                    return new ProductPurchasePolicy(new PurchasePreCondition(precondition), productId);
                }
                else if (basketPurchasePolicyRegex.IsMatch(text)) // (b:precondition)
                {
                    string[] constructs = text.Split(':');
                    int precondition = Convert.ToInt32(constructs[1]);
                    return new BasketPurchasePolicy(new PurchasePreCondition(precondition));
                }
                else if (systemPurchasePolicyRegex.IsMatch(text)) // (s:preCondition:storeID)
                {
                    string[] constructs = text.Split(':');
                    int preCondition = Convert.ToInt32(constructs[1]);
                    int storeID = Convert.ToInt32(constructs[2]);
                    return new SystemPurchasePolicy(new PurchasePreCondition(preCondition), storeID);
                }
                else if (userPurchasePolicyRegex.IsMatch(text)) // (u:precondition)
                {
                    string[] constructs = text.Split(':');
                    int precondition = Convert.ToInt32(constructs[1]);
                    //string username = constructs[2];
                    return new UserPurchasePolicy(new PurchasePreCondition(precondition));
                }
            }
            else // compound purchase policy
            {
                int counter = 0;
                string curr = "";
                string[] constructs = Regex.Split(text, "(XOR|OR|AND)");
                if (constructs.Length <= 1)
                    return new ProductPurchasePolicy(new PurchasePreCondition(- 1), -1);
                string opStr = constructs[1];
                int op = opStr == "XOR" ? 0 : opStr == "OR" ? 1 : opStr == "AND" ? 2 : -1;
                if (op == -1)
                    return new ProductPurchasePolicy(new PurchasePreCondition(- 3), -3);
                string restText = text.Substring(opStr.Length + 2, text.Length - 1 - opStr.Length - 2);
                List<string> policies = new List<string>();
                for (int i = 0; i < restText.Length; i++)
                {
                    if (restText[i] == '(')
                    {
                        if (counter == 0) // shift from 0 to 1 -> new element
                        {
                            curr = "";
                        }
                        counter++;
                    }
                    else if (restText[i] == ')')
                    {
                        if (counter == 1) // shift from 1 to 0 -> finish the current element
                        {
                            curr += restText[i];
                            policies.Add(curr);
                            curr = "";
                        }
                        counter--;
                    }
                    else if (restText[i] == ' ' && counter == 0)
                    {
                        if (!policies.Contains(curr) && curr != "")
                        {
                            policies.Add(curr);
                            curr = "";
                        }
                    }
                    if (!(counter == 0 || curr == ")"))
                        curr += restText[i];
                    if (counter == 0 && restText[i] != ')' && restText[i] != '(' && restText[i] != ' ')
                        curr += restText[i];
                }
                if (curr != "")
                    policies.Add(curr);
                if (counter != 0)
                    return new ProductPurchasePolicy(new PurchasePreCondition(- 1), -1);

                List<PurchasePolicy> children = new List<PurchasePolicy>();

                foreach (string policy in policies)
                {
                    PurchasePolicy purchasePolicy = Parse(policy);
                    if (CheckPurchasePolicy(purchasePolicy) == false) // this indicates an error!
                        return new ProductPurchasePolicy(new PurchasePreCondition(- 2), -2);
                    else
                        children.Add(purchasePolicy);
                }
                PurchasePolicy compoundPolicy = new CompundPurchasePolicy(op, children);
                return compoundPolicy;
            }
            return new ProductPurchasePolicy(new PurchasePreCondition(-4), -4);
        }
        // will return true iff <param> purchasePolicy is a malformed policy, i.e failed
        // to parse, i.e if it is instance of ProductPurchasePolicy with negative precondition.
        public static bool CheckPurchasePolicy(PurchasePolicy purchasePolicy)
        {
            try
            {
                ProductPurchasePolicy productPurchasePolicy = ((ProductPurchasePolicy)purchasePolicy);
                return productPurchasePolicy.PreCondition.PreConditionNumber < 0 ? false : true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
