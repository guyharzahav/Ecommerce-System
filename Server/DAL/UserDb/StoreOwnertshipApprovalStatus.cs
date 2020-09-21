using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL.StoreDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.UserDb
{
    public class StoreOwnertshipApprovalStatus
    {

        [Key, ForeignKey("Store")]
        [Column(Order = 1)]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }


        [Key, ForeignKey("Candidate")]
        [Column(Order = 2)]
        public string CandidateName { set; get; }
        public virtual DbUser Candidate { set; get; }


        public bool Status { set; get; }

        public StoreOwnertshipApprovalStatus(int storeId, bool status, string candidatename)
        {
            CandidateName = candidatename;
            StoreId= storeId;
            Status = status;
        }

        public StoreOwnertshipApprovalStatus()
        {

        }
    }
}
