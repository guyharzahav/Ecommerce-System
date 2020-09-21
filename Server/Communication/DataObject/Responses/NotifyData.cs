using Server.Communication.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Server.DAL;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL.UserDb;

namespace Server.UserComponent.Communication
{
    public class NotifyData : Message
    {
        public string UserName { get; set; }
        public string Context {get; set;}
        public NotifyData(string context, string username="") : base(Opcode.NOTIFICATION) 
        { 
            Context = context;
            UserName = username;
        }

        public NotifyData() : base()
        {

        }
    }
}
