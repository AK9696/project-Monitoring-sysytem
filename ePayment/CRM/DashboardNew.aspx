<%@ Page Title="" Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="DashboardNew.aspx.cs" Inherits="DashboardNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <div class="page-content">
                        <!-- /.ace-settings-container -->
                        <div class="page-header">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <h1>Dashboard
								<small>
                                    <i class="ace-icon fa fa-angle-double-right"></i>
                                    Overview &amp; Stats
                                </small>
                                    </h1>
                                </div>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Scheme</label>
                                    <asp:DropDownList ID="ddlScheme" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Zone</label>
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Search By</label>
                                    <asp:RadioButtonList ID="rbtSearchBy" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtSearchBy_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="1">Till Date</asp:ListItem>
                                        <asp:ListItem Value="3">Today</asp:ListItem>
                                        <asp:ListItem Value="2">Date Range</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-md-3" id="divFromDate" runat="server" visible="false">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Date From:</label>
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control date-picker" autocomplete="off" data-date-format="dd/mm/yyyy"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3" id="divTillDate" runat="server" visible="false">
                                <div class="form-group">
                                    <label class="control-label no-padding-right">Date Till:</label>
                                    <asp:TextBox ID="txtDateTill" runat="server" CssClass="form-control date-picker" autocomplete="off" data-date-format="dd/mm/yyyy"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <br />
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="btn btn-info" OnClick="btnSearch_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 infobox-container">
                                <div class="infobox infobox-green">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-server"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <span runat="server" id="lblTotalULB" class="infobox-data-number">707</span>
                                        <div class="infobox-content">Total Divisions</div>
                                    </div>

                                </div>

                                <div class="infobox infobox-blue">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-bus"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <span runat="server" id="lblTotalRunningProj" class="infobox-data-number">204</span>
                                        <div class="infobox-content">Running Packages</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-pink">
                                    <%--<div class="infobox-icon">
                                        <i class="ace-icon fa fa-signal"></i>
                                    </div>--%>

                                    <div class="infobox-data">
                                        <span runat="server" id="lblBudgetAllocated" class="infobox-data-number">3,320 Cr.</span>
                                        <div class="infobox-content">Total Budget Allocated</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-red">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-flask"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <span runat="server" id="lblFundAllocated" class="infobox-data-number">310</span>
                                        <div class="infobox-content">Total Release</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-orange2">
                                    <div class="infobox-chart">
                                        <span class="sparkline" data-values="196,128,202,177,154,94,100,170,224">
                                            <canvas width="44" height="33" style="display: inline-block; width: 44px; height: 33px; vertical-align: top;"></canvas>
                                        </span>
                                    </div>

                                    <div class="infobox-data">
                                        <span runat="server" id="lblFundExpences" class="infobox-data-number">2,710 Cr.</span>
                                        <div class="infobox-content">Total Expenditure</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-blue2">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-gamepad"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <span class="infobox-text">UC Submitted</span>

                                        <div class="infobox-content">
                                            <span runat="server" id="lblUC_Count" class="bigger-110"></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="space-6"></div>

                                <div class="infobox infobox-green infobox-small infobox-dark">
                                    <div class="infobox-progress" id="div_Phy_Pro" runat="server">
                                        <div class="easy-pie-chart percentage" data-percent="10" data-size="39" style="height: 39px; width: 39px; line-height: 38px;">
                                            <span class="percent">10</span>%
												<canvas height="48" width="48" style="height: 39px; width: 39px;"></canvas>
                                        </div>
                                    </div>

                                    <div class="infobox-data">
                                        <div class="infobox-content">Physical</div>
                                        <div class="infobox-content">Progress</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-blue infobox-small infobox-dark">
                                    <div class="infobox-chart">
                                        <span class="sparkline" data-values="3,4,2,3,4,4,2,2">
                                            <canvas width="39" height="19" style="display: inline-block; width: 39px; height: 19px; vertical-align: top;"></canvas>
                                        </span>
                                    </div>

                                    <div class="infobox-data">
                                        <div class="infobox-content">Fin. Pro.</div>
                                        <div class="infobox-content">0 Cr.</div>
                                    </div>
                                </div>

                                <div class="infobox infobox-grey infobox-small infobox-dark">
                                    <div class="infobox-icon">
                                        <i class="ace-icon fa fa-download"></i>
                                    </div>

                                    <div class="infobox-data">
                                        <div class="infobox-content">Downloads</div>
                                        <div class="infobox-content">205</div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6" runat="server">
                                <iframe src="Map_UP.aspx" width="100%" height="450px" name="iframeMap" id="iframeMap" frameborder="0" border="0" cellspacing="0"
                                    style="border-style: none; width: 100%; height: 450px"></iframe>
                            </div>
                        </div>
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
</asp:Content>

