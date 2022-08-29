$(document).ready(function () {

    var header = document.getElementById('header__logo');
    header.href = "EquipmentItem";
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>EDIT ITEM</span>
                        </div>`;

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            return this.optional(element) || regexp.test(value);
        },
        "please check your input."
    );

    var objequipmentItemAdd = {
        initialized: function () {
            var Self = this;
            Self.input;
            Self.bindEvents();
            Self.InitializeFormValidation();
            Self.fieldFormatting();
            setTimeout(function () { $('#EquipmentItemCode').focus() }, 500);

            // Equipment Item List
            var param = {
                Category: $('#Category').val() + "",
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
               
                if ($('#frmEquipmentItem').valid()) {
                    if ($('#isActive').val() == "true") {
                        if ($('#Status').val() == "Defective")
                            ModalConfirmation(MODAL_HEADER, "Equipment is marked 'Defective' but will be shown in borrwing list. Change to 'Inactive' to hide equipment in borrowing list. " + MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                        else if ($('#Status').val() == "Missing")
                            ModalConfirmation(MODAL_HEADER, "Equipment is marked 'Missing' and will be hidden in borrowing list. " + MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                        else
                            ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                    }
                    else if ($('#isActive').val() == "false") {
                        if ($('#Status').val() == "Defective")
                            ModalConfirmation(MODAL_HEADER, "Equipment is marked 'Defective' and hidden in borrowing list. Change to 'Active' to show equipmnet in borrowing list. " + MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                        else if ($('#Status').val() == "Missing")
                            ModalConfirmation(MODAL_HEADER, "Equipment is marked 'Missing' and will be hidden in borrowing list. " + MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                        else
                            ModalConfirmation(MODAL_HEADER, "Equipment is marked 'Functioning' but will be hidden in borrowing list. Change to 'Active' to show equipment in borrowing list. " + MSG_CONFIRM_SAVE, 'frmEquipmentItem', 'submit');
                    }

                    
                }
                return false;
            });

            $('#frmEquipmentItem').on('submit', function () {
                if ($('#frmEquipmentItem').valid()) {

                    Self.input = $('#frmEquipmentItem').serialize();

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

            validator = $('#frmEquipmentItem').validate({
                errorElement: 'label',
                errorClass: 'errMessage',
                onfocusout: false,
                onkeyup: false,
                onclick: false,
                rules: {
                    Category: {
                        required: true
                    },
                    ItemBrand: {
                        required: true,
                        regex: /^[a-zA-Z\- ]+$/
                    },
                    ItemModel: {
                        required: true,
                        regex: /^[a-zA-Z0-9\- ]+$/
                    },
                    ItemSerialNumber: {
                        required: true,
                        regex: /^[a-zA-Z0-9\-_:.\/ ]+$/
                    },
                    DateBought: {
                        required: true
                    },
                },
                messages: {
                    Category: {
                        required: 'Category' + SUFF_REQUIRED
                    },
                    ItemBrand: {
                        required: 'Brand' + SUFF_REQUIRED,
                        regex: 'Invalid Item Brand format'
                    },
                    ItemModel: {
                        required: 'Model' + SUFF_REQUIRED,
                        regex: 'Invalid Item Model format'
                    },
                    ItemSerialNumber: {
                        required: 'Serial Number' + SUFF_REQUIRED,
                        regex: 'Invalid Serial Number format'
                    },
                    DateBought: {
                        required: 'Date Bought' + SUFF_REQUIRED
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
                url: saveEquipmentItemURL,
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

                        // Equipment Item List
                        LoadPartial(editPageURL + "?EquipmentItemID=" + $('#EquipmentItemID').val())
                        $("input").prop("disabled", false);
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
                },
            });
        },

        // EQUIPMENT ITEM LIST

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
                colNames: ["Edit", "", "Active", "Category", "Equipment Item Code", "Brand", "Model", "Serial Number", "Usable", "Status", "Comments"],
                colModel: [
                    { name: "edit", index: "edit", width: 35, align: "center", sortable: false, formatter: Self.EditLink, frozen: true },
                    { key: true, hidden: true, name: "EquipmentItemID" },
                    { key: false, width: 50, name: "isActive", index: "1", editable: true, align: "center", sortable: true, formatter: Self.isActiveString },
                    { key: false, width: 50, name: "Category", index: "2", editable: true, align: "Left", sortable: true },
                    { key: false, width: 50, name: "EquipmentItemCode", index: "3", editable: true, align: "left", sortable: true, frozen: true },
                    { key: false, width: 50, name: "ItemBrand", index: "4", editable: true, align: "Left", sortable: true },
                    { key: false, width: 50, name: "ItemModel", index: "5", editable: true, align: "Left", sortable: true },
                    { key: false, width: 50, name: "ItemSerialNumber", index: "6", editable: true, align: "Left", sortable: true },
                    { key: false, width: 50, name: "isUsable", index: "9", editable: true, align: "center", sortable: true, formatter: Self.isUsableString },
                    { key: false, width: 50, name: "Status", index: "10", editable: true, align: "center", sortable: true },
                    { key: false, name: "Comments", index: "12", editable: true, align: "Left", sortable: true }
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
                multiselect: true,
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

        isActiveString: function (cellvalue, options, rowObject) {
            if (rowObject.isActive) {
                return "Active";
            }
            else
                return "Inactive";
        },

        isUsableString: function (cellvalue, options, rowObject) {
            if (rowObject.isUsable) {
                return "Servicable";
            }
            else
                return "Unserviceable";
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
    }
    equipmentItemAdd = Object.create(objequipmentItemAdd);
    equipmentItemAdd.initialized();
});
