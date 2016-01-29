using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using AdVenture.Models;
using AdVenture.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;




namespace AdVenture.Controllers
{
    public class PPController : Controller
    {
        public VentureCapitalDbContext db = new VentureCapitalDbContext();
        // GET: PP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithCreditCard(Bids bid)
        {
            ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

            #region Item Info
            Item item = new Item();
            item.name = bid.bidStake.ToString() + "% stake in " + bid.ventureID;
            item.currency = "USD";
            item.price = bid.bid.ToString();
            item.quantity = "1";
            item.sku = bid.ventureID.ToString();
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;
            #endregion

            #region Billing Info 
            Address billingAddress = new Address();
            //billingAddress.city = currentUser.City;
            //billingAddress.country_code = currentUser.CountryCode;
            //billingAddress.line1 = currentUser.AddressLine1;
            //billingAddress.postal_code = currentUser.ZipCode;
            //billingAddress.state = currentUser.State;
            #endregion

            #region Credit Card
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            //crdtCard.cvv2 = currentUser.CCV2;
            //crdtCard.expire_month = currentUser.CCExpireMonth;
            //crdtCard.expire_year = currentUser.CCExpireYear;
            //crdtCard.first_name = currentUser.FirstName;
            //crdtCard.last_name = currentUser.LastName;
            //crdtCard.number = currentUser.CCNumber;
            //crdtCard.type = currentUser.CCType; //paypal allows 4 types
            #endregion

            #region Transaction Details
            Details details = new Details();
            details.shipping = "0";
            details.subtotal = bid.bid.ToString();
            details.tax = "0";

            Amount amnt = new Amount();
            amnt.currency = "USD";
            amnt.total = details.subtotal;
            amnt.details = details;

            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = bid.createdOn.ToString();
            tran.item_list = itemList;
            tran.invoice_number = bid.Id.ToString();
            #endregion

            #region Payment
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);
            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;
            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            Payer payer = new Payer();
            payer.funding_instruments = fundingInstrumentList;
            payer.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            payment.transactions = transactions;

            try
            {
                APIContext apiContext = Configuration.GetAPIContext();
                Payment createdPayment = payment.Create(apiContext);

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            } catch (PayPal.PayPalException ex)
            {
                Logger.Log("Error: " + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
        }
        #endregion

        public ActionResult PaymentWithPaypal(Bids bid)
        {
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {

                    // baseURL is the url on which paypal sendsback the data, provided url is for controller.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;

                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = link.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                } else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            } catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
        }

        private Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "5",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = "5"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = details.subtotal, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }
    }

}