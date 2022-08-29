using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity.Entity
{
    public class ReportParameterEntity : CommonEntity
    {
        public string DataSet { get; set; }
        public object Data { get; set; }
        public object ReportParameters { get; set; }
        public string ReportPath { get; set; }
        public string ReportFilename { get; set; }
        public string Filename { get; set; }
        public Dictionary<string, object> DataSource { get; set; }
    }
}
