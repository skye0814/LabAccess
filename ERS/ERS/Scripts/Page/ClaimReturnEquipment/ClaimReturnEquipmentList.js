$(document).ready(function () {

    var objclaimReturnEquipmentList = {
        Initialized: function () {
            var Self = this;

            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtStudentNumber").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {
                Requestor: $('#txtRequestor').val(),
                RequestGUID: $('#txtRequestGuid').val(),
                Status: $('#ddlStatus').val() + ""
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;


            $("#btnSearch").click(function () {
                var param = {
                    Requestor: $('#txtRequestor').val(),
                    RequestGUID: $('#txtRequestGuid').val(),
                    Status: $('#ddlStatus').val() + ""
                };
                Self.FillLocalStorage();
                
                Self.LoadTable(param);
            });


            $("#btnScan").click(function () {
                Self.FillLocalStorage();
                ModalScan('', scanQRCodeURL);
            });

            $("#btnReset").click(function () {
                $('#ddlStatus').val("");
                $('#txtRequestor').val("");
                $('#txtRequestGuid').val("");
                var param = {}
                Self.LoadTable(param);
            });

            $('#ddlStatus').change(function () {
                var param = {
                    Status: $('#ddlStatus').val() + "",
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);

            });
         
        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);
            $("#tblRequestEquipmentList").jqGrid("GridUnload");
            $("#tblRequestEquipmentList").jqGrid("GridDestroy");

            $("#tblRequestEquipmentList").jqGrid({
                url: getRequestEquipmentListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["View", "Penalty", "Deny", "", "GUID", "Request Date", "Start Date", "End Date", "Status", "Remarks", "Claimed Date", "Return Date", "Requestor"],
                colModel: [
                    { name: "edit", index: "edit", width: 50, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { name: "penalty", index: "penalty", width: 50, align: "center", sortable: false, formatter: Self.PenaltyLink, frozen: true },
                    { name: "deny", index: "deny", width: 50, align: "center", sortable: false, formatter: Self.DenyRequestLink, frozen: true },
                    { key: false, hidden: true, name: "PenaltyID", sortable: true },
                    { key: true, width: 80, name: "RequestGUID", index: "10", editable: true, align: "left", sortable: true },
                    { key: false, width: 80, name: "RequestDateTime", index: "1", editable: true, align: "center", sortable: true, frozen: true, formatter: Self.FormatRequestDateTime },
                    { key: false, width: 80, name: "StartTime", index: "2", editable: true, align: "center", sortable: true, formatter: Self.FormatStartTime },
                    { key: false, width: 80, name: "EndTime", index: "3", editable: true, align: "center", sortable: true, formatter: Self.FormatEndTime },
                    { key: false, width: 90, name: "Status", index: "4", editable: true, align: "center", sortable: true, formatter: Self.StatusDots },
                    { key: false, width: 90, name: "Remarks", index: "5", editable: true, align: "center", sortable: true },
                    { key: false, width: 80, name: "ClaimedTime", index: "6", editable: true, align: "center", sortable: true, formatter: Self.FormatClaimedTime },
                    { key: false, width: 80, name: "ReturnedTime", index: "7", editable: true, align: "center", sortable: true, formatter: Self.FormatReturnedTime },
                    { key: false, width: 80, name: "Requestor", index: "8", editable: true, align: "center", sortable: true }
                ],
                toppager: $("#divPager"),
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
                emptyrecords: function() {
                    $("#gbox_tblRequestEquipmentList").css('display', 'none');
                    $(".empty-record-div").css('display', 'block');
                    $(".btnAddPage").css('display', 'none');
                    $(".accordion").css('display', 'none');
                    $(".panel").css('display', 'none');
                },
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
            $("#tblRequestEquipmentList").jqGrid("setFrozenColumns");
        },

        FormatRequestDateTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.RequestDateTime == '') {
                return 'N/A';
            }

            return moment(rowObject.RequestDateTime).format('DD-MMM-YY hh:mm A');
        },

        FormatStartTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.StartTime == '') {
                return 'N/A';
            }

            return moment(rowObject.StartTime).format('DD-MMM-YY hh:mm A');
        },

        FormatEndTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.EndTime == '') {
                return 'N/A';
            }

            return moment(rowObject.EndTime).format('DD-MMM-YY hh:mm A');
        },

        FormatClaimedTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.ClaimedTime == '') {
                return 'N/A';
            }

            return moment(rowObject.ClaimedTime).format('DD-MMM-YY hh:mm A');
        },

        FormatReturnedTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.ReturnedTime == '') {
                return 'N/A';
            }

            return moment(rowObject.ReturnedTime).format('DD-MMM-YY hh:mm A');
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-eye-open\"  onclick=\"return LoadPartial('" + viewEquipmentRequestURL + "?requestguid=" + rowObject.RequestGUID + "', 'divPartialContent');\"></a>";
            
        },

        PenaltyLink: function (cellvalue, options, rowObject) {
            var Self = this;
            if (rowObject.Status == "Completed" && rowObject.PenaltyID == 0) {
                return "<a href=\"\" class=\"glyphicon glyphicon-flag\"  onclick=\"return LoadPartial('" + addPenaltyURL + "?requestguid=" + rowObject.RequestGUID + "&requestType=Equipment" + "', 'divPartialContent');\"></a>";
            }
            else
                return "<a class=\"glyphicon glyphicon-ban-circle\"></a>";
        },

        DenyRequestLink: function (cellvalue, options, rowObject) {
            var Self = this;
            if (rowObject.Status == "Unclaimed") {
                return "<a class=\"glyphicon glyphicon-remove-sign\"  onclick=\"return ModalConfirmationCancel(MODAL_HEADER_DENYREQUEST, MSG_CONFIRM_DENYREQUEST, '" + rowObject.RequestGUID + "');\"></a>";
            }
            else {
                return "<a class=\"glyphicon glyphicon-ban-circle\"></a>";
            }
        },

        StatusDots: function (cellvalue, options, rowObject) {
            var Self = this;

            if (rowObject.Status == 'Unclaimed') {
                return "<div class='status-table'><span class='unclaimed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Claimed') {
                return "<div class='status-table'><span class='claimed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Completed') {
                return "<div class='status-table'><span class='completed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Cancelled') {
                return "<div class='status-table'><span class='cancelled'>" + rowObject.Status + "</span></div>";
            }
        },

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblRequestEquipmentList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblRequestEquipmentList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblRequestEquipmentList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['RequestEquipment_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['RequestEquipment_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['RequestEquipment_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['RequestEquipment_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

   
    }

    var claimReturnEquipmentList = Object.create(objclaimReturnEquipmentList);
    claimReturnEquipmentList.Initialized();
});

function CancelRequest(RequestGUID) {

    $.ajax({
        type: 'POST',
        url: window.cancelRequestEquipment,
        data: {
            RequestGUID: RequestGUID
        },
        dataType: "json",
        traditional: false,
        success: function (data) {
            ModalAlertSuccessfulCancel(MODAL_HEADER_SUCCESSFULLY_DENIED, MSG_SUCCESSFULLY_DENIED);
            $('#divErrorMessage').html('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        }
    });
}
