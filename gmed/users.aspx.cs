// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using AspDotNetStorefrontCore;

namespace AspDotNetStorefrontAdmin
{
    /// <summary>
    /// Summary description for users.
    /// </summary>
    public partial class users : AspDotNetStorefront.SkinBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");


            SectionTitle = "Manage Admin Users";
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (ThisCustomer.IsAdminSuperUser)
            {
                if (CommonLogic.QueryStringCanBeDangerousContent("RemoveAdminID").Length != 0)
                {
                    // remove admin rights from this user:
                    DB.ExecuteSQL("update customer set IsAdmin=0 where CustomerID=" + CommonLogic.QueryStringCanBeDangerousContent("RemoveAdminID"));
                }

                if (CommonLogic.FormBool("IsSubmit"))
                {
                    // add admin rights to this user:
                    DB.ExecuteSQL("update customer set IsAdmin=1 where deleted=0 and EMail=" + DB.SQuote(CommonLogic.FormCanBeDangerousContent("EMail").ToLowerInvariant().Trim()));
                }

                writer.Write("<p>The following users are store administrators:</p>\n");

                string SuperuserFilter = CommonLogic.IIF(ThisCustomer.IsAdminSuperUser, String.Empty, String.Format(" CustomerID not in ({0}) and ", AppLogic.AppConfig("Admin_Superuser")));

                writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"0\" width=\"100%\">\n");
                writer.Write("    <tr class=\"gridHeader\">\n");
                writer.Write("      <td ><b>ID</b></td>\n");
                writer.Write("      <td ><b>E-Mail</b></td>\n");
                writer.Write("      <td align=\"center\"><b>First Name</b></td>\n");
                writer.Write("      <td align=\"center\"><b>Last Name</b></td>\n");
                writer.Write("      <td align=\"center\"><b>Remove Admin Rights</b></td>\n");
                writer.Write("    </tr>\n");
                
                using (SqlConnection conn = new SqlConnection(DB.GetDBConn()))
                {
                    conn.Open();
                    using (IDataReader rs = DB.GetRS("select * from customer   with (NOLOCK)  where deleted=0 and " + SuperuserFilter.ToString() + " IsAdmin=1 order by EMail",conn))
                    {
                        int i = 0;
                        while (rs.Read())
                        {
                            i++; 
                            if (i % 2 == 0)
                            {
                                writer.Write("    <tr class=\"table-row2\">\n");
                            }
                            else 
                            {
                                writer.Write("    <tr class=\"table-alternatingrow2\">\n");
                            }
                            
                            writer.Write("      <td >" + DB.RSFieldInt(rs, "CustomerID").ToString() + "</td>\n");
                            writer.Write("      <td ><a href=\"cst_account.aspx?customerid=" + DB.RSFieldInt(rs, "CustomerID").ToString() + "\">" + CommonLogic.IIF(Customer.StaticIsAdminSuperUser(DB.RSFieldInt(rs, "CustomerID")), "*", "") + DB.RSField(rs, "EMail") + "</a></td>\n");
                            writer.Write("      <td >" + DB.RSField(rs, "FirstName") + "</td>\n");
                            writer.Write("      <td >" + DB.RSField(rs, "LastName") + "</td>\n");
                            if (Customer.StaticIsAdminSuperUser(DB.RSFieldInt(rs, "CustomerID")))
                            {
                                writer.Write("<td align=\"center\">Admin SuperUser</td>");
                            }
                            else
                            {
                                writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Remove Admin Rights\" name=\"RemoveAdmin_" + DB.RSFieldInt(rs, "CustomerID").ToString() + "\" onClick=\"RemoveAdmin(" + DB.RSFieldInt(rs, "CustomerID").ToString() + ")\"></td>\n");
                            }
                            writer.Write("    </tr>\n");
                        
                        }
                    
                    }
                
                
                }
                
                writer.Write("  </table>\n");

                writer.Write("<script type=\"text/javascript\">\n");
                writer.Write("function UserForm_Validator(theForm)\n");
                writer.Write("{\n");
                writer.Write("submitonce(theForm);\n");
                writer.Write("return (true);\n");
                writer.Write("}\n");
                writer.Write("</script>\n");

                writer.Write("<form action=\"users.aspx\" method=\"post\" id=\"UserForm\" name=\"UserForm\" onsubmit=\"return (validateForm(this) && UserForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
                writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
                writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
                writer.Write("              <tr valign=\"middle\">\n");
                writer.Write("                <td width=\"30%\" align=\"right\" valign=\"top\">*Assign Admin Privileges To User:&nbsp;&nbsp;</td>\n");
                writer.Write("                <td align=\"left\" valign=\"top\">\n");
                writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"EMail\" value=\"\"><br/>Enter the e-mail address of the user you want to make a store administrator.<br/>This customer record must already existing in the database<br/>If you need to create a new customer record first, please do that first.\n");
                writer.Write("                	<input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter the e-mail address of the user you want to set admin privileges for. This customer record must already exist. If you need to a new customer record first, select the User -> Add New Customer menu option!]\">\n");
                writer.Write("                	</td>\n");
                writer.Write("              </tr>\n");
                writer.Write("<tr>\n");
                writer.Write("<td></td><td align=\"left\"><br/>\n");
                writer.Write("<input type=\"submit\" class=\"normalButtons\" value=\"Add New Admin\" name=\"submit\">\n");
                writer.Write("        </td>\n");
                writer.Write("      </tr>\n");
                writer.Write("  </table>\n");
                writer.Write("</form>\n");

                writer.Write("<script type=\"text/javascript\">\n");
                writer.Write("function RemoveAdmin(id)\n");
                writer.Write("{\n");
                writer.Write("if(confirm('Are you sure you want to remove the admin rights of customer: ' + id + ' (this does not delete their user record, just their admin rights)'))\n");
                writer.Write("{\n");
                writer.Write("self.location = 'users.aspx?RemoveAdminId=' + id;\n");
                writer.Write("}\n");
                writer.Write("}\n");
                writer.Write("</SCRIPT>\n");
            }
            else
            {
                writer.Write("<p><b>INSUFFICIENT PERMISSIONS</b></p>");
            }
        }

    }
}
