using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PAYPAYPAY.Models
{
    public class _3pay
    {
    }
    /// <summary>
    /// GOOGLE PAY Request
    /// </summary>
    public class paymentDataRequest
    {
        public string totalPrice { get; set; }
        public string payload { get; set; }
    }
    
    [Serializable]
    [XmlRoot("MERCHANT")]
    public class TokenPayServiceRequest
    {
        /// <summary>
        /// 固定值TRS0001
        /// </summary>
        [XmlElement("MSGID")]
        public string MSGID { get; set; } //= "TRS0001";
        /// <summary>
        /// MD5 STOREID+ORDERNUMBER+AMOUNT+CUBKEY
        /// </summary>
        [XmlElement("CAVALUE")]
        public string CAVALUE { get; set; }
        [XmlElement("AUTHORDERINFO")]
        public TokenPayAuthInfoMerchant AUTHORDERINFO = new TokenPayAuthInfoMerchant();
    }
    public class TokenPayAuthInfoMerchant
    {
        [XmlElement("STOREID")]
        public string STOREID { get; set; }
        [XmlElement("ORDERNUMBER")]
        public string ORDERNUMBER { get; set; }
        [XmlElement("AMOUNT")]
        public string AMOUNT { get; set; }
        [XmlElement("PAYIN")]
        public string PAYIN { get; set; }
        [XmlElement("PAYMENTDATA")]
        public string PAYMENTDATA { get; set; }
    }
    public class PAYMENTDATA
    {
        public string payload { get; set; }
        public string transtype { get; set; }
        public string transdatetime { get; set; }
        public string deviceinfo { get; set; }
    }


    [Serializable]
    [XmlRoot("CUB")]
    public class TokenPayServiceResponse
    {
        [XmlElement("MSGID")]
        public string MSGID { get; set; } = "TRS0001";
        /// <summary>
        /// STOREID+ORDERNUMBER+AMOUNT+CUBKEY
        /// </summary>
        [XmlElement("CAVALUE")]
        public string CAVALUE { get; set; }
        [XmlElement("PAYTYPE")]
        public string PAYTYPE { get; set; }
        [XmlElement("AUTHORDERINFO")]
        public TokenPayAuthInfoCUB AUTHORDERINFO = new TokenPayAuthInfoCUB();
    }
    public class TokenPayAuthInfoCUB
    {
        [XmlElement("STOREID")]
        public string STOREID { get; set; }
        [XmlElement("ORDERNUMBER")]
        public string ORDERNUMBER { get; set; }        
        [XmlElement("AMOUNT")]
        public string AMOUNT { get; set; }
        [XmlElement("PERIODNUMBER")]
        public int PERIODNUMBER { get; set; }
        [XmlElement("REDEEM")]
        public string REDEEM { get; set; } //= "NONE";
        [XmlElement("AUTHSTATUS")]
        public string AUTHSTATUS { get; set; }
        [XmlElement("AUTHCODE")]
        public string AUTHCODE { get; set; }
        [XmlElement("AUTHTIME")]
        public string AUTHTIME { get; set; }
        [XmlElement("AUTHMSG")]
        public string AUTHMSG { get; set; }
        [XmlElement("REDEEMPOINT")]
        public int REDEEMPOINT { get; set; }
        [XmlElement("REDEEMAMOUNT")]
        public int REDEEMAMOUNT { get; set; }
        [XmlElement("PAIDAMOUNT")]
        public int PAIDAMOUNT { get; set; }
    }


    public class ApiResult<T>
    {
        /// <summary>
        /// 執行成功與否
        /// </summary>
        public bool Succ { get; set; } = false;
        /// <summary>
        /// 結果代碼(0000=成功，其餘為錯誤代號)
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 資料時間
        /// </summary>
        public DateTime DataTime { get; set; }
        /// <summary>
        /// 資料本體
        /// </summary>
        public T Data { get; set; }


        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult()//(T data)
        {
            Code = "0000";
            Succ = true;
            DataTime = DateTime.Now;
        }
        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="code"></param>
        /// /// <param name="message"></param>
        public ApiResult(string code, string message)
        {
            Code = code;
            Succ = false;
            this.DataTime = DateTime.Now;
            Message = message;
        }
        /// <summary>
        /// 建立資料查詢結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(T data)
        {
            Code = "0000";
            Succ = true;
            this.DataTime = DateTime.Now;
            Data = data;
            //Message = message;
        }
    }

    public class MerchantSessionRequest
    {
        [JsonProperty("merchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("initiative")]
        public string Initiative { get; set; }

        [JsonProperty("initiativeContext")]
        public string InitiativeContext { get; set; }
    }
    public class AppleReq
    {
        [JsonProperty("url")]
        public string url { get; set; }
    }
}