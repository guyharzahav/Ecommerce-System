using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;


namespace eCommerce_14a.UserComponent.ServiceLayer
{
    public class Appoitment_Service
    {
        AppoitmentManager AM;
        public Appoitment_Service()
        {
            AM = AppoitmentManager.Instance;
        }
        //Apoint user to be store owner by store owner
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-store-owner-43 </req>
        //Need to be store number but i need Liav
        public Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppoitmentManager.Instance.AppointStoreOwner(owner, appoint, store);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-a-store-manager-45 </req>        
        public Tuple<bool, string> AppointStoreManage(string owner, string appoint, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppoitmentManager.Instance.AppointStoreManager(owner, appoint, storeId);
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-demote-store-manager-47 </req>
        public Tuple<bool, string> RemoveStoreManager(string appointer, string appointed, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppoitmentManager.Instance.RemoveAppStoreManager(appointer, appointed, storeId);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-change-store-managers-permissions-46- </req>
        public Tuple<bool, string> ChangePermissions(string owner, string appoint, int storeId, int[] permissions)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (permissions.Length == 0)
                return new Tuple<bool, string>(false, "Empty Permission");
            return AppoitmentManager.Instance.ChangePermissions(owner, appoint, storeId, permissions);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-change-store-managers-permissions-46- </req>
        public Tuple<bool, string> RemoveStoreOwner(string owner, string PrevOwner, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppoitmentManager.Instance.RemoveStoreOwner(owner, PrevOwner, storeId);
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-store-owner-43 </req>
        internal Tuple<bool, string> ApproveAppointment(string owner, string appointed, int storeID, bool approval)
        {
            return AppoitmentManager.Instance.ApproveAppoitment(owner, appointed, storeID, approval);
        }
        //For Adim Uses
    }
}
