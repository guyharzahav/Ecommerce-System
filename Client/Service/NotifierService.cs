using Server.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Service
{
    public class NotifierService
    {
        public List<string> Notifications { get; private set; }
        public Statistic_View Statistics { get; set; }

        public NotifierService()
        {
            Notifications = new List<string>();
            Statistics = null;
        }
        // Can be called from anywhere
        public async Task Update(string context)
        {
            Notifications.Add(context);
            if (OnNotifyReceived != null)
            {
                await OnNotifyReceived.Invoke(context);
            }
        }

        public async Task Remove(string context)
        {
            Notifications.Remove(context);
            if (OnNotifyRemoved != null)
            {
                await OnNotifyRemoved.Invoke();
            }
        }

        public async Task GotStatistics(Statistic_View context)
        {
            //System.Threading.Thread.Sleep(2000);
            Statistics = context;
            if (OnStatisticsReceived != null)
            {
                await OnStatisticsReceived.Invoke(context);
            }
        }

        public event Func<string, Task> OnNotifyReceived;
        public event Func<Task> OnNotifyRemoved;
        public event Func<Statistic_View, Task> OnStatisticsReceived;
    }
}
