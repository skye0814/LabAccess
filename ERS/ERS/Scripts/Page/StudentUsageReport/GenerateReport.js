$(document).ready(function () {

    var objStudentUsage = {

        Initialize: function () {
            var Self = this;

            $("#divErrorMessage").html("");
            var pdata = {
                DateFrom: $("#dpDateFrom").val(),
                DateTo: $("#dpDateTo").val(),
            };
            Self.LoadTable(pdata);
            Self.ElementEvents();
            Self.ButtonBinding();
        },

        ElementEvents: function () {
            var Self = this;


            $('#dpDateFrom, #dpDateTo').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: false,
            })

        },

        ButtonBinding: function () {
            var Self = this;

            $("#btnGenerate").click(function () {
                $("#divErrorMessage").html("");

                resultDate = Self.ValidateSearchDate();

                if (resultDate == true) {
                    Self.StudentUsageParameter("EXCEL", "DOWNLOAD");
                }
                return false;
            });

            $('#dpDateFrom, #dpDateTo').datepicker({
                onSelect: function (dateText) {
                }
            }).on("change", function () {
                var pdata = {
                    DateFrom: $("#dpDateFrom").val(),
                    DateTo: $("#dpDateTo").val(),
                };
                Self.LoadTable(pdata);
            });
        },


        StudentUsageParameter: function (fileType, result) {
            var Self = this;

            Loading(true);
            var pdata = {
                DateFrom: $("#dpDateFrom").val(),
                DateTo: $("#dpDateTo").val(),
            };


            $('#divModal').modal("hide");
            HideExportModal();

            DownloadReportFile(StudentUsageDownloadURL, pdata);

            Loading(false);

        },

        ValidateSearchDate: function () {
            var Self = this;

            var datevalid = /\d{1,2}\/\d{1,2}\/\d{4}$/;

            if ($('#dpDateFrom').val() == '' && $('#dpDateTo').val() == '') {
                return true;
            }
            else {
                var dateFrom = $('#dpDateFrom').val();
                var dateTo = $('#dpDateTo').val();
                //Validation when only Date From has no value
                if ($('#dpDateFrom').val() == '' && $('#dpDateTo').val() != '') {
                    $('#divErrorMessage').html("Date From" + SUFF_REQUIRED);
                    return false;
                }
                //Validation when only Date To has no value

                else if (Date.parse(dateFrom) > Date.parse(dateTo)) {
                    $('#divErrorMessage').html("Date From must not be later than Date To. ");
                    return false;
                }
                else if ($('#dpDateTo').val() != '' && !$("#dpDateTo").val().match(datevalid)) {
                    $('#divErrorMessage').html("Date To must be in mm/dd/yyyy format.");
                    return false;
                }
                else if (!$("#dpDateFrom").val().match(datevalid)) {
                    $('#divErrorMessage').html("Date From must be in mm/dd/yyyy format.");
                    return false;
                }
                else {
                    $("#divErrorMessage").html("");
                    return true;
                }
            }
        },

        //Display Report
        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblStudentUsageList").jqGrid("GridUnload");
            $("#tblStudentUsageList").jqGrid("GridDestroy");

            $("#tblStudentUsageList").jqGrid({
                url: getStudentUsageListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Year Level", "Times Borrowed"],
                colModel: [
                    { key: false, width: 50, name: "YearLevel", index: "1", editable: false, align: "center", sortable: false },
                    { key: false, width: 50, name: "NoOfTimesBorrowed", index: "2", editable: false, align: "center", sortable: false },
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['StudentUsageReport_RowNum']),
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
                page: parseInt(localStorage['StudentUsageReport_PageNum']),
                sortorder: localStorage['StudentUsageReport_SortOrder'],
                sortname: localStorage['StudentUsageReport_SortColumn'],
                width: "100%",
                height: "100%",
                sortable: false,
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
            $("#tblStudentUsageReportList").jqGrid("setFrozenColumns");
        },

        FillLocalStorage: function () {
            var Self = this;

        },

        FillFieldWithLocalStorage: function () {
            var Self = this;

        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblStudentUsageList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblStudentUsageList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblStudentUsageList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['StudentUsageReport_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['StudentUsageReport_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['StudentUsageReport_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['StudentUsageReport_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },

    }//objStudentUsage

    var studentUsage = Object.create(objStudentUsage);
    studentUsage.Initialize();

});
