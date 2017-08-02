// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.IO;
using AspDotNetStorefrontCore;
using System.Data.SqlClient;

namespace AspDotNetStorefrontAdmin
{
    /// <summary>
    /// Summary description for currencies.
    /// </summary>
    public partial class currencies : AspDotNetStorefront.SkinBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            /****************************************************************************/
            // * WARNING TO DEVELOPERS
            // * The redirect below is a SAFETY feature.  Removing the redirect will not
            // * enable ML-only features on a lower version of AspDotNetStorefront.
            // * Attempting to do so can very easily result in a partially implemented
            // * feature, invalid or incomplete data in your DB, and other serious 
            // * conditions that will cause your store to be non-functional.
            // *
            // * If you break your store attempting to enable ML-only features in PRO or
            // * Standard, our staff cannot help you fix it, and it will also invalidate
            // * your AspDotNetStorefront License.
            /***************************************************************************/

            if (!AppLogic.m_ProductIsML() || AppLogic.ProductIsMLExpress())
            {
                Response.Redirect("restrictedfeature.aspx");
            }

            SectionTitle = "Manage Currencies";
            if (CommonLogic.QueryStringCanBeDangerousContent("update").Length != 0)
            {
                Currency.GetLiveRates();
                Response.Redirect("currencies.aspx"); // THROW away any table edits. a live rate check overrides those!
            }
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (CommonLogic.QueryStringCanBeDangerousContent("DeleteID").Length != 0)
            {
                // handle delete:
                DB.ExecuteSQL("delete from Currency where CurrencyID=" + CommonLogic.QueryStringCanBeDangerousContent("DeleteID"));
            }

            if (CommonLogic.FormCanBeDangerousContent("IsSubmit").Length != 0)
            {
                // handle updates:

                AppLogic.SetAppConfig("Localization.CurrencyFeedUrl", CommonLogic.FormCanBeDangerousContent("CurrencyFeedUrl").Trim());
                AppLogic.SetAppConfig("Localization.CurrencyFeedXmlPackage", CommonLogic.FormCanBeDangerousContent("CurrencyFeedXmlPackage").Trim());
                AppLogic.SetAppConfig("Localization.CurrencyFeedBaseRateCurrencyCode", CommonLogic.FormCanBeDangerousContent("CurrencyFeedBaseRateCurrencyCode").Trim());

                using (SqlConnection dbconn = DB.dbConn())
                {
                    dbconn.Open();
                    using (IDataReader rs = DB.GetRS("Select * from currency with (NOLOCK)", dbconn))
                    {
                        while (rs.Read())
                        {
                            int ID = DB.RSFieldInt(rs, "CurrencyID");
                            String Name = CommonLogic.FormCanBeDangerousContent("Name_" + ID.ToString());
                            String CurrencyCode = CommonLogic.FormCanBeDangerousContent("CurrencyCode_" + ID.ToString());
                            String Symbol = CommonLogic.FormCanBeDangerousContent("Symbol_" + ID.ToString());
                            Decimal ExchangeRate = CommonLogic.FormUSDecimal("ExchangeRate_" + ID.ToString());
                            String DisplayLocaleFormat = CommonLogic.FormCanBeDangerousContent("DisplayLocaleFormat_" + ID.ToString());
                            String DisplaySpec = CommonLogic.FormCanBeDangerousContent("DisplaySpec_" + ID.ToString());
                            bool Published = (CommonLogic.FormCanBeDangerousContent("Published_" + ID.ToString()).Length != 0);
                            int DisplayOrder = CommonLogic.FormUSInt("DisplayOrder_" + ID.ToString());
                            DB.ExecuteSQL("update Currency set Name=" + DB.SQuote(Name) + ", WasLiveRate=0, CurrencyCode=" + DB.SQuote(CurrencyCode) + ", Symbol=" + DB.SQuote(Symbol) + ", ExchangeRate=" + Localization.DecimalStringForDB(ExchangeRate) + ", DisplayLocaleFormat=" + DB.SQuote(DisplayLocaleFormat) + ", DisplaySpec=" + DB.SQuote(DisplaySpec) + ", Published=" + CommonLogic.IIF(Published, "1", "0") + ", DisplayOrder=" + DisplayOrder.ToString() + ", LastUpdated=getdate() where CurrencyID=" + ID.ToString());
                        }
                    }
                }

                // handle new add:
                if (CommonLogic.FormCanBeDangerousContent("Name_0").Trim().Length != 0)
                {
                    String Name = CommonLogic.FormCanBeDangerousContent("Name_0");
                    String CurrencyCode = CommonLogic.FormCanBeDangerousContent("CurrencyCode_0");
                    String Symbol = CommonLogic.FormCanBeDangerousContent("Symbol_0");
                    Decimal ExchangeRate = CommonLogic.FormNativeDecimal("ExchangeRate_0");
                    String DisplayLocaleFormat = CommonLogic.FormCanBeDangerousContent("DisplayLocaleFormat_0");
                    String DisplaySpec = CommonLogic.FormCanBeDangerousContent("DisplaySpec_0");
                    bool Published = (CommonLogic.FormCanBeDangerousContent("Published_0").Length != 0);
                    int DisplayOrder = CommonLogic.FormUSInt("DisplayOrder_0");
                    DB.ExecuteSQL("insert Currency(Name,CurrencyCode,Symbol,ExchangeRate,WasLiveRate,DisplayLocaleFormat,DisplaySpec,Published,DisplayOrder) values(" + DB.SQuote(Name) + "," + DB.SQuote(CurrencyCode) + "," + DB.SQuote(Symbol) + "," + Localization.DecimalStringForDB(ExchangeRate) + ",0," + DB.SQuote(DisplayLocaleFormat) + "," + DB.SQuote(DisplaySpec) + "," + CommonLogic.IIF(Published, "1", "0") + "," + DisplayOrder.ToString() + ")");
                }
            }
            Currency.FlushCache();

            writer.Write("<script type=\"text/javascript\">\n");
            writer.Write("function Form_Validator(theForm)\n");
            writer.Write("{\n");
            writer.Write("submitonce(theForm);\n");
            writer.Write("return (true);\n");
            writer.Write("}\n");
            writer.Write("</script>\n");

            writer.Write("<p align=\"left\"><input type=\"button\" class=\"normalButtons\" value=\"Get Live Rates\" onClick=\"javascript:self.location='currencies.aspx?update=true';\"></p>\n");
            writer.Write("<form method=\"POST\" action=\"currencies.aspx\" onsubmit=\"alert('Please Be Patient, this will take a minute to validate all exchange rates!');return (validateForm(document.forms[0]) && Form_Validator(document.forms[0]))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
            writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\"/>\n");

            writer.Write("<table>");
            writer.Write("<tr><td style=\"border-style: solid; border-width: 1px;\" width=\"280\"><b>Currency Feed Url " + CommonLogic.IIF(AppLogic.AppConfig("Localization.CurrencyFeedUrl").Length != 0, " (<a href=\"" + AppLogic.AppConfig("Localization.CurrencyFeedUrl") + "\" target=\"_blank\">test</a>)", "") + ":</b></td><td style=\"border-style: solid; border-width: 1px;\" ><input type=\"text\" size=\"60\" id=\"CurrencyFeedUrl\" name=\"CurrencyFeedUrl\" value=\"" + AppLogic.AppConfig("Localization.CurrencyFeedUrl") + "\"></td><td style=\"border-style: solid; border-width: 1px;\" ><small>If you will be maintaining your exchange rate table manually, set this appconfig value to be an empty string</small></td></tr>");
            writer.Write("<tr><td style=\"border-style: solid; border-width: 1px;\" width=\"280\"><b>*Currency Feed Base Currency Code:</b></td><td style=\"border-style: solid; border-width: 1px;\" ><input type=\"text\" size=\"3\" id=\"CurrencyFeedBaseRateCurrencyCode\" name=\"CurrencyFeedBaseRateCurrencyCode\" value=\"" + AppLogic.AppConfig("Localization.CurrencyFeedBaseRateCurrencyCode") + "\"></td><td style=\"border-style: solid; border-width: 1px;\" ><small>You MUST set this to a valid currency code, or the store will not know how to apply the exchange rates in the table. This MAY or MAY NOT be the same as your AppConfig:Localization.StoreCurrency code!</small></td></tr>");
            writer.Write("<tr><td style=\"border-style: solid; border-width: 1px;\" width=\"280\"><b>Currency Feed XmlPackage:</b></td><td style=\"border-style: solid; border-width: 1px;\" ><input type=\"text\" size=\"40\" id=\"CurrencyFeedXmlPackage\" name=\"CurrencyFeedXmlPackage\" value=\"" + AppLogic.AppConfig("Localization.CurrencyFeedXmlPackage") + "\"></td><td style=\"border-style: solid; border-width: 1px;\" ><small>If you will be maintaining your exchange rate table manually, set this appconfig value to be an empty string</small></td></tr>");
            writer.Write("</table>");

            writer.Write("<p align=\"left\">");
            writer.Write("<b>Test Conversion</b> ");
            Decimal SourceAmount = CommonLogic.FormNativeDecimal("SourceAmount");
            if (SourceAmount == System.Decimal.Zero)
            {
                SourceAmount = 1.00M;
            }
            writer.Write("Amount: <input type=\"text\" size=\"8\" id=\"SourceAmount\" name=\"SourceAmount\" value=\"" + Localization.CurrencyStringForDBWithoutExchangeRate(SourceAmount) + "\">");
            String SourceCurrency = CommonLogic.FormCanBeDangerousContent("SourceCurrency");
            writer.Write("&nbsp;&nbsp;Source: " + Currency.GetSelectList("SourceCurrency", String.Empty, String.Empty, SourceCurrency));
            String TargetCurrency = CommonLogic.FormCanBeDangerousContent("TargetCurrency");
            writer.Write("&nbsp;&nbsp;Target: " + Currency.GetSelectList("TargetCurrency", String.Empty, String.Empty, TargetCurrency));
            if (SourceCurrency.Length != 0 && TargetCurrency.Length != 0)
            {
                Decimal TargetAmount = Currency.Convert(SourceAmount, SourceCurrency, TargetCurrency);
                writer.Write("&nbsp;&nbsp;Result: <input type=\"text\" size=\"8\" id=\"TargetAmount\" name=\"TargetAmount\" value=\"" + Currency.ToString(TargetAmount, TargetCurrency) + "\" READONLY/>");
            }
            writer.Write("&nbsp;&nbsp;<input class=\"normalButtons\" type=\"submit\" value=\"Update & Convert\" name=\"Submit\"/>");
            writer.Write("</p>");

            writer.Write("<p align=\"left\"><input class=\"normalButtons\" type=\"submit\" value=\"Update\" name=\"Submit\"/></p>\n");

            writer.Write("  <table border=\"0\" cellpadding=\"0\" border=\"0\" cellspacing=\"0\" width=\"100%\">");
            writer.Write("    <tr class=\"table-header\">\n");
            writer.Write("</td>");
            writer.Write("      <td ><b>ID</b></td>\n");
            writer.Write("      <td ><b>*Name</b></td>\n");
            writer.Write("      <td ><b>*Code</b></td>\n");
            writer.Write("      <td ><b>Symbol</b></td>\n");
            writer.Write("      <td ><b>Exchange Rate</b></td>\n");
            writer.Write("      <td ><b>Display Locale</b></td>\n");
            writer.Write("      <td ><b>Display Spec</b></td>\n");
            writer.Write("      <td ><b>Published</b></td>\n");
            writer.Write("      <td ><b>Last Updated On</b></td>\n");
            writer.Write("      <td ><b>*Display Order</b></td>\n");
            writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
            writer.Write("    </tr>\n");

            string style;
            int counter = 0;

            using (SqlConnection dbconn = DB.dbConn())
            {
                dbconn.Open();
                using (IDataReader rs = DB.GetRS("select * from Currency  with (NOLOCK)  order by published desc, displayorder,name", dbconn))
                {
                    while (rs.Read())
                    {
                        if (counter % 2 == 0)
                        {
                            style = "\"table-row2\"";
                        }
                        else
                        {
                            style = "\"table-alternatingrow2\"";
                        }
                        int ID = DB.RSFieldInt(rs, "CurrencyID");
                        writer.Write("<tr class=" + style + ">\n");
                        writer.Write("<td>" + ID.ToString() + "</td>\n");
                        writer.Write("<td><input type=\"text\" size=\"30\" id=\"Name_" + ID.ToString() + "\" name=\"Name_" + ID.ToString() + "\" value=\"" + DB.RSField(rs, "Name").ToString() + "\"/></td>\n");
                        writer.Write("<td><input type=\"text\" size=\"4\" id=\"CurrencyCode_" + ID.ToString() + "\" name=\"CurrencyCode_" + ID.ToString() + "\" value=\"" + DB.RSField(rs, "CurrencyCode").ToString() + "\"/><input type=\"hidden\" id=\"CurrencyCode_" + ID.ToString() + "_vldt\" name=\"CurrencyCode_" + ID.ToString() + "_vldt\" value=\"[req]\"/></td>\n");
                        writer.Write("<td><input type=\"text\" size=\"5\" id=\"Symbol_" + ID.ToString() + "\" name=\"Symbol_" + ID.ToString() + "\" value=\"" + DB.RSField(rs, "Symbol").ToString() + "\"/></td>\n");
                        writer.Write("<td>");
                        String RTX = Localization.DecimalStringForDB(DB.RSFieldDecimal(rs, "ExchangeRate"));
                        if (DB.RSFieldDecimal(rs, "ExchangeRate") == System.Decimal.Zero && DB.RSFieldBool(rs, "Published"))
                        {
                            RTX = String.Empty; // force entry for all published currencies, 0.0 exchange rate is totally invalid!
                        }
                        writer.Write("<input type=\"text\" size=\"6\" id=\"ExchangeRate_" + ID.ToString() + "\" name=\"ExchangeRate_" + ID.ToString() + "\" value=\"" + RTX + "\"/>" + CommonLogic.IIF(DB.RSFieldBool(rs, "WasLiveRate"), " (Live)", ""));
                        writer.Write("<input type=\"hidden\" id=\"ExchangeRate_" + ID.ToString() + "_vldt\" name=\"ExchangeRate_" + ID.ToString() + "_vldt\" value=\"[req][number][blankalert=Please enter the Exchange Rate][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\"/>");
                        writer.Write("</td>\n");
                        writer.Write("<td><input type=\"text\" id=\"DisplayLocaleFormat_" + ID.ToString() + "\" name=\"DisplayLocaleFormat_" + ID.ToString() + "\" value=\"" + DB.RSField(rs, "DisplayLocaleFormat").ToString() + "\"/></td>\n");
                        writer.Write("<td><input type=\"text\" id=\"DisplaySpec_" + ID.ToString() + "\" name=\"DisplaySpec_" + ID.ToString() + "\" value=\"" + DB.RSField(rs, "DisplaySpec").ToString() + "\"/></td>\n");
                        writer.Write("<td><input type=\"checkbox\" id=\"Published_" + ID.ToString() + "\" name=\"Published_" + ID.ToString() + "\" " + CommonLogic.IIF(DB.RSFieldBool(rs, "Published"), " checked=\"checked\" ", "") + "/></td>\n");
                        writer.Write("<td>" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs, "LastUpdated")) + "</td>\n");
                        writer.Write("<td align=\"center\"><input size=\"2\" type=\"text\" name=\"DisplayOrder_" + ID.ToString() + "\" value=\"" + DB.RSFieldInt(rs, "DisplayOrder").ToString() + "\"/></td>\n");
                        writer.Write("<td align=\"center\"><input class=\"normalButtons\" type=\"button\" value=\"Delete\" name=\"Delete_" + ID.ToString() + "\" onClick=\"DeleteCurrency(" + ID.ToString() + ")\"/></td>\n");
                        writer.Write("</tr>\n");
                        counter++;
                    }
                }
            }

            writer.Write("<tr>\n");
            writer.Write("<td>Add New:</td>\n");
            writer.Write("<td><input type=\"text\" size=\"30\" id=\"Name_0\" name=\"Name_0\"/></td>\n");
            writer.Write("<td><input type=\"text\" size=\"4\" id=\"CurrencyCode_0\" name=\"CurrencyCode_0\"/></td>\n");
            writer.Write("<td><input type=\"text\" size=\"5\" id=\"Symbol_0\" name=\"Symbol_0\"/></td>\n");
            writer.Write("<td><input type=\"text\" size=\"6\" id=\"ExchangeRate_0\" name=\"ExchangeRate_0\"/></td>\n");
            writer.Write("<td><input type=\"text\" id=\"DisplayLocaleFormat_0\" name=\"DisplayLocaleFormat_0\"/></td>\n");
            writer.Write("<td><input type=\"text\" id=\"DisplaySpec_0\" name=\"DisplaySpec_0\"/></td>\n");
            writer.Write("<td><input type=\"checkbox\" id=\"Published_0\" name=\"Published_0\"/></td>\n");
            writer.Write("<td>&nbsp;</td>\n");
            writer.Write("<td align=\"center\"><input size=\"2\" type=\"text\" name=\"DisplayOrder_0\"/></td>\n");
            writer.Write("<td align=\"center\">&nbsp;</td>\n");
            writer.Write("</tr>\n");

            writer.Write("</table>\n");
            writer.Write("<p align=\"left\"><input class=\"normalButtons\" type=\"submit\" value=\"Update Changes Above\" name=\"Submit\"/></p>\n");
            writer.Write("</form>\n");

            writer.Write("<script type=\"text/javascript\">\n");
            writer.Write("function DeleteCurrency(id)\n");
            writer.Write("{\n");
            writer.Write("if(confirm('Are you sure you want to delete currency: ' + id))\n");
            writer.Write("{\n");
            writer.Write("self.location = 'currencies.aspx?deleteid=' + id;\n");
            writer.Write("}\n");
            writer.Write("}\n");
            writer.Write("</SCRIPT>\n");

            writer.Write("<hr size=\"1\">");
            writer.Write("<b>XmlPackage Doc (Raw Response From Rates Url)</b><br/>");
            writer.Write("<textarea style=\"width: 100%\" rows=\"60\">" + XmlCommon.PrettyPrintXml(Currency.m_LastRatesResponseXml) + "</textarea>");
            writer.Write("<b>Transform Master Xml</b><br/>");
            writer.Write("<textarea style=\"width: 100%\" rows=\"60\">" + XmlCommon.PrettyPrintXml(Currency.m_LastRatesTransformedXml) + "</textarea>");
        }


    }
}
