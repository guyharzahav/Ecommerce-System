using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace eCommerce_14a
{
    /*
     * wrapper class for log4net: using two loggers (events, errors).
     * can add new loggers here
     */

    public static class Logger
    {
        private static readonly log4net.ILog errorLogger = log4net.LogManager.GetLogger("errors");
        private static readonly log4net.ILog eventLogger = log4net.LogManager.GetLogger("events");

        public static Boolean logError(string msg, object classObj, MethodBase mb)
        {
            if (errorLogger == null)
            {
                Console.WriteLine("Error while writing to error log.");
                return false;
            }
            else
            {
                errorLogger.Error("[" + getClassName(classObj) + "." + getMethodName(mb) + "]" + " - " + msg);
                return true;
            }

        }

        public static Boolean logEvent(object classObj, MethodBase mb, String msg="")
        {
            if (eventLogger == null)
            {
                Console.WriteLine("Error while writing to event log.");
                return false;
            }
            else
            {
                if(msg.Equals(""))
                    eventLogger.Info("Function '" + getMethodName(mb) + "' was called within " + getClassName(classObj) + ".cs" + " with args: [" + argsPrettify(mb, false) + "]");
                else
                    eventLogger.Info("[" + getClassName(classObj) + "." + getMethodName(mb) + "]" + " - " + msg);
                return true;
            }

        }

        public static Boolean logSensitive(object classObj, MethodBase mb)
        {
            if (eventLogger == null)
            {
                Console.WriteLine("Error while writing to event log.");
                return false;
            }
            else
            {
                eventLogger.Info("Function '" + getMethodName(mb) + "' was called within " + getClassName(classObj) + ".cs" + " with args: [" + argsPrettify(mb, true) + "]");
                return true;
            }

        }

        private static string argsPrettify(MethodBase mb, Boolean sensitiveFlag)
        {
            ParameterInfo[] pis = mb.GetParameters();
            string argsStr = "";

            if (pis.Length == 0)
                argsStr = "None";

            if (sensitiveFlag) // will consider only first argument (i.e no passwords etc.)
                argsStr += pis[0].Name + ":" + pis[0].ParameterType.Name + ", ";

            else
            {
                foreach (ParameterInfo pi in pis)
                {
                    argsStr += pi.Name + ":" + pi.ParameterType.Name + ", ";
                }
            }
            char[] charsToTrim = { ',', ' ' };
            argsStr = argsStr.Trim(charsToTrim);
            return argsStr;
        }

        private static string getMethodName(MethodBase mb)
        {
            return mb.Name;
        }

        private static string getClassName(object obj)
        {
            return obj.GetType().Name;
        }
    }
}
