using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterProject : System.Web.UI.Page
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
            get_tbl_FundingPattern();
            get_tbl_Project();
        }
    }

    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
        }
        else
        {
            grdPost.DataSource = null;
            grdPost.DataBind();
        }
    }
    private void get_tbl_FundingPattern()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_FundingPattern();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdFundingPattern.DataSource = ds.Tables[0];
            grdFundingPattern.DataBind();
        }
        else
        {
            grdFundingPattern.DataSource = null;
            grdFundingPattern.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Msg = "";
        if (txtProject.Text.Trim() == string.Empty)
        {
            Msg = "Give Scheme";
            txtProject.Focus();
            return;
        }
        tbl_Project obj_tbl_Project = new tbl_Project();
        if (hf_Project_Id.Value == "0" || hf_Project_Id.Value == "")
        {
            obj_tbl_Project.Project_Id = 0;
        }
        else
        {
            obj_tbl_Project.Project_Id = Convert.ToInt32(hf_Project_Id.Value);
        }
        obj_tbl_Project.Project_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_Project.Project_Name = txtProject.Text.Trim();
        string ext1 = "";
        if (flUploadGO.HasFile)
        {
            obj_tbl_Project.Project_GO_Path_Bytes = flUploadGO.FileBytes;
            ext1 = flUploadGO.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_Project.Project_GO_Path = btnDownload.ToolTip;
        }
        string ext2 = "";
        if (flUploadGuideline.HasFile)
        {
            obj_tbl_Project.Project_Guideline_Path_Bytes = flUploadGuideline.FileBytes;
            ext2 = flUploadGuideline.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_Project.Project_Guideline_Path = btnDownloadGuideline.ToolTip;
        }
        try
        {
            obj_tbl_Project.Project_Budget = txtBudget.Text.Trim();
        }
        catch
        {
            obj_tbl_Project.Project_Budget = "0";
        }

        obj_tbl_Project.Project_Status = 1;
        if (obj_tbl_Project == null)
        {
            MessageBox.Show(Msg);
            return;
        }

        List<tbl_ProjectFundingPattern> obj_tbl_ProjectFundingPattern_Li = new List<tbl_ProjectFundingPattern>();
        for (int i = 0; i < grdFundingPattern.Rows.Count; i++)
        {
            tbl_ProjectFundingPattern obj_tbl_ProjectFundingPattern = new tbl_ProjectFundingPattern();
            obj_tbl_ProjectFundingPattern.ProjectFundingPattern_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectFundingPattern.ProjectFundingPattern_FundingPattern_Id = Convert.ToInt32(grdFundingPattern.Rows[i].Cells[0].Text.ToString());
            try
            {
                obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Percentage = Convert.ToDecimal((grdFundingPattern.Rows[i].FindControl("txtShareP") as TextBox).Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Percentage = 0;
            }
            try
            {
                obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Value = Convert.ToDecimal((grdFundingPattern.Rows[i].FindControl("txtShareV") as TextBox).Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Value = 0;
            }
            obj_tbl_ProjectFundingPattern.ProjectFundingPattern_ProjectId = obj_tbl_Project.Project_Id;
            obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Status = 1;
            if (obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Value + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Percentage > 0)
            {
                obj_tbl_ProjectFundingPattern_Li.Add(obj_tbl_ProjectFundingPattern);
            }
        }
        if (new DataLayer().Insert_tbl_Project(obj_tbl_Project, obj_tbl_ProjectFundingPattern_Li, obj_tbl_Project.Project_Id, ref Msg, ext1, ext2))
        {
            MessageBox.Show("Scheme Created Successfully ! ");
            reset();
            get_tbl_Project();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Scheme Already Exist. Give another! ");
            }
            else
            {
                MessageBox.Show("Error ! ");
            }
            return;
        }
    }

    private void reset()
    {
        txtProject.Text = "";
        txtBudget.Text = "";
        hf_Project_Id.Value = "0";
        get_tbl_Project();
        btnDownload.ToolTip = "";
        btnDownloadGuideline.ToolTip = "";
        divCreateNew.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int Project_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_Project_Id.Value = Project_Id.ToString();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).Edit_tbl_Project(Project_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtProject.Text = ds.Tables[0].Rows[0]["Project_Name"].ToString();
            try
            {
                txtBudget.Text = ds.Tables[0].Rows[0]["Project_Budget"].ToString().Replace("&nbsp;", "");
            }
            catch
            {
                txtBudget.Text = "0";
            }
            btnDownload.ToolTip = ds.Tables[0].Rows[0]["Project_GO_Path"].ToString();
            btnDownloadGuideline.ToolTip = ds.Tables[0].Rows[0]["Project_Guideline_Path"].ToString();
        }
        if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            grdFundingPattern.DataSource = ds.Tables[1];
            grdFundingPattern.DataBind();
        }
        divCreateNew.Visible = true;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Project_Id = Convert.ToInt32(hf_Project_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Delete_Project(Project_Id, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            reset();
            return;
        }
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
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        btnDelete.Visible = false;
        txtProject.Text = "";
        txtBudget.Text = "";
        hf_Project_Id.Value = "0";
        btnDownload.ToolTip = "";
        btnDownloadGuideline.ToolTip = "";
        divCreateNew.Visible = true;
    }

    protected void grdFundingPattern_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void txtShareP_TextChanged(object sender, EventArgs e)
    {

    }
}
