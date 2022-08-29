$(document).ready(function () {

    var objRequestEquipmentAdd = {
        initialized: function () {
            var Self = this;
            
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#EquipmentCategoryID').focus() }, 500);
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');

                if ($('#frmRequestEquipment').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmRequestEquipment', 'submit');
                }
                return false;
            });

            $('#frmRequestEquipment').on('submit', function () {
                if ($('#frmRequestEquipment').valid()) {

                    Self.input = $('#frmRequestEquipment').serialize();
                    console.log(Self.input);
                    Self.SaveRecord(Self.input);
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

            validator = $('#frmRequestEquipment').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    EquipmentCategoryID: {
                        required: true
                    },
                    Quantity: {
                        required: true
                    },
                    Date: {
                        required: true
                    },
                    Time:
                    {
                        required: true
                    }
                },
                messages: {
                    EquipmentCategoryID: {
                        required: 'EquipmentCategoryID' + SUFF_REQUIRED
                    },
                    Quantity: {
                        required: 'Quantity' + SUFF_REQUIRED
                    },
                    Date: {
                        required: 'Date' + SUFF_REQUIRED
                    },
                    Time:
                    {
                        required: 'Time' + SUFF_REQUIRED
                    }
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
            //var isAllValid = true;

            
            ////validate order items
            //$('#orderItemError').text('');
            //var list = [];
            //var errorItemCount = 0;
            //$('#orderdetailsItems tbody tr').each(function (index, ele) {
            //    if ($('select.ddlEquipmentCategory', this).val() == "0" || (parseInt($('.quantity', this).val()) || 0) == 0) {
            //        errorItemCount++;
            //        $(this).addClass('error');
            //    }
            //    else {
            //        var orderItem = {
            //            EquipmentCategoryID: $('.ddlEquipmentCategory', this).val(),
            //            Quantity: parseInt($('.quantity', this).val())
            //        }
            //        list.push(orderItem);
            //    }
            //})

            //function randomGUID() {
            //    var h = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
            //    var k = ['x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', '-', 'x', 'x', 'x', 'x', '-', '4', 'x', 'x', 'x', '-', 'y', 'x', 'x', 'x', '-', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'];
            //    var u = '', i = 0, rb = Math.random() * 0xffffffff | 0;
            //    while (i++ < 36) {
            //        var c = k[i - 1], r = rb & 0xf, v = c == 'x' ? r : (r & 0x3 | 0x8);
            //        u += (c == '-' || c == '4') ? c : h[v]; rb = i % 8 == 0 ? Math.random() * 0xffffffff | 0 : rb >> 4
            //    }
            //    return u;
            //}

            //var data = {
            //    RequestGUID: randomGUID().trim(),
            //    RequestDateTime: new Date().toLocaleDateString(),
            //    Remarks: $('#description').val(),
            //    RequestDetails: list
            //}

            $.ajax({
                url: saveRequestEquipmentURL,
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
                    }
                    //if (d.status) {
                    //    alert("Successfully saved");

                    //    // clear form
                    //    list = [];
                    //    $('#orderdetailsItems').empty();
                    //}
                    //else {
                    //    alert("Error");
                    //}

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },
    }
    requestEquipmentAdd = Object.create(objRequestEquipmentAdd);
    requestEquipmentAdd.initialized();
});
