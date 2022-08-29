$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = "Home";
    header.innerHTML = `<div style='display:flex'>
                            <span style='padding-top: 1px'>CPE LabAccess</span>
                        </div>`;

    var objEquipmentItemList = {
        Initialized: function () {
            var Self = this;
           
            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtEquipmentItemCode").focus() }, 500);

            Self.FillFieldWithLocalStorage();

                var param = {
               
                    Category: $('#txtCategory').val(),
                    EquipmentItemCode: $('#txtEquipmentItemCode').val().trim(),
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
      
            $("#btnSearch").click(function () {
                var param = {
                    Category: $('#txtCategory').val(),
                    EquipmentItemCode: $('#txtEquipmentItemCode').val().trim(),
                };
                Self.FillLocalStorage();
                localStorage['EquipmentItem_PageNum'] = "1";
                localStorage['EquipmentItem_RowNum'] = "10";
                Self.LoadTable(param);
            });

          
            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtCategory").val("");
                $("#txtEquipmentItemCode").val("");
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    ids: $("#tblEquipmentItemList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);
               
            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    ids: $("#tblEquipmentItemList").jqGrid("getGridParam", "selarrrow")
                };
                if (input.ids.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
                }
            });

           
        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblEquipmentItemList").jqGrid("GridUnload");
            $("#tblEquipmentItemList").jqGrid("GridDestroy");

            $("#tblEquipmentItemList").jqGrid({
                url: getEquipmentItemListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Edit", "QR", "", "Active", "Category", "Item Code", "Brand", "Model", "Serial Number", "Date Bought", "Warranty Status", "Usable", "Status", "Times Borrowed", "Comments"],
                colModel: [
                    { name: "edit", index: "edit", width: 35, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { name: "qrcode", index: "qrcode", width: 35, align: "center", sortable: false, formatter: Self.QrCodeLink, frozen: true },
                    { key: true, hidden: true, name: "EquipmentItemID" },
                    { key: false, width: 70, name: "isActive", index: "1", editable: true, align: "center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 80, name: "Category", index: "2", editable: true, align: "center", sortable: true },
                    { key: false, width: 80, name: "EquipmentItemCode", index: "3", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 70, name: "ItemBrand", index: "4", editable: true, align: "center", sortable: true },
                    { key: false, width: 70, name: "ItemModel", index: "5", editable: true, align: "center", sortable: true },
                    { key: false, width: 70, name: "ItemSerialNumber", index: "6", editable: true, align: "center", sortable: true },
                    { key: false, width: 70, name: "DateBought", index: "7", editable: true, align: "center", sortable: true, formatter: Self.FormatDateBought },
                    { key: false, width: 80, name: "WarrantyStatus", index: "8", editable: true, align: "center", sortable: true, formatter: Self.WarrantyStatusString },
                    { key: false, width: 70, name: "isUsable", index: "9", editable: true, align: "center", sortable: true, formatter: Self.isUsableString },
                    { key: false, width: 70, name: "Status", index: "10", editable: true, align: "center", sortable: true },
                    { key: false, width: 70, name: "NoOfTimesBorrowed", index: "11", editable: true, align: "center", sortable: true },
                    { key: false, name: "Comments", index: "12", editable: true, align: "center", sortable: true }
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['EquipmentItem_RowNum']),
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
                page: parseInt(localStorage['EquipmentItem_PageNum']),
                sortorder: localStorage['EquipmentItem_SortOrder'],
                sortname: localStorage['EquipmentItem_SortColumn'],
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
            $("#tblEquipmentItemList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?EquipmentItemID=" + rowObject.EquipmentItemID + "', 'divPartialContent');\"></a>";
        },

        FormatDateBought: function (cellvalue, options, rowObject) {
            var Self = this;
            moment.suppressDeprecationWarnings = true;

            if (rowObject.DateBought == '') {
                return 'N/A';
            }

            return moment(rowObject.DateBought).format('DD-MMM-YY');
        },

        isActiveString: function (cellvalue, options, rowObject) {
            if (rowObject.isActive) {
                return "Active";
            }
            else
                return "Inactive";
        },

        WarrantyStatusString: function (cellvalue, options, rowObject) {
            if (rowObject.WarrantyStatus) {
                return "Under Warranty";
            }
            else
                return "Warranty Expired";
        },

        isUsableString: function (cellvalue, options, rowObject) {
            if (rowObject.isUsable) {
                return "Servicable";
            }
            else
                return "Unserviceable";
        },

        QrCodeLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-qrcode\"  onclick=\"return GenerateEquipmentItemQRCode('" + rowObject.Category + "','" + rowObject.EquipmentItemCode + "');\"></a>";
        },

        FillLocalStorage: function () {
            var Self = this;
           
        },

        FillFieldWithLocalStorage: function () {
            var Self = this;
          
        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblEquipmentItemList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblEquipmentItemList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblEquipmentItemList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['EquipmentItem_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['EquipmentItem_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['EquipmentItem_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['EquipmentItem_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

        DeleteRecord : function (input)
        {
            var Self = this;
            $.ajax({
                url: deleteStudentURL,
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
        }
    }
  
    var equipmentItemList = Object.create(objEquipmentItemList);
        equipmentItemList.Initialized();
});

function GenerateEquipmentItemQRCode(Category, EquipmentItemCode) {

    $.ajax({
        url: window.generateQRCodeURL,
        type: "POST",
        data: JSON.stringify({ EquipmentItemCode: EquipmentItemCode }),
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
                a.download = Category + '_' + EquipmentItemCode + '.png'; //File name Here
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

