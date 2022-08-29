$(document).ready(function () {

    $(".checkbox-table").find(".checkbox-request").change(function () {

        if (this.checked) {
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').removeAttr('disabled');
            console.log($(this).val());
        }
        if (!this.checked) {
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').attr('disabled', 'disabled');
            $(this).parent().siblings('.td-quanti').children('.quantity-checkbox').val("");
            $(this).parent().siblings('.td-quanti').children('span.error').css('visibility', 'hidden');
        }
        // Remove red background if any checkbox are checked
        if ($('input[type=checkbox]').is(":checked")) {
            $('#orderdetailsItems tbody tr.checkbox-table').removeClass('error');
        }
    });

    
    //$(function () {
    //    if (!$('.start-time').val() == "") {
    //        console.log(@indexOfForEach);
    //    }
    //});
    //$('#add').click(function () {
    //    // validation and add order items
    //    console.log('working');
    //    var isAllValid = true;

    //    //if (!($('#quantity').val().trim != "" && (parseInt($('#quantity').val()) || 0))) {
    //    //    isAllValid = false;
    //    //    $('#quantity').siblings('span.error').css('visibility', 'visible');
    //    //}
    //    //else {
    //    //    $('#quantity').siblings('span.error').css('visibility', 'hidden');
    //    //}


    //    var $newRow = $('#mainrow').clone().removeAttr('id');
    //    $('#ddlEquipmentCategory ', $newRow).val($('#ddlEquipmentCategory').val());

    //    // Replace add button with remove btn
    //    $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

    //    // remove id attr from new clone row
    //    $('#ddlEquipmentCategory,#quantity,#add', $newRow).removeAttr('id');
    //    $('.td-category,.td-quantity', $newRow).css('visibility', 'visible');
    //    $('span-error', $newRow).remove();

    //    //append clone row
    //    $('#orderdetailsItems').append($newRow);

    //    // clear select data
    //    $('#ddlEquipmentCategory').val('0');
    //    $('#quantity').val('');
    //    $('#orderItemError').empty();
    //})

    //// remove button click event
    //$('#orderdetailsItems').on('click', '.remove', function () {
    //    $(this).parents('tr').remove();
    //})

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

        $('.quantity-checkbox').each(function () {
            if ($(this).parent().siblings('.td-checkbox').children('.checkbox-request').is(':checked')) {
                if ($(this).val() == "") {
                    isAllValid = false;
                    $(this).siblings('span.error').css('visibility', 'visible');
                }
                else {
                    $(this).siblings('span.error').css('visibility', 'hidden');
                }
            }
            else if ($(this).parent().siblings('.td-checkbox').children('.checkbox-request').is(':checked') == false) {
                $(this).siblings('span.error').css('visibility', 'hidden');
            }
        });



        //if (!($('#ddlEquipmentCategory').val().trim() != "" && (parseInt($('#quantity').val()) || 0))) {
        //    isAllValid = false;
        //    $('#ddlEquipmentCategory').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#ddlEquipmentCategory').siblings('span.error').css('visibility', 'hidden');
        //}
        //if ($('#ddlEquipmentCategory').val() == "0") {
        //    isAllValid = false;
        //    $('#ddlEquipmentCategory').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#ddlEquipmentCategory').siblings('span.error').css('visibility', 'hidden');
        //}

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

        //validate order items
        $('#orderItemError').text('');
        var list = [];
        var errorItemCount = 0;

        $('#orderdetailsItems tbody tr.checkbox-table td.td-checkbox input:checked').each(function (index, ele) {
            var quantity = $(this).parent().siblings('.td-quanti').children('.quantity-checkbox');
            var orderItem = {
                EquipmentCategoryID: $(this, this).val(),
                Quantity: parseInt($(quantity, this).val()),
                RequestGUID: requestID
            }
            list.push(orderItem);
        });

        //$('#orderdetailsItems tbody tr').each(function (index, ele) {
        //    if ($('select.ddlEquipmentCategory', this).val() == "0" || (parseInt($('.quantity-checkbox', this).val()) || 0) == 0) {

        //        errorItemCount++;
        //        $(this).addClass('error');
        //        isAllValid = false;
        //    }
        //    else {
        //        var orderItem = {
        //            EquipmentCategoryID: $('.ddlEquipmentCategory', this).val(),
        //            Quantity: parseInt($('.quantity', this).val()),
        //            RequestGUID: requestID
        //        }
        //        list.push(orderItem);
        //    }
        //})


        // If there are no checkboxes that are checked
        if ($('input[type=checkbox]').is(":checked") == false) {
            isAllValid = false;
            $('#orderdetailsItems tbody tr.checkbox-table').addClass('error');
        }

        if (isAllValid) {
            var data = {
                RequestGUID: requestID,
                RequestDateTime: new Date().toLocaleDateString(),
                Remarks: $('#description').val(),
                RequestDetails: list,
                StartTime: $('#start-time').val(),
                EndTime: $('#end-time').val()
            }
            //datalog = JSON.stringify(data);
            console.log(data);
            $(this).val('Please wait..');

            $.ajax({
                type: 'POST',
                url: saveRequestEquipmentURL,
                data: data,
                dataType: "json",
                traditional: false,
                success: function (data) {
                    if (data.IsSuccess) {
                        alert("Successfully saved");

                        // clear form
                        list = [];
                    }
                    else {
                        alert("Error");
                    }
                    $('#submit').text('Save');
                },
                error: function (error) {
                    console.log(error);

                }
            })
        }
        else {
            alert("Request failed. Fill out necessary fields.");
        }
    })
});

