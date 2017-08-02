﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="setupFTS.aspx.cs" Inherits="AspDotNetStorefrontAdmin.Admin_setupFTS" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ OutputCache  Duration="1"  Location="none" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Setup Full-Text Search</title>
      <link href="skins/Skin_1/style.css" rel="stylesheet" type="text/css" />
    
<script type="text/javascript">



function CreateNew()
{  
   var textBox1 = document.getElementById("txtNewCatalogName");
   var textBox2 = document.getElementById("txtNewCatalogPath");
   var radioReuse = document.getElementById("radioReuse");
   var radioCreate = document.getElementById("radioCreate");
   
   if (radioCreate.checked == true)
        {
            radioReuse.checked = false;
            var listBox = document.getElementById("lstCatalogNames");
            listBox.selectedIndex = -1;
            listBox.disabled = true;        
            textBox1.disabled = false;
            textBox2.disabled = false;
        }
}


function Reuse()
{
    var textBox1 = document.getElementById("txtNewCatalogName");
    var textBox2 = document.getElementById("txtNewCatalogPath");
    var radioReuse = document.getElementById("radioReuse");
    var radioCreate = document.getElementById("radioCreate");
    
    if (radioReuse.checked == true)
        {
            radioCreate.checked = false;
            var listBox = document.getElementById("lstCatalogNames");
            listBox.disabled = false;
            textBox1.disabled = true;
            textBox2.disabled = true;
            textBox1.value = "";
            textBox2.value = "";
        }
}



</script>
    
</head>
<body>
    <form id="setupFTS" runat="server">
    <script type="text/javascript">

    function CheckCatalog()
    {
        var textBox1 = document.getElementById("txtNewCatalogName");
        var textBox2 = document.getElementById("txtNewCatalogPath");
        var radioReuse = document.getElementById("radioReuse");
        var radioCreate = document.getElementById("radioCreate");
        
        if (radioCreate.checked == true && radioReuse.checked == false)
        {    
            if (textBox1.value != "" && textBox2.value != "")
            {
                if (confirm("<%=JSwarn1 %>" + textBox1.value + "<%=JSwarn2 %>" + textBox2.value + "?"))
                {
                    document.getElementById("setupFTS").submit;
                }
                else
                {
                    return false;
                }            
            }
        
            if (textBox1.value != "" && textBox2.value == "")
            {
                if (confirm("<%=JSwarn1 %>" + textBox1.value + "<%=JSwarn3 %>"))
                {
                    document.getElementById("setupFTS").submit;
                }
                else
                {
                    return false;
                }            
            }
            
            if (textBox1.value == "" && textBox2.value != "")
            {
                alert("<%=JSwarn4 %>");
                return false;         
            }
            
            if (textBox1.value == "" && textBox2.value == "")
            {
                alert("<%=JSwarn5 %>");
                return false;
            }  
        }
        
        else if (radioReuse.checked == true && radioCreate.checked == false)
        {
            if (textBox1.value == "" && textBox2.value == "")
            {
                var listBox = document.getElementById("lstCatalogNames");
                var text = "";   
       
                for (i = 0; i < listBox.options.length; i++)
                {
                    if (listBox.options[i].selected)
                    {   
                     text = text + listBox.options[i].text;
                    }
                }
                
                if (text == "")
                {
                    alert("<%=JSwarn6 %>");
                    return false;
                }
                else
                {
                    if (confirm("<%=JSwarn7 %>" + text + "?"))
                    {
                        document.getElementById("setupFTS").submit;
                    }
                    else
                    {
                        return false;
                    }                
                }
            }
            
            if (textBox1.value != "" || textBox2.value != "")
            {
                alert("<%=JSwarn8 %>");
                textBox1.value = "";
                textBox2.value = "";
                return false;
            }    
        }
        else
        {
            alert("<%=JSwarn6 %>");
            return false;
        }
    }

    function WarnUninstall()
    {
        if(confirm("<%=JSwarn9 %>"))
        {
            return true;        
        }
        else
        {
            return false;
        }
    }

    function WarnOptimize()
    {
        if(confirm("<%=JSwarn10 %>"))
        {
            return true;        
        }
        else
        {
            return false;
        }
    }

    </script>
    <div id=""> 
    <table class="toppage" width="100%" cellspacing="0" cellpadding="0" border="0">
       <tbody>
            <tr>
                <td valign="middle" align="left" style="height: 36px;">
                    <table class="breadCrumb3" cellspacing="0" cellpadding="5" border="0">
                <tbody>
            <tr>
                    <td valign="middle" align="left"><asp:Label ID="admincommonNowIn" runat="server" 
                                        Text="(!admin.common.NowIn!)"></asp:Label></td>
                    <td valign="middle" align="left"> <asp:Label ID="adminlabelView" runat="server" 
                                        Text="(!admin.label.View!)"></asp:Label></td>
                    <td valign= "middle" align="left"> <asp:Label ID="appconfigaspx1" runat="server" 
                                        Text="(!admin.label.View!)"></asp:Label></td>
                    <td valign="middle" align="left"><asp:HyperLink ID="lnkHome" runat="server">[lnkHome]</asp:HyperLink></td>
            </tr>
                </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
     </div>
      <div style="margin-bottom: 5px; margin-top: 5px;">
            <asp:Literal ID="ltError" runat="server"></asp:Literal>
        </div>
    <div>
    <table style="border-width: medium; border-color: #C0C0C0; border-top-style: solid;
        border-right-style: solid; border-left-style: solid; background-color: #F8F8F8;" 
        width="100%">
        <tr>
            <td></td>
        </tr>
        <tr>
            <td style="width:30%">
            </td>
            <td style="width:40%; padding-top: 15px;">
                <table style="border: thin solid #CACA00; background-color: #FFFFE8" width="100%">
                    <tr style="width:100%" align="center">
                        <td align="center">
                        <table>
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 15px; padding-bottom: 10px">
                                    <asp:Label ID="lblIntro" runat="server" Width="100%" Font-Bold="True" Text="(!setupFTS.aspx.2!)">
                                    </asp:Label>
                                </td>
                            </tr>  
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 10px; padding-bottom: 10px">
                                    <asp:Label ID="lblMSFTESQL" runat="server" Text="(!setupFTS.aspx.3!)" Width="100%">
                                    </asp:Label>
                                </td>
                            </tr>                            
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 10px; padding-bottom: 10px">
                                    <asp:Label ID="lblEnableFTS" runat="server" Text="(!setupFTS.aspx.4!)" Width="100%">
                                    </asp:Label>                                    
                                </td>
                            </tr>
                            <tr style="text-align:center" align="center">
                                <td style="vertical-align:middle; padding-top: 10px; padding-bottom: 10px">
                                    <asp:Label ID="lblLanguage" runat="server" Text="(!setupFTS.aspx.5!)"></asp:Label>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" Width="155px" 
                                        ForeColor="Black">
                                        <asp:ListItem>Chinese-Simplified</asp:ListItem>
                                        <asp:ListItem>Chinese-Traditional</asp:ListItem>
                                        <asp:ListItem>Danish</asp:ListItem>
                                        <asp:ListItem>Dutch</asp:ListItem>
                                        <asp:ListItem>English-International</asp:ListItem>
                                        <asp:ListItem>English-US</asp:ListItem>
                                        <asp:ListItem>French</asp:ListItem>
                                        <asp:ListItem>German</asp:ListItem>
                                        <asp:ListItem>Italian</asp:ListItem>
                                        <asp:ListItem>Japanese</asp:ListItem>
                                        <asp:ListItem>Korean</asp:ListItem>
                                        <asp:ListItem Selected="True">Neutral</asp:ListItem>
                                        <asp:ListItem>Polish</asp:ListItem>
                                        <asp:ListItem>Portuguese</asp:ListItem>
                                        <asp:ListItem>Portuguese(Brazil)</asp:ListItem>
                                        <asp:ListItem>Russian</asp:ListItem>
                                        <asp:ListItem>Spanish</asp:ListItem>
                                        <asp:ListItem>Swedish</asp:ListItem>
                                        <asp:ListItem>Thai</asp:ListItem>
                                        <asp:ListItem>Turkish</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 10px; padding-bottom: 10px">
                                <asp:RadioButton ID="radioCreate" runat="server" Text="(!setupFTS.aspx.6!)"/>
                                <asp:RadioButton ID="radioReuse" runat="server" Text="(!setupFTS.aspx.7!)" />                                
                                </td>
                            </tr>                            
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 10px; padding-bottom: 10px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNewCatalogName" runat="server" Text="(!setupFTS.aspx.8!)"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNewCatalogName" runat="server" BackColor="#FFFFE8" 
                                                MaxLength="30" Enabled="False" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top: 10px; padding-bottom: 10px">                                
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNewCatalogPath" runat="server" Text="(!setupFTS.aspx.9!)"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNewCatalogPath" runat="server" BackColor="#FFFFE8" 
                                                MaxLength="80" Enabled="False" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-top:10px;">
                                    <asp:Label ID="lblCatalogList" runat="server" Text="(!setupFTS.aspx.10!)" Width="100%">
                                    </asp:Label>                                    
                                </td>
                            </tr>                            
                            <tr style="text-align:center" align="center">
                                <td align="center" style="padding-bottom: 15px">                                    
                                    <asp:ListBox ID="lstCatalogNames" runat="server" Width="300px" 
                                        BackColor="#FFFFE8" Rows="6"></asp:ListBox>
                                        
                                </td>
                            </tr>                            
                        </table>                                               
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:30%">
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px; padding-bottom: 20px"></td>
        </tr>
    </table>
    </div>
    <div>
        <table style="border-width: medium; border-color: #C0C0C0; border-right-style: solid; border-left-style: solid; background-color: #F8F8F8; border-bottom-style: solid;"
            width="100%">            
            <tr align="center">
                <td style="width:28%">
                </td>
                <td style="padding-top: 10px; padding-bottom: 15px; width:44%">                                    
                        <asp:Button ID="btn_uninstallFTS" CssClass="normalButtons" runat="server" Text="(!setupFTS.aspx.21!)" 
                        OnClientClick="return WarnUninstall()" Width="155px" onclick="btn_uninstallFTS_Click"/>&nbsp;
                        <asp:Button ID="btn_installFTS" CssClass="normalButtons" runat="server" Text="(!setupFTS.aspx.11!)" 
                        OnClientClick="return CheckCatalog()" Width="155px"/>&nbsp;
                        <asp:Button ID="btn_optimize" CssClass="normalButtons" runat="server" Text="(!setupFTS.aspx.22!)" 
                        OnClientClick="return WarnOptimize()" Width="155px" 
                            onclick="btn_optimize_Click"/>
                </td>
                <td style=" text-align:right; width:28%; vertical-align:bottom;">
                    <asp:HyperLink ID="hyperNoiseWord" runat="server" NavigateUrl="setupFTS_NoiseWords.aspx" 
                        Text="(!setupFTS.aspx.28!)" Width="100%" Visible="False"></asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
