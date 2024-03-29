<%@ Page language="c#" Inherits="AspDotNetStorefrontAdmin.Affiliates" CodeFile="affiliates.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache  Duration="1"  Location="none" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Affiliates</title>
    <asp:Literal runat="server" ID="ltStyles"></asp:Literal>
      
    <link href="skins/Skin_1/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmAffiliate" runat="server">
    <asp:Literal ID="ltScript1" runat="server"></asp:Literal>
    <div id="">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="toppage">
              <tr>
                <td align="left" valign="middle" style="height: 36px">
	                    <table border="0" cellspacing="0" cellpadding="5" class="breadCrumb3">
                            <tr>
                                <td align="left" valign="middle"><b>Now In:</b></td>
                                <td align="left" valign="middle">Manage Affiliates</td>
                                <td align="left" valign="middle"><b>View:</b></td>
                                <td align="left" valign="middle"><a href="splash.aspx">Home</a></td>
                            </tr>
                        </table>
	            </td>
              </tr>
        </table>
        <div style="margin-bottom: 5px; margin-top: 5px;">
            <asp:Literal ID="ltNotice" runat="server"></asp:Literal>
            <asp:Literal ID="ltError" runat="server"></asp:Literal>
        </div>
    </div>
    <div id="container">
        <table border="0" cellpadding="1" cellspacing="0" class="outerTable" width="100%">
            <tr>
                <td>
                    <div class="wrapper">                       
                        <table border="0" cellpadding="0" cellspacing="0" class="innerTable" width="100%">
                            <tr>
                                <td class="titleTable" width="130">
                                    <font class="subTitle">Affiliate Search:</font>
                                </td>
                                <td style="width: 5px;" />
                                <td style="width: 1px; background-color: #a2a2a2;" />
                                <td style="width: 5px;" />
                                <td class="titleTable">
                                    <font class="subTitle">Affiliates:</font>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentTableNP" valign="top" width="130">
                                    <asp:TextBox ID="txtSearch" Width="130" runat="server"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnSearch" CssClass="normalButtons" Text="Search" OnClick="btnSearch_Click" />
                                    <br /><br />
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="titleTable">
                                                <font class="subTitle">Index:</font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="contentTableAPL">
                                                <div>
                                                    <asp:TreeView ID="treeMain" runat="server" OnSelectedNodeChanged="treeMain_SelectedNodeChanged">
                                                    </asp:TreeView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 5px;" />
                                <td style="width: 1px; background-color: #a2a2a2;" />
                                <td style="width: 5px;" />
                                <td class="contentTable" valign="top" width="*">
                                    <div class="wrapperLeft">
                                        <div class="wrapperTop">
                                            <br />
    					                    <asp:Button runat="server" ID="btnAdd" CssClass="normalButtons" Text="ADD NEW" OnClick="btnAdd_Click" /><br />
                                            <br />
                                            &nbsp;</div>
                                        <asp:GridView Width="100%" ID="gMain" runat="server" PagerStyle-HorizontalAlign="left" PagerSettings-Position="TopAndBottom" 
                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="15" AllowSorting="True" HorizontalAlign="Left" OnRowCommand="gMain_RowCommand" 
                                        OnRowDataBound="gMain_RowDataBound" OnSorting="gMain_Sorting" OnPageIndexChanging="gMain_PageIndexChanging" BorderWidth="0px" 
                                        CellPadding="0" BorderStyle="None" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <a href='editAffiliates.aspx?iden=<%# DataBinder.Eval(Container.DataItem, "AffiliateID")%>'><img src="skins/Skin_1/images/edit.gif" border="0" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="AffiliateID" HeaderText="ID" ReadOnly="True" SortExpression="AffiliateID" >
                                                    <ItemStyle CssClass="lighterData" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <a href='editAffiliates.aspx?iden=<%# DataBinder.Eval(Container.DataItem, "AffiliateID")%>'>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </a>
                                                        <asp:Literal id="ltName" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="normalData" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="E-Mail" SortExpression="EMail">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltEMail" runat="Server"></asp:Literal>
						                            </ItemTemplate>
                                                    <ItemStyle CssClass="lighterData" />
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltShipTo" runat="server"></asp:Literal>
							                        </ItemTemplate>
                                                    <ItemStyle CssClass="lightData" />
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField HeaderText="URL">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "URL")%>
							                        </ItemTemplate>
                                                    <ItemStyle CssClass="lighterData" />
						                        </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDelete" CommandName="DeleteItem" CommandArgument='<%# Eval("AffiliateID") %>' runat="Server" AlternateText="Delete" ImageUrl="skins/Skin_1/images/delete2.gif" />                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="selectData" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                Mode="NumericFirstLast" PageButtonCount="15" Position="TopAndBottom" />
                                            <FooterStyle CssClass="footerGrid" />
                                            <RowStyle CssClass="gridRow" />
                                            <EditRowStyle CssClass="DataCellGridEdit" />
                                            <PagerStyle CssClass="pagerGrid" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="gridHeader" />
                                            <AlternatingRowStyle CssClass="gridAlternatingRow" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <asp:Literal ID="ltScript" runat="server"></asp:Literal>
</body>
</html>
