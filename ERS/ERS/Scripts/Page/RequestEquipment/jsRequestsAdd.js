$(document).ready(function () {

    moment.suppressDeprecationWarnings = true;

    var header = document.getElementById('header__logo');
    header.href = mainPageURL;
    header.innerHTML = `<div style='display:flex'>
                            <i class='bx bxs-chevron-left' style='font-size: 25px;' ></i> 
                            <span style='padding-top: 1px'>ADD REQUEST</span>
                        </div>`;
    
    $('#return').click(function () {
        ReturnConfirmation(MODAL_HEADER_RETURN, MSG_CONFIRM_BACK, mainPageURL, false);
    });

    // Display data to the html element
    var table = document.getElementById("debug-table");
    function GetItemsByDate() {
        $.ajax({
            type: 'GET',
            url: getAvailableItemsByRequestDate,
            data: {
                StartTime: $(".start-time").val(),
                EndTime: $(".end-time").val()
                //StartTime: $('#selected-date').val() + " " + $('#first-time').val(),
                //EndTime: $('#selected-date').val() + " " + $('#second-time').val(),
            },
            dataType: "json",
            traditional: false,
            success: function (data) {

                $('.quantity-checkbox').val("");
                $('.checkbox-request').prop("checked", false);
                $('.checkbox-table').find('.bx-minus').css('visibility', 'hidden');
                $('.checkbox-table').find('.bx-plus').css('visibility', 'hidden');
                var maxEquipmentQuantity = document.getElementsByClassName("max-equipment-quantity");
                var equipmentCount = parseInt($('.indexOfForEach').val());

                var remainingEquipSpan = document.getElementsByClassName("remaining-text");

                if (data.length == 0) {

                    // No subtraction will happen, give default max quantities in each item
                    for (var j = 0; j < equipmentCount; j++) {
                        $('.indexOfForEach-' + j).attr({
                            "max": parseInt($('.default-equipment-quantity-' + j).val())
                        });

                        remainingEquipSpan[j].innerHTML = "Available: " + $('.default-equipment-quantity-' + j).val();
                    }
                }
                else {
                    // Reapply default max quantity per item if there are still data, then subtract later
                    for (var x = 0; x < equipmentCount; x++) {
                        $('.indexOfForEach-' + x).attr({
                            "max": parseInt($('.default-equipment-quantity-' + x).val())
                        });

                        remainingEquipSpan[x].innerHTML = "Available: " + $('.default-equipment-quantity-' + x).val();
                    }

                    for (var i = 0; i < data.length; i++) {
                        table.innerHTML += "<td>" +
                            data[i].EquipmentCategoryID +
                            "</td><td>" +
                            data[i].TotalSumBorrowed +
                            "</td><td>" + "</td>";
                        if (data.length > 0) {
                            // Subtraction happens here
                            // Limits max and min values in clicking stepdown/stepup buttons
                            var subtract = parseInt($('.max-equipment-quantity-' + data[i].EquipmentCategoryID).val()) - data[i].TotalSumBorrowed;
                            $('.equipcategory-' + data[i].EquipmentCategoryID).attr({
                                "max": (subtract < 0) ? 0 : subtract,
                                "min": 0
                            });
                            /*$('.remaining-textid-' + data[i].EquipmentCategoryID).text("Available: " + parseInt(subtract));*/
                            (subtract < 0) ? $('.remaining-textid-' + data[i].EquipmentCategoryID).text("Available: " + 0) : $('.remaining-textid-' + data[i].EquipmentCategoryID).text("Available: " + parseInt(subtract))
                        }
                    }
                }

                // Hide the row if no available item
                $('.remaining-text').each(function () {
                    if ($(this).text() == "Available: 0" || $(this).text() == "Available: -1") {
                        $(this).parent().parent('.checkbox-table').css('display', 'none');
                    }
                    else {
                        $(this).parent().parent('.checkbox-table').css('display', 'table-row');
                    }
                });
            },
            error: function (error) {
                console.log(error);
            },
            beforeSend: function(){
                /*console.log("Retrieving available equipments");*/
                $('.orderItems').css("display", 'none');
                $('.spinner').css("display", 'block');
                $('.form-controls').css('visibility', 'hidden');
                $('.label-category-list').css('visibility', 'hidden');
            },
            complete: function(){
                /*console.log("Retrieved available equipments");*/
                $('.orderItems').css("display", 'block');
                $('.spinner').css("display", 'none');
                $('.form-controls').css('visibility', 'visible');
                $('.label-category-list').css('visibility', 'visible');
            }
        });
    };

    $('.quantity-checkbox').change(function () {
        if ($(this).val() < 0) {
            $(this).val("0");
        }
    });
    
    // Call get function on change and clears the tabl;e
    $(".start-time").on("dp.change", function () {
        table.innerHTML = "";
        GetItemsByDate();
    });
    $(".end-time").on("dp.change", function () {
        table.innerHTML = "";
        GetItemsByDate();
        $('.label-category-list').css('visibility', 'visible');
        $('.orderItems').css("display", 'block');
    });

    // Logic for checkboxes and enabling textboxes of quantity
    $(".checkbox-table").find(".checkbox-request").change(function () {

        if (this.checked) {
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').removeAttr('disabled');
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').val(1);
            $(this).parent().siblings('.td-quanti').children('.bx').css('visibility', 'visible');
            /*console.log($(this).val());*/
        }
        if (!this.checked) {
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').attr('disabled', 'disabled');
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').val("");
            $(this).parent().siblings('.td-quanti').children('.bx').css('visibility', 'hidden');
            $(this).parent().siblings('.td-name').children('.error').css('visibility', 'hidden');
        }
        // Remove red background if any checkbox are checked
        if ($('input[type=checkbox]').is(":checked")) {
            $('#orderdetailsItems tbody tr.checkbox-table').removeClass('error');
        }
    });

    // Code for datetimepicker
    var dateToday = new Date(); //.toLocaleDateString("en-US", {});
    dateToday.setDate(dateToday.getDate());

    $(function () {
        $('.start-time').datetimepicker({
            format: "M/D/YYYY hh:mm A",
            icons: {
                today: 'glyphicon glyphicon-map-marker'
            },
            sideBySide: true,
        });

        $('.end-time').datetimepicker({
            format: "M/D/YYYY hh:mm A",
            useCurrent: false,
            icons: {
                today: 'glyphicon glyphicon-map-marker'
            },
            sideBySide: true,
        });

        $(".start-time").on("dp.change", function (e) {
            $('.end-time').removeAttr('disabled');
            $('.start-time').data("DateTimePicker").minDate(dateToday.toLocaleString("en-US", {}).split(",").join(" "));
            $('.end-time').data("DateTimePicker").minDate(moment(e.date).add(30, 'minutes'));

            // Start time options
            $('.start-time').data("DateTimePicker").daysOfWeekDisabled([0]);
            //$('.start-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]);

            // End time options
            $('.end-time').data("DateTimePicker").daysOfWeekDisabled([0]);
            //$('.end-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]);
        });
        $(".end-time").on("dp.change", function (e) {
            $('.start-time').data("DateTimePicker").maxDate(moment(e.date).add(-30, 'minutes'));
            $('.start-time').data("DateTimePicker").minDate(dateToday.toLocaleString("en-US", {}).split(",").join(" "));

            // End time options
            $('.end-time').data("DateTimePicker").daysOfWeekDisabled([0]);
            //$('.end-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]);
        });

    });

    //// Test date and time
    //$('#selected-date').datetimepicker({
    //    format: "M/D/YYYY",
    //    icons: {
    //        today: 'glyphicon glyphicon-map-marker'
    //    },
    //    sideBySide: true,
    //});
    //$('#first-time').datetimepicker({
    //    format: "hh:mm A",
    //});
    //$('#second-time').datetimepicker({
    //    format: "hh:mm A",
    //});
    //$('#first-time').on('dp.change', function (e) {
    //    $('#second-time').removeAttr('disabled');
    //    $('#first-time').data("DateTimePicker").minDate(new Date());
    //    $('#second-time').data("DateTimePicker").minDate(moment(new Date()).add(30, 'minutes'));

    //    //$('#first-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]);
    //    //$('#second-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]);
    //});
    //$('#second-time').on('dp.change', function (e) {
    //    $('#first-time').data("DateTimePicker").maxDate(e.date);
    //    $('#second-time').data("DateTimePicker").minDate(moment(new Date()).add(30, 'minutes'));

    //    //$('#first-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]);
    //    //$('#second-time').data("DateTimePicker").enabledHours([7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]);
    //});

    //$('#selected-date').on('dp.change', function (e) {
    //    $('#selected-date').data("DateTimePicker").minDate(moment(new Date()).format("M/D/YYYY"));
    //    $('#first-time').data("DateTimePicker").keepInvalid(false);
    //    $('#second-time').data("DateTimePicker").keepInvalid(false);
    //    $('#first-time').removeAttr('disabled');
    //    if ($('#first-time').val() != "" || $('#second-time').val() != "") {
    //        table.innerHTML = "";
    //        GetItemsByDate();
    //    }
    //});
    //$("#first-time").on("dp.change", function () {
    //    table.innerHTML = "";
    //    GetItemsByDate();
    //});
    //$("#second-time").on("dp.change", function () {
    //    table.innerHTML = "";
    //    GetItemsByDate();
    //    $('.label-category-list').css('visibility', 'visible');
    //    $('.orderItems').css("display", 'block');
    //});


    // Code after clicking submit
    $('#submit').click(function () {
        var isAllValid = true;


        if ($('#start-time').val() == "") {
            isAllValid = false;
            $('#start-time').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#start-time').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#end-time').val() == "") {
            isAllValid = false;
            $('#end-time').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#end-time').siblings('span.error').css('visibility', 'hidden');
        }

        // Check dates if equal
        var dateOnlyStartDate = $('#start-time').val().split(" ");
        var dateOnlyEndDate = $('#end-time').val().split(" ");

        if (dateOnlyStartDate[0] != dateOnlyEndDate[0]) {
            isAllValid = false;
            if ($('#start-time').val() != "" && $('#end-time').val() != "") {
                ModalAlert("REQUEST FAILED", "<strong>Start Date</strong> and <strong>End Date</strong> should be the same");
            }
        }
        else {
            var dateMin = moment(dateOnlyStartDate[0] + " 7:00 AM");
            var dateMax = moment(dateOnlyStartDate[0] + " 9:00 PM");
            var startTime = moment($('#start-time').val());
            var endTime = moment($('#end-time').val());

            if ((startTime < dateMin) || (endTime > dateMax)) {
                isAllValid = false;
                ModalAlert("REQUEST FAILED", "<strong>Start Time</strong> and <strong>End Time</strong> should only be between laboratory hours (7:00 AM - 9:00 PM)");
            }
        }
        //if ($('#selected-date').val() == "") {
        //    isAllValid = false;
        //    $('#selected-date').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#selected-date').siblings('span.error').css('visibility', 'hidden');
        //}

        //if ($('#first-time').val() == "") {
        //    isAllValid = false;
        //    $('#first-time').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#first-time').siblings('span.error').css('visibility', 'hidden');
        //}

        //if ($('#second-time').val() == "") {
        //    isAllValid = false;
        //    $('#second-time').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#second-time').siblings('span.error').css('visibility', 'hidden');
        //}

        $('.quantity-checkbox').each(function () {
            if ($(this).parent().siblings('.td-checkbox').children('.checkbox-request').is(':checked')) {
                if ($(this).val() == "" || $(this).val() == "0") {
                    isAllValid = false;
                    $(this).parent().siblings('.td-name').children('.error').css('visibility', 'visible');
                }
                else {
                    $(this).parent().siblings('.td-name').children('.error').css('visibility', 'hidden');
                }
            }
            else if ($(this).parent().siblings('.td-checkbox').children('.checkbox-request').is(':checked') == false) {
                $(this).parent().siblings('.td-name').children('.error').css('visibility', 'hidden');
            }
        });
       
        // Randomize GUID
        function randomGUID() {
            var h = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
            var k = ['x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', '-', 'x', 'x', 'x', 'x', '-', '4', 'x', 'x', 'x', '-', 'y', 'x', 'x', 'x', '-', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'];
            var u = '', i = 0, rb = Math.random() * 0xffffffff | 0;
            while (i++ < 36) {
                var c = k[i - 1], r = rb & 0xf, v = c == 'x' ? r : (r & 0x3 | 0x8);
                u += (c == '-' || c == '4') ? c : h[v]; rb = i % 8 == 0 ? Math.random() * 0xffffffff | 0 : rb >> 4
            }
            return u;
        }
        var requestID = randomGUID().trim();

        // Validate request items
        $('#orderItemError').text('');
        var list = [];
        var errorItemCount = 0;

        $('#orderdetailsItems tbody tr.checkbox-table td.td-checkbox input:checked').each(function (index, ele) {
            var quantity = $(this).parent().siblings('.td-quanti').children('.quantity-checkbox');
            var orderItem = {
                EquipmentCategoryID: $(this, this).val(),
                Quantity: parseInt($(quantity, this).val()),
                RequestGUID: requestID,
                StartTime: /*$('#selected-date').val() + " " + $('#first-time').val()*/ $('.start-time').val(),
                EndTime: /*$('#selected-date').val() + " " + $('#second-time').val()*/ $('.end-time').val()
            }
            list.push(orderItem);
        });


        // If there are no checkboxes that are checked
        if ($('input[type=checkbox]').is(":checked") == false) {
            isAllValid = false;
            $('#orderdetailsItems tbody tr.checkbox-table').addClass('error');
        }

        if (isAllValid) {

            var input = {
                RequestGUID: requestID,
                RequestDateTime: new Date().toLocaleString("en-US", {}).split(",").join(" "),
                Remarks: $('#description').val(),
                RequestDetails: list,
                StartTime: /*$('#selected-date').val() + " " + $('#first-time').val()*/ $('.start-time').val(),
                EndTime: /*$('#selected-date').val() + " " + $('#second-time').val()*/ $('.end-time').val()
            }
            // datalog = JSON.stringify(data);
            // console.log(data);
            $(this).val('Please wait..');
            $.ajax({
                type: 'POST',
                url: saveRequestEquipmentURL,
                data: input,
                dataType: "json",
                traditional: false,
                success: function (data) {
                    if (data.IsSuccess) {
                        // Clear list of request
                        list = [];
                        GenerateEquipmentItemQRCode(input.RequestGUID);
                    }
                    else {
                        var msg = '';

                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + '<br />';
                            }
                        }
                        else {
                            msg += data.Result;
                        }

                        ModalAlertSuccessfulCancel("REQUEST ABORTED", msg)
                    }
                },
                error: function (error) {
                    console.log(error);
                },
                beforeSend: function () {
                    $('#submit').html('<img src="\../../Content/Images/spinner.svg"\ style="\width: 30px"\> PROCESSING');
                    $('#submit').css('background-color', '#FFBF86');
                    $('#submit').attr('disabled', 'disabled');
                },
                complete: function () {
                    $('#submit').html('<i class="glyphicon glyphicon-plus"></i> Save Request');
                    $('#submit').css('background-color', 'var(--primary-color)');
                    $('#submit').removeAttr('disabled');
                }
            })
        }
        else {
        }
    })
});

function GenerateEquipmentItemQRCode(RequestGUID) {

    $.ajax({
        url: window.generateQRCodeURL,
        type: "POST",
        data: JSON.stringify({ RequestGUID: RequestGUID }),
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
                $('#qrcode-image').val("<img src='" + data.Result + "'/>");
                $('#qrcode-link').val(data.Result);
                $('#qrcode-filename').val(RequestGUID + '.png');
                ModalAlertShowQRCode(MODAL_HEADER_SUCCESSFULLY_SUBMITTED, MSG_SUCCESSFULLY_SAVED_QRCODE, $('#qrcode-image').val(), $('#qrcode-link').val(), $('#qrcode-filename').val(), RequestGUID);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, $(jqXHR.responseText).filter('title').text());
        },
        beforeSend: function(){
            $('#submit').html('<img src="\../../Content/Images/spinner.svg"\ style="\width: 30px"\> PROCESSING');
            $('#submit').css('background-color', '#FFBF86');
            $('#submit').attr('disabled', 'disabled');
            /*console.log("beforeSendQR");*/
        },
        complete: function(){
            $('#submit').html('<i class="glyphicon glyphicon-plus"></i> Save Request');
            $('#submit').css('background-color', 'var(--primary-color)');
            $('#submit').removeAttr('disabled');
            /*console.log("completeQR");*/
        }
    });

    return false;
}

