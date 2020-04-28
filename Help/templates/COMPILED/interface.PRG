TEXTENDTEXT lcSeeAlsoTopics = oHelp.InsertSeeAlsoTopics() TEXT	
<html>
<head>
<topictype value="<< TRIM(oHelp.oTopic.Type) >>"/>
<title><< TRIM(oHelp.oTopic.Topic) >></title>
<LINK rel="stylesheet" type="text/css" href="templates/wwhelp.css">
<SCRIPT SRC="templates/wwhelp.js"></SCRIPT>
</head>
<body leftmargin=0 topmargin=0>

<table border=0 width="100%" bgcolor="lightblue" class="banner" cellspacing="3">
<tr><td valign="center">
ENDTEXT if !EMPTY(oHelp.oTopic.SeeAlso) TEXT
	<span id="more" style="cursor:hand;color:darkblue" onclick="SeeAlsoButton();">
	    <img src="bmp/seealso.gif" border=0  alt="Related Topics" style="Font:normal bold 6pt Symbol;cursor:Hand"></span>
	</span>
	<div id="SeeAlsoTopics" class="hotlinkbox" style="display:none" onmouseleave="SeeAlsoButton();">
	<b>See also</b><br>
	<span style="margin-left:10px;margin-top:15px"><< lcSeeAlsoTopics >></span>
	</div>
ENDTEXT endif TEXT
<span style="font:8pt Verdana;font-style:italic"><< oHelp.cProjectname >></span>
</td></tr><tr><td>
<span style="font:10pt Verdana;font-weight:bold">
<img src="bmp/<< TRIM(oHelp.oTopic.Type)>>.gif"> << oHelp.FormatHTML( TRIM(oHelp.oTopic.Topic) ) >>
</span>
</td></tr>
</table>

<div class="contentpane">
<br>
<< oHelp.FormatHTML(oHelp.oTopic.Body) >>
<br><br>
<small>
<< oHelp.InsertInheritanceTree() >>
</small>

<br>
<table border="0" cellpadding="3" width="96%">
<pre class="SyntaxBox">
<< oHelp.FormatHTML(oHelp.oTopic.Syntax) >>
</pre>
</table>

ENDTEXT IF !EMPTY(oHelp.oTopic.Remarks) TEXT
<h3 class="outdent">Remarks</h3>
<< oHelp.FormatHTML(oHelp.oTopic.Remarks)>>
ENDTEXT ENDIF TEXT  

<table border="0" cellpadding="3" width="95%">
ENDTEXT IF !EMPTY(oHelp.oTopic.Example) TEXT
  <pre><< oHelp.FormatHTML(oHelp.oTopic.Example)>></pre>
ENDTEXT ENDIF TEXT  
</table>

<h3 class="outdent">Interface Properties, Events and Methods</h3>
<br>
<< ClassMemberTableHtml(oHelp,.t.) >>


<h3 class="outdent">Requirements</h3>
   ENDTEXT IF !EMPTY(oHelp.oTopic.Namespace) TEXT
<b>Namespace:</b> << oHelp.oTopic.Namespace >><br>
   ENDTEXT endif TEXT
   ENDTEXT IF !EMPTY(oHelp.oTopic.Assembly) TEXT
<b>Assembly:</b> << oHelp.oTopic.Assembly >>
<br>
   ENDTEXT endif TEXT

ENDTEXT if !EMPTY(oHelp.oTopic.SeeAlso) TEXT
<h3 class="outdent">See also</h3>
<< lcSeeAlsoTopics >>
ENDTEXT  endif TEXT
<br>
</div>
<hr>
<small>Last Updated: <i><< TTOD(oHelp.oTopic.Updated) >></i></small>
</body>
</html>ENDTEXT