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
    <table border="0" cellpadding="0" cellspacing="0" width="674">
	<tbody>
		<tr>
			<td style="width:225px;height:84px;"><strong>&nbsp; BAN D&Acirc;N NGUYỆN</strong><br />
			&nbsp;
			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="1">&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td><img height="2" src="file:///C:/Users/DELL/AppData/Local/Temp/msohtmlclip1/01/clip_image001.gif" width="85" /></td>
					</tr>
				</tbody>
			</table>
			&nbsp;<br />
			&nbsp;<br />
			<br clear="ALL" />
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /BDN-V.I</td>
			<td style="width:19px;height:84px;">&nbsp;</td>
			<td style="width:431px;height:84px;">
			<div style="text-align: center;"><strong>CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM</strong><br />
			<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Độc lập - Tự do - Hạnh ph&uacute;c</strong></div>

			<div style="text-align: center;"><br />
			<br />
			​</div>

			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="1" style="text-align: right;">&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</tbody>
			</table>

			<div style="text-align: center;"><em style="text-align: center;"><em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em></div>
			</td>
		</tr>
		<tr>
			<td style="width:225px;height:29px;">&nbsp;</td>
			<td style="width:19px;height:29px;">&nbsp;</td>
			<td style="width:431px;height:29px;">&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<div style="text-align: center;"><strong>GIẤY B&Aacute;O TIN</strong><br />
&nbsp;<br />
&nbsp;<br />
<strong>K&iacute;nh gửi: &ocirc;ng  <%=d.CNGUOIGUI_TEN %><br /></strong></div>
Địa chỉ: <% =ViewData["diachi"]%>.<br />
Đơn c&oacute; nội dung: <%=d.CNOIDUNG %><br />
Sau khi nghi&ecirc;n cứu đơn của &ocirc;ng đề nghị ng&agrave;y <% = Convert.ToDateTime(d.DNGAYNHAN).Day %> th&aacute;ng <% = Convert.ToDateTime(d.DNGAYNHAN).Month %> năm <% = Convert.ToDateTime(d.DNGAYNHAN).Year %><br />
khiếu nại về đất đai<br />
Ban D&acirc;n Nguyện thấy &ocirc;ng gửi đơn đ&oacute; đến ... để được xem x&eacute;t l&agrave; đ&uacute;ng quy định về thẩm quyền tại Luật Khiếu nại tố c&aacute;o. Đề nghị &ocirc;ng li&ecirc;n hệ với cơ quan n&ecirc;u tr&ecirc;n để biết kết quả.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0" width="600">
	<tbody>
		<tr>
			<td style="width:281px;"><strong><em>Nơi nhận:</em></strong><br />
			- Như tr&ecirc;n;<br />
			- ...&nbsp;<br />
			- ...<br />
			&nbsp;</td>
			<td style="width: 319px; text-align: center;"><strong>VỤ TRƯỞNG </strong><br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			<strong>...</strong></td>
		</tr>
	</tbody>
</table>
&nbsp;
</body>
</html>
