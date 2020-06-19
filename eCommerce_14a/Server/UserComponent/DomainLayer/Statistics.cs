using eCommerce_14a;
using eCommerce_14a.UserComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Statistics
    {
        public List<Tuple<string,DateTime>> visitors { get; set; }
        
        Statistics()
        {
            visitors = new List<Tuple<string, DateTime>>();
        }
        private static readonly object padlock = new object();
        private static Statistics instance = null;
        public static Statistics Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Statistics();
                        }
                    }
                }
                return instance;
            }
        }
        public void InserRecord(string uname, DateTime time)
        {
            visitors.Add(new Tuple<string, DateTime>(uname, time));
        }
        public void cleanup()
        {
            visitors = new List<Tuple<string, DateTime>>();
        }
        public Statistic_View getViewData(DateTime starttime, DateTime endTime)
        {
            Statistic_View sv = new Statistic_View();
            foreach(Tuple<string,DateTime> user in visitors)
            {
                if(user.Item2 >= starttime && user.Item2 <= endTime)
                {
                    SetNumber(user.Item1, sv);
                }
            }
            sv.SetTotal();
            return sv;
        }
        public Statistic_View getViewDataStart(DateTime starttime)
        {
            Statistic_View sv = new Statistic_View();
            foreach (Tuple<string, DateTime> user in visitors)
            {
                if((user.Item2 >= starttime))
                {
                    SetNumber(user.Item1, sv);
                }

            }
            sv.SetTotal();
            return sv;
        }
        public Statistic_View getViewDataEnd(DateTime endtime)
        {
            Statistic_View sv = new Statistic_View();
            foreach (Tuple<string, DateTime> user in visitors)
            {
                if(user.Item2 <= endtime)
                {
                    SetNumber(user.Item1, sv);
                }

            }
            sv.SetTotal();
            return sv;
        }
        public Statistic_View getViewDataAll()
        {
            Statistic_View sv = new Statistic_View();
            foreach (Tuple<string, DateTime> user in visitors)
            {
                SetNumber(user.Item1, sv);
            }
            sv.SetTotal();
            return sv;
        }
        public void SetNumber(string username, Statistic_View s)
        {
            User user = UserManager.Instance.GetAtiveUser(username);
            if(user is null)
            {
                user = UserManager.Instance.GetUser(username);
                if(user is null)
                {
                    Logger.logError("UserName is not in active users or Usersslist", this, System.Reflection.MethodBase.GetCurrentMethod());
                    return;
                }
            }
            if(user.isguest())
            {
                s.GuestVisitors++;
                return;
            }
            if(user.Store_Ownership.Count == 0 && user.Store_Managment.Count == 0)
            {
                s.RegularVisistors++;
                return;
            }
            if(user.Store_Ownership.Count == 0 && user.Store_Managment.Count > 0)
            {
                s.ManagersVisitors++;
                return;
            }
            if(user.Store_Ownership.Count > 0)
            {
                s.OwnersVisitors++;
                return;
            }
            if(user.IsAdmin)
            {
                s.AdministratorsVisitors++;
                return;
            }
        }
    }
}
