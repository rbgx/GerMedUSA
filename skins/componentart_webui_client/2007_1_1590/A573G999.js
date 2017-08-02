function ComponentArt_Dialog(id,_2,_3,_4){Dialogs.push(id);this.DomElementId=id;if(!document.getElementById(this.DomElementId)){if(!_2){_2=100;}if(!_3){_3=200;}var _5=document.createElement("div");_5.setAttribute("id",this.DomElementId);_5.style.position="absolute";_5.style.visibility="hidden";_5.style.height=_2+"px";_5.style.width=_3+"px";var _6="";if(this.FocusOnClick){_6="onclick='"+id+".Focus();' ";}if(cart_browser_ie&&_4.RenderOverWindowedObjects){_5.innerHTML="<iframe id='"+this.DomElementId+"_OverlayIFrame' style='position:absolute;top:0;left:0;width:100%;height:100%;display:block;z-index:-1;filter:progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0);'></iframe><div id='"+id+"_HeaderSpan'></div><div "+_6+"id='"+id+"_InnerSpan'></div><div id='"+id+"_FooterSpan'></div>";}else{_5.innerHTML="<div id='"+id+"_HeaderSpan'></div><div "+_6+"id='"+id+"_InnerSpan'></div><div id='"+id+"_FooterSpan'></div>";}var _7=document.createElement("div");_7.setAttribute("id",this.DomElementId+"_PlaceHolder");document.forms[0].appendChild(_7);document.forms[0].appendChild(_5);var _8=new Array();if(_4){for(i in _4.PublicProperties){if(_4.PublicProperties[i][0]!="Id"&&_4.PublicProperties[i][0]!="IsShowing"){var _9=_4.PublicProperties[i][0];var _a=_4[_9];var _b=[_9,_a];_8.push(_b);}}}else{_8=[["Alignment","MiddleCentre"],["OffsetX",0],["OffsetY",0],["X",null],["Y",null]];}this.ControlId=id;ComponentArt_SetProperties(this,_8);_q121(this);this.DomElement=document.getElementById(id);this.DomElement.onmousemove=new Function("e","eval(\""+id+".getMouseXY(e);\");");this.DomElement.onmousedown=new Function("e","eval(\""+id+".HandleMouseDown(e);\");");this.DomElement.onmouseout=new Function("e","eval(\""+id+".HandleMouseOut(e);\");");this.DomElement.onmouseup=new Function("e","eval(\""+id+".HandleMouseUp(e);\");");}this.DomElement=document.getElementById(this.DomElementId);this.element=document.getElementById(this.DomElementId+"_PlaceHolder");if(document.forms[0]){this.DomElement.parentNode.removeChild(this.DomElement);document.forms[0].insertBefore(this.DomElement,document.forms[0].firstChild);}else{setTimeout("art_AddtoForm(\""+this.DomElementId+"\")",250);}zTop++;this.DomElement.style.zIndex=zTop;if(window.ComponentArt_Atlas){ComponentArt.Web.UI.Dialog.initializeBase(this,[this.element]);this.beginUpdate=function(){this._updating=true;};this.endUpdate=function(){this._updating=false;this.Render();};this.get_isUpdating=function(){return this._updating;};this.getDescriptor=function(){return _qE4(this.constructor);};}else{this.beginUpdate=function(){this._updating=true;};this.endUpdate=function(){this._updating=false;this.Render();};this.get_isUpdating=function(){return this._updating;};}this.ClientControlId=this.Id=id;}ComponentArt_Dialog.prototype.Dispose=function(){ComponentArt_Dispose(this);var _c=document.getElementById(this.Id+"_HeaderSpan");var _d=document.getElementById(this.Id+"_InnerSpan");var _e=document.getElementById(this.Id+"_FooterSpan");_qE7(_c);_qE7(_d);_qE7(_e);if(document.getElementById(this.Id+"_ModalMask")){_qE7(document.getElementById(this.Id+"_ModalMask"));}if(document.getElementById(this.Id+"_ModalTable")){_qE7(document.getElementById(this.Id+"_ModalTable"));}if(document.getElementById(this.Id+"_OverlayIFrame")){_qE7(document.getElementById(this.Id+"_OverlayIFrame"));}if(document.getElementById(this.Id+"_IFrame")){_qE7(document.getElementById(this.Id+"_IFrame"));}};ComponentArt_Dialog.prototype.Initialize=function(){_q121(this);this.DomElement.onmousemove=new Function("e",this.Id+".getMouseXY(e);");this.DomElement.onmousedown=new Function("e",this.Id+".HandleMouseDown(e);");this.DomElement.onmouseout=new Function("e",this.Id+".HandleMouseOut(e);");this.DomElement.onmouseup=new Function("e",this.Id+".HandleMouseUp(e);");this.Render();};ComponentArt_Dialog.prototype.PublicProperties=[["Enabled",Boolean],["IsShowing",Boolean,1],["Modal",Boolean],["ModalMaskImage",String],["ResizeArea",Boolean],["FocusOnClick",Boolean,1],["Id",String,1],["Alignment",String],["MinimumHeight",Number],["MinimumWidth",Number],["Height",String],["Width",String],["OffsetX",Number],["OffsetY",Number],["X",Number],["Y",Number],["Content",String],["ContentUrl",String],["Title",String],["Icon",String],["Value",String],["Result",Object],["CloseTransition",Number],["ShowTransition",Number],["AlignmentElement",String],["AnimationType",String],["AnimationPath",String],["AnimationDirectionElement",String],["AnimationDuration",Number],["TransitionDuration",Number],["AnimationSlide",Number],["ClientTemplates",Array],["ClientEvents",Array],["AllowDrag",Boolean],["AllowResize",Boolean],["RenderOverWindowedObjects",Boolean],["HeaderClientTemplateId",String],["ContentClientTemplateId",String],["FooterClientTemplateId",String],["ModalMaskCssClass",String],["ModalScrollbarOffset",Number],["HeaderCssClass",String],["FooterCssClass",String],["ContentCssClass",String],["IFrameCssClass",String],["OutlineCssClass",String],["CssClass",String]];window.ComponentArt.Web.UI.Dialog=window.ComponentArt_Dialog;ComponentArt_Dialog.prototype.PublicMethods=[["Dispose"],["StartDrag"],["Focus"],["Show"],["Close"]];ComponentArt_Dialog.prototype.PublicEvents=[["OnShow"],["OnClose"],["OnFocus"],["OnDrag"],["OnDrop"]];_qE3(ComponentArt_Dialog,"this");window.ComponentArt.Web.UI.Dialog=window.ComponentArt_Dialog;if(window.ComponentArt_Atlas){ComponentArt.Web.UI.Dialog.registerClass("ComponentArt.Web.UI.Dialog",Sys.UI.Control);if(Sys.TypeDescriptor){Sys.TypeDescriptor.addType("componentArtWebUI","dialog",ComponentArt.Web.UI.Dialog);}}ComponentArt_Dialog.prototype.GetProperty=function(_f){return this[_f];};ComponentArt_Dialog.prototype.SetProperty=function(_10,_11){this[_10]=_11;};ComponentArt_Dialog.prototype.Focus=function(){if(Dialogs.length>1){var i=0;for(j=0;j<Dialogs.length;j++){if(eval(Dialogs[j]+".Modal")&&eval(Dialogs[j]+".IsShowing")){i++;}if(i>1){return;}}zTop++;this.DomElement.style.zIndex=zTop;}var _13=this.get_events().getHandler("onFocus");if(_13){_13(this,Sys.EventArgs.Empty);}};ComponentArt_Dialog.prototype.Show=function(_14,_15,_16,x,y){if(!this.IsShowing){if(this.AnimationType=="Live"&&cart_browser_ie){this.DomElement.style.zoom="1%";}this.Result=null;if(_14){this.Content=_14;}if(_15){this.Title=_15;}if(_16){this.Icon=_16;}if(x){this.X=x;}if(y){this.Y=y;}if(this.Modal){art_ShowModal(this.Id,this.ModalMaskCssClass);}this.DomElement.style.visibility="visible";this.Render();if(Dialogs.length>1){for(i=0;i<Dialogs.length;i++){var _19=document.getElementById(Dialogs[i]);var _1a=document.getElementById(this.Id);if(Math.abs(parseInt(_19.style.top)-parseInt(_1a.style.top))<10&&Math.abs(parseInt(_19.style.left)-parseInt(_1a.style.left))<10&&Dialogs[i]!=this.Id&&_19.style.visibility=="visible"){this.set_offsetX(this.OffsetX+15);this.set_offsetY(this.OffsetY+15);}}}var _1b=false;if(this.AnimationType!="None"){this.ShowAnimate(0,document.getElementById(this.AnimationDirectionElement),this.DomElement);_1b=true;}if((this.ShowTransition>0)&&cart_browser_transitions){this.DomElement.style.visibility="hidden";var _1c=this.DomElement;_1c.filters.clear();_1c.runtimeStyle.filter=null;var _1d=ComponentArt_EffectiveTransitionString(this.ShowTransition-0);_1c.ExpandTransitionFilterIndex=_1c.filters.length;_1c.ExpandTransitionFilterDefined=true;_1c.runtimeStyle.filter=_1c.currentStyle.filter+" "+_1d;_1c.filters[_1c.ExpandTransitionFilterIndex].apply();_1c.style.visibility="visible";_1c.filters[_1c.ExpandTransitionFilterIndex].play(this.TransitionDuration/1000);}this.Focus();if(!_1b){this.IsShowing=true;var _1e=this.get_events().getHandler("onShow");if(_1e){_1e(this,Sys.EventArgs.Empty);}}}};ComponentArt_Dialog.prototype.Close=function(_1f){if(this.IsShowing){if(!(_1f===void 0)){this.SetProperty("Result",_1f);}if(this.AnimationType!="None"){this.CloseAnimate();var _20=true;}if((this.CloseTransition>0)&&cart_browser_transitions){var _21=this.DomElement;_21.filters.clear();_21.runtimeStyle.filter=null;var _22=ComponentArt_EffectiveTransitionString(this.CloseTransition-0);_21.CollapseTransitionFilterIndex=_21.filters.length;_21.CollapseTransitionFilterDefined=true;_21.runtimeStyle.filter=_21.currentStyle.filter+" "+_22;_21.filters[_21.CollapseTransitionFilterIndex].apply();this.DomElement.style.visibility="hidden";_21.filters[_21.CollapseTransitionFilterIndex].play(this.TransitionDuration/1000);}else{if(!_20){this.DomElement.style.visibility="hidden";}}if(!_20){if(this.Modal){art_CloseModal(this.Id);}this.IsShowing=false;var _23=this.get_events().getHandler("onClose");if(_23){_23(this,Sys.EventArgs.Empty);}}}};ComponentArt_Dialog.prototype.Render=function(){if(this.AnimationType!="Live"){this.DomElement.style.zoom="100%";}if(this.Height){this.DomElement.style.height=this.Height;}if(this.Width){this.DomElement.style.width=this.Width;}art_PositionDialog(this.Id,this.Alignment,this.X,this.Y,this.OffsetX,this.OffsetY,this.AlignmentElement);if(this.ContentUrl&&document.getElementById(this.Id+"_IFrame")){document.getElementById(this.Id+"_IFrame").src=ComponentArt_ConvertUrl(null,this.ContentUrl,this.ApplicationPath);}if(this.HeaderClientTemplateId){var _24=this.GetClientTemplate(this.HeaderClientTemplateId);document.getElementById(this.Id+"_HeaderSpan").innerHTML=ComponentArt_InstantiateClientTemplate(_24,this);}if(this.ContentClientTemplateId){var _24=this.GetClientTemplate(this.ContentClientTemplateId);document.getElementById(this.Id+"_InnerSpan").innerHTML=ComponentArt_InstantiateClientTemplate(_24,this);}else{if(this.Content){document.getElementById(this.Id+"_InnerSpan").innerHTML=this.Content;}}if(this.FooterClientTemplateId){var _24=this.GetClientTemplate(this.FooterClientTemplateId);document.getElementById(this.Id+"_FooterSpan").innerHTML=ComponentArt_InstantiateClientTemplate(_24,this);}if(this.CssClass){this.DomElement.className=this.CssClass;}if(this.HeaderCssClass){document.getElementById(this.Id+"_HeaderSpan").className=this.HeaderCssClass;}if(this.ContentCssClass){document.getElementById(this.Id+"_InnerSpan").className=this.ContentCssClass;}if(this.FooterCssClass){document.getElementById(this.Id+"_FooterSpan").className=this.FooterCssClass;}if(this.RenderOverWindowedObjects&&cart_browser_ie){var _25=document.getElementById(this.Id+"_OverlayIFrame");_25.style.width=this.DomElement.offsetWidth+"px";_25.style.height=this.DomElement.offsetHeight+"px";}};var _q2F;var _q31;var art_oldResize;var art_dialogDragging;var art_dialogResizing;var art_mouseX;var art_mouseY;var art_offsetX;var art_offsetY;art_returnFalse=function(e){return false;};art_returnTrue=function(e){return true;};art_resizeUpdate=function(){var _28=document.getElementById(art_dialogResizing.Id+"_InnerSpan");var _29=document.getElementById(art_dialogResizing.Id+"_HeaderSpan");var _2a=document.getElementById(art_dialogResizing.Id+"_FooterSpan");var _2b=0;if(_29){_2b=_29.offsetHeight;}if(_2a){_2b+=_2a.offsetHeight;}var x=_q85(art_dialogResizing.DomElement);var y=_q86(art_dialogResizing.DomElement);if(art_mouseX-x>0){_28.style.width=((art_mouseX-x)+1)+"px";art_dialogResizing.DomElement.style.width=((art_mouseX-x)+1)+"px";art_dialogResizing.SetProperty("Width",art_mouseX-x+1);if(art_mouseX-x<art_dialogResizing.MinimumWidth){_28.style.width=(art_dialogResizing.MinimumWidth)+"px";art_dialogResizing.SetProperty("Width",art_dialogResizing.MinimumWidth);art_dialogResizing.DomElement.style.width=(art_dialogResizing.MinimumWidth)+"px";}}if(art_mouseY-y-_2b>0){_28.style.height=((art_mouseY-y)-_2b+1)+"px";art_dialogResizing.DomElement.style.height=(art_mouseY-y+1)+"px";art_dialogResizing.SetProperty("Height",art_mouseY-y+1);if(art_mouseY-y<art_dialogResizing.MinimumHeight){_28.style.height=(art_dialogResizing.MinimumHeight-_2b)+"px";art_dialogResizing.DomElement.style.height=(art_dialogResizing.MinimumHeight)+"px";art_dialogResizing.SetProperty("Height",art_dialogResizing.MinimumHeight);}}};art_GetMouseXY=function(e){if(!e){e=window.event;}art_mouseX=cart_browser_ie?e.clientX+(document.documentElement&&document.documentElement.scrollLeft?document.documentElement.scrollLeft:document.body.scrollLeft):e.pageX;art_mouseY=cart_browser_ie?e.clientY+(document.documentElement&&document.documentElement.scrollTop?document.documentElement.scrollTop:document.body.scrollTop):e.pageY;if(art_dialogResizing){art_resizeUpdate();ComponentArt_CancelEvent(e);}};ComponentArt_Dialog.prototype.HandleMouseUp=function(e){art_dialogDragging=false;art_dialogResizing=null;if(_q31){document.onmouseup=_q31;}document.onmousemove=null;document.onselectstart=null;if(!this.ResizeArea){this.DomElement.style.cursor="";}};ComponentArt_Dialog.prototype.HandleMouseDown=function(e){if(this.ResizeArea&&this.AllowResize){art_GetMouseXY(e);art_dialogResizing=this;document.onmousemove=art_GetMouseXY;}return true;};ComponentArt_Dialog.prototype.getMouseXY=function(e){if(!e){e=window.event;}art_mouseX=cart_browser_ie?e.clientX+(document.documentElement&&document.documentElement.scrollLeft?document.documentElement.scrollLeft:document.body.scrollLeft):e.pageX;art_mouseY=cart_browser_ie?e.clientY+(document.documentElement&&document.documentElement.scrollTop?document.documentElement.scrollTop:document.body.scrollTop):e.pageY;if(this.AllowResize){if(art_dialogResizing){art_resizeUpdate();}if(Math.abs(art_mouseX-(parseInt(this.DomElement.style.left)+this.DomElement.offsetWidth))<10&&Math.abs(art_mouseY-(parseInt(this.DomElement.style.top)+this.DomElement.offsetHeight))<10){this.ResizeArea=true;this.DomElement.style.cursor="se-resize";}else{this.ResizeArea=false;this.DomElement.style.cursor="";}}};ComponentArt_Dialog.prototype.HandleMouseOut=function(e){if(!art_dialogResizing){this.ResizeArea=false;this.DomElement.style.cursor="";}};ComponentArt_Dialog.prototype.StartDrag=function(e){this.Focus();if(this.AllowDrag){art_GetMouseXY(e);art_dialogDragging=true;document.onselectstart=art_returnFalse;_q2F=document.onmousemove;_q31=document.onmouseup;if(this.Modal){maskTable=document.getElementById(this.Id+"_ModalTable");maskTable.onmousemove=art_GetMouseXY;this.DomElement.onmousemove=art_GetMouseXY;}else{document.onmousemove=art_GetMouseXY;}art_offsetX=art_mouseX-_q85(this.DomElement);art_offsetY=art_mouseY-_q86(this.DomElement);art_DialogDragStep(this.Id);var _34=this.get_events().getHandler("onDrag");if(_34){_34(this,Sys.EventArgs.Empty);}ComponentArt_CancelEvent(e);}};art_DialogDragStep=function(id){if(art_dialogDragging){document.getElementById(id).style.left=(art_mouseX-art_offsetX)+"px";document.getElementById(id).style.top=(art_mouseY-art_offsetY)+"px";setTimeout("art_DialogDragStep(\""+id+"\")",50);}else{eval(id).SetProperty("X",_q85(document.getElementById(id)));eval(id).SetProperty("Y",_q86(document.getElementById(id)));var _36=eval(id).get_events().getHandler("onDrop");if(_36){_36(eval(id),Sys.EventArgs.Empty);}}};ComponentArt_Dialog.prototype.GetClientTemplate=function(sID){if(this.ClientTemplates){for(var i=0;i<this.ClientTemplates.length;i++){if(this.ClientTemplates[i][0]==sID){return this.ClientTemplates[i][1];}}}return null;};var ComponentArt_DialogToMinimize=null;var ComponentArt_AnimationType=null;var ComponentArt_AnimationPath=null;ComponentArt_Dialog.prototype.ShowAnimate=function(_39,_3a,_3b){var _3c=_q85(_3a);var _3d=_q86(_3a);var _3e=_3a.offsetWidth;var _3f=_3a.offsetHeight;var _40=_q85(_3b);var _41=_q86(_3b);var _42=_3b.offsetWidth;var _43=_3b.offsetHeight;if(!_39){_3b.style.visibility="hidden";}dialog_speed_x=0;dialog_speed_y=0;ComponentArt_DialogToMinimize=this;ComponentArt_AnimationPath=this.AnimationPath;if(this.AnimationType=="Live"&&cart_browser_ie){ComponentArt_DialogMinimizeObject=this.DomElement;var _44=0;if(!_39){_44=this.AnimationDuration-this.TransitionDuration;}ComponentArt_DialogMinimizeObject.MinimizeStartTime=(new Date()).getTime()-_44;ComponentArt_DialogMinimizeObject.style.top=_3d+"px";ComponentArt_DialogMinimizeObject.style.left=_3c+"px";ComponentArt_AnimationType="Live";}else{ComponentArt_AnimationType="Outline";}art_MinimizeAnimate(_39,this.OutlineCssClass,this.AnimationSlide,this.AnimationDuration,_3c,_3d,_3e,_3f,_40,_41,_42,_43,this.TransitionDuration);};ComponentArt_Dialog.prototype.CloseAnimate=function(){if(this.AnimationDirectionElement){var _45=document.getElementById(this.AnimationDirectionElement);if(_45){this.ShowAnimate(1,this.DomElement,_45);}}else{this.DomElement.style.visibility="hidden";}};if(!Dialogs){var Dialogs=new Array();}if(!zTop){var zTop=900;}var TabElements=new Array();var TabableElements=new Array("A","BUTTON","TEXTAREA","INPUT","IFRAME","SELECT");var ComponentArt_Dialog_Kernel_Loaded=true;
