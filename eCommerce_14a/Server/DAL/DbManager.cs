using eCommerce_14a.UserComponent.DomainLayer;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL
{
    class DbManager
    {

        private EcommerceContext dbConn;
        private static readonly object padlock = new object();
        private static DbManager instance = null;
        public static DbManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DbManager();
                        }
                    }
                }
                return instance;
            }
        }

        private DbManager()
        {
            dbConn = new EcommerceContext();
        }

        public NotifyData GetNotifyWithMaxId()
        {
           
            /*if (!dbConn.Notifies.Any())
            {
                // The table is empty
                return null;
            }
            return dbConn.Notifies.OrderByDescending(n => n.Id).FirstOrDefault();*/
            return null;
        }

        public void InsertUserNotification(NotifyData notification)
        {
            //dbConn.Notifies.Add(notification);
            //dbConn.SaveChanges();
        }
    }
}
