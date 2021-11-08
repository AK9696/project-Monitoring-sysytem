<%@ Page Title="" Language="C#" MasterPageFile="~/TemplateMasterAdmin.master" AutoEventWireup="true" CodeFile="ProjectDPR_Detail.aspx.cs" Inherits="ProjectDPR_Detail" %>

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

                    <div class="page-content">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-header">
                                    Project DPR Details
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
                                        <asp:GridView ID="grdPost" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="District" DataField="Jurisdiction_Name_Eng" />
                                                <asp:BoundField HeaderText="Selected ULB" DataField="ULB_Name" />
                                                <asp:BoundField HeaderText="Project" DataField="ProjectWork_Name" />
                                                <asp:BoundField HeaderText="Refrence No" DataField="ProjectDPR_RefrenceNo" />
                                                <asp:BoundField HeaderText="Lok Sabha" DataField="LokSabha" />
                                                <asp:BoundField HeaderText="Vidhan Sabha" DataField="VidhanSabha" />
                                                <asp:BoundField HeaderText="Project Type" DataField="ProjectType_Name" />
                                                <asp:BoundField HeaderText="परियोजना पूरी करने की अवधि (Target)" DataField="ProjectWork_Target_Date" />
                                                <asp:BoundField HeaderText="Instruction By Competent Authority (Designation)" DataField="Designation_DesignationName" />
                                                <asp:BoundField HeaderText="Competent Authority Name" DataField="ProjectDPRAdditionalInfo_CompetentAuthorityName" />
                                                <asp:BoundField HeaderText="Contact No" DataField="ProjectDPRAdditionalInfo_RecomendatorMobile" />
                                                <asp:BoundField HeaderText="Recomendation By (Authority / Designation)" DataField="ProjectDPRAdditionalInfo_Designation" />
                                                <asp:BoundField HeaderText="संस्तुतिकर्ता का नाम (Name Of Recomendator)" DataField="ProjectDPRAdditionalInfo_RecomendatorName" />
                                                <asp:BoundField HeaderText="Comments" DataField="ProjectDPR_Comments" />

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            Upload DPR
                                        </div>

                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdUploadDocument" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="grdUploadDocument_grdUploadDocument">
                                                <Columns>
                                                    <asp:BoundField DataField="ProjectDPR_FilePath1" HeaderText="ProjectDPR_FilePath1">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectDPR_File1" HeaderText="ProjectDPR_File1">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProjectDPR_File2" HeaderText="ProjectDPR_File2">
                                                        <HeaderStyle CssClass="displayStyle" />
                                                        <ItemStyle CssClass="displayStyle" />
                                                        <FooterStyle CssClass="displayStyle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Upload Date" DataField="ProjectDPR_UploadedOn" />
                                                    <asp:BoundField HeaderText="Comments (If Any)" DataField="ProjectDPR_Upload_Comments" />
                                                    <asp:TemplateField HeaderText="Download DPR">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAgreementUpload2_FilePath" runat="server" OnClick="lnkAgreementUpload2_FilePath_Click" Text="DPR"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Download File1">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAgreementFile3" runat="server" OnClick="lnkAgreementFile3_Click" Text="File1"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Download File2">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAgreementFile4" runat="server" OnClick="lnkAgreementFile4_Click" Text="File2"></asp:LinkButton>
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
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            DPR Verified History
                                        </div>

                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdVerifiedStatus" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Date" DataField="ProjectDPRStatus_Date" />
                                                    <asp:BoundField HeaderText="Status" DataField="DPR_Status_DPR_StatusName" />
                                                    <asp:BoundField HeaderText="Comments" DataField="ProjectDPRStatus_Comments" />

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
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            DPR Questionnaire
                                        </div>

                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="dgvQuestionnaire" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="DPR RQuestionnaire" DataField="DPRQuestionnaire_Name" />
                                                    <asp:BoundField HeaderText="DPR Answer" DataField="ProjectDPRQuestionire_Answer" />
                                                    <asp:BoundField HeaderText="DPR Remark" DataField="ProjectDPRQuestionire_Remark" />
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
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-header">
                                            DPR Approval
                                        </div>

                                        <!-- div.dataTables_borderWrap -->
                                        <div style="overflow: auto">
                                            <asp:GridView ID="grdApproval" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="grdApproval_RowDataBound">
                                                <Columns>
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
                                                    <asp:BoundField HeaderText="Received At HQ" DataField="ProjectDPR_ReceivedAtHQDate" />
                                                    <asp:BoundField HeaderText="Approval Date" DataField="ProjectDPR_ApprovedOn" />
                                                    <asp:BoundField HeaderText="Approved Allocated Budget (In Lakhs)" DataField="ProjectDPR_BudgetAllocated" />
                                                    <asp:BoundField HeaderText="GO Date" DataField="ProjectWork_GO_Date" />
                                                    <asp:BoundField HeaderText="GO Number" DataField="ProjectWork_GO_No" />
                                                    <asp:BoundField HeaderText="Physical Progress Tracking Type" DataField="ProjectDPR_PhysicalProgressTrackingType" />
                                                    <asp:BoundField HeaderText="Comments" DataField="ProjectDPR_BudgetAllocationComments" />
                                                    <asp:TemplateField HeaderText="Download">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAgreementFile2" runat="server" OnClick="lnkAgreementFile2_Click" Text="GO"></asp:LinkButton>
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
                        <div id="divExtendedTracking" runat="server" visible="false">
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
                                                        <asp:TemplateField HeaderText="S No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PhysicalProgressComponent_Component" HeaderText="Component" />
                                                        <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
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
                                                        <asp:TemplateField HeaderText="S No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Deliverables_Deliverables" HeaderText="Deliverables" />
                                                        <asp:BoundField DataField="Unit_Name" HeaderText="Unit" />
                                                    </Columns>
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </br>
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
                                            <asp:Button runat="server" ID="tbnpopupClose" CssClass="btn btn-warning" Text="Close" OnClick="tbnpopupClose_Click" />
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
</asp:Content>

