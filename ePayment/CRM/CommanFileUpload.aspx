<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommanFileUpload.aspx.cs" Inherits="CommanFileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <link rel="stylesheet" href="assets/css/bootstrap.min.css" />
     <link rel="stylesheet" href="assets/font-awesome/4.5.0/css/font-awesome.min.css" />
     <link rel="stylesheet" href="assets/css/ace.min.css" class="ace-main-stylesheet" />
</head>
<body style="background-color:white">
    <form id="form1" runat="server">
        
             
        <div class="row" id="Upload1" runat="server">
            <div class="col-md-12">
                <div class="col-md-12">
                    <div class="form-group">
                        
                        <label class="control-label no-padding-right" runat="server" id="lblname1">Photo Upload</label>
                        <br />
                        <asp:FileUpload ID="flUpload" runat="server"/>
                         <br />
                        <asp:Button runat="server" ID="Upload" Text="Upload" CssClass="btn btn-primary" OnClick="Save_Click"></asp:Button>
                         <br />
                         <label class="control-label no-padding-right" runat="server" id="UploadedFileName"></label>
                    </div>
                
                 </div>
                 </div>
            </div>
        
        <div class="row" id="Upload2" runat="server">
            <div class="col-md-12">
                <div class="col-md-12">
                    <div class="form-group">
                        
                        <label class="control-label no-padding-right" runat="server" id="lblname2">Agreement Upload</label>
                        <br />
                        <asp:FileUpload ID="flUpload2" runat="server"/>
                         <br />
                        <asp:Button runat="server" ID="Button1" Text="Upload" CssClass="btn btn-primary" OnClick="Save1_Click"></asp:Button>
                         <br />
                         <label class="control-label no-padding-right" runat="server" id="UploadedFileName1"></label>
                    </div>
                </div>
                 
                
            </div>
        </div>
        <div class="row" id="Upload3" runat="server">
            <div class="col-md-12">
                <div class="col-md-12">
                    <div class="form-group">
                        
                        <label class="control-label no-padding-right" runat="server" id="lblname3">Agreement Upload</label>
                        <br />
                        <asp:FileUpload ID="FileUpload3" runat="server"/>
                         <br />
                        <asp:Button runat="server" ID="Button3" Text="Upload" CssClass="btn btn-primary" OnClick="Save2_Click"></asp:Button>
                         <br />
                         <label class="control-label no-padding-right" runat="server" id="UploadedFileName3"></label>
                    </div>
                </div>
                 
                
            </div>
        </div>        
        <div class="row" id="Upload4" runat="server">
            <div class="col-md-12">
                <div class="col-md-12">
                    <div class="form-group">
                        
                        <label class="control-label no-padding-right" runat="server" id="lblname4">Agreement Upload</label>
                        <br />
                        <asp:FileUpload ID="FileUpload4" runat="server"/>
                         <br />
                        <asp:Button runat="server" ID="Button4" Text="Upload" CssClass="btn btn-primary" OnClick="Save3_Click"></asp:Button>
                         <br />
                         <label class="control-label no-padding-right" runat="server" id="UploadedFileName4"></label>
                    </div>
                </div>
                 
                
            </div>
        </div>     
    </form>
</body>
</html>
