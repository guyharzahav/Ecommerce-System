using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;


namespace eCommerce_14a.UserComponent.ServiceLayer
{
    public class System_Service
    {
        eSystem Commercial_System;
        public System_Service(string name, string pass)
        {
            Commercial_System = new eSystem(name,pass);
        }
        public void loaddata()
        {
            Commercial_System.loaddata();
        }
        public Tuple<bool, string> initSystem(string userName, string pass,bool paymentconnection = true)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.system_init(userName, pass,paymentconnection);
        }
        public bool SetDeliveryConnection(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.SetDeliveryConnection(con);
        }
        public bool SetPaymentConnection(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.SetPaymentConnection(con);
        }
        public bool CheckDeliveryConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.CheckDeliveryConnection();
        }
        public bool CheckPaymentConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.CheckPaymentConnection();
        }
        public Tuple<bool,string> pay(String PaymentDetails, double Amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.pay(PaymentDetails, Amount);
        }
        public Tuple<bool, string> ProvideDeliveryForUser(string username,bool paymentFlag)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Commercial_System.ProvideDeliveryForUser(username, paymentFlag);
        }


        public void clean(string n, string p)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Commercial_System.clean(n,p);
        }
    }
}
