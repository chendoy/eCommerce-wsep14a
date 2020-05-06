using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Service
{
    public class NotifierService
    {
        public List<string> notifications { get; }

        public NotifierService()
        {
            notifications = new List<string>();
        }
        // Can be called from anywhere
        public async Task Update(string context)
        {
            if (Notify != null)
            {
                notifications.Add(context);
                await Notify.Invoke(notifications.Count);
            }
        }

        public event Func<int, Task> Notify;
    }
}
