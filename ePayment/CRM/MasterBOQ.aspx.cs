using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterBOQ : System.Web.UI.Page
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
            get_tbl_ProcessConfigMaster();
            get_tbl_Unit();
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

    private void get_tbl_ProcessConfigMaster()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProcessConfigMaster("BOQ");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            hf_Approved.Value = "0";
        }
        else
        {
            hf_Approved.Value = "1";
        }
    }

    private void get_tbl_Unit()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Unit();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Unit"] = ds.Tables[0];
        }
        else
        {

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
        hf_ProjectWork_Id.Value = "0";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divEntry.Visible = true;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        hf_ProjectWork_Id.Value = gr.Cells[0].Text.Trim();
        gr.BackColor = Color.LightGreen;
        get_tbl_PackageBOQ(Convert.ToInt32(hf_ProjectWork_Id.Value));
    }
    private void get_tbl_PackageBOQ(int Package_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageBOQ(0, Package_Id);

        if (ds != null && ds.Tables.Count > 0)
        {
            ViewState["PackageBOQ"] = ds.Tables[0];
            grdBOQ.DataSource = ds.Tables[0];
            grdBOQ.DataBind();
        }
        else
        {
            ViewState["PackageBOQ"] = null;
            MessageBox.Show("Server Error!!");
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
        hf_ProjectWork_Id.Value = "";
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", false,"");
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

    protected void grdBOQ_PreRender(object sender, EventArgs e)
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

    protected void grdBOQ_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(e.Row.Cells[3].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }
            DropDownList ddlUnit = e.Row.FindControl("ddlUnit") as DropDownList;
            int Unit_Id = 0;
            try
            {
                Unit_Id = Convert.ToInt32(e.Row.Cells[2].Text.Trim());
            }
            catch
            {
                Unit_Id = 0;
            }

            if (ViewState["Unit"] != null)
            {
                if (e.Row.RowIndex == 0)
                    AllClasses.FillDropDown((DataTable)ViewState["Unit"], ddlUnit, "Unit_Name", "Unit_Id");
                else
                    AllClasses.FillDropDown_WithOutSelect((DataTable)ViewState["Unit"], ddlUnit, "Unit_Name", "Unit_Id");
                try
                {
                    ddlUnit.SelectedValue = Unit_Id.ToString();
                }
                catch
                {

                }
            }

            TextBox txtSpecification = e.Row.FindControl("txtSpecification") as TextBox;
            TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
            TextBox txtRateEstimate = e.Row.FindControl("txtRateEstimate") as TextBox;
            TextBox txtAmountEstimate = e.Row.FindControl("txtAmountEstimate") as TextBox;
            TextBox txtRateQuoted = e.Row.FindControl("txtRateQuoted") as TextBox;
            TextBox txtAmountQuoted = e.Row.FindControl("txtAmountQuoted") as TextBox;
            TextBox txtQtyPaid = e.Row.FindControl("txtQtyPaid") as TextBox;

            if (is_Approved > 0)
            {
                txtSpecification.Enabled = false;
                txtQty.Enabled = false;
                txtRateEstimate.Enabled = false;
                txtAmountEstimate.Enabled = false;
                txtRateQuoted.Enabled = false;
                txtAmountQuoted.Enabled = false;
                txtQtyPaid.Enabled = false;
                ddlUnit.Enabled = false;
            }
        }
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["PackageBOQ"] != null)
        {
            dt = (DataTable)ViewState["PackageBOQ"];
            DataRow dr = dt.NewRow();
            dr["PackageBOQ_Package_Id"] = 0;
            dt.Rows.Add(dr);
            ViewState["PackageBOQ"] = dt;

            grdBOQ.DataSource = dt;
            grdBOQ.DataBind();
        }
    }

    protected void btnMinus_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["PackageBOQ"] != null)
        {
            dt = (DataTable)ViewState["PackageBOQ"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows.RemoveAt(dt.Rows.Count - 1);

                grdBOQ.DataSource = dt;
                grdBOQ.DataBind();
            }
        }
    }

    protected void btnSaveBOQ_Click(object sender, EventArgs e)
    {
        List<tbl_PackageBOQ> obj_tbl_PackageBOQ_Li = new List<tbl_PackageBOQ>();
        for (int i = 0; i < grdBOQ.Rows.Count; i++)
        {
            DropDownList ddlUnit = (grdBOQ.Rows[i].FindControl("ddlUnit") as DropDownList);
            if (ddlUnit.SelectedValue == "0")
            {
                MessageBox.Show("Please Select Unit");
                ddlUnit.Focus();
                return;
            }
            TextBox txtSpecification = (grdBOQ.Rows[i].FindControl("txtSpecification") as TextBox);
            if (txtSpecification.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Specification");
                txtSpecification.Focus();
                return;
            }
            TextBox txtQtyPaid = (grdBOQ.Rows[i].FindControl("txtQtyPaid") as TextBox);
            TextBox txtQty = (grdBOQ.Rows[i].FindControl("txtQty") as TextBox);
            if (txtQty.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Quantity");
                txtQty.Focus();
                return;
            }
            TextBox txtRateEstimate = (grdBOQ.Rows[i].FindControl("txtRateEstimate") as TextBox);
            if (txtRateEstimate.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Rate Estimate");
                txtRateEstimate.Focus();
                return;
            }
            TextBox txtAmountEstimate = (grdBOQ.Rows[i].FindControl("txtAmountEstimate") as TextBox);
            if (txtAmountEstimate.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Amount Estimate");
                txtAmountEstimate.Focus();
                return;
            }
            TextBox txtRateQuoted = (grdBOQ.Rows[i].FindControl("txtRateQuoted") as TextBox);
            if (txtRateQuoted.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Rate Quoted");
                txtRateQuoted.Focus();
                return;
            }
            TextBox txtAmountQuoted = (grdBOQ.Rows[i].FindControl("txtAmountQuoted") as TextBox);
            if (txtAmountQuoted.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Amount Quoted");
                txtAmountQuoted.Focus();
                return;
            }

            tbl_PackageBOQ obj_tbl_PackageBOQ = new tbl_PackageBOQ();
            obj_tbl_PackageBOQ.PackageBOQ_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated = decimal.Parse(txtAmountEstimate.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted = decimal.Parse(txtAmountQuoted.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_RateEstimated = decimal.Parse(txtRateEstimate.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_RateEstimated = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_RateQuoted = decimal.Parse(txtRateQuoted.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_RateQuoted = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_Unit_Id = Convert.ToInt32(ddlUnit.SelectedValue);
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_Unit_Id = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_Qty = decimal.Parse(txtQty.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_Qty = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_QtyPaid = decimal.Parse(txtQtyPaid.Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_QtyPaid = 0;
            }
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_Id = Convert.ToInt32(grdBOQ.Rows[i].Cells[0].Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_Id = 0;
            }
            obj_tbl_PackageBOQ.PackageBOQ_Package_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
            obj_tbl_PackageBOQ.PackageBOQ_Specification  = txtSpecification.Text.Trim().Replace("'", "");
            try
            {
                obj_tbl_PackageBOQ.PackageBOQ_Is_Approved = Convert.ToInt32(grdBOQ.Rows[i].Cells[3].Text.Trim());
            }
            catch
            {
                obj_tbl_PackageBOQ.PackageBOQ_Is_Approved = 0;
            }
            obj_tbl_PackageBOQ.PackageBOQ_Is_Approved = Convert.ToInt32(hf_Approved.Value);
            
            obj_tbl_PackageBOQ.PackageBOQ_Status = 1;
            if (obj_tbl_PackageBOQ.PackageBOQ_Specification != "")
                obj_tbl_PackageBOQ_Li.Add(obj_tbl_PackageBOQ);
        }
        if (obj_tbl_PackageBOQ_Li.Count == 0)
        {
            MessageBox.Show("Please Add At least A Item To Save");
            return;
        }
        else
        {
            if ((new DataLayer()).Insert_tbl_PackageBOQ(obj_tbl_PackageBOQ_Li, Convert.ToInt32(hf_Approved.Value)))
            {
                MessageBox.Show("Package BOQ Created / Updated Successfully!");
                reset();
                return;
            }
            else
            {
                MessageBox.Show("Error In Creating Package BOQ!");
                return;
            }
        }
    }

    protected void lnkImport_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("BOQ_Import.aspx");
    }
}
