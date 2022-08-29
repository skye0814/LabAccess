$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = mainPageURL;
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>REQUEST DETAILS</span>
                        </div>`;

    var objRequestFacilityEdit = {
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
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');
                console.log($('#frmRequestFacility'));
                if ($('#frmRequestFacility').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmRequestFacility', 'submit');
                }
                return false;
            });

            $('#frmRequestFacility').on('submit', function () {
                if ($('#frmRequestFacility').valid()) {

                    Self.input = $('#frmRequestFacility').serialize();

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

            validator = $('#frmRequestFacility').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    FacilityID: {
                        required: true
                    },
                    RequestDate: {
                        required: true
                    },
                    StartTime:
                    {
                        required: true
                    },
                    EndTime:
                    {
                        required: true
                    }
                },
                messages: {
                    FacilityID: {
                        required: 'Description' + SUFF_REQUIRED
                    },
                    RequestDate: {
                        required: 'Request Date' + SUFF_REQUIRED
                    },
                    StartTime:
                    {
                        required: 'Start Time' + SUFF_REQUIRED
                    },
                    EndTime:
                    {
                        required: 'End Time' + SUFF_REQUIRED
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
            $.ajax({
                url: saveRequestFacilityURL,
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

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },
    }
    requestFacilityEdit = Object.create(objRequestFacilityEdit);
    requestFacilityEdit.initialized();
});
