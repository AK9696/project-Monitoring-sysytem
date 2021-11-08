using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PackageAdditionalDepartmentPayment : System.Web.UI.Page
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
            get_tbl_Zone();
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
                    // ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
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
                            //ddlULB.SelectedValue = Session["ULB_Id"].ToString();
                            //ddlULB.Enabled = false;
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
    private void get_tbl_Deduction()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction_Mode(0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Deduction"] = ds.Tables[0];
        }
        else
        {
            ViewState["Deduction"] = null;
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
    private void get_tbl_ADP_Item()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ADP_Item_PackageWise(Convert.ToInt32(hf_Package_Id.Value));
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdADPItems.DataSource = ds.Tables[0];
            grdADPItems.DataBind();

        }
        else
        {
            grdADPItems.DataSource = null;
            grdADPItems.DataBind();
        }
    }
    protected void grdADPItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int Package_ADP_Item_Item_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
            GridView grdTaxes = e.Row.FindControl("grdTaxes") as GridView;
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction_WithPackageADPTax(Package_ADP_Item_Item_Id);
            get_tbl_Deduction();
            if (AllClasses.CheckDataSet(ds))
            {
                grdTaxes.DataSource = ds.Tables[0];
                grdTaxes.DataBind();
            }
            else
            {
                grdTaxes.DataSource = null;
                grdTaxes.DataBind();
            }
        }
    }
    protected void grdTaxes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlTaxes = e.Row.FindControl("ddlTaxes") as DropDownList;

            if (ViewState["Deduction"] != null)
            {
                DataTable dt = (DataTable)ViewState["Deduction"];
                if (e.Row.RowIndex == 0)
                {
                    AllClasses.FillDropDown(dt, ddlTaxes, "Deduction_Name", "Deduction_Id");
                }
                else
                {
                    AllClasses.FillDropDown_WithOutSelect(dt, ddlTaxes, "Deduction_Name", "Deduction_Id");
                }
            }
            else
            {
                ddlTaxes.Items.Clear();
            }

            try
            {
                ddlTaxes.SelectedValue = e.Row.Cells[1].Text;
            }
            catch
            {

            }

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

    private void reset()
    {
        divEntry.Visible = false;
        hf_Package_Id.Value = "0";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divEntry.Visible = true;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        hf_Package_Id.Value = gr.Cells[0].Text.Trim();
        gr.BackColor = Color.LightGreen;
        get_tbl_Deduction();
        get_tbl_ADP_Item();
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
        hf_Package_Id.Value = "";
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", false, "");
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<tbl_Package_ADP_Item> obj_Package_ADP_Item_Li = new List<tbl_Package_ADP_Item>();
            for (int i = 0; i < grdADPItems.Rows.Count; i++)
            {
                CheckBox chkSelect = grdADPItems.Rows[i].FindControl("chkSelect") as CheckBox;
                TextBox txtRate = grdADPItems.Rows[i].FindControl("txtRate") as TextBox;
                TextBox txtQty = grdADPItems.Rows[i].FindControl("txtQty") as TextBox;
                if (chkSelect.Checked == true)
                {
                    tbl_Package_ADP_Item obj_Package_ADP_Item = new tbl_Package_ADP_Item();
                    obj_Package_ADP_Item.Package_ADP_Item_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_Package_ADP_Item.Package_ADP_Item_Item_Id = Convert.ToInt32(grdADPItems.Rows[i].Cells[1].Text);
                    obj_Package_ADP_Item.Package_ADP_Item_Package_Id = Convert.ToInt32(hf_Package_Id.Value);
                    obj_Package_ADP_Item.Package_ADP_Item_Category_Id = Convert.ToInt32(grdADPItems.Rows[i].Cells[2].Text);
                    obj_Package_ADP_Item.Package_ADP_Item_Specification = grdADPItems.Rows[i].Cells[1].Text;
                    obj_Package_ADP_Item.Package_ADP_Item_Unit_Id = Convert.ToInt32(grdADPItems.Rows[i].Cells[2].Text);
                    try
                    {
                        obj_Package_ADP_Item.Package_ADP_Item_Rate = Convert.ToDecimal(txtRate.Text);
                    }
                    catch
                    {
                        obj_Package_ADP_Item.Package_ADP_Item_Rate = 0;
                    }
                    try
                    {
                        obj_Package_ADP_Item.Package_ADP_Item_Qty = Convert.ToDecimal(txtQty.Text);
                    }
                    catch
                    {
                        obj_Package_ADP_Item.Package_ADP_Item_Qty = 0;
                    }
                    obj_Package_ADP_Item.Package_ADP_Item_Status = 1;
                    List<tbl_Package_ADP_Item_Tax> obj_tbl_Package_ADP_Item_Tax_Li = new List<tbl_Package_ADP_Item_Tax>();
                    GridView grdTaxes = grdADPItems.Rows[i].FindControl("grdTaxes") as GridView;
                    for (int k = 0; k < grdTaxes.Rows.Count; k++)
                    {
                        tbl_Package_ADP_Item_Tax obj_tbl_Package_ADP_Item_Tax = new tbl_Package_ADP_Item_Tax();
                        TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                        DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;
                        obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                        obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Package_Id = Convert.ToInt32(hf_Package_Id.Value);
                        try
                        {
                            obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
                        }
                        catch
                        {
                            obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Deduction_Id = 0;
                        }
                        try
                        {
                            obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
                        }
                        catch
                        {
                            obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Value = 0;
                        }
                        obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Status = 1;

                        if (obj_tbl_Package_ADP_Item_Tax.Package_ADP_Item_Tax_Deduction_Id > 0)
                        {
                            obj_tbl_Package_ADP_Item_Tax_Li.Add(obj_tbl_Package_ADP_Item_Tax);
                        }
                    }
                    if (obj_tbl_Package_ADP_Item_Tax_Li != null)
                    {
                        obj_Package_ADP_Item.tbl_Package_ADP_Item_Tax = obj_tbl_Package_ADP_Item_Tax_Li;
                    }
                    obj_Package_ADP_Item_Li.Add(obj_Package_ADP_Item);
                }
            }

            if (obj_Package_ADP_Item_Li == null)
            {
                MessageBox.Show("Please Select At One One Item.");
                   return;
            }
            if (new DataLayer().Insert_tbl_Package_ADP_Item(obj_Package_ADP_Item_Li))
            {
                MessageBox.Show("Items Saved Successfully");
                reset();
                return;
            }
            else
            {
                MessageBox.Show("Error In  Package Items.");
                return;
            }
        }
        catch
        {

        }
    }
}