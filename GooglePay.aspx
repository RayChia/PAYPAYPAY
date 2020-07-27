<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type='text' name='totalPrice' id='totalPrice' onkeyup="value=value.replace(/[^\d]/g,'') " />
        </div>        
        <div id="container"></div>
    </form>
    <script type='text/javascript' src='https://cdnjs.cloudflare.com/ajax/libs/jquery/1.9.1/jquery.js'></script>
    <!--
    <script>
        $(function () {
            $.ajax({
                url: "https://localhost:44329/APP",   //存取Json的網址             
                type: "POST",
                cache: false,
                dataType: 'json',
                data: { totalPrice: $("#totalPrice").val(), payload: "test" },
                //contentType: "application/json",
                success: function (data) {

                    alert('success');
                    return;
                    //方法一 (回傳多筆資料)                
                    for (var i = 0; i < data.length; i++) {
                        alert("name=" + data[i]["欄位名稱"]);
                    }

                    //方法二 (回傳多筆資料)
                    var i = 0;
                    $.each(data, function () {
                        alert(data[i].欄位名稱);
                        i++;
                    });

                    //方法三 (回傳單筆資料)
                    $.each(data, function (index, element) {
                        alert(element);
                    });
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

        });
    </script>
    -->
    <script>

        //var totalPrice = '8'//document.getElementById("totalPrice").value;
        const merchantName = 'TM';
        const environment = 'TEST';//正式 PRODUCTION   測試 TEST
        const type = 'PAYMENT_GATEWAY';// PAYMENT_GATEWAY  DIRECT:自行解碼交易資訊
       

        
        /**
         * Define the version of the Google Pay API referenced when creating your
         * configuration
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#PaymentDataRequest|apiVersion in PaymentDataRequest}
         */
        const baseRequest = {
            apiVersion: 2,
            apiVersionMinor: 0
        };

        /**
         * Card networks supported by your site and your gateway
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
         * @todo confirm card networks supported by your site and gateway
         */
        const allowedCardNetworks = ["JCB", "MASTERCARD", "VISA"];

        /**
         * Card authentication methods supported by your site and your gateway
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
         * @todo confirm your processor supports Android device tokens for your
         * supported card networks
         */
        const allowedCardAuthMethods = ["PAN_ONLY", "CRYPTOGRAM_3DS"];

        /**
         * Identify your gateway and your site's gateway merchant identifier
         *
         * The Google Pay API response will return an encrypted payment method capable
         * of being charged by a supported gateway after payer authorization
         *
         * @todo check with your gateway on the parameters to pass
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#gateway|PaymentMethodTokenizationSpecification}
         */
        const tokenizationSpecification = {
            type: type,
            parameters: {
                'gateway': 'cathaybk',
                'gatewayMerchantId': '990230053'
            }
        };

        /**
         * Describe your site's support for the CARD payment method and its required
         * fields
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
         */
        const baseCardPaymentMethod = {
            type: 'CARD',
            parameters: {
                allowedAuthMethods: allowedCardAuthMethods,
                allowedCardNetworks: allowedCardNetworks
            }
        };

        /**
         * Describe your site's support for the CARD payment method including optional
         * fields
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#CardParameters|CardParameters}
         */
        const cardPaymentMethod = Object.assign(
            {},
            baseCardPaymentMethod,
            {
                tokenizationSpecification: tokenizationSpecification
            }
        );

        /**
         * An initialized google.payments.api.PaymentsClient object or null if not yet set
         *
         * @see {@link getGooglePaymentsClient}
         */
        let paymentsClient = null;

        function GotoServer(paymentDataString) {
            console.log("GotoServer");
            $.ajax({
                url: "APP",   //存取Json的網址
                type: "POST",
                cache: false,
                dataType: 'json',
                data: { totalPrice: $("#totalPrice").val(), payload: paymentDataString },
                //contentType: "application/json",
                success: function (data) {


                    console.log(data);
                    return;
                    //方法一 (回傳多筆資料)                
                    for (var i = 0; i < data.length; i++) {
                        alert("name=" + data[i]["欄位名稱"]);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });





            /*
            console.log("123");
            console.log(paymentDataString);
            var url = 'GooglePay.aspx';
            var data = paymentDataString;

            fetch(url, {
                method: 'POST', // or 'PUT'
                body: JSON.stringify(data), // data can be `string` or {object}!
                headers: new Headers({
                    'Content-Type': 'application/json'
                })
            }).then(res => res.json())
                .catch(error => console.error('Error:', error))
                .then(response => console.log('Success:', response));
                */
            
        }

        /**
         * Configure your site's support for payment methods supported by the Google Pay
         * API.
         *
         * Each member of allowedPaymentMethods should contain only the required fields,
         * allowing reuse of this base request when determining a viewer's ability
         * to pay and later requesting a supported payment method
         *
         * @returns {object} Google Pay API version, payment methods supported by the site
         */
        function getGoogleIsReadyToPayRequest() {
            return Object.assign(
                {},
                baseRequest,
                {
                    allowedPaymentMethods: [baseCardPaymentMethod]
                }
            );
        }

        /**
         * Configure support for the Google Pay API
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#PaymentDataRequest|PaymentDataRequest}
         * @returns {object} PaymentDataRequest fields
         */
        function getGooglePaymentDataRequest() {
            const paymentDataRequest = Object.assign({}, baseRequest);
            paymentDataRequest.allowedPaymentMethods = [cardPaymentMethod];
            paymentDataRequest.transactionInfo = getGoogleTransactionInfo();
            paymentDataRequest.merchantInfo = {
                // @todo a merchant ID is available for a production environment after approval by Google
                // See {@link https://developers.google.com/pay/api/web/guides/test-and-deploy/integration-checklist|Integration checklist}
                // merchantId: '01234567890123456789',
                merchantName: merchantName
            };

            paymentDataRequest.callbackIntents = ["PAYMENT_AUTHORIZATION"];

            return paymentDataRequest;
        }

        /**
         * Return an active PaymentsClient or initialize
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/client#PaymentsClient|PaymentsClient constructor}
         * @returns {google.payments.api.PaymentsClient} Google Pay API client
         */
        function getGooglePaymentsClient() {
            if (paymentsClient === null) {
                paymentsClient = new google.payments.api.PaymentsClient({
                    environment: environment,
                    paymentDataCallbacks: {
                        onPaymentAuthorized: onPaymentAuthorized
                    }
                });
            }
            return paymentsClient;
        }

        /**
         * Handles authorize payments callback intents.
         *
         * @param {object} paymentData response from Google Pay API after a payer approves payment through user gesture.
         * @see {@link https://developers.google.com/pay/api/web/reference/response-objects#PaymentData object reference}
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/response-objects#PaymentAuthorizationResult}
         * @returns Promise<{object}> Promise of PaymentAuthorizationResult object to acknowledge the payment authorization status.
         */
        function onPaymentAuthorized(paymentData) {
            return new Promise(function (resolve, reject) {
                // handle the response
                processPayment(paymentData)
                    .then(function () {
                        resolve({ transactionState: 'SUCCESS' });
                        console.log("onPaymentAuthorized"),
                            console.log(paymentData.paymentMethodData.tokenizationData),
                            console.log("onPaymentAuthorized JSON.stringify"),
                            console.log(JSON.stringify(paymentData.paymentMethodData.tokenizationData)),
                            GotoServer(JSON.stringify(paymentData.paymentMethodData.tokenizationData))
                    })
                    .then(
                        
                    )
                    .catch(function () {
                        resolve({
                            transactionState: 'ERROR',
                            error: {
                                intent: 'PAYMENT_AUTHORIZATION',
                                message: 'Insufficient funds',
                                reason: 'PAYMENT_DATA_INVALID'
                            }
                        });
                    });
            });
        }

        /**
         * Initialize Google PaymentsClient after Google-hosted JavaScript has loaded
         *
         * Display a Google Pay payment button after confirmation of the viewer's
         * ability to pay.
         */
        function onGooglePayLoaded() {
            const paymentsClient = getGooglePaymentsClient();
            paymentsClient.isReadyToPay(getGoogleIsReadyToPayRequest())
                .then(function (response) {
                    if (response.result) {
                        addGooglePayButton();
                    }
                })
                .catch(function (err) {
                    // show error in developer console for debugging
                    console.error(err);
                });
        }

        /**
         * Add a Google Pay purchase button alongside an existing checkout button
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#ButtonOptions|Button options}
         * @see {@link https://developers.google.com/pay/api/web/guides/brand-guidelines|Google Pay brand guidelines}
         */
        function addGooglePayButton() {
            const paymentsClient = getGooglePaymentsClient();
            const button =
                paymentsClient.createButton({ onClick: onGooglePaymentButtonClicked, buttonColor: "white" });
            
            document.getElementById('container').appendChild(button);
        }

        /**
         * Provide Google Pay API with a payment amount, currency, and amount status
         *
         * @see {@link https://developers.google.com/pay/api/web/reference/request-objects#TransactionInfo|TransactionInfo}
         * @returns {object} transaction info, suitable for use as transactionInfo property of PaymentDataRequest
         */
        function getGoogleTransactionInfo() {
            return {
                countryCode: 'TW',
                currencyCode: "TWD",
                totalPriceStatus: "FINAL",
                totalPrice: document.getElementById("totalPrice").value,
                totalPriceLabel: "Total"
            };
        }

        /**
         * Show Google Pay payment sheet when Google Pay payment button is clicked
         */
        function onGooglePaymentButtonClicked() {
            console.log(document.getElementById("totalPrice").value);
            const paymentDataRequest = getGooglePaymentDataRequest();
            paymentDataRequest.transactionInfo = getGoogleTransactionInfo();

            const paymentsClient = getGooglePaymentsClient();
            paymentsClient.loadPaymentData(paymentDataRequest);
        }

        /**
         * Process payment data returned by the Google Pay API
         *
         * @param {object} paymentData response from Google Pay API after user approves payment
         * @see {@link https://developers.google.com/pay/api/web/reference/response-objects#PaymentData|PaymentData object reference}
         */
        function processPayment(paymentData) {
            return new Promise(function (resolve, reject) {
                setTimeout(function () {
                    // @todo pass payment token to your gateway to process payment
                    paymentToken = paymentData.paymentMethodData.tokenizationData;
                    console.log("paymentToken");
                    console.log(paymentToken);
                    resolve({paymentToken});
                }, 3000);
            });
        }</script>
    <script async
      src="https://pay.google.com/gp/p/js/pay.js"
      onload="onGooglePayLoaded()"></script>
    

</body>
</html>
