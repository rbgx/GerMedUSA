// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT.
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCore;
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefront
{
    /// <summary>
    /// Summary description for cardinal_process.
    /// </summary>
    public partial class cardinalecheck_process : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = -1;
            Response.AddHeader("pragma", "no-cache");

            Response.Cache.SetAllowResponseInBrowserHistory(false);

            Customer ThisCustomer = ((AspDotNetStorefrontPrincipal)Context.User).ThisCustomer;
            ThisCustomer.RequireCustomerRecord();

            int CustomerID = ThisCustomer.CustomerID;
            String Payload = ThisCustomer.ThisCustomerSession["Cardinal.Payload"];
            String PaRes = CommonLogic.FormCanBeDangerousContent("PaRes");
            String TransactionID = ThisCustomer.ThisCustomerSession["Cardinal.TransactionID"];
            int OrderNumber = ThisCustomer.ThisCustomerSession.SessionUSInt("Cardinal.OrderNumber");

            String ReturnURL = String.Empty;

            if (ShoppingCart.CartIsEmpty(CustomerID, CartTypeEnum.ShoppingCart))
            {
                ReturnURL = "ShoppingCart.aspx";
            }

            if (ReturnURL.Length == 0)
            {
                if (OrderNumber == 0)
                {
                    ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + Server.UrlEncode(AppLogic.GetString("cardinalecheck_process.aspx.1", 1, Localization.GetWebConfigLocale()));
                }
            }

            if (ReturnURL.Length == 0)
            {
                if (Payload.Length == 0 || TransactionID.Length == 0)
                {
                    ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + Server.UrlEncode(AppLogic.GetString("cardinalecheck_process.aspx.1", 1, Localization.GetWebConfigLocale()));
                }
            }

            String PAResStatus = String.Empty;
            String CardinalOrderId = String.Empty;
            String SignatureVerification = String.Empty;
            String ErrorNo = String.Empty;
            String ErrorDesc = String.Empty;

            if (ReturnURL.Length == 0)
            {
                String CardinalAuthenticateResult = String.Empty;
                String AuthResult = Cardinal.MyECheckAuthenticate(OrderNumber, PaRes, TransactionID, out CardinalOrderId, out PAResStatus, out SignatureVerification, out ErrorNo, out ErrorDesc, out CardinalAuthenticateResult);
                ThisCustomer.ThisCustomerSession["Cardinal.AuthenticateResult"] = CardinalAuthenticateResult;

                // handle success cases:
                if ((PAResStatus == "Y" && SignatureVerification == "Y"))
                {
                    ShoppingCart cart = new ShoppingCart(1, ThisCustomer, CartTypeEnum.ShoppingCart, 0, false);

                    Address UseBillingAddress = new Address();
                    UseBillingAddress.LoadByCustomer(ThisCustomer.CustomerID, ThisCustomer.PrimaryBillingAddressID, AddressTypes.Billing);

                    String status = Gateway.MakeOrder(String.Empty, AppLogic.TransactionMode(), cart, OrderNumber, String.Empty, String.Empty, String.Empty, String.Empty);

                    if (status != AppLogic.ro_OK)
                    {
                        ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + status;
                    }
                    else
                    {
                        // store cardinal call results for posterity:
                        string sql = "update orders set AuthorizationPNREF=" + DB.SQuote(CardinalOrderId) + ", CardinalLookupResult=" + DB.SQuote(ThisCustomer.ThisCustomerSession["Cardinal.LookupResult"]) + ", CardinalAuthenticateResult=" + DB.SQuote(ThisCustomer.ThisCustomerSession["Cardinal.AuthenticateResult"]) + " where OrderNumber=" + OrderNumber.ToString();
                        DB.ExecuteSQL(sql);
                        ReturnURL = "orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=eCheck";
                    }
                }
                else
                {
                    String sql = "insert into FailedTransaction(CustomerID,OrderNumber,IPAddress,OrderDate,PaymentGateway,PaymentMethod,TransactionCommand,TransactionResult) values(" + ThisCustomer.CustomerID.ToString() + "," + OrderNumber.ToString() + "," + DB.SQuote(ThisCustomer.LastIPAddress) + ",getdate(),'Cardinal'," + DB.SQuote(AppLogic.ro_PMCardinalMyECheck) + ",''," + DB.SQuote(CardinalAuthenticateResult) + ")";
                    DB.ExecuteSQL(sql);
                }

                // handle canceled:
                if (PAResStatus == "X")
                {
                    ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + Server.UrlEncode(AppLogic.GetString("cardinalecheck_process.aspx.2", 1, Localization.GetWebConfigLocale()));
                }


                // handle failure:
                if (PAResStatus == "E" && ErrorDesc.Length != 0)
                {
                    ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + Server.UrlEncode(String.Format(AppLogic.GetString("cardinalecheck_process.aspx.3", 1, Localization.GetWebConfigLocale()), ErrorDesc));
                }
            }

            if (ReturnURL.Length == 0)
            {
                ReturnURL = "checkoutpayment.aspx?error=1&errormsg=" + Server.UrlEncode(AppLogic.GetString("cardinalecheck_process.aspx.4", 1, Localization.GetWebConfigLocale()));
            }
            ThisCustomer.ThisCustomerSession["Cardinal.LookupResult"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.AuthenticateResult"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.ACSUrl"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.Payload"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.TransactionID"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.OrderNumber"] = String.Empty;
            ThisCustomer.ThisCustomerSession["Cardinal.LookupResult"] = String.Empty;

            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");
            Response.Write("<html><head><title>Cardinal Process</title></head><body>");
            Response.Write("<script type=\"text/javascript\">\n");
            Response.Write("top.location='" + ReturnURL + "';\n");
            Response.Write("</SCRIPT>\n");
            Response.Write("<div align=\"center\">" + String.Format(AppLogic.GetString("cardinalecheck_process.aspx.5", 1, Localization.GetWebConfigLocale()), ReturnURL) + "</div>");
            Response.Write("</body></html>");

        }
    }
}
