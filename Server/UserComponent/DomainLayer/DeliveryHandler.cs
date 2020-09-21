using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Utils;
using Server.UserComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.DomainLayer

{
    public class DeliveryHandler
    {
        bool connected;
        public bool mock { get; set; }
        public bool work { get; set; }
        //DeliverySystem deliverySystem;
        DeliveryHandler()
        {
            connected = true;
            //deliverySystem = new DeliverySystem();
            mock = false;
            work = true;
        }

        private static readonly object padlock = new object();
        private static DeliveryHandler instance = null;
        public static DeliveryHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DeliveryHandler();
                        }
                    }
                }
                return instance;
            }
        }

        public bool checkconnection()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.connected;
        }
        public bool Delivered(bool isAlive = true)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return isAlive;
        }
        public virtual Tuple<bool, string> ProvideDeliveryForUserold(string name, bool ispayed, bool Failed = false)
        {
            if(!DeliverySystem.IsAlive())
                return new Tuple<bool, string>(false, "Not Connected Delivery System");
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            int delivery_res = DeliverySystem.Supply("dummy", "dummy", "dummy", "dummy", "dummy");
            if(delivery_res < 0)
                return new Tuple<bool, string>(false, "Delivery Failed");

            return new Tuple<bool, string>(true, "FineByNow");
        }

        public virtual Tuple<bool, string> ProvideDeliveryForUser(string deliveryDetails, bool Failed = false)
        {
            if(deliveryDetails is null)
                return new Tuple<bool, string>(false, "Null Delivery System");
            if(mock && !work)
                return new Tuple<bool, string>(false, "System Is Down");
            if (!DeliverySystem.IsAlive(Failed))
                return new Tuple<bool, string>(false, "Not Connected Delivery System");
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            string[] parsedDetails = deliveryDetails.Split('&');
            if (parsedDetails.Length < 5)
            {
                return new Tuple<bool, string>(false, "Not Enough Data");
            }
            try
            {
                string Name = parsedDetails[0];
                if (Name.Length == 0)
                {
                    return new Tuple<bool, string>(false, "Name is Not good");
                }
                string add = parsedDetails[1];
                if (add.Length == 0)
                {
                    return new Tuple<bool, string>(false, "Address is Not good");
                }
                string city = parsedDetails[2];
                if (city.Length == 0)
                {
                    return new Tuple<bool, string>(false, "City is Not good");
                }
                string country = parsedDetails[3];
                if (country.Length == 0)
                {
                    return new Tuple<bool, string>(false, "Country is Not good");
                }
                string zip = parsedDetails[4];
                if (zip.Length == 0)
                {
                    return new Tuple<bool, string>(false, "Zip is Not good");
                }
                if (mock)
                {
                    if (work)
                    {
                        if (DeliverySystemMock.Supply(8) > 0)
                            return new Tuple<bool, string>(true, "Works");
                        return new Tuple<bool, string>(false, "not");
                    }
                    if (DeliverySystemMock.Supply(-1) > 0)
                        return new Tuple<bool, string>(false, "not");
                    return new Tuple<bool, string>(true, "Works");
                }
                int transaction_num = DeliverySystem.Supply(Name, add, city, country, zip);
                if (transaction_num < 0)
                    return new Tuple<bool, string>(false, "Transaction Failed");

                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(true, "Transaction Success");

            }
            catch (Exception ex)
            {
                Logger.logError("ParseFailed" + ex.Message, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Transaction Failed");
            }
        }
        public void setConnection(bool con)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.connected = con;
        }
    }
}