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
	<span id="more" style="cursor:hand;" onclick="SeeAlsoButton();">
	    <img src="bmp/seealso.gif" border=0  alt="Related Topics" style="Font:cursor:Hand">
	</span>
	<div id="SeeAlsoTopics" class="hotlinkbox" style="display:none" onmouseleave="SeeAlsoButton();">
	<b>See also</b><br>
	<span style="margin-left:10px;margin-top:15px"><< lcSeeAlsoTopics >></span>
	</div>
ENDTEXT endif TEXT
<span class="projectname"><< oHelp.cProjectname >></span>
</td></tr><tr><td>
<span class="topicname">
<img src="bmp/<< TRIM(oHelp.oTopic.Type)>>.gif"> << oHelp.FormatHtml(TRIM(oHelp.oTopic.Topic)) >>
</span>
</td></tr>
</table>

<div style="margin-left:10pt;margin-right=10pt">
<br>
<< oHelp.FormatHTML(oHelp.oTopic.Body) >>
<p>

<table border="0" cellpadding="3" width="96%">
<pre class="SyntaxBox">
<< oHelp.FormatHtml(oHelp.oTopic.Syntax) >>
</pre>
</table>

ENDTEXT IF !EMPTY(oHelp.oTopic.Remarks) TEXT
<h3 class="outdent">Remarks</h3>
    << oHelp.FormatHTML(oHelp.oTopic.Remarks) >>
ENDTEXT ENDIF TEXT  

ENDTEXT IF !EMPTY(oHelp.oTopic.Example) TEXT
<h3 class="Outdent">Example</h3>
<pre><< oHelp.FormatHTML(oHelp.oTopic.Example)>></pre>
ENDTEXT ENDIF TEXT  


ENDTEXT if !EMPTY(oHelp.oTopic.SeeAlso) TEXT
<h3 class="outdent">See also:</h3>
<< lcSeeAlsoTopics >>
ENDTEXT  endif TEXT
<p>
</div>
<hr>
<small>Last Updated: <i><< TTOD(oHelp.oTopic.Updated) >></i></small>
</body>
</html>ENDTEXT