using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Appoitment
    {
        public string Appointer { set; get; }
        public string Appointed { set; get; }
        public int storeId { set; get; }

        public Appoitment(string appointer, string appointed, int storeId)
        {
            Appointer = appointer;
            Appointed = appointed;
            this.storeId = storeId;
        }
    }
}
