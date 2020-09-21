using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Communication;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL;
using Server.DAL.CommunicationDb;
using Server.UserComponent.DomainLayer;

namespace Server.UserComponent.Communication
{
    public class Publisher
    {
        public Dictionary<int, LinkedList<string>> StoreSubscribers { get; set; }
        private UserManager UM;
        private WssServer ws;
        Publisher()
        {
            //ws = new WssServer();
            StoreSubscribers = new Dictionary<int, LinkedList<string>>();
            UM = UserManager.Instance;
        }
        private static readonly object padlock = new object();
        private static Publisher instance = null;
        public static Publisher Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Publisher();
                        }
                    }
                }
                return instance;
            }
        }
        public void setServer(WssServer server)
        {
            ws = server;
        }
        public Tuple<bool,string> subscribe(string username, int storeID)
        {
            LinkedList<string> users;
            if(!StoreSubscribers.TryGetValue(storeID,out users))
            {
                users = new LinkedList<string>();
                users.AddFirst(username);
                StoreSubscribers.Add(storeID, users);
                return new Tuple<bool, string>(true, "");
            }
            if(users.Contains(username))
            {
                return new Tuple<bool, string>(true, "User Already Subscribed to this store");
            }
            users.AddFirst(username);
            StoreSubscribers[storeID] = users;
            return new Tuple<bool, string>(true, "");

        }
        public Tuple<bool, string> Unsubscribe(string username, int storeID)
        {
            LinkedList<string> users;
            if (!StoreSubscribers.TryGetValue(storeID, out users))
            {
                return new Tuple<bool, string>(false, "There is No subscription for the Store");
            }
            if (users.Contains(username))
            {
                users.Remove(username);
                StoreSubscribers[storeID] = users;
                return new Tuple<bool, string>(true, "User Removed Subscription");
            }
            return new Tuple<bool, string>(false, "User DO not Subscribed to this store");

        }
        public bool RemoveSubscriptionStore(int storeID)
        {
            return StoreSubscribers.Remove(storeID);

        }
        public Tuple<bool, string> IsSubscribe(string username, int storeID)
        {
            LinkedList<string> users;
            if (!StoreSubscribers.TryGetValue(storeID, out users))
            {
                return new Tuple<bool, string>(false, "There is No subscription for the Store");
            }
            if (users.Contains(username))
            {
                return new Tuple<bool, string>(true, "User Subscribed to this store");
            }
            return new Tuple<bool, string>(false, "User DO not Subscribed to this store");

        }

        public Tuple<bool, string> Notify(int store,NotifyData notification)
        {
            if(ws is null)
            {
                return new Tuple<bool, string>(true, "ws undefiened");
            }
            LinkedList<string> users;
            if (!StoreSubscribers.TryGetValue(store, out users))
            {
                return new Tuple<bool, string>(false, "There is No Subscribers for the Store");
            }
            foreach(string username in users)
            {
                User user = UM.GetUser(username);
                //add notification's user
                notification.UserName = username;
                if (!user.LoggedStatus())
                {
                    user.AddMessage(notification);
                    //add message to db, thus user can get it later
                    if(!user.IsGuest)
                    {
                       DbManager.Instance.InsertUserNotification(AdapterCommunication.ConvertNotifyData(notification));
                    }
                }
                else
                {
                    ws.notify(username, notification);
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> Notify(string username, NotifyData notification)
        {
            if (ws is null)
            {
                return new Tuple<bool, string>(true, "ws undefiened");
            }
            User user = UM.GetUser(username);
            //add notification's user
            notification.UserName = username;
            if (!user.LoggedStatus())
            {
                user.AddMessage(notification);
                //add message to db, thus user can get it later
                if (!user.IsGuest)
                {
                   DbManager.Instance.InsertUserNotification(AdapterCommunication.ConvertNotifyData(notification));
                }
            }
            else
            {
                ws.notify(username, notification);
            }

            return new Tuple<bool, string>(true, "");
        }
        public void NotifyStatistics(string admin_user_name,Statistic_View sv)
        {
            if(ws is null)
            {
                return;
            }
            ws.notifyStatistics(admin_user_name,sv);
        }
        public void cleanup()
        {
            StoreSubscribers = new Dictionary<int, LinkedList<string>>();
            UM = UserManager.Instance;

        }
    }
}
