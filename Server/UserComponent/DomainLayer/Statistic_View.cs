using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Statistic_View
    {
        public int GuestVisitors { get; set; }
        public int RegularVisistors { get; set; }
        public int ManagersVisitors { get; set; }
        public int OwnersVisitors { get; set; }
        public int AdministratorsVisitors { get; set; }
        public int TotalVisistors { get; set; }

        public DateTime? start { get; set; }
        public DateTime? endt { get; set; }

        public bool start_bool { get; set; }
        public bool ends_bool { get; set; }
        public Statistic_View()
        {
            GuestVisitors = 0;
            RegularVisistors = 0;
            ManagersVisitors = 0;
            OwnersVisitors = 0;
            AdministratorsVisitors = 0;
            TotalVisistors = 0;
            start_bool = false;
            ends_bool = false;

        }
        public void SetTotal()
        {
            TotalVisistors = GuestVisitors + RegularVisistors + ManagersVisitors + OwnersVisitors + AdministratorsVisitors;
        }
    }
}
