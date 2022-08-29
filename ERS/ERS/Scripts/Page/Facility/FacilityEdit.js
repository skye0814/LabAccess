$(document).ready(function () {
    moment.suppressDeprecationWarnings = true;
    var header = document.getElementById('header__logo');
    header.href = "Facility";
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>EDIT FACILITY</span>
                        </div>`;

    $.validator.addMethod("regex", function (value, element, regexp) {
        return this.optional(element) || regexp.test(value);
    },
        "Please check your input."
    );

    $('.timein').datetimepicker({
        format: 'hh:mm A'
    });
    $('.timeout').datetimepicker({
        format: 'hh:mm A',
    });

    var objFacilityEdit = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            // Facility List
            var param = {
                FacilityID: $('#FacilityID').val() + "",
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, listPageURL, true, "divPartialContent");
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');

                if ($('#frmFacility').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmFacility', 'submit');
                }
                return false;
            });

            $('#frmFacility').on('submit', function () {
                if ($('#frmFacility').valid()) {

                    Self.input = $('#frmFacility').serialize();

                    Self.SaveRecord(Self.input);
                }

                return false;
            });

            $('#btnSchedule').click(function () {
                $('#divErrorMessage').html('');
                var param = {
                    ScheduleDay: $('#ddlDay').val() + "",
                    TimeIn: $('#TimeIn').val(),
                    TimeOut: $('#TimeOut').val(),
                    SubjectCode: $('#SubjectCode').val(),
                    CourseName: $('#CourseName').val(),
                    FacilityID: $('#FacilityID').val(),
                };
                var dateOnly = moment(new Date()).format("M/D/YYYY");
                var errorList = '';
                if (param.ScheduleDay == '') {
                    errorList += "<strong>Day</strong> is empty.<br/>";
                }
                if (param.TimeIn == '') {
                    errorList += "<strong>Time In</strong> is empty.<br/>";
                }
                if (param.TimeOut == '') {
                    errorList += "<strong>Time Out</strong> is empty.<br/>";
                }
                if (param.SubjectCode == '') {
                    errorList += "<strong>Subject Code</strong> is empty.<br/>";
                }
                if (param.CourseName == '') {
                    errorList += "<strong>Subject Description</strong> is empty.<br/>";
                }
                if (!moment(dateOnly + " " + param.TimeIn).isBefore(moment(dateOnly + " " + param.TimeOut))) {
                    errorList += '<strong>Time In</strong> should not be equal or greater than <strong>Time Out</strong>';
                }
                else if (moment(dateOnly + " " + param.TimeIn).isBefore(dateOnly + " " + moment(param.TimeOut))) {
                    if (moment(param.TimeIn, 'hh:mm A').isBefore(moment('7:00 AM', 'hh:mm A')) || moment(param.TimeOut, 'hh:mm A').isAfter(moment('9:00 PM', 'hh:mm A'))) {
                        errorList += '<strong>Time In</strong> and <strong>Time Out</strong> should only be between laboratory hours (7:00 AM-9:00 PM)';
                    }
                }

                if (errorList == '') {
                    Self.AddSchedule(param);
                }
                else {
                    ModalAlert("LabAccess", errorList);
                    errorList = '';
                }
                
            });

        },
        InitializeFormValidation: function () {
            var Self = this;

            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = $('#frmFacility').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    RoomNumber: {
                        required: true,
                        regex: /CEA-\d[0-9]{2}$|cea-\d[0-9]{2}$/
                    },
                    RoomType: {
                        required: true
                    },
                },
                messages: {
                    RoomNumber: {
                        required: 'Room Number' + SUFF_REQUIRED,
                        regex: 'Invalid Room Number format! (CEA-123)'
                    },
                    RoomType: {
                        required: 'Room Type ' + SUFF_REQUIRED
                    },
                },
                invalidHandler: function () {
                    errorCount = validator.numberOfInvalids();
                },
                errorPlacement: function (error, element) {
                    ctr++;
                    if ($(error).text().indexOf('required') < 0) {
                        otherErrors.push('<label class=\'errMessage\'><li>' + $(error).text() + '</li></label>' + '<br />');
                    }
                    else {
                        requiredErrors.push('<label class=\'errMessage\'><li>' + $(error).text() + '</li></label>' + '<br />');
                    }

                    if (ctr == errorCount) {
                        if (requiredErrors.length > 1) {
                            requiredErrors = [];
                            requiredErrors.push('<label class=\'errMessage\'><li>' + REQ_HIGHLIGHTED_FIELDS + '</li></label>' + '<br />');
                        }

                        if (otherErrors.length != 0) {
                            $('#divErrorMessage').append(otherErrors);
                        }
                        if (requiredErrors.length != 0) {
                            $('#divErrorMessage').append(requiredErrors);
                        }
                        requiredErrors = [];
                        otherErrors = [];
                        ctr = 0;
                    }
                },
            });
        },
        fieldFormatting: function () {
            var Self = this;


        },
        SaveRecord: function (input) {
            var Self = this;
            $.ajax({
                url: saveFacilityURL,
                //type: 'POST',
                //data: JSON.stringify(input),
                //dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                type: "POST",
                data: input,
                dataType: "json",
                traditional: true,
                success: function (data) {
                    if (data.IsSuccess != true) {
                        var msg = '';

                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + '<br />';
                            }
                        }
                        else {
                            msg += data.Result;
                        }

                        ModalAlert(MODAL_HEADER, msg);

                    } else {
                        ModalAlert(MODAL_HEADER, MSG_SUCCESSFULLY_SAVED);
                        $('#divErrorMessage').html('');

                        $('#btnSave').prop('disabled', false);
                        $("input").prop("disabled", false);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },

        AddSchedule: function (input) {
            var Self = this;
            $.ajax({
                url: addScheduleURL,
                //type: 'POST',
                //data: JSON.stringify(input),
                //dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                type: "POST",
                data: input,
                dataType: "json",
                traditional: true,
                success: function (data) {
                    if (data.IsSuccess != true) {
                        var msg = '';

                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + '<br />';
                            }
                        }
                        else {
                            msg += data.Result;
                        }

                        ModalAlert(MODAL_HEADER, msg);

                    } else {
                        $('#divErrorMessage').html('');

                        $('#btnSave').prop('disabled', false);

                        var param = {
                            FacilityID: $('#FacilityID').val() + "",
                        };
                        Self.LoadTable(param);
                        ModalAlert("SCHEDULE ADDED", 'The schedule is successfully added to the list');
                        $("input").prop("disabled", false);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },
        // Schedule List
        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblScheduleList").jqGrid("GridUnload");
            $("#tblScheduleList").jqGrid("GridDestroy");

            $("#tblScheduleList").jqGrid({
                url: getScheduleListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["", "Delete", "Subject Code", "Subject Description", "Day", "Time In", "Time Out",  "Reserved Status"],
                colModel: [
                    { key: true, hidden: true, name: "ScheduleID" },
                    { name: "delete", index: "delete", width: 20, align: "center", sortable: false, formatter: Self.DeleteLink, frozen: true },
                    { key: false, width: 30, name: "SubjectCode", editable: true, align: "center", sortable: false },
                    { key: false, width: 30, name: "CourseName", editable: true, align: "center", sortable: false },
                    { key: false, width: 30, name: "ScheduleDay", index: "1", editable: true, align: "center", sortable: true , formatter: Self.DayString},
                    { key: false, width: 30, name: "TimeIn", index: "2", editable: true, align: "center", sortable: false },
                    { key: false, width: 30, name: "TimeOut", index: "3", editable: true, align: "center", sortable: false },
                    { key: false, width: 30, name: "ReservedStatus", index: "5", editable: true, align: "center", sortable: true, formatter: Self.isReservedString },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['Facility_RowNum']),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    row: "Show",
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No schedules for Room " + $('#RoomNumber').val(),
                multiselect: false,
                rowNumbers: true,
                page: parseInt(localStorage['Facility_PageNum']),
                sortorder: localStorage['Facility_SortOrder'],
                sortname: localStorage['Facility_SortColumn'],
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function () {
                    Self.fillTableLocalStorage();
                    Loading(false);
                },
                onPaging: function (pgButton) {
                    var newpage, last;
                    if ("user" == pgButton) {
                        newpage = parseInt($(this.p.toppager).find('input:text').val());
                        last = parseInt($(this).getGridParam("lastpage"));
                        if (newpage > last) {
                            return 'stop';
                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    var msg = "";

                    if (xhr.responseJSON.IsListResult == true) {
                        for (var i = 0; i < xhr.responseJSON.Result.length; i++) {
                            msg += xhr.responseJSON.Result[i] + "<br />";
                        }
                    } else {
                        msg += xhr.responseJSON.Result;
                    }

                    ModalAlert(MODAL_HEADER, msg);
                    Loading(false);
                }
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show </label>");
            $("#tblScheduleList").jqGrid("setFrozenColumns");
        },

        isReservedString: function (cellvalue, options, rowObject) {
            if (rowObject.ReservedStatus) {
                return "Reserved";
            }
            else
                return "Unreserved";
        },
        DayString: function (cellvalue, options, rowObject) {
            switch (rowObject.ScheduleDay) {
                case 0:
                    return "Monday";
                    break;
                case 1:
                    return "Tuesday";
                    break;
                case 2:
                    return "Wednesday";
                    break;
                case 3:
                    return "Thursday";
                    break;
                case 4:
                    return "Friday";
                    break;
                case 5:
                    return "Saturday";
                    break;

            }
        },
        DeleteLink: function (cellvalue, options, rowObject) {
            var Self = this
            return "<a class=\"glyphicon glyphicon-remove-sign\" onclick=\"return DeleteSchedule('" + rowObject.ScheduleID + "', '" + $('#FacilityID').val() + "' );\"></a>";
        },
        

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblScheduleList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblScheduleList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblScheduleList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['Facility_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['Facility_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['Facility_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['Facility_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },
    }
    facilityEdit = Object.create(objFacilityEdit);
    facilityEdit.initialized();
});

//function DeleteSchedule(ScheduleID, FacilityID) {
//    var Self = this;
    
//}


function DeleteSchedule(ScheduleID, FacilityID) {
    var Self = this;
    $.ajax({
        url: window.deleteScheduleURL,
        type: "POST",
        data: { ScheduleID: ScheduleID },
        dataType: "json",
        success: function (data) {
            if (data.IsSuccess != true) {
                var msg = '';

                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + '<br />';
                    }
                }
                else {
                    msg += data.Result;
                }
                ModalAlert(MODAL_HEADER, msg);
            } else {
                LoadPartial(editPageURL + "?FacilityID=" + FacilityID, 'divPartialContent');
                //LoadPartial(window.addPageURL,'divPartialContent');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        },
    });
}
