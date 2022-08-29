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

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            return this.optional(element) || regexp.test(value);
        },
        "please check your input."
    );

    var objLabPersonnelRegistrationAdd = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#FirstName').focus() }, 500);

            var param = {
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);

        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, mainPageURL, false);
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');

                if ($('#frmLabPersonnelRegistration').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmLabPersonnelRegistration', 'submit');
                }
                return false;
            });

          

            $('#frmLabPersonnelRegistration').on('submit', function () {
                if ($('#frmLabPersonnelRegistration').valid()) {

                    Self.input = $('#frmLabPersonnelRegistration').serialize();

                    Self.SaveRecord(Self.input);
                }

                return false;
            });

        },
        InitializeFormValidation: function () {
            var Self = this;

            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = $('#frmLabPersonnelRegistration').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
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
                    EmailAddress: {
                        required: true,
                        regex: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/
                    },
                    UserName: {
                        required: true,
                        regex: /^[a-zA-Z0-9!@#+._-ñÑ]{4,15}$/
                    }

                },
                messages: {
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
                    EmailAddress: {
                        required: 'Email Address' + SUFF_REQUIRED,
                        regex: "Invalid Email Address format"
                    },
                    UserName: {
                        required: 'Username' + SUFF_REQUIRED,
                        regex: 'Invalid username format'
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
                url: saveLabPersonnelURL,
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

                        $('#FirstName').val("");
                        $('#MiddleName').val("");
                        $('#LastName').val("");
                        $('#EmailAddress').val("");
                        $('#UserName').val("");

                        var param = {
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

            $("#tblLabPersonnelList").jqGrid("GridUnload");
            $("#tblLabPersonnelList").jqGrid("GridDestroy");

            $("#tblLabPersonnelList").jqGrid({
                url: getLabPersonnelListURL,
                postData: param,
                datatype: "json",
                mtype: "POST",
                colNames: ["", "", "First Name", "Middle Name", "Last Name", 'Email Address', 'Username'],
                colModel: [{ name: "edit", index: "edit", width: 30, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                { key: true, hidden: true, name: "ID" },
                { key: false, name: "FirstName", index: "1", editable: true, align: "Left", sortable: true },
                { key: false, name: "MiddleName", index: "2", editable: true, align: "center", sortable: true },
                { key: false, name: "LastName", index: "3", editable: true, align: "center", sortable: true },
                { key: false, name: "EmailAddress", index: "4", editable: true, align: "center", sortable: true },
                { key: false, name: "UserName", index: "5", editable: true, align: "center", sortable: true },
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
    }
    labPersonnelRegistrationAdd = Object.create(objLabPersonnelRegistrationAdd);
    labPersonnelRegistrationAdd.initialized();
});