using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class InventoryData
    {
        public List<Tuple<ProductData, int>> invProducts;

        public InventoryData(List<Tuple<ProductData, int>> invProducts)
        {
            this.invProducts = invProducts;
        }

        public InventoryData()
        {
        }
    }
}
