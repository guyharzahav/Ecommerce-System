using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.UserComponent.DomainLayer
{
    public class Security
    {
        public Security()
        {
            //Console.WriteLine("Security Created\n");
        }
        //String that are sent are already checked to be valid -
        //Hence not null and not blank.
        public string CalcSha1(string pass)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (pass == null || pass == "")
                return null;
            var data = Encoding.ASCII.GetBytes(pass);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
    }
}
