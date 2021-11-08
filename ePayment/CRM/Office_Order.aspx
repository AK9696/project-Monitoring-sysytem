<%@ Page Language="C#" MasterPageFile="~/TemplatePopup.master" AutoEventWireup="true" CodeFile="Office_Order.aspx.cs" Inherits="Office_Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="main-content" onload="javascript:print();">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Work Progress Details
                                    <div class="form-group form-check" style="float: right; padding-right: 10px">
                                        <asp:Label ID="lblScheme" ForeColor="Yellow" Font-Bold="true" runat="server" Text="---NA---" CssClass="control-label no-padding-right"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdPost_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="परियोजना का नाम" DataField="ProjectWork_Name" />
                                                <asp:BoundField HeaderText="सैप  वर्ष जिसमे परियोजन सीवकृत है" DataField="ProjectWotkYear" />
                                                <asp:BoundField HeaderText="स्वीकृति लागत (लाख में)" DataField="ProjectWork_Budget" />
                                                <asp:BoundField HeaderText="कार्य लागत / सेंटेज (लाख में)" DataField="WorkCostWithCentage" />
                                                <asp:BoundField HeaderText="निविदित  लागत (लाख में)" DataField="ProjectWorkPkg_AgreementAmount" />
                                                <asp:BoundField HeaderText="प्रथम क़िस्त धनराशि केद्रांश (लाख में)" DataField="CentralShare1" />
                                                <asp:BoundField HeaderText="प्रथम क़िस्त धनराशि राज्यांश (लाख में)" DataField="StateShare1" />
                                                <asp:BoundField HeaderText="कुल धनराशी (लाख में)" DataField="Total1" />
                                                <asp:BoundField HeaderText="दिव्तीय क़िस्त धनराशि केद्रांश (लाख में)" DataField="CentralShare2" />
                                                <asp:BoundField HeaderText="दिव्तीय क़िस्त धनराशि राज्यांश (लाख में)" DataField="StateShare2" />
                                                <asp:BoundField HeaderText="कुल धनराशी (लाख में)" DataField="Total2" />
                                                <asp:BoundField HeaderText="तिर्तीय क़िस्त धनराशि केद्रांश (लाख में)" DataField="CentralShare3" />
                                                <asp:BoundField HeaderText="तिर्तीय क़िस्त धनराशि राज्यांश (लाख में)" DataField="StateShare3" />
                                                <asp:BoundField HeaderText="कुल धनराशी (लाख में)" DataField="Total3" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="grdPostSecond" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="परियोजना का नाम" DataField="ProjectWork_Name" />
                                                <asp:BoundField HeaderText="सैप  वर्ष जिसमे परियोजन सीवकृत है" DataField="ProjectWotkYear" />
                                                <asp:BoundField HeaderText="स्वीकृति लागत (लाख में)" DataField="ProjectWork_Budget" />
                                                <asp:BoundField HeaderText="कार्य लागत" DataField="ProjectWork_WorkCost" />
                                                <asp:BoundField HeaderText="निविदित  लागत (लाख में)" DataField="ProjectWorkPkg_AgreementAmount" />
                                                <asp:BoundField HeaderText="केद्रांश (लाख में)" DataField="CentralShare" />
                                                <asp:BoundField HeaderText="राज्यांश (लाख में)" DataField="StateShare" />
                                                <asp:BoundField HeaderText="सेंटेज (लाख में)" DataField="ProjectWork_Centage" />
                                                <asp:BoundField HeaderText="निकायांश (लाख में)" DataField="ULBShare" />
                                                <asp:BoundField HeaderText="कुल धनराशी (लाख में)" DataField="Total" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Description Of Project and Contract/Firm
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <table id="simple-table" class="table table-striped table-bordered table-hover">
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">City</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblCity" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Year</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblYear" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Sector</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblSector" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Project ID (MoUD)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblProjectID" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Project Name</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblProjectName" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Sanctioned Amount Without Centage (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblSanctionedAmountWithoutCentage" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Central Share (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblCentralShare" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">State Share (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblStateShare" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                                </tr>
                                                <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">ULB Share (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblULBShare" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                                    </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Centage (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblCentage" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Actual Centage Amount received by (Jal Nigam) Against this Project (till date)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblActualCentageAmountreceived" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Tendered Amount (In Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblTenderedAmount" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Amount Released To Implementing Agency (Jal Nigam) Against this Project (till date)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblAmountReleasedToImplementingAgency" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Expenditure done by Implementing Agency (Jal Nigam) on this Project (till date) (in Lakhs)</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblExpendituredonebyImplementingAgency" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label  runat="server" CssClass="control-label no-padding-right" Font-Bold="true">Balance amount (if any) of this project with Jal Nigam as shown in bank statement</asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lblBalanceamount" runat="server" CssClass="control-label no-padding-right" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Deduction Details
                                </div>
                            </div>
                        </div>
                         <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdDeductionHistory" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:BoundField DataField="PackageInvoiceAdditional_Id" HeaderText="PackageInvoiceAdditional_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceAdditional_Invoice_Id" HeaderText="PackageInvoiceAdditional_Invoice_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Deduction_Category" HeaderText="Category" />
                                                    <asp:BoundField DataField="Deduction_Name" HeaderText="Deduction" />
                                                    <asp:BoundField DataField="PackageInvoiceAdditional_Deduction_Value_Master" HeaderText="Value (Flat / Percentage)" />
                                                    <asp:BoundField DataField="PackageInvoiceAdditional_Deduction_Value_Final" HeaderText="Deduction Value" />
                                                    <asp:BoundField DataField="PackageInvoiceAdditional_Comments" HeaderText="Comments" />
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
                            <img src="assets/images/ulb/loading.gif" style="height: 150px; width: 150px;" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <!-- DataTable specific plugin scripts -->
    <script src="assets/js/jquery-2.1.4.min.js"></script>
    <script src="assets/js/jquery.dataTables.min.js"></script>
    <script src="assets/js/jquery.dataTables.bootstrap.min.js"></script>
    <script src="assets/js/dataTables.buttons.min.js"></script>
    <script src="assets/js/buttons.flash.min.js"></script>
    <script src="assets/js/buttons.html5.min.js"></script>
    <script src="assets/js/buttons.print.min.js"></script>
    <script src="assets/js/buttons.colVis.min.js"></script>
    <script src="assets/js/dataTables.select.min.js"></script>
    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>
    <script src="assets/js/dataTables.fixedHeader.min.js"></script>
    <script src="assets/js/jquery.mark.min.js"></script>
    <script src="assets/js/datatables.mark.js"></script>
    <script src="assets/js/dataTables.colReorder.min.js"></script>

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
            jQuery(function ($) {
                $('.modalBackground1').click(function () {
                    var id = $(this).attr('id').replace('_backgroundElement', '');
                    $find(id).hide();
                });
            })
        });
    </script>
</asp:Content>
