var _q3F=false;var _q3E;var _q37;var _q30;var _q32;function art_InitResizing(_1,_2,_3,_4){var _5=art_GetInstance(_1);if(cart_browser_ie){_5.Frame.onmousemove=_q3A;}_5.Frame.onmousedown=_q38;_5.Frame.onmouseout=_q3B;_5.MinWidth=_2;_5.MinHeight=_3;_5.ResizingMode=_4;}function _q20(_6,_7,_8,_9,_a){var _b="";if(_8<=_6){_b="n";}else{if(_8>=_a-_6){_b="s";}}if(_7<=_6){_b+="w";}else{if(_7>=_9-_6){_b+="e";}}return _b;}function _q51(){if(_q37!=""){document.body.style.cursor=_q37+"-resize";}else{document.body.style.cursor="default";}}function _q38(e){if(cart_browser_ie){event.cancelBubble=true;}else{e.preventDefault();e.stopPropagation();}if(_q3E==null||_q37==""){return true;}_q3F=true;_q3E.Frame.onmousemove=null;_q30=document.body.onmousemove;_q32=document.body.onmouseup;document.body.onmousemove=_q39;document.body.onmouseup=_q3D;_q1(_q3E);return false;}function _q39(){if(cart_browser_ie){var _d=_q34(event.srcElement);var _e=_q35(event.srcElement);var _f=event.offsetX+_d;var _10=event.offsetY+_e;}else{var _f=e.pageX;var _10=e.pageY;}var _11=_q3E.Frame;var _12=_q34(_11);var _13=_q35(_11);var _14=_11.offsetWidth;var _15=_11.offsetHeight;var _16=_q3E.MinHeight;var _17=_q3E.MinWidth;var _18=_12;var _19=_13;var _1a=_15;var _1b=_14;if(_q37.indexOf("n")>=0){_19=_10;_1a=_13-_10+_15;}else{if(_q37.indexOf("s")>=0){_1a=_10-_13;}}if(_q37.indexOf("e")>=0){_1b=_f-_12;}else{if(_q37.indexOf("w")>=0){_18=_f;_1b=_12-_f+_14;}}_1a=Math.max(_16,_1a);_1b=Math.max(_17,_1b);var _1c=_11.offsetHeight-_1a;var _1d=_q3E.InnerFrame.offsetHeight;_q3E.InnerFrame.style.height=_1d-_1c;_11.style.left=_18+"px";_11.style.top=_19+"px";_11.style.height=_1a+"px";_11.style.width=_1b+"px";_q40(_q3E,_18,_19,_1b,_1a);return false;}function _q3D(e){if(cart_browser_ie){document.body.onmousemove=_q30;document.body.onmouseup=_q32;_q3E.Frame.onmousemove=_q3A;}else{document.onmousemove=_q30;document.onmouseup=_q32;_q3E.Frame.onmouseover=_q3C;}_q3E.Frame.style.height=_q3E.Frame.offsetHeight+"px";_q3E.InnerFrame.style.height=_q3E.InnerFrame.offsetHeight+"px";_q3E.OriginalWidth=_q3E.Frame.offsetWidth;_q3E.OriginalHeight=_q3E.Frame.offsetHeight;_q52(_q3E);_q3F=false;_q37="";_q51();if(_q3E.EnableFloating){art_RepositionFloater(_q3E);}return false;}function _q3C(e){var _20=_q19(e.target);var _21=_q1E(_20);var _22=_q1C(_20);if(!_21||_q3F||_qC||_22.IsDocked||_22.CollapseExpandState==0){return true;}_q37="";_q3E=_22;var _23=e.layerX;var _24=e.layerY;var _25=_20.offsetWidth;var _26=_20.offsetHeight;var _27=_22.ResizeThreshold;_q37=_q20(_27,_23,_24,_25,_26);var _28=_22.ResizingMode;if(_28=="FreeStyle"||(_28=="Corners"&&_q37.length==2)||(_28=="Vertical"&&(_q37=="n"||_q37=="s"))||(_28=="Horizontal"&&(_q37=="w"||_q37=="e"))){}else{_q37="";}_q51();}function _q3A(){var _29=_q19(event.srcElement);var _2a=null;if(_29){_2a=_q1C(_29);}if(!_2a||_q3F||_qC||_2a.IsDocked||_2a.CollapseExpandState==0){return true;}_q37="";_q3E=_2a;var _2b=_q1A(event,_29);var _2c=_q1B(event,_29);var _2d=_29.offsetWidth;var _2e=_29.offsetHeight;var _2f=_2a.ResizeThreshold;_q37=_q20(_2f,_2b,_2c,_2d,_2e);var _30=_2a.ResizingMode;if(_30=="FreeStyle"||(_30=="Corners"&&_q37.length==2)||(_30=="Vertical"&&(_q37=="n"||_q37=="s"))||(_30=="Horizontal"&&(_q37=="w"||_q37=="e"))){}else{_q37="";}_q51();}function _q3B(e){if(!_q3F&&_q3E!=null){_q37="";_q51();}}var ComponentArt_Snap_Resize_Loaded=true;
