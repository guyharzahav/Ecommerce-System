using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class ProductData
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Details { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }

        public ProductData() { }

        public ProductData(int productId, string name, string category, string details, double price,  string imgUrl)
        {
            ProductId = productId;
            Name = name;
            Category = category;
            Details = details;
            Price = price;
            ImgUrl = imgUrl;
        }
    }
}
