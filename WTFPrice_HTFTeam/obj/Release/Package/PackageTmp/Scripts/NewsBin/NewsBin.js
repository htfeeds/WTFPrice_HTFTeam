//----------------------------start CommentFaceBook---------------------------------------------
    window.fbAsyncInit = function () {
        FB.init({
            appId: '482158608600562',
            xfbml: true,
            version: 'v2.3'
        });
    };
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) {
        return;
    }
    js = d.createElement(s);
    js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
//------------------------------end CommentFaceBook---------------------------------------------


//--------------------------------start Load More------------------------------------------------

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

var counter = 1;
function OnCompleted() {
    counter++;
    $(this).attr("href", "/News/ViewMore?times=" + counter);
    if (parseInt(getCookie("Hidden")) < 6) {
        $(".ViewMoreWrap").hide();
    }
}
//--------------------------------end Load More------------------------------------------------



//--------------------------------start SearchAuto------------------------------------------------
$(function () {

    $("[data-autocomplete-source]").each(function () {
        var target = $(this);
        target.autocomplete({ source: target.attr("data-autocomplete-source") });
    });

   
});

function searchFailed() {
    $("#SearchResultId").html("Sorry, there was a problem with the search.");
}

//--------------------------------end SearchAuto------------------------------------------------



//---------------------------------start upload user avatar---------------------------------------
function uploadFailed() {
    $("#SearchResultId").html("Sorry, there was a problem with the upload.");
}