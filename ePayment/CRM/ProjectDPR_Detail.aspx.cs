using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProjectDPR_Detail : System.Web.UI.Page
{
    private void BindGMap(DataTable datatable)
    {
        try
        {
            List<ProgramAddresses> AddressList = new List<ProgramAddresses>();
            foreach (DataRow dr in datatable.Rows)
            {
                try
                {
                    ProgramAddresses MapAddress = new ProgramAddresses();
                    MapAddress.lat = double.Parse(dr["ProjectDPR_Latitude"].ToString());
                    MapAddress.lng = double.Parse(dr["ProjectDPR_Longitude"].ToString());
                    AddressList.Add(MapAddress);
                }
                catch
                { }
            }

            string jsonString = JsonConvert.SerializeObject(AddressList);
            hf_Map_Data.Value = jsonString;
        }
        catch (Exception)
        {
            hf_Map_Data.Value = "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            divExtendedTracking.Visible = false;
            if (Request.QueryString.Count > 0)
            {
                int ProjectDPR_Id = Convert.ToInt32(Request.QueryString["ProjectDPR_Id"]);
                DataSet ds1 = new DataSet();
                ds1 = (new DataLayer()).get_tbl_ProjectDPRRequest_Details(ProjectDPR_Id);
                if (ds1 != null)
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        lblScheme.Text = ds1.Tables[0].Rows[0]["Project_Name"].ToString();
                        grdPost.DataSource = ds1.Tables[0];
                        grdPost.DataBind();
                        grdUploadDocument.DataSource = ds1.Tables[0];
                        grdUploadDocument.DataBind();
                        grdApproval.DataSource = ds1.Tables[0];
                        grdApproval.DataBind();
                        BindGMap(ds1.Tables[0]);
                        if (ds1.Tables[0].Rows[0]["ProjectDPR_PhysicalProgressTrackingType"].ToString() == "ExtendedTracking")
                        {
                            if (ds1.Tables.Count > 3 && ds1.Tables[3].Rows.Count > 0)
                            {
                                grdPhysicalProgress.DataSource = ds1.Tables[3];
                                grdPhysicalProgress.DataBind();
                            }
                            else
                            {
                                grdPhysicalProgress.DataSource = null;
                                grdPhysicalProgress.DataBind();
                            }
                            if (ds1.Tables.Count > 4 && ds1.Tables[4].Rows.Count > 0)
                            {
                                grdDeliverables.DataSource = ds1.Tables[4];
                                grdDeliverables.DataBind();
                            }
                            else
                            {
                                grdDeliverables.DataSource = null;
                                grdDeliverables.DataBind();
                            }
                            divExtendedTracking.Visible = true;
                        }
                        else
                        {
                            grdPhysicalProgress.DataSource = null;
                            grdPhysicalProgress.DataBind();
                            grdDeliverables.DataSource = null;
                            grdDeliverables.DataBind();
                           
                        }

                    }
                    else
                    {
                        grdPost.DataSource = null;
                        grdPost.DataBind();
                        grdUploadDocument.DataSource = null;
                        grdUploadDocument.DataBind();
                        grdApproval.DataSource = null;
                        grdApproval.DataBind();
                        grdPhysicalProgress.DataSource = null;
                        grdPhysicalProgress.DataBind();
                        grdDeliverables.DataSource = null;
                        grdDeliverables.DataBind();
                    }
                    if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0)
                    {
                        grdVerifiedStatus.DataSource = ds1.Tables[1];
                        grdVerifiedStatus.DataBind();
                    }
                    else
                    {
                        grdVerifiedStatus.DataSource = null;
                        grdVerifiedStatus.DataBind();
                    }
                    if (ds1.Tables.Count > 2 && ds1.Tables[2].Rows.Count > 0)
                    {
                        dgvQuestionnaire.DataSource = ds1.Tables[2];
                        dgvQuestionnaire.DataBind();
                    }
                    else
                    {
                        dgvQuestionnaire.DataSource = null;
                        dgvQuestionnaire.DataBind();
                    }

                }
                else
                {
                    grdPost.DataSource = null;
                    grdPost.DataBind();
                    grdUploadDocument.DataSource = null;
                    grdUploadDocument.DataBind();
                    grdVerifiedStatus.DataSource = null;
                    grdUploadDocument.DataBind();
                    dgvQuestionnaire.DataSource = null;
                    dgvQuestionnaire.DataBind();
                    grdApproval.DataSource = null;
                    grdApproval.DataBind();
                    grdPhysicalProgress.DataSource = null;
                    grdPhysicalProgress.DataBind();
                    grdDeliverables.DataSource = null;
                    grdDeliverables.DataBind();
                }
            }
            else
            {
                MessageBox.Show("Work Details Not Found!!");
                return;
            }
        }
    }

    protected void tbnpopupClose_Click(object sender, EventArgs e)
    {
        mp1.Hide();
    }

    protected void grdUploadDocument_grdUploadDocument(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkAgreementUpload2_FilePath = (e.Row.FindControl("lnkAgreementUpload2_FilePath") as LinkButton);
            if (e.Row.Cells[0].Text.Trim().Replace("&nbsp;", "") != "")
            {
                lnkAgreementUpload2_FilePath.Visible = true;
            }
            else
            {
                lnkAgreementUpload2_FilePath.Visible = false;
            }
            LinkButton lnkAgreementFile3 = (e.Row.FindControl("lnkAgreementFile3") as LinkButton);
            if (e.Row.Cells[1].Text.Trim().Replace("&nbsp;", "") != "")
            {
                lnkAgreementFile3.Visible = true;
            }
            else
            {
                lnkAgreementFile3.Visible = false;
            }
            LinkButton lnkAgreementFile4 = (e.Row.FindControl("lnkAgreementFile4") as LinkButton);
            if (e.Row.Cells[2].Text.Trim().Replace("&nbsp;", "") != "")
            {
                lnkAgreementFile4.Visible = true;
            }
            else
            {
                lnkAgreementFile4.Visible = false;
            }
            LinkButton lnkDownload = (e.Row.FindControl("lnkAgreementUpload2_FilePath") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload);

            LinkButton lnkDownload1 = (e.Row.FindControl("lnkAgreementFile3") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload1);

            LinkButton lnkDownload2 = (e.Row.FindControl("lnkAgreementFile4") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload2);
        }
    }

    protected void lnkAgreementUpload2_FilePath_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[0].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkAgreementFile3_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[1].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkAgreementFile4_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[2].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void grdApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkAgreementFile2 = (e.Row.FindControl("lnkAgreementFile2") as LinkButton);
            if (e.Row.Cells[0].Text.Trim().Replace("&nbsp;", "") != "")
            {
                lnkAgreementFile2.Visible = true;
            }
            else
            {
                lnkAgreementFile2.Visible = false;
            }
            
            
            LinkButton lnkDownload1 = (e.Row.FindControl("lnkAgreementFile2") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload1);

        }
    }

    protected void lnkAgreementFile2_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[2].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }
}