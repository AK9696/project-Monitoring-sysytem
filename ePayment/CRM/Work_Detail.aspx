<%@ Page Language="C#" MasterPageFile="~/TemplatePopup.master" AutoEventWireup="true" CodeFile="Work_Detail.aspx.cs" Inherits="Work_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="main-content" onload="javascript:print();">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnShowPopup"
                        CancelControlID="btnclose" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>
                    <asp:Button ID="btnShowPopup" Text="Show" runat="server" Style="display: none;"></asp:Button>

                    <cc1:ModalPopupExtender ID="mp2" runat="server" PopupControlID="Panel2" TargetControlID="btnShowPopup"
                        CancelControlID="btnclose2" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>

                    <cc1:ModalPopupExtender ID="mp3" runat="server" PopupControlID="Panel3" TargetControlID="btnShowPopup"
                        CancelControlID="btnclose3" BackgroundCssClass="modalBackground1">
                    </cc1:ModalPopupExtender>

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
                                        <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdPost_PreRender" OnRowDataBound="grdPost_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ProjectDPR_Id" HeaderText="ProjectDPR_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectWorkPkg_Id" HeaderText="ProjectWorkPkg_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectDPR_DPRPDFPath" HeaderText="ProjectDPR_DPRPDFPath">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectWork_GO_Path" HeaderText="ProjectWork_GO_Path">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                <asp:BoundField HeaderText="Project" DataField="Project_Name" />
                                                <asp:BoundField HeaderText="Work Code" DataField="ProjectWork_ProjectCode" />
                                                <asp:BoundField HeaderText="Work" DataField="ProjectWork_Name" />
                                                <asp:BoundField HeaderText="Package Code" DataField="ProjectWorkPkg_Code" />
                                                <asp:BoundField HeaderText="Package Name" DataField="ProjectWorkPkg_Name" />
                                                <asp:BoundField HeaderText="GO No" DataField="ProjectWork_GO_No" />
                                                <asp:BoundField HeaderText="GO Date" DataField="ProjectWork_GO_Date" />
                                                <asp:BoundField HeaderText="स्वीकृत धनराशी (लाख में)" DataField="ProjectDPR_BudgetAllocated" />
                                                <asp:BoundField HeaderText="कुल निर्गत धनराशी (लाख में)" DataField="Total_Release" />
                                                <asp:BoundField HeaderText="कुल व्यय धनराशी (लाख में)" DataField="Total_Expenditure" />
                                                <asp:BoundField HeaderText="कुल अवशेष धनराशी (लाख में)" DataField="Total_Available" />
                                                <asp:BoundField HeaderText="कार्यदायी संस्था" DataField="Vendor_Name" />
                                                <asp:TemplateField HeaderText="Download">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAgreementUpload2_FilePath" runat="server" OnClick="lnkAgreementUpload2_FilePath_Click" Text="DPR"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp; 
                                                             <asp:LinkButton ID="lnkAgreementFile2" runat="server" OnClick="lnkAgreementFile2_Click" Text="GO"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View DPR">
                                                    <ItemTemplate>
                                                        <img src="assets/images/progress.png" width="30px" height="30px" onclick="openPage(this);" projectdpr_id='<%#Eval("ProjectDPR_Id") %>' style="cursor: pointer;" />
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
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Fund Release and Expenditure History
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdCallProductDtls" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdCallProductDtls_PreRender" OnRowDataBound="grdCallProductDtls_RowDataBound" ShowFooter="true">
                                                <Columns>
                                                    <asp:BoundField DataField="FinancialTrans_Id" HeaderText="FinancialTrans_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FinancialTrans_FilePath1" HeaderText="FinancialTrans_FilePath1">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Date" DataField="FinancialTrans_Date" />
                                                    <asp:BoundField HeaderText="Transaction Type" DataField="FinancialTrans_EntryType" />
                                                    <asp:BoundField HeaderText="GO Date" DataField="FinancialTrans_GO_Date" />
                                                    <asp:BoundField HeaderText="GO Number" DataField="FinancialTrans_GO_Number" />
                                                    <asp:BoundField HeaderText="Narration / Comments" DataField="FinancialTrans_Comments" />
                                                    <asp:BoundField HeaderText="निर्गत स्वीकृति (लाख में)" DataField="TransAmount_C" />
                                                    <asp:BoundField HeaderText="कुल व्यय (लाख में)" DataField="TransAmount_D" />
                                                    <asp:BoundField HeaderText="अवशेष धनराशी (लाख में)" DataField="" />
                                                    <asp:TemplateField HeaderText="Download">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkGO" runat="server" OnClick="lnkGO_Click" Text="GO"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#666666" ForeColor="White" Font-Bold="true" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
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
                                            <asp:GridView ID="grdSaveVisit" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdSaveVisit_PreRender" OnRowDataBound="grdSaveVisit_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ProjectDPR_Id" HeaderText="ProjectDPR_Id">
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
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                    <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                    <asp:BoundField HeaderText="UC Submitted Date" DataField="ProjectUC_SubmitionDate1" />
                                                    <asp:BoundField HeaderText="Physical Progress Tracking Type" DataField="ProjectDPR_PhysicalProgressTrackingType" />
                                                    <asp:BoundField HeaderText="Total Allocation" DataField="ProjectUC_Total_Allocated" />
                                                    <asp:BoundField HeaderText="Budget Utilized" DataField="ProjectUC_BudgetUtilized" />
                                                    <asp:BoundField HeaderText="Achivment %" DataField="ProjectUC_Achivment" />
                                                    <%--<asp:BoundField HeaderText="Physical Progress" DataField="ProjectUC_PhysicalProgress" />--%>
                                                    <asp:TemplateField HeaderText="Physical Progress">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPhysicalProgress" runat="server" Text='<%# Eval("ProjectUC_PhysicalProgress") %>' OnClick="lnkPhysicalProgress_Click"></asp:LinkButton>
                                                            <asp:Label runat="server" ID="lblPhysicalProgress" Text='<%# Eval("ProjectUC_PhysicalProgress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Centage" DataField="ProjectUC_Centage" />
                                                    <asp:TemplateField HeaderText="View Inspection Form">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnForm" Width="40px" Height="30px" OnClick="btnForm_Click" ImageUrl="~/assets/images/Data_Entry.png" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Tender Details
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdTender" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdTender_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="ProjectDPRTenderInfo_Id" HeaderText="ProjectDPRTenderInfo_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="NIT Date" DataField="ProjectDPRTenderInfo_NITDate" />
                                                    <asp:BoundField HeaderText="Tender Opening Date" DataField="ProjectDPRTenderInfo_TenderDate" />
                                                    <asp:BoundField HeaderText="Tender Amount Finalized (In Lakhs)" DataField="ProjectDPRTenderInfo_TenderAmount" />
                                                    <asp:BoundField HeaderText="Tender Status" DataField="ProjectDPRTenderInfo_TenderStatus" />
                                                    <asp:BoundField HeaderText="Work Completion Time (in Months)" DataField="ProjectDPRTenderInfo_CompletionTime" />
                                                    <asp:BoundField HeaderText="Centage (%)" DataField="ProjectDPRTenderInfo_Centage" />
                                                    <asp:BoundField HeaderText="Detail of Work Cost Included in Tender (In Lakhs)" DataField="ProjectDPRTenderInfo_WorkCostIn" />
                                                    <asp:BoundField HeaderText="Detail of Work Cost Not Included in Tender (In Lakhs)" DataField="ProjectDPRTenderInfo_WorkCostOut" />
                                                    <asp:BoundField HeaderText="GST (If Not Included in Work Cost)" DataField="ProjectDPRTenderInfo_GSTNotIncludeWorkCost" />
                                                    <asp:BoundField HeaderText="Pre-Bid Meeting Date" DataField="ProjectDPRTenderInfo_PrebidMeetingDate" />
                                                    <asp:BoundField HeaderText="Bid Submission Date (Closing)" DataField="ProjectDPRTenderInfo_TenderOutDate" />
                                                    <asp:BoundField HeaderText="Tender Opening Date (Technical)" DataField="ProjectDPRTenderInfo_TenderTechnicalDate" />
                                                    <asp:BoundField HeaderText="Tender Opening Date (Financial)" DataField="ProjectDPRTenderInfo_TenderFinancialDate" />
                                                    <asp:BoundField HeaderText="Contract Signing Date" DataField="ProjectDPRTenderInfo_ContractSignDate" />
                                                    <asp:BoundField HeaderText="Contract Bond No" DataField="ProjectDPRTenderInfo_ContractBondNo" />
                                                    <asp:BoundField HeaderText="Vendor" DataField="Vendor_Person_Name" />
                                                    <asp:BoundField HeaderText="Comments" DataField="ProjectDPRTenderInfo_Comments" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Verifiying Officers Member Details
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdComiteeDetails" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdComiteeDetails_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="Person_Id" HeaderText="Person_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="User Type" DataField="UserType_Desc_E" />
                                                    <asp:BoundField HeaderText="Comitee Member Name" DataField="Person_Name" />
                                                    <asp:BoundField HeaderText="Mobile No" DataField="Person_Mobile1" />
                                                    <asp:BoundField HeaderText="Altername Mobile No" DataField="Person_Mobile2" />
                                                    <asp:BoundField HeaderText="Designation" DataField="Designation_DesignationName" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Image Gallery
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div runat="server" id="divGallery">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div id="cboxOverlay" style="opacity: 0; cursor: pointer; visibility: visible; display: none;"></div>
                        <div id="colorbox" class="" role="dialog" tabindex="-1" style="display: none; visibility: visible; top: 95px; left: 541px; position: absolute; width: 284px; height: 312px; opacity: 0;">
                            <div id="cboxWrapper" style="height: 312px; width: 284px;">
                                <div>
                                    <div id="cboxTopLeft" style="float: left;"></div>
                                    <div id="cboxTopCenter" style="float: left; width: 204px;"></div>
                                    <div id="cboxTopRight" style="float: left;"></div>
                                </div>
                                <div style="clear: left;">
                                    <div id="cboxMiddleLeft" style="float: left; height: 232px;"></div>
                                    <div id="cboxContent" style="float: left; width: 204px; height: 232px;">
                                        <div id="cboxTitle" style="float: left; display: block;"></div>
                                        <div id="cboxCurrent" style="float: left; display: block;">2 of 8</div>
                                        <button type="button" id="cboxPrevious" style="display: block;"><i class="ace-icon fa fa-arrow-left"></i></button>
                                        <button type="button" id="cboxNext" style="display: block;"><i class="ace-icon fa fa-arrow-right"></i></button>
                                        <button id="cboxSlideshow" style="display: none;"></button>
                                        <div id="cboxLoadingOverlay" style="float: left; display: none;"></div>
                                        <div id="cboxLoadingGraphic" style="float: left; display: none;"><i class="ace-icon fa fa-spinner orange fa-spin"></i></div>
                                        <button type="button" id="cboxClose">×</button>
                                    </div>
                                    <div id="cboxMiddleRight" style="float: left; height: 232px;"></div>
                                </div>
                                <div style="clear: left;">
                                    <div id="cboxBottomLeft" style="float: left;"></div>
                                    <div id="cboxBottomCenter" style="float: left; width: 204px;"></div>
                                    <div id="cboxBottomRight" style="float: left;"></div>
                                </div>
                            </div>
                            <div style="position: absolute; width: 9999px; visibility: hidden; max-width: none; display: none;"></div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="clearfix">
                                    <div class="pull-right tableTools-container"></div>
                                </div>
                                <div class="table-header">
                                    Map View
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div id="map" style="width: 100%; height: 500px"></div>
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

                        <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup1" Style="display: none; width: 950px; margin-left: -32px" Height="700px">

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Inspection Questionire Form
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdQuestionire" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No Records Found" OnPreRender="grdQuestionire_PreRender">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Inspection Question" DataField="ProjectQuestionnaire_Name" />
                                                    <asp:BoundField HeaderText="Answer" DataField="ProjectAnswer_Name" />
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
                                            <button id="btnclose2" runat="server" text="Close" cssclass="btn btn-warning" style="display: none"></button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup1" Style="display: none; width: 950px; margin-left: -32px" Height="700px">

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Physical Progress
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdPhysicalProgress" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PhysicalProgressComponent_Component" HeaderText="Component" />
                                                    <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
                                                    <asp:BoundField DataField="ProjectUC_PhysicalProgress_PhysicalProgress" HeaderText="Progress" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="grdDeliverables" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Deliverables_Deliverables" HeaderText="Deliverables" />
                                                <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
                                                <asp:BoundField DataField="ProjectUC_Deliverables_Deliverables" HeaderText="Deliverables_Value" />
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <button id="Button1" runat="server" text="Close" cssclass="btn btn-warning" style="display: none"></button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                    <asp:HiddenField ID="hf_Map_Data" runat="server" />
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

    <link rel="stylesheet" href="assets/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/4.5.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="assets/css/colorbox.min.css">
    <link rel="stylesheet" href="assets/css/fonts.googleapis.com.css">
    <link rel="stylesheet" href="assets/css/ace.min.css" class="ace-main-stylesheet" id="main-ace-style">
    <link rel="stylesheet" href="assets/css/ace-skins.min.css">
    <link rel="stylesheet" href="assets/css/ace-rtl.min.css">

    <script src="assets/js/jquery-2.1.4.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/jquery.colorbox.min.js"></script>
    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            var $overflow = '';
            var colorbox_params = {
                rel: 'colorbox',
                reposition: true,
                scalePhotos: true,
                scrolling: false,
                previous: '<i class="ace-icon fa fa-arrow-left"></i>',
                next: '<i class="ace-icon fa fa-arrow-right"></i>',
                close: '&times;',
                current: '{current} of {total}',
                maxWidth: '100%',
                maxHeight: '100%',
                onOpen: function () {
                    $overflow = document.body.style.overflow;
                    document.body.style.overflow = 'hidden';
                },
                onClosed: function () {
                    document.body.style.overflow = $overflow;
                },
                onComplete: function () {
                    $.colorbox.resize();
                }
            };
            $('.ace-thumbnails [data-rel="colorbox"]').colorbox(colorbox_params);
            $("#cboxLoadingGraphic").html("<i class='ace-icon fa fa-spinner orange fa-spin'></i>");//let's add a custom loading icon
            $(document).one('ajaxloadstart.page', function (e) {
                $('#colorbox, #cboxOverlay').remove();
            });
        })
    </script>

    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD1KxtLqZtakEaBbSMouMIPl5tDKUz0IqM"></script>
    <script src="Scripts/jquery-1.11.2.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var _lst = [];
            var _lsttemp = [];
            //var latCenter;
            //var lngCenter;
            function getMapData() {
                _lsttemp = eval($("#ctl00_ContentPlaceHolder1_hf_Map_Data").val());
                if (_lsttemp != 0) {
                    for (var i = 0; i < _lsttemp.length; i++) {
                        var _obj = {};
                        _obj.lat = parseFloat(_lsttemp[i].lat);
                        _obj.lng = parseFloat(_lsttemp[i].lng);
                        _obj.description = _lsttemp[i].description;
                        //if (i == 0) {
                        //    latCenter = parseFloat(_lsttemp[i].lat);
                        //    lngCenter = parseFloat(_lsttemp[i].leg);
                        //}
                        _lst.push(_obj);
                    }
                    if (_lst.length > 0)
                        initMap();
                }
            }
            var marker;
            getMapData();
            function initMap() {
                // debugger;
                var a = 0;
                var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
                var labelIndex = 0;
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 18,
                    center: { lat: 26.903031, lng: 80.947200 },
                    mapTypeId: 'terrain'
                });
                var lineSymbol = {
                    path: google.maps.SymbolPath.CIRCLE,
                    scale: 1,
                    strokeColor: '#393'
                };
                var endPoint = {
                    path: 'M17.070 2.93c-3.906-3.906-10.234-3.906-14.141 0-3.906 3.904-3.906 10.238 0 14.14 0.001 0 7.071 6.93 7.071 14.93 0-8 7.070-14.93 7.070-14.93 3.907-3.902 3.907-10.236 0-14.14zM10 14c-2.211 0-4-1.789-4-4s1.789-4 4-4 4 1.789 4 4-1.789 4-4 4z',
                    fillOpacity: 1,
                    scale: 1.5,
                    strokeColor: '#008000',
                    //strokeWeight: 8
                };
                var startPoint = {
                    path: 'M17.070 2.93c-3.906-3.906-10.234-3.906-14.141 0-3.906 3.904-3.906 10.238 0 14.14 0.001 0 7.071 6.93 7.071 14.93 0-8 7.070-14.93 7.070-14.93 3.907-3.902 3.907-10.236 0-14.14zM10 14c-2.211 0-4-1.789-4-4s1.789-4 4-4 4 1.789 4 4-1.789 4-4 4z',
                    fillOpacity: 0.8,
                    scale: 1.5,
                    // strokeWeight: 10
                    strokeColor: '#1e90ff '
                };
                var flightPlanCoordinates = new Array();
                for (var i = 0; i < _lst.length; i++) {
                    flightPlanCoordinates.push('lat: ' + _lst[i].lat + ', lng: ' + _lst[i].lng + '');
                    var marker = new google.maps.Marker({
                        position: _lst[i],
                        map: map,
                        draggable: false,
                        label: labels[labelIndex++ % labels.length],
                        //animation: google.maps.Animation.BOUNCE,
                        //animation: google.maps.Animation.DROP,
                        title: 'Lat: ' + _lst[i].lat + ', Lng: ' + _lst[i].lng + ' Action: ' + _lst[i].description
                    });
                    marker.addListener('click', toggleBounce);
                }
                flightPlanCoordinates = _lst;
                var flightPath = new google.maps.Polyline({
                    path: flightPlanCoordinates,
                    geodesic: true,
                    strokeColor: '#393',
                    strokeOpacity: 1.0,
                    strokeWeight: 1
                });
                flightPath.setMap(map);
                marker = new google.maps.Marker({
                    map: map,
                    draggable: true,
                    animation: google.maps.Animation.DROP,
                    //position: { lat: 59.327, lng: 18.067 }
                });
                marker.addListener('click', toggleBounce);
                var lineSymbol = {
                    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                    scale: 4,
                    strokeColor: '#393'
                };
                var line = new google.maps.Polyline({
                    path: flightPlanCoordinates,
                    icons: [{
                        icon: lineSymbol,
                        offset: '100%'
                    }],
                    map: map
                });
                animateCircle(line);
                function animateCircle(line) {
                    var count = 0;
                    window.setInterval(function () {
                        count = (count + 1) % 200;
                        var icons = line.get('icons');
                        icons[0].offset = (count / 2) + '%';
                        line.set('icons', icons);
                    }, 400);
                }
                function toggleBounce() { };
            };
        })
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
    <script type="text/javascript">
        function openPage(obj) {
            var ProjectDPR_Id;
            ProjectDPR_Id = obj.attributes.projectdpr_id.nodeValue;
            window.open("ProjectDPR_Detail?ProjectDPR_Id=" + ProjectDPR_Id, "_blank", "", false);
        }
    </script>
</asp:Content>
