using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterULBAccountLinking : System.Web.UI.Page
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
            get_tbl_Project();
            get_tbl_Bank();
            get_M_Jurisdiction();
            get_tbl_Zone();
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
        }
    }
    private void get_tbl_Bank()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Bank();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlBank, "Bank_Name", "Bank_Id");
        }
        else
        {
            ddlBank.Items.Clear();
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
    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedValue == "0")
        {
            ddlULB.Items.Clear();
        }
        else
        {
            get_tbl_ULB(Convert.ToInt32(ddlDistrict.SelectedValue));
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            return;
        }
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
        get_tbl_ULBAccountDtls(District_Id, Zone_Id, Circle_Id, Division_Id);
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
    private void get_tbl_ULBAccountDtls(int District_Id, int Zone_Id, int Circle_Id, int Division_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ULBAccountDtls(District_Id, Zone_Id, Circle_Id, Division_Id);
        if (AllClasses.CheckDataSet(ds))
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

    private void reset()
    {

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
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

    protected void btnSearchIFSC_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = new DataLayer().get_Branch_List(txtIFSC_Search.Text.Trim());
        if (AllClasses.CheckDataSet(ds))
        {
            try
            {
                ddlBank.SelectedValue = ds.Tables[0].Rows[0]["Bank_Id"].ToString();
            }
            catch
            {
                ddlBank.SelectedValue = "0";
            }
            txtBranchName.Text = ds.Tables[0].Rows[0]["Branch"].ToString() + ", " + ds.Tables[0].Rows[0]["City"].ToString() + ", " + ds.Tables[0].Rows[0]["District"].ToString();
            txtIFSCCode.Text = txtIFSC_Search.Text.Trim();
        }
        else
        {
            ddlBank.SelectedValue = "0";
            txtIFSCCode.Text = "";
            txtBranchName.Text = "";
        }
    }

    protected void btnSaveAccount_Click(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Zone");
            return;
        }
        if (ddlCircle.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Circle");
            return;
        }
        if (ddlDivision.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Division");
            return;
        }
        if (ddlBank.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Bank");
            return;
        }
        if (txtIFSCCode.Text.Trim() == "")
        {
            MessageBox.Show("Please Fill / Search For A IFSC No.");
            return;
        }
        if (txtAccountNo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Account No.");
            return;
        }
        if (ddlProjectMaster.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            return;
        }
        tbl_ULBAccountDtls obj_tbl_ULBAccountDtls = new tbl_ULBAccountDtls();
        obj_tbl_ULBAccountDtls.ULBAccount_AccountNo = txtAccountNo.Text.Trim();
        obj_tbl_ULBAccountDtls.ULBAccount_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ULBAccountDtls.ULBAccount_BankId = Convert.ToInt32(ddlBank.SelectedValue);
        obj_tbl_ULBAccountDtls.ULBAccount_SchemeId = Convert.ToInt32(ddlProjectMaster.SelectedValue);
        try
        {
            obj_tbl_ULBAccountDtls.ULBAccount_ULB_Id = Convert.ToInt32(ddlULB.SelectedValue);
        }
        catch
        {
            obj_tbl_ULBAccountDtls.ULBAccount_ULB_Id = 0;
        }
        try
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Zone_Id= Convert.ToInt32(ddlZone.SelectedValue);
        }
        catch
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Zone_Id = 0;
        }
        try
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Circle_Id= Convert.ToInt32(ddlCircle.SelectedValue);
        }
        catch
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Circle_Id = 0;
        }
        try
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Division_Id= Convert.ToInt32(ddlDivision.SelectedValue);
        }
        catch
        {
            obj_tbl_ULBAccountDtls.ULBAccount_Division_Id = 0;
        }
        obj_tbl_ULBAccountDtls.ULBAccount_BranchAddress = txtBranchAddress.Text.Trim();
        obj_tbl_ULBAccountDtls.ULBAccount_BranchName = txtBranchName.Text.Trim();
        obj_tbl_ULBAccountDtls.ULBAccount_IFSCCode = txtIFSCCode.Text.Trim();
        obj_tbl_ULBAccountDtls.ULBAccount_Status = 1;
        string Msg = "";
        if (new DataLayer().Insert_tbl_ULBAccountDtls(obj_tbl_ULBAccountDtls, ref Msg))
        {
            MessageBox.Show("Account Details Created Successfully");
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
            get_tbl_ULBAccountDtls(District_Id, Zone_Id, Circle_Id, Division_Id);
        }
        else
        {
            MessageBox.Show("Error In Saving Account Details");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ImageButton btnDelete = sender as ImageButton;
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int ULBAccount_Id = Convert.ToInt32((btnDelete.Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        if (new DataLayer().Delete_tbl_ULBAccountDtls(ULBAccount_Id, Person_Id))
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
}
