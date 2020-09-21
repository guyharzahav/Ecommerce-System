using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.Requests
{
    public class UpdateProductOfStoreRequest : Message
    {
        public string UserName { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public string PDetails { get; set; }
        public double PPrice { get; set; }
        public string PName { get; set; }
        public string PCategory { get; set; }
        public string ImgUrl { get; set; }

        public UpdateProductOfStoreRequest(string userName, int storeId, int productId, string pDetails, double pPrice, string pName,
            string pCategory, string imgUrl) : base(Opcode.UPDATE_PRODUCT_OF_STORE)
        {
            this.UserName = userName;
            this.StoreId = storeId;
            this.ProductId = productId;
            this.PDetails = pDetails;
            this.PPrice = pPrice;
            this.PName = pName;
            this.PCategory = pCategory;
            this.ImgUrl = imgUrl;
        }
    }
}
