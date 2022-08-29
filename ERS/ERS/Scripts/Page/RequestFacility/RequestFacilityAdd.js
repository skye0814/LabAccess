$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = mainPageURL;
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>ADD REQUEST</span>
                        </div>`;

    var dateToday = new Date();
    dateToday.setDate(dateToday.getDate());

    $(function () {
        $('#StartTime').datetimepicker({
            format: "M/D/YYYY",
            icons: {
                today: 'glyphicon glyphicon-map-marker'
            },
            sideBySide: true,
        });

        $('#VacantStart').datetimepicker({
            format: "hh:mm A",
            icons: {
                today: 'glyphicon glyphicon-map-marker'
            },
            sideBySide: true,
        });

        $('#VacantEnd').datetimepicker({
            format: "hh:mm A",
            icons: {
                today: 'glyphicon glyphicon-map-marker'
            },
            sideBySide: true,
        });

        $("#StartTime").on("dp.change", function (e) {
            /*$('#EndTime').removeAttr('disabled');*/
            /*$('#StartTime').data("DateTimePicker").minDate(dateToday.toLocaleString("en-US", {}).split(",").join(" "));*/
            /*$('#EndTime').data('DateTimePicker').minDate(moment(e.date).add(30, 'minutes'));*/

            $('#StartTime').data("DateTimePicker").minDate(moment(new Date()).format("M/D/YYYY"));
            /*$('#EndTime').data('DateTimePicker').minDate(moment(e.date).format("M/D/YYYY"));*/

            // Start time options
            $('#StartTime').data("DateTimePicker").daysOfWeekDisabled([0]);
            //$('#StartTime').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]);

            // End time options
            /*$('#EndTime').data("DateTimePicker").daysOfWeekDisabled([0]);*/
            //$('#EndTime').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]);
        });

    });

    // on change functions
    $('#StartTime').on('dp.change', function () {
        $('#requests-conflict tr').remove();
        $('.vacant-time').css('display', 'none');
        $('#VacantStart').val("");
        $('#VacantEnd').val("");
        $('#ddlFacility').val("");
        $('#Schedule').val("");
        $('#Schedule option').remove();
        $('#Schedule').append('<option value> ' + 'Select Schedule' + ' </option>');
        $('#Schedule').append('<option value="vacant"> ' + 'Pick vacant time outside class hours' + ' </option>');
        $('.input100-select').css('display', 'block');

    });
    $('#ddlFacility').on("change", function () {
        if ($('#ddlFacility').val() != "") {
            GetSchedulesByFacilityAndBorrowDate();
        }
        $('.styled-table').css('display', 'none');
        $('.vacant-time').css('display', 'none');
        $('#requests-conflict tr').remove();
        $('#VacantStart').val("");
        $('#VacantEnd').val("");
        $('#Schedule').val("");
        GetSchedulesByFacilityAndBorrowDate();
        
    });
    $('#Schedule').on('change', function () {
        if ($(this).val() == "vacant") {
            $('.vacant-time').css('display', 'block');
            $('.styled-table').css('display', 'block');
        }
        else {
            $('.styled-table').css('display', 'none');
            $('.vacant-time').css('display', 'none');
        }
    });
    $('#VacantStart').on('dp.change', function () {
        $('#requests-conflict tr').remove();
        GetConflictingRequestsforVacantTime();
    });
    $('#VacantEnd').on('dp.change', function () {
        $('#requests-conflict tr').remove();
        GetConflictingRequestsforVacantTime();
    });

    function GetReservedClassSchedulesByDate() {
        $.ajax({
            url: 'RequestFacility/GetReservedClassSchedulesByDate',
            type: "POST",
            data: {
                StartTime: $('#StartTime').val()
            },
            dataType: "json",
            traditional: false,
            success: function (data) {
                if (data.length == 0) {
                }
                else {
                    for (var i = 0; i < data.length; i++) {
                        $('#Schedule option[value=' + data[i].Schedule + ']').append(" --RESERVED--");
                        $('#Schedule option[value=' + data[i].Schedule + ']').attr('disabled', 'disabled');
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Loading(false);
                ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
            },
        });
    }

    function GetSchedulesByFacilityAndBorrowDate() {
        $.ajax({
            url: 'RequestFacility/GetSchedulesByFacilityAndBorrowDate',
            type: "POST",
            data: {
                BorrowDate: $('#StartTime').val(),
                FacilityID: $('#ddlFacility').val()
            },
            dataType: "json",
            traditional: false,
            success: function (data) {
                if (data.length == 0) {
                    $('#Schedule option').remove();
                    $('#Schedule').append('<option value> ' + 'Select Schedule' + ' </option>');
                    $('#Schedule').append('<option value="vacant"> ' + 'Pick vacant time outside class hours' + ' </option>');
                }
                else {
                    $('#Schedule option').remove();
                    $('#Schedule').append('<option value> ' + 'Select Schedule' + ' </option>');

                    for (var i = 0; i < data.length; i++) {
                        $('#Schedule').append('<option value=' + data[i].ScheduleID + '>' + data[i].CourseName + ' ' + '(' + data[i].TimeIn + '-' + data[i].TimeOut + ')' + '</option>');
                    }

                    $('#Schedule').append('<option value="vacant"> ' + 'Pick vacant time outside class hours' + ' </option>');
                    GetReservedClassSchedulesByDate();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Loading(false);
                ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
            },
        });
    }

    function GetConflictingRequestsforVacantTime() {
        $.ajax({
            url: 'RequestFacility/GetConflictingRequestsforVacantTime',
            type: "POST",
            data: {
                StartTime: $('#StartTime').val() + " " + $('#VacantStart').val(),
                EndTime: $('#StartTime').val() + " " + $('#VacantEnd').val(),
                FacilityID: $('#ddlFacility').val()
             },
            dataType: "json",
            traditional: false,
            success: function (data) {
                var tablerow = document.getElementById('requests-conflict');
                if (data.length == 0) {
                    tablerow.innerHTML += "<tr><td>No reserved schedules</td><td>N/A</td></tr>";
                }
                else {
                    for (var i = 0; i < data.length; i++) {
                        tablerow.innerHTML += "<tr><td>" + data[i].CourseName + "</td><td>" + data[i].TimeIn + "-" + data[i].TimeOut + "</td></tr>";
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Loading(false);
                ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
            },
        });
    }

    function GenerateRequestFacilityQRCode(RequestFacilityGUID, FacilityID) {

        $.ajax({
            url: window.generateQRCodeURL,
            type: "POST",
            data: JSON.stringify({ RequestFacilityGUID: RequestFacilityGUID }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.IsSuccess != true) {
                    var msg = "";

                    if (data.IsListResult == true) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                    else {
                        msg += data.Result;
                    }

                    ModalAlert(MODAL_HEADER, msg);
                }
                else {

                    IsEdit = false;

                    var qrimage = $('#qrcode-image').val("<img src='" + data.Result + "'/>");
                    var qrlink = $('#qrcode-link').val(data.Result);
                    var filename = $('#qrcode-filename').val(RequestFacilityGUID + '_' + FacilityID + '.png');
                    ModalAlertShowQRCode(MODAL_HEADER_SUCCESSFULLY_SUBMITTED, MSG_SUCCESSFULLY_SAVED_QRCODE, $('#qrcode-image').val(), $('#qrcode-link').val(), $('#qrcode-filename').val(), RequestFacilityGUID);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Loading(false);
                ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
            },
            beforeSend: function () {
                console.log('loadingQR');
            },
            complete: function () {
                console.log('completeQR');
            }
        });

        return false;
    }

    //function GetFacilityByRequestDate() {
    //    $.ajax({
    //        type: 'GET',
    //        url: getFacilityByRequestDate,
    //        data: {
    //            StartTime: /*$("#BorrowDate").val() + " " + $("#FirstTime").val()*/ $("#StartTime").val(),
    //            EndTime: /*$("#BorrowDate").val() + " " + $("#SecondTime").val()*/ $("#EndTime").val()
    //        },
    //        dataType: 'json',
    //        traditional: false,
    //        success: function (data) {
    //            if (data.length == 0) {
    //                $('#ddlFacility option').remove();
    //                var defaultFacility = document.getElementsByClassName("defaultFacility");
    //                var defaultFacilityValue = document.getElementsByClassName("defaultFacilityValue");
    //                var defaultFacilityText = document.getElementsByClassName("defaultFacilityText");
                    
    //                $('#ddlFacility').append('<option value> ' + 'Select Facility' + ' </option>')

    //                for (var i = 0; i < defaultFacility.length; i++) {
    //                    $('#ddlFacility').append('<option value = ' + defaultFacilityValue[i].innerHTML + '> ' + defaultFacilityText[i].innerHTML + ' </option>')
    //                }
    //            }
    //            else if (data.length > 0) {
    //                for (var i = 0; i < data.length; i++) {
    //                    $('#ddlFacility option[value = ' + data[i].FacilityID + ']').remove();
    //                }
    //            }
                
    //        },
    //        error: function (error) {
    //            alert(error);
    //        },
    //        beforeSend: function(){
    //            // console.log('Retrieving available rooms');
    //            $('.spinner').css('display', 'block');
    //            $('.input100-select').css('display', 'none');
    //            $('.form-controls').css('visibility', 'hidden');
    //        },
    //        complete: function(){
    //            // console.log('Retrieve success!');
    //            $('.spinner').css('display', 'none');
    //            $('.input100-select').css('display', 'block');
    //            $('.form-controls').css('visibility', 'visible');
    //        }
    //    });
    //}

    //$("#StartTime").on("dp.change", function () {
    //    $('#ddlFacility').val("");
    //    GetFacilityByRequestDate();
    //});

    //$("#EndTime").on("dp.change", function () {
    //    $('#ddlFacility').val("");
    //    $('.input100-select').css('display', 'block');
    //    GetFacilityByRequestDate();
    //});

   

    var objRequestFacilityAdd = {
        initialized: function () {
            var Self = this;

            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#FacilityID').focus() }, 500);

            
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER_RETURN, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');

                if ($('#frmFacilityRequest').valid()) {

                    var isAllValid = true;

                    if ($('#Schedule').val() == "vacant") {
                        if ($('#VacantStart').val() == "") {
                            isAllValid = false;
                            $('#divErrorMessage').append("<strong>Start Time is required.</strong><br/>");
                        }
                        if ($('#VacantEnd').val() == "") {
                            isAllValid = false;
                            $('#divErrorMessage').append("<strong>End Time is required.</strong><br/>");
                        }
                        if ($('#VacantStart').val() != "" && $('#VacantEnd').val() != "") {
                            var dateMin = moment($('#StartTime').val() + " 7:00 AM");
                            var dateMax = moment($('#StartTime').val() + " 9:00 PM");
                            var vacantStart = moment($('#StartTime').val() + " " + $('#VacantStart').val());
                            var vacantEnd = moment($('#StartTime').val() + " " + $('#VacantEnd').val());

                            if (vacantStart < dateMin || vacantEnd > dateMax) {
                                isAllValid = false;
                                ModalAlert("REQUEST FAILED", "<strong>Start Time</strong> and <strong>End Time</strong> should only be between laboratory hours (7:00 AM - 9:00 PM)");
                            }
                            else {
                                if (vacantStart > vacantEnd) {
                                    isAllValid = false;
                                    ModalAlert("REQUEST FAILED", "<strong>Start Time</strong> cannot be greater than <strong>End Time</strong>");
                                }
                            }
                        }
                    }

                    if (isAllValid) {
                        ModalConfirmation(MODAL_HEADER_SUBMIT, MSG_CONFIRM_SAVE, 'frmFacilityRequest', 'submit');
                    }
                    
                }
                return false;
            });

            function goBack() {
                window.history.back(-1);
                setTimeout(() => {
                    location.reload();
                }, 0);
            }

            $('#frmFacilityRequest').on('submit', function () {
                if ($('#frmFacilityRequest').valid()) {

                    Self.input = $('#frmFacilityRequest').serialize();
                    Self.SaveRecord(Self.input);

                    //// Check dates if equal
                    //var dateOnlyStartDate = $('#StartTime').val().split(" ");
                    //var dateOnlyEndDate = $('#EndTime').val().split(" ");

                    //if (dateOnlyStartDate[0] != dateOnlyEndDate[0]) {
                    //    if ($('#StartTime').val() != "" && $('#EndTime').val() != "") {
                    //        ModalAlert("REQUEST FAILED", "<strong>Start Date</strong> and <strong>End Date</strong> should be the same");
                    //    }
                    //}
                    //else {
                    //    var dateMin = moment(dateOnlyStartDate[0] + " 7:00 AM");
                    //    var dateMax = moment(dateOnlyStartDate[0] + " 9:00 PM");
                    //    var startTime = moment($('#StartTime').val());
                    //    var endTime = moment($('#EndTime').val());

                    //    if ((startTime < dateMin) || (endTime > dateMax)) {
                    //        ModalAlert("REQUEST FAILED", "<strong>Start Time</strong> and <strong>End Time</strong> should only be between laboratory hours (7:00 AM - 9:00 PM)");
                    //    }
                    //    else {
                    //        Self.input = $('#frmFacilityRequest').serialize();
                    //        Self.SaveRecord(Self.input);
                    //    }
                    //}
                }

                return false;
            });

        },
        InitializeFormValidation: function () {
            var Self = this;

            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = $('#frmFacilityRequest').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    FacilityID: {
                        required: true
                    },
                    StartTime:
                    {
                        required: true
                    },
                    EndTime:
                    {
                        required: true
                    },
                    Schedule: {
                        required: true
                    }
                    //BorrowDate: {
                    //    required: true
                    //},
                    //FirstTime: {
                    //    required: true
                    //},
                    //SecondTime: {
                    //    required: true
                    //}
                },
                messages: {
                    FacilityID: {
                        required: 'Facility' + SUFF_REQUIRED
                    },
                    StartTime:
                    {
                        required: 'Borrow Date' + SUFF_REQUIRED
                    },
                    EndTime:
                    {
                        required: 'End Date and Time' + SUFF_REQUIRED
                    },
                    Schedule:
                    {
                        required: 'Schedule' + SUFF_REQUIRED
                    }
                    //BorrowDate: {
                    //    required: 'Borrow Date' + SUFF_REQUIRED
                    //},
                    //FirstTime: {
                    //    required: 'Start Time' + SUFF_REQUIRED
                    //},
                    //SecondTime: {
                    //    required: 'End Time' + SUFF_REQUIRED
                    //}
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
            /*console.log(input.FacilityID);*/
            $.ajax({
                url: saveRequestFacilityURL,
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

                        ModalAlert("REQUEST ABORTED", msg);

                    } else {
                        var input = {
                            RequestFacilityGUID: $('#RequestFacilityGUID').val(),
                            FacilityID: $('#ddlFacility').val()
                        }
                        GenerateRequestFacilityQRCode(input.RequestFacilityGUID, input.FacilityID);


                        $('#divErrorMessage').html('');
                        $('#btnSave').prop('disabled', false);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
                beforeSend: function () {
                    $('#btnConfirmationModal').html('<img src="\../../Content/Images/spinner.svg"\ style="\width: 20px"\> PROCESSING');
                    $('#btnConfirmationModal').css('background-color', '#FFBF86 !important');
                    $('#btnConfirmationModal').attr('disabled', 'disabled');
                    console.log('loadingSubmit');
                },
                complete: function () {
                    console.log('completeSubmit');
                }
            });
        },


    }
    requestFacilityAdd = Object.create(objRequestFacilityAdd);
    requestFacilityAdd.initialized();
});
