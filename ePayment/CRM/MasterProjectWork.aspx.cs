using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterProjectWork : System.Web.UI.Page
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
            get_M_Jurisdiction();
            get_tbl_Zone();
            get_tbl_Zone_Addditional();
            get_tbl_FundingPattern();
            get_tbl_ProjectType();
            get_tbl_Project();
            if (Session["UserType"].ToString() == "2" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {//District
                try
                {
                    ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    ddlDistrict.Enabled = false;
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {
                try
                {
                    ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    ddlDistrict.Enabled = false;
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
            get_tbl_ProjectWork();
        }
        if (Session["UserType"].ToString() == "1")
        {
            divCreate.Visible = true;
        }
        else
        {
            divCreate.Visible = false;
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
    protected void get_tbl_ProjectWork()
    {
        int District_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            District_Id = Convert.ToInt32(ddlDistrict.SelectedValue);
        }
        catch
        {
            District_Id = 0;
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
        ds = (new DataLayer()).get_tbl_ProjectWork(0, District_Id, Zone_Id, Circle_Id, Division_Id, 0,"");
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
    private void get_tbl_ProjectType()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectType();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlProjectType, "ProjectType_Name", "ProjectType_Id");
        }
        else
        {
            ddlProjectType.Items.Clear();
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
    private void get_tbl_Zone_Addditional()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionZone, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlAdditionZone.Items.Clear();
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
    private void get_M_Jurisdiction()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(3, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlDistrict.Items.Clear();
        }
    }

    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlProjectMaster, "Project_Name", "Project_Id");
        }
        else
        {
            ddlProjectMaster.Items.Clear();
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
        if (ddlProjectMaster.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            ddlProjectMaster.Focus();
            return;
        }
        if (txtProjectWorkName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Project Name");
            txtProjectWorkName.Focus();
            return;
        }
        if (ddlDistrict.SelectedValue == "0")
        {
            MessageBox.Show("Please Select District");
            ddlDistrict.Focus();
            return;
        }
        if (txtProjectCode.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Project Code");
            txtProjectCode.Focus();
            return;
        }
        if (txtBudget.Text.Trim() == "" || txtBudget.Text.Trim() == "0")
        {
            MessageBox.Show("Please Provide Project Allocated Budget");
            txtBudget.Focus();
            return;
        }
        if (txtGODate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide GO Date");
            txtGODate.Focus();
            return;
        }
        if (txtGONo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide GO No");
            txtGONo.Focus();
            return;
        }
        tbl_ProjectWork obj_tbl_ProjectWork = new tbl_ProjectWork();
        try
        {
            obj_tbl_ProjectWork.ProjectWork_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWork_Id = 0;
        }

        obj_tbl_ProjectWork.ProjectWork_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());

        string extGO = "";
        if (flUploadGO.HasFile)
        {
            obj_tbl_ProjectWork.ProjectWork_GO_Path_Bytes = flUploadGO.FileBytes;
            extGO = flUploadGO.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        obj_tbl_ProjectWork.ProjectWork_GO_Date = txtGODate.Text.Trim();
        obj_tbl_ProjectWork.ProjectWork_GO_No = txtGONo.Text.Trim();
        try
        {
            obj_tbl_ProjectWork.ProjectWork_Budget = Convert.ToDecimal(txtBudget.Text.Trim());
        }
        catch {
            obj_tbl_ProjectWork.ProjectWork_Budget = 0;
        }
        try
        {
            obj_tbl_ProjectWork.ProjectWork_Centage = Convert.ToDecimal(txtCentage.Text.Trim());
        }
        catch {
            obj_tbl_ProjectWork.ProjectWork_Centage = 0;
        }
        
        obj_tbl_ProjectWork.ProjectWork_Name = txtProjectWorkName.Text.Trim();
        obj_tbl_ProjectWork.ProjectWork_Description = txtProjectWorkDesc.Text.Trim();
        obj_tbl_ProjectWork.ProjectWork_ProjectCode = txtProjectCode.Text.Trim();
        obj_tbl_ProjectWork.ProjectWork_Project_Id = Convert.ToInt32(ddlProjectMaster.SelectedValue);
        try
        {
            obj_tbl_ProjectWork.ProjectWork_ProjectType_Id = Convert.ToInt32(ddlProjectType.SelectedValue);
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWork_ProjectType_Id = 0;
        }
        obj_tbl_ProjectWork.ProjectWork_DistrictId = Convert.ToInt32(ddlDistrict.SelectedValue);
        obj_tbl_ProjectWork.ProjectWork_BlockId = Convert.ToInt32(ddlBlock.SelectedValue);
        if (rbtMappingWith.SelectedValue == "U")
        {
            obj_tbl_ProjectWork.ProjectWork_ULB_Id = Convert.ToInt32(ddlULB.SelectedValue);
        }
        else
        {
            obj_tbl_ProjectWork.ProjectWork_DivisionId = Convert.ToInt32(ddlDivision.SelectedValue);
        }
        obj_tbl_ProjectWork.ProjectWork_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProjectWork.ProjectWork_Status = 1;
        List<tbl_ProjectWorkFundingPattern> obj_tbl_ProjectWorkFundingPattern_Li = new List<tbl_ProjectWorkFundingPattern>();

        for (int i = 0; i < grdFundingPattern.Rows.Count; i++)
        {
            tbl_ProjectWorkFundingPattern obj_tbl_ProjectWorkFundingPattern = new tbl_ProjectWorkFundingPattern();
            obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_FundingPatternId = Convert.ToInt32(grdFundingPattern.Rows[i].Cells[0].Text.ToString());
            try
            {
                obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Percentage = Convert.ToDecimal((grdFundingPattern.Rows[i].FindControl("txtShareP") as TextBox).Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Percentage = 0;
            }
            try
            {
                obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Value = Convert.ToDecimal((grdFundingPattern.Rows[i].FindControl("txtShareV") as TextBox).Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Value = 0;
            }
            obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_ProjectWorkId = obj_tbl_ProjectWork.ProjectWork_Id;
            obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Status = 1;
            if (obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Value + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Percentage > 0)
            {
                obj_tbl_ProjectWorkFundingPattern_Li.Add(obj_tbl_ProjectWorkFundingPattern);
            }
        }

        List<tbl_ProjectAdditionalArea> obj_tbl_ProjectAdditionalArea_Li = new List<tbl_ProjectAdditionalArea>();
        for (int i = 0; i < dgvAdditionalDivision.Rows.Count; i++)
        {
            tbl_ProjectAdditionalArea obj_tbl_ProjectAdditionalArea = new tbl_ProjectAdditionalArea();
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_ZoneId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[0].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_CircleId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[1].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_DevisionId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[2].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_Status = 1;
            obj_tbl_ProjectAdditionalArea_Li.Add(obj_tbl_ProjectAdditionalArea);
        }


        if ((new DataLayer()).Insert_tbl_ProjectWork(obj_tbl_ProjectWork, obj_tbl_ProjectWorkFundingPattern_Li, extGO, obj_tbl_ProjectAdditionalArea_Li))
        {
            MessageBox.Show("Project Created Successfully!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Creating Project!");
            return;
        }
    }

    private void reset()
    {
        ddlDistrict.SelectedValue = "0";
        ddlULB.Items.Clear();
        hf_ProjectWork_Id.Value = "0";
        txtProjectWorkName.Text = "";
        //ddlProjectMaster.SelectedValue = "0";
        //get_tbl_ProjectWork();
        txtBudget.Text = "0";
        txtCentage.Text = "0";
        txtGODate.Text = "";
        txtProjectCode.Text = "";
        txtProjectWorkDesc.Text = "";
        txtProjectWorkName.Text = "";
        txtGONo.Text = "";
        get_tbl_FundingPattern();
        ViewState["AdditionalDivision"] = null;
        dgvAdditionalDivision.DataSource = null;
        dgvAdditionalDivision.DataBind();
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedValue == "0")
        {
            ddlULB.Items.Clear();
            ddlBlock.Items.Clear();
        }
        else
        {
            get_tbl_ULB(Convert.ToInt32(ddlDistrict.SelectedValue));
            get_tbl_Block(Convert.ToInt32(ddlDistrict.SelectedValue));
        }
    }

    private void get_tbl_ULB(int District_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ULB(District_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlULB, "ULB_Name", "ULB_Id");
        }
        else
        {
            ddlULB.Items.Clear();
        }
    }
    private void get_tbl_Block(int District_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(4, District_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlBlock, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlBlock.Items.Clear();
        }
    }
    protected void ddlProjectMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProjectMaster.SelectedValue == "0")
        {
            txtProjectWorkName.Text = "";
        }
        else
        {
            txtProjectWorkName.Text = ddlProjectMaster.SelectedItem.Text.Trim();
        }
    }

    protected void rbtMappingWith_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtMappingWith.SelectedValue == "D")
        {
            lblULB.Visible = false;
            ddlULB.Visible = false;
            divCircle.Visible = true;
            divDivision.Visible = true;

            lblZone.Visible = true;
            ddlZone.Visible = true;
        }
        else
        {
            lblZone.Visible = false;
            ddlZone.Visible = false;
            divCircle.Visible = false;
            divDivision.Visible = false;

            lblULB.Visible = true;
            ddlULB.Visible = true;
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

    protected void txtShareP_TextChanged(object sender, EventArgs e)
    {

    }

    protected void grdFundingPattern_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkUpdate = sender as ImageButton;
        hf_ProjectWork_Id.Value = (lnkUpdate.Parent.Parent as GridViewRow).Cells[0].Text.Trim();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWork_Edit(Convert.ToInt32(hf_ProjectWork_Id.Value));
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_DistrictId"].ToString();
            }
            catch
            {
                ddlDistrict.SelectedValue = "0";
            }

            ddlDistrict_SelectedIndexChanged(null, null);
            try
            {
                ddlBlock.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_DistrictId"].ToString();
            }
            catch
            {
                ddlBlock.SelectedValue = "0";
            }
            try
            {
                ddlBlock.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_BlockId"].ToString();
            }
            catch
            {
                ddlBlock.SelectedValue = "0";
            }
            try
            {
                ddlULB.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_ULB_Id"].ToString();
            }
            catch
            {
                ddlULB.SelectedValue = "0";
            }
            try
            {
                ddlZone.SelectedValue = ds.Tables[0].Rows[0]["Circle_ZoneId"].ToString();
            }
            catch
            {
                ddlZone.SelectedValue = "0";
            }
            ddlZone_SelectedIndexChanged(null, null);
            try
            {
                ddlCircle.SelectedValue = ds.Tables[0].Rows[0]["Division_CircleId"].ToString();
            }
            catch
            {
                ddlCircle.SelectedValue = "0";
            }
            ddlCircle_SelectedIndexChanged(null, null);
            try
            {
                ddlDivision.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_DivisionId"].ToString();
            }
            catch
            {
                ddlDivision.SelectedValue = "0";
            }
            try
            {
                ddlProjectMaster.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_Project_Id"].ToString();
            }
            catch
            {
                ddlProjectMaster.SelectedValue = "0";
            }
            try
            {
                ddlProjectType.SelectedValue = ds.Tables[0].Rows[0]["ProjectWork_ProjectType_Id"].ToString();
            }
            catch
            {
                ddlProjectType.SelectedValue = "0";
            }
            txtProjectCode.Text = ds.Tables[0].Rows[0]["ProjectWork_ProjectCode"].ToString();
            txtProjectWorkName.Text = ds.Tables[0].Rows[0]["ProjectWork_Name"].ToString();
            txtProjectWorkDesc.Text = ds.Tables[0].Rows[0]["ProjectWork_Description"].ToString();
            txtBudget.Text = ds.Tables[0].Rows[0]["ProjectWork_Budget"].ToString();
            txtCentage.Text = ds.Tables[0].Rows[0]["ProjectWork_Centage"].ToString();
            txtGONo.Text = ds.Tables[0].Rows[0]["ProjectWork_GO_No"].ToString();
            txtGODate.Text = ds.Tables[0].Rows[0]["ProjectWork_GO_Date"].ToString();
        }
        else
        {
            reset();
        }
        if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            grdFundingPattern.DataSource = ds.Tables[1];
            grdFundingPattern.DataBind();
        }
        else
        {
            grdFundingPattern.DataSource = null;
            grdFundingPattern.DataBind();
        }
        if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
        {
            ViewState["AdditionalDivision"] = ds.Tables[2];
            dgvAdditionalDivision.DataSource = ds.Tables[2];
            dgvAdditionalDivision.DataBind();
        }
        else
        {
            ViewState["AdditionalDivision"] = null;
            dgvAdditionalDivision.DataSource = null;
            dgvAdditionalDivision.DataBind();
        }
      
       
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Person_Id_Delete = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        if (new DataLayer().Delete_ProjectWork(Person_Id_Delete, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            get_tbl_ProjectWork();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            return;
        }
    }
    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
            if (Session["UserType"].ToString() == "1")
            {
                btnDelete.Visible = true;
            }
            else
            {
                btnDelete.Visible = false;
            }
        }
    }

    protected void ddlAdditionZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdditionZone.SelectedValue == "0")
        {
            ddlAdditionalCircle.Items.Clear();
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            get_tbl_Circle_Additional(Convert.ToInt32(ddlAdditionZone.SelectedValue));
        }
    }

    protected void ddlAdditionalCircle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdditionalCircle.SelectedValue == "0")
        {
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            get_tbl_Division_Additional(Convert.ToInt32(ddlAdditionalCircle.SelectedValue));
        }
    }
    private void get_tbl_Circle_Additional(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(Zone_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionalCircle, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlAdditionalCircle.Items.Clear();
        }
    }
    private void get_tbl_Division_Additional(int Circle_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(Circle_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionalDivision, "Division_Name", "Division_Id");
        }
        else
        {
            ddlAdditionalDivision.Items.Clear();
        }
    }

    protected void btnAddAdditionalDivision_Click(object sender, EventArgs e)
    {
        if (ddlAdditionZone.SelectedValue == "0" || ddlAdditionZone.SelectedValue == "")
        {
            MessageBox.Show("Please Select Zone");
            ddlAdditionZone.Focus();
            return;
        }
        if (ddlAdditionalCircle.SelectedValue == "0" || ddlAdditionalCircle.SelectedValue == "")
        {
            MessageBox.Show("Please Select Circle");
            ddlAdditionalCircle.Focus();
            return;
        }
        if (ddlAdditionalDivision.SelectedValue == "0" || ddlAdditionalDivision.SelectedValue == "")
        {
            MessageBox.Show("Please Select Divison");
            ddlAdditionalDivision.Focus();
            return;
        }

        if (ViewState["AdditionalDivision"] != null)
        {
            DataTable dt = (DataTable)ViewState["AdditionalDivision"];

            DataRow dr = dt.NewRow();

            dr["ProjectAdditionalArea_ZoneId"] = ddlAdditionZone.SelectedValue;
            dr["ProjectAdditionalArea_CircleId"] = ddlAdditionalCircle.SelectedValue;
            dr["ProjectAdditionalArea_DevisionId"] = ddlAdditionalDivision.SelectedValue;
            dr["Zone_Name"] = ddlAdditionZone.SelectedItem.Text.Trim();
            dr["Circle_Name"] = ddlAdditionalCircle.SelectedItem.Text.Trim();
            dr["Division_Name"] = ddlAdditionalDivision.SelectedItem.Text.Trim();
            dt.Rows.Add(dr);

            ViewState["AdditionalDivision"] = dt;

            dgvAdditionalDivision.DataSource = dt;
            dgvAdditionalDivision.DataBind();
            ddlAdditionZone.SelectedValue = "0";
            ddlAdditionalCircle.Items.Clear();
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            DataSet ds = (new DataLayer()).get_tbl_ProjectWork_Edit(0);
            try
            {
                ViewState["AdditionalDivision"] = ds.Tables[2];
            }
            catch
            {
                ViewState["AdditionalDivision"] = null;
            }
        }

    }

    protected void lnkDeleteAdditionalDivision_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["AdditionalDivision"] != null)
        {
            if (index >= 0)
            {
                DataTable dt = (DataTable)ViewState["AdditionalDivision"];
                dt.Rows.RemoveAt(index);
                dgvAdditionalDivision.DataSource = dt;
                dgvAdditionalDivision.DataBind();
                ViewState["AdditionalDivision"] = dt;
            }

        }
    }
}