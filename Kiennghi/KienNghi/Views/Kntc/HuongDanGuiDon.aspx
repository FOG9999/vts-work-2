<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
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
          <% KNTC_DON d = (KNTC_DON)ViewData["Thongtindonkntc"]; %>
	<tbody>
		<tr>
			<td style="width: 225px; height: 84px;">&nbsp; <strong>ĐƠN VỊ QUẢN L&Yacute;</strong><br />
			<img height="2" src="file:///C:/Users/DELL/AppData/Local/Temp/msohtmlclip1/01/clip_image001.gif" width="85" />&nbsp;<br />
			&nbsp;<br />
			<br clear="ALL" />
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;/-V.I</td>
			<td style="width: 19px; height: 84px;">&nbsp;</td>
			<td style="width: 431px; height: 84px;">
			<div style="text-align: center;"><strong>CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM<br />
			​</strong><strong>&nbsp; &nbsp; Độc lập - Tự do - Hạnh ph&uacute;c</strong></div>

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

			<div style="text-align: center;"><br />
			<br />
			<em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em></div>
			</td>
		</tr>
		<tr>
			<td style="width: 225px; height: 73px;"><em>V/v hướng dẫn gửi đơn khiếu nại đến cơ quan, tổ chức, c&aacute; nh&acirc;n c&oacute; thẩm quyền giải quyết</em></td>
			<td style="width: 19px; height: 73px;">&nbsp;</td>
			<td style="width: 431px; height: 73px; text-align: center;">&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<div style="text-align: center;"><strong>PHIẾU HƯỚNG DẪN</strong><br />
&nbsp;<br />
K&iacute;nh gửi: &ocirc;ng/bà </div>
&nbsp;<br />
Ng&agrave;y <% = Convert.ToDateTime(d.DNGAYNHAN).Day %> th&aacute;ng <% = Convert.ToDateTime(d.DNGAYNHAN).Month %> năm <% = Convert.ToDateTime(d.DNGAYNHAN).Year %> Ban D&acirc;n Nguyện nhận được đơn khiếu nại của &ocirc;ng <% = d.CNGUOIGUI_TEN %>.<br />
Địa chỉ: <% =ViewData["diachi"]%>.<br />
Đơn c&oacute; nội dung: <%=d.CNOIDUNG %><br />
Căn cứ nội dung đơn khiếu nại; theo quy định tại Điều 6 Nghị định số 136/2006/NĐ-CP ng&agrave;y 14 th&aacute;ng 11 năm 2006 của Ban D&acirc;n Nguyện chi tiết v&agrave; hướng dẫn thi h&agrave;nh một số điều của Luật Khiếu nại, tố c&aacute;o v&agrave; c&aacute;c Luật sửa đổi, bổ sung một số điều của Luật Khiếu nại, tố c&aacute;o Ban D&acirc;n Nguyện thấy đơn khiếu nại của &ocirc;ng kh&ocirc;ng thuộc thẩm quyền giải quyết của Ban D&acirc;n Nguyện.<br />
&nbsp;Đề nghị &ocirc;ng gửi đơn đến ... để được giải quyết theo quy định của ph&aacute;p luật.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0">
	<tbody>
		<tr>
			<td style="width: 283px;"><strong><em>Nơi nhận:</em></strong><br />
			- Như tr&ecirc;n;<br />
			- ...&nbsp;<br />
			- ...<br />
			&nbsp;</td>
			<td style="width: 325px;">
			<div style="text-align: center;"><strong>VỤ TRƯỞNG </strong></div>
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;<br />
			&nbsp;
			<div style="text-align: center;"><strong>...

			                                 </strong></div>
			</td>
		</tr>
	</tbody>
</table>

</body>
</html>
