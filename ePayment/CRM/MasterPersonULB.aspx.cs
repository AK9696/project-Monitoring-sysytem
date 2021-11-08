using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPersonULB : System.Web.UI.Page
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
            get_Employee();
            //Department
            get_tbl_Department();
            //Designation
            get_tbl_Designation();
            //District
            get_tbl_Jurisdiction(1, 0);
        }
        if (Session["UserType"].ToString() == "1")
        {
            btnAddNew.Visible = true;
        }
        else
        {
            btnAddNew.Visible = false;
        }
    }

    private void get_Employee()
    {
        string UserTypeId = "3";
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee(UserTypeId, 0, 0, 0, 0, 0, 0, 0, "", "", 0, 0,0);
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
    private void get_tbl_Department()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Department();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDepartment, "Department_Name", "Department_Id");
            if (ddlDepartment.Items.Count == 2)
            {
                ddlDepartment.SelectedIndex = 1;
            }
        }
        else
        {
            ddlDepartment.Items.Clear();
        }
    }

    private void get_Employee_Reporting_Manager(int Designation_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee_Reporting_Manager(Designation_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlReportingManager, "Person_Name_Full", "Person_Id");
        }
        else
        {
            ddlReportingManager.Items.Clear();
        }
    }
    private void get_tbl_Designation()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Designation();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDesignation, "Designation_DesignationName", "Designation_Id");
            if (ddlDesignation.Items.Count == 2)
            {
                ddlDesignation.SelectedIndex = 1;
            }
        }
        else
        {
            ddlDesignation.Items.Clear();
        }
    }

    private void get_tbl_Jurisdiction(int Level_Id, int Parent_Jurisdiction_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(Level_Id, Parent_Jurisdiction_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Level_Id == 1)
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlState, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
            else if (Level_Id == 2)
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlMandal, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
            else if (Level_Id == 3)
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
        }
        else
        {
            if (Level_Id == 1)
            {
                ddlState.Items.Clear();
            }
            if (Level_Id == 2)
            {
                ddlMandal.Items.Clear();
            }
            if (Level_Id == 2)
            {
                ddlDistrict.Items.Clear();
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        tbl_PersonDetail obj_PersonDetail = new tbl_PersonDetail();
        try
        {
            obj_PersonDetail.Person_Id = Convert.ToInt32(hf_Person_Id.Value);
        }
        catch
        {
            obj_PersonDetail.Person_Id = 0;
        }
        if (txtPersonName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Employee Name");
            txtPersonName.Focus();
            return;
        }
        if (txtPersonFName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Employee Father's Name");
            txtPersonFName.Focus();
            return;
        }
        if (txtMobileNo1.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Employee Mobile No");
            txtMobileNo1.Focus();
            return;
        }
        if (txtMobileNo1.Text.Trim().Length != 10)
        {
            MessageBox.Show("Please Provide Correct Mobile No");
            txtMobileNo1.Focus();
            return;
        }
        if (ddlDepartment.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Department");
            ddlDepartment.Focus();
            return;
        }
        if (ddlDesignation.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Designation");
            ddlDesignation.Focus();
            return;
        }
        if (ddlState.SelectedValue == "0")
        {
            MessageBox.Show("Please Select State");
            ddlState.Focus();
            return;
        }
        if (ddlMandal.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Mandal");
            ddlMandal.Focus();
            return;
        }
        if (ddlDistrict.SelectedValue == "0")
        {
            MessageBox.Show("Please Select District");
            ddlDistrict.Focus();
            return;
        }
        if (ddlULB.SelectedValue == "0")
        {
            MessageBox.Show("Please Select ULB");
            ddlULB.Focus();
            return;
        }
        obj_PersonDetail.Person_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_PersonDetail.Person_Id = Convert.ToInt32(hf_Person_Id.Value);
        }
        catch
        {
            obj_PersonDetail.Person_Id = 0;
        }
        obj_PersonDetail.Person_BranchOffice_Id = 1;
        obj_PersonDetail.Person_AddressLine1 = txtAddress.Text.Trim();
        obj_PersonDetail.Person_EmailId = txtEmailId.Text.Trim();
        obj_PersonDetail.Person_Mobile1 = txtMobileNo1.Text.Trim();
        obj_PersonDetail.Person_Mobile2 = txtMobileNo2.Text.Trim();
        obj_PersonDetail.Person_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_PersonDetail.Person_Name = txtPersonName.Text.Trim();
        obj_PersonDetail.Person_FName = txtPersonFName.Text.Trim();
        obj_PersonDetail.Person_Status = 1;

        tbl_PersonJuridiction obj_PersonJuridiction = new tbl_PersonJuridiction();
        try
        {
            obj_PersonJuridiction.PersonJuridiction_Id = Convert.ToInt32(hf_PersonJuridiction_Id.Value);
        }
        catch
        {
            obj_PersonJuridiction.PersonJuridiction_Id = 0;
        }
        try
        {
            obj_PersonJuridiction.PersonJuridiction_PersonId = Convert.ToInt32(hf_Person_Id.Value);
        }
        catch
        {
            obj_PersonJuridiction.PersonJuridiction_PersonId = 0;
        }
        obj_PersonJuridiction.M_Level_Id = 3;
        if (obj_PersonJuridiction.M_Level_Id == 1)
        {
            obj_PersonJuridiction.M_Jurisdiction_Id = Convert.ToInt32(ddlState.SelectedValue);
        }
        else if (obj_PersonJuridiction.M_Level_Id == 2)
        {
            obj_PersonJuridiction.M_Jurisdiction_Id = Convert.ToInt32(ddlMandal.SelectedValue);
        }
        else if (obj_PersonJuridiction.M_Level_Id == 3)
        {
            obj_PersonJuridiction.M_Jurisdiction_Id = Convert.ToInt32(ddlDistrict.SelectedValue);
        }
        try
        {
            obj_PersonJuridiction.PersonJuridiction_ULB_Id = Convert.ToInt32(ddlULB.SelectedValue);
        }
        catch
        {
            obj_PersonJuridiction.PersonJuridiction_ULB_Id = 0;
        }
        obj_PersonJuridiction.PersonJuridiction_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_PersonJuridiction.PersonJuridiction_DepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
        obj_PersonJuridiction.PersonJuridiction_DesignationId = Convert.ToInt32(ddlDesignation.SelectedValue);
        obj_PersonJuridiction.PersonJuridiction_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_PersonJuridiction.PersonJuridiction_ParentPerson_Id = Convert.ToInt32(ddlReportingManager.SelectedValue);
        }
        catch
        {
            obj_PersonJuridiction.PersonJuridiction_ParentPerson_Id = 0;
        }
        obj_PersonJuridiction.PersonJuridiction_Status = 1;

        obj_PersonJuridiction.PersonJuridiction_UserTypeId = 3;
        
        tbl_UserLogin obj_UserLogin = new tbl_UserLogin();
        try
        {
            obj_UserLogin.Login_PersonId = Convert.ToInt32(hf_Person_Id.Value);
        }
        catch
        {
            obj_UserLogin.Login_PersonId = 0;
        }
        obj_UserLogin.Login_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_UserLogin.Login_password = "ok";
        obj_UserLogin.Login_Status = 1;
        obj_UserLogin.Login_UserName = "";
        string msg = "";
        if ((new DataLayer()).Insert_Employee(obj_PersonDetail, obj_PersonJuridiction, obj_UserLogin, Convert.ToInt32(hf_Person_Id.Value), null, null, null, null, null, ref msg))
        {
            if (msg == "")
            {
                if (Convert.ToInt32(hf_Person_Id.Value) > 0)
                {
                    MessageBox.Show("Employee Updated Successfully!");
                }
                else
                {
                    MessageBox.Show("Employee Created Successfully!");
                }
                reset();
                return;
            }
            else
            {
                MessageBox.Show(msg);
                return;
            }
        }
        else
        {
            MessageBox.Show("Error In Creating Employee!");
            return;
        }
    }

    private void reset()
    {
        hf_PersonJuridiction_Id.Value = "0";
        hf_Person_Id.Value = "0";
        txtAddress.Text = "";
        ddlDepartment.SelectedValue = "0";
        ddlDesignation.SelectedValue = "0";
        txtEmailId.Text = "";
        txtMobileNo1.Text = "";
        txtMobileNo2.Text = "";
        txtAddress.Text = "";
        txtEmailId.Text = "";
        txtPersonFName.Text = "";
        txtPersonName.Text = "";
        get_Employee();
        divCreateNew.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Person_Id_Delete = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        if (new DataLayer().Delete_Person(Person_Id_Delete, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            get_Employee();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
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

    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        M_Jurisdiction_Detailed obj = new M_Jurisdiction_Detailed();
        ImageButton lnkUpdate = sender as ImageButton;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        (lnkUpdate.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        divCreateNew.Visible = true;
        hf_PersonJuridiction_Id.Value = (lnkUpdate.Parent.Parent as GridViewRow).Cells[1].Text.Trim();
        hf_Person_Id.Value = (lnkUpdate.Parent.Parent as GridViewRow).Cells[0].Text.Trim();
        GridViewRow grd = lnkUpdate.Parent.Parent as GridViewRow;
        string M_Jurisdiction_Id = grd.Cells[3].Text.Replace("&nbsp;", "").Trim();
        obj = (new DataLayer()).get_M_Jurisdiction_Detailed("3", M_Jurisdiction_Id);
        if (obj.State_Id > 0)
        {
            ddlState.SelectedValue = obj.State_Id.ToString();
            ddlState_SelectedIndexChanged(ddlState, e);
        }
        if (obj.Mandal_Id > 0)
        {
            ddlMandal.SelectedValue = obj.Mandal_Id.ToString();
            ddlMandal_SelectedIndexChanged(ddlMandal, e);
        }
        if (obj.District_Id > 0)
        {
            ddlDistrict.SelectedValue = obj.District_Id.ToString();
            ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
        }
        try
        {
            ddlDesignation.SelectedValue = grd.Cells[4].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlDesignation.SelectedValue = "0";
        }
        try
        {
            ddlDepartment.SelectedValue = grd.Cells[5].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlDepartment.SelectedValue = "0";
        }
        try
        {
            ddlReportingManager.SelectedValue = grd.Cells[7].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlReportingManager.SelectedValue = "0";
        }
        try
        {
            ddlULB.SelectedValue = grd.Cells[22].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlULB.SelectedValue = "0";
        }
        txtPersonName.Text = grd.Cells[10].Text.Replace("&nbsp;", "").Trim();
        txtPersonFName.Text = grd.Cells[11].Text.Replace("&nbsp;", "").Trim();
        txtAddress.Text = grd.Cells[15].Text.Replace("&nbsp;", "").Trim();
        txtEmailId.Text = grd.Cells[23].Text.Replace("&nbsp;", "").Trim();
        txtLandLine.Text = grd.Cells[12].Text.Replace("&nbsp;", "").Trim();
        txtMobileNo1.Text = grd.Cells[13].Text.Replace("&nbsp;", "").Trim();
        txtMobileNo2.Text = grd.Cells[14].Text.Replace("&nbsp;", "").Trim();
        divCreateNew.Focus();
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        reset();
        divCreateNew.Visible = true;
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0")
        {
            ddlMandal.Items.Clear();
        }
        else
        {
            get_tbl_Jurisdiction(2, Convert.ToInt32(ddlState.SelectedValue));
        }
    }

    protected void ddlMandal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMandal.SelectedValue == "0")
        {
            ddlDistrict.Items.Clear();
        }
        else
        {
            get_tbl_Jurisdiction(3, Convert.ToInt32(ddlMandal.SelectedValue));
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedValue == "0")
        {
            ddlULB.Items.Clear();
        }
        else
        {
            get_Tbl_ULB(Convert.ToInt32(ddlDistrict.SelectedValue));
        }
    }

    private void get_Tbl_ULB(int District_Id)
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

    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDesignation.SelectedValue == "0")
        {
            ddlReportingManager.Items.Clear();
        }
        else
        {
            get_Employee_Reporting_Manager(Convert.ToInt32(ddlDesignation.SelectedValue));
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
}