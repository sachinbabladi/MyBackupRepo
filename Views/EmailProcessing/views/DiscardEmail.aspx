<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<script runat="server">

    protected void btnDiscard_Click(object sender, EventArgs e)
    {
        
    }
</script>


<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>DiscardTicket</title>
    <style type="text/css">
        #TextArea1 {
            height: 146px;
            width: 622px;
            margin-left: 52px;
        }
        #Button1 {
            width: 79px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" action="Discard">
    <table style="width:100%">
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                Reason:

            </td>
        </tr>
        <tr>
            <td>
                <textarea id="txtDiscardReason" name="txtDiscardReason"></textarea><br />
               
            </td>
        </tr>
        <tr>
            <td style="text-align: right;padding-right:40px">
                 <br />
                 <input id="btnDiscard" type="Submit" value="Discard Email" style="width: 114px" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
