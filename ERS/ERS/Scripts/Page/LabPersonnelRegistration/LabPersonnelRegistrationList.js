$(document).ready(function () {

    var objLabRegistrationRegistrationList = {
        Initialized: function () {
            var Self = this;

            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtFirstName").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {

                FirstName: $('#txtFirstName').val(),
                MiddleName: $('#txtMiddleName').val().trim(),
                LastName: $('#txtLastName').val().trim(),
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;

            $("#btnSearch").click(function () {
                var param = {
                    FirstName: $('#txtFirstName').val().trim(),
                    MiddleName: $('#txtMiddleName').val().trim(),
                    LastName: $('#txtLastName').val().trim(),
                };
                Self.FillLocalStorage();
                localStorage['LabPersonnelRegistration_PageNum'] = "1";
                localStorage['LabPersonnelRegistration_RowNum'] = "10";
                Self.LoadTable(param);
            });


            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtMiddleName").val("");
                $("#txtFirstName").val("");
                $("#txtLastName").val("");
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    SystemUserIDs: $("#tblLabPersonnelList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);

            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    SystemUserIDs: $("#tblLabPersonnelList").jqGrid("getGridParam", "selarrrow")
                };
                /*alert(input.SystemUserIDs);*/
                if (input.SystemUserIDs.length > 0) {
                    ModalConfirmation(MODAL_HEADER_ARCHIVE, MSG_CONFIRM_ARCHIVE, "btnDeleteRecord", "submit");
                }
            });


        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblLabPersonnelList").jqGrid("GridUnload");
            $("#tblLabPersonnelList").jqGrid("GridDestroy");

            $("#tblLabPersonnelList").jqGrid({
                url: getLabPersonnelListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Edit", "", "First Name", "Middle Name", "Last Name", 'Email Address', 'Username',"SystemUserID"],
                colModel: [{ name: "edit", index: "edit", width: 50, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                { key: false, hidden: true, name: "ID" },
                { key: false, name: "FirstName", index: "1", editable: true, align: "center", sortable: true },
                { key: false, name: "MiddleName", index: "2", editable: true, align: "center", sortable: true },
                { key: false, name: "LastName", index: "3", editable: true, align: "center", sortable: true },
                { key: false, name: "EmailAddress", index: "4", editable: true, align: "center", sortable: true },
                { key: false, name: "UserName", index: "5", editable: true, align: "center", sortable: true },
                { key: true, name: "SystemUserID", index: "6", editable: true, align: "center", sortable: true, hidden: true }
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['LabPersonnelRegistration_RowNum']),
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
                multiselect: true,
                rowNumbers: true,
                page: parseInt(localStorage['LabPersonnelRegistration_PageNum']),
                sortorder: localStorage['LabPersonnelRegistration_SortOrder'],
                sortname: localStorage['LabPersonnelRegistration_SortColumn'],
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
            $("#tblLabPersonnelList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-edit\"  onclick=\"return LoadPartial('" + editPageURL + "?id=" + rowObject.ID + "', 'divPartialContent');\"></a>";
        },

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblLabPersonnelList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblLabPersonnelList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblLabPersonnelList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['LabPersonnelRegistration_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['LabPersonnelRegistration_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['LabPersonnelRegistration_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['LabPersonnelRegistration_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

        DeleteRecord: function (input) {
            var Self = this;
            $.ajax({
                url: deleteLabPersonnelURL,
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
                        ModalAlert(MODAL_HEADER_ARCHIVE, data.Result);
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

    var labPersonnelRegistrationList = Object.create(objLabRegistrationRegistrationList);
    labPersonnelRegistrationList.Initialized();
});
