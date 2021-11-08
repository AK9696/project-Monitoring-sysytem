<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true"
    CodeFile="MasterGenerateInvoice_Detail.aspx.cs" Inherits="MasterGenerateInvoice_Detail" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="mpViewCoverLetter" runat="server" PopupControlID="Panel2" TargetControlID="btnShowPopup2"
                        CancelControlID="btnclose2" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup2" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <cc1:ModalPopupExtender ID="mpViewSummery" runat="server" PopupControlID="Panel1" TargetControlID="btnShowPopup1"
                        CancelControlID="btnclose1" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup1" Text="Show" runat="server" Style="display: none;"></asp:Button>
                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Details Of Invoice Items
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdInvoice" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdInvoice_PreRender">
                                            <Columns>
                                                <asp:BoundField DataField="PackageInvoice_Id" HeaderText="PackageInvoice_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PackageInvoice_Package_Id" HeaderText="PackageInvoice_Package_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectWork_Id" HeaderText="ProjectWork_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnOpenInvoice" Width="20px" Height="20px" OnClick="btnOpenInvoice_Click" ImageUrl="~/assets/images/edit.png" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="List_EMBNo" HeaderText="EMB No" />
                                                <asp:BoundField DataField="PackageInvoice_Date" HeaderText="Invoice Date" />
                                                <asp:BoundField DataField="PackageInvoice_VoucherNo" HeaderText="Voucher No" />
                                                <asp:BoundField DataField="PackageInvoice_SBR_No" HeaderText="SBR No" />
                                                <asp:BoundField DataField="PackageInvoice_DBR_No" HeaderText="DBR No" />
                                                <asp:BoundField DataField="Total_Line_Items" HeaderText="Total Line Items" />
                                                <asp:BoundField DataField="PackageInvoiceItem_Total_Qty" HeaderText="Total Qty" />
                                                <asp:BoundField DataField="Total_Amount" HeaderText="Total Amount" />
                                                <asp:BoundField DataField="PackageInvoice_AddedOn" HeaderText="Added On" />
                                                <asp:TemplateField HeaderText="Invoice">
                                                    <ItemTemplate>
                                                        <a href="MasterGenerateInvoice_View.aspx?Package_Id=0&Invoice_Id=<%# Eval("PackageInvoice_Id") %>" target="_blank">View Invoice</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EMB">
                                                    <ItemTemplate>
                                                        <a href="ViewEMBDetails.aspx?Invoice_Id=<%# Eval("PackageInvoice_Id") %>"" target="_blank"> View EMB</a>
                                                         <%--<asp:ImageButton ID="btnViewEMB" ToolTip="View EMB" Width="20px" Height="20px"  ImageUrl="~/assets/images/edit.png" runat="server">
                                                         </asp:ImageButton>--%>
                                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BOQ">
                                                    <ItemTemplate>
                                                        <a href="View_BOQ_Details.aspx?Package_Id=<%# Eval("PackageInvoice_Package_Id") %>"" target="_blank"> View BOQ</a>
                                                         <%--<asp:ImageButton ID="tnViewBOQ" ToolTip="View BOQ" Width="20px" Height="20px" OnClick="tnViewBOQ_Click" ImageUrl="~/assets/images/edit.png" runat="server" OnClientClick="target ='_blank';"/>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space-6"></div>

                        <hr />
                        <div id="divEntry" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdPackageDetails" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdPackageDetails_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="ProjectWorkPkg_Id" HeaderText="ProjectWorkPkg_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectWork_Id" HeaderText="ProjectWork_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectWork_Project_Id" HeaderText="ProjectWork_Project_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectWork_DistrictId" HeaderText="ProjectWork_DistrictId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectWork_ULB_Id" HeaderText="ProjectWork_ULB_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectWork_DivisionId" HeaderText="ProjectWork_DivisionId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="" HeaderText="">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="District" DataField="Jurisdiction_Name_Eng">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                    <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                    <asp:BoundField HeaderText="Project" DataField="Project_Name" />
                                                    <asp:BoundField HeaderText="Work Code" DataField="ProjectWork_ProjectCode" />
                                                    <asp:BoundField HeaderText="Work" DataField="ProjectWork_Name" />
                                                    <asp:BoundField HeaderText="Budget" DataField="ProjectWork_Budget" />
                                                    <asp:BoundField HeaderText="Package Code" DataField="ProjectWorkPkg_Code" />
                                                    <asp:BoundField HeaderText="Package Name" DataField="ProjectWorkPkg_Name" />
                                                    <asp:BoundField HeaderText="Agreement Amount" DataField="ProjectWorkPkg_AgreementAmount" />
                                                    <asp:BoundField HeaderText="Agreement No" DataField="ProjectWorkPkg_Agreement_No" />
                                                    <asp:BoundField HeaderText="Due Date Of Completion" DataField="ProjectWorkPkg_Due_Date" />
                                                    <asp:BoundField HeaderText="Vendor / Contractor" DataField="Vendor_Name" />
                                                    <asp:BoundField HeaderText="Vendor / Contractor (Mobile)" DataField="Vendor_Mobile" />
                                                    <asp:BoundField HeaderText="Reporting Staff (JE / APE)" DataField="List_ReportingStaff_JEAPE_Name" />
                                                    <asp:BoundField HeaderText="Reporting Staff (AE / PE)" DataField="List_ReportingStaff_AEPE_Name" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="space-6"></div>

                                <div class="col-sm-7 infobox-container">
                                    <div class="infobox infobox-grey infobox-small infobox-dark">

                                        <a class="infobox-icon" runat="server" id="lnk_ContractBond" onserverclick="lnk_ContractBond_ServerClick">
                                            <i class="ace-icon fa fa-download"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Contract</div>
                                            <div class="infobox-content">Bond</div>
                                        </div>
                                    </div>

                                    <div class="infobox infobox-grey infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="lnk_PerformanceSecurity" onserverclick="lnk_PerformanceSecurity_ServerClick">
                                            <i class="ace-icon fa fa-download"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Performance</div>
                                            <div class="infobox-content">Security</div>
                                        </div>
                                    </div>

                                    <div class="infobox infobox-grey infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="lnk_BankGurantee" onserverclick="lnk_BankGurantee_ServerClick">
                                            <i class="ace-icon fa fa-download"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Bank</div>
                                            <div class="infobox-content">Guarantee</div>
                                        </div>
                                    </div>

                                    <div class="infobox infobox-grey infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="lnk_MoblizationAdvance" onserverclick="lnk_MoblizationAdvance_ServerClick">
                                            <i class="ace-icon fa fa-download"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Mobilization</div>
                                            <div class="infobox-content">Advance</div>
                                        </div>
                                    </div>

                                    <div class="space-6"></div>

                                    <div class="infobox infobox-blue infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="lnk_CoverLetter" onserverclick="lnk_CoverLetter_ServerClick">
                                            <i class="ace-icon fa fa-envelope-o"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Cover </div>
                                            <div class="infobox-content">Letter</div>
                                        </div>
                                    </div>

                                    <div class="infobox infobox-green infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="lnk_PaymentSummery" onserverclick="lnk_PaymentSummery_ServerClick">
                                            <i class="ace-icon fa fa-rupee"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Payment</div>
                                            <div class="infobox-content">Summery</div>
                                        </div>
                                    </div>

                                    <div class="infobox infobox-green infobox-small infobox-dark">
                                        <a class="infobox-icon" runat="server" id="link_PaymentOrder" onserverclick="link_PaymentOrder_ServerClick">
                                            <i class="ace-icon fa fa-rupee"></i>
                                        </a>

                                        <div class="infobox-data">
                                            <div class="infobox-content">Payment</div>
                                            <div class="infobox-content">Order</div>
                                        </div>
                                    </div>
                                </div>

                                <div class="vspace-12-sm"></div>

                                <div class="col-sm-5">
                                    <div class="widget-box">
                                        <div class="widget-header widget-header-flat widget-header-small">
                                            <h5 class="widget-title">
                                                <i class="ace-icon fa fa-signal"></i>
                                                Funding Pattern Breakup
                                            </h5>
                                            <div class="widget-body">
                                                <div class="widget-main">
                                                    <div style="overflow: auto">
                                                        <asp:GridView ID="grdFundingPattern" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdFundingPattern_PreRender">
                                                            <Columns>
                                                                <asp:BoundField DataField="FundingPattern_Id" HeaderText="FundingPattern_Id">
                                                                    <HeaderStyle CssClass="displayStyle" />
                                                                    <ItemStyle CssClass="displayStyle" />
                                                                    <FooterStyle CssClass="displayStyle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FundingPattern_Name" HeaderText="Funding Pattern" />
                                                                <asp:BoundField DataField="ProjectWorkFundingPattern_Value" HeaderText="Sanction" />
                                                                <asp:BoundField DataField="As_Per_GO" HeaderText="Received As Per GO" />
                                                                <asp:BoundField DataField="Release" HeaderText="Reeleased" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- /.widget-box -->
                                </div>
                                <!-- /.col -->
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Invoice No* </label>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblZone" runat="server" Text="Invoice Date" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control date-picker" autocomplete="off" data-date-format="dd/mm/yyyy"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label9" runat="server" Text="SBR No" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtSBRNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">DBR No</label>
                                            <asp:TextBox ID="txtDBRNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Narration </label>
                                            <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdBOQ" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdBOQ_PreRender" OnRowDataBound="grdBOQ_RowDataBound" ShowFooter="true">
                                                <Columns>
                                                    <asp:BoundField DataField="PackageInvoiceItem_Id" HeaderText="PackageInvoiceItem_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceItem_Invoice_Id" HeaderText="PackageInvoiceItem_Invoice_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Unit_Id" HeaderText="PackageEMB_Unit_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceItem_PackageEMB_Id" HeaderText="PackageInvoiceItem_PackageEMB_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>

                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Specification / Description" HeaderStyle-Width="40%" ItemStyle-Width="40%"
                                                        FooterStyle-Width="40%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSpecification" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageEMB_Specification") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Unit" DataField="Unit_Name" />
                                                    <asp:BoundField HeaderText="Quantity Paid Till Date" DataField="PackageBOQ_QtyPaid" />
                                                    <asp:BoundField HeaderText="Rate" DataField="Total_Rate" />
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <table class="table table-striped table-bordered table-hover">
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Label ID="lblQty" runat="server" Font-Bold="true" Text='<%# Eval("PackageInvoiceItem_Total_Qty_BOQ") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Label ID="lblQtyExtra" runat="server" CssClass="label label-danger arrowed" Font-Bold="true" Text='<%# Eval("PackageInvoiceItem_QtyExtra") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Label ID="lblQtyExtraPer" runat="server" CssClass="label label-danger arrowed" Font-Bold="true" Text='<%# Eval("PackageInvoiceItem_QtyExtraPer") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Percentage Amount To Be Released">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPercentageToBeReleased" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageInvoiceItem_PercentageToBeReleased") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    <asp:BoundField DataField="PackageInvoiceItem_RateQuoted" HeaderText="Quoted Rate">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceItem_RateEstimated" HeaderText="Estimated Rate">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceItem_Total_Qty" HeaderText="Current Quantity">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Taxes / (%)" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                        <HeaderStyle Width="250px" />
                                                        <ItemStyle Width="250px" />
                                                        <ItemTemplate>
                                                            <asp:GridView ID="grdTaxes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnPreRender="grdTaxes_PreRender" ShowHeader="false" OnRowDataBound="grdTaxes_RowDataBound" Width="250px">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                                        <HeaderStyle CssClass="displayStyle" />
                                                                        <ItemStyle CssClass="displayStyle" />
                                                                        <FooterStyle CssClass="displayStyle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PackageInvoiceItem_Tax_Deduction_Id" HeaderText="PackageInvoiceItem_Tax_Deduction_Id">
                                                                        <HeaderStyle CssClass="displayStyle" />
                                                                        <ItemStyle CssClass="displayStyle" />
                                                                        <FooterStyle CssClass="displayStyle" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlTaxes" runat="server" CssClass="form-control" Width="80px" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTaxesP" runat="server" CssClass="form-control" MaxLength="10" onkeyup="isNumericVal(this);" Text='<%# Eval("PackageInvoiceItem_Tax_Value") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmountQuoted" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Width="100px" TextMode="MultiLine" Text='<%# Eval("PackageInvoiceItem_Remarks") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle Font-Bold="true" BackColor="LightGray" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">

                                    <div class="tabbable">
                                        <ul class="nav nav-tabs" id="myTab">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home" aria-expanded="true">
                                                    <i class="green ace-icon fa fa-home bigger-120"></i>
                                                    Deductions
                                                </a>
                                            </li>

                                            <li class="">
                                                <a data-toggle="tab" href="#messages" aria-expanded="false">Additions
                                                </a>
                                            </li>
                                        </ul>

                                        <div class="tab-content">
                                            <div id="home" class="tab-pane fade active in">
                                                <div style="overflow: auto">
                                                    <asp:GridView ID="grdDeductions" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdDeductions_PreRender" ShowFooter="true" OnRowDataBound="grdDeductions_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PackageInvoiceAdditional_Id" HeaderText="PackageInvoiceAdditional_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PackageInvoiceAdditional_Deduction_isFlat" HeaderText="PackageInvoiceAdditional_Deduction_isFlat">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Deduction_Type" HeaderText="Deduction_Type">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Category" DataField="Deduction_Category" />
                                                            <asp:BoundField HeaderText="Deduction" DataField="Deduction_Name" />
                                                            <asp:BoundField HeaderText="Deduction Type" DataField="Deduction_Type" />
                                                            <asp:TemplateField HeaderText="Deduction Value (%)">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkFlat" AutoPostBack="true" OnCheckedChanged="chkFlat_CheckedChanged" ToolTip="Select This Checkbox for Flat Value" runat="server" />

                                                                    <asp:TextBox ID="txtDeductionValue" runat="server" CssClass="form-control" Width="120px" Text='<%# Eval("Deduction_Value") %>' OnTextChanged="txtDeductionValue_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deduction Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDeductionAmount" runat="server" CssClass="form-control" Width="120px" Text='<%# Eval("PackageInvoiceAdditional_Deduction_Value_Final") %>' AutoPostBack="true" OnTextChanged="txtDeductionAmount_TextChanged"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comments">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDeductionComments" runat="server" CssClass="form-control" TextMode="MultiLine" Text='<%# Eval("PackageInvoiceAdditional_Comments") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle Font-Bold="true" BackColor="LightGray" />
                                                    </asp:GridView>
                                                </div>
                                            </div>

                                            <div id="messages" class="tab-pane fade">
                                                <div style="overflow: auto">
                                                    <asp:GridView ID="grdDeductions2" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdDeductions2_PreRender" ShowFooter="true" OnRowDataBound="grdDeductions2_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PackageInvoiceAdditional_Id" HeaderText="PackageInvoiceAdditional_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PackageInvoiceAdditional_Deduction_isFlat" HeaderText="PackageInvoiceAdditional_Deduction_isFlat">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Deduction_Type" HeaderText="Deduction_Type">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect2" AutoPostBack="true" OnCheckedChanged="chkSelect2_CheckedChanged" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Category" DataField="Deduction_Category" />
                                                            <asp:BoundField HeaderText="Deduction" DataField="Deduction_Name" />
                                                            <asp:BoundField HeaderText="Deduction Type" DataField="Deduction_Type" />
                                                            <asp:TemplateField HeaderText="Deduction Value (%)">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkFlat2" AutoPostBack="true" OnCheckedChanged="chkFlat2_CheckedChanged" ToolTip="Select This Checkbox for Flat Value" runat="server" />

                                                                    <asp:TextBox ID="txtDeductionValue2" runat="server" CssClass="form-control" Width="120px" Text='<%# Eval("Deduction_Value") %>' OnTextChanged="txtDeductionValue2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deduction Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDeductionAmount2" runat="server" CssClass="form-control" Width="120px" Text='<%# Eval("PackageInvoiceAdditional_Deduction_Value_Final") %>' AutoPostBack="true" OnTextChanged="txtDeductionAmount2_TextChanged"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comments">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDeductionComments2" runat="server" CssClass="form-control" TextMode="MultiLine" Text='<%# Eval("PackageInvoiceAdditional_Comments") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle Font-Bold="true" BackColor="LightGray" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Comments and Approvals
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Status</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Total Amount (Invoice Total - Deductions + Additions)</label>
                                            <br />
                                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="label label-xlg label-inverse arrowed arrowed-right"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" runat="server" visible="false" id="divAmountTransfered">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Total Amount Transfered</label>
                                            <asp:TextBox ID="txtFudTransfered" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <hr />
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-sm-12">

                                    <div class="tabbable">
                                        <ul class="nav nav-tabs" id="myTab2">
                                            <li class="active">
                                                <a data-toggle="tab" href="#doc1" aria-expanded="true">
                                                    <i class="green ace-icon fa fa-file-pdf-o"></i>
                                                    Upload Document (If Any)
                                                </a>
                                            </li>

                                            <li class="">
                                                <a data-toggle="tab" href="#doc2" aria-expanded="false">
                                                    <i class="green ace-icon fa fa-file-pdf-o"></i>
                                                    Download / View Previous Uploaded Document
                                                </a>
                                            </li>
                                        </ul>

                                        <div class="tab-content">
                                            <div id="doc1" class="tab-pane fade active in">
                                                <asp:GridView ID="grdDocumentMaster" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Documents Configured To Upload" OnPreRender="grdDocumentMaster_PreRender" OnRowDataBound="grdDocumentMaster_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="TradeDocument_Id" HeaderText="TradeDocument_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProcessConfigDocumentLinking_DocumentMaster_Id" HeaderText="ProcessConfigDocumentLinking_DocumentMaster_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="S No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Document To Upload" DataField="TradeDocument_Name" />
                                                        <asp:TemplateField HeaderText="Order No">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDocumentOrderNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select PDF File To Attach">
                                                            <ItemTemplate>
                                                                <asp:FileUpload ID="flUpload" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDocumentComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>

                                            <div id="doc2" class="tab-pane fade">
                                                <div style="overflow: auto">
                                                    <asp:GridView ID="grdMultipleFiles" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="grdMultipleFiles_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="PackageInvoiceDocs_FileName" HeaderText="PackageInvoiceDocs_FileName">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="S No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="File Name" DataField="TradeDocument_Name" />
                                                            <asp:BoundField HeaderText="Order No" DataField="PackageInvoiceDocs_OrderNo" />
                                                            <asp:BoundField HeaderText="Comments" DataField="PackageInvoiceDocs_Comments" />
                                                            <asp:TemplateField HeaderText="Download">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" PersonFiles_FilePath='<%#Eval("PackageInvoiceDocs_FileName") %>' OnClientClick="return downloadFile(this);"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Reason / Comments</label>
                                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnGenerateInvoice" Text="Update Invoice" OnClick="btnGenerateInvoice_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                            &nbsp; 
                                            <asp:Button ID="btnApproveInvoice" Text="Approve Invoice" OnClick="btnApproveInvoice_Click" runat="server" CssClass="btn btn-danger"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" Style="display: none; width: 930px; margin-left: -32px">
                            <div class="row">
                                <div class="col-md-12">
                                    <iframe style="width: 910px; height: 600px;" id="Iframe1" src="BillPaymentSummeryView.aspx" runat="server"></iframe>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnclose1" Text="Close" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup1" Style="display: none; width: 930px; margin-left: -32px">
                            <div class="row">
                                <div class="col-md-12">
                                    <iframe style="width: 910px; height: 600px;" id="ifrm1" src="BillCoverLetterView.aspx" runat="server"></iframe>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnclose2" Text="Close" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        
                    </div>
                    <asp:HiddenField ID="hf_Invoice_Id" runat="server" Value="0" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenerateInvoice" />
                    <asp:PostBackTrigger ControlID="btnApproveInvoice" />
                </Triggers>
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

    <script>
        function downloadFile(obj) {
            var PersonFiles_FilePath;
            PersonFiles_FilePath = obj.attributes.PersonFiles_FilePath.nodeValue;
            window.open(window.location.origin + PersonFiles_FilePath, "_blank", "", false);
            //location.href = window.location.origin + PersonFiles_FilePath;
            return false;
        }
    </script>
</asp:Content>





