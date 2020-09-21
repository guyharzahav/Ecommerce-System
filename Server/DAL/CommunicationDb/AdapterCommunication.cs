using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.CommunicationDb
{
    class AdapterCommunication
    {
        public static List<DbNotifyData> ConvertNotifyData(List<NotifyData> notifications)
        {
            List<DbNotifyData> Notifications = new List<DbNotifyData>();
            foreach(NotifyData Message in notifications)
            {
                Notifications.Add(ConvertNotifyData(Message));
            }
            return Notifications;

        }
        public static DbNotifyData ConvertNotifyData(NotifyData notification)
        {
            int nextId = DbManager.Instance.GetNotifyWithMaxId();
            DbNotifyData DBnotification = new DbNotifyData(nextId,notification.Context, notification.UserName);

            return DBnotification;

        }
    }
}
