<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillView.aspx.cs" Inherits="BillView" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bill Preview</title>
    <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" Visible="false" />
        <br />
        <div id="dvReport">
            <CR:CrystalReportViewer ID="crvBillView" runat="server" ToolPanelView="None" />
        </div>
    </form>
</body>
</html>
