<%@ Page Title="" Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="MasterCombineInvoice.aspx.cs" Inherits="MasterCombineInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <div class="page-content">
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
                                        <asp:Label ID="Label6" runat="server" Text="RA Bill No" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:TextBox ID="txtRABillNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btSearch" Text="Search" OnClick="btSearch_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="btnShowSpecification" Text="Show Specification" OnClick="btnShowSpecification_Click" runat="server" CssClass="btn btn-primary" Visible="false"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div runat="server" visible="false" id="divData">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <h3 class="header smaller lighter blue">Project Package List</h3>

                                            <!-- div.table-responsive -->
                                            <div class="clearfix" id="dtOptions" runat="server">
                                                <div class="pull-right tableTools-container"></div>
                                            </div>
                                            <!-- div.dataTables_borderWrap -->
                                            <div style="overflow: auto">
                                                <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdPost_PreRender">
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
                                                        <asp:BoundField DataField="PackageEMB_Master_Id" HeaderText="PackageEMB_Master_Id">
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
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkBxHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkBxHeader_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkPost" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="EMB Date" DataField="PackageEMB_Master_Date" />
                                                        <asp:BoundField HeaderText="EMB No" DataField="PackageEMB_Master_VoucherNo" />
                                                        <asp:BoundField HeaderText="RA Bill No" DataField="PackageEMB_Master_RA_BillNo" />
                                                        <asp:BoundField HeaderText="Total Items" DataField="Total_Items" />
                                                        <asp:BoundField HeaderText="Total Approved" DataField="Total_Approved" />
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
                                                        <asp:TemplateField HeaderText="Invoice">
                                                            <ItemTemplate>
                                                                <a href="MasterGenerateInvoice_View.aspx?Package_Id=<%# Eval("ProjectWorkPkg_Id") %>&Invoice_Id=0" target="_blank">View Invoice</a>
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
                        <div id="divEntry" runat="server" visible="false">

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Details Of EMB Items
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdEMB" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="grdEMB_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="PackageEMB_Id" HeaderText="PackageEMB_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_Id" HeaderText="PackageBOQ_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Package_Id" HeaderText="PackageEMB_Package_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Unit_Id" HeaderText="PackageEMB_Unit_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Approval_Id" HeaderText="PackageEMB_Approval_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_AmountEstimated" HeaderText="PackageBOQ_AmountEstimated">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_AmountQuoted" HeaderText="PackageBOQ_AmountQuoted">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Is_Billed" HeaderText="PackageEMB_Is_Billed">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description Of Goods">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSpecification" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageEMB_Specification") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" Enabled="false" Width="60px"></asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQty" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageBOQ_Qty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Qty") %>' Width="80px" MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageBOQ_RateEstimated" HeaderText="Rate Estimated">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_RateQuoted" HeaderText="Rate Quoted" />
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLength" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Length") %>' MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateQty(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Breadth">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBreadth" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Breadth") %>' MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateQty(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Height") %>' MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateQty(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contents or Area / Volume">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtContents" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Contents") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <table class="table table-striped table-bordered table-hover">
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Button ID="btnApprove" Text="Approve" OnClick="btnApprove_Click" runat="server" CssClass="btn btn-xs btn-pink"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Button ID="btnDisApprove" Text="Reject" OnClick="btnDisApprove_Click" runat="server" CssClass="btn btn-xs btn-inverse"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="">
                                                                            <asp:Label ID="lblQtyExtra" runat="server" CssClass="label label-danger arrowed" Font-Bold="true" Text='<%# Eval("PackageEMB_QtyExtra") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageInvoiceItem_Id" HeaderText="PackageInvoiceItem_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Billing Status">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgBilling" Width="20px" Height="20px" ImageUrl="~/assets/images/OK.png" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Unit_Length_Applicable" HeaderText="Unit_Length_Applicable">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Unit_Bredth_Applicable" HeaderText="Unit_Bredth_Applicable">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Unit_Height_Applicable" HeaderText="Unit_Height_Applicable">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Percentage Amount To Be Released">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPercentageToBeReleased" runat="server" CssClass="form-control" MaxLength="3" onkeyup="isNumericVal(this);" Text='<%# Eval("PackageEMB_PercentageToBeReleased") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity Paid Till Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyMax" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PackageBOQ_QtyPaid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageEMB_PackageEMB_Master_Id" HeaderText="PackageEMB_PackageEMB_Master_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_Qty" HeaderText="PackageEMB_Qty">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_GSTType" HeaderText="PackageEMB_GSTType">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_GSTPercenatge" HeaderText="PackageEMB_GSTPercenatge">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageEMB_PackageBOQ_OrderNo" HeaderText="PackageEMB_PackageBOQ_OrderNo">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Taxes / (%)" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                        <HeaderStyle Width="250px" />
                                                        <ItemStyle Width="250px" />
                                                        <ItemTemplate>
                                                            <asp:GridView ID="grdTaxes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" ShowHeader="false" OnRowDataBound="grdTaxes_RowDataBound" Width="250px">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                                        <HeaderStyle CssClass="displayStyle" />
                                                                        <ItemStyle CssClass="displayStyle" />
                                                                        <FooterStyle CssClass="displayStyle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PackageEMB_Tax_Deduction_Id" HeaderText="PackageEMB_Tax_Deduction_Id">
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
                                                                            <asp:TextBox ID="txtTaxesP" runat="server" CssClass="form-control" MaxLength="10" onkeyup="isNumericVal(this);" Text='<%# Eval("PackageEMB_Tax_Value") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Status</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="RA Bill No" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtRA_Bill_No" onkeyup="isNumericVal(this);" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-12">
                                        <span class="label label-danger arrowed">
                                            <i class="ace-icon fa fa-angle-double-right"></i>
                                            MB With Same RA Bill Number will be combined into One Invoice After Approval.
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="space-6"></div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Upload Document Related To Qty Variation (If Any)
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblUnit" runat="server" Text="Approval No" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtApprovalNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label2" runat="server" Text="Approved Date" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtApprovalDate" runat="server" CssClass="form-control date-picker" autocomplete="off" data-date-format="dd/mm/yyyy"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label3" runat="server" Text="Attach Document" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:FileUpload ID="flApprovedDoc" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Comments (if Any) </label>
                                            <asp:TextBox ID="txtContentsMaster" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                              <div class="row" runat="server" id="DivDeductionDetails">
                                <div class="col-sm-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdDeductions" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" >
                                            <Columns>
                                                <asp:BoundField DataField="Deduction_Id" HeaderText="Deduction_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Deduction" DataField="Deduction_Name" />
                                                <asp:TemplateField HeaderText="Deduction Type">
                                                    <ItemTemplate>
                                                        <asp:RadioButtonList ID="rblDeductionType" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="%" Value="Per" />
                                                            <asp:ListItem Text="Flat" Value="Flat" />
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deduction Value">
                                                    <ItemTemplate>
                                                         <asp:TextBox ID="txtDeductionValue" onkeyup="isNumericVal(this);" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle Font-Bold="true" BackColor="LightGray" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="btnGenerateBill" Text="Generate Bill (For Approved EMB)" OnClick="btnGenerateBill_Click" runat="server" CssClass="btn btn-info"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hf_Package_Id" runat="server" Value="0" />
                        <asp:HiddenField ID="hf_PackageEMB_Master_Id" runat="server" Value="0" />
                        <asp:HiddenField ID="hf_ProjectWork_Id" runat="server" Value="0" />
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
                var DataTableLength = $('#ctl00_ContentPlaceHolder1_grdPost').length;
                if (DataTableLength > 0) {
                    var outerHTML = $('#ctl00_ContentPlaceHolder1_grdPost')[0].outerText;
                    if (outerHTML !== "No Records Found") {
                        //initiate dataTables plugin
                        var myTable =
                            $('#ctl00_ContentPlaceHolder1_grdPost')
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
                                        null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
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

                                    "iDisplayLength": 100,
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

                        $(document).on('click', '#ctl00_ContentPlaceHolder1_grdPost .dropdown-toggle', function (e) {
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

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
            jQuery(function ($) {
                $('.modalBackground1').click(function () {
                    var id = $(this).attr('id').replace('_backgroundElement', '');
                    $find(id).hide();
                });
            })
        });

        function calculateQty(obj) {
            debugger;
            var id = obj.id;
            var row = obj.parentNode.parentNode;
            var id1 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtLength';
            var id2 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtBreadth';
            var id3 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtHeight';
            var Qty_Master = 0;
            try {
                Qty_Master = parseFloat(row.childNodes[12].innerText);
            }
            catch (ee) { }

            var unit_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'ddlUnit';
            var comments_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtContents';
            var qty_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtQty';

            var val1 = parseFloat(document.getElementById(id1).value);
            var val2 = parseFloat(document.getElementById(id2).value);
            var val3 = parseFloat(document.getElementById(id3).value);

            if ($.isNumeric(val1)) {
                val1 = val1;
            }
            else {
                val1 = 1;
            }

            if ($.isNumeric(val2)) {
                val2 = val2;
            }
            else {
                val2 = 1;
            }

            if ($.isNumeric(val3)) {
                val3 = val3;
            }
            else {
                val3 = 1;
            }
            var qty_val = parseFloat(document.getElementById(qty_id).value);
            var Qty_Paid_Till = 0;
            try {
                Qty_Paid_Till = parseFloat(row.childNodes[27].innerText) - qty_val;
            }
            catch (ee) { }
            var Qty_Available = Qty_Master - Qty_Paid_Till;

            var ddlunit = document.getElementById(unit_id);
            var unit_text = ddlunit.options[ddlunit.selectedIndex].text;

            //if ((val1 * val2 * val3) > Qty_Available) {
            //    document.getElementById(comments_id).value = '';
            //    document.getElementById(qty_id).value = '';
            //    document.getElementById(id1).value = '';
            //    document.getElementById(id2).value = '';
            //    document.getElementById(id3).value = '';
            //}
            //else {
            //    document.getElementById(comments_id).value = (val1 * val2 * val3) + ' ' + unit_text;
            //    document.getElementById(qty_id).value = (val1 * val2 * val3);
            //}
            document.getElementById(comments_id).value = (val1 * val2 * val3) + ' ' + unit_text;
            document.getElementById(qty_id).value = (val1 * val2 * val3);
        }
    </script>
</asp:Content>

