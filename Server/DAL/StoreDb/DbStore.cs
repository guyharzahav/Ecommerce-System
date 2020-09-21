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
    public class DbStore
    {

        [Key]
        public int Id { set; get; }
        public int Rank { set; get; }
        public string StoreName { set; get; }
        public bool ActiveStore { set; get; }


        
        public DbStore(int id, int rank, string storename, bool activestore)
        {
            Id = id;
            Rank = rank;
            StoreName = storename;
            ActiveStore = activestore;
        }
        
        public DbStore()
        {

        }

    }
}
