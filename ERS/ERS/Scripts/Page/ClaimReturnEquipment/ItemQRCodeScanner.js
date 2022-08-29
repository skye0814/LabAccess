$(document).ready(function () {
    var savedQRValue = "saved";
    var html5QrcodeScanner = new Html5QrcodeScanner(
        "reader", { fps: 30, qrbox: { width: 250, height: 250 } });
    var objItemQRCodeScanner = {
        initialized: function () {
            var Self = this;
            Self.scanQRCode();
        },
        onSuccessScan: function (qrCodeValue) {
            var Self = this;
            if (savedQRValue != qrCodeValue) {
                savedQRValue = qrCodeValue;
                var input =
                {
                    QRCode: qrCodeValue
                    , requestEquipmentItemID: $('#RequestEquipmentItemID').val()
                };
                html5QrcodeScanner.clear();
                $.ajax({
                    url: decodeQRCodeURL,
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
                            $("#divScanModal").modal('hide');
                            ModalAlert(MODAL_HEADER, msg);
                        } else {
                            $("#divScanModal").modal('hide');
                            LoadPartial(viewEquipmentRequestURL + '?requestGUID=' + data.RequestGUID, "divPartialContent");
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
    }
    var itemQRCodeScanner = Object.create(objItemQRCodeScanner);
    itemQRCodeScanner.initialized();
});
