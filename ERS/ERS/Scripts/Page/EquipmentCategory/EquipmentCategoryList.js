$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = "Home";
    header.innerHTML = `<div style='display:flex'>
                            <span style='padding-top: 1px'>CPE LabAccess</span>
                        </div>`;

    var objEquipmentCategoryList = {
        Initialized: function () {
            var Self = this;
            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtCategory").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {
                Category: $('#txtCategory').val(),
                CategoryCode: $('#txtCategoryCode').val().trim(),
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
      
            $("#btnSearch").click(function () {
                var param = {
                    Category: $('#txtCategory').val().trim(),
                    CategoryCode: $('#txtCategoryCode').val().trim(),
                };
                Self.FillLocalStorage();
                localStorage['EquipmentCategory_PageNum'] = "1";
                localStorage['EquipmentCategory_RowNum'] = "10";
                Self.LoadTable(param);
            });

          
            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtCategory").val("");
                $("#txtCategoryCode").val("");
                $("#btnSearch").click();
            });

            //$("#btnDeleteRecord").on("submit", function () {
            //    Loading(true);

            //    var input = {
            //        ids: $("#tblEquipmentCategoryList").jqGrid("getGridParam", "selarrrow")
            //    };

            //    Self.DeleteRecord(input);

            //    $("#btnDelete").prop("disabled", true);
               
            //});

            //$("#btnDeleteRecord").click(function () {
            //    var input = {
            //        ids: $("#tblEquipmentCategoryList").jqGrid("getGridParam", "selarrrow")
            //    };
            //    if (input.ids.length > 0) {
            //        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
            //    }
            //});

           
        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblEquipmentCategoryList").jqGrid("GridUnload");
            $("#tblEquipmentCategoryList").jqGrid("GridDestroy");

            $("#tblEquipmentCategoryList").jqGrid({
                url: getEquipmentCategoryListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Edit", "", "Status", "Category","Category Code", "Total", "Usable", "Defective", "Missing", "Times Borrowed", "Comments" ],
                colModel: [
                    { name: "edit", index: "edit", width: 40, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { key: true, hidden: true, name: "EquipmentCategoryID" },
                    { key: false, width: 50, name: "isActive", index: "1", editable: true, align: "Center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 80, name: "Category", index: "2", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 50, name: "CategoryCode", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "QuantityTotal", index: "4", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "QuantityUsable", index: "5", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "QuantityDefective", index: "6", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "QuantityMissing", index: "7", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "NoOfTimesBorrowed", index: "8", editable: true, align: "center", sortable: true },
                    { key: false, name: "Comments", index: "9", editable: true, align: "center", sortable: true },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['EquipmentCategory_RowNum']),
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
                page: parseInt(localStorage['EquipmentCategory_PageNum']),
                sortorder: localStorage['EquipmentCategory_SortOrder'],
                sortname: localStorage['EquipmentCategory_SortColumn'],
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
            $("#tblEquipmentCategoryList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?EquipmentCategoryID=" + rowObject.EquipmentCategoryID + "', 'divPartialContent');\"></a>";
        },

        isActiveString: function (cellvalue, options, rowObject) {
            if (rowObject.isActive) {
                return "Active";
            }
            else
                return "Inactive";
        },

        FillLocalStorage: function () {
            var Self = this;
           
        },

        FillFieldWithLocalStorage: function () {
            var Self = this;
          
        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblEquipmentCategoryList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblEquipmentCategoryList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblEquipmentCategoryList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['EquipmentCategory_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['EquipmentCategory_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['EquipmentCategory_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['EquipmentCategory_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

        //DeleteRecord : function (input)
        //{
        //    var Self = this;
        //    $.ajax({
        //        url: deleteStudentURL,
        //        type: "POST",
        //        data: JSON.stringify(input),
        //        dataType: "json",
        //        contentType: "application/json; charset=utf-8",
        //        success: function (data) {
        //            Loading(false);

        //            if (!data.IsSuccess) {

        //                var msg = "";
        //                if (data.IsListResult) {
        //                    for (var i = 0; i < data.Result.length; i++) {
        //                        msg += data.Result[i] + "<br />";
        //                    }
        //                } else {
        //                    msg += data.Result;
        //                }
        //                ModalAlert(MODAL_HEADER, msg);
        //            } else {
        //                $("#divErrorMessage").html("");
        //                ModalAlert(MODAL_HEADER, data.Result);
        //                $("#btnReset").click();

        //            }

        //            $("#btnDeleteRecord").prop("disabled", false);
        //        },
        //        error: function (jqXHR, textStatus, errorThrown) {
        //            Loading(false);
        //            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        //            $("#btnDeleteRecord").prop("disabled", false);
        //        },
        //    });
        //}
    }
  
    var equipmentCategoryList = Object.create(objEquipmentCategoryList);
        equipmentCategoryList.Initialized();
});


