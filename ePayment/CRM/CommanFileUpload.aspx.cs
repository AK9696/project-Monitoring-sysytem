using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommanFileUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }

        if (Session["FileName"] != null)
        {
            UploadedFileName.InnerText = "File Name: " + Session["FileName"].ToString();
        }
        else
        {
            UploadedFileName.InnerText = "";
        }
        if (Session["FileName1"] != null)
        {
            UploadedFileName1.InnerText = "File Name: " + Session["FileName1"].ToString();
        }
        else
        {
            UploadedFileName1.InnerText = "";
        }
        if (Session["FileName3"] != null)
        {
            UploadedFileName3.InnerText = "File Name: " + Session["FileName3"].ToString();
        }
        else
        {
            UploadedFileName4.InnerText = "";
        }
        if (Session["FileName4"] != null)
        {
            UploadedFileName4.InnerText = "File Name: " + Session["FileName4"].ToString();
        }
        else
        {
            UploadedFileName4.InnerText = "";
        }
        if (!IsPostBack)
        {
            Upload1.Visible = false;
            Upload2.Visible = false;
            Upload3.Visible = false;
            Upload4.Visible = false;
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
                        //Upload2.Visible = false;
                    }
                    if (Request.QueryString["UploadCheck"].ToString() == "3")
                    {
                        if (Request.QueryString["Name"] != null)
                        {
                            lblname3.InnerText = Request.QueryString["Name"].ToString();
                        }
                        Upload3.Visible = true;
                        
                    }
                    if (Request.QueryString["UploadCheck"].ToString() == "4")
                    {
                        if (Request.QueryString["Name"] != null)
                        {
                            lblname4.InnerText = Request.QueryString["Name"].ToString();
                        }
                        Upload4.Visible = true;

                    }
                    else
                    {
                        if (Request.QueryString["Name"] != null)
                        {
                            lblname2.InnerText = Request.QueryString["Name"].ToString();
                        }
                        //Upload1.Visible = false;
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
            Session["FileName"] = flUpload.FileName;
            Session["FileBytes"] = flUpload.FileBytes;
            UploadedFileName.InnerText = "File Name: " + Session["FileName"].ToString();
        }
        else
        {
            Session["FileName"] = null;
            Session["FileBytes"] = null;
        }
    }

    protected void Save1_Click(object sender, EventArgs e)
    {
        if (flUpload2.HasFile)
        {
            Session["FileName1"] = flUpload2.FileName;
            Session["FileBytes1"] = flUpload2.FileBytes;
            UploadedFileName1.InnerText = "File Name: " + Session["FileName1"].ToString();
        }
        else
        {
            Session["FileName1"] = null;
            Session["FileBytes1"] = null;
        }
    }
    protected void Save2_Click(object sender, EventArgs e)
    {
        if (FileUpload3.HasFile)
        {
            Session["FileName3"] = FileUpload3.FileName;
            Session["FileBytes3"] = FileUpload3.FileBytes;
            UploadedFileName3.InnerText = "File Name: " + Session["FileName3"].ToString();
        }
        else
        {
            Session["FileName3"] = null;
            Session["FileBytes3"] = null;
        }
    }
    protected void Save3_Click(object sender, EventArgs e)
    {
        if (FileUpload4.HasFile)
        {
            Session["FileName4"] = FileUpload4.FileName;
            Session["FileBytes4"] = FileUpload4.FileBytes;
            UploadedFileName4.InnerText = "File Name: " + Session["FileName4"].ToString();
        }
        else
        {
            Session["FileName4"] = null;
            Session["FileBytes4"] = null;
        }
    }
}