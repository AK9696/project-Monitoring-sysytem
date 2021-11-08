<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true"
    CodeFile="MasterApprovalHierarchy.aspx.cs" Inherits="MasterApprovalHierarchy" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

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
                                    Create Dynamic Approval Hierarchy
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Organization* </label>
                                        <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:Label ID="lblDistrict" runat="server" Text="Department" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlDepartment" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Designation*</label>
                                        <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <br />
                                        <asp:CheckBox ID="chkCreate" runat="server" CssClass="form-control" Text="Permission Granted For Creation / Addition"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <br />
                                        <asp:CheckBox ID="chkUpdate" runat="server" CssClass="form-control" Text="Permission Granted For Updations / Approval"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Loop*</label>
                                        <asp:DropDownList ID="ddlLoop" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="1" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Permission To Change Status* </label>
                                        <asp:CheckBoxList id="chkInvoiceStatus" CssClass="form-control" CellSpacing="50" CellPadding="10" Font-Bold="true" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                    </div>
                                </div>                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:Button ID="btnAdd" Text="Add Process" OnClick="btnAdd_Click" runat="server" CssClass="btn btn-info"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="clearfix" id="Div1" runat="server">
                                            <div class="pull-right tableTools-container"></div>
                                        </div>
                                        <div class="table-header">
                                            Process Approval Hierarchy
                                        </div>

                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdComiteeMember" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnPreRender="grdComiteeMember_PreRender">
                                                <Columns>
                                                    <asp:BoundField DataField="ProcessConfigMaster_Id" HeaderText="ProcessConfigMaster_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProcessConfigMaster_OrgId" HeaderText="ProcessConfigMaster_OrgId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProcessConfigMaster_Department_Id" HeaderText="ProcessConfigMaster_Department_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProcessConfigMaster_Designation_Id" HeaderText="ProcessConfigMaster_Designation_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProcessConfigInvStatus_InvoiceStatus_Id" HeaderText="ProcessConfigInvStatus_InvoiceStatus_Id">
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
                                                    <asp:BoundField HeaderText="Organization" DataField="Organization_Name" />
                                                    <asp:BoundField HeaderText="Department" DataField="Department_Name" />
                                                    <asp:BoundField HeaderText="Designation" DataField="Designation_Name" />
                                                    <asp:BoundField HeaderText="Allowed For Creation" DataField="ProcessConfigMaster_Creation_Allowed" />
                                                    <asp:BoundField HeaderText="Allowed For Updation / Approval" DataField="ProcessConfigMaster_Updation_Allowed" />
                                                    <asp:BoundField HeaderText="Allowed Status" DataField="InvoiceStatus_Name" />
                                                    <asp:BoundField HeaderText="Loop" DataField="ProcessConfigMaster_Loop" />
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnRemove" Width="20px" Height="20px" OnClick="btnRemove_Click" ImageUrl="~/assets/images/minus-icon.png" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                    </div>
                                </div>
                                <!-- PAGE CONTENT ENDS -->
                            </div>
                            <!-- /.col -->
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label no-padding-right">Comments* </label>
                                        <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <br />
                                        &nbsp; &nbsp;
                                        <asp:Button ID="btnUpload" Text="Save Process" OnClick="btnUpload_Click" runat="server" CssClass="btn btn-warning"></asp:Button>
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



