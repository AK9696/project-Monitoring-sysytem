<%@ Page Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
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
                                        Change Password
                                    </div>
                                </div>
                            </div>

                        <div class="position-relative">
                            <div class="row">
                     <div class="col-xs-12">
                         <div class="col-xs-3"></div>
                         <div class="col-xs-5">
                        <div  class="signup-box widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <h4 class="header green lighter bigger">
                        <i class="ace-icon fa fa-users blue"></i>
                        Admin Change Password Details
                    </h4>

                    <div class="space-6"></div>
                    <p>Enter your Password Details: </p>

                    <fieldset>

                        <label class="block clearfix">
                            <span class="block input-icon input-icon-right">
                                <asp:TextBox ID="txtOld" runat="server" CssClass="form-control" placeholder="Old Password" TextMode="Password"></asp:TextBox>
                            </span>
                        </label>

                        <label class="block clearfix">
                            <span class="block input-icon input-icon-right">
                                <asp:TextBox ID="txtNewPassowrd" runat="server" CssClass="form-control" placeholder="New Password" TextMode="Password">
                                </asp:TextBox>
                            </span>
                        </label>
                        <label class="block clearfix">
                            <span class="block input-icon input-icon-right">
                                <asp:TextBox ID="txtConfirmPassowrd" runat="server" CssClass="form-control" placeholder="Confirm Password" TextMode="Password">
                                </asp:TextBox>
                            </span>
                        </label>
                        
                        <div class="space-24"></div>

                        <div class="clearfix">
                           
                            <asp:Button ID="btnChange" runat="server" Text="Change Password" CssClass="width-65 pull-right btn btn-sm btn-success"  OnClick="btnChange_Click"/>
                        </div>
                    </fieldset>

                </div>

               
            </div>
            <!-- /.widget-body -->
        </div>
                            </div>
                            </div>
                            </div>
                        </div>
                         <asp:HiddenField ID="hf_Beat_Id" runat="server" Value="0" />
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
</asp:Content>

