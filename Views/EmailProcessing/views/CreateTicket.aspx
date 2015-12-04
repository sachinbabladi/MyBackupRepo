<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void DropDownListTickettype0_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListTicketPriority_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListTicketGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListTicketUser_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownListTickettype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
</script>


<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Create Ticket</title>
    <style type="text/css">
        
        #txtBody {
            height: 110px;
            width: 491px;
        }
        
        #Button2 {
            margin-left: 0px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server" action="Create">
        <table runat="server" style="width: 100%;">
            <tr>
                <td class="auto-style1">MSP Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>

                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td class="auto-style2">MSP Site&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:DropDownList ID="dropdownSite" runat="server" DataSourceID="" Width="300px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Resource&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>

                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>Ticket Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                    <asp:DropDownList ID="DropDownListTickettype" runat="server" DataSourceID="" OnSelectedIndexChanged="DropDownListTickettype_SelectedIndexChanged" Width="299px" style="margin-left: 0px"></asp:DropDownList>
                    <br />
                </td>
            </tr>
            <tr>
                <td> <span style="vertical-align:top">Email Subject&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </span><asp:TextBox ID="txtSubject" runat="server" Width="486px"></asp:TextBox>
                    <br />
                    <br />
                     <span style="vertical-align:top">Email Body&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </span><textarea ID="txtBody" runat="server"></textarea>
                    <br />
                    <br />
                    <span style="vertical-align:top">Ticket Priority&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </span><asp:DropDownList ID="DropDownListTicketPriority" runat="server" DataSourceID="" OnSelectedIndexChanged="DropDownListTicketPriority_SelectedIndexChanged" Width="327px"></asp:DropDownList>
                    <br />
                    <br />
                    <span style="vertical-align:top">Assign To Group&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </span><asp:DropDownList ID="DropDownListTicketGroup" runat="server" DataSourceID="" OnSelectedIndexChanged="DropDownListTicketGroup_SelectedIndexChanged" Width="326px"></asp:DropDownList>
                    <br />
                    <br />
                    <span style="vertical-align:top">Assign To User&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </span><asp:DropDownList ID="DropDownListTicketUser" runat="server" DataSourceID="" OnSelectedIndexChanged="DropDownListTicketUser_SelectedIndexChanged" Width="333px"></asp:DropDownList>
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
