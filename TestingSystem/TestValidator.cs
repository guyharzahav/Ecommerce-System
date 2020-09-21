using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class TestValidator
    {
        private Dictionary<int, Func<PurchaseBasket, int, bool>> discountValidatorFunctions;
        private Dictionary<int, Func<PurchaseBasket, int, User, Store, bool>> purchaseValidatorFunctions;

        public TestValidator(Dictionary<int, Func<PurchaseBasket, int, bool>> discountFunctions, Dictionary<int, Func<PurchaseBasket, int, User, Store, bool>> purchaseValidatorFunctions)
        {
            if (discountFunctions != null)
                this.discountValidatorFunctions = discountFunctions;
            else
                this.discountValidatorFunctions = new Dictionary<int, Func<PurchaseBasket, int, bool>>();

            if (purchaseValidatorFunctions != null)
                this.purchaseValidatorFunctions = purchaseValidatorFunctions;
            else
                this.purchaseValidatorFunctions = new Dictionary<int, Func<PurchaseBasket, int, User, Store, bool>>();
        }


        public void AddDiscountFunction(int preConditionNumber, Func<PurchaseBasket, int, bool> func)
        {
            if (!discountValidatorFunctions.ContainsKey(preConditionNumber))
                discountValidatorFunctions.Add(preConditionNumber, func);
        }
        public void AddPurachseFunction(int preConditionNumber, Func<PurchaseBasket, int, User, Store, bool> func)
        {
            if (!purchaseValidatorFunctions.ContainsKey(preConditionNumber))
                purchaseValidatorFunctions.Add(preConditionNumber, func);
        }

        public void RemoveDiscountFunction(int preConditionNumber)
        {
            if (discountValidatorFunctions.ContainsKey(preConditionNumber))
                discountValidatorFunctions.Remove(preConditionNumber);
        }

        public void RemovePurchaseFunction(int preConditionNumber)
        {
            if (purchaseValidatorFunctions.ContainsKey(preConditionNumber))
                purchaseValidatorFunctions.Remove(preConditionNumber);
        }

        public Dictionary<int, Func<PurchaseBasket, int, bool>> DiscountValidatorFuncs
        {
            get { return discountValidatorFunctions; }
        }

        public Dictionary<int, Func<PurchaseBasket, int, User, Store, bool>> PurchaseValidatorFuncs
        {
            get { return purchaseValidatorFunctions; }
        }
    }
}

