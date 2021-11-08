<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true"
    CodeFile="MasterEMB.aspx.cs" Inherits="MasterEMB" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

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
                                    Select A Work Package TO Create EMB
                                    
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
                                                        <asp:BoundField DataField="ProjectWorkPkg_LastRABillNo" HeaderText="ProjectWorkPkg_LastRABillNo">
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
                                                        <asp:BoundField HeaderText="Total BOQ Count" DataField="Total_BOQ" />
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
                                        <div class="form-group" style="float: right; padding-right: 10px">
                                            <asp:CheckBox ID="chkAbstractOnly" Checked="true" Text="Fill Only Abstract" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdEMB" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdEMB_PreRender" OnRowDataBound="grdEMB_RowDataBound">
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
                                                    <asp:BoundField DataField="PackageBOQ_RateEstimated" HeaderText="PackageBOQ_RateEstimated">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_AmountEstimated" HeaderText="PackageBOQ_AmountEstimated">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_RateQuoted" HeaderText="PackageBOQ_RateQuoted">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_AmountQuoted" HeaderText="PackageBOQ_AmountQuoted">
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
                                                    <asp:BoundField DataField="PackageBOQ_RateEstimated" HeaderText="Rate Estimated">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PackageBOQ_RateQuoted" HeaderText="Rate Quoted" />
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLength" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Length") %>' Width="80px" onchange="return calculateQty(this);" MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Breadth">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBreadth" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Breadth") %>' Width="80px" onchange="return calculateQty(this);" MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Height") %>' Width="80px" onchange="return calculateQty(this);" MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Qty") %>' Width="80px" MaxLength="10" onkeyup="isNumericVal(this);" onchange="return calculateQtyA(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contents or Area / Volume">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtContents" runat="server" CssClass="form-control" Text='<%# Eval("PackageEMB_Contents") %>' TextMode="MultiLine"></asp:TextBox>
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
                                                    <asp:BoundField DataField="PackageEMB_PackageEMB_Master_Id" HeaderText="PackageEMB_PackageEMB_Master_Id">
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
                                                    <asp:TemplateField HeaderText="Percentage Value Paid Till Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPercentageValuePaidTillDate" runat="server" CssClass="control-label no-padding-right" Text='<%# Eval("PercentageValuePaidTillDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PackageEMB_PackageBOQ_OrderNo" HeaderText="PackageEMB_PackageBOQ_OrderNo">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="DivADPItems" visible="false">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            Add Additional Department Payment Item
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdADPItems" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:BoundField DataField="ADP_Item_Id" HeaderText="ADP_Item_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADP_Item_Category_Id" HeaderText="ADP_Item_Category_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ADP_Item_Unit_Id" HeaderText="ADP_Item_Unit_Id">
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
                                                    <asp:BoundField DataField="ADP_Category_Name" HeaderText="Category_Name" />
                                                    <asp:BoundField DataField="ADP_Item_Specification" HeaderText="Specification" />
                                                    <asp:BoundField DataField="Unit_Name" HeaderText="Unit_Name" />
                                                    <asp:TemplateField HeaderText="Rate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRate" onkeyup="isNumericVal(this);" runat="server" CssClass="form-control" Text='<%# Eval("ADP_Item_Rate") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Qty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQty" onkeyup="isNumericVal(this);" runat="server" CssClass="form-control" Text='<%# Eval("ADP_Item_Qty") %>'></asp:TextBox>
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
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="RA Bill No" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtRABillNo" onkeyup="isNumericVal(this);" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="lblDeduction" runat="server" Text="MB Ref. No (Physical)" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtMB_No" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="Label2" runat="server" Text="MB Date" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:TextBox ID="txtMBDate" runat="server" CssClass="form-control date-picker" autocomplete="off" data-date-format="dd/mm/yyyy"></asp:TextBox>
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

                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="btnSaveEMB" Text="Save EMB" OnClick="btnSaveEMB_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="btnSaveAndForward" Text="Save EMB and Forward For Approval" OnClick="btnSaveAndForward_Click" runat="server" CssClass="btn btn-info"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <asp:HiddenField ID="hf_ProjectWork_Id" runat="server" Value="0" />
                        <asp:HiddenField ID="hf_ProjectWorkPkg_LastRABillNo" runat="server" Value="0" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" DynamicLayout="true" runat="server" AssociatedUpdatePanelID="up">
                <ProgressTemplate>
                    <div style="position: fixed; z-index: 999; height: 100%; width: 100%; top: 0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8; cursor: not-allowed;">
                        <div style="z-index: 1000; margin: 300px auto; padding: 10px; width: 130px; background-color: White; border-radius: 10px; filter: alpha(opacity=100); opacity: 1; -moz-opacity: 1;">
                            <img src="assets/images/mb/mbloader.gif" style="height: 100px; width: 100px;" />
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

        //Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
        //    jQuery(function ($) {
        function calculateQty(obj) {
            debugger;
            var id = obj.id;
            var row = obj.parentNode.parentNode;
            var id1 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtLength';
            var id2 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtBreadth';
            var id3 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtHeight';
            var Qty_Master = 0;
            try {
                Qty_Master = parseFloat(row.childNodes[13].innerText);
            }
            catch (ee) { }
            var Qty_Paid_Till = 0;
            try {
                Qty_Paid_Till = parseFloat(row.childNodes[26].innerText);
            }
            catch (ee) { }
            var unit_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'ddlUnit';
            var comments_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtContents';
            var qty_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '') + 'txtQty';

            var Qty_Available = Qty_Master - Qty_Paid_Till;
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

            var ddlunit = document.getElementById(unit_id);
            var unit_text = ddlunit.options[ddlunit.selectedIndex].text;

            if ((val1 * val2 * val3) > Qty_Available) {
                document.getElementById(comments_id).value = '';
                document.getElementById(qty_id).value = '';
                document.getElementById(id1).value = '';
                document.getElementById(id2).value = '';
                document.getElementById(id3).value = '';
            }
            else {
                document.getElementById(comments_id).value = (val1 * val2 * val3) + ' ' + unit_text;
                document.getElementById(qty_id).value = (val1 * val2 * val3);
            }
        }
        //    })
        //});

        function calculateQtyA(obj) {
            debugger;
            if (document.getElementById('ctl00_ContentPlaceHolder1_chkAbstractOnly').checked == true) {
                var id = obj.id;
                var row = obj.parentNode.parentNode;
                var id1 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'txtLength';
                var id2 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'txtBreadth';
                var id3 = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'txtHeight';
                var Qty_Master = 0;
                try {
                    Qty_Master = parseFloat(row.childNodes[13].innerText);
                }
                catch (ee) { }
                var Qty_Paid_Till = 0;
                try {
                    Qty_Paid_Till = parseFloat(row.childNodes[26].innerText);
                }
                catch (ee) { }
                var unit_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'ddlUnit';
                var comments_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'txtContents';
                var qty_id = id.replace('txtLength', '').replace('txtBreadth', '').replace('txtHeight', '').replace('txtQty', '') + 'txtQty';

                var Qty_Available = Qty_Master - Qty_Paid_Till;

                var qty_Current = 0;
                try {
                    qty_Current = parseFloat(obj.value);
                }
                catch
                {
                    qty_Current = 0;
                }
                var ddlunit = document.getElementById(unit_id);
                var unit_text = ddlunit.options[ddlunit.selectedIndex].text;

                //if (qty_Current > Qty_Available) {
                //    document.getElementById(comments_id).value = '';
                //    document.getElementById(qty_id).value = '';
                //    document.getElementById(id1).value = '';
                //    document.getElementById(id2).value = '';
                //    document.getElementById(id3).value = '';
                //}
                //else {
                //    document.getElementById(comments_id).value = qty_Current + ' ' + unit_text;
                //    document.getElementById(qty_id).value = qty_Current;
                //}
                document.getElementById(comments_id).value = qty_Current + ' ' + unit_text;
                document.getElementById(qty_id).value = qty_Current;
            }
        }
    </script>
</asp:Content>

