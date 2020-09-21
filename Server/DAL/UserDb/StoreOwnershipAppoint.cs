using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL.StoreDb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Server.DAL.UserDb
{
    public class StoreOwnershipAppoint
    {

        [Key, ForeignKey("Appointer")]
        [Column(Order = 1)]
        public string AppointerName { set; get; }      
        public virtual DbUser Appointer { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }


        [Key, ForeignKey("Appointed")]
        [Column(Order = 3)]
        public string AppointedName { set; get; }
        public virtual DbUser Appointed { set; get; }




        public StoreOwnershipAppoint(string appointer, string appointed, int storeid)
        {
            AppointedName = appointed;
            AppointerName = appointer;
            StoreId = storeid;
        }

        public StoreOwnershipAppoint()
        {

        }
    }
}
