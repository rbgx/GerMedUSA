// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
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
using System.Data.SqlClient;

namespace AspDotNetStorefrontAdmin
{

    public partial class localesettings : System.Web.UI.Page
    {
        protected string selectSQL = "select * from LocaleSetting  with (NOLOCK) ";
        protected Customer cust;

        protected void Page_Load(object sender, EventArgs e)
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

            cust = ((AspDotNetStorefrontPrincipal)Context.User).ThisCustomer;

            if (!IsPostBack)
            {
                deleteXX();

                this.loadScript(false);
                ViewState["Sort"] = "displayorder,description";
                ViewState["SortOrder"] = "";
                ViewState["SQLString"] = this.selectSQL;

                ShowAddPanel(false);
                if ((AppLogic.NumLocaleSettingsInstalled() > 0 && AppLogic.NumLocaleSettingsInstalled() < 2) && (AppLogic.ProductIsMLX() || AppLogic.ProductIsMLExpress()))
                {
                    btnInsert.Visible = false;
                }
                else
                {
                    btnInsert.Visible = true;
                }
            }
        }

        protected void loadScript(bool load)
        {
            if (load)
            {
                this.ltScript.Text = "";
            }
            else
            {
                this.ltScript.Text = "";
            }
        }

        protected void buildGridData()
        {

            string sql = ViewState["SQLString"].ToString();
            sql += " order by " + ViewState["Sort"].ToString() + " " + ViewState["SortOrder"].ToString();

            using (SqlConnection dbconn = DB.dbConn())
            {
                dbconn.Open();
                using (IDataReader rs = DB.GetRS(sql, dbconn))
                {
                    using (DataTable dt = new DataTable())
                    {
                        dt.Load(rs);
                        gMain.DataSource = dt;
                        gMain.DataBind();
                    }
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

            ((Literal)this.Form.FindControl("ltError")).Text = str;
        }

        protected void gMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ViewState["SQLString"] = this.selectSQL;

            gMain.EditIndex = -1;
            this.buildGridData();
        }
        protected void gMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                DataRowView myrow = (DataRowView)e.Row.DataItem;
                int cID = Localization.ParseNativeInt(myrow["DefaultCurrencyID"].ToString());
                DropDownList dd = (DropDownList)e.Row.FindControl("ddCurrency");
                ArrayList list = Currency.getCurrencyList();
                foreach (ListItemClass li in list)
                {
                    dd.Items.Add(new ListItem(li.Item, li.Value.ToString()));
                    if (li.Value == cID)
                    {
                        dd.Items.FindByValue(cID.ToString()).Selected = true;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ib = (ImageButton)e.Row.FindControl("imgDelete");
                ib.Attributes.Add("onClick", "javascript: return confirm('Confirm Delete?')");

                if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                {
                    //Click to edit
                    e.Row.Attributes.Add("ondblclick", "javascript:__doPostBack('gMain','Edit$" + e.Row.RowIndex + "')");

                    //load Currency
                    DataRowView myrow = (DataRowView)e.Row.DataItem;
                    int cID = Localization.ParseNativeInt(myrow["DefaultCurrencyID"].ToString());
                    e.Row.Cells[4].Text = Currency.GetCurrencyCode(cID) + " (" + Currency.GetName(cID) + ")";
                }
            }
        }
        protected void gMain_Sorting(object sender, GridViewSortEventArgs e)
        {
            gMain.EditIndex = -1;
            ViewState["Sort"] = e.SortExpression.ToString();
            ViewState["SortOrder"] = (ViewState["SortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.buildGridData();
        }
        protected void gMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.resetError("", false);

            if (e.CommandName == "DeleteItem")
            {
                gMain.EditIndex = -1;
                int iden = Localization.ParseNativeInt(e.CommandArgument.ToString());
                this.deleteRowPerm(iden);
            }
        }
        protected void deleteRowPerm(int iden)
        {
            try
            {
                DB.ExecuteSQL("delete from LocaleSetting where LocaleSettingid=" + iden);
                AppLogic.UpdateNumLocaleSettingsInstalled();
                this.buildGridData();
                this.resetError("Item Deleted", false);
                if ((AppLogic.NumLocaleSettingsInstalled() > 0 && AppLogic.NumLocaleSettingsInstalled() < 2) && (AppLogic.ProductIsMLX() || AppLogic.ProductIsMLExpress()))
                {
                    btnInsert.Visible = false;
                }
                else
                {
                    btnInsert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.resetError("Couldn't delete from database: " + ex.ToString(), true);
            }
        }
        protected void deleteXX()
        {
            try
            {
                DB.ExecuteSQL("delete from LocaleSetting where [Name] LIKE 'xx-XX%'");
                AppLogic.UpdateNumLocaleSettingsInstalled();
            }
            catch (Exception ex)
            {
                this.resetError("Couldn't delete from database: " + ex.ToString(), true);
            }
        }

        protected void gMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.resetError("", false);
            gMain.PageIndex = e.NewPageIndex;
            gMain.EditIndex = -1;
            this.buildGridData();
        }
        protected void gMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gMain.Rows[e.RowIndex];

            if (row != null)
            {
                string iden = row.Cells[1].Text.ToString();
                string name = CommonLogic.Left(((TextBox)row.FindControl("txtName")).Text, 10).Replace(" ", "-");
                string description = CommonLogic.Left(((TextBox)row.FindControl("txtDescription")).Text, 100);
                string cID = ((DropDownList)row.FindControl("ddCurrency")).SelectedValue;
                string order = ((TextBox)row.FindControl("txtOrder")).Text;

                // see if this LocaleSetting already exists:
                int N = DB.GetSqlN("select count(name) as N from LocaleSetting   with (NOLOCK)  where LocaleSettingID<>" + iden + " and Name=" + DB.SQuote(name));
                if (N != 0)
                {
                    this.resetError("There is already another Locale with that name.", true);
                    return;
                }

                StringBuilder sql = new StringBuilder(2500);

                // ok to update:
                sql.Append("update LocaleSetting set ");
                sql.Append("Name=" + DB.SQuote(Localization.CheckLocaleSettingForProperCase(name)) + ",");
                sql.Append("Description=" + DB.SQuote(description) + ",");
                sql.Append("DefaultCurrencyID=" + cID + ",");
                sql.Append("DisplayOrder=" + order);
                sql.Append(" where LocaleSettingID=" + iden);

                this.resetError("Item updated", false);

                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    deleteXX();

                    gMain.EditIndex = -1;
                    ViewState["SQLString"] = this.selectSQL;
                    this.buildGridData();
                }
                catch (Exception ex)
                {
                    throw new Exception("Couldn't update database: " + sql.ToString() + ex.ToString());
                }
            }
        }
        protected void gMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gMain.EditIndex = e.NewEditIndex;

            this.loadScript(true);

            this.buildGridData();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            resetError("", false);

            ShowAddPanel(true);

            txtDescription.Text = "";
            txtName.Text = "";
            txtOrder.Text = "1";

            ddCurrency.Items.Clear();
            ArrayList list = Currency.getCurrencyList();
            ddCurrency.Items.Add(new ListItem("- Select -", "0"));

            foreach (ListItemClass li in list)
            {
                ddCurrency.Items.Add(new ListItem(li.Item, li.Value.ToString()));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            resetError("", false);
            StringBuilder sql = new StringBuilder();

            if (ValidInput())
            {
                string name = txtName.Text.Trim();
                string description = txtDescription.Text.Trim();
                string currency = ddCurrency.SelectedValue;
                string displayorder = txtOrder.Text.Trim();

                // ok to add them:
                String NewGUID = DB.GetNewGUID();
                sql.Append("insert into LocaleSetting(LocaleSettingGUID,Name,Description,DefaultCurrencyID,DisplayOrder) values(");
                sql.Append(DB.SQuote(NewGUID) + ",");
                sql.Append(DB.SQuote(name) + ",");
                sql.Append(DB.SQuote(description) + ",");
                sql.Append(currency + ",");
                sql.Append(DB.SQuote(displayorder));
                sql.Append(")");

                try
                {
                    DB.ExecuteSQL(sql.ToString());
                    this.resetError("Locale added.", false);

                    this.gMain.EditIndex = -1;

                    ShowAddPanel(false);
                }
                catch
                {
                    this.resetError("Locale already exists.", true);
                    ShowAddPanel(true);
                }
            }
            else
            {
                this.resetError("Please input all required fields.", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            resetError("", false);

            ShowAddPanel(false);
        }

        protected bool ValidInput()
        {
            return true;
        }

        protected void ShowAddPanel(bool showAdd)
        {
            if (showAdd)
            {
                loadScript(true);
                pnlAdd.Visible = true;
                pnlGrid.Visible = false;
            }
            else
            {
                loadScript(false);
                pnlAdd.Visible = false;
                pnlGrid.Visible = true;

                buildGridData();
            }
        }
    }
}