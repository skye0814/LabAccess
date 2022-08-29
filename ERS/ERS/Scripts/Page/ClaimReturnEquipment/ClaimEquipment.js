$(document).ready(function () {
    var objClaimEquipment = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, window.mainPageURL, false);
            });
            $('#btnScan').click(function () {
                alert();
                LoadPartial(scanQRCodeURL, 'divPartialContent');
            });

        },

    }
    var claimEquipment = Object.create(objClaimEquipment);
    claimEquipment.initialized();
});
