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
	font-size: 13pt;
	font-family:"Times New Roman", Times, serif;
    vertical-align:middle;
}
h4{
    margin-bottom:5px; font-size:15pt; margin-top:3px !important;
}
body{ width:880px; margin:0 auto}
p{ padding-bottom:5px !important;}
table thead tr td strong{text-transform: uppercase;}
.baocao{
   
    border-bottom:1px solid #333; 
    border-right:1px solid #333; 
    width:98%;
    
}
.baocao thead tr td{
    border-left:1px solid #333; 
    border-top:1px solid #333;
    background:#f4f4f4; font-weight:bold; padding:5px; text-align:center }
.baocao tr td{
    border-top:1px solid #333;
    border-left:1px solid #333; 
    padding:5px;
    font-size:13pt;
}
.tcenter{text-align: center !important;}
.tright{text-align: right !important;}
h4,p{
    font-size:12pt !important;
    padding:0px !important;    
    margin:0px 0px 5px 0px !important;
}

TABLE {
    page: rotated;vertical-align:top;
    
}
@page narrow {
    size:9in 11in;
	margin: 1.4cm;	
}
@page rotated {
    size: landscape;
	margin: 0.8cm;	
}
DIV {page: rotated}

</style>
</head>
<body>
<div>
    <%=ViewData["list"] %>
</div>
</body>
</html>
