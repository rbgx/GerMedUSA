// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Globalization;
using AspDotNetStorefrontCore;
using AspDotNetStorefrontGateways;
using System.Data.SqlClient;

namespace AspDotNetStorefrontAdmin
{
    /// <summary>
    /// Summary description for cst_history.
    /// </summary>
    public partial class cst_history : AspDotNetStorefront.SkinBase
    {

        private Customer TargetCustomer;
        bool control = true;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            TargetCustomer = new Customer(CommonLogic.QueryStringUSInt("CustomerID"), true);
            if (TargetCustomer.CustomerID == 0)
            {
                Response.Redirect("Customers.aspx");
            }
            if (TargetCustomer.IsAdminSuperUser && !ThisCustomer.IsAdminSuperUser)
            {
                control = false;
                SectionTitle = "Customers - <b>You are not allowed to view this page because of insufficient authority associated with your account.</b>";
            }
            else
            {
                SectionTitle = "<a href=\"Customers.aspx?searchfor=" + TargetCustomer.CustomerID.ToString() + "\">Customers</a> - Order History: " + TargetCustomer.FullName() + " (" + TargetCustomer.EMail + ")";
            }            
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (control)
            {
                //If there is a DeleteID remove it from the cart
                int DeleteRecurringOrderNumber = CommonLogic.QueryStringUSInt("DeleteID");
                String DeleteRecurringOrderResult = String.Empty;
                if (DeleteRecurringOrderNumber != 0)
                {
                    RecurringOrderMgr rmgr = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                    DeleteRecurringOrderResult = rmgr.CancelRecurringOrder(DeleteRecurringOrderNumber);
                }

                //If there is a FullRefundID refund it
                int FullRefundID = CommonLogic.QueryStringUSInt("FullRefundID");
                String FullRefundResult = String.Empty;
                if (FullRefundID != 0)
                {
                    RecurringOrderMgr rmgr = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                    FullRefundResult = rmgr.ProcessAutoBillFullRefund(FullRefundID);
                }

                //If there is a PartialRefundID refund it
                int PartialRefundID = CommonLogic.QueryStringUSInt("PartialRefundID");
                String PartialRefundResult = String.Empty;
                if (PartialRefundID != 0)
                {
                    RecurringOrderMgr rmgr = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                    PartialRefundResult = rmgr.ProcessAutoBillPartialRefund(PartialRefundID);
                }

                //If there is a retrypaymentid, retry it
                int RetryPaymentID = CommonLogic.QueryStringUSInt("retrypaymentid");
                String RetryPaymentResult = String.Empty;
                if (RetryPaymentID != 0)
                {
                    RecurringOrderMgr rmgr = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                    RetryPaymentResult = rmgr.ProcessAutoBillRetryPayment(RetryPaymentID);
                }

                //If there is a restartid, restart it
                int RestartPaymentID = CommonLogic.QueryStringUSInt("restartid");
                String RestartPaymentResult = String.Empty;
                if (RestartPaymentID != 0)
                {
                    RecurringOrderMgr rmgr = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                    RestartPaymentResult = rmgr.ProcessAutoBillRestartPayment(RestartPaymentID);
                }

                if (AppLogic.AppConfigBool("AuditLog.Enabled"))
                {
                    writer.Write("<p><a href=\"auditlog.aspx?CustomerID=" + TargetCustomer.CustomerID.ToString() + "\">View Customer Activity Log</a></p>\n");
                }

                if (ShoppingCart.NumItems(TargetCustomer.CustomerID, CartTypeEnum.RecurringCart) != 0)
                {

                    writer.Write("<p align=\"left\"><b>This customer has active recurring (auto-ship) orders.</b></p>\n");

                    // build JS code to show/hide address update block:
                    StringBuilder tmpS = new StringBuilder(4096);
                    tmpS.Append("<script type=\"text/javascript\">\n");
                    tmpS.Append("function toggleLayer(DivID)\n");
                    tmpS.Append("{\n");
                    tmpS.Append("	var elem;\n");
                    tmpS.Append("	var vis;\n");
                    tmpS.Append("	if(document.getElementById)\n");
                    tmpS.Append("	{\n");
                    tmpS.Append("		// standards\n");
                    tmpS.Append("		elem = document.getElementById(DivID);\n");
                    tmpS.Append("	}\n");
                    tmpS.Append("	else if(document.all)\n");
                    tmpS.Append("	{\n");
                    tmpS.Append("		// old msie versions\n");
                    tmpS.Append("		elem = document.all[DivID];\n");
                    tmpS.Append("	}\n");
                    tmpS.Append("	else if(document.layers)\n");
                    tmpS.Append("	{\n");
                    tmpS.Append("		// nn4\n");
                    tmpS.Append("		elem = document.layers[DivID];\n");
                    tmpS.Append("	}\n");
                    tmpS.Append("	vis = elem.style;\n");
                    tmpS.Append("	if(vis.display == '' && elem.offsetWidth != undefined && elem.offsetHeight != undefined)\n");
                    tmpS.Append("	{\n");
                    tmpS.Append("		vis.display = (elem.offsetWidth != 0 && elem.offsetHeight != 0) ? 'block' : 'none';\n");
                    tmpS.Append("	}\n");
                    tmpS.Append("	vis.display = (vis.display == '' || vis.display == 'block') ? 'none' : 'block' ;\n");
                    tmpS.Append("}\n");
                    tmpS.Append("</script>\n");
                    tmpS.Append("\n");
                    tmpS.Append("<style type=\"text/css\">\n");
                    tmpS.Append("	.addressBlockDiv { margin: 0px 20px 0px 20px;  display: none;}\n");
                    tmpS.Append("</style>\n");
                    writer.Write(tmpS.ToString());


                    using (SqlConnection dbconn = DB.dbConn())
                    {
                        dbconn.Open();
                        using (IDataReader rsr = DB.GetRS("Select distinct OriginalRecurringOrderNumber from ShoppingCart   with (NOLOCK)  where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + TargetCustomer.CustomerID.ToString() + " order by OriginalRecurringOrderNumber desc", dbconn))
                        {
                            while (rsr.Read())
                            {
                                bool ShowCancelButton = true;
                                bool ShowRetryButton = false;
                                bool ShowRestartButton = false;
                                String GatewayStatus = String.Empty;

                                RecurringOrderMgr rmgr1 = new RecurringOrderMgr(base.EntityHelpers, base.GetParser);
                                rmgr1.ProcessAutoBillGetAdminButtons(DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"), out ShowCancelButton, out ShowRetryButton, out ShowRestartButton, out GatewayStatus);

                                if (DeleteRecurringOrderNumber == DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"))
                                {
                                    writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                                    writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                                    writer.Write("<font size=\"3\" color=\"blue\"><b>Stop Billing Result: " + DeleteRecurringOrderResult + "<b></font>\n");
                                    writer.Write("</td></tr>\n");
                                    writer.Write("</table>\n");
                                }

                                if (FullRefundID == DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"))
                                {
                                    writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                                    writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                                    writer.Write("<font size=\"3\" color=\"blue\"><b>Full Refund Result: " + FullRefundResult + "<b></font>\n");
                                    writer.Write("</td></tr>\n");
                                    writer.Write("</table>\n");
                                }

                                if (PartialRefundID == DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"))
                                {
                                    writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                                    writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                                    writer.Write("<font size=\"3\" color=\"blue\"><b>Partial Refund Result: " + PartialRefundResult + "<b></font>\n");
                                    writer.Write("</td></tr>\n");
                                    writer.Write("</table>\n");
                                }

                                if (RetryPaymentID == DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"))
                                {
                                    writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                                    writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                                    writer.Write("<font size=\"3\" color=\"blue\"><b>Retry Payment Result: " + RetryPaymentResult + "<b></font>\n");
                                    writer.Write("</td></tr>\n");
                                    writer.Write("</table>\n");
                                }

                                if (RestartPaymentID == DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"))
                                {
                                    writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                                    writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                                    writer.Write("<font size=\"3\" color=\"blue\"><b>Restart Payment Result: " + RestartPaymentResult + "<b></font>\n");
                                    writer.Write("</td></tr>\n");
                                    writer.Write("</table>\n");
                                }

                                writer.Write(AppLogic.GetRecurringCart(base.EntityHelpers, base.GetParser, TargetCustomer, DB.RSFieldInt(rsr, "OriginalRecurringOrderNumber"), SkinID, false, ShowCancelButton, ShowRetryButton, ShowRestartButton, GatewayStatus));
                            }
                        }
                    }
                }

                writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
                writer.Write("<a href=\"news.aspx\"><img src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/orderhistory.gif") + "\" border=\"0\"></a><br/>");
                writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + AppLogic.AppConfig("BoxFrameStyle") + "\">\n");
                writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

                writer.Write("<p align=\"left\" ><b>The customer's order/billing history is shown below. Click on any order number for details.</b></p>");
                int N = 0;

                writer.Write("<table align=\"center\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"1\">\n");
                writer.Write("<tr class=\"gridHeader\">\n");
                writer.Write("<td valign=\"top\"><b>Order Number</b></td>\n");
                writer.Write("<td valign=\"top\"><b>Order Date</b></td>\n");
                writer.Write("<td valign=\"top\"><b>Payment Status</b></td>\n");
                writer.Write("<td valign=\"top\"><b>Shipping Status</b></td>\n");
                writer.Write("<td valign=\"top\"><b>Order Total</b></td>\n");
                if (AppLogic.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
                {
                    writer.Write("<td valign=\"top\"><b>Customer Service Notes</b></td>\n");
                }
                writer.Write("</tr>\n");

                using (SqlConnection dbconn = DB.dbConn())
                {
                    dbconn.Open();
                    using (IDataReader rs = DB.GetRS("Select '' Failed, PaymentGateway, PaymentMethod, ShippedOn, ShippedVIA, ShippingTrackingNumber, OrderNumber, OrderDate, OrderTotal, cast(CustomerServiceNotes as nvarchar(4000)) CustomerServiceNotes, TransactionState, DownloadEMailSentOn, CustomerID, RecurringSubscriptionID from orders  with (NOLOCK)  where CustomerID=" + TargetCustomer.CustomerID.ToString()
                    + " union select 'Failed' Failed, PaymentGateway, PaymentMethod, null ShippedOn, null ShippedVIA, null ShippingTrackingNumber, OrderNumber, OrderDate, null OrderTotal, cast(TransactionResult as nvarchar(4000)) CustomerServiceNotes, null TransactionState, null DownloadEMailSentOn, CustomerID, RecurringSubscriptionID  from FailedTransaction  with (NOLOCK)  where CustomerID=" + TargetCustomer.CustomerID.ToString()
                    + " order by OrderDate desc", dbconn))
                    {
                        while (rs.Read())
                        {
                            String PaymentStatus = String.Empty;
                            if (DB.RSField(rs, "PaymentMethod").Length != 0)
                            {
                                PaymentStatus = "Payment Method: " + DB.RSField(rs, "PaymentMethod") + "<br/>";
                            }
                            else
                            {
                                PaymentStatus = "Payment Method: " + CommonLogic.IIF(DB.RSField(rs, "CardNumber").StartsWith(AppLogic.ro_PMPayPal, StringComparison.InvariantCultureIgnoreCase), AppLogic.ro_PMPayPal, "Credit Card") + "<br/>";
                            }

                            if (DB.RSField(rs, "RecurringSubscriptionID").Length > 0 &&
                                (DB.RSField(rs, "PaymentGateway") == AspDotNetStorefrontGateways.Gateway.ro_GWVERISIGN || DB.RSField(rs, "PaymentGateway") == AspDotNetStorefrontGateways.Gateway.ro_GWPAYFLOWPRO))
                            { // include link to recurringgatewaydetails.aspx for live gateway status 
                                PaymentStatus += "Subscription ID: <a href=\"recurringgatewaydetails.aspx?RecurringSubscriptionID=" + DB.RSField(rs, "RecurringSubscriptionID") + "\">" + DB.RSField(rs, "RecurringSubscriptionID") + "</a><br/>";
                            }

                            String ShippingStatus = String.Empty;
                            if (AppLogic.OrderHasShippableComponents(DB.RSFieldInt(rs, "OrderNumber")))
                            {
                                if (DB.RSFieldDateTime(rs, "ShippedOn") != System.DateTime.MinValue)
                                {
                                    ShippingStatus = "Shipped";
                                    if (DB.RSField(rs, "ShippedVIA").Length != 0)
                                    {
                                        ShippingStatus += " via " + DB.RSField(rs, "ShippedVIA");
                                    }
                                    ShippingStatus += " on " + Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs, "ShippedOn")) + ".";
                                    if (DB.RSField(rs, "ShippingTrackingNumber").Length != 0)
                                    {
                                        ShippingStatus += " Tracking Number: ";

                                        String TrackURL = Shipping.GetTrackingURL(DB.RSField(rs, "ShippingTrackingNumber"));
                                        if (TrackURL.Length != 0)
                                        {
                                            ShippingStatus += "<a href=\"" + TrackURL + "\" target=\"_blank\">" + DB.RSField(rs, "ShippingTrackingNumber") + "</a>";
                                        }
                                        else
                                        {
                                            ShippingStatus += DB.RSField(rs, "ShippingTrackingNumber");
                                        }
                                    }
                                }
                                else
                                {
                                    ShippingStatus = "Not Yet Shipped";
                                }
                            }
                            if (AppLogic.OrderHasDownloadComponents(DB.RSFieldInt(rs, "OrderNumber"), true))
                            {
                                if (DB.RSField(rs, "TransactionState") == AppLogic.ro_TXStateCaptured && DB.RSFieldDateTime(rs, "DownloadEMailSentOn") != System.DateTime.MinValue)
                                {
                                    Order ord = new Order(DB.RSFieldInt(rs, "OrderNumber"), ThisCustomer.LocaleSetting);
                                    if (ShippingStatus.Length != 0)
                                    {
                                        ShippingStatus += "<hr size=\"1\"/>";
                                    }
                                    ShippingStatus += ord.GetDownloadList(false);
                                    ord = null;
                                }
                                else
                                {
                                    if (ShippingStatus.Length == 0)
                                    {
                                        ShippingStatus += "Download List Pending Payment";
                                    }
                                }
                            }
                            writer.Write("<tr>\n");
                            writer.Write("<td valign=\"top\">");
                            writer.Write("<a href=\"orderframe.aspx?ordernumber=" + DB.RSFieldInt(rs, "OrderNumber").ToString() + "\">" + DB.RSFieldInt(rs, "OrderNumber").ToString() + "</a>");
                            writer.Write("<br/><br/>");
                            if (DB.RSField(rs, "Failed").Length == 0)
                            {
                                writer.Write("<a href=\"" + AppLogic.GetStoreHTTPLocation(true) + "receipt.aspx?ordernumber=" + DB.RSFieldInt(rs, "OrderNumber").ToString() + "&customerid=" + DB.RSFieldInt(rs, "CustomerID").ToString() + "\" target=\"_blank\">Printable Receipt</a>");
                            }
                            else
                            {
                                writer.Write("<font color=\"red\">" + DB.RSField(rs, "Failed") + "</font>");
                            }
                            writer.Write("</td>");
                            writer.Write("<td valign=\"top\">" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs, "OrderDate")));
                            writer.Write("</td>");
                            writer.Write("<td valign=\"top\">" + PaymentStatus + "&nbsp;" + "</td>");
                            writer.Write("<td valign=\"top\">" + ShippingStatus + "&nbsp;" + "</td>");
                            writer.Write("<td valign=\"top\">" + ThisCustomer.CurrencyString(DB.RSFieldDecimal(rs, "OrderTotal")) + "</td>");
                            if (AppLogic.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
                            {
                                if (DB.RSField(rs, "CustomerServiceNotes").Length > 110)
                                {
                                    writer.Write("<td valign=\"top\"><textarea READONLY rows=\"10\" cols=\"50\">" + DB.RSField(rs, "CustomerServiceNotes") + "</textarea></td>");
                                }
                                else
                                {
                                    writer.Write("<td valign=\"top\">" + CommonLogic.IIF(DB.RSField(rs, "CustomerServiceNotes").Length == 0, "None", DB.RSField(rs, "CustomerServiceNotes")) + "</td>");
                                }
                            }
                            else
                            {
                                writer.Write("&nbsp;");
                            }
                            writer.Write("</tr>\n");
                            N++;
                        }
                    }
                }
                writer.Write("</table>\n");
                if (N == 0)
                {
                    writer.Write("<p align=\"left\">No orders found</p>\n");
                }

                writer.Write("</td></tr>\n");
                writer.Write("</table>\n");
                writer.Write("</td></tr>\n");
                writer.Write("</table>\n");
            }
        }
    }
}
