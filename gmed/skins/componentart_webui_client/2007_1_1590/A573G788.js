function ComponentArt_Ticker(){this.GlobalID="";this.ElementID="";this.CharDelay=0;this.LineDelay=0;this.Lines=new Array();this.NextTickerDelay=0;this.TickerType="default";this.IntervalID=0;this.CurLine=0;this.CurChar=0;this.OnEnd=function(){_q192();};}ComponentArt_Ticker.prototype.GetProperty=function(_1){return this[_1];};ComponentArt_Ticker.prototype.SetProperty=function(_2,_3){this[_2]=_3;};function _q195(_4){if(_4.TickerType=="statusbar"){return window.status;}else{return _q191(document.getElementById(_4.ElementID).innerHTML);}}function _q19B(_5,_6){if(_5.TickerType=="statusbar"){window.status=_6;}else{document.getElementById(_5.ElementID).innerHTML=_q193(_6);}}function _q193(_7){if(navigator.userAgent.indexOf("MSIE")!=-1||navigator.userAgent.indexOf("Opera")!=-1){return _7.replace("&","&amp;");}else{return _7.replace("&"," ");}}function _q191(_8){if(navigator.userAgent.indexOf("MSIE")!=-1||navigator.userAgent.indexOf("Opera")!=-1){return _8.replace("&amp;","&");}else{return _8;}}window.rcr_StartTicker=function(_9){_q19B(_9,"");var _a="rcr_PrintNextChar("+_9.GlobalID+")";_9.IntervalID=window.setInterval(_a,_9.CharDelay);};function _q19D(_b){window.clearInterval(_b.IntervalID);}function rcr_PrintNextChar(_c){if(_q195(_c).length==_c.Lines[_c.CurLine].length){_q196(_c);}else{_q19B(_c,_q195(_c)+_c.Lines[_c.CurLine].charAt(_c.CurChar));_c.CurChar++;}}function _q196(_d){_d.CurChar=0;window.clearInterval(_d.IntervalID);if(_d.CurLine==_d.Lines.length-1){if(_d.Loop){_d.CurLine=0;}else{_q19D(_d);var _e="rcr_OnEnd("+_d.GlobalID+")";var _f=window.setTimeout(_e,_d.NextTickerDelay);return null;}}else{_d.CurLine++;}var _e="rcr_StartTicker("+_d.GlobalID+")";var _10=window.setTimeout(_e,_d.LineDelay);}function rcr_OnEnd(_11){_11.OnEnd();}window._q192=function(){};