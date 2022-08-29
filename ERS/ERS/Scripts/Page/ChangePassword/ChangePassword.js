$(document).ready(function () {

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            return this.optional(element) || regexp.test(value);
        },
        "please check your input."
    );

    var objChangePassword = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#OldPassword').focus() }, 500);
        },
        bindEvents: function () {
            var Self = this;

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');
                console.log($('#frmChangePassword'));
                if ($('#frmChangePassword').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmChangePassword', 'submit');
                }
                return false;
            });



            $('#frmChangePassword').on('submit', function () {
                if ($('#frmChangePassword').valid()) {

                    //var key = CryptoJS.enc.Utf8.parse('@keybytes');
                    //var iv =  CryptoJS.enc.Utf8.parse('@ivbytes');
                    //var encryptedOldPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($('#OldPassword').val()), key,
                    //    {
                    //        keySize: 128 / 8,
                    //        iv: iv,
                    //        mode: CryptoJS.mode.CBC,
                    //        padding: CryptoJS.pad.Pkcs7
                    //    }
                    //);
                    //var encryptedNewPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($('#NewPassword').val()), key,
                    //    {
                    //        keySize: 128 / 8,
                    //        iv: iv,
                    //        mode: CryptoJS.mode.CBC,
                    //        padding: CryptoJS.pad.Pkcs7
                    //    }
                    //);
                    //$('#OldPassword').val(encryptedOldPassword);
                    //$('#NewPassword').val(encryptedNewPassword);

                    Self.input = $('#frmChangePassword').serialize();

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

            jQuery.validator.addMethod("NewPasswordSame", function (value, element) {
                var result = true;
                if ($('#OldPassword').val() == $('#NewPassword').val()) {
                    return false;
                }

                return result;
            });

            jQuery.validator.addMethod("NewConfirmNotEqual", function (value, element) {
                var result = true;
                if ($('#NewPassword').val() != $('#ConfirmPassword').val()) {
                    return false;
                }

                return result;
            });

            validator = $('#frmChangePassword').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    OldPassword: {
                        required: true
                    },
                    NewPassword: {
                        required: true,
                        regex: /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[A-Z])[a-zA-Z0-9!@#$%^&*]{6,16}$/,
                        NewPasswordSame: true

                    },
                    ConfirmPassword: {
                        required: true,
                        NewConfirmNotEqual: true
                    }

                },
                messages: {
                    OldPassword: {
                        required: 'Old Password' + SUFF_REQUIRED
                    },
                    NewPassword: {
                        required: 'New Password' + SUFF_REQUIRED,
                        NewPasswordSame: 'New password must not be the same as the old password.',
                        regex: 'Password must contain at least an uppercase, a numeric, and a special character that is 6 characters minimum'
                    },
                    ConfirmPassword: {
                        required: 'Confirm Password' + SUFF_REQUIRED,
                        NewConfirmNotEqual: 'Confirm password must be the same as the new password.'
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
                url: savePasswordURL,
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
    ChangePassword = Object.create(objChangePassword);
    ChangePassword.initialized();
});