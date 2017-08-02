<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news.aspx.cs" Inherits="news" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache  Duration="1"  Location="none" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Coupons</title>
    <asp:Literal runat="server" ID="ltStyles"></asp:Literal>
 
    <link href="skins/Skin_1/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmNews" runat="server">
        <div class="breadCrumb3">
    <asp:Literal ID="ltScript1" runat="server"></asp:Literal></div>
    <div id="help">
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="toppage">
              <tr>
                <td align="left" valign="middle" style="height: 36px">
	                    <table border="0" cellspacing="0" cellpadding="5" class="breadCrumb3">
                            <tr>
                                <td align="left" valign="middle">Now In:</td>
                                <td align="left" valign="middle">Manage News</td>
                                <td align="left" valign="middle">View:</td>
                                <td align="left" valign="middle"><a href="splash.aspx">Home</a></td>
                            </tr>
                        </table>
	            </td>
              </tr>
        </table>
        <div style="margin-bottom: 5px; margin-top: 5px;" class="breadCrumb3">
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
                                <td class="titleTable" width="150">
                                    <font class="subTitle">News List:</font>
                                </td>
                                <td style="width: 5px;" />
                                <td style="width: 1px; background-color: #EBEBEB;" />
                                <td style="width: 5px;" />
                                <td class="titleTable">
                                    <font class="subTitle">News Details:</font>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentTable" valign="top" width="150">
				                    <div class="wrapperTopBottom">
					                    <asp:Button runat="server" ID="btnAdd" CssClass="normalButtons" Text="ADD NEW" OnClick="btnAdd_Click" />
				                    </div>
                                    <div class="wrapperTopBottom">
                                        <asp:TreeView ID="treeMain" runat="server" OnSelectedNodeChanged="treeMain_SelectedNodeChanged">
                                        </asp:TreeView>
                                    </div>                                    
                                </td>
                                <td style="width: 5px;" />
                                <td style="width: 1px; background-color: #EBEBEB;" />
                                <td style="width: 5px;" />
                                <td class="contentTable" valign="top" width="*">
                                    <div class="wrapperLeft">
                                        <asp:PlaceHolder ID="phMain" runat="server">
                                            <div style="width: 100% " runat ="server" id="div1" class="titleMessage">
                                                <asp:Literal runat="server" ID="ltMode"></asp:Literal> News
                                            </div>
                                            
                                            <div style="width: 100%" runat ="server" id="divPageLocale" class="titleMessage">
                                                &nbsp;&nbsp;Select Locale  <asp:DropDownList ID="ddlPageLocales" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPageLocales_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            
                                            <p>
                                                Please enter the following information about this news item. Fields marked with an asterisk (*) are required. All other fields are optional.
                                            </p>
                                            <table width="100%" cellpadding="1" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="right" valign="top">
                                                        <font class="subTitleSmall">*Headline:</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox runat="server" ID="ltHeadline" Columns="30"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top">
                                                        <font class="subTitleSmall">News Copy:</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Literal ID="ltCopy" runat="server"></asp:Literal>
                                                        <ed:RadeditorWrapper id="radCopy" runat="server"></ed:RadeditorWrapper>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle">
                                                        <font class="subTitleSmall">*Expiration Date:</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                                        <asp:Literal ID="ltDate" runat="server"></asp:Literal>
                                                        <asp:RequiredFieldValidator ErrorMessage="Fill in Expiration Date" ControlToValidate="txtDate" ID="RequiredFieldValidator1" SetFocusOnError="true" runat="server">!!</asp:RequiredFieldValidator> 
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle">
                                                        <font class="subTitleSmall">*Published:</font>
                                                    </td>
                                                    <td align="left" valign="middle">
                                                        <asp:RadioButtonList ID="rbPublished" runat="server">
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                            <asp:ListItem Value="1" Selected="true">Yes</asp:ListItem>                                                            
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:ValidationSummary ID="validationSummary" runat="server" EnableClientScript="true" ShowMessageBox="true" ShowSummary="false" Enabled="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div style="width: 100%; text-align: center;">
                                                &nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" CssClass="normalButtons" OnClick="btnSubmit_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="btnDelete" runat="server" CssClass="normalButtons" OnClick="btnDelete_Click" Text="Delete News Item" />
                                            </div>
                                        </asp:PlaceHolder>
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
