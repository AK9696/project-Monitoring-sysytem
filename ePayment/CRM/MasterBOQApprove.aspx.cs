using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterBOQApprove : System.Web.UI.Page
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
                AllClasses.FillDropDown((DataTable)ViewState["Unit"], ddlUnit, "Unit_Name", "Unit_Id");
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

            Button btnApprove = e.Row.FindControl("btnApprove") as Button;
            Button btnQtyVariation = e.Row.FindControl("btnQtyVariation") as Button;

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

                btnApprove.Visible = false;
                btnQtyVariation.Visible = false;
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

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as Button).Parent.Parent as GridViewRow;
        TextBox txtQty = gr.FindControl("txtQty") as TextBox;
        List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li = new List<tbl_PackageBOQ_Approval>();
        tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = Convert.ToDecimal(txtQty.Text);
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = "";
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = "";
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
        obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";
        obj_tbl_PackageBOQ_Approval_Li.Add(obj_tbl_PackageBOQ_Approval);

        if (new DataLayer().Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval_Li, null))
        {
            MessageBox.Show("BOQ Details Approved Successfully");
            get_tbl_PackageBOQ(Convert.ToInt32(hf_ProjectWork_Id.Value));
            return;
        }
        else
        {
            MessageBox.Show("Error In BOQ Details Approval");
            return;
        }
    }

    protected void btnLock_Click(object sender, EventArgs e)
    {
        List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li = new List<tbl_PackageBOQ_Approval>();
        for (int i = 0; i < grdBOQ.Rows.Count; i++)
        {
            int is_Approved= 0;
            try
            {
                is_Approved = Convert.ToInt32(grdBOQ.Rows[i].Cells[3].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }
            if (is_Approved == 0)
            {
                TextBox txtQty = grdBOQ.Rows[i].FindControl("txtQty") as TextBox;
                tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = Convert.ToDecimal(txtQty.Text);
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = "";
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = "";
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = Convert.ToInt32(grdBOQ.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
                obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";
                obj_tbl_PackageBOQ_Approval_Li.Add(obj_tbl_PackageBOQ_Approval);
            }
        }
        int ProjectWorkPkg_Id = 0;
        try
        {
            ProjectWorkPkg_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
        }
        catch
        {
            ProjectWorkPkg_Id = 0;
        }
        if (new DataLayer().Update_tbl_ProjectWorkPkg_Lock(obj_tbl_PackageBOQ_Approval_Li, ProjectWorkPkg_Id, Convert.ToInt32(Session["Person_Id"].ToString())))
        {
            MessageBox.Show("Package BOQ Locked Successfully");
            return;
        }
        else
        {
            MessageBox.Show("Error In  Package BOQ Locking");
            return;
        }
    }

    protected void btnQtyVariation_Click(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as Button).Parent.Parent as GridViewRow;
        TextBox txtQty = gr.FindControl("txtQty") as TextBox;
        hf_PackageBOQ_Id.Value = gr.Cells[0].Text.Trim() + "|" + txtQty.Text.Trim();
        mp1.Show();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string[] _data = hf_PackageBOQ_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (_data == null)
        {
            MessageBox.Show("Error Occured");
            return;
        }
        else
        {
            List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li = new List<tbl_PackageBOQ_Approval>();
            tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = Convert.ToDecimal(_data[1]);
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = txtComments.Text.Trim();
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = txtApprovalDate.Text.Trim();
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = txtApprovalNo.Text.Trim();
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = Convert.ToInt32(_data[0]);
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
            obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";
            obj_tbl_PackageBOQ_Approval_Li.Add(obj_tbl_PackageBOQ_Approval);
            decimal Approved_Qty = Convert.ToDecimal(txtApprovedQty.Text.Trim());

            if (new DataLayer().Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval_Li, Approved_Qty))
            {
                MessageBox.Show("BOQ Details Approved Successfully");
                mp1.Hide();
                get_tbl_PackageBOQ(Convert.ToInt32(hf_ProjectWork_Id.Value));
                return;
            }
            else
            {
                MessageBox.Show("Error In BOQ Details Qty Variation Approval");
                return;
            }
        }
    }

    protected void btnApproveBulk_Click(object sender, EventArgs e)
    {
        List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li = new List<tbl_PackageBOQ_Approval>();
        for (int i = 0; i < grdBOQ.Rows.Count; i++)
        {
            GridViewRow gr = grdBOQ.Rows[i];
            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(gr.Cells[3].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }
            if (is_Approved == 0)
            {
                TextBox txtQty = gr.FindControl("txtQty") as TextBox;

                tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = Convert.ToDecimal(txtQty.Text);
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = "";
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = "";
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
                obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";
                obj_tbl_PackageBOQ_Approval_Li.Add(obj_tbl_PackageBOQ_Approval);
            }
        }
        
        if (new DataLayer().Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval_Li, null))
        {
            MessageBox.Show("BOQ Details Approved Successfully");
            get_tbl_PackageBOQ(Convert.ToInt32(hf_ProjectWork_Id.Value));
            return;
        }
        else
        {
            MessageBox.Show("Error In BOQ Details Approval");
            return;
        }
    }
}
