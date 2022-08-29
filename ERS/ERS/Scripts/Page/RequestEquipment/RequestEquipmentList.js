$(document).ready(function () {

    var objRequestEquipmentList = {

        Initialized: function () {
            var Self = this;
           
            $("#divErrorMessage").html("");

            Self.BindEvent();

            

            Self.FillFieldWithLocalStorage();

            var param = {
                RequestorID: $('#SystemUserID').val(),
                StatusMode: 'Unclaimed&Claimed'
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
      
            $("#btnSearch").click(function () {
                var param = {
                    Remarks: $('#txtRemarks').val(),
                    StartTime: $('#txtStartTime').val().trim(),
                    EndTime: $('#txtEndTime').val(),
                    Requestor: $('#txtRequestor').val(),
                    RequestDateTime: $('#txtRequestDateTime').val() 
                };
                Self.FillLocalStorage();
                localStorage['RequestEquipment_PageNum'] = "1";
                localStorage['RequestEquipment_RowNum'] = "10";
                Self.LoadTable(param);
            });

          
            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtRemarks").val("");
                $("#txtStartTime").val("");
                $("#txtEndTime").val("");
                $("#txtRequestor").val("");
                $("#txtRequestDateTime").val("");
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    ids: $("#tblRequestEquipmentList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);
            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    ids: $("#tblRequestEquipmentList").jqGrid("getGridParam", "selarrrow")
                };
                if (input.ids.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
                }
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
                colNames: ["Action","RequestGUID","Request Date","Start Date","End Time","Status","Remarks","Claimed Date","Return Date","Requestor"],
                colModel: [
                    { name: "action", index: "action", width: 80, align: "center", sortable: false, formatter: Self.Actions, frozen: true },
                    { key: true, hidden: true, name: "RequestGUID", sortable: true },
                    { key: false, width: 75, name: "RequestDateTime", index: "1", editable: false, align: "center", sortable: true, formatter: Self.FormatRequestDateTime },
                    { key: false, width: 75, name: "StartTime", index: "2", editable: true, align: "center", sortable: true, formatter: Self.FormatStartTime },
                    { key: false, width: 75, name: "EndTime", index: "3", editable: true, align: "center", sortable: true, formatter: Self.FormatEndTime },
                    { key: false, width: 80, name: "Status", index: "4", editable: false, align: "center", sortable: false, formatter: Self.StatusDots},
                    { key: false, width: 60, name: "Remarks", index: "5", editable: true, align: "center", sortable: true },
                    { key: false, width: 75, name: "ClaimedTime", index: "6", editable: true, align: "center", sortable: true, formatter: Self.FormatClaimedTime },
                    { key: false, width: 75, name: "ReturnedTime", index: "7", editable: true, align: "center", sortable: true, hidden: true },
                    { key: false, width: 80, name: "Requestor", index: "8", editable: true, align: "center", sortable: true, hidden: true }
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
                emptyrecords: function () {
                    $("#gbox_tblRequestEquipmentList").css('display', 'none');
                    $(".empty-record-div").css('display', 'block');
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
            var editLink = "<a href=\"\" class=\"glyphicon glyphicon-eye-open\"  onclick=\"return LoadPartial('" + editPageURL + "?requestguid=" + rowObject.RequestGUID + "', 'divPartialContent');\"></a>";
            var qrCodeLink = "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateEquipmentItemQRCode('" + rowObject.RequestGUID + "');\"></a>";

            if (rowObject.Status != "Claimed") {
                var cancelLink = "<a class=\"glyphicon glyphicon-remove-sign\"  onclick=\"return ModalConfirmationCancel(MODAL_HEADER_CANCELREQUEST, MSG_CONFIRM_CANCELREQUEST, '" + rowObject.RequestGUID + "');\"></a>";
            }
            else {
                var cancelLink = "";
            }

            return "<div class='action-button'><span class='action-button-span'>" + editLink + "" + qrCodeLink + "" + cancelLink + "</span></div>";
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;

            
            return "<a href=\"\" class=\"glyphicon glyphicon-eye-open\"  onclick=\"return LoadPartial('" + editPageURL + "?requestguid=" + rowObject.RequestGUID + "', 'divPartialContent');\"></a>";
            //return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadTableEdit('" + "" + "" + rowObject.RequestGUID + "');\"></a>";
            //return "<button onclick=\"objRequestEquipmentList.LoadTableEdit('" + "" + "" + rowObject.RequestGUID + "');\">Button</button>"
        },

        CancelRequestLink: function (cellvalue, options, rowObject) {
            var Self = this;

            if (rowObject.Status != "Claimed") {
                return "<a class=\"glyphicon glyphicon-remove-sign\"  onclick=\"return ModalConfirmationCancel(MODAL_HEADER_CANCELREQUEST, MSG_CONFIRM_CANCELREQUEST, '" + rowObject.RequestGUID + "');\"></a>";
            }
            else {
                return "<p></p>"
            }
        },

        QrCodeLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateEquipmentItemQRCode('" + rowObject.RequestGUID + "');\"></a>";
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

        DeleteRecord : function (input)
        {
            var Self = this;
            $.ajax({
                url: deleteRequestEquipmentURL,
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
  
    var requestEquipmentList = Object.create(objRequestEquipmentList);
    requestEquipmentList.Initialized();
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
            ModalAlertSuccessfulCancel(MODAL_HEADER_SUCCESSFULLY_CANCELLED, MSG_SUCCESSFULLY_CANCELLED);
            $('#divErrorMessage').html('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        }
    });
}

function GenerateEquipmentItemQRCode(RequestGUID) {

    $.ajax({
        url: window.generateQRCodeURL,
        type: "POST",
        data: JSON.stringify({ RequestGUID: RequestGUID }),
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
                a.download = RequestGUID + '.png'; //File name Here
                a.click(); //Downloaded file
                //var div = document.getElementById("field-content");
                //div.innerHTML = "<img src='" + data.Result + "'/>";
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        },
    });

    return false;
}
