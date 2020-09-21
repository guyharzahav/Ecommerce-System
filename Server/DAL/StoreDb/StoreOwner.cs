using Server.DAL.UserDb;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Server.DAL.StoreDb
{
    public class StoreOwner
    {
        [Key, ForeignKey("Owner")]
        [Column(Order = 1)]
        public string OwnerName { set; get; }
        public virtual DbUser Owner { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public virtual DbStore Store{ set; get; }

        public StoreOwner(int storeId, string ownerName)
        {
            StoreId = storeId;
            OwnerName = ownerName;
        }

        public StoreOwner()
        {

        }
    }
} 
