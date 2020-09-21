using eCommerce_14a.StoreComponent.DomainLayer;
using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class StoreManager
    {
        [Key, ForeignKey("Manager")]
        [Column(Order = 1)]
        public string ManagerName { set; get; }
        public virtual DbUser Manager { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public virtual DbStore Store { set; get; }

        public StoreManager(int storeId, string managerName)
        {
            StoreId = storeId;
            ManagerName = managerName;
        }
        
        public StoreManager()
        {

        }

    }
}
