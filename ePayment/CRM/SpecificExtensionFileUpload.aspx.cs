using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SpecificExtensionFileUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }

        if (Session["FileNameExcel"] != null)
        {
            UploadedFileName.InnerText = "File Name: " + Session["FileNameExcel"].ToString();
        }
        else
        {
            UploadedFileName.InnerText = "";
        }
        if (Session["FileNamePDF"] != null)
        {
            UploadedFileName1.InnerText = "File Name: " + Session["FileNamePDF"].ToString();
        }
        else
        {
            UploadedFileName1.InnerText = "";
        }
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["UploadCheck"] != null)
                {
                    if (Request.QueryString["UploadCheck"].ToString() == "1")
                    {
                        if (Request.QueryString["Name"] != null)
                        {
                            lblname1.InnerText = Request.QueryString["Name"].ToString();
                        }
                        Upload1.Visible = true;
                        Upload2.Visible = false;
                    }
                    else
                    {
                        if (Request.QueryString["Name"] != null)
                        {
                            lblname2.InnerText = Request.QueryString["Name"].ToString();
                        }
                        Upload1.Visible = false;
                        Upload2.Visible = true;
                    }

                }
            }
        }

    }

    protected void Save_Click(object sender, EventArgs e)
    {
        if (flUpload.HasFile)
        {
            Session["FileNameExcel"] = flUpload.FileName;
            Session["FileBytesExcel"] = flUpload.FileBytes;
            UploadedFileName.InnerText = "File Name: " + Session["FileNameExcel"].ToString();
        }
        else
        {
            Session["FileNameExcel"] = null;
            Session["FileBytesExcel"] = null;
        }
    }

    protected void Save1_Click(object sender, EventArgs e)
    {
        if (flUpload2.HasFile)
        {
            Session["FileNamePDF"] = flUpload2.FileName;
            Session["FileBytesPDF"] = flUpload2.FileBytes;
            UploadedFileName1.InnerText = "File Name: " + Session["FileNamePDF"].ToString();
        }
        else
        {
            Session["FileNamePDF"] = null;
            Session["FileBytesPDF"] = null;
        }
    }
}