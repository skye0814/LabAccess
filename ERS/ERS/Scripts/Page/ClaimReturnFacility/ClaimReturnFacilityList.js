$(document).ready(function () {

    var objclaimReturnFacilityList = {
        Initialized: function () {
            var Self = this;

            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtStudentNumber").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {
                Requestor: $('#txtRequestor').val(),
                RequestFacilityGUID: $('#txtRequestGUID').val(),
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
                    RequestFacilityGUID: $('#txtRequestGUID').val(),
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
                $('#txtRequestGUID').val("");
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

            $("#tblRequestFacilityList").jqGrid("GridUnload");
            $("#tblRequestFacilityList").jqGrid("GridDestroy");

            $("#tblRequestFacilityList").jqGrid({
                url: getRequestFacilityListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["View", "Penalty", "Deny", "FacilityID", "", "GUID", "Facility", "Request Date", "Start Time", "End Time", "Status","Remarks","Claimed Date","Return Date","Requestor"],
                colModel: [
                    { name: "edit", index: "edit", width: 50, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { name: "penalty", index: "penalty", width: 50, align: "center", sortable: false, formatter: Self.PenaltyLink, frozen: true },
                    { name: "deny", index: "deny", width: 50, align: "center", sortable: false, formatter: Self.DenyRequestLink, frozen: true },
                    { key: true, hidden: true, name: "FacilityRequestID", align: "center" },
                    { key: false, hidden: true, name: "PenaltyID" },
                    { key: true, width: 80, name: "RequestFacilityGUID", index: "8", editable: true, align: "left", sortable: true },
                    { key: false, width: 80, name: "Facility", index: "1", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 80, name: "RequestDate", index: "2", editable: true, align: "center", sortable: true, formatter: Self.FormatRequestDateTime },
                    { key: false, width: 80, name: "StartTime", index: "3", editable: true, align: "center", sortable: true, formatter: Self.FormatStartTime },
                    { key: false, width: 80, name: "EndTime", index: "4", editable: true, align: "center", sortable: true, formatter: Self.FormatEndTime },
                    { key: false, width: 90, name: "Status", index: "6", editable: true, align: "center", sortable: true, formatter: Self.StatusDots },
                    { key: false, width: 90, name: "Remarks", index: "7", editable: true, align: "center", sortable: true },
                    { key: false, width: 80, name: "ClaimedTime", editable: true, align: "center", sortable: true, formatter: Self.FormatClaimedTime },
                    { key: false, width: 80, name: "ReturnedTime", editable: true, align: "center", sortable: true, formatter: Self.FormatReturnedTime },
                    { key: false, width: 80, name: "FacilityRequestor", index: "5", editable: true, align: "center", sortable: true },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['RequestFacility_RowNum']),
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
                    $("#gbox_tblRequestFacilityList").css('display', 'none');
                    $(".empty-record-div").css('display', 'block');
                    $(".btnAddPage").css('display', 'none');
                    $(".accordion").css('display', 'none');
                    $(".panel").css('display', 'none');
                },
                multiselect: false,
                rowNumbers: true,
                page: parseInt(localStorage['RequestFacility_PageNum']),
                sortorder: localStorage['RequestFacility_SortOrder'],
                sortname: localStorage['RequestFacility_SortColumn'],
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
            $("#tblRequestFacilityList").jqGrid("setFrozenColumns");
        },

        FormatRequestDateTime: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.RequestDate == '') {
                return 'N/A';
            }

            return moment(rowObject.RequestDate).format('DD-MMM-YY hh:mm A');
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

        StatusDots: function (cellvalue, options, rowObject){
            var Self = this;

            if (rowObject.Status == 'Unclaimed') {
                return "<div class='status-table'><span class='unclaimed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Claimed') {
                return "<div class='status-table'><span class='claimed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Completed' || rowObject.Status == 'Returned') {
                return "<div class='status-table'><span class='completed'>" + rowObject.Status + "</span></div>";
            }
            if (rowObject.Status == 'Cancelled') {
                return "<div class='status-table'><span class='cancelled'>" + rowObject.Status + "</span></div>";
            }
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;


            return "<a href=\"\" class=\"glyphicon glyphicon-eye-open\"  onclick=\"return LoadPartial('" + viewFacilityRequestURL + "?requestguid=" + rowObject.RequestFacilityGUID + "', 'divPartialContent');\"></a>";
        },

        PenaltyLink: function (cellvalue, options, rowObject) {
            var Self = this;
            if (rowObject.Status == "Completed" && rowObject.PenaltyID == 0) {
                return "<a href=\"\" class=\"glyphicon glyphicon-flag\"  onclick=\"return LoadPartial('" + addPenaltyURL + "?requestguid=" + rowObject.RequestFacilityGUID + "&requestType=Facility" + "', 'divPartialContent');\"></a>";
            }
            else
                return "<a class=\"glyphicon glyphicon-ban-circle\"></a>";
        },

        DenyRequestLink: function (cellvalue, options, rowObject) {
            var Self = this;
            if (rowObject.Status == "Unclaimed") {
                return "<a class=\"glyphicon glyphicon-remove-sign\"  onclick=\"return ModalConfirmationCancel(MODAL_HEADER_DENYREQUEST, MSG_CONFIRM_DENYREQUESTFACILITY, '" + rowObject.FacilityRequestID + "');\"></a>";
            }
            else {
                return "<a class=\"glyphicon glyphicon-ban-circle\"></a>";
            }
        },

        QrCodeLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateFacilityItemQRCode('" + rowObject.RequestGUID + "');\"></a>";
        },

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblRequestFacilityList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblRequestFacilityList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblRequestFacilityList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['RequestFacility_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['RequestFacility_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['RequestFacility_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['RequestFacility_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

   
    }

    var claimReturnFacilityList = Object.create(objclaimReturnFacilityList);
    claimReturnFacilityList.Initialized();
});

function CancelRequest(FacilityRequestID) {

    $.ajax({
        type: 'POST',
        url: window.cancelRequestFacility,
        data: {
            FacilityRequestID: FacilityRequestID
        },
        dataType: "json",
        traditional: false,
        success: function (data) {
            ModalAlertSuccessfulCancel(MODAL_HEADER_SUCCESSFULLY_DENIED, MSG_SUCCESSFULLY_CANCELLED);
            $('#divErrorMessage').html('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        }
    });
}