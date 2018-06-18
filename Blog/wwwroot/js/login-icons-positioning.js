adjustIcon();

var element = document.getElementsByClassName('relative')[0];

// Options for the observer (which mutations to observe)
var config = { attributes: true, childList: true, subtree: true };

// Callback function to execute when mutations are observed
var callback = function (mutationsList) {
    for (var i = 0; i < mutationsList.length; i++) {
        var mutation = mutationsList[i];
        if (mutation.type == 'childList') {
            adjustIcon();
        }
    }
};

// Create an observer instance linked to the callback function
var observer = new MutationObserver(callback);

// Start observing the target node for configured mutations
observer.observe(element, config);

function adjustIcon() {
    var rect = document.getElementById('password').getBoundingClientRect();
    var element = document.getElementsByClassName("icon2")[0];
    if (rect != null & element != null) {
        element.style.top = "" + rect.top - 135 + "px";
    }
}