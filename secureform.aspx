<%@ Page Language="c#" Inherits="AspDotNetStorefront.secureform" CodeFile="secureform.aspx.cs" %>
<%@ Register TagPrefix="aspdnsf" TagName="Topic" Src="TopicControl.ascx" %>
<html>
<head>
</head>
<body>
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%" Visible="false">
        <p>
            <b>
                <asp:Literal ID="Literal1" runat="server" Text="(!secureform.aspx.1!)"></asp:Literal></b></p>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="100%" Visible="true">
        <div style="margin: 0px; padding: 8px; border-width: 1px; border-style: solid; border-color: #888888; background-color: #EEEEEE;">
            <aspdnsf:Topic ID="Hdr" runat="Server" TopicName="3DSecureExplanation"/>
        </div>
        <br />
        <div>
            <iframe src="secureauth.aspx" width="100%" height="500" scrolling="auto" frameborder="0" style="margin: 0px; padding: 8px; border-width: 1px; border-style: solid; border-color: #888888;">
            </iframe>
        </div>
    </asp:Panel>
</body>
</html>
