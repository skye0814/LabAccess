$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = mainPageURL;
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>EDIT</span>
                        </div>`;
    
    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            return this.optional(element) || regexp.test(value);
        },
        "please check your input."
    );

    var objStudentRegistrationEdit = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#StudentNumber').focus() }, 500);
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');
                console.log($('#frmStudentRegistration'));
                if ($('#frmStudentRegistration').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmStudentRegistration', 'submit');
                }
                return false;
            });

            

            $('#frmStudentRegistration').on('submit', function () {
                if ($('#frmStudentRegistration').valid()) {

                    Self.input = $('#frmStudentRegistration').serialize();

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

            validator = $('#frmStudentRegistration').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    StudentNumber: {
                        required: true,
                        regex: /^\d[0-9]{3}-\d[0-9]{4}-MN-\d[0-1]{0}$/
                    },
                    FirstName: {
                        required: true,
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    MiddleName: {
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    LastName: {
                        required: true,
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    CourseID:
                    {
                        required: true
                    },
                    YearID:
                    {
                        required: true
                    },
                    SectionID:
                    {
                        required: true
                    },
                    UserName:
                    {
                        required: true
                    }
                },
                messages: {
                    StudentNumber: {
                        required: 'Student Number' + SUFF_REQUIRED,
                        regex: 'Invalid Student Number Format!!! (YYYY-XXXXX-MN-X)'
                    },
                    FirstName: {
                        required: 'First Name' + SUFF_REQUIRED,
                        regex: 'Invalid first name input'
                    },
                    MiddleName: {
                        regex: 'Invalid middle name input'
                    },
                    LastName: {
                        required: 'Last Name' + SUFF_REQUIRED,
                        regex: 'Invalid first name input'
                    },
                    CourseID:
                    {
                        required: 'Course' + SUFF_REQUIRED
                    },
                    YearID:
                    {
                        required: 'Year' + SUFF_REQUIRED
                    },
                    SectionID:
                    {
                        required: 'Section' + SUFF_REQUIRED
                    },
                    UserName:
                    {
                        required: 'Username' + SUFF_REQUIRED
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
                url: saveStudentURL,
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
    studentRegistrationEdit = Object.create(objStudentRegistrationEdit);
    studentRegistrationEdit.initialized();
});
