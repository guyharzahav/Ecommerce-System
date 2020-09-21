using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbProduct
    {
        [Key]
        public int Id { set; get; }

        [ForeignKey("Store")]
        public int StoreId { set; get; }
        public DbStore Store { set; get; }

        public double Price { set; get; }

        public string Details { set; get; }

        public int Rank { set; get; }

        public string Name { set; get; }

        public string Category { set; get; }

        public string ImgUrl { set; get; }

        public DbProduct(int id, int storeId,string details = "this is product", double price = 100, string name = "", int rank = 3, string category = "Electricity", string imgUrl = "")
        {
            Id = id;
            StoreId = storeId; 
            Details = details;
            Price = price;
            Name = name;
            Rank = rank;
            Category = category;
            ImgUrl = imgUrl;
        }

        public DbProduct()
        {

        }


    }
}
