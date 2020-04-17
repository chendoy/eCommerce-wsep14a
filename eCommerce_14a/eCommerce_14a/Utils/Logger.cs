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

        public static Boolean logEvent(string msg, object classObj, MethodBase mb)
        {
            if (eventLogger == null)
            {
                Console.WriteLine("Error while writing to event log.");
                return false;
            }
            else
            {
                eventLogger.Info("[" + getClassName(classObj) + "." + getMethodName(mb) + "]" + " - " + msg);
                return true;
            }

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
