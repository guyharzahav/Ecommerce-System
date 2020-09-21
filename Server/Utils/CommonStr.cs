using eCommerce_14a.StoreComponent.DomainLayer;

namespace eCommerce_14a.Utils
{
    public static class CommonStr
    {

        public static class GeneralErrMessage
        {
            public static string UnKnownErr = "UnKnown Error Occured";
            public static string DbErrorMessage = "There was error while editing the DB";
        }
        
        public static class StoreRoles
        {
            public static string Owner = "owner";
            public static string Manager = "manager";
        }
        public static class SearcherKeys
        {
            public static string ProductKeyWord = "SearchByProductKeyWord";
            public static string ProductRank = "searchByProductRank";
            public static string MinPrice = "minPrice";
            public static string MaxPrice = "maxPrice";
            public static string ProductName = "SearchByProductName";
            public static string ProductCategory = "searchByCategory";
            public static string StoreRank = "SearchByStoreRank";
            public static string StoreId = "SearchByStioreId";
        }

        public static class DiscountPolicyTypes
        {
            public static int CompundDiscount = 1;
            public static int ConditionalProductDiscount = 1;
            public static int ConditionalBasketDiscount = 2;
            public static int RevealdDiscount = 3;
        }

        public static class PurchasePolicyTypes
        {
            public static int CompundPurchasePolicy = 1;
            public static int ProductPurchasePolicy = 2;
            public static int BasketPurchasePolicy = 3;
            public static int SystemPurchasePolicy = 4;
            public static int UserPurchasePolicy = 5;

        }


        public static class DiscountMergeTypes
        {
            public static int XOR = 0;
            public static int OR = 1;
            public static int AND = 2;
        }

        public static class PurchaseMergeTypes
        {
            public static int XOR = 0;
            public static int OR = 1;
            public static int AND = 2;
        }
        public static class DiscountPreConditions
        {
            public static int pre_min = 0;
            public static int pre_max = 4;
            public static int NoDiscount = 0;
            public static int BasketProductPriceAboveEqX = 1; // parameters: product_price, pre_condition
            public static int NumUnitsOfProductAboveEqX = 2; // parameters: product_id, MinUnits, pre_condition 
            public static int BasketPriceAboveX = 3; // parameters: Price, pre_condition
            public static int NumUnitsInBasketAboveEqX = 4; // parameters: Min_NumUnits, pre_condition
        }

        public static class PoliciesErrors
        {
            public static string PreConditionNumberErr = "Pre Condition Number Is out of Boundries";
            public static string DiscountValueErr = "Discount value must be between 0 and 100";
            public static string MergeTypeErr = "Invalid Merge Type";

        }

        public static class PurchasePreCondition
        {
            public static int pre_min = 0;
            public static int pre_max = 8;
            public static int allwaysTrue = 0;
            public static int MaxUnitsOfProductType = 1; // parameters: product_id, max_units, pre_condition
            public static int MinUnitsOfProductType = 2; // parameters: porduct_id, min_units, pre_condition
            public static int MaxItemsAtBasket = 3; // parameters: max_itesm_number, pre_condition
            public static int MinItemsAtBasket = 4;// parameters: min_items_number, pre_condition
            public static int StoreMustBeActive = 5; // parameters: store_id, precondition 
            public static int OwnerCantBuy = 6; //parameters: precondition, pre_condition
            public static int MinBasketPrice = 7; // parameter: min_price, pre_condition
            public static int MaxBasketPrice = 8; // parameters: max_price, pre_condition

        }
        public static class StoreParams
        {
            public static string StoreId = "StoreId";
            public static string mainOwner = "StoreOwner";
            public static string StoreName = "StoreName";
            public static string StoreDiscountPolicy = "StoreDiscountPolicy";
            public static string StorePuarchsePolicy = "StorePuarchsePolicy";
            public static string StoreInventory = "StoreInventory";
            public static string StoreRank = "StoreRank";
            public static string IsActiveStore = "isActive";
            public static string Validator = "Validator";
            public static string Owners = "Owners";
            public static string Managers = "Managers";
        }

        public static class ProductParams
        {
            public static string ProductId = "ProductId";
            public static string ProductPrice = "ProductPrice";
            public static string ProductDetails = "ProductDetails";
            public static string ProductName = "ProductName";
            public static string ProductCategory = "ProductCategory";
            public static string ProductImgUrl = "ImageUrl";
        }

        public static class ProductCategoty
        {
            public static string Consola = "Consols";
            public static string Kitchen = "Kitchen";
            public static string Computers = "Computers";
            public static string Health = "Health";
            public static string CoffeMachine = "Coffe Machines";
            public static string Beauty = "Beauty";
            public static string Cleaning = "Cleaning";


        }

        public static class StoreErrorMessage
        {
            public static string noStoreOwnersAtAllErrMsg = "The Action can't be performed because there is no store owner";
            public static string notAOwnerOrManagerErrMsg = "This user isn't a store owner or manager, thus he can't perform this action";
            public static string ManagerNoPermissionErrMsg = "This manager doesn't have permission to perform this action";
            public static string BasketNotAcceptPurchasePolicy = "The baske not accepted the store's purchase policy";
            public static string PurchasePolicyErrMessage = "Purchase policy not updated due to incorrect data";
            public static string DiscountPolicyErrMessage = "Purchase policy not updated due to incorrect data";

        }

        public static class InventoryErrorMessage
        {
            public static string ProductPriceErrMsg = "Product price invalid";
            public static string ProductAlreadyExistErrMsg = "Product already exists!";
            public static string ProductNotExistErrMsg = "Product not exist!";
            public static string productAmountErrMsg = "This Action leads to invalid Product Amount";
            public static string ProductShortageErrMsg = "There arn't enough product in the Inventory to perform this action";
            public static string NullInventroyErrMsg = "Inventory Cann't be Null";
            public static string NegativeProductAmountErrMsg = "Product amount cann't be negative in the inventory";
            public static string UnmatchedProductAnKeyErrMsg = " Proudct Id dos'nt matches his key";
            public static string InvalidInventory = "Invalid Inventory";
            public static string DbErrMessage = "there was error while editing the db";
        }

        public static class StoreMangmentErrorMessage
        {
            public static string nonExistOrActiveUserErrMessage = "This user doesn't exist or no active";
            public static string nonExistingStoreErrMessage = "Non existing Store with this Id";
            public static string notMainOwnerErrMessage = "This Action can't be performed because this user isn't a Main Owner";
            public static string notStoreOwnerErrMessage = "This Action can't  be performed because the user is not a store Owner";
            public static string userNotFoundErrMsg = "Null User Supplied";
            public static string illegalStoreName = "Store name iilegal";
            public static string DiscountPolicyParsedFailed = "There was error parsing the discount policy";
            public static string PurchasePolicyParsedFailed = "There was error parsing the purchase policy";


        }
        public static class MangerPermission
        {
            public static string Comments = "CommentsPermission";
            public static string Purchase = "ViewPuarchseHistory";
            public static string Product = "ProductPermission";
            public static string PurachsePolicy = "PurachsePolicy";
            public static string DiscountPolicy = "DiscountPolicy";
        }
        public static class UserListsOptions
        {
            public static string MasterAppointers = "MasterAppointer";
            public static string IsNeedToApprove = "IsNeedToApproveSomeone";
            public static string OwnStores = "IsOwnsStores";
            public static string ManageStores = "IsManagesStores";
            public static string StoreApprovalStatus = "StoreApprovalStatus";
            public static string TheyNeedApprove = "TheyNeedToApproveUser";
            public static string Permmisions = "PermissionSets";
        }
        public static class ArgsTypes
        {
            public static string Empty = "Blank Arguments";
            public static string None = "NullArguments";
        }

        public static class PurchaseMangmentErrorMessage
        {
            public static string BlankOrNullInputErrMsg = "Got null or blank input";
            public static string NegativeProductAmountErrMsg = "Cannot have negative amount of product in cart";
            public static string ZeroProductAmountErrMsg = "Cannot add zero amount of product to cart";
            public static string ProducAlreadyExistInCartErrMsg = "The product is not already in the shopping cart";
            public static string ProductExistInCartErrMsg = "The product is already in the shopping cart";
            public static string NotValidPaymentErrMsg = "Not a valid paymentDetails";
            public static string NotValidAddressErrMsg = "Not a valid address";
            public static string EmptyCartPurchaseErrMsg = "Cannot add zero amount of product to cart";
            public static string NotAdminErrMsg = "This Action is for admins only";
        }
        public static class StorePermissions
        {
            public static int[] RegularManager = {1,1,0,0,0 };
            public static int[] FullPermissions = { 1, 1, 1, 1, 1};
            public static int[] BlankPermisions = { 0, 0, 0, 0, 0 };
            public static int[] RegularAndProduct = { 1, 1, 1, 0, 0 };
        }

        public static class PreConditionType
        {
            public static int DiscountPreCondition = 0;
            public static int PurchasePreCondition = 1;


        }
    }
}
