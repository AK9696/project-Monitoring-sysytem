<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true"
    CodeFile="MasterGenerateInvoice_View.aspx.cs" Inherits="MasterGenerateInvoice_View" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="mpViewBill" runat="server" PopupControlID="Panel2" TargetControlID="btnShowPopup2"
                        CancelControlID="btnclose2" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup2" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <cc1:ModalPopupExtender ID="mpDeduction" runat="server" PopupControlID="Panel1" TargetControlID="btnShowPopup1"
                        CancelControlID="btnclose1" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup1" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <cc1:ModalPopupExtender ID="mpViewCoverLetter" runat="server" PopupControlID="Panel4" TargetControlID="btnShowPopup2"
                        CancelControlID="btnclose4" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup4" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <cc1:ModalPopupExtender ID="mpViewSummery" runat="server" PopupControlID="Panel3" TargetControlID="btnShowPopup1"
                        CancelControlID="btnclose3" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup3" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Details Of Invoice Items
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Scheme </label>
                                            <asp:DropDownList ID="ddlSearchScheme" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="col-md-3" id="divCircle" runat="server">
                                    <div class="form-group">
                                        <asp:Label ID="Label9" runat="server" Text="Circle" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="divDivision" runat="server">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Division </label>
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btnSearch" Text="Search" OnClick="btnSearch_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdInvoice" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdInvoice_PreRender" OnRowDataBound="grdInvoice_RowDataBound">
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
                                                <asp:BoundField DataField="PackageInvoice_PackageEMBMaster_Id" HeaderText="PackageInvoice_PackageEMBMaster_Id">
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
                                                        <asp:ImageButton ID="btnEdit" Width="20px" Height="20px" OnClick="btnEdit_Click" ImageUrl="~/assets/images/edit.png" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="List_EMBNo" HeaderText="EMB No" />
                                                <asp:BoundField DataField="PackageInvoice_Date" HeaderText="Invoice Date" />
                                                <asp:BoundField DataField="PackageInvoice_VoucherNo" HeaderText="Voucher No" />
                                                <asp:BoundField DataField="PackageInvoice_SBR_No" HeaderText="SBR No" />
                                                <asp:BoundField DataField="PackageInvoice_DBR_No" HeaderText="DBR No" />
                                                <asp:BoundField DataField="Total_Line_Items" HeaderText="Total Line Items" />
                                                <asp:BoundField DataField="PackageInvoiceItem_Total_Qty" HeaderText="Total Qty" />
                                                <asp:BoundField DataField="Total_Amount" HeaderText="Total Invoice Amount" />
                                                <asp:BoundField DataField="FinancialTrans_TransAmount" HeaderText="Total Fund Transfred" />
                                                <asp:BoundField DataField="PackageInvoice_AddedOn" HeaderText="Added On" />
                                                <asp:BoundField DataField="Designation_DesignationName" HeaderText="Pending at Designation" />
                                                <asp:BoundField DataField="OfficeBranch_Name" HeaderText="Pending at Organization" />
                                                <asp:BoundField DataField="Invoice_Status" HeaderText="Current Status" />
                                                <asp:TemplateField HeaderText="View Invoice">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnOpenInvoice" Width="40px" Height="40px" OnClick="btnOpenInvoice_Click" ImageUrl="~/assets/images/print.png" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete Invoice">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnDeleteInvoice" Width="30px" Height="30px" OnClick="btnDeleteInvoice_Click" ImageUrl="~/assets/images/delete.png" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="divHistory" runat="server" visible="false">
                            <div class="space-6"></div>
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
                                                                <asp:BoundField DataField="FundingPattern_Name" HeaderText="Funding Pattern Share" />
                                                                <asp:BoundField DataField="ProjectWorkFundingPattern_Value" HeaderText="Value" />
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
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Details Of EMB Approval History
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdEMBHistory" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdEMBHistory_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="PackageEMBApproval_Id" HeaderText="PackageEMBApproval_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMBApproval_PackageEMBMaster_Id" HeaderText="PackageEMBApproval_PackageEMBMaster_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMBApproval_Package_Id" HeaderText="PackageEMBApproval_Package_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMBApproval_Status_Id" HeaderText="PackageEMBApproval_Status_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageEMBApproval_Date" HeaderText="Approval Date" />
                                                    <asp:BoundField DataField="PackageEMBApproval_Status_Text" HeaderText="Action Taken" />
                                                    <asp:BoundField DataField="PackageEMBApproval_Comments" HeaderText="Comments" />
                                                    <asp:BoundField DataField="Designation_Current" HeaderText="Action By (Designation)" />
                                                    <asp:BoundField DataField="Person_Name" HeaderText="Action By (Name)" />
                                                    <asp:BoundField DataField="Organisation_Next" HeaderText="Next Action (Organisation)" />
                                                    <asp:BoundField DataField="Designation_Next" HeaderText="Next Action (Designation)" />
                                                    <asp:BoundField DataField="PackageEMBApproval_AddedOn" HeaderText="Added On" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Details Of Invoice Approval History
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdInvoiceHistory" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdInvoiceHistory_PreRender" >
                                                <Columns>
                                                    <asp:BoundField DataField="PackageInvoiceApproval_Id" HeaderText="PackageInvoiceApproval_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceApproval_PackageInvoice_Id" HeaderText="PackageInvoiceApproval_PackageInvoice_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceApproval_Package_Id" HeaderText="PackageInvoiceApproval_Package_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageInvoiceApproval_AddedBy" HeaderText="PackageInvoiceApproval_AddedBy">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageInvoiceApproval_Date" HeaderText="Approval Date" />
                                                    <asp:BoundField DataField="PackageInvoiceApproval_Status_Text" HeaderText="Action Taken" />
                                                    <asp:BoundField DataField="PackageInvoiceApproval_Comments" HeaderText="Comments" />
                                                    <asp:BoundField DataField="Designation_Current" HeaderText="Action By (Designation)" />
                                                    <asp:BoundField DataField="Person_Name" HeaderText="Action By (Name)" />
                                                    <asp:BoundField DataField="Organisation_Next" HeaderText="Next Action (Organisation)" />
                                                    <asp:BoundField DataField="Designation_Next" HeaderText="Next Action (Designation)" />
                                                    <asp:BoundField DataField="PackageInvoiceApproval_AddedOn" HeaderText="Added On" />
                                                    <asp:TemplateField HeaderText="Deduction Change History">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDeductionHistory" runat="server" Text='<%# Eval("PackageInvoiceApproval_Deduction") %>' OnClick="lnkDeductionHistory_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Roll Back">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnRollBack" Width="30px" Height="30px" OnClick="btnRollBack_Click" ImageUrl="~/assets/images/delete.png" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Download / View Previous Uploaded Document
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
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
                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" Style="display: none; width: 930px; margin-left: -32px">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdDeductionHistory" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdDeductionHistory_PreRender">
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
                            <div class="row">
                                <div class="col-md-12">
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
                                    <iframe style="width: 910px; height: 600px;" id="ifrm1" src="BillView.aspx" runat="server"></iframe>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                   <%-- <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" />
                                        </div>
                                    </div>--%>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnclose2" Text="Close" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup1" Style="display: none; width: 930px; margin-left: -32px">
                            <div class="row">
                                <div class="col-md-12">
                                    <iframe style="width: 910px; height: 600px;" id="Iframe1" src="BillPaymentSummeryView.aspx" runat="server"></iframe>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:ImageButton ID="btnPrint3" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnclose3" Text="Close" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel4" runat="server" CssClass="modalPopup1" Style="display: none; width: 930px; margin-left: -32px">
                            <div class="row">
                                <div class="col-md-12">
                                    <iframe style="width: 910px; height: 600px;" id="Iframe2" src="BillCoverLetterView.aspx" runat="server"></iframe>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:ImageButton ID="btnPrint4" runat="server" ImageUrl="/assets/images/print.png" Width="50px" Height="60px" OnClientClick=" return Print();" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnclose4" Text="Close" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="hf_Invoice_Id" runat="server" Value="0" />
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

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
            jQuery(function ($) {
                var DataTableLength = $('#ctl00_ContentPlaceHolder1_grdInvoice').length;
                if (DataTableLength > 0) {
                    var outerHTML = $('#ctl00_ContentPlaceHolder1_grdInvoice')[0].outerText;
                    if (outerHTML !== "No Records Found") {
                        //initiate dataTables plugin
                        var myTable =
                            $('#ctl00_ContentPlaceHolder1_grdInvoice')
                                //.wrap("<div class='dataTables_borderWrap' />")   //if you are applying horizontal scrolling (sScrollX)
                                .DataTable({
                                    mark: true,
                                    colReorder: true,
                                    fixedHeader: {
                                        header: true,
                                        footer: false
                                    },
                                    bAutoWidth: false,
                                    "aoColumns": [
                                        { "bSortable": false },
                                        null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null,null,
                                        { "bSortable": false }
                                    ],
                                    "aaSorting": [],
                                    //"bProcessing": true,
                                    //"bServerSide": true,
                                    //"sAjaxSource": "http://127.0.0.1/table.php"	,

                                    //,
                                    //"sScrollY": "200px",
                                    //"bPaginate": false,
                                    //"sScrollX": "100%",
                                    //"sScrollXInner": "120%",
                                    //"bScrollCollapse": true,
                                    //Note: if you are applying horizontal scrolling (sScrollX) on a ".table-bordered"
                                    //you may want to wrap the table inside a "div.dataTables_borderWrap" element

                                    "iDisplayLength": 25,
                                    select: {
                                        style: 'multi'
                                    }
                                });
                        $.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';
                        new $.fn.dataTable.Buttons(myTable, {
                            buttons: [
                                {
                                    "extend": "colvis",
                                    "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
                                    "className": "btn btn-white btn-primary btn-bold",
                                    columns: ':not(:first):not(:last)'
                                },
                                {
                                    "extend": "copy",
                                    "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
                                    "className": "btn btn-white btn-primary btn-bold"
                                },
                                {
                                    "extend": "csv",
                                    "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                                    "className": "btn btn-white btn-primary btn-bold"
                                },
                                {
                                    "extend": "excel",
                                    "text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
                                    "className": "btn btn-white btn-primary btn-bold"
                                },
                                {
                                    "extend": "pdf",
                                    "text": "<i class='fa fa-file-pdf-o bigger-110 red'></i> <span class='hidden'>Export to PDF</span>",
                                    "className": "btn btn-white btn-primary btn-bold"
                                },
                                {
                                    "extend": "print",
                                    "text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
                                    "className": "btn btn-white btn-primary btn-bold",
                                    autoPrint: true,
                                    message: 'This print was produced using the Print button for DataTables',
                                    exportOptions: {
                                        columns: ':visible'
                                    }
                                }
                            ]
                        });
                        myTable.buttons().container().appendTo($('.tableTools-container'));

                        //style the message box
                        var defaultCopyAction = myTable.button(1).action();
                        myTable.button(1).action(function (e, dt, button, config) {
                            defaultCopyAction(e, dt, button, config);
                            $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
                        });
                        var defaultColvisAction = myTable.button(0).action();
                        myTable.button(0).action(function (e, dt, button, config) {

                            defaultColvisAction(e, dt, button, config);
                            if ($('.dt-button-collection > .dropdown-menu').length == 0) {
                                $('.dt-button-collection')
                                    .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
                                    .find('a').attr('href', '#').wrap("<li />")
                            }
                            $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
                        });
                        ////
                        setTimeout(function () {
                            $($('.tableTools-container')).find('a.dt-button').each(function () {
                                var div = $(this).find(' > div').first();
                                if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                                else $(this).tooltip({ container: 'body', title: $(this).text() });
                            });
                        }, 500);

                        $(document).on('click', '#ctl00_ContentPlaceHolder1_grdInvoice .dropdown-toggle', function (e) {
                            e.stopImmediatePropagation();
                            e.stopPropagation();
                            //e.preventDefault();
                        });
                        //And for the first simple table, which doesn't have TableTools or dataTables
                        //select/deselect all rows according to table header checkbox
                        var active_class = 'active';
                        /********************************/
                        //add tooltip for small view action buttons in dropdown menu
                        $('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });

                        //tooltip placement on right or left
                        function tooltip_placement(context, source) {
                            var $source = $(source);
                            var $parent = $source.closest('table')
                            var off1 = $parent.offset();
                            var w1 = $parent.width();

                            var off2 = $source.offset();
                            //var w2 = $source.width();

                            if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
                            return 'left';
                        }
                        /***************/
                        $('.show-details-btn').on('click', function (e) {
                            e.preventDefault();
                            $(this).closest('tr').next().toggleClass('open');
                            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
                        });
                    }
                }
            })
        });

    </script>

    <script type="text/javascript">
        function Print() {
            debugger;
            var frame1 = document.getElementById('ctl00_ContentPlaceHolder1_ifrm1').contentWindow.document.getElementById('dvReport').getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[0].focus();
                window.frames[0].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
            return false;
        }
    </script>
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





