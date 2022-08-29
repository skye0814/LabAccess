//now create our observer and get our target element
var observer = new MutationObserver(triggerHandler),
        targetElement = document.querySelector("div#divContent"),
        objConfig = {
            childList: true,
            subtree: true,
            attributes: false,
            characterData: true
        };

//then actually do some observing
observer.observe(targetElement, objConfig);

function triggerHandler() {
    $("div#divContent").ready(function () {
        //alert("DETECTED_VARIATION");
        processingEnd();
        //processingBlockEnd();
    });
}

function processingBlockStart() {
    $("div#progress").css("display", "none");
    $("div#divContent").css("display", "none");
    //$("html").block({ message: '<img src="../Content/Images/preloader.gif" alt="Loading..." />' });
    //$.blockUI({ message: '<img src="../Content/Images/preloader.gif" alt="Loading..." />' });
    $.blockUI();
}

function processingBlockEnd() {
    $("div#progress").css("display", "none");
    $("div#divContent").css("display", "block");
    //$("html").unblock();
    $.unblockUI();
}

function processingStart() {
    $("div#progress").css("display", "block");
    $("div#divContent").css("display", "none");
    //$("div.col-md-9").css("display", "none");
}

function processingEnd() {
    $("div#progress").css("display", "none");
    $("div#divContent").css("display", "block");
    //$("div.col-md-9").css("display", "block");
}

//Change url on browser without reloading
function setLocation(url) {
    try {
        history.pushState(null, null, url);
        return false;
    } catch (e) { }
    location.hash = '#' + url;
}

$(".navDiv").on("click", function () {
    $(".navDiv").css({ backgroundColor: "" });
    $(".navDiv").css({ color: "" });
    $(this).css({ backgroundColor: "#750000" });
    $(this).css({ color: "#FFFFFF" });
});
