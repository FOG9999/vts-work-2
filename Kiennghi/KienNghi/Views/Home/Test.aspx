<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Test</title>
</head>
<body>
    <form method="post" enctype="multipart/form-data">
        <input name="file_upload" type="file" />
        <button value="cập nhật" type="submit"></button>
    </form>
    <div>
        <table>
            <%=ViewData["list"] %>
        </table>
    </div>
</body>
</html>
