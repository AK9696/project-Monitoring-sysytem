using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class MasterDPR_Approval : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.MasterPageFile = SetMasterPage.ReturnPage();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            Session["FileName"] = null;
            Session["FileBytes"] = null;
            get_tbl_Project();
            get_tbl_Zone();
            get_tbl_PhysicalProgressComponent();
            get_tbl_Deliverables();
            if (Session["UserType"].ToString() != "1")
            {
                try
                {
                    if (Session["PersonJuridiction_Project_Id"].ToString() != "" && Session["PersonJuridiction_Project_Id"].ToString() != "0")
                    {
                        ddlSearchScheme.SelectedValue = Session["PersonJuridiction_Project_Id"].ToString();
                        ddlSearchScheme.Enabled = false;
                    }
                }
                catch
                {

                }

            }
            if (Session["UserType"].ToString() == "2" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {//District
                try
                {
                    //ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    //ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    //ddlDistrict.Enabled = false;
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {
                try
                {
                    //ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    //ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    //ddlDistrict.Enabled = false;
                    if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
                    {//ULB
                        try
                        {
                            ddlULB.SelectedValue = Session["ULB_Id"].ToString();
                            ddlULB.Enabled = false;
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "4" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {//Zone
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "6" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                    if (Session["UserType"].ToString() == "6" && Convert.ToInt32(Session["PersonJuridiction_CircleId"].ToString()) > 0)
                    {//Circle
                        try
                        {
                            ddlCircle.SelectedValue = Session["PersonJuridiction_CircleId"].ToString();
                            ddlCircle_SelectedIndexChanged(ddlCircle, e);
                            ddlCircle.Enabled = false;
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                    if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_CircleId"].ToString()) > 0)
                    {//Circle
                        try
                        {
                            ddlCircle.SelectedValue = Session["PersonJuridiction_CircleId"].ToString();
                            ddlCircle_SelectedIndexChanged(ddlCircle, e);
                            ddlCircle.Enabled = false;
                            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString()) > 0)
                            {//Circle
                                try
                                {
                                    ddlDivision.SelectedValue = Session["PersonJuridiction_DivisionId"].ToString();
                                    ddlDivision.Enabled = false;
                                }
                                catch
                                { }
                            }
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }
        }
    }
    private void get_tbl_PhysicalProgressComponent()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PhysicalProgressComponent();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPhysicalProgress.DataSource = ds.Tables[0];
            grdPhysicalProgress.DataBind();
        }
        else
        {
            grdPhysicalProgress.DataSource = null;
            grdPhysicalProgress.DataBind();
        }
    }
    private void get_tbl_Deliverables()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deliverables();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdDeliverables.DataSource = ds.Tables[0];
            grdDeliverables.DataBind();
        }
        else
        {
            grdDeliverables.DataSource = null;
            grdDeliverables.DataBind();
        }
    }

    private void get_tbl_Zone()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlZone, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlZone.Items.Clear();
        }
    }
    private void get_tbl_Circle(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(Zone_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlCircle, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlCircle.Items.Clear();
        }
    }
    private void get_tbl_Division(int Circle_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(Circle_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDivision, "Division_Name", "Division_Id");
        }
        else
        {
            ddlDivision.Items.Clear();
        }
    }
    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlSearchScheme, "Project_Name", "Project_Id");
        }
        else
        {
            ddlSearchScheme.Items.Clear();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }

    private void reset()
    {
       
        hf_ProjectWork_Id.Value = "0";
        hf_Project_Id.Value = "0";
        divEntry.Visible = false;
    }

    protected void grdPost_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            gv.UseAccessibleHeader = true;
        }
        if ((gv.ShowHeader == true && gv.Rows.Count > 0) || (gv.ShowHeaderWhenEmpty == true))
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        if (gv.ShowFooter == true && gv.Rows.Count > 0)
        {
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {
            ddlCircle.Items.Clear();
            ddlDivision.Items.Clear();
        }
        else
        {
            get_tbl_Circle(Convert.ToInt32(ddlZone.SelectedValue));
        }
    }

    protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCircle.SelectedValue == "0")
        {
            ddlDivision.Items.Clear();
        }
        else
        {
            get_tbl_Division(Convert.ToInt32(ddlCircle.SelectedValue));
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlSearchScheme.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            ddlSearchScheme.Focus();
            return;
        }
        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            return;
        }
        int Project_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            Project_Id = Convert.ToInt32(ddlSearchScheme.SelectedValue);
        }
        catch
        {
            Project_Id = 0;
        }
        try
        {
            Zone_Id = Convert.ToInt32(ddlZone.SelectedValue);
        }
        catch
        {
            Zone_Id = 0;
        }
        try
        {
            Circle_Id = Convert.ToInt32(ddlCircle.SelectedValue);
        }
        catch
        {
            Circle_Id = 0;
        }
        try
        {
            Division_Id = Convert.ToInt32(ddlDivision.SelectedValue);
        }
        catch
        {
            Division_Id = 0;
        }

        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", false, "IsVerified");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            divData.Visible = true;
            divEntry.Visible = false;
        }
        else
        {
            divData.Visible = false;
            divEntry.Visible = false;
            grdPost.DataSource = null;
            grdPost.DataBind();
            MessageBox.Show("No Records Found");
        }
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Work_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        int Project_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        divEntry.Visible = true;
        hf_ProjectWork_Id.Value = gr.Cells[1].Text.Trim();
        hf_Project_Id.Value = gr.Cells[2].Text.Trim();
        hf_ProjectDPR_Id.Value = gr.Cells[3].Text.Trim();
        hf_ProjectWorkPkg_Id.Value = gr.Cells[0].Text.Trim();
        DataSet ds = new DataLayer().get_ProjectWork_Update_Detailed(Convert.ToInt32(hf_ProjectWork_Id.Value), Convert.ToInt32(hf_Project_Id.Value));
        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
        {
            dgvQuestionnaire.DataSource = ds.Tables[2];
            dgvQuestionnaire.DataBind();
        }
        else
        {
            dgvQuestionnaire.DataSource = null;
            dgvQuestionnaire.DataBind();
        }
    }

    protected void dgvQuestionnaire_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        if (gv.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            gv.UseAccessibleHeader = true;
        }
        if ((gv.ShowHeader == true && gv.Rows.Count > 0) || (gv.ShowHeaderWhenEmpty == true))
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        if (gv.ShowFooter == true && gv.Rows.Count > 0)
        {
            gv.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    protected void chkSelectAllApproveH_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAllApproveH1 = (sender as CheckBox);
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            chkSelectAllApprove.Checked = chkSelectAllApproveH1.Checked;
        }
    }

    protected void chkSelectAllDeliverables_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAllApproveH = (sender as CheckBox);
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            chkSelectAllApprove.Checked = chkSelectAllApproveH.Checked;
        }
    }

    protected void rbtPhysicalProgressTrackingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtPhysicalProgressTrackingType.SelectedValue == "ExtendedTracking")
        {
            divExtendedTracking.Visible = true;
        }
        else
        {
            divExtendedTracking.Visible = false;
        }
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            chkSelectAllApprove.Checked = false;
            CheckBox chkSelectAllApproveH = grdDeliverables.HeaderRow.FindControl("chkSelectAllDeliverables") as CheckBox;
            chkSelectAllApproveH.Checked = false;
        }
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {

            CheckBox chkSelectAllApprove = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            chkSelectAllApprove.Checked = false;
            CheckBox chkSelectAllDeliverables = grdPhysicalProgress.HeaderRow.FindControl("chkSelectAllApproveH") as CheckBox;
            chkSelectAllDeliverables.Checked = false;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int Project_Id = 0;
        try
        {
            Project_Id = Convert.ToInt32(hf_Project_Id.Value);
        }
        catch
        {
            Project_Id = 0;
        }
        int Project_Work_Id = 0;
        try
        {
            Project_Work_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
        }
        catch
        {
            Project_Work_Id = 0;
        }
        int ProjectDPR_Id = 0;
        try
        {
            ProjectDPR_Id = Convert.ToInt32(hf_ProjectDPR_Id.Value);
        }
        catch
        {
            ProjectDPR_Id = 0;
        }
        if (Project_Work_Id == 0)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (txtComments.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Comments");
            txtComments.Focus();
            return;
        }
        if (txtGODate.Text == "")
        {
            MessageBox.Show("Please Provide Go Date");
            txtGODate.Focus();
            return;
        }
        if (txtGONo.Text == "")
        {
            MessageBox.Show("Please Provide Go Number");
            txtGONo.Focus();
            return;
        }
        if (rbtPhysicalProgressTrackingType.SelectedValue.Trim() == "" || rbtPhysicalProgressTrackingType.SelectedValue.Trim() == null)
        {
            MessageBox.Show("Please Select Physical Progress Tracking Type");
            rbtPhysicalProgressTrackingType.Focus();
            return;
        }

        List<tbl_ProjectDPR_PhysicalProgress> obj_tbl_ProjectDPR_PhysicalProgress = new List<tbl_ProjectDPR_PhysicalProgress>();
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {
            CheckBox checkBox = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            if (checkBox.Checked == true)
            {
                tbl_ProjectDPR_PhysicalProgress obj_tbl_ProjectDPR_PhysicalProgress1 = new tbl_ProjectDPR_PhysicalProgress();
                obj_tbl_ProjectDPR_PhysicalProgress1.ProjectDPR_PhysicalProgress_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectDPR_PhysicalProgress1.ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id = Convert.ToInt32(grdPhysicalProgress.Rows[i].Cells[0].Text.Trim());
                obj_tbl_ProjectDPR_PhysicalProgress1.ProjectDPR_PhysicalProgress_Status = 1;
                obj_tbl_ProjectDPR_PhysicalProgress.Add(obj_tbl_ProjectDPR_PhysicalProgress1);
            }

        }

        List<tbl_ProjectDPR_Deliverables> obj_tbl_ProjectDPR_Deliverables = new List<tbl_ProjectDPR_Deliverables>();
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox checkBox = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            if (checkBox.Checked == true)
            {
                tbl_ProjectDPR_Deliverables obj_tbl_ProjectDPR_Deliverables1 = new tbl_ProjectDPR_Deliverables();
                obj_tbl_ProjectDPR_Deliverables1.ProjectDPR_Deliverables_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectDPR_Deliverables1.ProjectDPR_Deliverables_Deliverables_Id = Convert.ToInt32(grdDeliverables.Rows[i].Cells[0].Text.Trim());
                obj_tbl_ProjectDPR_Deliverables1.ProjectDPR_Deliverables_Status = 1;
                obj_tbl_ProjectDPR_Deliverables.Add(obj_tbl_ProjectDPR_Deliverables1);
            }

        }
        if (rbtPhysicalProgressTrackingType.SelectedValue == "ExtendedTracking" && obj_tbl_ProjectDPR_PhysicalProgress == null)
        {
            MessageBox.Show("Please Check At Least One Physical Progress!");
            return;
        }
        if (rbtPhysicalProgressTrackingType.SelectedValue == "ExtendedTracking" && obj_tbl_ProjectDPR_Deliverables == null)
        {
            MessageBox.Show("Please Check At Least One Deliverables!");
            return;
        }
        if (rbtPhysicalProgressTrackingType.SelectedValue == "BasicTracking")
        {
            obj_tbl_ProjectDPR_PhysicalProgress = null;
            obj_tbl_ProjectDPR_Deliverables = null;
        }
        string ProgressTrackingType = rbtPhysicalProgressTrackingType.SelectedValue;

        decimal Allocated_Budget = 0;

        try
        {
            Allocated_Budget = Convert.ToDecimal(txtAmount.Text.Trim()) * 100000;
        }
        catch
        {
            Allocated_Budget = 0;
        }
        if (Allocated_Budget == 0)
        {
            MessageBox.Show("Plese Provide Allocated Budget");
            txtAmount.Focus();
            return;
        }
        string filepath1 = "";
        Byte[] fileBytes1 = null;
        if (Session["FileBytes"] != null)
        {
            filepath1 = "\\Downloads\\GO\\" + DateTime.Now.ToString("MMddyyyyHHmmss") + "" + Session["FileName"].ToString();
            fileBytes1 = (Byte[])Session["FileBytes"];
        }
        else
        {
            fileBytes1 = null;
            filepath1 = "";
        }

        if (fileBytes1 == null)
        {
            MessageBox.Show("Please Upload GO");
            return;
        }
                
        File_Objects obj_File_Objects = new File_Objects();
        obj_File_Objects.Work_Id= Project_Work_Id;
        obj_File_Objects.Package_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
        obj_File_Objects.ProjectDPR_Id = ProjectDPR_Id;
        obj_File_Objects.Added_By = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_File_Objects.File_Path_1 = filepath1;
        obj_File_Objects.File_Path_Bytes_1 = fileBytes1;
        obj_File_Objects.File_Path_2 = "";
        obj_File_Objects.File_Path_Bytes_2 = null;
        obj_File_Objects.Comments = txtComments.Text;

        if (new DataLayer().Update_tbl_ProjectDPR_Approval(Allocated_Budget, AllClasses.re_Organize_GO_No(txtGONo.Text.Trim(), false), txtGODate.Text.Trim(), obj_File_Objects, obj_tbl_ProjectDPR_PhysicalProgress, obj_tbl_ProjectDPR_Deliverables, ProgressTrackingType))
        {
            MessageBox.Show("DPR Status Updated Successfully");
            btnSearch_Click(btnSearch, e);
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Unable To Update DPR Status");
            return;
        }
    }

    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDPRFile = (e.Row.FindControl("lnkDPRFile") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDPRFile);
            LinkButton lnkDocmentFile = (e.Row.FindControl("lnkDocmentFile") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDocmentFile);
            LinkButton lnkSitePic1 = (e.Row.FindControl("lnkSitePic1") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkSitePic1);
            LinkButton lnkSitePic2 = (e.Row.FindControl("lnkSitePic2") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkSitePic2);
        }
    }
    protected void lnkDPRFile_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[6].Text.Replace("&nbsp", "").ToString().Trim();
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

    protected void lnkDocmentFile_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[7].Text.Replace("&nbsp", "").ToString().Trim();
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

    protected void lnkSitePic1_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[8].Text.Replace("&nbsp", "").ToString().Trim();
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

    protected void lnkSitePic2_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[9].Text.Replace("&nbsp", "").ToString().Trim();
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