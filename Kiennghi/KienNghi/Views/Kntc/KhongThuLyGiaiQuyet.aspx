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
   &nbsp;
<table border="0" cellpadding="0" cellspacing="0" width="624">
	<tbody>
		<tr>
			<td style="width:225px;height:84px;"><strong>BAN D&Acirc;N NGUYỆN</strong><br />
			<img height="2" src="file:///C:/Users/DELL/AppData/Local/Temp/msohtmlclip1/01/clip_image001.gif" width="85" />&nbsp;<br />
			&nbsp;<br />
			<br clear="ALL" />
			Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /BDN-V.I</td>
			<td style="width:399px;height:84px;">
			<div style="text-align: center;"><strong>CỘNG HO&Agrave; X&Atilde; HỘI CHỦ NGHĨA VIỆT NAM</strong><br />
			<strong>&nbsp;&nbsp; Độc lập - Tự do - Hạnh ph&uacute;c</strong></div>

			<table align="left" cellpadding="0" cellspacing="0">
				<tbody>
					<tr>
						<td height="1" style="text-align: center;">&nbsp;</td>
					</tr>
					<tr>
						<td style="text-align: center;">&nbsp;</td>
						<td style="text-align: center;"><img height="2" src="file:///C:/Users/DELL/AppData/Local/Temp/msohtmlclip1/01/clip_image002.gif" width="209" /></td>
					</tr>
				</tbody>
			</table>

			<div style="text-align: center;">&nbsp;<br />
			<br clear="ALL" />
			<em>​H&agrave; Nội, ng&agrave;y <% = DateTime.Now.Day %> th&aacute;ng <% = DateTime.Now.Month %> năm <% = DateTime.Now.Year %></em></div>
			</td>
		</tr>
		<tr>
			<td style="width:225px;height:73px;"><em>V/v kh&ocirc;ng thụ l&yacute; giải quyết khiếu nại do cơ quan, tổ chức, c&aacute; nh&acirc;n c&oacute; thẩm quyền chuyển đến</em></td>
			<td style="width:399px;height:73px;">&nbsp;</td>
		</tr>
	</tbody>
</table>
&nbsp;<br />
&nbsp;
<div style="text-align: center;"><strong>K&iacute;nh gửi: ...</strong></div>
&nbsp;<br />
&nbsp;<br />
Ng&agrave;y <% = Convert.ToDateTime(d.DNGAYNHAN).Day %> th&aacute;ng <% = Convert.ToDateTime(d.DNGAYNHAN).Month %> năm <% = Convert.ToDateTime(d.DNGAYNHAN).Year %>, Ban D&acirc;n Nguyện nhận được văn bản số ... ng&agrave;y ... của ... chuyển đơn của &ocirc;ng <%=d.CNGUOIGUI_TEN %>, thường tr&uacute; tại <% =ViewData["diachi"]%>.<br />
Đơn c&oacute; nội dung: <% =d.CNOIDUNG%><br />
Căn cứ nội dung đơn khiếu nại; theo quy định tại Điều 7 Nghị định số 136/2006/NĐ-CP ng&agrave;y 14 th&aacute;ng 11 năm 2006 của Ch&iacute;nh phủ quy định chi tiết v&agrave; hướng dẫn thi h&agrave;nh một số điều của Luật Khiếu nại, tố c&aacute;o v&agrave; c&aacute;c Luật sửa đổi, bổ sung một số điều của Luật Khiếu nại, tố c&aacute;o; Ban D&acirc;n Nguyện thấy đơn khiếu nại của &ocirc;ng Trần Minh Tuấn kh&ocirc;ng thuộc thẩm quyền giải quyết, v&igrave; vậy Ban D&acirc;n Nguyện trả lại đơn v&agrave; đề nghị chuyển đơn tr&ecirc;n đến đ&uacute;ng cơ quan c&oacute; thẩm quyền để được giải quyết theo quy định của ph&aacute;p luật.<br />
&nbsp;
<table border="0" cellpadding="0" cellspacing="0">
	<tbody>
		<tr>
			<td style="width:281px;"><strong><em>Nơi nhận:</em></strong><br />
			- Như tr&ecirc;n;<br />
			- ...<br />
			- ...<br />
			- ...<br />
			&nbsp;</td>
			<td style="width:327px;">
			<h1 align="center"><span style="font-size:14px;">TL. BỘ TRƯỞNG, CHỦ NHIỆM</span></h1>

			<h1 align="center"><span style="font-size:14px;">VỤ TRƯỞNG VỤ I</span></h1>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

			<h1>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</h1>
			&nbsp;<br />
			&nbsp;&nbsp;&nbsp;&nbsp;<br />
			<strong>...</strong></td>
		</tr>
	</tbody>
</table>
&nbsp;
</body>
</html>
