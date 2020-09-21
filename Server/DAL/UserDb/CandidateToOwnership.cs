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
    public class CandidateToOwnership
    {

        [Key, ForeignKey("Appointer")]
        [Column(Order = 1)]
        public string AppointerName { set; get; }
        public virtual DbUser Appointer { set; get; }


        [Key, ForeignKey("Store")]
        [Column(Order = 2)]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }


        [Key, ForeignKey("Candidate")]
        [Column(Order = 3)]
        public string CandidateName { set; get; }
        public virtual DbUser Candidate { set; get; }




        public CandidateToOwnership(string appointer, string candidate, int storeid)
        {
            CandidateName = candidate;
            AppointerName = appointer;
            StoreId = storeid;
        }

        public CandidateToOwnership()
        {

        }
    }
}
