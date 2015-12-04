<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
</script>


<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Create Ticket</title>
    <style type="text/css">
        
        #txtDesc {
            height: 115px;
            width: 501px;
        }
        #txtNotes {
            height: 116px;
            width: 503px;
        }
        #txtNewNotes {
            height: 115px;
            width: 486px;
            margin-left: 0px;
        }
        
        .auto-style1 {
            height: 80px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server" action="Update">
        <table runat="server" style="width: 100%;">
            <tr>
                <td class="auto-style1">
                    <br />
                    <br />
                    <br />
                    
                    MSP Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;
               </td>
            </tr>
            <tr>
                <td class="auto-style2">MSP Site&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:DropDownList ID="dropdownSite" runat="server" DataSourceID="" Width="300px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <span style="vertical-align:top">  Subject&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </span> <asp:TextBox ID="txtSubject" runat="server" Width="498px"></asp:TextBox>
                    <br />
                    <br />
                     <span style="vertical-align:top"> Ticket Description&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                    <textarea ID="txtDesc" runat="server"></textarea>
                    <br />
                    <br />
                     <span style="vertical-align:top"> Ticket Notes&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </span><textarea ID="txtNotes" runat="server"></textarea>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    </td>
            </tr>
            <tr>
                <td>
                    
                    <strong>Update Ticket Notes:</strong></td>
                </tr>
            <tr>
                <td style="border:solid">
                    <span style="vertical-align:top"> New Ticket Note&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                    </span><textarea ID="txtNewNotes" runat="server"></textarea>
                    </td>
            </tr>
            <tr style="text-align: right">
                <td>
                    <input id="btnSubmit" type="submit" value="Create Ticket" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
