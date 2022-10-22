document.getElementById("inlineCheckbox").addEventListener('click', (e) => {
    var inputs = document.getElementsByClassName('price');

    Array.prototype.forEach.call(inputs, function (el) {
        // Do stuff here
        if (el.disabled === true) {
            el.disabled = false;
        } else {
            el.disabled = true;
        }
    });
})