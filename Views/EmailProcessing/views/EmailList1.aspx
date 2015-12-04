<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<script runat="server">

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
</script>


<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Email Processing</title>
</head>
<body>
    <div>
        <asp:GridView ID="GridView1" runat="server" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="482px"></asp:GridView>
    </div>
</body>
</html>
