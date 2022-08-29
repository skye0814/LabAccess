$(document).ready(function () {

    var objPenaltyList = {
        Initialized: function () {
            var Self = this;
            $("#divErrorMessage").html("");
            Self.BindEvent();
            Self.FillFieldWithLocalStorage();
            var param = {
                RequestType: $('#RequestType').val(),
                Requestor: $('#txtStudentName').val()
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
      
            $("#btnSearch").click(function () {
                var param = {
                    RequestType: $('#RequestType').val(),
                    Requestor: $('#txtStudentName').val()
                };
                Self.FillLocalStorage();
                localStorage['Penalty_PageNum'] = "1";
                localStorage['Penalty_RowNum'] = "10";
                Self.LoadTable(param);
            });
            $("#btnReset").click(function () {
                $('#RequestType').val("");
                $('#txtStudentName').val("");
                $("#btnSearch").click();
            });
            $('#ddlRequestType').change(function () {
                var param = {
                    RequestType: $('#ddlRequestType').val() + "",
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);

            });
        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblPenaltyList").jqGrid("GridUnload");
            $("#tblPenaltyList").jqGrid("GridDestroy");

            $("#tblPenaltyList").jqGrid({
                url: getPenaltyListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Action", "", "Status", "Requestor","Request Type", "Request GUID" ],
                colModel: [
                    { name: "edit", index: "edit", width: 30, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { key: true, hidden: true, name: "PenaltyID" },
                    { key: false, width: 40, name: "isActive", index: "1", editable: true, align: "Center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 50, name: "Requestor", index: "2", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 50, name: "RequestType", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, width: 50, name: "RequestGUID", index: "4", editable: true, align: "center", sortable: true },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['Penalty_RowNum']),
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
                    $("#gbox_tblPenaltyList").css('display', 'none');
                    $(".empty-record-div").css('display', 'block');
                },
                multiselect: false,
                rowNumbers: true,
                page: parseInt(localStorage['Penalty_PageNum']),
                sortorder: localStorage['Penalty_SortOrder'],
                sortname: localStorage['Penalty_SortColumn'],
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
            $("#tblPenaltyList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?PenaltyID=" + rowObject.PenaltyID + "', 'divPartialContent');\"></a>";
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
            var sortColumn = $("#tblPenaltyList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblPenaltyList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblPenaltyList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['Penalty_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['Penalty_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['Penalty_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['Penalty_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },
    }
  
    var PenaltyList = Object.create(objPenaltyList);
        PenaltyList.Initialized();
});


