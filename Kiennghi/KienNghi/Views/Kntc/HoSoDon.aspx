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
    <div class="tcenter"><strong >HỒ SƠ ĐƠN</strong></div><br />
&nbsp;
       <% KNTC_DON don = (KNTC_DON)ViewData["don"];
               KNTC d = (KNTC)ViewData["d"];
            %>
<table border="0" cellpadding="0" cellspacing="0" width="605">
	<tbody>
		<tr>
			<td style="width:338px;">Số hồ sơ: ...</td>
			<td style="width:267px;">Ng&agrave;y nhập: <%=Convert.ToDateTime(don.DNGAYNHAN).ToString("dd/MM/yyyy")%></td>
		</tr>
	</tbody>
</table>
Ng&agrave;y viết đơn: ...<br />
Người viết đơn: &ocirc;ng/Bà: <%=don.CNGUOIGUI_TEN %><br />
Địa chỉ: <%=d.diachi_nguoinop%><br />
Nguồn đơn: <%=d.nguondon %><br />
Đối tượng gửi đơn: ...<br />
Loại khiếu tố: <%=d.loaidon %><br />
Nội dung: <%=don.CNOIDUNG %><br /><br />
Đối tượng bị khiếu nại, tố c&aacute;o: ...<br />
T&ecirc;n đối tượng bị khiếu nại, tố c&aacute;o: ...<br />
Cấp quản l&yacute; của cơ quan bị khiếu nại, tố c&aacute;o: ...&nbsp;<br />
Hướng giải quyết: ...<br />
Cơ quan giải quyết tiếp: <%=d.donvi_thuly %><br />
Người xử l&yacute;: ...
</body>
</html>
