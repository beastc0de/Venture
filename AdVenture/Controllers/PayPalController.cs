using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;
using AdVenture.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace AdVenture.Controllers
{
    public class PayPalController : Controller
    {
        public VentureCapitalDbContext db = new VentureCapitalDbContext();
        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Payment(Bids bid)
        {
            ReceiverList receiverList = new ReceiverList();
            receiverList.receiver = new List<Receiver>();
            //Receiver receiver = new Receiver(bid.bid);
            Receiver receiver = new Receiver(20000);
            receiver.email = "kanyewest@gmail.com";
            var receiverID = from v in db.Ventures where v.Id == bid.ventureID select v.investorID;
            ApplicationUser recvUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(receiverID.ToString());
            //receiver.email = recvUser.Email;
            receiverList.receiver.Add(receiver);

            RequestEnvelope requestEnvelope = new RequestEnvelope("en_US");
            string actionType = "PAY";
            string returnUrl = "https://devtools-paypal.com/guide/ap_simple_payment/dotnet?success=true";
            string cancelUrl = "https://devtools-paypal.com/guide/ap_simple_payment/dotnet?cancel=true";
            string currencyCode = "USD";
            PayRequest payRequest = new PayRequest(requestEnvelope, actionType, cancelUrl, currencyCode, receiverList, returnUrl);
            payRequest.ipnNotificationUrl = "http://replaceIpnUrl.com";

            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            sdkConfig.Add("mode", "sandbox");
            sdkConfig.Add("account1.apiUsername", "mattjheller-facilitator_api1.yahoo.com");
            sdkConfig.Add("account1.apiPassword", "DG6GB55TRBWLESWG");
            sdkConfig.Add("account1.apiSignature", "AFcWxV21C7fd0v3bYYYRCpSSRl31AafAKKwBsAp2EBV9PExGkablGWhj");
            sdkConfig.Add("account1.applicationId", "APP-80W284485P519543T");

            AdaptivePaymentsService adaptivePaymentsService = new AdaptivePaymentsService(sdkConfig);
            PayResponse payResponse = adaptivePaymentsService.Pay(payRequest);
            string payKey = payResponse.payKey;
            string paymentExecStatus = payResponse.paymentExecStatus;
            string payURL = String.Format("https://www.sandbox.paypal.com/webscr?cmd=_ap-payment&paykey={0}", payKey);

            return Redirect(payURL);

        }
        
    }
}