// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2009.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. 
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCore;
using System.Data.SqlClient;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for failedtransactions.
	/// </summary>
    public partial class failedtransactions : AspDotNetStorefront.SkinBase
	{
        int ColumnRecurringSubscriptionID;
        int ColumnGateway;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            if (!IsPostBack)
			{
				DateSelect.Items.Add("Last 120");
				for(DateTime dt = System.DateTime.Now; dt >= System.DateTime.Now.AddDays(-60); dt = dt.AddDays(-1))
				{
					DateSelect.Items.Add(dt.ToShortDateString());
				}
			}
			SectionTitle = "Failed Transactions";
			LoadGrid();
		}

		private void LoadGrid()
		{
			String sql = String.Empty;
			if(DateSelect.Items[DateSelect.SelectedIndex].ToString() == "Last 120")
			{
                sql = "select top 120 * from FailedTransaction order by OrderDate desc";
			}
			else
			{
				DateTime dt = Localization.ParseNativeDateTime(DateSelect.Items[DateSelect.SelectedIndex].ToString());
                sql = "select * from FailedTransaction where Year(OrderDate)=year(" + DB.DateQuote(Localization.ToDBDateTimeString(dt)) + ") and month(OrderDate)=month(" + DB.DateQuote(Localization.ToDBDateTimeString(dt)) + ") and day(OrderDate)=day(" + DB.DateQuote(Localization.ToDBDateTimeString(dt)) + ") order by OrderDate desc";
			}

            using (DataTable dt = new DataTable())
            {
                using (SqlConnection con = new SqlConnection(DB.GetDBConn()))
                {
                    con.Open();
                    using (IDataReader rs = DB.GetRS(sql, con))
                    {
                        dt.Load(rs);
                    }
                }

                if (!AppLogic.ProductIsMLX() && !AppLogic.ProductIsMLExpress())
                {
                    ColumnRecurringSubscriptionID = dt.Columns["RecurringSubscriptionID"].Ordinal;
                }
                else
                {
                    dt.Columns.Remove("recurringsubscriptionid");
                }
                ColumnGateway = dt.Columns["PaymentGateway"].Ordinal;

                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
            }
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				// convert the long data fields to scrolling textarea fields, for compactness:
				foreach(TableCell c in e.Item.Cells)
				{
					if(c.Text.Length > 50)
					{
						c.Text = "<textarea READONLY rows=\"12\" cols=\"50\">" + c.Text + "</textarea>";
					}
				}
                if (!AppLogic.ProductIsMLX() && !AppLogic.ProductIsMLExpress())
                {
                    e.Item.Cells[ColumnRecurringSubscriptionID].Text = BuildSubscriptionIDLink(e.Item.Cells[ColumnRecurringSubscriptionID].Text, e.Item.Cells[ColumnGateway].Text);
                }
            }
			else if(e.Item.ItemType == ListItemType.Footer )
			{
			} 
		}

        private String BuildSubscriptionIDLink(String SubID, String GW)
        {
            if (GW == AspDotNetStorefrontGateways.Gateway.ro_GWVERISIGN || GW == AspDotNetStorefrontGateways.Gateway.ro_GWPAYFLOWPRO)
            {
                return "<a href=\"recurringgatewaydetails.aspx?RecurringSubscriptionID=" + SubID + "\">" + SubID + "</a>";
            }
            else
            {
                return SubID;
            }
        }
	}
}
