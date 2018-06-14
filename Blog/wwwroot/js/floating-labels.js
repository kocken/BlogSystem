$('#usernameLabel').click(function () {
    $('#username').focus();
});

$('#passwordLabel').click(function () {
    $('#password').focus();
});

$('*').on('blur change click dblclick error focus focusin focusout hover keydown keypress keyup load mousedown mouseenter mouseleave mousemove mouseout mouseover mouseup resize scroll select submit', function () {
    var rect = document.getElementById('password').getBoundingClientRect();
    document.getElementsByClassName("icon2")[0].style.top = "" + rect.top - 135 + "px";
});