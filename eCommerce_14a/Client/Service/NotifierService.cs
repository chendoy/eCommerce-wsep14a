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
            System.Threading.Thread.Sleep(2000);
            if (NotifyReceived != null)
            {
                Notifications.Add(context);
                await NotifyReceived.Invoke(context);
            }
        }

        public async Task Remove(string context)
        {
            if (NotifyRemoved != null)
            {
                Notifications.Remove(context);
                await NotifyRemoved.Invoke();
            }
        }

        public event Func<string, Task> NotifyReceived;
        public event Func<Task> NotifyRemoved;
    }
}
