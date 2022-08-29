using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class ResultEntity
    {
        public bool IsSuccess { get; set; }
        public bool IsListResult { get; set; }
        public object Result { get; set; }

        public string ResultString { get; set; }

    }
}
