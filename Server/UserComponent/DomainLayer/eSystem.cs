using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.UserComponent.Communication;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class eSystem
    {
        private UserManager UManagment;
        private Security bodyguard;
        private DeliveryHandler DH;
        private PaymentHandler PH;
        public eSystem(string name, string pass)
        {
            DH = DeliveryHandler.Instance;
            PH = PaymentHandler.Instance;
            bodyguard = new Security();
            UManagment = UserManager.Instance;
            system_init(name, pass);
            
        }
         
        public Tuple<bool, string> system_init(string admin, string password,bool paymmentconnection = true)
        {
            if (admin is null || password is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (admin == "" || password == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            PH.setConnections(paymmentconnection);
            if (!DH.checkconnection() || !PH.checkconnection() || !paymmentconnection)
            {
                return new Tuple<bool, string>(false, "cann't connect to 3rd party system");
            }
            
            Tuple<bool, string> ans;
            ans = UManagment.RegisterMaster(admin, password);
            if (!ans.Item1)
            {
                return new Tuple<bool, string>(true, "System Admin didn't register");

            }
            return new Tuple<bool, string>(true, "");
        }
        public void loaddata()
        {
            DbManager.Instance.LoadAllUsers();
            Statistics.Instance.visitors = DbManager.Instance.GetStatisticsRecords();
            StoreManagment.Instance.LoadFromDb();
            PurchaseManagement.Instance.LoadFromDb();
            Publisher.Instance.StoreSubscribers = DbManager.Instance.GetAllsubsribers();
            //AppoitmentManager.Instance.LoadAppointments();
        }
        public bool SetDeliveryConnection(bool conn)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            DH.setConnection(conn);
            return true;
        }
        public bool SetPaymentConnection(bool conn)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            PH.setConnections(conn);
            return true;
        }
        public bool CheckDeliveryConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return DH.checkconnection();
        }
        public bool CheckPaymentConnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PH.checkconnection();
        }
        public Tuple<bool,string> pay(string PaymentDetails, double amount)
        {
            if (PaymentDetails is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (PaymentDetails == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PH.pay(PaymentDetails, amount);
        }
        public Tuple<bool, string>  ProvideDeliveryForUser(string name ,bool faield=false)
        {
            if(name is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (name == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
         
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return DH.ProvideDeliveryForUser(name, faield);
        }
        public void clean(string name,string pass)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            system_init(name, pass);
        }
    }
}