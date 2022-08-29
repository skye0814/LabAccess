$(document).ready(function () {
    var savedQRValue = "saved";
    var html5QrcodeScanner = new Html5QrcodeScanner(
        "reader", { fps: 10, qrbox: { width: 250, height: 250 } });

    var objViewEquipmentRequest = {

        Initialized: function () {
            var Self = this;
           
            $("#divErrorMessage").html("");
            $('#divScanModal').modal('hide');
            Self.BindEvent();
            if ($('#Status').val() == "Claimed" && $('#ClaimedTime').val() == "0") {
                $('#reader').val("Reader Disabled");
            }
            else if ($('#Status').val() == "Returned" && $('#ReturnedTime').val() == "0") {
                $('#reader').val("Reader Disabled");
            }
            else if ($('#Status').val() == "Completed") {
                $('#reader').val("Reader Disabled");
                $('#btnClaim').prop('disabled', true).css('opacity', 0.5);;
            }
            else {
                Self.scanQRCode();
                $('#btnClaim').prop('disabled', true).css('opacity', 0.5);;
            }
        },
        BindEvent: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                var param = {
                    requestGUID: $('#RequestFacilityGUID').val(),
                    mode: 'reset'
                }
                ModalConfirmationReturnModeReset(MODAL_HEADER, MSG_CONFIRM_BACK + " Records will not be saved.", param.requestGUID, param.mode, "ClaimReturnFacility/UpdateClaimedRequest");
                /*ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK + " Records will not be saved.", 'frmViewFacilityRequest', 'returnsubmit');*/
            });

            $('#frmViewFacilityRequest').on('returnsubmit', function () {
                var param = {
                    requestGUID: $('#RequestFacilityGUID').val(),
                    mode: 'reset'
                }
                Self.updateClaimedItems(param);

                return false;
            });

            $('#btnClaim').click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmViewFacilityRequest', 'submit');
            });

            $('#frmViewFacilityRequest').on('submit', function () {
                var param = {
                    requestGUID: $('#RequestFacilityGUID').val(),
                    mode: 'confirm'
                }
                Self.updateClaimedItems(param);
                return false;
            });
        },
        
        updateClaimedItems: function (param) {
            var Self = this;
            $.ajax({
                url: updateClaimedRequestURL,
                type: "POST",
                data: param,
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

                        LoadPartial(listPageURL, "divPartialContent");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },

        //QR CODE SCANNER
        onSuccessScan: function (qrCodeValue) {
            var x = this;
            if (savedQRValue != qrCodeValue) {
                savedQRValue = qrCodeValue;
                var input =
                {
                    QRCode: qrCodeValue
                    , FacilityID: $('#FacilityID').val()
                    , RoomNumber: $('#RoomNumber').val()
                    , RequestFacilityGUID: $('#RequestFacilityGUID').val()
                    , Status: $('#Status').val()
                };
                html5QrcodeScanner.clear();
                $.ajax({
                    url: setRoomURL,
                    type: "POST",
                    data: input,
                    traditional: true,
                    dataType: "JSON",
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
                            LoadPartial(viewFacilityRequestURL + '?requestGUID=' + $('#RequestFacilityGUID').val(), "divPartialContent");
                        } else {
                            $("#divScanModal").modal('hide');
                            LoadPartial(viewFacilityRequestURL + '?requestGUID=' + data.RequestFacilityGUID, "divPartialContent");
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Loading(false);
                        ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                    },
                });
            }
        },
        onScanError: function (errorMessage) {
            var Self = this;
        },
        scanQRCode: function () {
            var Self = this;
            html5QrcodeScanner.clear();
            html5QrcodeScanner.render(Self.onSuccessScan, Self.onScanError);
        },
    };  
    var viewRequestEquipment = Object.create(objViewEquipmentRequest);
    viewRequestEquipment.Initialized();
});