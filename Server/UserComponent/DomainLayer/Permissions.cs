using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Permission
    {
        public bool ViewComments { get; set; }
        public bool ViewPuarchseHistory { get; set; }
        public bool EditInventory { get; set; }
        public bool EditPurachsePolicy { get; set; }
        public bool EditDiscountPolicy { get; set; }

        public Permission()
        {
        }

        public Permission(int[] permissions)
        {
            ViewComments = permissions[0] == 1;
            ViewPuarchseHistory = permissions[1] == 1;
            EditInventory = permissions[2] == 1;
            EditPurachsePolicy = permissions[3] == 1;
            EditDiscountPolicy = permissions[4] == 1;
        }

        public int[] ToArray()
        {
            return new int[] {
                ViewComments ? 1 : 0,
                ViewPuarchseHistory ? 1 : 0,
                EditInventory ? 1 : 0,
                EditPurachsePolicy ? 1 : 0,
                EditDiscountPolicy ? 1 : 0,
            };
        }
    }
}
