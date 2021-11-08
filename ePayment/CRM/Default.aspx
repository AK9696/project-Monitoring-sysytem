<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 70px;
        }
        .auto-style2 {
            height: 70px;
            width: 553px;
        }
        .auto-style3 {
            width: 553px;
        }
        .auto-style4 {
            height: 70px;
            width: 356px;
        }
        .auto-style5 {
            width: 356px;
        }
        .auto-style6 {
            width: 200%;
            height: 176px;
        }
        .auto-style7 {
            width: 139px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 579px">
            <table style="width: 100%; height: 200px;">
                <tr style="background-color:green;">
                    <td class="auto-style2"></td>
                    <td class="auto-style4">Reg_Page</td>
                    <td class="auto-style1"></td>
                </tr>
                <tr style="background-color:gray;">
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style5">
                        <table class="auto-style6">
                            <tr>
                                <td class="auto-style7">NAME</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7">F_NAME</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7">M_NAME</td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                            <td class="auto-style7">ADDERSS</td>
                       <td>
                       <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                         </td>
                         <td>&nbsp;</td>
                       </tr>
                           <tr>
                            <td class="auto-style7"></td>
                       <td>
                       
                           <asp:Button ID="Button1" runat="server" Text="Button" />
                       
                         </td>
                         <td>&nbsp;</td>
                       </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                </tr>
               
                   

            </table>
            </div>
            
        
    </form>
</body>
</html>
