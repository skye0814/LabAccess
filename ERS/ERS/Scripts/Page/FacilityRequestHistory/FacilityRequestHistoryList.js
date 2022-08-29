$(document).ready(function () {

    var objRequestFacilityList = {
        Initialized: function () {
            var Self = this;

            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtDescription").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {
                FacilityRequestorID: $('#SystemUserID').val(),
                StatusMode: 'Completed&Cancelled'
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;

            $("#btnSearch").click(function () {
                var param = {
                    
                };
                Self.FillLocalStorage();
                localStorage['RequestFacility_PageNum'] = "1";
                localStorage['RequestFacility_RowNum'] = "10";
                Self.LoadTable(param);
            });


            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtDescription").val("");
                $("#txtRemarks").val("");
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    ids: $("#tblRequestFacilityList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);
            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    ids: $("#tblRequestFacilityList").jqGrid("getGridParam", "selarrrow")
                };
                if (input.ids.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
                }
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
                colNames: ["Action", "FacilityRequestID", "RequestFacilityGUID", "Facility", "Request Date", "Status", "Start Time", "End Time", "Remarks", "Claimed Date", "Returned Date", "Requestor"],
                colModel: [
                { name: "action", index: "action", width: 50, align: "center", sortable: false, formatter: Self.Actions, frozen: true },
                //{ name: "edit", index: "edit", width: 30, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                //{ name: "qrcode", index: "qrcode", width: 30, align: "center", sortable: false, formatter: Self.QrCodeLink, frozen: true },
                //{ name: "cancel", index: "cancel", width: 30, align: "center", sortable: false, formatter: Self.CancelRequestLink, frozen: true },
                { key: true, hidden: true, name: "FacilityRequestID"},
                { key: false, hidden: true, name: "RequestFacilityGUID"},
                { key: false, width: 50, name: "Facility", index: "1", editable: true, align: "center", sortable: false, frozen: true },
                { key: false, width: 70, name: "RequestDate", index: "2", editable: true, align: "center", sortable: true, formatter: Self.FormatRequestDateTime },
                { key: false, width: 80, name: "Status", index: "3", editable: false, align: "center", sortable: false, formatter: Self.StatusDots },
                { key: false, width: 70, name: "StartTime", index: "4", editable: true, align: "center", sortable: false, formatter: Self.FormatStartTime },
                { key: false, width: 70, name: "EndTime", index: "5", editable: true, align: "center", sortable: false, formatter: Self.FormatEndTime },
                { key: false, width: 80, name: "Remarks", index: "6", editable: true, align: "center", sortable: false },
                { key: false, width: 70, name: "ClaimedTime", index: "8", editable: true, align: "center", sortable: false, formatter: Self.FormatClaimedTime },
                { key: false, width: 70, name: "ReturnedTime", index: "9", editable: true, align: "center", sortable: false, formatter: Self.FormatReturnedTime },
                { key: false, width: 80, name: "FacilityRequestor", index: "7", editable: true, align: "center", sortable: false, hidden: false },
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
                emptyrecords: function () {
                    $("#gbox_tblRequestFacilityList").css('display', 'none');
                    $(".empty-record-div").css('display', 'block');
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

        //EditLink: function (cellvalue, options, rowObject) {
        //    var Self = this;
        //    return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?id=" + rowObject.FacilityRequestID + "', 'divPartialContent');\"></a>";
        //},

        //QrCodeLink: function (cellvalue, options, rowObject) {
        //    var Self = this;
        //    return "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateRequestFacilityQRCode('" + rowObject.RequestFacilityGUID + "','" + rowObject.FacilityID + "');\"></a>";
        //},

        //CancelRequestLink: function (cellvalue, options, rowObject) {
        //    var Self = this;

        //    if (rowObject.Status != "Claimed") {
        //        return "<a class=\"glyphicon glyphicon-remove-sign\"  onclick=\"return ModalConfirmationCancel(MODAL_HEADER_CANCELREQUEST, MSG_CONFIRM_CANCELREQUEST, '" + rowObject.FacilityRequestID + "');\"></a>";
        //    }
        //    else {
        //        return "<p></p>"
        //    }
        //},

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

        Actions: function (cellvalue, options, rowObject) {
            var editLink = "<a href=\"\" class=\"glyphicon glyphicon-eye-open\"  onclick=\"return LoadPartial('" + editPageURL + "?FacilityRequestID=" + rowObject.FacilityRequestID + "', 'divPartialContent');\"></a>";

            return "<div class='action-button'><span class='action-button-span'>" + editLink + "</span></div>";
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

        DeleteRecord: function (input) {
            var Self = this;
            $.ajax({
                url: deleteRequestFacilityURL,
                type: "POST",
                data: JSON.stringify(input),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    Loading(false);

                    if (!data.IsSuccess) {

                        var msg = "";
                        if (data.IsListResult) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        ModalAlert(MODAL_HEADER, msg);
                    } else {
                        $("#divErrorMessage").html("");
                        ModalAlert(MODAL_HEADER, data.Result);
                        $("#btnSearch").click();
                    }

                    $("#btnDeleteRecord").prop("disabled", false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                    $("#btnDeleteRecord").prop("disabled", false);
                },
            });
        }
    }

    var requestFacilityList = Object.create(objRequestFacilityList);
    requestFacilityList.Initialized();
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
            ModalAlertSuccessfulCancel(MODAL_HEADER_SUCCESSFULLY_CANCELLED, MSG_SUCCESSFULLY_CANCELLED);
            $('#divErrorMessage').html('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        }
    });
}

function GenerateRequestFacilityQRCode(RequestFacilityGUID, FacilityID) {
    $.ajax({
        url: window.generateQRCodeURL,
        type: "POST",
        data: JSON.stringify({ RequestFacilityGUID: RequestFacilityGUID }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess != true) {
                var msg = "";

                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                }
                else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
            else {

                IsEdit = false;
                var a = document.createElement("a"); //Create <a>
                a.href = data.Result;
                a.download = RequestFacilityGUID + '_' + FacilityID + '.png'; //File name Here
                a.click(); //Downloaded file
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        },
    });

    return false;
}


