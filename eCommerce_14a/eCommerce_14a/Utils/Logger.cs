using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace eCommerce_14a.Utils
{
    /*
     * wrapper class for log4net: using two loggers (events, errors).
     * can add new loggers here
     */

    static class Logger
    {
        private static readonly log4net.ILog errorLogger = log4net.LogManager.GetLogger("errors");
        private static readonly log4net.ILog eventLogger = log4net.LogManager.GetLogger("events");

        public static Boolean logError(string msg, object that)
        {
            if (errorLogger == null)
            {
                Console.WriteLine("Error while writing to error log.");
                return false;
            }
            else
            {
                errorLogger.Error("[" + getMethodName(that) + "]" + " - " + msg);
                return true;
            }

        }

        public static Boolean logEvent(string msg, object that)
        {
            if (eventLogger == null)
            {
                Console.WriteLine("Error while writing to event log.");
                return false;
            }
            else
            {
                eventLogger.Info("[" + getMethodName(that) + "]" + " - " + msg);
                return true;
            }

        }

        private static string getMethodName(object obj)
        {
            return obj.GetType().Name;
        }
    }
}