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
    <table border="0" cellpadding="0" cellspacing="0" width="624">
	<tbody>
		<tr>
			<td style="width:232px;height:85px;"><strong>&nbsp; ĐƠN VỊ QUẢN L&Yacute;</strong><br />
			&nbsp;
			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="0">&nbsp;</td>
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
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /ĐV-V.I</td>
			<td style="width:392px;height:85px;">
			<div style="text-align: center;"><strong>CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM</strong></div>
			<strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Độc lập - Tự do - Hạnh ph&uacute;c</strong><br />
			&nbsp;
			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="2">&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</tbody>
			</table>

			<div style="text-align: center;"><em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em></div>
			</td>
		</tr>
		<tr>
			<td style="width:232px;height:73px;"><em>V/v chuyển đơn khiếu nại sang&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; cơ quan nh&agrave; nước c&oacute; thẩm quyền giải quyết</em></td>
			<td style="width:392px;height:73px;">&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0" width="607">
	<tbody>
		<tr>
			<td style="width: 607px; text-align: center;">K&iacute;nh gửi: ...</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;<br />
Ng&agrave;y 02 th&aacute;ng 11 năm 2017 Ban D&acirc;n Nguyện nhận được đơn của &ocirc;ng Trần Minh Tuấn.<br />
Địa chỉ: x&atilde; Cổ Loa, huyện Đ&ocirc;ng Anh, th&agrave;nh phố H&agrave; Nội.<br />
Đơn c&oacute; nội dung:<br />
khiếu nại về đất đai<br />
Sau khi xem x&eacute;t đơn, căn cứ Điều 59, Điều 60 v&agrave; Điều 66 Luật Khiếu nại, tố c&aacute;o Ban D&acirc;n Nguyện xin chuyển đơn khiếu nại của &ocirc;ng Trần Minh Tuấn đến cơ quan để giải quyết theo quy định của ph&aacute;p luật.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0">
	<tbody>
		<tr>
			<td style="width:283px;"><strong><em>Nơi nhận:</em></strong><br />
			- Như tr&ecirc;n;<br />
			- &Ocirc;ng Trần Minh Tuấn (để biết);<br />
			- VPCP: BTCN, PCN Nguyễn Quang Thắng;<br />
			- Lưu: VT, V.I (3b), ĐH.<br />
			&nbsp;</td>
			<td style="width:325px;">
			<h1 style="text-align: center;"><span style="font-size:14px;">TL. BỘ TRƯỞNG, CHỦ NHIỆM<br />
			VỤ TRƯỞNG VỤ I</span></h1>
			&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;

			<div style="text-align: center;"><strong>Phan Văn Minh</strong></div>
			</td>
		</tr>
	</tbody>
</table>
&nbsp;
</html>
