using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class UploadSessionInfoEntity
    {
        public string OriginalFilename { get; set; }
        public string ServerFilename { get; set; }
        public string ContentSessionId { get; set; }
        public object Content { get; set; }
    }
}
