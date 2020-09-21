using Server.DAL.StoreDb;
using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StatisticsDb
{
    public class DbStatistics
    {
        [Key]
        [Column(Order = 1)]
        public string Name { set; get; }

        [Key]
        [Column(Order = 2)]
        public DateTime DateTime { set; get; }

        public DbStatistics(String userName, DateTime dateTime)
        {
            Name = userName;
            DateTime = dateTime;
        }

        public DbStatistics()
        {

        }

    }
}
