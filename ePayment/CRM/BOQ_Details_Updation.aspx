<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true"
    CodeFile="BOQ_Details_Updation.aspx.cs" Inherits="BOQ_Details_Updation" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>

                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Details Of BOQ Items
                                </div>
                            </div>
                        </div>

                        <div id="divAddExtra" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Text Append* </label>
                                            <asp:TextBox ID="txtAppend" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label2" runat="server" Text="From SR No." CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtFromSRNo" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="To SR No." CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="TxtToSRNo" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="btnAd" Text="Add" OnClick="btnAd_Click" runat="server" CssClass="btn btn-info"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdBOQ" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdBOQ_PreRender" OnRowDataBound="grdBOQ_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="PackageBOQ_Id" HeaderText="PackageBOQ_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_Package_Id" HeaderText="PackageBOQ_Package_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_Unit_Id" HeaderText="PackageBOQ_Unit_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_Approval_Id" HeaderText="PackageBOQ_Approval_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GSTType" HeaderText="GSTType">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GSTPercenatge" HeaderText="GSTPercenatge">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="col-md-12">
                                                                    <table class="table table-striped table-bordered table-hover">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="" colspan="9">
                                                                                    <asp:TextBox ID="txtSpecification" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_Specification") %>' TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:ImageButton ID="btnDelete" Width="20px" Height="30px" OnClick="btnDelete_Click" ImageUrl="~/assets/images/delete.png" runat="server" />
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td class="">
                                                                                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" Width="110px"></asp:DropDownList>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_Qty") %>' MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtRateEstimate" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_RateEstimated") %>' MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateEstimate(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtAmountEstimate" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_AmountEstimated") %>' MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtRateQuoted" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_RateQuoted") %>' MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateQuote(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtAmountQuoted" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_AmountQuoted") %>' MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtQtyPaid" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_QtyPaid") %>' MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:TextBox ID="txtPerValuePaid" runat="server" CssClass="form-control" Text='<%# Eval("PackageBOQ_PercentageValuePaidTillDate") %>' MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:RadioButtonList ID="rblGST" runat="server" RepeatDirection="Horizontal">
                                                                                        <asp:ListItem Text="Include GST" Value="Include GST" />
                                                                                        <asp:ListItem Text="Exclude GST" Value="Exclude GST" />
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                                <td class="">
                                                                                    <asp:DropDownList ID="ddlGST" runat="server" RepeatDirection="Horizontal">
                                                                                        <asp:ListItem Text="0" Value="0" />
                                                                                        <asp:ListItem Text="5" Value="5" />
                                                                                        <asp:ListItem Text="12" Value="12" Selected="True" />
                                                                                        <asp:ListItem Text="18" Value="18" />
                                                                                        <asp:ListItem Text="28" Value="28" />
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="col-md-12">
                                                                    <table class="table table-striped table-bordered table-hover">
                                                                        <thead class="thin-border-bottom">
                                                                            <tr>
                                                                                <th class="">Unit</th>
                                                                                <th class="">Quantity</th>
                                                                                <th class="">Estimated Rate</th>
                                                                                <th class="">Estimated Amount</th>
                                                                                <th class="">Quoted Rate</th>
                                                                                <th class="">Quoted Amount</th>
                                                                                <th class="">Qty Paid Till Date</th>
                                                                                <th class="">Percentage Value Paid Till Date</th>
                                                                                <th class="">
                                                                                    <asp:RadioButtonList ID="rblGST_H" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblGST_H_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="Include GST" Value="Include GST" />
                                                                                        <asp:ListItem Text="Exclude GST" Value="Exclude GST" />
                                                                                    </asp:RadioButtonList>
                                                                                </th>
                                                                                <th class="">
                                                                                    <asp:DropDownList ID="ddlGST_H" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ddlGST_H_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="0" Value="0" />
                                                                                        <asp:ListItem Text="5" Value="5" />
                                                                                        <asp:ListItem Text="12" Value="12" Selected="True" />
                                                                                        <asp:ListItem Text="18" Value="18" />
                                                                                        <asp:ListItem Text="28" Value="28" />
                                                                                    </asp:DropDownList>
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btnSaveBOQ" Text="Update BOQ" OnClick="btnSaveBOQ_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnRefresh" Text=".." OnClick="btnRefresh_Click" runat="server" CssClass="" Width="2px" Height="2px"></asp:Button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" DynamicLayout="true" runat="server" AssociatedUpdatePanelID="up">
                <ProgressTemplate>
                    <div style="position: fixed; z-index: 999; height: 100%; width: 100%; top: 0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8; cursor: not-allowed;">
                        <div style="z-index: 1000; margin: 300px auto; padding: 10px; width: 130px; background-color: transparent; border-radius: 1px; filter: alpha(opacity=100); opacity: 1; -moz-opacity: 1;">
                            <img src="assets/images/mb/mbloader.gif" style="height: 150px; width: 150px;" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <!-- /.main-content -->
    </div>

    <script type="text/javascript">
        var _totalIdleTime = 0;

        function setSeconds() {
            _totalIdleTime = _totalIdleTime + 1;
            if (_totalIdleTime > 360) {
                _totalIdleTime = 0;
                __doPostBack('ctl00_ContentPlaceHolder1_btnRefresh', 'OnClick');
            }
        }


        $(document).ready(function () {
            setInterval(function () { setSeconds() }, 1000);
        });
    </script>

</asp:Content>
