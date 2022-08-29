using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class FileManagerEntity : CommonEntity
    {
        public byte[] Byte { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
    }
}
