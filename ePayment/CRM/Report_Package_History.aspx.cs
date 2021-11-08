using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_Package_History : System.Web.UI.Page
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, txtProjectCode.Text.Trim(), txtPackageCode.Text.Trim(), false,"");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            divData.Visible = true;
        }
        else
        {
            divData.Visible = false;
            grdPost.DataSource = null;
            grdPost.DataBind();
            MessageBox.Show("No Records Found");
        }
    }
    
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divInvoice.Visible = true;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        gr.BackColor = Color.LightGreen;
        get_tbl_PackageInvoice(Convert.ToInt32(gr.Cells[0].Text.Trim()));
    }
    private void get_tbl_PackageInvoice(int Package_Id)
    {
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, 0, 0, 0, 0, true);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, 0, 0, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), true);
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

    protected void btnEditInv_Click(object sender, ImageClickEventArgs e)
    {
        divHistory.Visible = true;
        int Invoice_Id = 0;
        int Package_Id = 0;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
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
        get_tbl_PackageInvoiceApproval_History(Invoice_Id);
        get_tbl_PackageEMBApproval_History(Invoice_Id, Package_Id);
    }

    private void get_tbl_PackageInvoiceApproval_History(int PackageInvoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageInvoiceApproval_History(PackageInvoice_Id);

        if (AllClasses.CheckDataSet(ds))
        {
            grdInvoiceHistory.DataSource = ds.Tables[0];
            grdInvoiceHistory.DataBind();
        }
        else
        {
            grdInvoiceHistory.DataSource = null;
            grdInvoiceHistory.DataBind();
        }
    }

    private void get_tbl_PackageEMBApproval_History(int PackageInvoice_Id, int Package_Id)
    {
        //DataSet ds = new DataSet();
        //ds = (new DataLayer()).get_tbl_PackageInvoiceApproval_History(PackageInvoice_Id);

        //if (AllClasses.CheckDataSet(ds))
        //{
        //    grdEMBHistory.DataSource = ds.Tables[0];
        //    grdEMBHistory.DataBind();
        //}
        //else
        //{
        //    grdEMBHistory.DataSource = null;
        //    grdEMBHistory.DataBind();
        //}
        grdEMBHistory.DataSource = null;
        grdEMBHistory.DataBind();
    }
}
