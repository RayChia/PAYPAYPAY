using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PAYPAYPAY
{
    public partial class GooglePay : System.Web.UI.Page
    {
        public class paymentData
        {
            public int apiVersion;
            public int apiVersionMinor;

            paymentMethodData paymentMethodData = new paymentMethodData();
        }
        public class paymentMethodData
        {
            public string description { get; set; }
            info info = new info();
            tokenizationData tokenizationData = new tokenizationData();
            public string type { get; set; }
    }
        public class info
        {
            public string cardDetails { get; set; }
            public string cardNetwork { get; set; }
        }
        public class tokenizationData
        {
            public string token { get; set; }
            public string type { get; set; }

        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ss(paymentData PMData)
        {
            return ("a");
        }
        public string MethodName()
        {
            return ("a");
        }
        
    }
}