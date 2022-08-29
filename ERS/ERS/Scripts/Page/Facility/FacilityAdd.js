$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = "Facility";
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>ADD FACILITY</span>
                        </div>`;

    $.validator.addMethod("regex", function (value, element, regexp) {
        return this.optional(element) || regexp.test(value);
    },
        "Please check your input."
    );

    var objfacilityAdd = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#RoomNumber').focus() }, 500);
            // Facility List
            var param = {
            };
            Self.LoadTable(param);
            $("input").prop("disabled", false);
        },
        bindEvents: function () {
            var Self = this;
            $('#btnReturnList').click(function () {
                ReturnConfirmation(MODAL_HEADER, MSG_CONFIRM_BACK, listPageURL, true, "divPartialContent");
            });

            $('#btnSave').click(function () {
                $('#divErrorMessage').html('');

                if ($('#frmFacility').valid()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmFacility', 'submit');
                }
                return false;
            });

         

            $('#frmFacility').on('submit', function () {
                if ($('#frmFacility').valid()) {

                    Self.input = $('#frmFacility').serialize();

                    Self.SaveRecord(Self.input);
                }
                return false;
            });

            $('#RoomType').change(function () {
                var param = {
                };
                Self.LoadTable(param);
                $("input").prop("disabled", false);

            });

        },
        InitializeFormValidation: function () {
            var Self = this;

            var ctr = 0;
            var errorCount = 0;
            var requiredErrors = [];
            var otherErrors = [];

            validator = $('#frmFacility').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    RoomNumber: {
                        required: true,
                        regex: /CEA-\d[0-9]{2}$|cea-\d[0-9]{2}$/
                    },
                    RoomType: {
                        required: true
                    },
                },
                messages: {
                    RoomNumber: {
                        required: 'Room Number' + SUFF_REQUIRED,
                        regex: "Invalid Room Number format! (CEA-123)"
                    },
                    RoomType: {
                        required: 'Room Type ' + SUFF_REQUIRED
                    },
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
                url: saveFacilityURL,
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
                        //Clear inputs
                        $("#RoomNumber").val("");
                        $("#RoomDescription").val("");
                        $("#Comments").val("");
                        // Reload Table
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
        // Facility List
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
                colNames: ["", "Active", "Room Number", "Room Type", "Room Description", "Comments"],
                colModel: [
                    { key: true, hidden: true, name: "FacilityID" },
                    { key: false, width: 30, name: "isActive", index: "1", editable: true, align: "center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 30, name: "RoomNumber", index: "2", editable: true, align: "center", sortable: true, frozen: true },
                    { key: false, width: 30, name: "RoomType", index: "3", editable: true, align: "center", sortable: true },
                    { key: false, hidden: true, width: 50, name: "RoomDescription", index: "4", editable: false, align: "center", sortable: true },
                    { key: false, name: "Comments", index: "7", editable: true, align: "center", sortable: true },
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
            var sortColumn = $("#tblFacilityList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tblFacilityList").jqGrid('getGridParam', 'sortorder');
            var pageNum = $('#tblFacilityList').getGridParam('page');
            var rowNum = $('.ui-pg-selbox').val();

            localStorage['Facility_SortColumn'] = (sortColumn == undefined || sortColumn == "" || sortColumn == null ? "" : sortColumn);
            localStorage['Facility_SortOrder'] = (sortOrder == undefined || sortOrder == "" || sortOrder == null ? "desc" : sortOrder);
            localStorage['Facility_PageNum'] = (pageNum == undefined || pageNum == "" || pageNum == null ? "1" : pageNum);
            localStorage['Facility_RowNum'] = (rowNum == undefined || rowNum == "" || rowNum == null ? "10" : rowNum);

        },
    }
    facilityAdd = Object.create(objfacilityAdd);
    facilityAdd.initialized();
});
