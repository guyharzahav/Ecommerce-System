using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class GetStatisticsRequest : Message
    {

        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public string username { get; set;}

        public GetStatisticsRequest(string username, DateTime? startTime, DateTime? endTime) : base(Opcode.GET_STATISTICS)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.username = username;
        }

    }
}
