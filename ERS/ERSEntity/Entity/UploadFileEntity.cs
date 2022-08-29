using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class UploadFileEntity : CommonEntity
    {
        public string UploadedFilename { get; set; }
        public string UploadedServerFilename { get; set; }
    }
}
