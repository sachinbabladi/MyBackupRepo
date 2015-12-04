<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<script runat="server">

    protected void page_load(object sender, EventArgs e)
    {
        this.HyperLink1.Text = "200";
        
    }
</script>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body onload="page_load">
    <form id="form1" runat="server">
   <table style="width:100%">
        <tr>
            <td style="text-align:left;font:bold 15px arial;padding-left:20px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
                <asp:Label ID="Label2" runat="server" Text="Email To Ticket Unparsed Email"></asp:Label>
               
            </td>
        </tr>
        <tr>
            <td style="text-align:left;font: 15px arial;padding-left:20px;">
                
               
                <asp:Label ID="Label1" runat="server" Text="Total Count of Unprocessed Emails:"></asp:Label>
                
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="../../EmailProcessing/List" Text="100"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;padding-right:40px">
               

            </td>
        </tr>
    </table>
    </form>
</body>
</html>
