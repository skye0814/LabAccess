using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSUtil
{
    public static class MessageUTIL
    {
        public static string PRE_PROVIDE_INPUT = "Please provide valid input for ";

        public static string PRO_VALID_VAL = "Provide valid ";

        public static string SUFF_INCORRECT = " is incorrect.";

        public static string ACCT_INACTIVE = "Account is inactive.";

        public static string PROVIDE_VALID_FILTER = "Provide valid filter for ";

        public static string NO_RECORD = "No selected record/s.";
        public static string SUFF_SUCESS_DELETE = " record/s successfully deleted permanently.";
        public static string SUFF_SUCESS_ARCHIVE = " record/s successfully removed.";
        public static string NO_RECORD_DELETED = "No record/s deleted.";
        public static string NO_RECORD_ARCHIVED = "A record was unable to be removed.";

        #region  Error Messages
        public static readonly string SUFF_ERRMSG_INPUT_REQUIRED = "is required.";
        public static readonly string ERROR_PDF_FILE = "An error has occurred while processing uploaded file. Please valid file.";
        public static readonly string ERROR_EXCEL_FILE = "An error has occurred while processing uploaded file. Please use the provided template by clicking the download link.";
        public static readonly string ERRMSG_INVALID_FILE = "File format is invalid.";
        #endregion

        #region Success message
        public static readonly string SCSSMSG_REC_SAVE = "Record successfully saved.";
        public static readonly string SCSSMSG_REC_UPDATE = "Record successfully saved.";
        public static readonly string SCSSMSG_REC_DELETE = "Record(s) successfully deleted.";
        public static readonly string SCSSMSG_REC_CANCEL = "Successfully cancelled record(s).";
        public static readonly string SCSSMSG_REC_DISABLE = "Successfully disabled record(s).";
        public static readonly string SCSSMSG_REC_PROCESS = "Successfully processed record(s).";
        public static readonly string SCSSMSG_REC_FILE_UPLOAD = "Successfully uploaded.";
        public static readonly string SCSSMSG_REC_REJECT = "Successfully rejected record(s).";
        public static readonly string SCSSMSG_REC_RECALL = "Successfully recalled record(s).";
        public static readonly string SCSSMSG_REC_APPROVE = "Successfully approved record(s).";
        #endregion
    }
}
