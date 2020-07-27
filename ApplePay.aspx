<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link rel="apple-touch-icon" sizes="120x120" href="images/touch-icon-120.png">
    <link rel="apple-touch-icon" sizes="152x152" href="images/touch-icon-152.png">
    <link rel="apple-touch-icon" sizes="167x167" href="images/touch-icon-167.png">
    <link rel="apple-touch-icon" sizes="180x180" href="images/touch-icon-180.png">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <script src="https://request-prelive.np-securepaypage-litle.com/LitlePayPage/litle-api2.js"></script>
    <title>Apple Pay Example</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>apple pay</div>
        <input type='text' name='totalPrice' id='totalPrice' onkeyup="value=value.replace(/[^\d]/g,'') " />
    </form>
    <div class="apple-pay-button" onclick="applePayButtonClicked()">
    </div>
    <script>
        /*
 Copyright (C) 2016 Apple Inc. All Rights Reserved.
 See LICENSE.txt for this sample’s licensing information
 Abstract:
 The main client-side JS. Handles displaying the Apple Pay button and requesting a payment.
 */

        /**
         * This method is called when the page is loaded.
         * We use it to show the Apple Pay button as appropriate.
         * Here we're using the ApplePaySession.canMakePayments() method,
         * which performs a basic hardware check.
         *
         * If we wanted more fine-grained control, we could use
         * ApplePaySession.canMakePaymentsWithActiveCards() instead.
         */
        console.log("Apple Pay Start");
        /*
        document.addEventListener('DOMContentLoaded', () => {
            console.log("Apple Pay 1");
            if (window.ApplePaySession) {
                console.log("Apple Pay 2");
                console.log(window.ApplePaySession);
                if (ApplePaySession.canMakePayments()) {
                    console.log("Apple Pay 3");
                    console.log(ApplePaySession.canMakePayments);
                    showApplePayButton();
                    console.log("Apple Pay 4");
                }
            }
        });
        */

        if (window.ApplePaySession) {
            var merchantIdentifier = "merchant.asia.gomypay.applepay";
            var promise = ApplePaySession.canMakePaymentsWithActiveCard(merchantIdentifier);
            promise.then(function (canMakePayments) {
                if (canMakePayments) {
                    // Display Apple Pay Buttons here…
                    console.log("Display Apple Pay Buttons");
                    showApplePayButton();
                    console.log("Display Apple Pay Buttons END");
                } else {
                    // Check for the existence of the openPaymentSetup method.
                    if (ApplePaySession.openPaymentSetup) {
                        // Display the Set up Apple Pay Button here…             
                        ApplePaySession.openPaymentSetup(merchantIdentifier)
                            .then(function (success) {
                                if (success) {
                                    console.log("payment setup success");
                                    // Open payment setup successful
                                } else {
                                    console.log("payment setup failed !!!");
                                    // Open payment setup failed
                                }
                            })
                            .catch(function (e) {
                                // Open payment setup error handling
                            });
                    }
                }
            });
        }

        function showApplePayButton() {
            HTMLCollection.prototype[Symbol.iterator] = Array.prototype[Symbol.iterator];
            const buttons = document.getElementsByClassName("apple-pay-button");
            for (let button of buttons) {
                button.className += " visible";
            }
        }


        /**
         * Apple Pay Logic
         * Our entry point for Apple Pay interactions.
         * Triggered when the Apple Pay button is pressed
         */
        function applePayButtonClicked() {
            console.log("apple pay button clicked");
            const paymentRequest = {
                countryCode: 'TW',
                currencyCode: 'TWD',
                supportedNetworks: ['masterCard', 'visa'],
                merchantCapabilities: ['supports3DS'],
                total: {
                    label: 'Pay for GomyPay !!',
                    type: 'final',
                    amount: '5',
                }

            };

            const session = new ApplePaySession(2, paymentRequest);
            console.log("session object created")

            session.oncancel = (event) => {
                console.log("oncancel");
                console.log(event);
            };

            /**
             * Merchant Validation
             * We call our merchant session endpoint, passing the URL to use
             */
            session.onvalidatemerchant = (event) => {
                const validationURL = event.validationURL;
                alert(validationURL);
                console.log("validation url=" + validationURL);
                getApplePaySession(validationURL).then(function (response) {
                    console.log("===response===");
                    console.log(response);
                    session.completeMerchantValidation(response);
                });
            };

            /**
             * Shipping Method Selection
             * If the user changes their chosen shipping method we need to recalculate
             * the total price. We can use the shipping method identifier to determine
             * which method was selected.
             */

            /*
             session.onshippingmethodselected = (event) => {
                console.log("onshippingmethodselected");
                // const shippingCost = event.shippingMethod.identifier === 'free' ? '0.00' : '5.00';
                // const totalCost = event.shippingMethod.identifier === 'free' ? '8.99' : '13.99';
                //
                // const lineItems = [
                //     {
                //         label: 'Shipping',
                //         amount: shippingCost,
                //     },
                // ];
                //
                // const total = {
                //     label: 'Apple Pay Example',
                //     amount: totalCost,
                // };
                //
                // session.completeShippingMethodSelection(ApplePaySession.STATUS_SUCCESS, total, lineItems);
            };
            */

            /**
             * Payment Authorization
             * Here you receive the encrypted payment data. You would then send it
             * on to your payment provider for processing, and return an appropriate
             * status in session.completePayment()
             */

            
            session.onpaymentauthorized = (event) => {
                console.log("==onpaymentauthorized==");
                // Send payment for processing...
                const payment = event.payment;
                console.log(JSON.stringify(payment.token));

                authorize(payment.token).then(function (response) {
                    console.log("response from datatrans received");
                    console.log(response);

                    if (response.match(/status=.error./)) {
                        console.log("an error occured!");
                        console.log(response);
                        return session.abort();
                    }

                    session.completePayment(ApplePaySession.STATUS_SUCCESS);
                    //window.location.href = "/success.html";
                });

            };

            // All our handlers are setup - start the Apple Pay payment
            console.log("start Apple Pay payment");
            session.begin();
        }

        function getApplePaySession(paymentDataString) {
            console.log("getApplePaySession   " + paymentDataString);
            $.ajax({
                url: "ApplePaySession",   //存取Json的網址             
                type: "POST",
                cache: false,
                dataType: 'json',
                data: { url: paymentDataString },
                //contentType: "application/json",
                success: function (data) {
                    console.log(data);
                    return ;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }


        /*
        function getApplePaySession222(url) {
            return new Promise(function (resolve, reject) {
                var xhr = new XMLHttpRequest();
                xhr.open('GET', '/api/session/create');
                xhr.onload = function () {
                    if (this.status >= 200 && this.status < 300) {
                        resolve(JSON.parse(xhr.response));
                    } else {
                        reject({
                            status: this.status,
                            statusText: xhr.statusText
                        });
                    }
                };
                xhr.onerror = function () {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                };
                xhr.setRequestHeader("Content-Type", "application/json");
                xhr.send(JSON.stringify({ validationUrl: url }));
            });
        }
        */

        function authorize(token) {
            return new Promise(function (resolve, reject) {
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/api/authorize');
                xhr.onload = function () {
                    if (this.status >= 200 && this.status < 300) {
                        resolve(xhr.response);
                    } else {
                        reject({
                            status: this.status,
                            statusText: xhr.statusText
                        });
                    }
                };
                xhr.onerror = function () {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                };
                xhr.setRequestHeader("Content-Type", "application/json");
                xhr.send(JSON.stringify(token));
            });
        }


    </script>
</body>
</html>
