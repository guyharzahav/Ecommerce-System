using eCommerce_14a;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Statistics
    {
        public List<Tuple<string,DateTime>> visitors { get; set; }
        Dictionary<string, Statistic_View> adminsStatistics { get; set; }
        //public Statistic_View sv { get; set; }

        bool view_is_active { get; set; }
        Statistics()
        {
            adminsStatistics = new Dictionary<string, Statistic_View>();
            visitors = new List<Tuple<string, DateTime>>();
            view_is_active = false;
        }
        private static readonly object padlock = new object();
        private static Statistics instance = null;
        public static Statistics Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Statistics();
                        }
                    }
                }
                return instance;
            }
        }
        public void InserRecord(string uname, DateTime time)
        {

            visitors.Add(new Tuple<string, DateTime>(uname, time));
            DbManager.Instance.InsertStatisticRecord(uname, time);
            if (adminsStatistics.Count == 0)
                return;
            foreach(string admin_name in adminsStatistics.Keys)
            {
                SetNumberInsert(uname, adminsStatistics[admin_name],time);
                adminsStatistics[admin_name].SetTotal();
                Publisher.Instance.NotifyStatistics(admin_name,adminsStatistics[admin_name]);
            }
        }
        public void cleanup()
        {
            adminsStatistics = new Dictionary<string, Statistic_View>();
            view_is_active = false;
            visitors = new List<Tuple<string, DateTime>>();
        }
        public Statistic_View getViewData(string adminName,DateTime? starttime, DateTime? endTime)
        {
            if (!UserManager.Instance.GetAllAdmins().Contains(adminName))
            {
                return null;
            }
            if(!adminsStatistics.ContainsKey(adminName))
            {
                adminsStatistics.Add(adminName, new Statistic_View());
            }
            adminsStatistics[adminName] = new Statistic_View();
            adminsStatistics[adminName].ends_bool = true;
            adminsStatistics[adminName].start_bool = true;
            adminsStatistics[adminName].start = starttime;
            adminsStatistics[adminName].endt = endTime;
            foreach (Tuple<string,DateTime> user in visitors)
            {
                if(user.Item2 >= starttime && user.Item2 <= endTime)
                {
                    FirstSet(user.Item1, adminsStatistics[adminName]);
                }
            }
            adminsStatistics[adminName].SetTotal();
            return adminsStatistics[adminName];
        }
        public Statistic_View getViewDataStart(string adminName,DateTime? starttime)
        {
            if (!UserManager.Instance.GetAllAdmins().Contains(adminName))
            {
                return null;
            }
            if (!adminsStatistics.ContainsKey(adminName))
            {
                adminsStatistics.Add(adminName, new Statistic_View());
            }
            adminsStatistics[adminName] = new Statistic_View();
            adminsStatistics[adminName].start_bool = true;
            adminsStatistics[adminName].ends_bool = false;
            adminsStatistics[adminName].start = starttime;            
            foreach (Tuple<string, DateTime> user in visitors)
            {
                if((user.Item2 >= starttime))
                {
                    FirstSet(user.Item1, adminsStatistics[adminName]);
                }

            }
            adminsStatistics[adminName].SetTotal();
            return adminsStatistics[adminName];
        }
        public Statistic_View getViewDataEnd(string adminName,DateTime? endtime)
        {
            if (!UserManager.Instance.GetAllAdmins().Contains(adminName))
            {
                return null;
            }
            if (!adminsStatistics.ContainsKey(adminName))
            {
                adminsStatistics.Add(adminName, new Statistic_View());
            }
            adminsStatistics[adminName] = new Statistic_View();
            adminsStatistics[adminName].ends_bool = true;
            adminsStatistics[adminName].start_bool = false;
            adminsStatistics[adminName].endt = endtime;
            foreach (Tuple<string, DateTime> user in visitors)
            {
                if(user.Item2 <= endtime)
                {
                    FirstSet(user.Item1, adminsStatistics[adminName]);
                }

            }
            adminsStatistics[adminName].SetTotal();
            return adminsStatistics[adminName];
        }
        public Statistic_View getViewDataAll(string adminName)
        {
            if(!UserManager.Instance.GetAllAdmins().Contains(adminName))
            {
                return null;
            }
            view_is_active = true;
            if (!adminsStatistics.ContainsKey(adminName))
            {
                adminsStatistics.Add(adminName, new Statistic_View());
            }
            adminsStatistics[adminName] = new Statistic_View();
            adminsStatistics[adminName].start_bool = false;
            adminsStatistics[adminName].ends_bool = false;
            foreach (Tuple<string, DateTime> user in visitors)
            {
                FirstSet(user.Item1, adminsStatistics[adminName]);
            }
            adminsStatistics[adminName].SetTotal();
            return adminsStatistics[adminName];
        }
        public void FirstSet(string username, Statistic_View s)
        {
            User user = UserManager.Instance.GetAtiveUser(username);
            if (user is null)
            {
                user = UserManager.Instance.GetUser(username);
                if (user is null)
                {
                    Logger.logError("UserName is not in active users or Usersslist", this, System.Reflection.MethodBase.GetCurrentMethod());
                    return;
                }
            }
            if (user.isguest())
            {
                s.GuestVisitors++;
                return;
            }
            if (user.IsAdmin)
            {
                s.AdministratorsVisitors++;
                return;
            }
            if (user.Store_Ownership.Count == 0 && user.Store_Managment.Count == 0)
            {
                s.RegularVisistors++;
                return;
            }
            if (user.Store_Ownership.Count == 0 && user.Store_Managment.Count > 0)
            {
                s.ManagersVisitors++;
                return;
            }
            if (user.Store_Ownership.Count > 0)
            {
                s.OwnersVisitors++;
                return;
            }
              
        }
        public void SetNumberInsert(string username, Statistic_View s,DateTime time)
        {
            User user = UserManager.Instance.GetAtiveUser(username);
            if(user is null)
            {
                user = UserManager.Instance.GetUser(username);
                if(user is null)
                {
                    Logger.logError("UserName is not in active users or Usersslist", this, System.Reflection.MethodBase.GetCurrentMethod());
                    return;
                }
            }
            if(user.isguest())
            {
                if(!s.ends_bool)
                {
                    if(!s.start_bool)
                    {
                        s.GuestVisitors++;
                        return;
                    }
                    if(s.start < time)
                    {
                        s.GuestVisitors++;
                        return;
                    }
                }
                if(s.ends_bool && s.endt > time)
                {
                    if (!s.start_bool)
                    {
                        s.GuestVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.GuestVisitors++;
                        return;
                    }
                    return;
                }
                return;
            }

            if (user.IsAdmin)
            {
                if (!s.ends_bool)
                {
                    if (!s.start_bool)
                    {
                        s.AdministratorsVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.AdministratorsVisitors++;
                        return;
                    }
                }
                if (s.ends_bool && s.endt > time)
                {
                    if (!s.start_bool)
                    {
                        s.AdministratorsVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.AdministratorsVisitors++;
                        return;
                    }
                    return;
                }
                return;
            }
            if (user.Store_Ownership.Count == 0 && user.Store_Managment.Count == 0)
            {
                if (!s.ends_bool)
                {
                    if (!s.start_bool)
                    {
                        s.RegularVisistors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.RegularVisistors++;
                        return;
                    }
                }
                if (s.ends_bool && s.endt > time)
                {
                    if (!s.start_bool)
                    {
                        s.RegularVisistors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.RegularVisistors++;
                        return;
                    }
                    return;
                }
                return;
            }
            if(user.Store_Ownership.Count == 0 && user.Store_Managment.Count > 0)
            {
                if (!s.ends_bool)
                {
                    if (!s.start_bool)
                    {
                        s.ManagersVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.ManagersVisitors++;
                        return;
                    }
                }
                if (s.ends_bool && s.endt > time)
                {
                    if (!s.start_bool)
                    {
                        s.ManagersVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.ManagersVisitors++;
                        return;
                    }
                    return;
                }
                return;
            }
            if(user.Store_Ownership.Count > 0)
            {
                if (!s.ends_bool)
                {
                    if (!s.start_bool)
                    {
                        s.OwnersVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.OwnersVisitors++;
                        return;
                    }
                }
                if (s.ends_bool && s.endt > time)
                {
                    if (!s.start_bool)
                    {
                        s.OwnersVisitors++;
                        return;
                    }
                    if (s.start < time)
                    {
                        s.OwnersVisitors++;
                        return;
                    }
                    return;
                }
                return;
            }
        }
    }
}
