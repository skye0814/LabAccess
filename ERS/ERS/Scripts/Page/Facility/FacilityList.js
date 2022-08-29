$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = "Home";
    header.innerHTML = `<div style='display:flex'>
                            <span style='padding-top: 1px'>CPE LabAccess</span>
                        </div>`;

    var objFacilityList = {
        Initialized: function () {
            var Self = this;

            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtRoomNumber").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {

                RoomNumber: $('#txtRoomNumber').val(),
                RoomType: $('#txtRoomType').val().trim(),
                isAvailable: $('#chkisAvailable').is(":checked") ? true : false
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;

            $("#btnSearch").click(function () {
                var param = {
                    RoomNumber: $('#txtRoomNumber').val(),
                    RoomType: $('#txtRoomType').val().trim(),
                };
                Self.FillLocalStorage();
                localStorage['Facility_PageNum'] = "1";
                localStorage['Facility_RowNum'] = "10";
                Self.LoadTable(param);
            });


            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtRoomNumber").val("");
                $("#txtRoomType").val("");
                $('#txtAvailable').val("");
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    ids: $("#tblFacilityList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);

            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    ids: $("#tblFacilityList").jqGrid("getGridParam", "selarrrow")
                };
                if (input.ids.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
                }
            });


        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblFacilityList").jqGrid("GridUnload");
            $("#tblFacilityList").jqGrid("GridDestroy");

            $("#tblFacilityList").jqGrid({
                url: getFacilityListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Edit", "QR", "", "Active", "Room Number", "Room Type", "Description", "Availability", "Times Booked", "Comments"],
                colModel: [
                    { name: "edit", index: "edit", width: 40, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { name: "qrcode", index: "qrcode", width: 40, align: "center", sortable: false, formatter: Self.QrCodeLink, frozen: true },
                    { key: true, hidden: true, name: "FacilityID" },
                    { key: false, width: 60, name: "isActive", index: "1", editable: true, align: "center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 60, name: "RoomNumber", index: "2", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 60, name: "RoomType", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, hidden: true, width: 60, name: "RoomDescription", index: "4", editable: false, align: "center", sortable: true },
                    { key: false, width: 50, name: "isAvailable", index: "5", editable: false, align: "center", sortable: true, formatter: Self.isAvailableString },
                    { key: false, width: 50, name: "NoOfTimesBooked", index: "6", editable: false, align: "center", sortable: true, hidden: true },
                    { key: false, width: 80, name: "Comments", index: "7", editable: true, align: "center", sortable: true },
                   // { key: false, width: 30, name: "TimeIn", index: "8", editable: true, align: "Left", sortable: true },
                   // { key: false, width: 30, name: "TimeOut", index: "9", editable: true, align: "Left", sortable: true },
                    //{ key: false, width: 30, name: "NextSchedule", index: "8", editable: true, align: "Left", sortable: true },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['Facility_RowNum']),
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
                page: parseInt(localStorage['Facility_PageNum']),
                sortorder: localStorage['Facility_SortOrder'],
                sortname: localStorage['Facility_SortColumn'],
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
            $("#tblFacilityList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?FacilityID=" + rowObject.FacilityID + "', 'divPartialContent');\"></a>";
        },

        isActiveString: function (cellvalue, options, rowObject) {
            if (rowObject.isActive) {
                return "Active";
            }
            else
                return "Inactive";
        },

        isAvailableString: function (cellvalue, options, rowObject) {
            if (rowObject.isActive) {
                return "Available";
            }
            else
                return "Unavailable";
        },

        QrCodeLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateFacilityQRCode('" + rowObject.RoomNumber + "','" + rowObject.RoomType +"');\"></a>";
        },

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblFacilityList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblFacilityList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblFacilityList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['Facility_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['Facility_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['Facility_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['Facility_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

        DeleteRecord: function (input) {
            var Self = this;
            $.ajax({
                url: deleteFacilityURL,
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
                        $("#btnReset").click();

                    }

                    $("#btnDeleteRecord").prop("disabled", false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                    $("#btnDeleteRecord").prop("disabled", false);
                },
            });
        },

    }

    var FacilityList = Object.create(objFacilityList);
    FacilityList.Initialized();


});


function GenerateFacilityQRCode(RoomNumber,RoomType) {

    $.ajax({
        url: window.generateQRCodeURL,
        type: "POST",
        data: JSON.stringify({ RoomNumber: RoomNumber }),
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
                a.download = RoomType + '_' + RoomNumber + '.png'; //File name Here
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