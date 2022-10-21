<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
      <% KNTC_DON d = (KNTC_DON)ViewData["Thongtindonkntc"]; %>
    <meta http-equiv="Content-Language" content="en-us">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

    <meta content="Root" name="Author">
    <meta cache-control: private>
    <style>
        @charset "utf-8";
        /* CSS Document */
        /* HTML5 display-role reset for older browsers */
        html, body {
            font-size: 13pt;
            font-family: "Times New Roman", Times, serif;
            vertical-align: middle;
        }

        h3 {
            text-align: center;
        }

        /*table tr td {
            text-align: center;
        }*/

        .tcenter {
            text-align: center;
        }

        .tright {
            text-align: right;
        }

        .tleft {
            text-align: left;
        }

        td.b {
            font-weight: bold;
        }
    </style>
</head>
<body>
   <table border="0" cellpadding="0" cellspacing="0" width="656">
	<tbody>
		<tr>
			<td>&nbsp;</td>
			<td style="width:227px;height:84px;"><strong>BAN D&Acirc;N NGUYỆN</strong><br />
			&nbsp;<br />
			&nbsp;<br />
			<br clear="ALL" />
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /BDN-V.I</td>
			<td style="width: 419px; height: 84px; text-align: center;"><strong>&nbsp;CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM</strong><br />
			<strong>&nbsp; &nbsp; &nbsp;Độc lập - Tự do - Hạnh ph&uacute;c</strong><br />
			&nbsp;<br />
			<em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em>
		</tr>
		<tr>
			<td colspan="2" style="width:236px;height:73px;"><em>V/v trả đơn khiếu nại của c&ocirc;ng d&acirc;n do kh&ocirc;ng đủ điều kiện thụ l&yacute; giải quyết</em></td>
			<td style="width:419px;height:73px;">&nbsp;</td>
		</tr>
		<tr height="0">
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<div style="text-align: center;"><strong>PHIẾU TRẢ ĐƠN KHIẾU NẠI</strong><br />
&nbsp;<br />
&nbsp;<br />
<strong>K&iacute;nh gửi: &ocirc;ng <% = d.CNGUOIGUI_TEN %></strong><br />
&nbsp;</div>
&nbsp;<br />
Ng&agrave;y <% = Convert.ToDateTime(d.DNGAYNHAN).Day %> th&aacute;ng <% = Convert.ToDateTime(d.DNGAYNHAN).Month %> năm <% = Convert.ToDateTime(d.DNGAYNHAN).Year %>, Ban D&acirc;n Nguyện nhận được đơn khiếu nại của &ocirc;ng  <% = d.CNGUOIGUI_TEN %>.<br />
Địa chỉ: <% =ViewData["diachi"]%>.<br />
Đơn c&oacute; nội dung: <%=d.CNOIDUNG %><br />
Căn cứ nội dung đơn khiếu nại; theo quy định tại Điều 6 Nghị định số 136/2006/NĐ-CP ng&agrave;y 14 th&aacute;ng 11 năm 2006 quy định chi tiết v&agrave; hướng dẫn thi h&agrave;nh một số điều của Luật Khiếu nại, tố c&aacute;o v&agrave; c&aacute;c Luật sửa đổi, bổ sung một số điều của Luật Khiếu nại, tố c&aacute;o; Ban D&acirc;n Nguyện thấy đơn khiếu nại của &ocirc;ng (b&agrave;) kh&ocirc;ng đủ điều kiện thụ l&yacute; giải quyết.<br />
Vậy Ban D&acirc;n Nguyện trả lại đơn để &ocirc;ng biết.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0">
	<tbody>
		<tr>
			<td style="width:281px;"><strong><em>Nơi nhận:</em></strong><br />
			- Như tr&ecirc;n;<br />
			- ...&nbsp;<br />
			- ...<br />
			&nbsp;</td>
			<td style="width:319px;">
			<div style="text-align: center;"><strong>VỤ TRƯỞNG </strong><br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			<strong>...</strong></div>
			&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;
</body>
</html>
