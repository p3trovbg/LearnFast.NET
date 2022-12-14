var client_token = "@ViewBag.ClientToken";
var form = document.querySelector('#payment-form');

braintree.dropin.create({
    authorization: client_token,
    container: '#bt-dropin'
}, function (createErr, instance) {
    form.addEventListener('submit', function (event) {
        event.preventDefault();

        instance.requestPaymentMethod(function (err, payload) {
            if (err) {
                return;
            }

            document.querySelector('#nonce').value = payload.nonce;
            form.submit();
        });
    });
});
