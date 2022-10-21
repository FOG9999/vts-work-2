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
			<td style="width:227px;height:84px;"><strong>&nbsp; BAN D&Acirc;N NGUYỆN</strong><br />
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
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /BDN-V.I</td>
			<td style="width:429px;height:84px;">
			<div style="text-align: center;"><strong>&nbsp;&nbsp; CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM</strong><br />
			<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Độc lập - Tự do - Hạnh ph&uacute;c</strong><br />
			&nbsp;</div>

			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="1" style="text-align: center;">&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</tbody>
			</table>

			<div style="text-align: center;"><br />
			<em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em></div>
			</td>
		</tr>
		<tr>
			<td style="width:227px;height:29px;">&nbsp;</td>
			<td style="width:429px;height:29px;">&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<div style="text-align: center;"><strong>PHIẾU ĐỀ XUẤT XỬ L&Yacute; ĐƠN</strong><br />
&nbsp;<br />
&nbsp;<br />
<strong>K&iacute;nh gửi: L&atilde;nh đạo Văn ph&ograve;ng</strong></div>
&nbsp;<br />
&nbsp;<br />
Ng&agrave;y 02 th&aacute;ng 11 năm 2017, Ph&ograve;ng Xử l&yacute; đơn, Ban D&acirc;n Nguyện nhận được đơn khiếu nại của &ocirc;ng Trần Minh Tuấn.<br />
Địa chỉ: x&atilde; Cổ Loa, huyện Đ&ocirc;ng Anh, th&agrave;nh phố H&agrave; Nội.<br />
T&oacute;m tắt nội dung đơn:<br />
khiếu nại về đất đai<br />
Căn cứ nội dung đơn v&agrave; thẩm quyền giải quyết, đề xuất.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0" style="width:586px;" width="586">
	<tbody>
		<tr>
			<td style="width:288px;">
			<div style="text-align: center;"><strong>PH&Ecirc; DUYỆT CỦA THỦ TRƯỞNG</strong><br />
			<strong>CƠ QUAN, ĐƠN VỊ</strong><br />
			&nbsp;</div>
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;
			<div style="text-align: center;">Ng&agrave;y&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; th&aacute;ng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; năm</div>
			&nbsp;</td>
			<td style="width:298px;">
			<div style="text-align: center;"><strong>C&Aacute;N BỘ ĐỀ XUẤT</strong></div>
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;
</body>
</html>
