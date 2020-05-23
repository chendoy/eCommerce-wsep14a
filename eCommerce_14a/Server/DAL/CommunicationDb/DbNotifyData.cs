using Server.DAL.UserDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.CommunicationDb
{
    public class DbNotifyData
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Key, ForeignKey("User")]
        [Column(Order = 2)]
        public string UserName { get; set; }
        public DbUser User { get; set; }

        public string Context { get; set; }

        public DbNotifyData (string context, string name)
        {
            Context = context;
            UserName = name;
        }

    }
}
