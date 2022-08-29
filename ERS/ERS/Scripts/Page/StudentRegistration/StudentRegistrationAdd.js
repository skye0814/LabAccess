$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = mainPageURL;
    if($('#restore').val() == 'restore'){
        header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>RESTORE</span>
                        </div>`;
    }
    else{
        header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>REGISTER</span>
                        </div>`;
    }

    $('#StudentNumber').on('input', function () {
        $('#UserName').val($(this).val());
    });

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            return this.optional(element) || regexp.test(value);
        },
        "please check your input."
    );

    var objStudentRegistrationAdd = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#StudentNumber').focus() }, 500);

            var param = {
                Year: $('#ddlYear').val() + "",
                Section: $('#ddlSection').val() + "",
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);

            //var re = /^\d[0-9]{3}-\d[0-9]{4}-MN-\d[0-1]{0}$/;

            //console.log(re.test("2018-02632-0"));
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');
               
                if ($('#frmStudentRegistration').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmStudentRegistration', 'submit');
                }
                return false;
            });


            $('#frmStudentRegistration').on('submit', function () {
                if ($('#frmStudentRegistration').valid()) {

                    Self.input = $('#frmStudentRegistration').serialize();

                    Self.SaveRecord(Self.input);
                }

                return false;
            });

            $('#ddlYear').change(function () {
                var param = {
                    Year: $('#ddlYear').val() + "",
                    Section: $('#ddlSection').val() + "",
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);
            });

             $('#ddlSection').change(function () {
                var param = {
                    Year: $('#ddlYear').val() + "",
                    Section: $('#ddlSection').val() + "",
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);
            })

        },

        InitializeFormValidation: function () {
            var Self = this;
            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = $('#frmStudentRegistration').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    StudentNumber: {
                        required: true,
                        regex: /^\d[0-9]{3}-\d[0-9]{4}-MN-\d[0-1]{0}$/
                    },
                    FirstName: {
                        required: true,
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    MiddleName: {
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    LastName: {
                        required: true,
                        regex: /^[a-zA-Z\-.ñÑ ]+$/
                    },
                    CourseID:
                    {
                        required: true
                    },
                    Year:
                    {
                        required: true
                    },
                    Section:
                    {
                        required: true
                    },
                    EmailAddress:
                    {
                        required: true,
                        regex: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/
                    },
                    UserName:
                    {
                        required: true
                    }
                },
                messages: {
                    StudentNumber: {
                        required: 'Student Number' + SUFF_REQUIRED,
                        regex: 'Invalid Student Number Format!!! (YYYY-XXXXX-MN-X)'
                    },
                    FirstName: {
                        required: 'First Name' + SUFF_REQUIRED,
                        regex: 'Invalid first name input'
                    },
                    MiddleName: {
                        regex: 'Invalid middle name input'
                    },
                    LastName: {
                        required: 'Last Name' + SUFF_REQUIRED,
                        regex: 'Invalid last name input'
                    },
                    CourseID:
                    {
                        required: 'Course' + SUFF_REQUIRED
                    },
                    Year:
                    {
                        required: 'Year' + SUFF_REQUIRED
                    },
                    Section:
                    {
                        required: 'Section' + SUFF_REQUIRED
                    },
                    EmailAddress:
                    {
                        required: 'EmailAddress' + SUFF_REQUIRED,
                        regex: "Invalid Email Address format"
                    },
                    UserName:
                    {
                        required: 'Username' + SUFF_REQUIRED
                    }
                },
                invalidHandler: function () {
                    errorCount = validator.numberOfInvalids();
                },
                errorPlacement: function (error, element) {
                    ctr++;
                    if ($(error).text().indexOf('required') < 0) {
                        otherErrors.push('<label class=\'errMessage\'><li>' + $(error).text() + '</li></label>' + '<br />');
                    }
                    else {
                        requiredErrors.push('<label class=\'errMessage\'><li>' + $(error).text() + '</li></label>' + '<br />');
                    }

                    if (ctr == errorCount) {
                        if (requiredErrors.length > 1) {
                            requiredErrors = [];
                            requiredErrors.push('<label class=\'errMessage\'><li>' + REQ_HIGHLIGHTED_FIELDS + '</li></label>' + '<br />');
                        }

                        if (otherErrors.length != 0) {
                            $('#divErrorMessage').append(otherErrors);
                        }
                        if (requiredErrors.length != 0) {
                            $('#divErrorMessage').append(requiredErrors);
                        }
                        requiredErrors = [];
                        otherErrors = [];
                        ctr = 0;
                    }
                },
            });
        },
        fieldFormatting: function () {
            var Self = this;

           
        },
        SaveRecord: function (input) {
            var Self = this;
            $.ajax({
                url: saveStudentURL,
                //type: 'POST',
                //data: JSON.stringify(input),
                //dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                type: "POST",
                data: input,
                dataType: "json",
                traditional: true,
                success: function (data) {
                    if (data.IsSuccess != true) {
                        var msg = '';

                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + '<br />';
                            }
                        }
                        else {
                            msg += data.Result;
                        }

                        ModalAlert(MODAL_HEADER, msg);

                    } else {
                        ModalAlert(MODAL_HEADER, MSG_SUCCESSFULLY_SAVED);
                        $('#divErrorMessage').html('');
                     
                        $('#btnSave').prop('disabled', false);
                        $('#StudentNumber').val("");
                        $('#FirstName').val("");
                        $('#MiddleName').val("");
                        $('#LastName').val("");
                        $('#EmailAddress').val("");
                        $('#UserName').val("");

                        var param = {
                            Year: $('#ddlYear').val() + "",
                            Section: $('#ddlSection').val() + "",
                        };
                        Self.LoadTable(param);
                        $("input").prop("disabled", false);

                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
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
                colNames: ["", "", "Student Number", "First Name", "Middle Name", "Last Name", "Course", "Year", 'Section', "Email Address", 'User Name'],
                colModel: [{ name: "edit", index: "edit", width: 30, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                { key: true, hidden: true, name: "ID" },
                { key: false, name: "StudentNumber", index: "1", editable: true, align: "left", sortable: true, frozen: true },
                { key: false, name: "FirstName", index: "2", editable: true, align: "Left", sortable: true },
                { key: false, name: "MiddleName", index: "3", editable: true, align: "center", sortable: true },
                { key: false, name: "LastName", index: "4", editable: true, align: "center", sortable: true },
                { key: false, name: "Course", index: "5", editable: true, align: "center", sortable: true },
                { key: false, name: "Year", index: "6", editable: true, align: "right", sortable: true },
                { key: false, name: "Section", index: "7", editable: true, align: "right", sortable: true },
                { key: false, name: "EmailAddress", index: "8", editable: true, align: "center", sortable: true },
                { key: false, name: "UserName", index: "9", editable: true, align: "center", sortable: true },
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
            var sortColumn = $("#tblStudentList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblStudentList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblStudentList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['StudentRegistration_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['StudentRegistration_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['StudentRegistration_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['StudentRegistration_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },
    }
    studentRegistrationAdd = Object.create(objStudentRegistrationAdd);
    studentRegistrationAdd.initialized();
});
