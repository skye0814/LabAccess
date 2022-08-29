using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSUtil
{
    public static class RegexUTIL
    {
        public static readonly string REGEX_PASSWORD = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$";
        public static readonly string REGEX_JS_INJECTION = @"\<+[a-zA-Z/]";
        public static readonly string REGEX_UPLOAD_EXCEL = @".*\.(xls|xlsx|xlsm)?$";
        public static readonly string REGEX_UPLOAD_TXT_CSV = @".*\.(txt|TXT|csv|CSV)?$";
        public static readonly string REGEX_UPLOAD_XML = @".*\.(xml|XML)?$";
        public static readonly string REGEX_SPACE = @"[\s]";
        public static readonly string REGEX_NAME = @"^[a-zA-Z ]*$";
        public static readonly string REGEX_NUMERIC = @" ^[0-9]*$";
        public static readonly string REGEX_BIT = @"^[0-1]*$";
        public static readonly string REGEX_STUDENTNUMBER = @"^\d[0-9]{3}-\d[0-9]{4}-MN-\d[0-1]{0}$";

    }
}
