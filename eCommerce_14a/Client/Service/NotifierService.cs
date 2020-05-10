using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Service
{
    public class NotifierService
    {
        public List<string> Notifications { get; private set; }

        public NotifierService()
        {
            Notifications = new List<string>();
        }
        // Can be called from anywhere
        public async Task Update(string context)
        {
            //System.Threading.Thread.Sleep(2000);
            Notifications.Add(context);
            if (OnNotifyReceived != null)
            {
                await OnNotifyReceived.Invoke(context);
            }
        }

        public async Task Remove(string context)
        {
            Notifications.Remove(context);
            if (OnNotifyRemoved != null)
            {
                await OnNotifyRemoved.Invoke();
            }
        }

        public event Func<string, Task> OnNotifyReceived;
        public event Func<Task> OnNotifyRemoved;
    }
}
