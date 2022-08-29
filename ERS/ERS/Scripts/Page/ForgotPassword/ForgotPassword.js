$(document).ready(function () {

    $('#txtEmailAddress').on('input', function () {
        var divErrMessage = document.getElementById("divErrorMessage");
        divErrMessage.innerHTML = "";

        var divErrMessageClass = document.getElementsByClassName("divErrorMessage");
        divErrMessageClass[0].innerHTML = "";
    });

    var objLogIn = {
        initialized: function () {
            var Self = this;
            Self.$frmForgotPassword = $("#frmForgotPassword");
            Self.$btnForgotPassword = $("#btnForgotPassword");
            Self.$btnCancel = $("#btnCancel");
            Self.$divErrorMessage = $("#divErrorMessage");
            Self.$iconShowPassword = $("#iconShowPassword");
            Self.formatting();
            Self.bindEvent();
            Self.InitializeFormValidation();
        },
        bindEvent: function () {
            var Self = this;

            Self.$frmForgotPassword.on("submit", function () {
                Loading(true);
                objLogIn.Login();
                return false;
            });

            Self.$btnForgotPassword.click(function () {
                Self.$divErrorMessage.html('');
                if (Self.$frmForgotPassword.valid()) {
                    if ($('#txtEmailAddress').val() == '') {
                        Self.$divErrorMessage.html('<span style="color: red">Email address is empty</span>');
                    }
                    else {
                        Self.$frmForgotPassword.submit();
                    }
                    
                }
            });

            Self.$btnCancel.click(function () {
                Self.$divErrorMessage.html("");
            });

            Self.$iconShowPassword.click(function () {

                if ($(this).hasClass("glyphicon-eye-close")) {
                    $(this).removeClass("glyphicon glyphicon-eye-close");
                    $(this).addClass("glyphicon glyphicon-eye-open");
                } else {
                    $(this).removeClass("glyphicon glyphicon-eye-open");
                    $(this).addClass("glyphicon glyphicon-eye-close");
                }


                if ($("#txtPassword").attr("type") == "password") {
                    $("#txtPassword").attr("type", "text");
                } else {
                    $("#txtPassword").attr("type", "password");
                }
            });

        },
        formatting: function () {
            var Self = this;
            Self.$divErrorMessage.html("");
        },
        InitializeFormValidation: function () {
            var Self = this;

            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = Self.$frmForgotPassword.validate({
                errorElement: "label",
                errorClass: "errMessage",
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    EmailAddress: {
                        required: true
                    },
                    txtEmailAddress: {
                        required: true
                    },
                },
                messages: {
                    EmailAddress: {
                        required: "Username is required."
                    },
                    txtEmailAddress: {
                        required: "Username is required."
                    },
                },
                invalidHandler: function () {
                    errorCount = validator.numberOfInvalids();
                },
                errorPlacement: function (error, element) {
                    ctr++;
                    if ($(error).text().indexOf('required') < 0) {
                        otherErrors.push('<label class=\"errMessage\"><li>' + $(error).text() + '</li></label>' + '<br />');
                    }
                    else {
                        requiredErrors.push('<label class=\"errMessage\"><li>' + $(error).text() + '</li></label>' + '<br />');
                    }

                    if (ctr == errorCount) {
                        if (requiredErrors.length > 1) {
                            requiredErrors = [];
                            requiredErrors.push('<label class=\"errMessage\"><li>Highlighted fields are required.</li></label>' + '<br />');
                        }

                        if (otherErrors.length != 0) {
                            $('#divErrorMessage').append(otherErrors);
                        }
                        if (requiredErrors.length != 0) {
                            $('#divErrorMessage').append(requiredErrors);
                        }
                        $('#divErrorMessage').css({ 'background-color': 'rgba(183, 2, 2, 0.08)' });
                        requiredErrors = [];
                        otherErrors = [];
                        ctr = 0;
                    }
                },
            });
        },

        Login: function () {
            var Self = this;

            var input = {
                EmailID: $('#txtEmailAddress').val(),
            };
            $.ajax({
                url: 'ForgotPassword',
                type: "POST",
                data: input,
                dataType: "json",
                traditional: true,
                success: function (data) {
                    if (!data.IsSuccess) {
                        Loading(false);
                        var msg = "";

                        if (data.IsListResult) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        /*ModalAlert("", msg);*/
                        var divErrMessage = document.getElementById("divErrorMessage");
                        divErrMessage.innerHTML = msg;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert("Error", $(jqXHR.responseText).filter('title').text());
                },
                beforeSend: function () {
                    $("#btnForgotPassword").val("PLEASE WAIT");
                },
                complete: function () {
                    $("#btnForgotPassword").val("SEND");
                }
            });
        },
    };


    var logIn = Object.create(objLogIn);
    logIn.initialized();

});

