using Server.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Responses
{
    public class NotifyStatisticsData : Message
    {
        public Statistic_View statistics {get; set;}

        public NotifyStatisticsData(Statistic_View statistics) : base(Opcode.STATISTICS)
        {
            this.statistics = statistics;
        }
    }
}
