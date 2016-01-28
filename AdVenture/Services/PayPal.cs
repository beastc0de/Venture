using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;
namespace AdVenture.Services
{
    public class PayPal
    {
        public PayPal()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
        }    
    }
}