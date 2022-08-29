///Allow alphabet and ('), (-) characters in 'Name' fields
function NameOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[a-zA-Z-.\']*$/;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }

        var a = $(this).val();

        a = a.split(".").join("");
        a = a.split(" ").join("");
        a = a.split("-").join("");
        a = a.split("'").join("");

        if (a.length == 0) {
            $(this).val("");
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
        if (txtbox.val().indexOf('+') != -1) {
            if (e.shiftKey == 1 && e.keyCode == 187 || (e.keycode == 107))
                e.preventDefault();
        }
    });
}

///9 Digits with 2 Decimal Places 
///Formatting for Amount
function AmountDecimalFormat(FieldName) {
    FieldName.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{2})?$/g;
        var Amount = FieldName.val().replace(/,/g, "");

        if (e.keyCode == 32) {
            e.preventDefault();
        }
        if (FieldName.val() == "" && e.keyCode == 190) {
            FieldName.val("0");
        }
        if (("" + reg.test(e.key)) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 9 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///MM/dd/yyyy Formatting for date
function DateFormat(dtp) {
    var date = new Date(dtp.val());
    dtp.val(formatDateToString(date));
}

function formatDateToString(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    return (MM + "/" + dd + "/" + yyyy);
}

function Code(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[a-zA-Z0-9_]*$/; //removed \-
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}


///1 Digit with 4 Decimal Places for Percentage
function PercentDecimalFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }
        
        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }

        if (("" + reg.test(e.key)) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 1 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///Automatic put comma when textbox changed / remove focus
function AutoCommaAmount(txtbox) {
    txtbox.on('change', function () {
        if (txtbox.val() != "") {
            var amount = txtbox.val();
            txtbox.val(parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
        }
    });
}

///Automatic put comma in display, 2 decimal places
function AutoCommaAmount_showOnEdit(txtbox) {
    if (txtbox.val() != "") {
        var amount = txtbox.val();
        txtbox.val(parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
}

///Automatic put comma in display, 4 decimal places
function AutoCommaAmount_showOnEdit4(txtbox) {
    if (txtbox.val() != "") {
        var amount = txtbox.val();
        txtbox.val(parseFloat(amount, 12).toFixed(4).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
}

///Accepts Numbers Only
function NumericFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
    });
}

//Accepts Negative and positive whole number
function IntegerFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var value = txtbox.val();
        var charvalue = String.fromCharCode(e.keyCode)

        if (("" + reg.test(e.key)) == "false"
            && e.keyCode != 8 && charvalue != "½"
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        if (charvalue == "½" && value.includes("-"))
        {
            e.preventDefault(); 
        }

        if (charvalue == "½" && value.length > 0) {
            e.preventDefault();
        }

        if (charvalue == "0" && value.length == 1 && value.substring(0, 1) == "-") {
            e.preventDefault();
        }

        if (charvalue == "0" && value.length == 0) {
            e.preventDefault();
        }

    });
}

//Accepts Negative and positive decimal
function DecimalsFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^-?[0-9]\d*(\.\d+)?$/;
        var value = txtbox.val();
        var charvalue = String.fromCharCode(e.keyCode)

        if (("" + reg.test(e.keyCode)) == "false"
            && e.keyCode != 8 && charvalue != "½" && charvalue != "¾"
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }

        if (charvalue == "½" && value.includes("-")) {
            e.preventDefault();
        }

        if (charvalue == "¾" && value.includes(".")) {
            e.preventDefault();
        }

        if (charvalue == "½" && value.length > 0) {
            e.preventDefault();
        }

        if (charvalue == "¾" && value.length == 1 && value.substring(0, 1) == "-") {
            e.preventDefault();
        }

    });
}

function Days3Format(txtbox) {
     txtbox.keydown(function (e) {
         if ($(this).val().length == 3
          && e.keyCode != 46 // delete
          && e.keyCode != 8 // backspace
         ) {
             e.preventDefault();
         }
    })
}

///no Space Allowed
function PreventSpace(txtbox) {
    txtbox.keydown(function (e) {
        if (e.keyCode == 32
            ) {
            e.preventDefault();
        }
    });
}

///1 Digit with 4 Decimal Places for Percentage
function PercentDecimalFormatPagibig(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }
        if (txtbox.val() == "" && e.keyCode != 49 && e.keyCode != 190 && e.keyCode != 48) {
            e.preventDefault();
        }
        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }
        if (txtbox.val() == 1 &&  e.keyCode != 8) {
            e.preventDefault();
        }
        
        if (("" + reg.test(e.key)) == "false"
             && e.keyCode != 8
             && parseFloat(Amount) > 1
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9 
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 1 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///Remove value of fields when Key up and down.
function RemoveValue(txtbox)
{
    txtbox.keydown(function (e) {
        txtbox.val("");
    });

    txtbox.keyup(function (e) {
        txtbox.val("");
    });
}

///Accept alphanumeric only
function AlphaNumeric(txtbox)
{
    txtbox.bind('input', function () {
        $(this).val($(this).val().replace(/[^a-z0-9\s]/gi, ''));
    });
}

function PersonnelCode(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[a-zA-Z0-9_-]*$/; //removed \-
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}

function PersonnelName(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[a-zA-Z-]*$/; //removed \-
        if (("" + reg.test(e.key)) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}



/* FOR GOVERNMENT DEDUCTION MODALS*/

// Upload using modal
// url - Upload URL (Optional)
// header - Header title of the modal (Optional)
// sampleFormURL - Form where to get the URL (Optional)
// customFunction - Local Function to execute (Optional but Required if url parameter is not provided). If this is provided you have to create your own upload function
function ModalPayUpload(url, header, sampleFormURL, customFunction, ActiveDate) {

    formatModalEffectiveDate(ActiveDate);
    var button = "<button type=\"button\" id=\"btnPayUploadFile\" class=\"btnRed formbtn\" onclick=\"PayUpload('" + url + "');\">Upload</button>";


    var sampleFormLink = "";

    if (customFunction != "")
        button = "<button type=\"button\" id=\"btnPayUploadFile\" class=\"btnRed formbtn\" onclick=\"" + customFunction + ";\">Upload</button>";

    button += "&nbsp <button type=\"button\" id=\"btnPayUploadFileClose\" class=\"btnGray formbtn\" data-dismiss=\"modal\">Cancel</button>";

    if (sampleFormURL != "")
        sampleFormLink = "<a href=\"" + sampleFormURL + "\">Click to Download Form</a>";

    if (header != "")
        $("#pPayheader").html(header);

    $("#filePayUpload").val("");
    $("#divPayUploadForm").html(sampleFormLink);
    $("#divPayFooterUploadModal").html(button);
    $("#divPayUploadModal").modal("show");
}

// Save Uploaded File
function PayUpload(url) {
    if (url != "") {
        $("#btnPayUploadFile").prop("disabled", true);
        var file;
        file = $("#filePayUpload")[0].files[0];
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
                $("#divPayUploadModal").modal("hide");

                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                if (data.IsSuccess) {
                    ModalAlert(MODAL_HEADER, msg);
                } else {
                    $("#divErrorMessage").html(msg);
                    ModalAlert(MODAL_HEADER, msg);
                }

                $("#btnPayUpload").prop("disabled", false);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#divPayUploadModal").modal("hide");
                ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                $("#btnPayUpload").prop("disabled", false);
            }
        });
    }
}

function formatModalEffectiveDate(ActiveDate) {
    $("#txtModalStatus").prop("disabled", true);
    var hdnCurrenDate = new Date(ActiveDate);

    hdnCurrenDate.setDate(hdnCurrenDate.getDate() + 1);

    $("#dtpModalEffectiveDate").datepicker({
        onSelect: function (selected, evnt) {
            ChangeModalStatus(selected);
        },
        minDate: hdnCurrenDate,
        changeMonth: true,
    changeYear: true,
    });
}

function ChangeModalStatus(SelectedDate) {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }
    var DateToday = new Date(mm + '/' + dd + '/' + yyyy);
    var Effectivedate = new Date(SelectedDate);

    if (Effectivedate > DateToday) {
        $("#txtModalStatus").val("0");
    }
    else {
        $("#txtModalStatus").val("1");
    }

}

function HighlightFieldsUpload(isError) {
    //RemoveUploadHighlights();
    if (isError == 1) {
        document.getElementById("fileUpload").style.border = "1px solid #c40000";
    }
    else {
        document.getElementById("fileUpload").style.border = "";
    }
}

function HighlightFieldsPayUpload(isError) {
    if (isError == 1) {
        document.getElementById("filePayUpload").style.border = "1px solid #c40000";
    }
}

function ValidateUploadFields() {
    RemoveModalHighlight();
    var isvalid = true;
    var counter = 0;
    if ($("#txtModalTableName").val() == "") {
        document.getElementById("txtModalTableName").style.border = "1px solid #c40000";
        $("#divPayModalErrorMessage").html("Table Name" + SUFF_REQUIRED);
        counter += 1;
        isvalid = false;
    }

    if ($("#txtModalStatus").val() == "") {
        document.getElementById("txtModalStatus").style.border = "1px solid #c40000";
        $("#divPayModalErrorMessage").html("Status" + SUFF_REQUIRED);
        counter += 1;
        isvalid = false;
    }


    if ($("#dtpModalEffectiveDate").val() == "") {
        document.getElementById("dtpModalEffectiveDate").style.border = "1px solid #c40000";
        $("#divPayModalErrorMessage").html("Effective Date" + SUFF_REQUIRED);
        counter += 1;
        isvalid = false;
    }


    if (document.getElementById("filePayUpload").files.length == 0) {
        document.getElementById("filePayUpload").style.border = "1px solid #c40000";
        $("#divPayModalErrorMessage").html("Upload" + SUFF_REQUIRED);
        counter += 1;
        isvalid = false;
    }

    if (counter > 1) {
        $("#divPayModalErrorMessage").html(REQ_HIGHLIGHTED_FIELDS);
    }
    else if (counter == 0) {
        isvalid = true;
    }


    return isvalid;
}

function RemoveModalHighlight() {
    document.getElementById("txtModalTableName").style.border = "";
    document.getElementById("txtModalStatus").style.border = "";
    document.getElementById("dtpModalEffectiveDate").style.border = "";
    document.getElementById("filePayUpload").style.border = "";
    $("#divPayModalErrorMessage").html("");
}

function RemoveUploadHighlights() {
    document.getElementById("fileUpload").style.border = "";
    $("#divModalErrorMessage").html("");
}

function RemoveExportModal(button)
{
    button.click(function () {
        $('#divModal').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    });
}
/* END FOR GOVERNMENT DEDUCTION MODALS*/

//HIDE UPLOAD MODAL
function HideModalPayUpload() {
    $("#filePayUpload").val("");
    $("#divPayUploadForm").html("");
    $("#divPayFooterUploadModal").html("");
    $("#divPayUploadModal").modal("hide");
}


///2 Digit with 4 Decimal Places for Percentage
function DaysDecimalFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }

        if (txtbox.val() == "" && e.keyCode == 190 && e.keyCode == 110) {
            txtbox.val("0");
        }
		
		if (e.shiftKey) {
			e.preventDefault();
		}
        
		if (("" + reg.test(e.key)) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 110 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && (e.keyCode == 190 || e.keyCode == 110)) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 2 && e.keyCode != 190 && e.keyCode != 110 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}
///3 Digit with 4 Decimal Places for Percentage
function AnnualDaysDecimalFormat(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }

        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }

        if (e.shiftKey) {
            e.preventDefault();
        }

        if (("" + reg.test(e.key)) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 3 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

//format of amount in textbox 0.00
function formatAmountZero(textbox) {
    if (textbox.val() != "")
        textbox.val(AddZeroes(textbox.val()).withComma());
    else
        textbox.val("0.00");
}

function textChange(e, a) {
    //If text box is 0.00 set value to blank
    e.focus(function () {
        if (parseInt($(this).val()) == 0)
            $(this).val("");

        if (a == 1)
            $(this).val($(this).val().withComma());
    });

}


///2 Digits with 2 Decimal Places 
function TwoDigit2Decimal(FieldName) {
    FieldName.keydown(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{2})?$/g;
        var Amount = FieldName.val().replace(/,/g, "");

        if (e.keyCode == 32) {
            e.preventDefault();
        }
        if (FieldName.val() == "" && e.keyCode == 190) {
            FieldName.val("0");
        }
        if (("" + reg.test(e.key)) == "false"
            && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 2 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}