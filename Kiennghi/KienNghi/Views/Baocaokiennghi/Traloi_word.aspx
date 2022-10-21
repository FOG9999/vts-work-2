<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html><head>
<meta http-equiv="Content-Language" content="en-us">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">

<META content="Root" name=Author >
<META Cache-Control: private>
<style>
@charset "utf-8";
/* CSS Document */
/* HTML5 display-role reset for older browsers */
body,p,strong,em,table,td {
	font-size: 12.5pt;
	font-family:"Times New Roman", Times, serif;
    vertical-align:middle;
}
h4{
    margin:0px 0px 10px 0px !important;
    padding:0px !important;
    text-transform:uppercase;
}
body{ width:880px; margin:0 auto}
p{ 
    margin:0px 0px 10px 0px !important;
    padding:0px !important;
    line-height:20px;
}

.tcenter{text-align: center !important;}
.tright{text-align: right !important;}

TABLE {page: rotated;vertical-align:top;}
@page narrow {
    size:9in 11in;
	margin: 1.4cm;	
}
@page rotated {size: landscape}
DIV {page: narrow}

</style>
</head>
<body>
<div>
    <%=ViewData["list"] %>
</div>
</body>
</html>
