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
        ALL_STORES, // get all stores in the system
        PRODUCTS_OF_STORE,
        PROD_INFO,
        PURCHASE,
        USER_CART,
        SEARCH_PROD,
        OPEN_STORE,
        BUYER_HISTORY,
        APPOINT_MANAGER,
        APPOINT_OWNER,
        DEMOTE_MANAGER,
        DEMOTE_OWNER,
        LOGIN_AS_GUEST,
        REMOVE_PRODUCT_FROM_CART,
        UPDATE_DISCOUNT_POLICY,
        UPDATE_PURCHASE_POLICY,
        GET_ALL_REGISTERED_USERS,
        STORES_OWNED_BY,
        ADD_PRODUCT_TO_CART,
        CHANGE_PRODUCT_AMOUNT_CART,
        GET_STAFF_OF_STORE,
        GET_AVAILABLE_DISCOUNTS,
        GET_AVAILABLE_PURCHASES,
        ADD_PRODUCT_TO_STORE,
        REMOVE_PRODUCT_FROM_STORE,
        UPDATE_PRODUCT_OF_STORE,
        STORE_HISTORY,
        GET_USER_PERMISSIONS,
        GET_DISCOUNT_POLICY,
        GET_PURCHASE_POLICY,
        ALL_STORE_HISTORY,
        ALL_BUYERS_HISTORY,
        GET_MANAGER_PERMISSION,
        STORE_BY_ID,
        CHANGE_PERMISSIONS,
        APPROVE_APPOINTMENT,
        INCREASE_PRODUCT_AMOUNT,
        DECREASE_PRODUCT_AMOUNT,
        NOTIFICATION,
        GET_ALL_PRECONDITIONS,
        GET_APPROVAL_LIST,
        MAKE_ADMIN,
        GET_STATISTICS,
        STATISTICS,
        PURCHASE_NO_CONNECTION,
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
