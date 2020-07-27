using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Net.Security;
using System.Web.Http;
using Newtonsoft.Json;
using NLog;
using PAYPAYPAY.Models;
using PAYPAYPAY.Service;
using System.Text;
using System.Configuration;
using System.IO;

namespace PAYPAYPAY.Controllers
{
    public class ValuesController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static _3PayService PayService = new _3PayService();
        private string cerpath = ConfigurationManager.AppSettings["cerpath"];

        [Route("APP")]
        [HttpPost]
        public IHttpActionResult APP([FromBody]paymentDataRequest body)
        {

            try
            {
                //TokenPayServiceClient client = new TokenPayServiceClient();
                //client.TokenPay
                string StrBody = JsonConvert.SerializeObject(body);
                string STOREID = "990230053";
                string CUBKEY = "2222222222";
                _logger.Debug("GOOGLE PAY Payload Request : " + StrBody);
                _logger.Debug(" Payload Request : " + body.payload);
                string ORDERNUMBER = "TEST" + DateTime.Now.ToString("MMddhhmmss");
                /*
                TokenPayServiceRequest Request = new TokenPayServiceRequest();
                Request.MSGID = "TRS0001";
                Request.AUTHORDERINFO.STOREID = STOREID;
                Request.AUTHORDERINFO.ORDERNUMBER = "test000001";
                Request.AUTHORDERINFO.AMOUNT = body.totalPrice;
                Request.AUTHORDERINFO.PAYIN = "2";
                //STOREID + ORDERNUMBER + AMOUNT + CUBKEY
                Request.CAVALUE = PayService.ToMD5(Request.AUTHORDERINFO.STOREID + Request.AUTHORDERINFO.ORDERNUMBER + Request.AUTHORDERINFO.AMOUNT + CUBKEY);
                */
                PAYMENTDATA paymentdata = new PAYMENTDATA();
                paymentdata.payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(body.payload.ToString()));
                paymentdata.transtype = "GGP"; //ApplePay：APP GoogoePay：GGP SamsungPay：SSP
                paymentdata.transdatetime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                paymentdata.deviceinfo = "Google Pay Test " + DateTime.Now.ToString("yyyyMMddhhmmss");
                //body.AUTHORDERINFO.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata)));
                //Request.AUTHORDERINFO.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata)));


                /// sample

                //TokenPayServiceClient objTokenPayServiceClient = new TokenPayServiceClient();

                tempuri.org.TokenPayServiceRequest objTokenPayServiceRequest = new tempuri.org.TokenPayServiceRequest();


                tempuri.org.TokenCardAuthRequest objTokenCardAuthRequest = new tempuri.org.TokenCardAuthRequest();
                objTokenCardAuthRequest.MSGID = tempuri.org.AUTHMSGID.TRS0001;
                objTokenCardAuthRequest.CAVALUE = PayService.ToMD5(STOREID + ORDERNUMBER + body.totalPrice + CUBKEY);

                tempuri.org.TokenPayAuthInfoMerchant objTokenPayAuthInfoMerchant = new tempuri.org.TokenPayAuthInfoMerchant();
                objTokenPayAuthInfoMerchant.STOREID = STOREID;
                objTokenPayAuthInfoMerchant.ORDERNUMBER = ORDERNUMBER;
                objTokenPayAuthInfoMerchant.AMOUNT = body.totalPrice;
                objTokenPayAuthInfoMerchant.PAYIN = "2";
                //string strPAYMENTDATA = "{\"payload\": \"payload\",\"transtype\": \"transtype\",\"transdatetime\": \"YYYY /MM/DD HH:mm:ss\",\"deviceinfo\": \"使用者裝置資訊\"}";
                objTokenPayAuthInfoMerchant.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata))); ;

                objTokenCardAuthRequest.AUTHORDERINFO = objTokenPayAuthInfoMerchant;

                objTokenPayServiceRequest.MERCHANT = objTokenCardAuthRequest;

                _logger.Debug("PayService Request : " + PayService.Serialize(objTokenPayServiceRequest));
                _logger.Debug("JsonConvert Request : " + JsonConvert.SerializeObject(objTokenPayServiceRequest));

                tempuri.org.TokenPayServiceResponse objTokenPayServiceResponse = new tempuri.org.TokenPayServiceResponse();
                TokenPayServiceClient objTokenPayServiceClient = new TokenPayServiceClient();
                objTokenPayServiceResponse = objTokenPayServiceClient.TokenPay(objTokenPayServiceRequest);

                string MSGID = objTokenPayServiceResponse.CUB.MSGID.ToString();
                _logger.Debug("MSGID : " + MSGID);
                _logger.Debug("JsonConvert AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse));
                /*

                string CAVALUE = objTokenPayServiceResponse.CUB.CAVALUE.ToString();
                string PAYTYPE = objTokenPayServiceResponse.CUB.PAYMENTTYPE.ToString();
                string STOREID2 = objTokenPayServiceResponse.CUB.AUTHORDERINFO.STOREID;
                string CAVALUE2 = PayService.ToMD5(objTokenPayServiceResponse.CUB.AUTHORDERINFO.STOREID +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.ORDERNUMBER +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AMOUNT +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHSTATUS +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHCODE + CUBKEY);

                //objTokenPayServiceClient.Close();
                ///sample

                //string XMLbody = PayService.Serialize(Request);

                
                _logger.Debug("CAVALUE : " + CAVALUE);
                _logger.Debug("CAVALUE MD5 : " + CAVALUE2);
                _logger.Debug("PAYTYPE : " + PAYTYPE);
                _logger.Debug("STOREID2 : " + STOREID2);
                _logger.Debug("AUTHSTATUS : " + objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHSTATUS);
                
                _logger.Debug("XML AUTHORDERINFO : " + PayService.Serialize(objTokenPayServiceResponse));
                _logger.Debug("JsonConvert AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse));
                _logger.Debug("AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse.CUB.AUTHORDERINFO));
                */

                return Ok(new ApiResult<object>(JsonConvert.SerializeObject(objTokenPayServiceResponse)));
            }
            catch (Exception e)
            {
                _logger.Debug("Exception : " + e);
            }

            //string XMLResponse = PayService.SendRequest(XMLbody);

            //_logger.Debug("APP XML : " + XMLResponse);




            return Ok(new ApiResult<object>());
        }


        [Route("ApplePay")]
        [HttpPost]
        public IHttpActionResult ApplePay([FromBody]paymentDataRequest body)
        {

            try
            {
                //TokenPayServiceClient client = new TokenPayServiceClient();
                //client.TokenPay
                string StrBody = JsonConvert.SerializeObject(body);
                string STOREID = "990230053";
                string CUBKEY = "2222222222";
                _logger.Debug("GOOGLE PAY Payload Request : " + StrBody);
                _logger.Debug(" Payload Request : " + body.payload);
                string ORDERNUMBER = "TEST" + DateTime.Now.ToString("MMddhhmmss");
                /*
                TokenPayServiceRequest Request = new TokenPayServiceRequest();
                Request.MSGID = "TRS0001";
                Request.AUTHORDERINFO.STOREID = STOREID;
                Request.AUTHORDERINFO.ORDERNUMBER = "test000001";
                Request.AUTHORDERINFO.AMOUNT = body.totalPrice;
                Request.AUTHORDERINFO.PAYIN = "2";
                //STOREID + ORDERNUMBER + AMOUNT + CUBKEY
                Request.CAVALUE = PayService.ToMD5(Request.AUTHORDERINFO.STOREID + Request.AUTHORDERINFO.ORDERNUMBER + Request.AUTHORDERINFO.AMOUNT + CUBKEY);
                */
                PAYMENTDATA paymentdata = new PAYMENTDATA();
                paymentdata.payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(body.payload.ToString()));
                paymentdata.transtype = "GGP"; //ApplePay：APP GoogoePay：GGP SamsungPay：SSP
                paymentdata.transdatetime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                paymentdata.deviceinfo = "Google Pay Test " + DateTime.Now.ToString("yyyyMMddhhmmss");
                //body.AUTHORDERINFO.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata)));
                //Request.AUTHORDERINFO.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata)));


                /// sample

                //TokenPayServiceClient objTokenPayServiceClient = new TokenPayServiceClient();

                tempuri.org.TokenPayServiceRequest objTokenPayServiceRequest = new tempuri.org.TokenPayServiceRequest();


                tempuri.org.TokenCardAuthRequest objTokenCardAuthRequest = new tempuri.org.TokenCardAuthRequest();
                objTokenCardAuthRequest.MSGID = tempuri.org.AUTHMSGID.TRS0001;
                objTokenCardAuthRequest.CAVALUE = PayService.ToMD5(STOREID + ORDERNUMBER + body.totalPrice + CUBKEY);

                tempuri.org.TokenPayAuthInfoMerchant objTokenPayAuthInfoMerchant = new tempuri.org.TokenPayAuthInfoMerchant();
                objTokenPayAuthInfoMerchant.STOREID = STOREID;
                objTokenPayAuthInfoMerchant.ORDERNUMBER = ORDERNUMBER;
                objTokenPayAuthInfoMerchant.AMOUNT = body.totalPrice;
                objTokenPayAuthInfoMerchant.PAYIN = "2";
                //string strPAYMENTDATA = "{\"payload\": \"payload\",\"transtype\": \"transtype\",\"transdatetime\": \"YYYY /MM/DD HH:mm:ss\",\"deviceinfo\": \"使用者裝置資訊\"}";
                objTokenPayAuthInfoMerchant.PAYMENTDATA = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentdata))); ;

                objTokenCardAuthRequest.AUTHORDERINFO = objTokenPayAuthInfoMerchant;

                objTokenPayServiceRequest.MERCHANT = objTokenCardAuthRequest;

                _logger.Debug("PayService Request : " + PayService.Serialize(objTokenPayServiceRequest));
                _logger.Debug("JsonConvert Request : " + JsonConvert.SerializeObject(objTokenPayServiceRequest));

                tempuri.org.TokenPayServiceResponse objTokenPayServiceResponse = new tempuri.org.TokenPayServiceResponse();
                TokenPayServiceClient objTokenPayServiceClient = new TokenPayServiceClient();
                objTokenPayServiceResponse = objTokenPayServiceClient.TokenPay(objTokenPayServiceRequest);

                string MSGID = objTokenPayServiceResponse.CUB.MSGID.ToString();
                _logger.Debug("MSGID : " + MSGID);
                _logger.Debug("JsonConvert AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse));
                /*

                string CAVALUE = objTokenPayServiceResponse.CUB.CAVALUE.ToString();
                string PAYTYPE = objTokenPayServiceResponse.CUB.PAYMENTTYPE.ToString();
                string STOREID2 = objTokenPayServiceResponse.CUB.AUTHORDERINFO.STOREID;
                string CAVALUE2 = PayService.ToMD5(objTokenPayServiceResponse.CUB.AUTHORDERINFO.STOREID +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.ORDERNUMBER +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AMOUNT +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHSTATUS +
                    objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHCODE + CUBKEY);

                //objTokenPayServiceClient.Close();
                ///sample

                //string XMLbody = PayService.Serialize(Request);

                
                _logger.Debug("CAVALUE : " + CAVALUE);
                _logger.Debug("CAVALUE MD5 : " + CAVALUE2);
                _logger.Debug("PAYTYPE : " + PAYTYPE);
                _logger.Debug("STOREID2 : " + STOREID2);
                _logger.Debug("AUTHSTATUS : " + objTokenPayServiceResponse.CUB.AUTHORDERINFO.AUTHSTATUS);
                
                _logger.Debug("XML AUTHORDERINFO : " + PayService.Serialize(objTokenPayServiceResponse));
                _logger.Debug("JsonConvert AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse));
                _logger.Debug("AUTHORDERINFO : " + JsonConvert.SerializeObject(objTokenPayServiceResponse.CUB.AUTHORDERINFO));
                */
            }
            catch (Exception e)
            {
                _logger.Debug("Exception : " + e);
            }

            //string XMLResponse = PayService.SendRequest(XMLbody);

            //_logger.Debug("APP XML : " + XMLResponse);




            return Ok(new ApiResult<object>());
        }


        [Route("ApplePaySession")]
        [HttpPost]
        public async Task<string> GetAsync([FromBody]AppleReq urlreq)//GetAsync([FromBody]AppleReq urlreq)
        {
            _logger.Debug("===Apple Pay STAR===");
            string url = urlreq.url;
            _logger.Debug("url = " + url);
            //_logger.Debug(Path.GetFullPath(cerpath));
            #region Load certificate
            //var certificate = new X509Certificate2(cerpath);
            //var certificate = new X509Certificate2(cerpath);
            ////var certificate = new X509Certificate2("D:\\Dropbox\\GMP\\merchant.asia.gomypay.applepay.cer");
            //_logger.Debug(certificate.ToString());
            #endregion

            try
            {
                #region Load certificate
                var certificate = new X509Certificate2(cerpath);                
                _logger.Debug(certificate.ToString());
                #endregion

                HttpClientHandler handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);
                //handler.SslProtocols = SslProtocols.Tls12;
                //handler.SslProtocols = SslProtocols.Tls12;

                

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    //request = WebRequest.Create(url) as HttpWebRequest;
                    //request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    //request = WebRequest.Create(url) as HttpWebRequest;
                }

                /*
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                */

                var http = new HttpClient(handler);
                http.DefaultRequestHeaders.Add("Accept", "application/json");

                _logger.Debug(http.ToString());
                var options = new MerchantSessionRequest()
                {
                    MerchantIdentifier = "merchant.asia.gomypay.applepay",
                    DisplayName = "GomyPay",
                    Initiative = "web",
                    InitiativeContext = "cathay.gomytw.com"
                };

                var json = JsonConvert.SerializeObject(options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                _logger.Debug(request.ToString());

                HttpResponseMessage response = await http.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ex.ToString();
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
