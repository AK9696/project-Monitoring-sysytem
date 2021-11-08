<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Map_UP.aspx.cs" Inherits="Map_UP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map View</title>
    <script src="http://code.jquery.com/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="assets/js/jquery.tooltip.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function InitializeToolTip() {
            $(".gridViewToolTip").tooltip({
                track: true,
                delay: 0,
                showURL: false,
                fade: 100,
                bodyHandler: function () {
                    var content = $($(this).next().html())[0].innerText;
                    var data1 = content.split(':-');
                    var District;
                    var Demand;
                    var Collection;
                    var Outstanding;
                    var Expenditure;
                    if (data1 != null && data1.length > 0) {
                        District = data1[0];
                        var data2 = data1[1].split(':');
                        if (data2 != null && data2.length > 0) {
                            Demand = data2[0];
                            Collection = data2[1];
                            Outstanding = data2[2];
                            Expenditure = data2[3];
                        }
                    }
                    var toolTipData = "<div><h3>District: " + District + "</h3> <br> <table class='gridtable'> <tbody><tr> <th>निकाय में चल रहे प्रोजेक्ट की संख्या: </th> <td><i class='fa fa-inr'></i>" + Demand + "</td> </tr> <tr> <th>स्वीकृत धनराशी (लाख में):</th> <td><i class='fa fa-inr'></i> " + Collection + "</td> </tr> <tr> <th>कुल अवमुक्त धनराशी (लाख में):</th> <td><i class='fa fa-inr'></i> " + Outstanding + "</td> </tr> <tr> <th>कुल व्यय (लाख में):</th> <td>" + Expenditure + "</td> </tr> </tbody></table></div>";
                    return $(toolTipData);
                },
                showURL: false
            });
        }
    </script>
    <script type="text/javascript">
        $(function () {
            InitializeToolTip();
            debugger;
            var legends_HTML = document.getElementById('hfLegends').value;
            var frame = $(this);
        })
    </script>
    <style type="text/css">
        #tooltip {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: rgba(255, 255, 255, 0.5);
            padding: 5px;
            opacity: 0.85;
        }

            #tooltip h3, #tooltip div {
                margin: 0;
            }

        .table1 {
            width: 100%;
            max-width: 100%;
            margin-bottom: 20px;
        }

        table.gridtable {
            width: 100%;
            font-size: 11px;
            color: #333333;
            border-collapse: collapse;
            overflow-x: auto;
            text-align: center;
        }

            table.gridtable th {
                padding: 8px;
                background-color: rgba(81, 81, 81, 0.1);
                border: 1px solid rgba(81, 81, 81, 0.4);
            }

                table.gridtable th.bnd {
                    padding: 4px !important;
                    background-color: rgba(81, 81, 81, 0.1);
                    border: 1px solid rgba(81, 81, 81, 0.4);
                }

            table.gridtable td {
                font-size: 11px;
                line-height: 20px;
                padding: 8px;
                background-color: rgba(255, 255, 255, 0.5);
                border: 1px solid rgba(81, 81, 81, 0.4);
                text-align: left;
                vertical-align: top;
            }

                table.gridtable td.bnd {
                    font-size: 11px;
                    line-height: 17px;
                    padding: 4px !important;
                    background-color: rgba(255, 255, 255, 0.5);
                    border: 1px solid rgba(81, 81, 81, 0.4);
                    text-align: left;
                    vertical-align: top;
                }

            table.gridtable tr:nth-child(2n) {
                background: #f1f1f1;
            }

            table.gridtable td a {
                word-break: break-all;
            }
    </style>
</head>
<body>
    <form id="frmMap" runat="server">
        <div id="div_Progress_Bar" runat="server" style="margin-top: 50px;"></div>
        <center>
            <asp:RadioButtonList ID="rbtData" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtData_SelectedIndexChanged">
                <asp:ListItem Selected="True" Value="1">Running Projects</asp:ListItem>
                <asp:ListItem Value="2">Total Budget</asp:ListItem>
                <asp:ListItem Value="3">Total Release</asp:ListItem>
            </asp:RadioButtonList>
        </center>

        <div runat="server" id="divDataPointsL" name="divDataPointsL" style="display:none;">
            <table class="table1">
                <thead>
                    <tr>
                        <th style="background-color: #E6EDFE">1</th>
                        <th style="background-color: #CDDBFD">2</th>
                        <th style="background-color: #B4C9FC">3</th>
                        <th style="background-color: #9CB7FB">4</th>
                        <th style="background-color: #83A5FB">5</th>
                        <th style="background-color: #6A93FA">6</th>
                        <th style="background-color: #5281F9">7</th>
                        <th style="background-color: #396FF8">8</th>
                        <th style="background-color: #205DF7">9</th>
                        <th style="background-color: #084CF7">10</th>
                    </tr>
                </thead>
            </table>
        </div>
    </form>
</body>
</html>
