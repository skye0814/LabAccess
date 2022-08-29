$(document).ready(function () {

    var objstudentRegistrationList = {
        Initialized: function () {
            var Self = this;
           
            $("#divErrorMessage").html("");

            Self.BindEvent();

            setTimeout(function () { $("#txtStudentNumber").focus() }, 500);

            Self.FillFieldWithLocalStorage();

            var param = {
               
                StudentNumber: $('#txtStudentNumber').val(),
                FirstName: $('#txtFirstName').val(),
                LastName: $('#txtLastName').val(),
                isArchive: true
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        BindEvent: function () {
            var Self = this;
      
            $("#btnSearch").click(function () {
                var param = {
                    StudentNumber: $('#txtStudentNumber').val(),
                    FirstName: $('#txtFirstName').val(),
                    LastName: $('#txtLastName').val(),
                    isArchive: true
                };
                Self.FillLocalStorage();
                localStorage['StudentRegistration_PageNum'] = "1";
                localStorage['StudentRegistration_RowNum'] = "10";
                Self.LoadTable(param);
            });

          
            $("#btnAddPage").click(function () {
                Self.FillLocalStorage();
                LoadPartial(addPageURL, "divPartialContent");
            });

            $("#btnReset").click(function () {
                $("#txtStudentNumber").val("");
                $("#txtFirstName").val("");
                $("#txtLastName").val("");
                isArchive: true
                $("#btnSearch").click();
            });

            $("#btnDeleteRecord").on("submit", function () {
                Loading(true);

                var input = {
                    SystemUserIDs: $("#tblStudentList").jqGrid("getGridParam", "selarrrow")
                };

                Self.DeleteRecord(input);

                $("#btnDelete").prop("disabled", true);
            });

            $("#btnDeleteRecord").click(function () {
                var input = {
                    SystemUserIDs: $("#tblStudentList").jqGrid("getGridParam", "selarrrow")
                };
                /*alert(input.SystemUserIDs);*/
                if (input.SystemUserIDs.length > 0) {
                    ModalConfirmation(MODAL_HEADER_DELETE, MSG_CONFIRM_DELETE, "btnDeleteRecord", "submit");
                }
            });

            $("#btnUpload").click(function () {
                RemoveUploadHighlights();
                ModalUpload("", "", window.formStudentRegistrationURL, "");
                $('#btnUploadFile').click(function () {
                    Self.UploadFile();
                });
            });

            // For valiation of file implement a change function for fileUpload
            $("#fileUpload").change(function () {
                RemoveUploadHighlights();
                var file;
                file = $("#fileUpload")[0].files[0];

                if (!(/\.(xls|xlsx|xlsm)$/i).test(file.name)) {
                    $("#fileUpload").val("");
                    $("#divModalErrorMessage").html(SELECT_EXCEL_FILE);
                    HighlightFieldsUpload(1);
                }
                else {
                    if (file.size > 25000000) {
                        $("#fileUpload").val("");
                        $("#divModalErrorMessage").html(EXCEL_FILE_SIZE);
                        HighlightFieldsUpload(1);
                    }
                }
            });

            $("#btnSaveUploadContent").click(function () {
                Loading(true);

                $("#btnSaveUploadContent").prop("disabled", true);

                var input = {
                    contentSessionId: $("#hdnContentSessionId").val()
                };

                $.ajax({
                    url: window.saveUploadedStudentRegistrationURL,
                    type: "POST",
                    data: JSON.stringify(input),
                    datatype: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#divErrorMessage").html("");
                        Loading(false);

                        var msg = "";
                        if (data.IsListResult) {
                            //msg += "<span style='color:#980000; font-style:italic;'> " +
                            //   data.Result[0] + "</span><br /><br />";

                            if (data.Result.length > 0) {
                                msg += "<div style='overflow-y: auto; max-height:400px;'>" +
                                    "<table style='border:1px solid #d3d3d3;width:100%;'>" +
                                    "<tr style='border:1px solid #d3d3d3; width:100%; '>" +
                                    "<th style='width:15%; text-align:center;border:1px solid #d3d3d3;background-color:#f7f7f7;' id=\"hdRow\">ROW</th>" +
                                    "<th style='width:20%; text-align:center;border:1px solid #d3d3d3;background-color:#f7f7f7;' id=\"hdValue\">VALUE</th>" +
                                    "<th style='width:65%; text-align:center;border:1px solid #d3d3d3;background-color:#f7f7f7;' id=\"hdRemarks\">REMARKS</th>" +
                                    "</tr>";

                                for (var i = 0; i < data.Result.length; i++) {
                                    var temp = data.Result[i].split('*');
                                    msg += "<tr style='border:1px solid #d3d3d3; width:100%; '>" +
                                        "<td headers=\"hdRow\" style='width:15%;text-align:right; border:1px solid #d3d3d3;'>" + temp[0] + "</td>" +
                                        "<td headers=\"hdValue\" style='width:20%; border:1px solid #d3d3d3;'>" + temp[1] + "</td>" +
                                        "<td headers=\"hdRemarks\" style='width:65%; border:1px solid #d3d3d3;'>" + temp[2] + "</td>";
                                    msg += "</tr>";
                                }
                                msg += "</table></div>";
                            }

                        } else {
                            msg += data.Result;
                        }

                        if (data.IsSuccess) {
                                ModalAlert(MODAL_HEADER, msg);
                            $("#btnSearch").click();
                        } else {
                            ModalAlert(MODAL_HEADER, msg);
                        }

                        $("#btnSaveUploadContent").prop("disabled", false);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Loading(false);
                        ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                        $("#btnSaveUploadContent").prop("disabled", false);
                    }
                });
            });
        },

        LoadTable: function (param) {
            var Self = this;
            Loading(true);

            $("#tblStudentList").jqGrid("GridUnload");
            $("#tblStudentList").jqGrid("GridDestroy");

            $("#tblStudentList").jqGrid({
                url: getStudentListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["Restore","","Student Number","First Name","Middle Name","Last Name","Course", "Year",'Section',"Email Address",'User Name', "SystemUserID" ],
                colModel: [ { name: "edit", index: "edit", width: 60, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { key: false, hidden: true, name: "ID" },
                    { key: false, name: "StudentNumber", index: "1", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, name: "FirstName", index: "2", editable: true, align: "center", sortable: true },
                    { key: false, name: "MiddleName", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, name: "LastName", index: "4", editable: true, align: "center", sortable: true },
                    { key: false, name: "Course", index: "5", editable: true, align: "center", sortable: true },
                    { key: false, name: "Year", index: "6", editable: true, align: "center", sortable: true },
                    { key: false, name: "Section", index: "7", editable: true, align: "center", sortable: true },
                    { key: false, name: "EmailAddress", index: "8", editable: true, align: "center", sortable: true },
                    { key: false, name: "UserName", index: "9", editable: true, align: "center", sortable: true },
                    { key: true, name: "SystemUserID", index: "10", editable: true, align: "center", sortable: true, hidden: true }
                ],
                toppager: $("#divPager"),
                rowNum: parseInt(localStorage['StudentRegistration_RowNum']),
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
                page: parseInt(localStorage['StudentRegistration_PageNum']),
                sortorder: localStorage['StudentRegistration_SortOrder'],
                sortname: localStorage['StudentRegistration_SortColumn'],
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
            $("#tblStudentList").jqGrid("setFrozenColumns");
        },

        EditLink: function (cellvalue, options, rowObject) {
            var Self = this;
            return "<a href=\"\" class=\"glyphicon glyphicon-refresh\"  onclick=\"return LoadPartial('" + editPageURL + "?id=" + rowObject.ID + "&isArchive=true" + "', 'divPartialContent');\"></a>";
        },

        FillLocalStorage: function () {
            var Self = this;
           
        },

        FillFieldWithLocalStorage: function () {
            var Self = this;
          
        },

        fillTableLocalStorage: function () {
            var Self = this;
            var sortColumn = $("#tblStudentList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblStudentList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblStudentList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['StudentRegistration_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['StudentRegistration_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['StudentRegistration_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['StudentRegistration_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

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
        },

        UploadFile: function () {
            var Self = this;
            if (Self.Validate() == true) {
                var file;
                file = $("#fileUpload")[0].files[0];

                if (file != undefined) {
                    Loading(true);

                    $("#btnUploadFile").prop("disabled", true);

                    $.ajax({
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("Cache-Control", "no-cache");
                            xhr.setRequestHeader("X-File-Name", file.name);
                            xhr.setRequestHeader("X-File-Size", file.size);
                            xhr.setRequestHeader("Content-Type", "multipart/form-data");
                        },
                        type: "POST",
                        url: window.uploadStudentRegistrationURL,
                        processData: false,
                        cache: false,
                        data: file,
                        success: function (data, textStatus, xhr) {
                            $("#divErrorMessage").html("");
                            Loading(false);
                            if (data.IsSuccess) {
                                $("#hdnContentSessionId").val(data.Result.ContentSessionId);

                                if (data.Result.Content.length > 0) {
                                    HideUploadModal();
                                    ModalConfirmation(MODAL_HEADER, "Gathered " + data.Result.Content.length + " record(s) . Do you want to continue?", "btnSaveUploadContent", "click", "DeleteFile()");
                                }
                                else {
                                    document.getElementById("fileUpload").style.border = "1px solid #c40000";
                                    $("#divModalErrorMessage").html(UPLOAD_NO_RECORD);
                                    noRecord = true;
                                }
                            } else {
                                var msg = "";
                                if (data.IsListResult) {
                                    for (var i = 0; i < data.Result.length; i++) {
                                        msg += data.Result[i] + "<br />";
                                    }
                                } else {
                                    msg += data.Result;
                                }
                                HideUploadModal();
                                ModalAlert(MODAL_HEADER_ARCHIVE, msg);
                            }

                            $("#btnUploadFile").prop("disabled", false);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            Loading(false);
                            HideUploadModal();
                            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                            $("#btnUpload").prop("disabled", false);
                        }
                    });
                }
            }
            return false;
        },
        DeleteFile: function () {
            var Self = this;
            var input = {
                contentSessionId: $("#hdnContentSessionId").val()
            };

            $.ajax({
                url: window.deleteUploadedFile,
                type: "POST",
                data: JSON.stringify(input),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                }
            });
        },
        Validate: function () {
            var Self = this;
            var isvalid = true;

            if (document.getElementById("fileUpload").files.length == 0) {
                document.getElementById("fileUpload").style.border = "1px solid #c40000";
                $("#divModalErrorMessage").html("File" + SUFF_REQUIRED);

                isvalid = false;
            }
            return isvalid;
        }
    }
  
    var studentRegistrationList = Object.create(objstudentRegistrationList);
        studentRegistrationList.Initialized();
});
