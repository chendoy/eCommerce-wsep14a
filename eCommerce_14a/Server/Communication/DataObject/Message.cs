using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject
{

    // https://docs.google.com/document/d/1Fwkb6kEsC5_iS_1LBlOuKMUb3DijDjcgyAT20i2UPdY/edit?usp=sharing

    public enum Opcode
    {
        LOGIN = 1,
        LOGOUT,
        REGISTER,
        STORES, // get all stores in the system
        PRODUCTS,
        PROD_INFO,
        PURCHASE,
        CART,
        SEARCH_PROD,
        OPEN_STORE,
        HISTORY,
        APPOINT_MANAGER,
        APPOINT_OWNER,
        DEMOTE,
        LOGIN_AS_GUEST,
        REMOVE_PRODUCT_FROM_CART,
        UPDATE_DISCOUNT_POLICY,
        UPDATE_PURCHASE_POLICY,
        STORES_BY,
        NOTIFICATION,
        RESPONSE,
        ERROR
    }
    public class Message
    {
        public Opcode _Opcode { get; set; }

        public Message(Opcode opcode)
        {
            _Opcode = opcode;
        }

        public Message()
        {
        }
    }
}
