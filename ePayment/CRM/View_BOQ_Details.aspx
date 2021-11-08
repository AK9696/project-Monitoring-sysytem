<%@ Page Title="" Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="View_BOQ_Details.aspx.cs" Inherits="View_BOQ_Details" %>

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
                                    View BOQ Items Details 
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdBOQ" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdBOQ_PreRender" OnRowDataBound="grdBOQ_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description Of Goods">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSpecification" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageBOQ_Specification") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Unit_Name" HeaderText="Unit"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_Qty" HeaderText="Quantity"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_RateEstimated" HeaderText="Estimated Rate"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_AmountEstimated" HeaderText="Estimated Amount"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_RateQuoted" HeaderText="Quoted Rate"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_AmountQuoted" HeaderText="Quoted Amount"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_QtyPaid" HeaderText="Qty Paid Till Date"></asp:BoundField>
                                                <asp:BoundField DataField="PackageBOQ_PercentageValuePaidTillDate" HeaderText="Percentage Value Paid Till Date"></asp:BoundField>
                                                <asp:BoundField DataField="GSTType" HeaderText="GST Type"></asp:BoundField>
                                                <asp:BoundField DataField="GSTPercenatge" HeaderText="GST Percentage"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
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
</asp:Content>

