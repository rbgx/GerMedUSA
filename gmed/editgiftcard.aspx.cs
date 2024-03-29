// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using AspDotNetStorefrontCore;
using System.Xml;

namespace AspDotNetStorefrontAdmin
{

    public partial class editgiftcard : System.Web.UI.Page
    {
        protected string selectSQL = "SELECT G.*, C.FirstName, C.LastName from GiftCard G with (NOLOCK) LEFT OUTER JOIN Customer C with (NOLOCK) ON G.PurchasedByCustomerID = C.CustomerID ";
        private Customer cust;
        private int GiftCardID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            cust = ((AspDotNetStorefrontPrincipal)Context.User).ThisCustomer;
            GiftCardID = CommonLogic.QueryStringNativeInt("iden");
            lnkUsage.NavigateUrl = "giftcardusage.aspx?iden=" + GiftCardID;

            if (GiftCardID == 0)
            {
                OrderNumberRow.Visible = false;
                RemainingBalanceRow.Visible = false;
                ltAmount.Visible = false;
                PurchasedByCustomerIDLiteralRow.Visible = false;
                GiftCardTypeDisplayRow.Visible = false;
                InitialAmountLiteralRow.Visible = false;
                PurchasedByCustomerIDTextRow.Visible = true;
                reqCustID.Enabled = true;
            }
            else
            {
                txtAmount.Visible = false; // cannot change after first created
                PurchasedByCustomerIDTextRow.Visible = false;
                reqCustID.Enabled = false;
                GiftCardTypeSelectRow.Visible = false;
                InitialAmountTextRow.Visible = false;
            }

            if (!IsPostBack)
            {
                ltScript.Text = ("\n<script type=\"text/javascript\">\n");
                ltScript.Text += ("    Calendar.setup({\n");
                ltScript.Text += ("        inputField     :    \"txtDate\",      // id of the input field\n");
                ltScript.Text += ("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
                ltScript.Text += ("        showsTime      :    false,            // will display a time selector\n");
                ltScript.Text += ("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
                ltScript.Text += ("        singleClick    :    true            // double-click mode\n");
                ltScript.Text += ("    });\n");
                ltScript.Text += ("</script>\n");

                ltDate.Text = "<img src=\"" + AppLogic.LocateImageURL("skins/skin_1/images/calendar.gif") + "\" style=\"cursor:hand;cursor:pointer;\" align=\"absmiddle\" id=\"f_trigger_s\">&nbsp;<small>(" + Localization.ShortDateFormat() + ")</small>";

                ltStyles.Text = ("  <!-- calendar stylesheet -->\n");
                ltStyles.Text += ("  <link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"jscalendar/calendar-win2k-cold-1.css\" title=\"win2k-cold-1\" />\n");
                ltStyles.Text += ("\n");
                ltStyles.Text += ("  <!-- main calendar program -->\n");
                ltStyles.Text += ("  <script type=\"text/javascript\" src=\"jscalendar/calendar.js\"></script>\n");
                ltStyles.Text += ("\n");
                ltStyles.Text += ("  <!-- language for the calendar -->\n");
                ltStyles.Text += ("  <script type=\"text/javascript\" src=\"jscalendar/lang/" + Localization.JSCalendarLanguageFile() + "\"></script>\n");
                ltStyles.Text += ("\n");
                ltStyles.Text += ("  <!-- the following script defines the Calendar.setup helper function, which makes\n");
                ltStyles.Text += ("       adding a calendar a matter of 1 or 2 lines of code. -->\n");
                ltStyles.Text += ("  <script type=\"text/javascript\" src=\"jscalendar/calendar-setup.js\"></script>\n");

                if (GiftCardID > 0)
                {
                    trEmail.Visible = false;
                    ltCard.Text = DB.GetSqlS("SELECT SerialNumber AS S FROM GiftCard with (NOLOCK) WHERE GiftCardID=" + CommonLogic.QueryStringCanBeDangerousContent("iden"));
                    btnSubmit.Text = "Update Gift Card";

                    loadData();

                    if (CommonLogic.QueryStringNativeInt("added") == 1)
                    {
                        resetError("Gift Card added.", false);
                    }
                    else if (CommonLogic.QueryStringNativeInt("added") == 2)
                    {
                        resetError("Gift Card updated.", false);
                    }
                    else if (CommonLogic.QueryStringNativeInt("added") == 3)
                    {
                        resetError(AppLogic.GetString("giftcard.email.error.1", cust.SkinID, cust.LocaleSetting), true);
                    }
                }
                else
                {
                    lblAction.Visible = false;
                    rblAction.Visible = false;
                    ltCurrentBalance.Text = "NA";
                    trEmail.Visible = true;
                    ltCard.Text = "New Gift Card";
                    btnSubmit.Text = "Add Gift Card";
                    rblAction.SelectedIndex = 0;

                    XmlPackage2 iData = new XmlPackage2("giftcardassignment.xml.config");
                    System.Xml.XmlDocument xd = iData.XmlDataDocument;
                    txtSerial.Text = xd.SelectSingleNode("/root/GiftCardAssignment/row/CardNumber").InnerText;
                    txtDate.Text = xd.SelectSingleNode("/root/GiftCardAssignment/row/ExpirationDate").InnerText;
                }
            }
        }

        protected void resetError(string error, bool isError)
        {
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";

            ((Literal)Form.FindControl("ltError")).Text = str;
        }

        protected void loadData()
        {
            using (SqlConnection dbconn = DB.dbConn())
            {
                dbconn.Open();
                using (IDataReader rs = DB.GetRS("SELECT * FROM GiftCard  with (NOLOCK)  WHERE GiftCardID=" + GiftCardID, dbconn))
                {
                    if (!rs.Read())
                    {
                        rs.Close();
                        resetError("Unable to retrieve data.", true);
                        return;
                    }

                    txtSerial.Text = Server.HtmlEncode(DB.RSField(rs, "SerialNumber"));
                    txtCustomer.Text = DB.RSFieldInt(rs, "PurchasedByCustomerID").ToString();
                    ltCustomerID.Text = DB.RSFieldInt(rs, "PurchasedByCustomerID").ToString();

                    if (DB.RSFieldInt(rs, "PurchasedByCustomerID") != 0)
                    {
                        ltCustomer2.Text = "Customer Name: " + DB.GetSqlS("SELECT (LastName + ', ' + FirstName) AS S FROM Customer with (NOLOCK) WHERE CustomerID=" + DB.RSFieldInt(rs, "PurchasedByCustomerID").ToString());
                    }
                    else
                    {
                        ltCustomer2.Text = "Customer Name: N/A";
                    }

                    txtOrder.Text = Server.HtmlEncode(DB.RSFieldInt(rs, "OrderNumber").ToString());
                    txtDate.Text = Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs, "ExpirationDate"));

                    txtAmount.Text = Localization.CurrencyStringForDBWithoutExchangeRate(DB.RSFieldDecimal(rs, "InitialAmount"));
                    ltAmount.Text = Localization.CurrencyStringForDBWithoutExchangeRate(DB.RSFieldDecimal(rs, "InitialAmount"));
                    ltCurrentBalance.Text = Localization.CurrencyStringForDBWithoutExchangeRate(DB.RSFieldDecimal(rs, "Balance"));

                    ddType.Items.FindByValue(DB.RSFieldInt(rs, "GiftCardTypeID").ToString()).Selected = true;
                    ltGiftCardType.Text = ((GiftCardTypes)DB.RSFieldInt(rs, "GiftCardTypeID")).ToString();

                    rblAction.ClearSelection();
                    rblAction.SelectedIndex = CommonLogic.IIF(DB.RSFieldBool(rs, "DisabledByAdministrator"), 1, 0);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                resetError("", false);

                StringBuilder sql = new StringBuilder(1024);

                //validate for the email type on insert for emailing
                int type = Localization.ParseNativeInt(ddType.SelectedValue);
                if ((type == 101) && (GiftCardID == 0))
                {
                    if ((txtEmailBody.Text.Length == 0) || (txtEmailName.Text.Length == 0) || (txtEmailTo.Text.Length == 0))
                    {
                        resetError("Please fill out the E-Mail preferences.", true);
                        return;
                    }
                }

                //make sure the customer has set up their email properly
                if (type == 101 && GiftCardID == 0 && (AppLogic.AppConfig("MailMe_Server").Length == 0 || AppLogic.AppConfig("MailMe_FromAddress") == "sales@yourdomain.com"))
                {
                    //Customer has not configured their MailMe AppConfigs yet
                    resetError(AppLogic.GetString("giftcard.email.error.2", cust.SkinID, cust.LocaleSetting), true);
                    return;
                }

                if (GiftCardID == 0)
                {
                    //insert a new card

                    //check if valid SN
                    int N = DB.GetSqlN("select count(GiftCardID) as N from GiftCard   with (NOLOCK)  where lower(SerialNumber)=" + DB.SQuote(txtSerial.Text.ToLowerInvariant().Trim()));
                    if (N != 0)
                    {
                        resetError("There is already another Gift Card with that Serial Number.", true);
                        return;
                    }

                    //ok to add them
                    GiftCard card = GiftCard.CreateGiftCard(Localization.ParseNativeInt(txtCustomer.Text),
                                        txtSerial.Text,
                                        Localization.ParseNativeInt(txtOrder.Text),
                                        0,
                                        0,
                                        0,
                                        Localization.ParseNativeDecimal(txtAmount.Text),
                                        txtDate.Text,
                                        Localization.ParseNativeDecimal(txtAmount.Text),
                                        ddType.SelectedValue,
                                        CommonLogic.Left(txtEmailName.Text, 100),
                                        CommonLogic.Left(txtEmailTo.Text, 100),
                                        txtEmailBody.Text,
                                        null,
                                        null,
                                        null,
                                        null,
                                        null,
                                        null);
                    GiftCardID = card.GiftCardID;

                    try
                    {
                        card.SendGiftCardEmail();
                    }
                    catch
                    {
                        //reload page, but inform the admin the the email could not be sent
                        Response.Redirect("editgiftcard.aspx?iden=" + GiftCardID + "&added=3");
                    }

                    //reload page
                    Response.Redirect("editgiftcard.aspx?iden=" + GiftCardID + "&added=1");
                }
                else
                {
                    //update existing card

                    //check if valid SN
                    int N = DB.GetSqlN("select count(GiftCardID) as N from GiftCard   with (NOLOCK)  where GiftCardID<>" + GiftCardID.ToString() + " and lower(SerialNumber)=" + DB.SQuote(txtSerial.Text.ToLowerInvariant().Trim()));
                    if (N != 0)
                    {
                        resetError("There is already another Gift Card with that Serial Number.", true);
                        return;
                    }

                    //ok to update
                    sql.Append("UPDATE GiftCard SET ");
                    sql.Append("SerialNumber=" + DB.SQuote(txtSerial.Text) + ",");
                    sql.Append("ExpirationDate=" + DB.SQuote(Localization.ToDBShortDateString(Localization.ParseNativeDateTime(txtDate.Text))) + ",");
                    sql.Append("DisabledByAdministrator=" + Localization.ParseNativeInt(rblAction.SelectedValue));
                    sql.Append(" WHERE GiftCardID=" + GiftCardID);
                    DB.ExecuteSQL(sql.ToString());

                    //reload page
                    Response.Redirect("editgiftcard.aspx?iden=" + GiftCardID + "&added=2");
                }
            }
        }
}
}