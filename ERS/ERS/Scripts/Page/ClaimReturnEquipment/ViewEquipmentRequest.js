$(document).ready(function () {

    var objViewEquipmentRequest = {

        Initialized: function () {
            var Self = this;
           
            $("#divErrorMessage").html("");
            $('#divScanModal').modal('hide');
            Self.BindEvent();
            Self.FillFieldWithLocalStorage();
            if ($('#Status').val() == "Completed") {
                $('#btnClaim').prop('disabled', true).css('opacity', 0.5);;
            }
            // load table
            var param = {
                RequestGUID: $('#RequestGUID').val(),
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                if ($('#Status').val() == "Completed")
                {
                    LoadPartial(listPageURL, 'divPartialContent');
                }
                else {
                    var param = {
                        requestGUID: $('#RequestGUID').val(),
                        mode: 'reset'
                    };
                    ModalConfirmationReturnModeReset(MODAL_HEADER, MSG_CONFIRM_BACK + " Records will not be saved.", param.requestGUID, param.mode, "ClaimReturnEquipment/UpdateClaimedItems");
                    /*ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK + " Records will not be saved.", 'frmViewEquipmentRequest', 'change');*/
                }
            });

            $('#frmViewEquipmentRequest').on('change', function () {
                var param = {
                    requestGUID: $('#RequestGUID').val(),
                    mode: 'reset'
                }
                Self.updateClaimedItems(param);

                return false;
            });

            $('#btnClaim').click(function () {
                var param = {
                    requestGUID: $('#RequestGUID').val(),
                }
                Self.checkClaimedItems(param);
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmViewEquipmentRequest', 'submit');
            });

            $('#frmViewEquipmentRequest').on('submit', function () {
                var param = {
                    requestGUID: $('#RequestGUID').val(),
                    mode: 'confirm'
                }
                Self.updateClaimedItems(param);
                return false;
            });

        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);
            $("#tblRequestEquipmentItemList").jqGrid("GridUnload");
            $("#tblRequestEquipmentItemList").jqGrid("GridDestroy");

            $("#tblRequestEquipmentItemList").jqGrid({
                url: getRequestEquipmentItemListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["RequestEquipmentItemID", "RequestGUID", "Category", "Item Code", "isClaimed", "Status", "Scan"],
                colModel: [
                    { key: true, hidden: true, name: "RequestEquipmentItemID" },
                    { key: false, hidden: true, name: "RequestGUID" },
                    { key: false, width: 40, name: "Category", index: "1", editable: true, align: "center", sortable: true },
                    { key: false, width: 40, name: "EquipmentItemCode", index: "2", editable: true, align: "center", sortable: true },
                    { key: false, width: 40, hidden: true, name: "isClaimed", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, width: 40, name: "Status", index: "4", editable: true, align: "center", sortable: true },
                    { name: "scanQR", index: "scanQR", width: 35, align: "center", sortable: false, formatter: Self.ScanQRCode, frozen: true },
                ],
                rowNum: parseInt(localStorage['RequestEquipment_RowNum']),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    row: "Show",
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No record(s) to display.",
                multiselect: false,
                rowNumbers: true,
                page: parseInt(localStorage['RequestEquipment_PageNum']),
                sortorder: localStorage['RequestEquipment_SortOrder'],
                sortname: localStorage['RequestEquipment_SortColumn'],
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function () {
                    Self.fillTableLocalStorage();
                    Loading(false);
                },
                onPaging: function (pgButton) {
                    var newpage, last;
                    if ("user" == pgButton) {
                        newpage = parseInt($(this.p.toppager).find('input:text').val());
                        last = parseInt($(this).getGridParam("lastpage"));
                        if (newpage > last) {
                            return 'stop';
                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    var msg = "";

                    if (xhr.responseJSON.IsListResult == true) {
                        for (var i = 0; i < xhr.responseJSON.Result.length; i++) {
                            msg += xhr.responseJSON.Result[i] + "<br />";
                        }
                    } else {
                        msg += xhr.responseJSON.Result;
                    }

                    ModalAlert(MODAL_HEADER, msg);
                    Loading(false);
                }
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show </label>");
            $("#tblRequestEquipmentItemList").jqGrid("setFrozenColumns");
        },

        ScanQRCode: function (cellvalue, options, rowObject) {
            var Self = this;
            if (rowObject.Status == "Returned" || (rowObject.Status == "Unclaimed" && $('#Status').val() == "Claimed") || $('#Status').val() == "Completed")
                return "<a class=\"glyphicon glyphicon-ban-circle\"></a>";
            else
                return "<a class=\"glyphicon glyphicon-qrcode\" onclick=\"return ModalScan('','" + scanItemQRCodeURL + "?RequestEquipmentItemID=" + rowObject.RequestEquipmentItemID + "');\"></a>";            
        },
        
        FillLocalStorage: function () {
            var Self = this;
           
        },

        FillFieldWithLocalStorage: function () {
            var Self = this;
          
        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblRequestEquipmentItemList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblRequestEquipmentItemList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblRequestEquipmentItemList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['RequestEquipment_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['RequestEquipment_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['RequestEquipment_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['RequestEquipment_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },
        updateClaimedItems: function (param) {
            var Self = this;
            $.ajax({
                url: updatetClaimedItemsURL,
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
                        ModalAlert(MODAL_HEADER, msg);
                    } else {
                        //$('#divModal').modal('hide');
                        if (data.intReturnValue > 0) {
                            var link = addPenaltyURL + "?requestguid=" + $('#RequestGUID').val() + "&requestType=Equipment";
                            ModalPenalty(MODAL_HEADER, "Not all claimed items are returned. Proceeding to adding penalties", link, true, 'divPartialContent');
                            //LoadPartial(link, 'divPartialContent');
                        }
                        else {
                            $('#divModal').modal('hide');
                            LoadPartial(listPageURL, "divPartialContent");
                        }
                        
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },
        checkClaimedItems: function (param) {
            var Self = this;
            $.ajax({
                url: checkClaimedItemsURL,
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
                        ModalConfirmation(MODAL_HEADER, 'Some requested items are not scanned, do you want to proceed with the transaction?', 'frmViewEquipmentRequest', 'submit');
                    } else {
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        }
    };
  
    var viewRequestEquipment = Object.create(objViewEquipmentRequest);
    viewRequestEquipment.Initialized();
});