<%@ Page Title="" Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="ProjectWorkStatus.aspx.cs" Inherits="ProjectWorkStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                     <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnShowPopup"
                        CancelControlID="btnclose" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup" Text="Show" runat="server" Style="display: none;"></asp:Button>
                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">

                                <div class="table-header">
                                    Select A Work Package For Funds Allocation
                                    
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
                                <div class="col-md-3"  runat="server">
                                    <div class="form-group">
                                        <asp:Label ID="Label9" runat="server" Text="Circle" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" runat="server">
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
                                            <h3 class="header smaller lighter blue">Project Work Status Update</h3>

                                            <!-- div.table-responsive -->
                                            <div class="clearfix" id="dtOptions" runat="server">
                                                <div class="pull-right tableTools-container"></div>
                                            </div>
                                            <!-- div.dataTables_borderWrap -->
                                            <div style="overflow: auto">
                                                <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdPost_PreRender" OnRowDataBound="grdPost_RowDataBound">
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
                                                        <asp:BoundField DataField="ProjectDPR_Id" HeaderText="ProjectDPR_Id">
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
                                                          <asp:BoundField DataField="ProjectDPR_DPRPDFPath" HeaderText="ProjectDPR_DPRPDFPath">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectDPR_DocumentDesignPath" HeaderText="ProjectDPR_DocumentDesignPath">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectDPR_SitePic1Path" HeaderText="ProjectDPR_SitePic1Path">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectDPR_SitePic2Path" HeaderText="ProjectDPR_SitePic2Path">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectWorkPkg_Agreement_Path" HeaderText="ProjectWorkPkg_Agreement_Path">
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
                                                        <asp:BoundField HeaderText="District" DataField="Jurisdiction_Name_Eng" />
                                                        <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                        <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                        <asp:BoundField HeaderText="Project" DataField="Project_Name" />
                                                        <asp:BoundField HeaderText="Project Code" DataField="ProjectWork_ProjectCode" />
                                                        <asp:BoundField HeaderText="Work" DataField="ProjectWork_Name" />
                                                        <asp:BoundField HeaderText="Description" DataField="ProjectWork_Description" />
                                                        <asp:BoundField HeaderText="Budget (In Lakhs)" DataField="ProjectWork_Budget" />
                                                         <asp:TemplateField HeaderText="Download DPR File">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDPRFile" runat="server" OnClick="lnkDPRFile_Click" Text="File"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Download Document File">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDocmentFile" runat="server" OnClick="lnkDocmentFile_Click" Text="File"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Download Site Pic 1">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSitePic1" runat="server" OnClick="lnkSitePic1_Click" Text="File"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Download Site Pic 2">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSitePic2" runat="server" OnClick="lnkSitePic2_Click" Text="File"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField HeaderText="DPR_Comments" DataField="ProjectDPR_Comments" />
                                                         <asp:BoundField HeaderText="DPR AddedBy" DataField="DPRAddedBy" />
                                                         <asp:BoundField HeaderText="DPR_AddedOn" DataField="ProjectDPR_AddedOn" />
                                                        <asp:BoundField HeaderText="ReceivedAtHQDate" DataField="ProjectDPR_ReceivedAtHQDate" />
                                                        <asp:BoundField HeaderText="Verified_Comments" DataField="ProjectDPR_Verified_Comments" />
                                                        <asp:BoundField HeaderText="Verified by" DataField="Verifiedby" />
                                                        <asp:BoundField HeaderText="Verified On" DataField="ProjectDPR_VerifiedOn" />
                                                         <asp:BoundField HeaderText="Approved Allocated Budget (In Lakhs)" DataField="ProjectDPR_BudgetAllocated" />
                                                         <asp:TemplateField HeaderText="Download GO">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkGO" runat="server" OnClick="lnkGO_Click" Text="File"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField HeaderText="GO Date" DataField="ProjectWorkPkg_Agreement_Date" />
                                                         <asp:BoundField HeaderText="GO Number" DataField="ProjectWorkPkg_Agreement_No" />
                                                         <asp:BoundField HeaderText="Approved_Comments" DataField="ProjectDPR_Approved_Comments" />
                                                         <asp:BoundField HeaderText="PhysicalProgressTrackingType" DataField="ProjectDPR_PhysicalProgressTrackingType" />
                                                         <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                                                         <asp:BoundField HeaderText="Approved On" DataField="ProjectDPR_ApprovedOn" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divDPRUpload" runat="server" visible="false">
                             <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Utilization Filled
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div style="overflow: auto">
                                                <asp:GridView ID="grdUC" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found"  OnRowDataBound="grdUC_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ProjectWorkPkg_Id" HeaderText="ProjectWorkPkg_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectWork_GO_Path" HeaderText="ProjectWork_GO_Path">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectUC_Id" HeaderText="ProjectUC_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectUC_FilePath1" HeaderText="ProjectUC_FilePath1">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectUC_FilePath2" HeaderText="ProjectUC_FilePath2">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Division_Id" HeaderText="Division_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Circle_Id" HeaderText="Circle_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Zone_Id" HeaderText="Zone_Id">
                                                            <HeaderStyle CssClass="displayStyle" />
                                                            <ItemStyle CssClass="displayStyle" />
                                                            <FooterStyle CssClass="displayStyle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="S No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Zone" DataField="Zone_Name" />
                                                        <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                         <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                        <asp:BoundField HeaderText="UC Submitted Date" DataField="ProjectUC_SubmitionDate1" />
                                                        <asp:BoundField HeaderText="Total Allocation" DataField="ProjectUC_Total_Allocated" />
                                                        <asp:BoundField HeaderText="Budget Utilized" DataField="ProjectUC_BudgetUtilized" />
                                                        <asp:BoundField HeaderText="Achivment %" DataField="ProjectUC_Achivment" />
                                                        <asp:BoundField HeaderText="Physical Progress" DataField="ProjectUC_PhysicalProgress" />
                                                        <asp:TemplateField HeaderText="View UC">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnUC" Width="20px" Height="20px" OnClick="btnUC_Click" ImageUrl="~/assets/images/edit.png" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-striped table-bordered table-hover table-responsive">
                                        <tr>
                                            <td>SR</td>
                                            <td></td>
                                            <td>Till Prev. Fin. Year
                                            </td>
                                            <td>Current Fin. Year
                                            </td>
                                            <td>Prev. Month
                                            </td>
                                            <td>Current Month
                                            </td>
                                            <td>Total
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>1</td>
                                            <td>Release (In Lacs)</td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblRealseTillPreFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblReleaseCurrentFinYears" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblReleasePrevMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblReleaseCurrentMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblReleaseTotal" runat="server"></asp:Label></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>2</td>
                                            <td>Work Expenditure Amount (In Lacs)</td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblWorkExpenditureAmtTillPrevFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblWorkExpenditureAmtCurrentFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblWorkExpenditureAmtPrevMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFundUtilized" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblWorkExpenditureAmtTotal" runat="server"></asp:Label></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>3</td>
                                            <td>Centage (In Lacs)</td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblCentageTillPrevFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblCentageCurrentFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblCentagePrevMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFundCentage" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblCentageTotal" runat="server"></asp:Label></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>4</td>
                                            <td>Total Expenditure (In Lacs)</td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblTotalExpenditureTillPrevFinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblTotalExpenditureCurrentinYear" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblTotalExpenditurePrevMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblTotalExpenditureCurrentMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblTotalExpenditureTotal" runat="server"></asp:Label></b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-striped table-bordered table-hover table-responsive">
                                        <tr>
                                            <td>SR</td>
                                            <td></td>
                                            <td>Prev. Total
                                            </td>
                                            <td>Last Month
                                            </td>
                                            <td>Current Month
                                            </td>
                                            <td>Total
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>1</td>
                                            <td>Overalll Progress (In % age)</td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblProgressPrevTotal" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblProgressLastMonth" runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicalProgress" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);" MaxLength="3"></asp:TextBox>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblProgressTotal" runat="server"></asp:Label></b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                           
                            <div id="divExtendedTracking" runat="server" visible="True">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            Physical Progress
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView ID="grdPhysicalProgress" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                        <Columns>
                                                            <asp:BoundField DataField="PhysicalProgressComponent_Id" HeaderText="PhysicalProgressComponent_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="S No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PhysicalProgressComponent_Component" HeaderText="Component" />
                                                            <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
                                                            <asp:BoundField DataField="PhysicalProgressPreTotal" HeaderText="Prev. Total" />
                                                            <asp:BoundField DataField="PhysicalProgressLastMonth" HeaderText="Last Month" />
                                                            <asp:TemplateField HeaderText="Current Month">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPhysicalCompenentCurrentmonth" onfocus="this.select();" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);" Text='<%# Eval("PhysicalProgressCurrentMonth") %>' Width="150px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PhysicalProgressTotal" HeaderText="Total" />
                                                        </Columns>
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </br>
                         <div class="row">
                             <div class="col-xs-12">
                                 <div class="table-header">
                                     Deliverables
                                 </div>
                             </div>
                         </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView ID="grdDeliverables" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                        <Columns>
                                                            <asp:BoundField DataField="Deliverables_Id" HeaderText="Deliverables_Id">
                                                                <HeaderStyle CssClass="displayStyle" />
                                                                <ItemStyle CssClass="displayStyle" />
                                                                <FooterStyle CssClass="displayStyle" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="S No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Deliverables_Deliverables" HeaderText="Deliverables" />
                                                            <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
                                                            <asp:BoundField DataField="DeliverablesPrevTotal" HeaderText="Prev. Total" />
                                                            <asp:BoundField DataField="DeliverablesLastMonth" HeaderText="Last Month" />
                                                            <asp:TemplateField HeaderText="Current Month">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDeliverablesCurrentmonth" onfocus="this.select();" runat="server" CssClass="form-control" onkeyup="isNumericVal(this);" Text='<%# Eval("DeliverablesCurrentMonth") %>' Width="150px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="DeliverablesTotal" HeaderText="Total" />
                                                        </Columns>
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </br>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="col-xs-6">
                                        <div class="table-header">
                                            Site Photo (Before Work)
                                        </div>

                                    </div>
                                    <div class="col-xs-6">
                                        <div class="table-header">
                                            Site Photo (After Work)
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="col-md-6">
                                        <iframe id="frmMultiplePre" src="CommanFileUpload_Multiple1.aspx" runat="server" width="100%"></iframe>
                                    </div>
                                    <div class="col-md-6">
                                        <iframe id="frmMultiplePost" src="CommanFileUpload_Multiple2.aspx" runat="server" width="100%"></iframe>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            Fund Utilization Concent
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="col-xs-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="dgvQuestionnaire" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="dgvQuestionnaire_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ProjectQuestionnaire_ProjectId" HeaderText="ProjectQuestionnaire_ProjectId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectQuestionnaire_Id" HeaderText="ProjectQuestionnaire_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ProjectQuestionnaire_Name" HeaderText="Verifiying Officers Concent Point(s)" />
                                                    <asp:TemplateField HeaderText="Answers">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuestionnaireAnswer" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlQuestionnaireAnswer" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Comments</label>
                                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <br />
                                            &nbsp; &nbsp;
                                        <asp:Button ID="btnUpload" Text="Update Work Status" OnClick="btnUpload_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>
                       <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" Style="display: none; width: 950px; margin-left: -32px" Height="700px">

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Document
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <asp:Literal ID="ltEmbed" runat="server" />
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <button id="btnclose" runat="server" text="Close" cssclass="btn btn-warning" style="display: none"></button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="hf_PackageEMB_Id" runat="server" Value="0" />
                        <asp:HiddenField runat="server" id="hf_PackageEMB_Master_Id" Value="0" />
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
                                        null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
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
</asp:Content>

