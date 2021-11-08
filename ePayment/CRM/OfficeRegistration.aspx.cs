using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OfficeRegistration : System.Web.UI.Page
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
            get_State();
            get_Branch_Office_Details();
        }
    }
    private void get_Branch_Office_Details()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Branch_Office_Details(0);
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

    private void get_State()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(1, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlState, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlState.Items.Clear();
        }
    }

    private void get_District(int State_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction_District_Additional(State_Id, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlDistrict.Items.Clear();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtOfficeBranchName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Branch Office Name!");
            txtOfficeBranchName.Focus();
            return;
        }
        if (txtMobileNo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Branch Office Mobile No!");
            txtMobileNo.Focus();
            return;
        }
        if (txtAddress.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Branch Office Address!");
            txtAddress.Focus();
            return;
        }
        if (ddlDistrict.SelectedValue == "0")
        {
            MessageBox.Show("Please Provide Branch Office District!");
            ddlDistrict.Focus();
            return;
        }
        if (ddlState.SelectedValue == "0")
        {
            MessageBox.Show("Please Provide Branch Office State!");
            ddlState.Focus();
            return;
        }
        tbl_OfficeBranch obj_OfficeBranch = new tbl_OfficeBranch();
        obj_OfficeBranch.OfficeBranch_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_OfficeBranch.OfficeBranch_Id = Convert.ToInt32(hf_OfficeBranch_Id.Value);
        }
        catch
        {
            obj_OfficeBranch.OfficeBranch_Id = 0;
        }
        obj_OfficeBranch.OfficeBranch_EmailId = txtEmailId.Text.Trim();
        obj_OfficeBranch.OfficeBranch_FullAddress = txtAddress.Text.Trim();
        obj_OfficeBranch.OfficeBranch_GSTIN = txtGSTN.Text.Trim();
        obj_OfficeBranch.OfficeBranch_IsHO = "1";

        obj_OfficeBranch.OfficeBranch_JurisdictionId = Convert.ToInt32(ddlDistrict.SelectedValue);
        obj_OfficeBranch.OfficeBranch_LL = txtLandLineNo.Text.Trim();
        obj_OfficeBranch.OfficeBranch_Mobile = txtMobileNo.Text.Trim();
        obj_OfficeBranch.OfficeBranch_Name = txtOfficeBranchName.Text.Trim();
        obj_OfficeBranch.OfficeBranch_PANNumber = "";
        obj_OfficeBranch.OfficeBranch_RegistrationNo = rbtOrgType.SelectedItem.Value;
        obj_OfficeBranch.OfficeBranch_Status = 1;
        obj_OfficeBranch.OfficeBranch_WebSite = "";
        if (flLogo.HasFile)
        {
            obj_OfficeBranch.OfficeBranch_Logo_Bytes = flLogo.FileBytes;
        }
        if ((new DataLayer()).Insert_Branch_Office(obj_OfficeBranch))
        {
            MessageBox.Show("Branch Office Created Successfully!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Creating Branch Office!");
            return;
        }
    }

    private void reset()
    {
        hf_OfficeBranch_Id.Value = "0";
        txtAddress.Text = "";
        txtEmailId.Text = "";
        txtGSTN.Text = "";
        txtLandLineNo.Text = "";
        txtMobileNo.Text = "";
        txtOfficeBranchName.Text = "";
        ddlDistrict.Items.Clear();
        ddlState.SelectedValue = "0";
        get_Branch_Office_Details();
    }

    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        ImageButton chk = sender as ImageButton;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        (chk.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        GridViewRow grd = chk.Parent.Parent as GridViewRow;
        hf_OfficeBranch_Id.Value = grd.Cells[0].Text.Trim();
        try
        {
            ddlState.SelectedValue = grd.Cells[1].Text.Trim();
            ddlState_SelectedIndexChanged(ddlState, e);
        }
        catch
        { }
        try
        {
            ddlDistrict.SelectedValue = grd.Cells[2].Text.Trim();
        }
        catch
        { }
        txtAddress.Text = grd.Cells[11].Text.Replace("&nbsp;", "").Trim();
        
        txtEmailId.Text = grd.Cells[13].Text.Replace("&nbsp;", "").Trim();
        txtGSTN.Text = grd.Cells[12].Text.Replace("&nbsp;", "").Trim();
        txtLandLineNo.Text = grd.Cells[9].Text.Replace("&nbsp;", "").Trim();
        txtMobileNo.Text = grd.Cells[10].Text.Replace("&nbsp;", "").Trim();
        
        string fileName = grd.Cells[17].Text.Replace("&nbsp;", "").Trim();
        string webURI = "";
        if (Page.Request.Url.Query.Trim() == "")
        {
            webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "") + fileName).Replace("\\", "/");
        }
        else
        {
            webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace(Page.Request.Url.Query, "") + fileName).Replace("\\", "/");
        }
        imgPreview.ImageUrl = webURI;
        try
        {
            rbtOrgType.SelectedValue = grd.Cells[7].Text.Trim();
        }
        catch
        { }
        txtOfficeBranchName.Text = grd.Cells[8].Text.Replace("&nbsp;", "").Trim();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0")
        {
            ddlDistrict.Items.Clear();
        }
        else
        {
            get_District(Convert.ToInt32(ddlState.SelectedValue));
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
}
