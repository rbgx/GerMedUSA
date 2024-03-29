<%@ Control Language="c#" AutoEventWireup="false" Inherits="AspDotNetStorefront.TemplateBase" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>(!METATITLE!) - GerMedUSA Inc</title>
(!CURRENCY_LOCALE_ROBOTS_TAG!)
<meta name="description" content="(!METADESCRIPTION!)">
<meta name="keywords" content="(!METAKEYWORDS!)">
<link rel="stylesheet" href="skins/Skin_(!SKINID!)/style.css" type="text/css">
<script type="text/javascript" src="jscripts/formValidate.js"></script>
(!BUYSAFEJSURL!)
</head>
<body>
(!XmlPackage Name="skin.adminalert.xml.config"!)
(!PAGEINFO!)
    <asp:Panel ID="pnlForm" runat="server" Visible="false"  />

    <div id="wrapper">
        <div id="login">
            <span id="userName">(!USERNAME!)</span><span id="loginText"><a href="(!SIGNINOUT_LINK!)">(!SIGNINOUT_TEXT!)</a></span>
        </div>
        <div id="header">
            <a id="logo" href="default.aspx" title="YourCompany.com"><b>YourCompany.com</b></a>            
            <a class="cart" href="shoppingcart.aspx">Shopping Cart ((!NUM_CART_ITEMS!))</a> 
            <a class="contact" href="t-contact.aspx">Contact Us</a> <a class="account" href="account.aspx">Your Account</a>
        </div>
        <div id="horizNav">
            <!-- TOP MENU -->
            <asp:Panel ID="PageMenu_Panel" runat="server" >                  
                    <ComponentArt:Menu id="PageMenu" 
                      ClientScriptLocation="skins/componentart_webui_client/"
                      ScrollingEnabled="true"
                      ScrollUpLookId="ScrollUpItemLook"
                      ScrollDownLookId="ScrollDownItemLook"
                      Orientation="horizontal"
                      CssClass="TopMenuGroup"
                      DefaultGroupCssClass="MenuGroup"
                      DefaultItemLookID="DefaultItemLook"
                      DefaultGroupItemSpacing="1"
                      ExpandDelay="0"
                      ExpandDuration="0"
                      ExpandSlide="None"
                      ExpandTransition="None"
                      CascadeCollapse="false"
                      CollapseDelay="0"
                      CollapseSlide="None"
                      CollapseTransition="None"                      
                      ImagesBaseUrl="skins/skin_1/images/"
                      EnableViewState="false"
                      runat="server">
                    <ItemLooks>
	                      <ComponentArt:ItemLook LookId="TopItemLook" CssClass="TopMenuItem" HoverCssClass="TopMenuItemHover" LabelPaddingLeft="4" LabelPaddingRight="4" LabelPaddingTop="4" LabelPaddingBottom="5" />
	                      <ComponentArt:ItemLook LookId="DefaultItemLook" CssClass="MenuItem" HoverCssClass="MenuItemHover" LabelPaddingLeft="4" LabelPaddingRight="4" LabelPaddingTop="2" LabelPaddingBottom="2" />
	                      <ComponentArt:ItemLook LookID="ScrollUpItemLook" ImageUrl="scroll_up.gif" ImageWidth="15" ImageHeight="13" CssClass="ScrollItem" HoverCssClass="ScrollItemH" ActiveCssClass="ScrollItemA" />
                          <ComponentArt:ItemLook LookID="ScrollDownItemLook" ImageUrl="scroll_down.gif" ImageWidth="15" ImageHeight="13" CssClass="ScrollItem" HoverCssClass="ScrollItemH" ActiveCssClass="ScrollItemA" />
                    </ItemLooks>
                    </ComponentArt:Menu>                  
                  </asp:Panel>
            <!-- END TOP MENU -->
        </div>
        <div id="horizNav2">
            <a href="#"><img src="skins/skin_(!SKINID!)/images/live-chat.gif" alt="Click Here to Chat With a Representative" class="liveHelp" /></a>
            <form name="topsearchform" method="get" action="search.aspx">
                <fieldset>
                    <label>Search:</label>
                    <input type="text" size="15" name="SearchTerm" class="searchBox" id="searchBox" autocomplete="off" onFocus="javascript:this.style.background='#ffffff';" onBlur="javascript:this.style.background='#dddddd';" />
                    <input type="button" onClick="document.topsearchform.submit()" title="Click Go to Submit" id="Go" class="submit" value="Go" /><br />
                </fieldset>
            </form>
            <ul class="tameHoriz">
                <li><a href="account.aspx">Track Your Order</a><span class="pipe">|</span></li>
                <li><a href="t-returns.aspx">Returns</a><span class="pipe">|</span></li>
                <li><a href="t-shipping.aspx">Shipping Policy</a><span class="pipe">|</span></li>
                <li><a href="t-faq.aspx">FAQ</a><span class="pipe">|</span></li>
                <li><span>1-800-555-1234</span></li>
            </ul>
        </div>
        <div id="bodyWrapper">
            <!-- <div id="miniCart">You have (!NUM_CART_ITEMS!) item(s) in your <a class="username" href="shoppingcart.aspx">
                    (!CARTPROMPT!)</a></div> -->
            <div id="ML">
                <div style="visibility: (!COUNTRYDIVVISIBILITY!); display: (!COUNTRYDIVDISPLAY!);">Language: (!COUNTRYSELECTLIST!)</div>
                <div style="visibility: (!CURRENCYDIVVISIBILITY!); display: (!CURRENCYDIVDISPLAY!);">Currency: (!CURRENCYSELECTLIST!)</div>
                <div style="visibility: (!VATDIVVISIBILITY!); display: (!VATDIVDISPLAY!);">VAT Mode: (!VATSELECTLIST!)</div>
            </div>
            <div id="breadcrumb">Now In: (!SECTION_TITLE!)</div>
            <div id="leftWrap">
                <div class="navHeader">Browse (!StringResource Name="AppConfig.ManufacturerPromptPlural"!)</div>
                <div class="leftNav" id="manufacturers">(!XmlPackage Name="rev.manufacturers"!)</div>
                <div class="navHeader">Browse (!StringResource Name="AppConfig.CategoryPromptPlural"!)</div>
                <div class="leftNav" id="categories">(!XmlPackage Name="rev.categories"!)</div>
                <div class="navHeader">Browse (!StringResource Name="AppConfig.SectionPromptPlural"!)</div>
                <div class="leftNav" id="departments">(!XmlPackage Name="rev.departments"!)</div>
                <div class="navHeader">Help &amp; Info</div>
                <div class="leftNav" id="helpbox">(!Topic Name="helpbox"!)</div>
            </div>
            <div id="content">
                <!-- CONTENTS START -->
                <asp:PlaceHolder ID="PageContent" runat="server"></asp:PlaceHolder>
                <!-- CONTENTS END -->
            </div>
        </div>
        <div id="footer">
            <div id="footerWrap">
                <ul class="tameHoriz">
                    <li><a href="t-about.aspx">About YourCompany.com</a> |</li>
                    <li><a href="t-returns.aspx">Returns</a> |</li>
                    <li><a href="t-faq.aspx">FAQ</a> |</li>
                    <li><a href="t-contact.aspx">Contact Us</a></li>
                </ul>
                <ul class="tameHoriz">
                    <li><a href="sitemap2.aspx">Site Map</a> |</li>
                    <li><a href="t-privacy.aspx">Privacy Policy</a> |</li>
                    <li><a href="t-security.aspx">Security</a></li>
                </ul>
                <br />
                <ul class="tame">
                    <li>&copy; YourCompany.com 2009. All Rights Reserved.</li>
                </ul>
            </div>
        </div>
        
</div>
    </div> <!-- wrapper -->

    <table border="0" width="100%"><tr><td align="center" width="100%">(!BUYSAFESEAL!)</td></tr></table>
    <noscript>E-Commerce Solutions</noscript>
</body>
</html>
