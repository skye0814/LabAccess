using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class CommonEntity
    {
        public bool IsSuccess { get; set; }
        public List<string> MessageList { get; set; }
        public int CreatedBy { get; set; }

        //for duplicate checking
        public string Mode { get; set; }

    }
}
