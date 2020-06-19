﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.UserComponent.DomainLayer
{
    public class Statistic_View
    {
        public int GuestVisitors { get; set; }
        public int RegularVisistors { get; set; }
        public int ManagersVisitors { get; set; }
        public int OwnersVisitors { get; set; }
        public int AdministratorsVisitors { get; set; }
        public int TotalVisistors { get; set; }

        public Statistic_View()
        {
            GuestVisitors = 0;
            RegularVisistors = 0;
            ManagersVisitors = 0;
            OwnersVisitors = 0;
            AdministratorsVisitors = 0;
            TotalVisistors = 0;
        }
        public void SetTotal()
        {
            TotalVisistors = GuestVisitors + RegularVisistors + ManagersVisitors + OwnersVisitors + AdministratorsVisitors;
        }
    }
}
