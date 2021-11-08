using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PreviousGenerateInvoice : System.Web.UI.Page
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
            txtMBDate.Text = Session["ServerDate"].ToString();
            get_tbl_Unit();
            get_tbl_Project();
            get_tbl_Zone();
            get_tbl_Deduction1();
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
        hf_ProjectWorkPkg_Id.Value = "0";
        txtAmount.Text = "0";
        txtRABillNo.Text = "";
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divEntry.Visible = true;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        gr.BackColor = Color.LightGreen;
        string end_Date = gr.Cells[20].Text.Trim();

        DateTime dtEndDate = DateTime.ParseExact(end_Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        int Days = DateTime.Now.Date.Subtract(dtEndDate).Days;
        //if (Days > 1)
        //{
        //    MessageBox.Show("Due Date Of Completion of the Package has been Passed. Please Contact Administrator..!");
        //    return;
        //}

        hf_ProjectWorkPkg_Id.Value = gr.Cells[0].Text.Trim();
        hf_ProjectWork_Id.Value = gr.Cells[1].Text.Trim();
        txtAmount.Text = "0";
        txtRABillNo.Text = "";
        get_tbl_PackageInvoice(Convert.ToInt32(hf_ProjectWorkPkg_Id.Value));
    }

    private void get_tbl_PackageInvoice(int Package_Id)
    {
        DataSet ds = new DataSet();

        ds = (new DataLayer()).get_PreviousInvoiceDetails(Package_Id);

        if (AllClasses.CheckDataSet(ds))
        {
            grdInvoice.DataSource = ds.Tables[0];
            grdInvoice.DataBind();
            divPreInvoiceDetails.Visible = true;
        }
        else
        {
            divPreInvoiceDetails.Visible = false;
            grdInvoice.DataSource = null;
            grdInvoice.DataBind();
        }
    }
    private void get_tbl_Deduction1()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction(0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdDeductions.DataSource = ds.Tables[0];
            grdDeductions.DataBind();
        }
        else
        {
            grdDeductions.DataSource = null;
            grdDeductions.DataBind();
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
            MessageBox.Show("No Package Details Found");
        }
    }

    protected void btnSaveEMB_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRABillNo.Text == "")
            {
                MessageBox.Show("Please Provide RA Bill No");
                txtRABillNo.Focus();
                return;
            }
            if (txtMBDate.Text == "")
            {
                MessageBox.Show("Please Provide RA Bill Date");
                txtRABillNo.Focus();
                return;
            }

            tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
            obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageInvoice.PackageInvoice_Date = txtMBDate.Text;
            obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
            obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
            obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = txtRABillNo.Text.Trim();

            decimal Allocated_Budget = 0;
            try
            {
                Allocated_Budget = Convert.ToDecimal(txtAmount.Text);
            }
            catch
            {
                Allocated_Budget = 0;
            }
            tbl_FinancialTrans obj_tbl_FinancialTrans = new tbl_FinancialTrans();
            obj_tbl_FinancialTrans.FinancialTrans_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_FinancialTrans.FinancialTrans_Status = 1;
            obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = 0;
            obj_tbl_FinancialTrans.FinancialTrans_Date = txtMBDate.Text;
            obj_tbl_FinancialTrans.FinancialTrans_GO_Number = txtRABillNo.Text;
            obj_tbl_FinancialTrans.FinancialTrans_GO_Date = txtMBDate.Text;
            obj_tbl_FinancialTrans.FinancialTrans_Package_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
            obj_tbl_FinancialTrans.FinancialTrans_Work_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
            obj_tbl_FinancialTrans.FinancialTrans_FilePath1 = "";
            obj_tbl_FinancialTrans.FinancialTrans_TransType = "C";
            obj_tbl_FinancialTrans.FinancialTrans_EntryType = "Fund Allocated";
            obj_tbl_FinancialTrans.FinancialTrans_TransAmount = Allocated_Budget * 100000;
            obj_tbl_FinancialTrans.FinancialTrans_Comments = "";

            List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li = new List<tbl_PackageInvoiceAdditional>();

            for (int i = 0; i < grdDeductions.Rows.Count; i++)
            {
                CheckBox chkSelect = grdDeductions.Rows[i].FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked == true)
                {
                    TextBox txtDeductionValue = grdDeductions.Rows[i].FindControl("txtDeductionValue") as TextBox;
                    tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();

                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    try
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions.Rows[i].Cells[0].Text);
                    }
                    catch
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = 0;
                    }
                    try
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = Convert.ToDecimal(txtDeductionValue.Text);
                    }
                    catch
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = 0;
                    }

                    try
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionValue.Text);
                    }
                    catch
                    {
                        obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = 0;
                    }

                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = "";
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                    obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
                }
            }

            if (new DataLayer().Insert_PreviousInvoiceGenerate(obj_tbl_PackageInvoice, obj_tbl_FinancialTrans, obj_tbl_PackageInvoiceAdditional_Li))
            {
                MessageBox.Show("Invoice Details Saved Successfully");
                btnSearch_Click(null, null);
                get_tbl_PackageInvoice(Convert.ToInt32(hf_ProjectWorkPkg_Id.Value));
                return;
            }
            else
            {
                MessageBox.Show("Unable To Update Details");
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnViewDeduction_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Invoice_Id = 0;
        try
        {
            Invoice_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        }
        catch
        {
            Invoice_Id = 0;
        }


        if (Invoice_Id > 0)
        {
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction(Invoice_Id,0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                mpDeduction.Show();
                grdDeductionHistory.DataSource = ds.Tables[0];
                grdDeductionHistory.DataBind();
            }
            else
            {
                grdDeductionHistory.DataSource = null;
                grdDeductionHistory.DataBind();
            }
        }
    }
}