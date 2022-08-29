$(document).ready(function () {

    var imageProperty = 0;
    setInterval(function () {
        imageProperty = parseInt(imageProperty) + 1;
        /*imageProperty = 5;*/
        switch (imageProperty) {
            case 0:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/loginbackground.jpg')");
                $('.img-container').css('background-size', 'cover');
                break;
            case 1:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/Rooms/CEA 302.jpg')");
                $('.img-container').css('background-size', '170% 100%');
                break;
            case 2:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/Rooms/CEA 300.jpg')");
                $('.img-container').css('background-size', 'cover');
                break;
            case 3:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/Rooms/CEA 311.jpg')");
                break;
            case 4:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/Rooms/CEA 312.jpg')");
                break;
            case 5:
                $('.img-container').css('background-image', 'none');
                $('.img-container').css('background-image', "url('../../Content/Images/Rooms/CEA 313.jpg')");
                break;
            default:
                imageProperty = -1;
                break;
        }
    }, 5000);

    $('#txtUsername').on('input', function () {
        var divErrMessage = document.getElementById("divErrorMessage");
        divErrMessage.innerHTML = "";
    });

    $('#txtPassword').on('input', function () {
        var divErrMessage = document.getElementById("divErrorMessage");
        divErrMessage.innerHTML = "";
    });

    var objLogIn = {
        initialized: function () {
            var Self = this;
            Self.$frmLogin = $("#frmLogin");
            Self.$btnLogin = $("#btnLogin");
            Self.$btnCancel = $("#btnCancel");
            Self.$divErrorMessage = $("#divErrorMessage");
            Self.$iconShowPassword = $("#iconShowPassword");
            Self.formatting();
            Self.bindEvent();
            Self.InitializeFormValidation();
        },
        bindEvent: function () {
            var Self = this;

            Self.$frmLogin.on("submit", function () {
                Loading(true);
                objLogIn.Login();
                return false;
            });

            Self.$btnLogin.click(function () {
                 Self.$divErrorMessage.html('');
                if (Self.$frmLogin.valid()) {
                    Self.$frmLogin.submit();
                }
            });

            Self.$btnCancel.click(function () {
                 Self.$divErrorMessage.html("");
            });

            Self.$frmLogin.keypress(function (e) {
                if (e.which == 13) {
                    Self.$btnLogin.click();
                }
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

            validator = Self.$frmLogin.validate({
                errorElement: "label",
                errorClass: "errMessage",
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    Username: {
                        required: true
                    },
                    Password: {
                        required: true
                    }
                },
                messages: {
                    Username: {
                        required: "Username is required."
                    },
                    Password: {
                        required: "Password is required."
                    }
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
            var key = CryptoJS.enc.Utf8.parse(keybytes);
            var iv = CryptoJS.enc.Utf8.parse(ivbytes);

            var encryptedUsername = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($('#txtUsername').val()), key,
                {
                    keySize: 128 / 8,
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                }
            );

            var encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($('#txtPassword').val()), key,
                {
                    keySize: 128 / 8,
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                }
            );
            var input = {
                Username: encryptedUsername.toString(),
                Password: encryptedPassword.toString(),
                __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
            };
            $.ajax({
                url: validateCredentialsURL,
                type: "POST",
                data: input,
                dataType: "json",
                traditional: true,
                headers: {
                    "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val()

                },
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
                    } else {
                        window.location = landingPageURL;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert("Error", $(jqXHR.responseText).filter('title').text());
                },
            });
        },
    };


    var logIn = Object.create(objLogIn);
    logIn.initialized();

});

