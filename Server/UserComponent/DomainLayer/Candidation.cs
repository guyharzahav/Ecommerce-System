using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Candidation
    {
        public string Appointer { set; get; }
        public string Candidate { set; get; }
        public int storeId { set; get; }

        public Candidation(string appointer, string appointed, int storeId)
        {
            Appointer = appointer;
            Candidate = appointed;
            this.storeId = storeId;
        }
    }
}
