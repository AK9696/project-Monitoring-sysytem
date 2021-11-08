using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterGenerateInvoice_View : System.Web.UI.Page
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
            int Package_Id = 0;
            int Invoice_Id = 0;
            int Division_Id = 0;
            int Scheme_Id = 0;

            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString()) > 0)
            {//Division
                Division_Id = Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString());
            }

            if (Request.QueryString.Count > 0)
            {
                try
                {
                    Package_Id = Convert.ToInt32(Request.QueryString["Package_Id"].ToString());
                }
                catch
                {
                    Package_Id = 0;
                }

                try
                {
                    Invoice_Id = Convert.ToInt32(Request.QueryString["Invoice_Id"].ToString());
                }
                catch
                {
                    Invoice_Id = 0;
                }

                try
                {
                    Division_Id = Convert.ToInt32(Request.QueryString["Division_Id"].ToString());
                }
                catch
                {
                    Division_Id = 0;
                }

                try
                {
                    Scheme_Id = Convert.ToInt32(Request.QueryString["Scheme_Id"].ToString());
                }
                catch
                {
                    Scheme_Id = 0;
                }
            }
            else
            {
                Invoice_Id = 0;
                Package_Id = 0;
                //Division_Id = Division_Id;
                try
                {
                    Scheme_Id = Convert.ToInt32(ddlSearchScheme.SelectedValue);
                }
                catch
                {
                    Scheme_Id = 0;
                }

                
            }

            if (Invoice_Id > 0)
            {
                hf_Invoice_Id.Value = Invoice_Id.ToString();
                get_tbl_Invoice_Details(Invoice_Id);
            }
            else
            {
                get_tbl_PackageInvoice(Package_Id, Division_Id, Scheme_Id);
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

    private void get_tbl_PackageInvoice(int Package_Id, int Division_Id, int Scheme_Id)
    {
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, Division_Id, Scheme_Id, 0, 0, true);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, Division_Id, Scheme_Id, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), true);
        }
        if (AllClasses.CheckDataSet(ds))
        {
            grdInvoice.DataSource = ds.Tables[0];
            grdInvoice.DataBind();
        }
        else
        {
            MessageBox.Show("No Invoice Generated!!");
            return;
        }
    }
    protected void btnOpenInvoice_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Invoice_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        hf_Invoice_Id.Value = Invoice_Id.ToString();
        get_tbl_Invoice_Details(Invoice_Id);
    }

    private void get_tbl_Invoice_Details(int Invoice_Id)
    {
        List<Bill_Info> obj_Bill_Info_Li = new List<Bill_Info>();
        List<PackageInvoiceAdditional> obj_PackageInvoiceAdditional_Li = new List<PackageInvoiceAdditional>();
        DataSet ds = new DataSet();
        decimal deduction_total = 0;
        decimal addition_total = 0;
        decimal invoice_total = 0;
        decimal grand_total = 0;
        ds = (new DataLayer()).get_tbl_Invoice_Details(Invoice_Id);
        if (ds != null)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Bill_Info obj_Bill_Info = new Bill_Info();
                        obj_Bill_Info.Contractor_Address = ds.Tables[0].Rows[0]["Vendor_Address"].ToString();
                        obj_Bill_Info.Contractor_EmailID = ds.Tables[0].Rows[0]["Vendor_EmailId"].ToString();
                        obj_Bill_Info.Contractor_GSTIN = ds.Tables[0].Rows[0]["Vendor_GSTIN"].ToString();
                        obj_Bill_Info.Contractor_Mobile = ds.Tables[0].Rows[0]["Vendor_Mobile"].ToString();
                        obj_Bill_Info.Contractor_Name = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
                        obj_Bill_Info.Division_Name = ds.Tables[0].Rows[0]["Division_Name"].ToString();
                        obj_Bill_Info.Invoice_Date = ds.Tables[0].Rows[0]["PackageInvoice_Date"].ToString();
                        obj_Bill_Info.Invoice_No = ds.Tables[0].Rows[0]["PackageInvoice_VoucherNo"].ToString();
                        obj_Bill_Info.Narration = ds.Tables[0].Rows[0]["PackageInvoice_Narration"].ToString();
                        obj_Bill_Info.SBR_No = ds.Tables[0].Rows[0]["PackageInvoice_SBR_No"].ToString();
                        obj_Bill_Info.DBR_No = ds.Tables[0].Rows[0]["PackageInvoice_DBR_No"].ToString();
                        obj_Bill_Info.Start_Date = ds.Tables[0].Rows[0]["start_Date"].ToString();
                        obj_Bill_Info.End_Date = ds.Tables[0].Rows[0]["End_Date"].ToString();
                        obj_Bill_Info.Agreement_No = ds.Tables[0].Rows[0]["ProjectWorkPkg_Agreement_No"].ToString();
                        obj_Bill_Info.Scheme_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();
                        obj_Bill_Info.Additional_Data_2 = "Package: " + ds.Tables[0].Rows[0]["ProjectWorkPkg_Name"].ToString();
                        obj_Bill_Info.Additional_Data_1 = "Project Work: " + ds.Tables[0].Rows[0]["ProjectWork_Name"].ToString();
                        obj_Bill_Info.Additional_Data_3 = ds.Tables[0].Rows[0]["ProjectWork_Description"].ToString();

                        obj_Bill_Info.EMB_Amount = decimal.Parse(ds.Tables[1].Rows[i]["Total_Amount"].ToString());
                        obj_Bill_Info.EMB_Qty = decimal.Parse(ds.Tables[1].Rows[i]["PackageInvoiceItem_Total_Qty_BOQ"].ToString());
                        obj_Bill_Info.EMB_Rate = decimal.Parse(ds.Tables[1].Rows[i]["Total_Rate"].ToString());
                        obj_Bill_Info.EMB_Specification = ds.Tables[1].Rows[i]["PackageEMB_Specification"].ToString().Replace("<br />", "");
                        obj_Bill_Info.EMB_Unit_Name = ds.Tables[1].Rows[i]["Unit_Name"].ToString();
                        obj_Bill_Info.EMB_Date = ds.Tables[1].Rows[i]["EMB_Master_Date"].ToString() + ", " + ds.Tables[1].Rows[i]["PackageEMB_Master_VoucherNo"].ToString();
                        obj_Bill_Info.Additional_Data_4 = ds.Tables[1].Rows[i]["Total_Tax"].ToString();

                        invoice_total += obj_Bill_Info.EMB_Amount;

                        obj_Bill_Info_Li.Add(obj_Bill_Info);
                    }
                }
            }

            if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    PackageInvoiceAdditional obj_PackageInvoiceAdditional = new PackageInvoiceAdditional();
                    obj_PackageInvoiceAdditional.Deduction_Name = ds.Tables[3].Rows[i]["Deduction_Name"].ToString();
                    try
                    {
                        obj_PackageInvoiceAdditional.Deduction_Value = decimal.Parse(ds.Tables[3].Rows[i]["PackageInvoiceAdditional_Deduction_Value_Final"].ToString());
                    }
                    catch
                    {
                        obj_PackageInvoiceAdditional.Deduction_Value = 0;
                    }
                    obj_PackageInvoiceAdditional.Value_Type = ds.Tables[3].Rows[i]["Deduction_Category"].ToString();
                    if (ds.Tables[3].Rows[i]["Deduction_Mode"].ToString().Trim() == "-")
                        deduction_total += obj_PackageInvoiceAdditional.Deduction_Value;
                    else
                        addition_total += obj_PackageInvoiceAdditional.Deduction_Value;
                    obj_PackageInvoiceAdditional_Li.Add(obj_PackageInvoiceAdditional);
                }
            }
            grand_total = invoice_total - deduction_total + addition_total;
            string amount_in_Words = new ConvertMoneyToWord.ConvertMoneyToWord().CMoney(grand_total.ToString());
            for (int i = 0; i < obj_Bill_Info_Li.Count; i++)
            {
                obj_Bill_Info_Li[i].Invoie_Amount_Final = decimal.Round(grand_total, 0, MidpointRounding.AwayFromZero);
                obj_Bill_Info_Li[i].EMB_Amount_In_Words = amount_in_Words;
            }
            if (obj_Bill_Info_Li != null && obj_Bill_Info_Li.Count > 0)
            {
                Invoice_View obj_Invoice_View = new Invoice_View();
                obj_Invoice_View.obj_Bill_Info_Li = obj_Bill_Info_Li;
                obj_Invoice_View.obj_PackageInvoiceAdditional_Li = obj_PackageInvoiceAdditional_Li;
                Session["Invoice_View"] = obj_Invoice_View;
                mpViewBill.Show();
            }
            else
            {
                Session["Invoice_View"] = null;
            }
        }
        else
        {
            Session["Invoice_View"] = null;
            MessageBox.Show("Server Error!!");
            return;
        }
    }
    protected void grdInvoice_PreRender(object sender, EventArgs e)
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
    protected void grdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDelete = e.Row.FindControl("btnDeleteInvoice") as ImageButton;
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

    protected void grdEMBHistory_PreRender(object sender, EventArgs e)
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

    protected void grdInvoiceHistory_PreRender(object sender, EventArgs e)
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

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divHistory.Visible = true;
        int Invoice_Id = 0;
        int Package_Id = 0;
        int PackageEMBMaster_Id = 0;
        int Work_Id = 0;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        for (int i = 0; i < grdInvoice.Rows.Count; i++)
        {
            grdInvoice.Rows[i].BackColor = Color.Transparent;
        }
        gr.BackColor = Color.LightGreen;
        try
        {
            Invoice_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        }
        catch
        {
            Invoice_Id = 0;
        }

        try
        {
            Package_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            Package_Id = 0;
        }

        try
        {
            PackageEMBMaster_Id = Convert.ToInt32(gr.Cells[2].Text.Trim());
        }
        catch
        {
            PackageEMBMaster_Id = 0;
        }

        try
        {
            Work_Id = Convert.ToInt32(gr.Cells[3].Text.Trim());
        }
        catch
        {
            Work_Id = 0;
        }
        hf_Invoice_Id.Value = Invoice_Id.ToString();
        getPackage_Details(Package_Id);
        get_tbl_PackageInvoiceApproval_History(Invoice_Id);
        get_tbl_PackageEMBApproval_History(Invoice_Id);
        get_tbl_ProjectFundingPattern(Work_Id);
        get_tbl_Invoice_Documents_Details(Invoice_Id);
    }
    private void getPackage_Details(int Package_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, 0, 0, 0, 0, 0, Package_Id, "", "", false,"");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPackageDetails.DataSource = ds.Tables[0];
            grdPackageDetails.DataBind();
        }
        else
        {
            grdPackageDetails.DataSource = null;
            grdPackageDetails.DataBind();
        }
    }

    private void get_tbl_Invoice_Documents_Details(int Invoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Invoice_Documents_Details(Invoice_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdMultipleFiles.DataSource = ds.Tables[0];
            grdMultipleFiles.DataBind();
        }
        else
        {
            grdMultipleFiles.DataSource = null;
            grdMultipleFiles.DataBind();
        }
    }

    private void get_tbl_PackageInvoiceApproval_History(int PackageInvoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageInvoiceApproval_History(PackageInvoice_Id);

        if (AllClasses.CheckDataSet(ds))
        {
            grdInvoiceHistory.DataSource = ds.Tables[0];
            grdInvoiceHistory.DataBind();

            for (int i = 0; i < grdInvoiceHistory.Rows.Count; i++)
            {
                ImageButton btnDelete = grdInvoiceHistory.Rows[i].FindControl("btnRollBack") as ImageButton;
                if (Session["UserType"].ToString() == "1")
                {
                    if (i == (grdInvoiceHistory.Rows.Count - 1))
                    {
                        btnDelete.Visible = true;
                    }
                    else
                    {
                        btnDelete.Visible = false;
                    }
                }
                else
                {
                    btnDelete.Visible = false;
                }
            }
        }
        else
        {
            grdInvoiceHistory.DataSource = null;
            grdInvoiceHistory.DataBind();
        }
    }

    private void get_tbl_PackageEMBApproval_History(int PackageInvoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageEMBApproval_History(PackageInvoice_Id);

        if (AllClasses.CheckDataSet(ds))
        {
            grdEMBHistory.DataSource = ds.Tables[0];
            grdEMBHistory.DataBind();
        }
        else
        {
            grdEMBHistory.DataSource = null;
            grdEMBHistory.DataBind();
        }
    }
    private void get_tbl_ProjectFundingPattern(int ProjectWorkId)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectFundingPattern(ProjectWorkId);

        if (AllClasses.CheckDataSet(ds))
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
    protected void grdPackageDetails_PreRender(object sender, EventArgs e)
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

    protected void lnkDeductionHistory_Click(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as LinkButton).Parent.Parent as GridViewRow;
        int Invoice_Id = 0;
        int Added_By = 0;
        try
        {
            Invoice_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            Invoice_Id = 0;
        }

        try
        {
            Added_By = Convert.ToInt32(gr.Cells[3].Text.Trim());
        }
        catch
        {
            Added_By = 0;
        }
        if (Invoice_Id > 0 && Added_By > 0)
        {
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction(Invoice_Id, Added_By);
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

    protected void grdDeductionHistory_PreRender(object sender, EventArgs e)
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

    protected void grdFundingPattern_PreRender(object sender, EventArgs e)
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

    protected void lnk_ContractBond_ServerClick(object sender, EventArgs e)
    {
        MessageBox.Show("File Not Available To Download");
        return;
    }

    protected void lnk_PerformanceSecurity_ServerClick(object sender, EventArgs e)
    {
        MessageBox.Show("File Not Available To Download");
        return;
    }

    protected void lnk_BankGurantee_ServerClick(object sender, EventArgs e)
    {
        MessageBox.Show("File Not Available To Download");
        return;
    }

    protected void lnk_MoblizationAdvance_ServerClick(object sender, EventArgs e)
    {
        MessageBox.Show("File Not Available To Download");
        return;
    }

    protected void lnk_CoverLetter_ServerClick(object sender, EventArgs e)
    {
        List<Cover_Letter> obj_Cover_Letter_Li = new List<Cover_Letter>();
        Session["Cover_Letter"] = obj_Cover_Letter_Li;
        mpViewCoverLetter.Show();
    }

    protected void lnk_PaymentSummery_ServerClick(object sender, EventArgs e)
    {
        List<ProjectSummery> obj_ProjectSummery_Li = new List<ProjectSummery>();

        ProjectSummery obj_ProjectSummery = new ProjectSummery();
        DataSet ds = new DataSet();
        int Invoice_Id = Convert.ToInt32(hf_Invoice_Id.Value);
        ds = (new DataLayer()).get_Report_Summery_Details(Invoice_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            obj_ProjectSummery.Contractor_Name = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
            obj_ProjectSummery.Extra_Item_Condition = "No";
            obj_ProjectSummery.Installment_Condition = "";
            try
            {
                obj_ProjectSummery.Project_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectWork_Budget"].ToString());
            }
            catch
            { }
            obj_ProjectSummery.Project_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();
            try
            {
                obj_ProjectSummery.Sactioned_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectWorkPkg_AgreementAmount"].ToString());
            }
            catch
            { }
            //obj_ProjectSummery.Scheme_Name = ds.Tables[0].Rows[0][""].ToString();
            try
            {
                obj_ProjectSummery.Tender_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Tender_Cost"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Tender_Cost_Less = Convert.ToDecimal(ds.Tables[0].Rows[0]["Tender_Cost_Less"].ToString());
            }
            catch
            { }
            obj_ProjectSummery.Tender_Cost_Less_With_Text = "(80% of Tender Cost " + ds.Tables[0].Rows[0]["Tender_Cost_Less"].ToString() + ")";

            try
            {
                obj_ProjectSummery.Total_Bill_Raised = Convert.ToDecimal(ds.Tables[0].Rows[0]["Total_Amount"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_Centre_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["Central_Share"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_State_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["State_Share"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_ULB_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["ULB_Share"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_Calculated = obj_ProjectSummery.Total_Centre_Share + obj_ProjectSummery.Total_State_Share + obj_ProjectSummery.Total_ULB_Share;
            }
            catch
            { }

            try
            {
                obj_ProjectSummery.Total_Payment_Earlier = 0;
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_Proposed_Payment_Jal_Nigam = 0;
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_Release = 0;
            }
            catch
            { }

            try
            {
                obj_ProjectSummery.Total_With_Held_Amount = 0;
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Work_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Work_Amount"].ToString());
            }
            catch
            { }
            try
            {
                obj_ProjectSummery.Total_Balance = 0;
            }
            catch
            { }
            obj_ProjectSummery.Work_Name = ds.Tables[0].Rows[0]["ProjectWork_Name"].ToString();
            obj_ProjectSummery_Li.Add(obj_ProjectSummery);

            Session["ProjectSummery"] = obj_ProjectSummery_Li;
            mpViewSummery.Show();
        }
        else
        {
            MessageBox.Show("Unable To View Report");
            return;
        }
    }

    protected void grdMultipleFiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDownload = (e.Row.FindControl("lnkDownload") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload);
        }
    }

    protected void btnDeleteInvoice_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
            int Invoice_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
            int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
            if (new DataLayer().Delete_Invoicing(Invoice_Id, Person_Id))
            {
                MessageBox.Show("Deleted Successfully!!");
                int Package_Id = 0;
                int Division_Id = 0;
                int Scheme_Id = 0;
                if (Request.QueryString.Count > 0)
                {
                    try
                    {
                        Package_Id = Convert.ToInt32(Request.QueryString["Package_Id"].ToString());
                    }
                    catch
                    {
                        Package_Id = 0;
                    }

                    try
                    {
                        Invoice_Id = Convert.ToInt32(Request.QueryString["Invoice_Id"].ToString());
                    }
                    catch
                    {
                        Invoice_Id = 0;
                    }

                    try
                    {
                        Division_Id = Convert.ToInt32(Request.QueryString["Division_Id"].ToString());
                    }
                    catch
                    {
                        Division_Id = 0;
                    }

                    try
                    {
                        Scheme_Id = Convert.ToInt32(Request.QueryString["Scheme_Id"].ToString());
                    }
                    catch
                    {
                        Scheme_Id = 0;
                    }
                }
                else
                {
                    Invoice_Id = 0;
                    Package_Id = 0;
                    Division_Id = 0;
                    Scheme_Id = 0;
                }
                
                    get_tbl_PackageInvoice(Package_Id, Division_Id, Scheme_Id);
                
                return;
            }
            else
            {
                MessageBox.Show("Invoice Also Approved Amirut So Invoice Is Not Deleted.!!");
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

   
    protected void btnRollBack_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;

        int PackageInvoiceApproval_Id = 0;
        try
        {
            PackageInvoiceApproval_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        }
        catch
        {
            PackageInvoiceApproval_Id = 0;
        }
        int Invoice_Id = 0;
        try
        {
            Invoice_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            Invoice_Id = 0;
        }
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().RollBack_Invoicing(PackageInvoiceApproval_Id, Invoice_Id, Person_Id))
        {
            MessageBox.Show("Invoice Roll Back Successfully!!");
            return;
        }
        else
        {
            MessageBox.Show("Invoice Also Approved Amirut So Invoice Is Not Roll Back.!!");
            return;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
}
