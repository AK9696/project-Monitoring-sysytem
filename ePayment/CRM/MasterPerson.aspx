<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="MasterPerson.aspx.cs" Inherits="MasterPerson" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <div class="page-content">

                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Scheme </label>
                                    <asp:DropDownList ID="ddlSearchScheme" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="clearfix">
                                    <div class="form-group">
                                        <asp:Label ID="Label1" runat="server" Text="Zone" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlZoneS" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlZoneS_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="clearfix">
                                    <div class="form-group">
                                        <asp:Label ID="Label2" runat="server" Text="Circle" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlCircleS" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCircleS_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="clearfix">
                                    <div class="form-group">
                                        <asp:Label ID="Label3" runat="server" Text="Division" CssClass="control-label no-padding-right"></asp:Label>
                                        <asp:DropDownList ID="ddlDivisionS" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDivisionS_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label13" runat="server" Text="Designation" CssClass="control-label no-padding-right"></asp:Label>
                                    <asp:ListBox ID="lbDesignationS" runat="server" SelectionMode="Multiple" class="chosen-select form-control" data-placeholder="Choose a Designation..."></asp:ListBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Search (MobileNo / Name)</label>
                                    <asp:TextBox ID="txtSearchMobile" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="clearfix pull-right">
                                    <br />
                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <div class="clearfix">
                                    <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Create New" CssClass="btn btn-warning"></asp:Button>
                                </div>
                            </div>
                            
                        </div>


                        <div id="divCreateNew" runat="server" visible="false">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="table-header">
                                        Create / Update Employee
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Scheme* </label>
                                            <asp:DropDownList ID="ddlProjectMaster" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">
                                                Employee Name  
                                    <span style="color: red; font-weight: bold;">*</span></label>
                                            <asp:TextBox ID="txtPersonName" runat="server" CssClass="form-control" onfocusout="checkDataFilled(this);"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Father's Name</label>
                                            <asp:TextBox ID="txtPersonFName" runat="server" CssClass="form-control" onfocusout="checkDataFilled(this);"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Mobile No*</label>
                                            <asp:TextBox ID="txtMobileNo1" runat="server" CssClass="form-control" MaxLength="10" onkeyup="isNumericVal(this);" onfocusout="checkDataFilled(this);"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Altername Mobile No</label>
                                            <asp:TextBox ID="txtMobileNo2" runat="server" CssClass="form-control" MaxLength="10" onkeyup="isNumericVal(this);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Land Line No (If Any)</label>
                                            <asp:TextBox ID="txtLandLine" runat="server" CssClass="form-control" MaxLength="11" onkeyup="isNumericVal(this);"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">E-Mail Id</label>
                                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Address</label>
                                            <asp:TextBox ID="txtAddress" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Organization*</label>
                                            <asp:DropDownList ID="ddlBranchOffice" runat="server" CssClass="form-control" onfocusout="checkDataFilledDDL(this);"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12">

                                    <div class="table-header">
                                        Posting Details
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Deparement*</label>
                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Designation*</label>
                                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--<div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">User Type*</label>
                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Level*</label>
                                            <asp:DropDownList ID="ddlLevel" AutoPostBack="true" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Reporting Manager</label>
                                            <asp:DropDownList ID="ddlReportingManager" runat="server" class="chosen-select form-control" data-placeholder="Choose a Reporting Manager..."></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblState" runat="server" Text="State" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblMandal" runat="server" Text="Mandal" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlMandal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMandal_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblDistrict" runat="server" Text="District" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlDistrict" runat="server" class="chosen-select form-control" data-placeholder="Choose a District..." AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblBlock" runat="server" Text="Block" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlBlock" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">User Type*</label>
                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="divExpert" runat="server" visible="false">
                                        <div class="form-group">
                                            <asp:Label ID="Label4" runat="server" Text="Assign Zones for Expert*" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:ListBox ID="lbZone" runat="server" SelectionMode="Multiple" class="chosen-select form-control" data-placeholder="Choose a Zone..."></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-xs-12">

                                    <div class="table-header">
                                        Additional Division
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                          <label class="control-label no-padding-right">Designation </label>
                                            <asp:DropDownList ID="ddlAdditionDesignation" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Zone" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlAdditionZone" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdditionZone_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                           <asp:Label ID="Label6" runat="server" Text="Circle" CssClass="control-label no-padding-right"></asp:Label>
                                            <asp:DropDownList ID="ddlAdditionalCircle" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdditionalCircle_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                     <div class="col-md-3">
                                        <div class="form-group">
                                          <label class="control-label no-padding-right">Division </label>
                                            <asp:DropDownList ID="ddlAdditionalDivision" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                     <div class="col-md-12">
                                        <div class="form-group pull-right">
                                            <br />
                                            <asp:Button ID="btnAddAdditionalDivision" OnClick="btnAddAdditionalDivision_Click" Text="Add" runat="server" CssClass="btn btn-info"></asp:Button>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div style="overflow: auto">
                                        <asp:GridView ID="dgvAdditionalDivision" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                            <Columns>
                                                <asp:BoundField DataField="PersonAdditionalArea_Designation_Id" HeaderText="PersonAdditionalArea_Designation_Id">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PersonAdditionalArea_ZoneId" HeaderText="PersonAdditionalArea_ZoneId">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PersonAdditionalArea_CircleId" HeaderText="PersonAdditionalArea_CircleId">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="PersonAdditionalArea_DivisionId" HeaderText="PersonAdditionalArea_DivisionId">
                                                    <HeaderStyle CssClass="displayStyle" />
                                                    <ItemStyle CssClass="displayStyle" />
                                                    <FooterStyle CssClass="displayStyle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Designation" DataField="Designation_Name" />
                                                <asp:BoundField HeaderText="Zone" DataField="Zone_Name" />
                                                <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                 <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkDeleteAdditionalDivision" runat="server" OnClick="lnkDeleteAdditionalDivision_Click" ImageUrl="~/assets/images/delete.png" Width="20px" Height="20px"></asp:ImageButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row" runat="server" id="divLoginDetails1">
                                <div class="col-xs-12">

                                    <div class="table-header">
                                        Login Details
                                    </div>

                                </div>
                            </div>

                            <div class="row" runat="server" id="divLoginDetails2">
                                <div class="col-sm-12">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">User Name* </label>
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Password* </label>
                                            <asp:TextBox ID="txtPassowrd" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label no-padding-right">Confirm Password* </label>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" Text="Save" OnClick="btnSave_Click" runat="server" CssClass="btn btn-info"></asp:Button>
                                            &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" CssClass="btn"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">

                                        <h3 class="header smaller lighter blue">Person Master</h3>
                                        <div class="clearfix" id="dtOptions" runat="server">
                                            <div class="pull-right tableTools-container"></div>
                                        </div>
                                        <!-- div.table-responsive -->
                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" HorizontalAlign="Left" EmptyDataText="No Records Found" OnPreRender="grdPost_PreRender" OnRowDataBound="grdPost_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Person_Id" HeaderText="Person_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PersonJuridiction_Id" HeaderText="PersonJuridiction_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="M_Level_Id" HeaderText="M_Level_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="M_Jurisdiction_Id" HeaderText="M_Jurisdiction_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PersonJuridiction_DesignationId" HeaderText="PersonJuridiction_DesignationId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PersonJuridiction_DepartmentId" HeaderText="PersonJuridiction_DepartmentId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PersonJuridiction_UserTypeId" HeaderText="PersonJuridiction_UserTypeId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PersonJuridiction_ParentPerson_Id" HeaderText="PersonJuridiction_ParentPerson_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="PersonJuridiction_ZoneId" DataField="PersonJuridiction_ZoneId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="PersonJuridiction_CircleId" DataField="PersonJuridiction_CircleId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="PersonJuridiction_DivisionId" DataField="PersonJuridiction_DivisionId">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Person_BranchOffice_Id" HeaderText="Person_BranchOffice_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="List_Role" DataField="List_Role">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="PersonJuridiction_ULB_Id" DataField="PersonJuridiction_ULB_Id">
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
                                                            <asp:ImageButton ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click" ImageUrl="~/assets/images/edit.png" Width="20px" Height="20px"></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Employee Name" DataField="Person_Name" />
                                                    <asp:BoundField HeaderText="Father Name" DataField="Person_FName">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Land Line" DataField="Person_TelePhone">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Mobile" DataField="Person_Mobile1" />
                                                    <asp:BoundField HeaderText="Alternate Mobile" DataField="Person_Mobile2">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Full Address" DataField="Person_AddressLine1">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="EmailId" DataField="Person_EmailId" />
                                                    <asp:BoundField HeaderText="Department" DataField="Department_Name" />
                                                    <asp:BoundField HeaderText="Designation" DataField="Designation_DesignationName" />
                                                    <asp:BoundField HeaderText="Reporting Manager" DataField="Reporting_Manager_Name">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="District" DataField="District" />
                                                    <asp:BoundField HeaderText="Zone" DataField="Zone_Name" />
                                                    <asp:BoundField HeaderText="Circle" DataField="Circle_Name" />
                                                    <asp:BoundField HeaderText="Division" DataField="Division_Name" />
                                                    <asp:BoundField HeaderText="User Name" DataField="Login_UserName" />
                                                    <asp:BoundField HeaderText="List_AdditionZone" DataField="List_AdditionZone">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="PersonJuridiction_Project_Id" DataField="PersonJuridiction_Project_Id">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                                    <asp:BoundField DataField="Created_Date" HeaderText="Created Date" />
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnDelete" Width="20px" Height="30px" OnClick="btnDelete_Click" ImageUrl="~/assets/images/delete.png" runat="server" />
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

                    <asp:HiddenField ID="hf_Person_Id" runat="server" Value="0" />
                    <asp:HiddenField ID="hf_PersonJuridiction_Id" runat="server" Value="0" />
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
                                        //{ "bSortable": false },
                                        null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
                                        //{ "bSortable": false }
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
                                    //columns: ':not(:first):not(:last)',
                                    columns: ':gt(9)',
                                    //columns: '1,2,3,4,5'

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

                        //$(document).on('click', '#ctl00_ContentPlaceHolder1_grdPost .dropdown-toggle', function (e) {
                        //    e.stopImmediatePropagation();
                        //    e.stopPropagation();
                        //    e.preventDefault();
                        //});

                        //And for the first simple table, which doesn't have TableTools or dataTables
                        //select/deselect all rows according to table header checkbox
                        //var active_class = 'active';
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
                            //e.preventDefault();
                            $(this).closest('tr').next().toggleClass('open');
                            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
                        });
                    }
                }
            })
        });

    </script>
</asp:Content>
