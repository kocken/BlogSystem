$('#usernameLabel').click(function () {
    $('#username').focus();
});

$('#passwordLabel').click(function () {
    $('#password').focus();
});

positionIcon2();

window.onresize = function (event) {
    positionIcon2();
};

function positionIcon2() {
    var rect = document.getElementById('password').getBoundingClientRect();
    console.log(rect.top, rect.right, rect.bottom, rect.left);
    document.getElementsByClassName("icon2")[0].style.top = "" + rect.top-135 + "px";
}