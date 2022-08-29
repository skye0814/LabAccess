
//Messages
var MSG_CONFIRM_SAVE = "Are you sure you want to save record?";
var MSG_CONFIRM_PROCEED = "Are you sure you want to proceed?";
var MSG_CONFIRM_BACK = "Are you sure you want to leave this page?";
var MSG_CONFIRM_CANCEL = "Are you sure you want to cancel?";
var MSG_CONFIRM_CANCELREQUEST = "Are you sure you want to cancel the request? This action cannot be undone.";
var MSG_CONFIRM_DENYREQUEST = "Are you sure you want to deny this request? The requested equipment items will be unreserved.";
var MSG_CONFIRM_DENYREQUESTFACILITY = "Are you sure you want to deny this request? The requested facility will be unreserved.";
var MSG_CONFIRM_DELETE = "Are you sure you want to permanently delete record(s)? This action cannot be undone.";
var MSG_CONFIRM_ARCHIVE = "Are you sure you want to remove record(s)? Removed record(s) will be moved to Archive."
var MSG_CONFIRM_DEACTIVATE = "Are you sure you want to deactivate record(s)?";
var MSG_CONFIRM_DELETE_SINGLE = "Are you sure you want to delete record?";

var SUFF_PRE_MSG_CONFIRM_DELETE = " , are you sure you want to delete record(s)?";

var MSG_PAYROLL_PROCESS_FINALIZED = "Payroll Process already finalized";
var MSG_PAYROLL_PROCESS_POSTED = "Payroll Process already posted";

var MSG_CONFIRM_SUCCESS_GATHER = "Record(s) successfully gathered. Are you sure you want to proceed?"

var MSG_CONFIRM_ADD_DETAILS = "Are you want to add payroll details?";
var MSG_CONFIRM_PROCESS_PAY = "Are you sure you want to process payroll?";
var MSG_CONFIRM_REPROCESS_PAY = "Are you sure you want to reprocess payroll?";
var MSG_CONFIRM_CONTINUE_CANCEL = "Change(s) won't be saved. Are you sure you want to cancel?";
var MSG_CONFIRM_NEXT = "Are you sure you want to go to next page?";
var MSG_CONFIRM_TAB_BACK = "Are you sure you want to go back?";
var MSG_CONFIRM_EDATE = "Current Pag-IBIG record being used will be inactivated. Are you sure you want to save record?";
var MSG_CONFIRM_PHILHEALTH_EDATE = "Current PhilHealth record being used will be inactivated. Are you sure you want to save record?";
var MSG_CONFIRM_SSS_EDATE = "Current SSS record being used will be inactivated. Are you sure you want to save record?";
var MSG_CONFIRM_TAX_EDATE = "Current Tax record being used will be inactivated. Are you sure you want to save record?";
var MSG_CONFIRM_WAGE_EDATE = "Current Wage Order record being used will be inactivated. Are you sure you want to save record?";
var MSG_CONFIRM_TAXCODE_EDATE = "The current Tax Code record being used will be disabled. Are you sure you want to save record?";
var MSG_SUCCESSFULLY_SAVED = "Record successfully saved.";
var MSG_SUCCESSFULLY_SAVED_QRCODE = "Request successfully submitted. Show this QR Code to the Lab Personnel to claim your items";
var MSG_SUCCESSFULLY_CANCELLED = "The request was successfully cancelled.";
var MSG_SUCCESSFULLY_DENIED = "The request was successfully denied.";
var MSG_SUCCESSFULLY_SAVED_S = "Records successfully saved.";
var MSG_SUCCESSFULLY_PROCCESSED = "Payroll successfully processed.";
var MSG_SUCCESSFULLY_REPROCCESSED = "Payroll successfully reprocessed.";
var MSG_SUCCESSULLY_CHANGED_PASSWORD = "Password successfully changed. Please re-login."
var REQ_HIGHLIGHTED_FIELDS = "Highlighted fields are required.";
var DUPLICATE_HIGHLIGHTED_FIELDS = "Highlighted fields are duplicate.";
var MSG_CONFIRM_ACTIVE = "Record already in used.";
var MSG_CONFIRM_DISABLED = "Record can no longer be edited.";
var MSG_REC_EXIST = "Unable to add new record as a new rate is already available.";
var MSG_AUTO_ACTIVE = "Record is automatically set to Active. Changes will be applied once saved.";
var EXCEL_FILE_SIZE = "File cannot be greater than 25MB.";
var NO_RECORD_UPLOAD = "No record(s) was gathered from the uploaded file.";
var REC_DUPLICATING_VALUES = "Selected payroll Type already in use."
var MSG_TEMP_UNAVAILABLE = "Temporarily unavailable.";
var MSG_NO_EXPORT = "No record(s) found.";
var SELECT_LOAN = "Must select atleast one loan.";
var SELECT_EARNING = "Must select atleast one earning.";
var SELECT_DEDUCTION = "Must select atleast one deduction.";
var SELECT_LEAVE = "Must select atleast one leave."; //Revised "Must select atleast one overtime.";
var SELECT_CYCLE = "Must select atleast one payroll cycle.";
var SELECT_OVERTIME = "Must select atleast one overtime.";
var SELECT_OVERTIMEPREMIUM = "Must select atleast one overtime with premium.";
var INV_OTANDOTPREMIUM = "Overtime and Overtime w/ premium must be unique.";
var SELECT_EXCEL_FILE = "Please select an .xls, .xlsx, or .xlsm file.";//Revised "Please select an .xls or .xlsx file.";
var NO_RECORD_SAVED = "No new record(s) saved.";
var NO_RECORD_DISPLAY = "No record(s) to display.";
var NO_RECORD_FOUND = "No record(s) found."
var CHECK_INPUT = "Please check your input.";
var UPLOAD_NO_RECORD = "No record(s) was gathered from the uploaded file.";
var INV_HIGHLIGHTED_FIELDS = "Highlighted fields are invalid.";
var MSG_CONFIRM_SAVE_PASS = "The system will logout requiring you to login using your newly created password. Are you sure you want to change your password?";
var MSG_DISCARD_RECORD = "Are you sure you want to discard changes?";
var MSG_CONFIRM_EXPORT_CHINAPAY = "Are you sure you want to export record(s) to ChinaPay text file?";
var MSG_CONFIRM_DELETE_FILTERED = "Are you sure you want to delete all filtered record(s)?";
var MSG_CONFIRM_CLEAR = "Are you sure you want to clear record(s)?";
var MSG_CONFIRM_CONTINUE = "Are you sure you want to continue?";
var MSG_CONFIRM_DELETE_REC = "Are you sure you want to delete record?";
var MSG_SUCCESS_DELETE_REC = "Record successfully deleted.";
var MSG_SUCCESS_SAVED = "Record successfully saved.";
//Suffix
var SUFF_REQUIRED = " is required."; // for InitializeFormValidation ex. "Code" + SUFF_REQUIRED
var SUFF_INCORRECT_FORMAT = " is in incorrect format.";
var SUFF_INVALID = " is invalid.";
var SUFF_NOT_EXIST = " does not exists.";
var SUFF_ALREADY_ASSIGNED = " are already assigned.";
var SUFF_EXCEED_CURRENT_DATE = " must not exceed current date.";
var SUFF_ALREADY_ADDED = " is already added.";
var GREATERTHAN_ZERO = "{0} must be greater than 0.";
var SUFF_EXCEED_HIRED_DATE = " must not exceed Personnel hired date.";
var MSG_CONFIRM_SAVE_ADJ_OE = "Your adjustment is set to be deduction.Are you sure you want to save record?";
var MSG_CONFIRM_SAVE_ADJ_OD = "Your adjustment is set to be earning.Are you sure you want to save record?";

//Prefix
var PREF_SELECT = "Please select a " // for Selection of Dropdown list ex. PREF_SELECT + "Payroll Group."
var PREF_INVALID = "Invalid ";
var PREF_SELECT_ONE = "Select atleast one ";
var PREF_NO_AVAILABLE = "No available ";
var PREF_PROVIDE = "Please provide a valid ";

//Modal Header
var MODAL_HEADER = "LabAccess";
var MODAL_HEADER_CANCELREQUEST = "CANCEL REQUEST";
var MODAL_HEADER_DENYREQUEST = "DENY REQUEST";
var MODAL_HEADER_ARCHIVE = "REMOVE RECORD";
var MODAL_HEADER_DELETE = "PERMANENTLY DELETE";
var MODAL_HEADER_RETURN = "RETURN";
var MODAL_HEADER_SUBMIT = "SUBMIT"
var MODAL_HEADER_SUCCESSFULLY_CANCELLED = "REQUEST CANCELLED";
var MODAL_HEADER_SUCCESSFULLY_DENIED = "REQUEST DENIED";
var MODAL_HEADER_SUCCESSFULLY_SUBMITTED = "REQUEST SUBMITTED";

var REC_DUPLICATING_CODE = "Selected Code already in use.";
var MSG_CONFIRM_POST = "Are you sure you want to post record(s)?";
var MSG_CONFIRM_UNPOST = "Are you sure you want to unpost record(s)?";
var MSG_CONFIRM_FINALIZE = "Are you sure you want to finalize record(s)?";

var AMOUNT_RANGE = "Amount should be between Minimum and Maximum Amount.";

//Payroll Register
var MSG_SELECT_PAYROLL_PROCESS = "Select a payroll process.";
var MSG_SELECT_ONE_PAYROLL_PROCESS = "Select one payroll process only.";

//DTR
var INVALID_DAYS = "Summation of Ordinary workday, worked holiday and absent must not exceed the required working days.";
var INVALID_LEAVE = "Total Leave Applied must be equal or less than the Total Absent Days.";
var INVALID_LEAVEBAL = "Leave must not be more than Leave Balance.";
var MSG_CONFIRM_GENERATE = "Are you sure you want to generate record(s)?";
var MSG_SUCCESSFULLY_GENERATE = "DTR successfully generated.";

//Comparisons
var COMPARE_GREATER_EQUAL = " must be greater than or equal to ";
var COMPARE_GREATER = " must be greater than ";
var COMPARE_EARLIER = " cannot be earlier than ";
var COMPARE_BETWEEN = " must be between ";
var COMPARE_EQUAL_LESS = " must be less than or equal to ";
var COMPARE_LESS = " must be less than ";
var COMPARE_SET = " must be set to ";
var COMPARE_DOES_NOT_MATCH = " does not match with ";
//MonthPay
var MSG_CONFIRM_RESET = "Are you sure you want to recompute record(s)?";
var MSG_CONFIRM_GENERATE_MP = "Are you sure you want to generate 13th Month Pay?";
var MSG_SUCCESSFULLY_GENERATEMONTHPAY = "Successfully generated 13th month pay.";
var MSG_SUCCESSFULLY_RESETMONTHPAY = "Successfully reset record(s).";
var SELECT_YEAR = "Must select year.";
var SELECT_PAYROLLGROUP = "Must select atleast one payroll group.";
var SELECT_PERSONNEL = "Must select atleast one personnel.";

//Annualization
var MSG_CONFIRM_ANNUALIZE = "Are you sure you want to compute for annualization?";
var MSG_SUCCESSFULLY_ANNUALIZE = "Finished annualization process.";

//Leave Convert
var MSG_CONFIRM_CONVERTLEAVE = "Are you sure you want to convert this leave?";
var MSG_SUCCESSFULLY_CONVERTLEAVE = "Successfully converted leave.";
var MSG_CONFIRM_HOLDLEAVE = "Are you sure you want to hold this leave?";
var MSG_CONFIRM_ACTIVATELEAVE = "Are you sure you want to activate this leave?";


//Save adjustments
var MSG_CONFIRM_SAVE_AOE = "Other earning will reflect as deduction.";
var MSG_CONFIRM_SAVE_AOD = "Other deduction will reflect as earning.";
var MSG_CONFIRM_SAVE_LOAN = "Loan will reflect as earning.";

// PAYROLLITEMCATEGORY
var MSG_ONE_CHECKED_CATEGORY = "Can only check one (1) payroll item category.";
var MSG_NO_CHECKED_CATEGORY = "Check one (1) payroll item category.";

var MSG_CHKCATEGORY_TOOLTIP = "Check to indicate that selected unassigned payroll item will be included to this Payroll Item Category.";
var MSG_EDIT_TOOLTIP = "Click to edit payroll item category.";
var MSG_DELETE_TOOLTIP = "Click to delete payroll item category.";
var MSG_UNASSIGNED_TOOLTIP = "Drag to designated Payroll Item Category or double click to assign on checked Payroll Item Category.";
var MSG_ASSIGNED_TOOLTIP = "Double click or drag to list of Unassigned Payroll Items to remove from category. Drag to designated Payroll Item Category to transfer item.";

var MSG_CONFIRM_ASSIGN = "Are you sure you want to assign this payroll item?";
var MSG_CONFIRM_REMOVE = "Are you sure you want to remove the payroll item?";
var MSG_CONFIRM_TRANSFER = "Are you sure you want to transfer the payroll item to a different category?";

var MSG_CONFIRM_RQ_SAVE = "Are you sure you want to save request?";
var MSG_CONFIRM_RQ_SAVE_PAYROLL_PROFILE = "Once the request was approved, personnel's existing unprocessed DTR Summary will be deleted and need to be recreated or re-uploaded. Are you sure you want to save request?";
var MSG_CONFIRM_PG_SAVE = "Payroll Group(s) using the same rate setup will also be updated.  Are you sure you want to save record?";

var MSG_MULTILEVEL_APPROVE = "Select only 1 record when the Multilevel Request is not for Final Approval."