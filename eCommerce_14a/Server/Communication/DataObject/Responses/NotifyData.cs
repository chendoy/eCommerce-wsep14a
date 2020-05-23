using Server.Communication.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Server.DAL;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerce_14a.UserComponent.DomainLayer;
using Server.DAL.UserDb;

namespace Server.UserComponent.Communication
{
    public class NotifyData : Message
    {


        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key ,ForeignKey("User")]
        [Column(Order = 1)]
        public string UserName { get; set; }

        public DbUser User { get; set; }

        public string Context {get; set;}
        public NotifyData(string context, String username="") : base(Opcode.NOTIFICATION) 
        { 
            //NotifyData max_notify = DbManager.Instance.GetNotifyWithMaxId();
            //if (max_notify is null)
            //    Id = 1;
            //else
            //    Id = max_notify.Id + 1;

            Context = context;

        }

        public NotifyData() : base()
        {

        }
    }
}
