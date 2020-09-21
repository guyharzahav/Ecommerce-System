using eCommerce_14a.Utils;
using Server.UserComponent.DomainLayer;
using SuperSocket.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class PaymentHandler
    {
        bool connected;
        public bool mock { get; set; }
        public bool work { get; set; }
        //PaymentSystem paymentSystem;
        PaymentHandler()
        {
            connected = true;
            //paymentSystem = new PaymentSystem();
            mock = false;
            work = true;
        }

        private static readonly object padlock = new object();
        private static PaymentHandler instance = null;
        public static PaymentHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PaymentHandler();
                        }
                    }
                }
                return instance;
            }
        }

        public bool checkconnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return PaymentSystem.IsAlive();
        }
        public virtual Tuple<bool,string> pay(string paymentDetails, double amount, bool Failed = false)
        {
            if (!PaymentSystem.IsAlive(Failed))
                return new Tuple<bool, string>(false, "Not Connected");
            string[] parsedDetails = paymentDetails.Split('&');
            int transaction_num = PaymentSystem.Pay(parsedDetails[0], parsedDetails[1].ToInt32(), parsedDetails[2].ToInt32(), parsedDetails[3], parsedDetails[4], parsedDetails[5]);
            if(transaction_num < 0)
                return new Tuple<bool, string>(false, "Payment Failed");

            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return new Tuple<bool,string>(true,"OK");
        }

        public virtual  int pay(string paymentDetails, bool Failed = false)
        {
            if (paymentDetails is null)
                return -1;
            if (mock && !work)
                return  -1;
            if (!PaymentSystem.IsAlive(Failed))
                return -1;

            string[] parsedDetails = paymentDetails.Split('&');
            if (parsedDetails.Length < 6)
            {
                return -1;
            }
            try
            {
                string cardNum = parsedDetails[0];
                if(cardNum.Length == 0)
                {
                    return -1;
                }
                int month = parsedDetails[1].ToInt32();
                if(month < 1 || month > 12)
                {
                    return -1;
                }
                int year = parsedDetails[2].ToInt32();
                string name = parsedDetails[3];
                if (name.Length == 0)
                {
                    return -1;
                }
                string cvv = parsedDetails[4];
                if (cvv.Length == 0)
                {
                    return -1;
                }
                string sid = parsedDetails[5];
                if (sid.Length == 0)
                {
                    return -1;
                }
                if (mock)
                {
                    if (work)
                    {
                        return PaymentSystemMock.Pay(cardNum, month, year, name, cvv, sid, 8);
                    }
                    return PaymentSystemMock.Pay(cardNum, month, year, name, cvv, sid, -1);
                }
                int transaction_num = PaymentSystem.Pay(cardNum, month, year, name, cvv, sid);
                if (transaction_num < 0)
                    return -1;

                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
                return transaction_num;

            }
            catch(Exception ex)
            {
                Logger.logError("ParseFailed"+ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return -1;
            }
            
        }

        public virtual Tuple<bool, string> refund(string paymentDetails, double amount)
        {
            if (paymentDetails is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null args");
            }
            if (paymentDetails == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank args");
            }
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return new Tuple<bool, string>(true, "OK");
        }

        public virtual Tuple<bool, string> refund(int transactionId)
        {
            if(transactionId < 0 )
                return new Tuple<bool, string>(false, "Invalid TransactionId");


            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (mock)
            {
                if (work)
                {
                    if (PaymentSystemMock.CancelPayment(8,8) > 0)
                        return new Tuple<bool, string>(true, "Works");
                    return new Tuple<bool, string>(false, "not");
                }
                if (PaymentSystemMock.CancelPayment(8, -1) > 0)
                    return new Tuple<bool, string>(false, "not");
                return new Tuple<bool, string>(true, "Works");
            }
            int cancel_res = PaymentSystem.CancelPayment(transactionId);
            if(cancel_res < 0)
                return new Tuple<bool, string>(false, "refund failed");

            return new Tuple<bool, string>(true, "OK");
        }
        public void setConnections(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.connected = con;
        }
    }
}