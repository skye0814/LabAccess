$(document).ready(function () {
    var pathName = window.location.pathname;
    if (pathName == "/Login" ||
        pathName == "/") {
        window.history.forward();
    }
});

// Add Comma to numbers. To use :: varname.withComma
String.prototype.withComma = function (value) { return this.replace(/\B(?=(\d{3})+(?!\d))/g, ","); };

// Remove Comma to numbers. To use :: varname.noComma
String.prototype.noComma = function (value) { return this.replace(/,/g, ""); };

// Date converter of ASP.NET
String.prototype.convertDateFromASP = function () {
    var dte = eval("new " + this.replace(/\//g, '') + ";");
    dte = (dte.getDate()) + "-" + (dte.getMonth() + 1) + "-" + (dte.getFullYear());

    return dte;
}

// Adds decimal zeroes to end of the number.
// 1 = 1.00 | 1.1 = 1.10 | 1.11 = 1.11 | 1.567 = 1.57
function AddZeroes(num) {
    num = num.noComma();
    var value = parseFloat(num).toFixed(2);
    var res = value.toString().split(".");
    if (num.indexOf('.') === -1) {
        num = value.toString();
    } else if (res[1] != undefined) {
        if (res[1].length < 3) {
            num = value.toString();
        }
    } else {
        num = '0.00'
    }

    if (isNaN(num)) {
        num = '0.00'
    }

    return num;
}

// Set text change for decimal number
function AmountTextChange(e) {
    //If text box is 0.00 set value to blank
    e.focus(function () {
        if (parseInt($(this).val()) == 0) {
            $(this).val("");
        }

        $(this).val($(this).val().withComma());
    });
    //If text box is empty set value to 0.00
    e.blur(function () {
        if ($(this).val() != "") {
            $(this).val(AddZeroes($(this).val()).withComma());
        }
    });
}

//Accept only numbers, commas and dots
function AmountOnly(txtbox) {
    txtbox.keydown(function (e) {

        if (e.shiftKey && (e.keyCode == 188 || e.keyCode == 190)) {
            e.preventDefault();
        }
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 188, 190]) !== -1 ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            (e.keyCode == 67 && e.ctrlKey === true) ||
            (e.keyCode == 88 && e.ctrlKey === true) ||
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}

// Remove Space
function NoSpace(txtbox) {
    txtbox.keydown(function (e) {
        if (e.keyCode == 32) {
            e.preventDefault();
        }
    });
}

// Number only
function NumberOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Letters, Numbers and Hyphen
function LetterNumberHyphenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([a-z]|[0-9]|[-])/ig;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Letters, Space, Hypen only (Added by Charles 03/16/2017)
function LetterHyphenSpaceOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([a-z]|[-\s.])/ig;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// No special characters (Added by Charles 03/16/2017)
function InvalidSpecialChar(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([@!#$%^&*|?"'])/ig;
        if (("" + reg.test(e.key)) == "true" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// For Addresses  (Added by Kevin 09/28/2020)
function Address(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([a-z]|[0-9]|[ ()#,-.;'&])/ig;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}


//Number and Hypen Only (Added by Charles 03/16/2017)
function NumberHyphenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([-]|[0-9])/ig;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

//User Define Expression  
///txtbox  - (e.g. $('#txtItem15PrintName'))
// reg     - (e.g. /([a-z]|[-])/ig) //Regular Expression
//Example Usage:
//              UserDefineRegEx($('#txtItem15PrintName'), /([a-z]|[-])/ig);
function UserDefineRegEx(txtbox, reg) {
    txtbox.keydown(function (e) {
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
} //Added by Charles 03/10/2017

//Number and Hypen Only (Added by Charles 03/16/2017)
function NumberHypenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([-]|[0-9])/ig;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Allows Number and + only
function TelNoOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9]|[+])$/g;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Submit action using Ajax call
// url - URL of the function (Required)
// type - Indicate if POST or GET (Required)
// param - Parameters to be pass (Optional)
// button - Name of button to be disabled while waiting for the server response (Optional)
function AjaxSubmit(url, type, param, button) {
    if (param == undefined)
        param = "";

    Loading(true);

    $("#" + button + "").prop("disabled", true);

    $.ajax({
        url: url,
        type: "POST",
        data: param,
        //dataType: "json",
        //contentType: "application/json; charset=utf-8",
        success: function (data) {
            var msg = "";
            if (data.IsListResult == true) {
                for (var i = 0; i < data.Result.length; i++) {
                    msg += data.Result[i] + "<br />";
                }
            } else {
                msg += data.Result;
            }

            Loading(false);
            ModalAlert("ERS", msg, "");

            $("#" + button + "").prop('disabled', false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
            $("#" + button + "").prop("disabled", false);
        },
    });
}

// Alert using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlertShowQRCode(header, body, qrimage, qrlink, filename, guid) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" onclick=\"location.reload()\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }
    else {
        var footer = "<a style=\"margin-right: 7px;\" type=\"button\" class=\"btnRed formbtn\" href=\"" + qrlink + "\" download=\"" + filename + "\"><i class=\"glyphicon glyphicon-save\"></i> Download</a><a id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\"><i class=\"glyphicon glyphicon-repeat\"></i> Return</a>";
                      
    }

    $("#divBodyModalLogo").html(qrimage);
    $("#divHeaderModal").html(header);

    if (navigator.onLine) {
        if (body.length > 0) {
            $("#divBodyModal").html(body + "<br/>" + "<strong>Transaction ID: </strong>" + guid);
        }
        else {
            $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
        }
    } else {
        $("#divBodyModal").html('Connection to server is lost. Please try again.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}
function ModalAlert(header, body) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" onclick=\"location.reload()\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }

    $("#divHeaderModal").html(header);

    if (navigator.onLine) {
        if (body.length > 0) {
            $("#divBodyModal").html(body);
        }
        else {
            $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
        }
    } else {
        $("#divBodyModal").html('Connection to server is lost. Please try again.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

function ModalPenalty(header, body, url, isPartialLoad, div) {
    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" onclick=\"location.reload()\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" title=\"OK\" onclick=\"ReturnPage('" + url + "', '" + isPartialLoad + "', '" + div + "');\"> OK</button>";
    }

    if (navigator.onLine) {
        if (body.length > 0) {
            $("#divBodyModal").html(body);
        }
        else {
            $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
        }
    } else {
        $("#divBodyModal").html('Connection to server is lost. Please try again.');
    }

    $("#divHeaderModal").html(header);

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

function ModalAlertSuccessfulCancel(header, body) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" onclick=\"location.reload()\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnGray formbtn glyphicon glyphicon-thumbs-up\" onclick=\"location.reload()\" data-dismiss=\"modal\" title=\"OK\"> OK</button>";
    }

    $("#divHeaderModal").html(header);

    if (navigator.onLine) {
        if (body.length > 0) {
            $("#divBodyModal").html(body);
        }
        else {
            $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
        }
    } else {
        $("#divBodyModal").html('Connection to server is lost. Please try again.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Confirmation using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
// controlId - Id of the html control to execute the action (Required)
// action - Type of action to execute click, submit, change, or function (Required)
// cancelButton - Function to execute if button cancel is clicked (Optional)
function ModalConfirmation(header, body, controlId, action, cancelButton) {
    var button = "";

    if (action == "function")
        button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnRed formbtn glyphicon glyphicon-ok\" onclick=\"" + controlId + "\" title=\"Yes\"> Yes</button>&nbsp;";
    else
        button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnRed formbtn glyphicon glyphicon-ok\" title=\"Yes\" onclick=\"ContinueConfirmation('" + controlId + "', '" + action + "');\"> Yes</button>&nbsp;";

    button += "&nbsp;";

    if (cancelButton != "")
        button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnGray formbtn glyphicon glyphicon-remove\" onclick=\"" + cancelButton + "\" data-dismiss=\"modal\"title=\"No\"> No</button>";
    else
        button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnGray formbtn glyphicon glyphicon-remove\" data-dismiss=\"modal\" title=\"No\"> No</button>";

    $("#divHeaderModal").html(header);
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

function ModalConfirmationCancel(header, body, value, action, cancelButton) {
    var button = "";

    button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnRed formbtn glyphicon glyphicon-ok\" onclick=\"return CancelRequest('" + value + "')\" title=\"Yes\"> Yes</button>&nbsp;";
    button += "&nbsp;";
    button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnGray formbtn glyphicon glyphicon-remove\" onclick=\"" + cancelButton + "\" data-dismiss=\"modal\"title=\"No\"> No</button>";

    
    $("#divHeaderModal").html(header);
    $("#divBodyModalLogo").html("<i class='bx bx-error-circle'></i>");
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

function ModalConfirmationReturnModeReset(header, body, value, value2, url, cancelButton) {
    var button = "";

    button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnRed formbtn glyphicon glyphicon-ok\" onclick=\"return ReturnModeReset('" + value + "','" + value2 + "','" + url + "')\" title=\"Yes\"> Yes</button>&nbsp;";
    button += "&nbsp;";
    button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnGray formbtn glyphicon glyphicon-remove\" onclick=\"" + cancelButton + "\" data-dismiss=\"modal\"title=\"No\"> No</button>";


    $("#divHeaderModal").html(header);
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

function ReturnModeReset(value, value2, url) {

    $.ajax({
        url: url,
        type: "POST",
        data: {
            requestGUID: value,
            mode: value2
        },
        traditional: true,
        dataType: "JSON",
        success: function (data) {
            if (data.IsSuccess != true) {
                var msg = '';

                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.MessageList[i] + '<br />';
                    }
                }
                else {
                    msg += data.Result;
                }
                ModalAlert(MODAL_HEADER, "Something went wrong.");
            } else {
                $('#divModal').modal('hide');
                $("#divScanModal").modal('hide');
                LoadPartial(listPageURL, "divPartialContent");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        },
    });
}


// Execute Confirmation modal command
function ContinueConfirmation(controlId, action) {
    $("#btnConfirmationModal").prop("disabled", true);

    if (action == "submit")
        $("#" + controlId + "").submit();
    else if (action == "change")
        $("#" + controlId + "").change();
    else
        $("#" + controlId + "").click();
}

// Confirmation using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
// destinationURL - Destination URL (Required)
// isPartialLoad - Indicates if the page will be load to a HTML tag (Optional)
// div - HTML tags where partival view will be loaded (Required if isPartialLoad is true)
function ReturnConfirmation(header, body, destinationURL, isPartialLoad, div) {
    var button = "";

    button = "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnRed formbtn glyphicon glyphicon-ok\" title=\"Yes\" onclick=\"ReturnPage('" + destinationURL + "', '" + isPartialLoad + "', '" + div + "');\">Yes</button>&nbsp;";
    button += "&nbsp;";
    button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnGray formbtn glyphicon glyphicon-remove\" data-dismiss=\"modal\" title=\"No\">No</button>";

    $("#divHeaderModal").html(header);
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Execute Confirmation modal command
function ReturnPage(destinationURL, isPartialLoad, div) {
    window.IsTransactionPage = false;
    $("#btnConfirmationModal").prop("disabled", true);
    $("#divModal").modal("hide");
    if (isPartialLoad == "true" || isPartialLoad == true)
        LoadPartial(destinationURL, div);
    else
        window.location = destinationURL;
}

// Upload using modal
// url - Upload URL (Optional)
// header - Header title of the modal (Optional)
// sampleFormURL - Form where to get the URL (Optional)
// customFunction - Local Function to execute (Optional but Required if url parameter is not provided). If this is provided you have to create your own upload function
function ModalUpload(url, header, sampleFormURL, customFunction) {
    var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnRed formbtn glyphicon glyphicon-import\" title=\"Upload\" onclick=\"Upload('" + url + "');\">Upload</button>";
    if (url == "")
        button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnRed formbtn glyphicon glyphicon-import\" title=\"Upload\">Upload</button>";

    var sampleFormLink = "";

    if (customFunction != "")
        button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnRed formbtn glyphicon glyphicon-import\" title=\"Upload\" onclick=\"" + customFunction + ";\">Upload</button>";


    if (sampleFormURL != "")
        sampleFormLink = "<a href=\"" + sampleFormURL + "\">Click to Download Form</a>";

    if (header != "")
        $("#pheader").html(header);

    button += "&nbsp <button type=\"button\" id=\"btnUploadFileClose\" class=\"btnGray formbtn glyphicon glyphicon-remove\" data-dismiss=\"modal\" title=\"Cancel\">Cancel</button>";

    $("#fileUpload").val("");
    $("#divUploadForm").html(sampleFormLink);
    $("#divFooterUploadModal").html(button);
    $("#divUploadModal").modal("show");
}

function ModalScan(header, ScanQRURL) {

    if (header != "")
        $("#pheader").html(header);

    
    $("#divScanModal").modal('show');
    LoadPartial(ScanQRURL, 'divScan');

}

// Save Uploaded File
function Upload(url) {
    $("#btnUploadFile").prop("disabled", true);

    var file;
    file = $("#fileUpload")[0].files[0];

    $.ajax({
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Cache-Control", "no-cache");
            xhr.setRequestHeader("X-File-Name", file.name);
            xhr.setRequestHeader("X-File-Size", file.size);
            xhr.setRequestHeader("Content-Type", "multipart/form-data");
        },
        type: "POST",
        url: url,
        processData: false,
        cache: false,
        data: file,
        success: function (data, textStatus, xhr) {
            $("#divUploadModal").modal("hide");

            var msg = "";
            if (data.IsListResult) {
                for (var i = 0; i < data.Result.length; i++) {
                    msg += data.Result[i] + "<br />";
                }
            } else {
                msg += data.Result;
            }

            if (data.IsSuccess) {
                ModalAlert("ERS", msg);
            } else {
                ModalAlert("ERS", msg);
            }

            $("#btnUpload").prop("disabled", false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#divUploadModal").modal("hide");
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
            $("#btnUpload").prop("disabled", false);
        }
    });
}

// Hide Upload Modal
function HideUploadModal() {
    $("#fileUpload").val("");
    $("#divUploadForm").html("");
    $("#divFooterUploadModal").html("");
    $("#divUploadModal").modal("hide");
}

// Load partial view into a div
// url - URL Page to load (Required)
// div - HTML tags where partival view will be loaded (Required)
// urlRouteForError - URL page to be assign on the button to load if error is encountered (Optional)
function LoadPartial(url, div, urlRouteForError) {
    Loading(true);

    if (urlRouteForError != undefined && urlRouteForError != "")
        $("#" + div + "").html("");

    $.ajax({
        url: url,
        success: function (data) {
            if (data.IsSuccess == undefined) {
                $("#" + div + "").html(data);
            }
            else {
                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }
                ModalAlert("ERS", msg);

                if (urlRouteForError != undefined && urlRouteForError != "")
                    $("#" + div + "").html("<button type=\"button\" class=\"btnRed formbtn glyphicon glyphicon-chevron-left\" title=\"Back\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());

            if (urlRouteForError != undefined && urlRouteForError != "")
                $("#" + div + "").html("<button type=\"button\" class=\"btnRed formbtn glyphicon glyphicon-chevron-left\" title=\"Back\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");

            //ModalAlert("Session Expired", "Please try to Login again.");
            //window.location = window.loginPageURL;
        }
    });

    return false;
}

// Load page view into a div
// url - URL Page to load (Required)
// type - Indicate if POST or GET (Required)
// param - Parameters to be pass (Optional)
// traditional - Indicate if the parameter would be treated traditional or not (Optional) | Boolean
// div - HTML tags where partival view will be loaded (Required)
// urlRouteForError - URL page to be assign on the button to load if error is encountered (Optional)
function LoadPage(url, type, param, traditional, div, urlRouteForError) {
    if (param == undefined)
        param = "";

    if (traditional == undefined)
        param = false;

    Loading(true);

    $.ajax({
        url: url,
        type: type,
        data: param,
        traditional: traditional,
        success: function (data) {
            if (data.IsSuccess == undefined) {
                $("#" + div + "").html(data);
            }
            else {
                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }
                ModalAlert("ERS", msg);

                $("#" + div + "").html("<button type=\"button\" class=\"btnRed formbtn glyphicon glyphicon-chevron-left\" title=\"Back\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        }
    });

    return false;
}

// Generate Dropdown
// url - URL where to get the data that will populate the dropdown (Required)
// ddlId - Id of the select tag (Required)
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// defaulValue - Default value of the select (Optional)
// isFilter - To determine if the select is will be used on Form or Filter (Required) | Boolean
function GenerateDropdownValues(url, ddlId, valueColumn, displayColumn, defaulValue, defaulValueDisplay, isFilter) {
    $.ajax({
        url: url,
        type: "POST",
        data: "",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var values = "";

            $("#" + ddlId + "").empty();

            if (isFilter) {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- " + (defaulValueDisplay == "" ? "Any" : defaulValueDisplay) + " -"
                }));
            }
            else {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- Select an " + (defaulValueDisplay == "" ? "Item" : defaulValueDisplay) + " -"
                }));
            }

            if (data.IsSuccess) {
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    if (data.Result != null && data.Result != undefined) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert("ERS", msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
    });
}

// Generate Dropdown
// url - URL where to get the data that will populate the dropdown (Required)
// param - Paramenter
// ddlId - Id of the select tag (Required)
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// defaulValue - Default value of the select (Optional)
// isFilter - To determine if the select is will be used on Form or Filter (Required) | Boolean
function GenerateDropdownValuesWithParam(url, param, ddlId, valueColumn, displayColumn, defaulValue, defaulValueDisplay, isFilter) {
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var values = "";

            $("#" + ddlId + "").empty();

            if (isFilter) {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- " + (defaulValueDisplay == "" ? "Any" : defaulValueDisplay) + " -"
                }));
            }
            else {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- Select an " + (defaulValueDisplay == "" ? "Item" : defaulValueDisplay) + " -"
                }));
            }

            if (data.IsSuccess) {
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    if (data.Result != null && data.Result != undefined) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert("ERS", msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
    });
}

// Download Report File (Look Tax Exemption file export for sample)
// url - Link where to download file (Required)
// param - Paremeter of link for downloading file (optional)
function DownloadReportFile(url, param) {

    Loading(true);
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                window.open(window.downloadReportURL + "?id=" + data.Result);

            }
            else {
                var msg = "";
                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                HideExportModal();
                ModalAlert("ERS", msg);
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideExportModal();
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
    });
}

// Download Text File 
// url - Link where to download file (Required)
// param - Parameter of link for downloading file (optional)
function DownloadTextFile(url, param) {
    Loading(true);

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                window.open(window.downloadTextFileURL + "?id=" + data.Result);
            }
            else {
                var msg = "";
                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                HideExportModal();
                ModalAlert("ERS", msg);

            }
            // setTimeout(location.reload.bind(location), 2000);
            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideExportModal();
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
    });
}

// Build a table
// tableHeader - List of header columns of the table (Required)
// tableColumn - List of Column name or object name (Required)
// tableData - List of values to display (Required)
// Note: tableHeader and tableColumn must have the same length
function TableBuilder(tableHeader, tableColumn, tableData) {
    var headerLength = tableHeader.length;

    var result = "<table id=\"tblGenerated\">";

    result += "<thead>";
    result += "<tr>";
    for (var i = 0; i < headerLength; i++) {
        result += "<td>" + tableHeader[i] + "</td>";
    }
    result += "</tr>";
    result += "</thead>";

    result += "<tbody>";
    for (var i = 0; i < tableData.length; i++) {
        result += "<tr>";
        for (var j = 0; j < headerLength; j++) {
            result += "<td>" + tableData[i][tableColumn[j]] + "</td>";
        }
        result += "</tr>";
    }
    result += "</tbody>";

    result += "</table>";

    return result;
}

// Create table to be use for JQGrid
// Note: You can only initialize it once on the page
function InitializeTableForJQGrid() {
    var result = "<table id=\"tblJQGrid\" class=\"table\">";
    result += "</table><div id=\"divJQGridPager\"></div>";
    return result;
}

// Covert the InitializeTableForJQGrid to JQGrid
// tableHeader - List of header columns of the table (Required)
// tableColumn - List of Column name or object name (Required)
// tableData - List of values to display (Required)
// Note: tableHeader and tableColumn must have the same length
function ConvertTableBuilderToJQGrid(tableHeader, tableColumn, tableData) {
    $("#tblJQGrid").jqGrid("GridUnload");
    $("#tblJQGrid").jqGrid("GridDestroy");
    $("#tblJQGrid").jqGrid({
        data: tableData,
        datatype: "local",
        colNames: tableHeader,
        colModel: tableColumn,
        toppager: $("#divJQGridPager"),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        loadonce: true,
        viewrecords: true,
        emptyrecords: "No records to display",
        rowNumbers: true,
        width: "100%",
        height: "100%",
        sortable: true,
    }).navGrid("#divJQGridPager",
        { edit: false, add: false, del: false, search: false, refresh: false }
    );
}

// Convert the table build by the TableBuilder function to DataTable
// Note: You must TableBuilder function first before this 
// columnToFreeze - No of column to freeze on the left (Optional) (Don't forget to include the dataTables.fixedColumns extention)
function ConvertTableBuilderToDataTable(columnToFreeze) {

    var table = $("#tblGenerated").DataTable({
        bFilter: false,
        bInfo: false,
        bAutoWidth: false,
        sScrollX: true,
        sScrollY: true,
    });

    if (columnToFreeze > 0) {
        //new $.fn.dataTable.FixedColumns(table, {
        //    leftColumns: columnToFreeze,
        //});        
    }
}

// To show the export modal
function ShowExportModal() {
    $("#divExportModal").modal("show");
}

// To hide export modal
function HideExportModal() {
    $("#divExportModal").modal("hide");
}

// Generate Listbox values
// url - URL where to get the data that will populate the dropdown (Required)
// ddlId - Id of the select tag (Required)
// param - Parameter
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// ddlFilterId - Id of the 2nd select tag, if the valueColumn of ddlId exist on ddlFilterId value it will not be shown (Optional)
function ListBoxGenerateValues(url, param, ddlId, valueColumn, displayColumn, ddlFilterId) {
    Loading(true);
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                $("#" + ddlId + "").empty();
                $("#" + ddlId + "").prop("multiple", "multiple");
                for (var i = 0; i < data.Result.length; i++) {
                    if ($("#" + ddlFilterId + " option[value='" + data.Result[i][valueColumn] + "']").length == 0) {
                        $("#" + ddlId + "").append($('<option/>', {
                            value: data.Result[i][valueColumn],
                            text: data.Result[i][displayColumn]
                        }));
                    }
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert("ERS", msg);
            }
            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
    });
}

function ListBoxGenerateValues2(url, param, ddlId, valueColumn, displayColumn, button1, button2, button3, button4) {
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                $("#" + ddlId + "").empty();
                $("#" + ddlId + "").prop("multiple", "multiple");
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert("ERS", msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert("ERS", $(jqXHR.responseText).filter('title').text());
        },
        beforeSend: function () {
            $("#" + button1 + "").prop("disabled", true);
            $("#" + button2 + "").prop("disabled", true);
            $("#" + button3 + "").prop("disabled", true);
            $("#" + button4 + "").prop("disabled", true);
        },
        complete: function () {
            $("#" + button1 + "").prop("disabled", false);
            $("#" + button2 + "").prop("disabled", false);
            $("#" + button3 + "").prop("disabled", false);
            $("#" + button4 + "").prop("disabled", false);
        },
    });
}

// Move list box values from one to another
// sourceId - Id of the source list box (Required)
// destinationId - Id of the list box that will received the values (Required)
// isAll - Identifies if the values would be transfer is selected only or all values | Boolean (Optional)
// isSort - Identifies if the values would be sorted | Boolean (Optional)
function ListBoxMove(sourceId, destinationId, isAll, isSort) {
    if (isAll == undefined)
        isAll = false;

    $("#" + sourceId + (isAll ? " option" : " :selected") + "").each(function (i, value) {
        $("#" + destinationId + "").append($("<option>", {
            value: value.value,
            text: value.text
        }));

        $(this).remove();
    });

    if (isSort) {
        $("#" + destinationId + "").append($("#" + destinationId + " option").remove().sort(function (a, b) {
            return ($(a).text() > $(b).text()) ? 1 : (($(a).text() < $(b).text()) ? -1 : 0);
        }));
    }
}

// Search on the list box values
// id - Id of the list box to be searched (Required)
// searchValue - Value to be searched (Required)
// isCS - Identifies if the value to be search is case sensitive or not | Boolean (Optional)
function ListBoxSearch(id, searchValue, isCS) {
    searchValue = $.trim("" + searchValue);

    if (isCS == undefined) {
        isCS = false;
    }

    if (!isCS) {
        searchValue = searchValue.toUpperCase();

        // Overried the 'contains' function of jquery
        $.expr[":"].contains = $.expr.createPseudo(function (arg) {
            return function (elem) {
                return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
            };
        });
    }

    if (searchValue != "") {
        $("#" + id + " option:not(:contains('" + searchValue + "'))").hide();
        $("#" + id + " option:contains('" + searchValue + "')").show();
    } else {
        $("#" + id + " option").show();
    }
}

// Got to page
// link - URL where to go
function GoToPage(link) {
    window.location = link;
}

// Show the loading
// show - Indicates if the loading overlay will show | Boolean
function Loading(show) {
    if (show == true)
        show = "show";
    else if (show == false)
        show = "hide";
    else
        show = "hide";

   // $("body").plainOverlay(show);
}

//Change the character into UPPERCASE
function SetToUpper(fieldID) {
    $("#" + fieldID).bind('keyup', function (e) {
        if (e.which >= 97 && e.which <= 122) {
            var newKey = e.which - 32;

            e.keyCode = newKey;
            e.charCode = newKey;
        }

        $("#" + fieldID).val(($("#" + fieldID).val()).toUpperCase());
    });
}

function SinglePeriodOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[0-9]*\.?[0-9]*$/;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9 ||
            (e.key == "." && $(txtbox).val().split('.').length === 2)
        ) {
            e.preventDefault();
        }
    });
}

function SetRowList() {
    var TABLE_SHOW_LIST = [10, 20, 50, 100];
    return TABLE_SHOW_LIST;
}

