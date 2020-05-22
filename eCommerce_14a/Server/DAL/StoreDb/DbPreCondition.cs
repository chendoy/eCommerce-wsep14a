using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.StoreDb
{
    public class DbPreCondition
    {

        [Key]
        public int Id { set; get; }

        public int PreConditionType { set; get; }

        public int PreConditionNum { set; get; }

        public DbPreCondition(int preconditiontype, int preconditionnum)
        {
            PreConditionNum = preconditionnum;
            PreConditionType = preconditiontype;
        }
    }
}
