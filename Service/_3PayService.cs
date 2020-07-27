using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Cryptography;

namespace PAYPAYPAY.Service
{
    public class _3PayService
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public string SendRequest(string QueryString)
        {
            try
            {
                string targetUrl = ConfigurationManager.AppSettings["CathayBKPayUrl"];
                //string parame = "CardNo=4907060600015101&ExpireDate=2012&CVV=615&TransMode=1&Installment=0&e_return=1&Str_Check=lm378l3a72lx5sxzg54frkua5e4uathb&Send_Type=0&Pay_Mode_No=2&CustomerId=01478523&Order_No=&Amount=35&TransCode=00&Buyer_Name=BigDish&Buyer_Telm=0987654321&Buyer_Mail=hahaha0417@hotmail.com&Buyer_Memo=moMo&Return_url=&Callback_Url=";
                byte[] postData = Encoding.UTF8.GetBytes(QueryString);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                //request.ContentLength = postData.Length;
                // 寫入 Post Body Message 資料流
                using (Stream st = request.GetRequestStream())
                {
                    st.Write(postData, 0, postData.Length);
                }

                string result = "";
                // 取得回應資料
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                _logger.Debug("取得CathayBK回應資料 : " + result);
                //System.Diagnostics.Debug.WriteLine("取得回應資料 : " + result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug("Exception : " + ex);
            }

            return "ERROR";
        }

        public string ToMD5(string str)
        {
            using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
            {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(str);

                //取得雜湊值位元組陣列
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得 MD5
                var md5 = BitConverter.ToString(hash)
                  .Replace("-", String.Empty)
                  .ToUpper();

                return md5;
            }
        }

        /// <summary>
        /// 將物件序列化成XML格式字串
        /// </summary>
        /// <typeparam name="T">物件型別</typeparam>
        /// <param name="obj">物件</param>
        /// <returns>XML格式字串</returns>
        public string Serialize<T>(T obj) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                serializer.Serialize(writer, obj);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// 將XML格式字串反序列化成物件
        /// </summary>
        /// <typeparam name="T">物件型別</typeparam>
        /// <param name="xmlString">XML格式字串</param>
        /// <returns>反序列化後的物件</returns>
        public T Deserialize<T>(string xmlString) where T : class
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xmlString))
            {
                object deserializationObj = deserializer.Deserialize(reader);
                return deserializationObj as T;
            };
        }
    }
}