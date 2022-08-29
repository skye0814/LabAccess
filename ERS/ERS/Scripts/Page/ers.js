$(document).ready(function () {


    $(".AccordionList .field-title").click(function () {
        if ($(".AccordionList").hasClass("off")) {
            $(".AccordionList").removeClass("off");
            $(".AccordionList").addClass("on");
        }
        else if ($(".AccordionList").hasClass("on")) {
            $(".AccordionList").removeClass("on");
            $(".AccordionList").addClass("off");
        }
    });

    if ($('#back-to-top').length) {
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > 100) {
                $('#back-to-top').addClass('show');
            } else {
                $('#back-to-top').removeClass('show');
            }
        });

        $('#back-to-top').on('click', function (e) {
            $('html,body').animate({
                scrollTop: 0
            });
            return false;
        });
    }
});

