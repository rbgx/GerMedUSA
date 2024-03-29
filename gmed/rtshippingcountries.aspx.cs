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
using AspDotNetStorefrontCore;
using System.Data.Sql;
using System.Data.SqlClient;

namespace AspDotNetStorefrontAdmin
{
    /// <summary>
    /// Summary description for ShippingMethodCountries.
    /// </summary>
    public partial class RTShippingCountries : AspDotNetStorefront.SkinBase
    {

        int _rtShippingProviderID = 0;
        string _rtShippingProviderName = string.Empty;
        bool IsUpdated = false;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");


            _rtShippingProviderID = CommonLogic.QueryStringUSInt("RtShippingProviderID");
            if (_rtShippingProviderID == 0)
            {
                Response.Redirect("rtshippingmgr.aspx");
            }

            using (SqlConnection con = new SqlConnection(DB.GetDBConn()))
            {
                con.Open();
                using (IDataReader reader = DB.GetRS(string.Format("SELECT [Name] FROM RTShippingProvider WHERE RTShippingProviderID = {0}", _rtShippingProviderID.ToString()), con))
                {
                    if (reader.Read())
                    {
                        _rtShippingProviderName = DB.RSField(reader, "Name");
                    }
                }
            }

            SectionTitle = "<a href=\"rtshippingmgr.aspx\">RTShipping Providers</a> - Setting Allowed Countries for Real Time Shipping Providers: " + _rtShippingProviderName;

            if (CommonLogic.FormBool("IsSubmit"))
            {

                IsUpdated = true;

                DB.ExecuteSQL("delete from RTShippingProviderToCountryMap where RtShippingProviderID=" + _rtShippingProviderID.ToString());
                foreach (String s in CommonLogic.FormCanBeDangerousContent("CountryList").Split(','))
                {
                    if (s.Trim().Length != 0)
                    {
                        DB.ExecuteSQL("insert RTShippingProviderToCountryMap(RtShippingProviderID,CountryID) values(" + _rtShippingProviderID.ToString() + "," + s + ")");
                    }
                }
            }

            if (CommonLogic.QueryStringCanBeDangerousContent("clearall").Length != 0)
            {
                DB.ExecuteSQL("delete from RTShippingProviderToCountryMap where RtShippingProviderID=" + _rtShippingProviderID.ToString());
            }
            if (CommonLogic.QueryStringCanBeDangerousContent("allowall").Length != 0)
            {
                DB.ExecuteSQL("delete from RTShippingProviderToCountryMap where RtShippingProviderID=" + _rtShippingProviderID.ToString());
                DB.ExecuteSQL("insert into RTShippingProviderToCountryMap(RtShippingProviderID,CountryID) select " + _rtShippingProviderID.ToString() + ",CountryID from Country");
            }
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (DB.GetSqlN("select count(*) as N from Country with (NOLOCK)") == 0)
            {
                writer.Write("<p><b><font color=red>No Countries are defined!</font></b></p>");
            }
            else
            {

                if (IsUpdated)
                {
                    writer.Write("<p><strong>NOTICE: </strong> Item Updated</p?");
                }

                writer.Write("<form method=\"POST\" action=\"rtshippingcountries.aspx?RtShippingProviderID=" + _rtShippingProviderID.ToString() + "\">\n");
                writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
                writer.Write("<p align=\"left\">Check the Countries that you want to <b>ALLOW</b> for this RTShipping Provider.&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"rtshippingcountries.aspx?RtShippingProviderID=" + _rtShippingProviderID.ToString() + "&allowall=true\">ALLOW ALL</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"rtshippingcountries.aspx?RtShippingProviderID=" + _rtShippingProviderID.ToString() + "&clearall=true\">CLEAR ALL</a><p>");

                writer.Write("<p align=\"left\"><input type=\"submit\" value=\"Update\"><p>");
                writer.Write("<table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" >\n");


                using (SqlConnection con = new SqlConnection(DB.GetDBConn()))
                {
                    con.Open();
                    using (IDataReader rs = DB.GetRS("select Country.CountryID,Country.Name,RTShippingProviderToCountryMap.RtShippingProviderID from Country  with (NOLOCK)  left outer join RTShippingProviderToCountryMap  with (NOLOCK)  on Country.CountryID=RTShippingProviderToCountryMap.CountryID and RTShippingProviderToCountryMap.RtShippingProviderID=" + _rtShippingProviderID.ToString() + " order by displayorder,name", con))
                    {
                        while (rs.Read())
                        {
                            bool AllowedForThisCountry = DB.RSFieldInt(rs, "RtShippingProviderID") != 0;
                            writer.Write("<tr class=\"gridHeader\">");
                            writer.Write("<td>");
                            writer.Write(DB.RSField(rs, "Name"));
                            writer.Write("</td>");
                            writer.Write("<td>");
                            writer.Write("<input type=\"checkbox\" name=\"CountryList\" value=\"" + DB.RSFieldInt(rs, "CountryID").ToString() + "\" " + CommonLogic.IIF(AllowedForThisCountry, " checked ", "") + ">");
                            writer.Write("</td>");
                            writer.Write("</tr>\n");
                        }
                    }
                }
                writer.Write("</table>");

                writer.Write("<p align=\"left\"><input type=\"submit\" class=\"normalButtons\" value=\"Update\"><p>");
                writer.Write("</form>\n");
            }
        }

    }
}
