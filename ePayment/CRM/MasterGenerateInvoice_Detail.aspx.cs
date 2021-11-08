using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterGenerateInvoice_Detail : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.MasterPageFile = SetMasterPage.ReturnPage();
    }
    private void get_tbl_InvoiceStatus(int ConfigMasterId)
    {
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(0, 0, 0, "Invoice");
        }
        else
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), ConfigMasterId, "Invoice");
        }
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlStatus, "InvoiceStatus_Name", "InvoiceStatus_Id");
        }
        else
        {
            ddlStatus.Items.Clear();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            divAmountTransfered.Visible = false;
            int Package_Id = 0;
            int Invoice_Id = 0;
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
            }
            else
            {
                Invoice_Id = 0;
                Package_Id = 0;
            }
            if (Invoice_Id > 0)
            {
                divEntry.Visible = true;
                hf_Invoice_Id.Value = Invoice_Id.ToString();
                get_tbl_Invoice_Details(Invoice_Id, true);
                get_ProcessConfig_Current(Invoice_Id);
            }
            else if (Package_Id > 0)
            {
                hf_Invoice_Id.Value = "0";
                get_tbl_PackageInvoice(Package_Id);
            }
        }
    }
    private void get_ProcessConfig_Current(int PackageInvoice_Id)
    {
        if (Session["UserType"].ToString() == "1")
        {
            btnGenerateInvoice.Visible = true;
            btnApproveInvoice.Visible = true;
        }
        else
        {
            DataSet ds = new DataSet();
            int _Loop = 0;
            if (Session["UserType"].ToString() == "1")
            {
                _Loop = (new DataLayer()).get_Loop("Invoice", 0, 0, null, null);
            }
            else
            {
                _Loop = (new DataLayer()).get_Loop("Invoice", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), null, null);
            }
            ds = (new DataLayer()).get_ProcessConfig_Current("Invoice", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), _Loop, PackageInvoice_Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int ConfigMaster_Id = 0;
                try
                {
                    ConfigMaster_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Id"].ToString());
                    get_tbl_TradeDocument(ConfigMaster_Id);

                    get_tbl_InvoiceStatus(ConfigMaster_Id);
                }
                catch
                {
                    grdDocumentMaster.DataSource = null;
                    grdDocumentMaster.DataBind();
                    ConfigMaster_Id = 0;
                    get_tbl_InvoiceStatus(ConfigMaster_Id);
                }
                if (ds.Tables[0].Rows[0]["ProcessConfigMaster_Deduction_Allowed"].ToString() == "1")
                {
                    for (int i = 0; i < grdDeductions.Rows.Count; i++)
                    {
                        grdDeductions.Rows[i].Enabled = true;
                    }
                    for (int i = 0; i < grdDeductions2.Rows.Count; i++)
                    {
                        grdDeductions2.Rows[i].Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdDeductions.Rows.Count; i++)
                    {
                        grdDeductions.Rows[i].Enabled = false;
                    }
                    for (int i = 0; i < grdDeductions2.Rows.Count; i++)
                    {
                        grdDeductions2.Rows[i].Enabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ProcessConfigMaster_Transfer_Allowed"].ToString() == "1")
                {
                    divAmountTransfered.Visible = true;
                }
                else
                {
                    divAmountTransfered.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["ProcessConfigMaster_Creation_Allowed"].ToString() == "1")
                {
                    btnGenerateInvoice.Visible = true;
                }
                else if (ds.Tables[0].Rows[0]["ProcessConfigMaster_Updation_Allowed"].ToString() == "1")
                {
                    btnApproveInvoice.Visible = true;
                }
                else
                {
                    btnGenerateInvoice.Visible = false;
                    btnApproveInvoice.Visible = false;
                    if (PackageInvoice_Id > 0)
                    {
                        btnApproveInvoice.Visible = true;
                    }
                    else
                    {
                        btnGenerateInvoice.Visible = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < grdDeductions.Rows.Count; i++)
                {
                    grdDeductions.Rows[i].Enabled = false;
                }
                for (int i = 0; i < grdDeductions2.Rows.Count; i++)
                {
                    grdDeductions2.Rows[i].Enabled = false;
                }
                btnGenerateInvoice.Visible = false;
                btnApproveInvoice.Visible = false;
            }
        }
    }

    private void get_tbl_TradeDocument(int ConfigMaster_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_TradeDocument(ConfigMaster_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdDocumentMaster.DataSource = ds.Tables[0];
            grdDocumentMaster.DataBind();
        }
        else
        {
            grdDocumentMaster.DataSource = null;
            grdDocumentMaster.DataBind();
        }
    }

    private void get_tbl_PackageInvoice(int Package_Id)
    {
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, 0, 0, 0, 0, false);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(Package_Id, 0, 0, 0, 0, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), false);
        }

        if (AllClasses.CheckDataSet(ds))
        {
            grdInvoice.DataSource = ds.Tables[0];
            grdInvoice.DataBind();

            if (grdInvoice.Rows.Count == 1)
            {
                ImageButton btnOpenInvoice = grdInvoice.Rows[0].FindControl("btnOpenInvoice") as ImageButton;
                btnOpenInvoice_Click(btnOpenInvoice, new ImageClickEventArgs(0, 0));
            }
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
        int Package_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        int Work_Id = Convert.ToInt32(gr.Cells[2].Text.Trim());
        divEntry.Visible = true;
        hf_Invoice_Id.Value = Invoice_Id.ToString();
        for (int i = 0; i < grdInvoice.Rows.Count; i++)
        {
            grdInvoice.Rows[i].BackColor = Color.Transparent;
        }
        gr.BackColor = Color.LightGreen;
        get_tbl_Invoice_Details(Invoice_Id, false);
        getPackage_Details(Package_Id);
        get_tbl_ProjectFundingPattern(Work_Id);
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
    private void get_tbl_Invoice_Details(int Invoice_Id, bool bind_grdInvoice)
    {
        DataSet ds = new DataSet();
        int PackageEMB_Master_Id = 0;
        ds = (new DataLayer()).get_tbl_Invoice_Details(Invoice_Id);
        if (ds != null)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["PackageInvoice_VoucherNo"].ToString();
                if (txtInvoiceNo.Text.Trim() == "")
                    txtInvoiceNo.Text = new DataLayer().get_tbl_TransactionNos(VoucherTypes.Invoice, Convert.ToInt32(ds.Tables[0].Rows[0]["PackageInvoice_Package_Id"].ToString()), null, null);
                txtInvoiceDate.Text = ds.Tables[0].Rows[0]["PackageInvoice_Date"].ToString();
                txtSBRNo.Text = ds.Tables[0].Rows[0]["PackageInvoice_SBR_No"].ToString();
                txtDBRNo.Text = ds.Tables[0].Rows[0]["PackageInvoice_DBR_No"].ToString();
                txtNarration.Text = ds.Tables[0].Rows[0]["PackageInvoice_Narration"].ToString();
                PackageEMB_Master_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["PackageInvoice_PackageEMBMaster_Id"].ToString());
                if (ds.Tables[0].Rows[0]["PackageInvoice_ProcessedBy"].ToString().Trim().Replace("&nbsp;", "") == "")
                {
                    btnGenerateInvoice.Visible = true;
                    btnApproveInvoice.Visible = false;
                }
                else
                {
                    btnGenerateInvoice.Visible = false;
                    btnApproveInvoice.Visible = true;
                }
            }
            else
            {
                txtInvoiceNo.Text = new DataLayer().get_tbl_TransactionNos(VoucherTypes.Invoice, 0, null, null);
                txtInvoiceDate.Text = "";
                txtSBRNo.Text = "";
                txtDBRNo.Text = "";
                txtNarration.Text = "";
            }
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                grdBOQ.DataSource = ds.Tables[1];
                grdBOQ.DataBind();

                grdBOQ.FooterRow.Cells[7].Text = ds.Tables[1].Compute("sum(PackageInvoiceItem_Total_Qty_BOQ)", "").ToString();
                grdBOQ.FooterRow.Cells[11].Text = ds.Tables[1].Compute("sum(Amount)", "").ToString();
                grdBOQ.FooterRow.Cells[16].Text = ds.Tables[1].Compute("sum(Total_Amount)", "").ToString();
            }
            else
            {
                grdBOQ.DataSource = null;
                grdBOQ.DataBind();
            }
            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                grdMultipleFiles.DataSource = ds.Tables[4];
                grdMultipleFiles.DataBind();
            }
            else
            {
                grdMultipleFiles.DataSource = null;
                grdMultipleFiles.DataBind();
            }
            if (bind_grdInvoice)
            {
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    grdInvoice.DataSource = ds.Tables[2];
                    grdInvoice.DataBind();

                    if (grdInvoice.Rows.Count == 1)
                    {
                        ImageButton btnOpenInvoice = grdInvoice.Rows[0].FindControl("btnOpenInvoice") as ImageButton;
                        btnOpenInvoice_Click(btnOpenInvoice, new ImageClickEventArgs(0, 0));
                    }
                }
                else
                {
                    grdInvoice.DataSource = null;
                    grdInvoice.DataBind();
                }
            }
        }
        else
        {
            MessageBox.Show("Server Error!!");
            return;
        }
        get_tbl_Deduction(Invoice_Id, PackageEMB_Master_Id);
        Calculate_Total();
        get_ProcessConfig_Current(Invoice_Id);
    }

    private void Calculate_Total()
    {
        decimal deduction_Value = 0;
        decimal addition_Value = 0;
        decimal invoice_total = 0;
        if (grdDeductions.FooterRow != null)
        {
            try
            {
                deduction_Value = decimal.Parse(grdDeductions.FooterRow.Cells[10].Text.Trim());
            }
            catch
            {
                deduction_Value = 0;
            }
        }
        if (grdDeductions2.FooterRow != null)
        {
            try
            {
                addition_Value = decimal.Parse(grdDeductions2.FooterRow.Cells[10].Text.Trim());
            }
            catch
            {
                addition_Value = 0;
            }
        }
        if (grdBOQ.FooterRow != null)
        {
            try
            {
                invoice_total = decimal.Parse(grdBOQ.FooterRow.Cells[16].Text.Trim());
            }
            catch
            {
                invoice_total = 0;
            }
        }
        lblTotalAmount.Text = decimal.Round(invoice_total - deduction_Value + addition_Value, 2, MidpointRounding.AwayFromZero).ToString();
        txtFudTransfered.Text = lblTotalAmount.Text;
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
            int PackageInvoiceItem_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
            GridView grdTaxes = e.Row.FindControl("grdTaxes") as GridView;
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction_WithPackageInvoiceItem_Tax(PackageInvoiceItem_Id);
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

    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Invoice No");
            return;
        }
        if (ddlStatus.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Status");
            return;
        }
        if (ddlStatus.SelectedValue == "2" || ddlStatus.SelectedValue == "3")
        {
            MessageBox.Show("Please Provide Comments / Reason");
            return;
        }
        tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
        obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_ProcessedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_Date = txtInvoiceDate.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_DBR_No = txtDBRNo.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
        obj_tbl_PackageInvoice.PackageInvoice_Narration = txtNarration.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
        obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = txtInvoiceNo.Text.Trim();

        List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
        for (int i = 0; i < grdBOQ.Rows.Count; i++)
        {
            TextBox txtRemarks = grdBOQ.Rows[i].FindControl("txtRemarks") as TextBox;
            tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[0].Text.Trim());
            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Remarks = txtRemarks.Text.Trim();
            GridView grdTaxes = grdBOQ.Rows[i].FindControl("grdTaxes") as GridView;
            List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
            for (int k = 0; k < grdTaxes.Rows.Count; k++)
            {
                tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_PackageInvoiceItem_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[0].Text.Trim());
                try
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = 0;
                }
                try
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = 0;
                }
                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status = 1;
                if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                {
                    if (obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        for (int j = 0; j < obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li.Count; j++)
                        {
                            if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_PackageInvoiceItem_Id == obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li[j].PackageInvoiceItem_Tax_PackageInvoiceItem_Id && obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id == obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li[j].PackageInvoiceItem_Tax_Deduction_Id)
                            {
                                MessageBox.Show("Deduction Already Addded. ");
                                return;
                            }
                        }
                    }
                }

                if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                {
                    obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                }
                if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                {
                    obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                }

            }
            obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);

        }

        List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li = new List<tbl_PackageInvoiceAdditional>();
        for (int i = 0; i < grdDeductions.Rows.Count; i++)
        {
            TextBox txtDeductionValue = grdDeductions.Rows[i].FindControl("txtDeductionValue") as TextBox;
            TextBox txtDeductionAmount = grdDeductions.Rows[i].FindControl("txtDeductionAmount") as TextBox;
            TextBox txtDeductionComments = grdDeductions.Rows[i].FindControl("txtDeductionComments") as TextBox;
            CheckBox chkSelect = grdDeductions.Rows[i].FindControl("chkSelect") as CheckBox;
            CheckBox chkFlat = grdDeductions.Rows[i].FindControl("chkFlat") as CheckBox;
            decimal DeductionValue = 0;
            if (chkSelect.Checked && !chkFlat.Checked)
            {
                try
                {
                    DeductionValue = Convert.ToDecimal(txtDeductionValue.Text.Trim());
                }
                catch
                {
                    DeductionValue = 0;
                }
                try
                {
                    txtDeductionAmount.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * DeductionValue) / 100), 2, MidpointRounding.AwayFromZero).ToString();
                }
                catch
                {
                    txtDeductionAmount.Text = "0";
                }

                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments.Text.Trim();
                if (chkFlat.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = DeductionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
            else if (chkFlat.Checked && chkSelect.Checked)
            {
                try
                {
                    DeductionValue = Convert.ToDecimal(txtDeductionValue.Text.Trim());
                }
                catch
                {
                    DeductionValue = 0;
                }
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments.Text.Trim();
                if (chkFlat.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = DeductionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }

        for (int i = 0; i < grdDeductions2.Rows.Count; i++)
        {
            TextBox txtDeductionValue2 = grdDeductions2.Rows[i].FindControl("txtDeductionValue2") as TextBox;
            TextBox txtDeductionAmount2 = grdDeductions2.Rows[i].FindControl("txtDeductionAmount2") as TextBox;
            TextBox txtDeductionComments2 = grdDeductions2.Rows[i].FindControl("txtDeductionComments2") as TextBox;
            CheckBox chkSelect2 = grdDeductions2.Rows[i].FindControl("chkSelect2") as CheckBox;
            CheckBox chkFlat2 = grdDeductions2.Rows[i].FindControl("chkFlat2") as CheckBox;
            decimal AdditionValue = 0;
            if (chkSelect2.Checked && !chkFlat2.Checked)
            {
                try
                {
                    AdditionValue = Convert.ToDecimal(txtDeductionValue2.Text.Trim());
                }
                catch
                {
                    AdditionValue = 0;
                }
                try
                {
                    txtDeductionAmount2.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * AdditionValue) / 100), 2, MidpointRounding.AwayFromZero).ToString();
                }
                catch
                {
                    txtDeductionAmount2.Text = "0";
                }

                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions2.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments2.Text.Trim();
                if (chkFlat2.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = AdditionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
            else if (chkFlat2.Checked && chkSelect2.Checked)
            {
                try
                {
                    AdditionValue = Convert.ToDecimal(txtDeductionValue2.Text.Trim());
                }
                catch
                {
                    AdditionValue = 0;
                }
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions2.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments2.Text.Trim();
                if (chkFlat2.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = AdditionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }
        List<tbl_PackageInvoiceDocs> obj_tbl_PackageInvoiceDocs_Li = new List<tbl_PackageInvoiceDocs>();
        for (int i = 0; i < grdDocumentMaster.Rows.Count; i++)
        {
            FileUpload flUpload = grdDocumentMaster.Rows[i].FindControl("flUpload") as FileUpload;
            TextBox txtDocumentOrderNo = grdDocumentMaster.Rows[i].FindControl("txtDocumentOrderNo") as TextBox;
            TextBox txtDocumentComments = grdDocumentMaster.Rows[i].FindControl("txtDocumentComments") as TextBox;
            if (flUpload.HasFile)
            {
                tbl_PackageInvoiceDocs obj_tbl_PackageInvoiceDocs = new tbl_PackageInvoiceDocs();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileBytes = flUpload.FileBytes;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName = flUpload.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Status = 1;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_OrderNo = txtDocumentOrderNo.Text.Trim();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Comments = txtDocumentComments.Text.Trim();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Type = Convert.ToInt32(grdDocumentMaster.Rows[i].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceDocs_Li.Add(obj_tbl_PackageInvoiceDocs);
            }
            else
            {
                MessageBox.Show("Please Upload Document.");
                return;
            }
        }

        tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = txtComments.Text.Trim();
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = Convert.ToInt32(ddlStatus.SelectedValue);
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
        if (new DataLayer().Update_tbl_PackageInvoice(obj_tbl_PackageInvoice, obj_tbl_PackageInvoiceAdditional_Li, obj_tbl_PackageInvoiceDocs_Li, obj_tbl_PackageInvoiceItem_Li, null, "P", obj_tbl_PackageInvoiceApproval, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString())))
        {
            MessageBox.Show("Invoice Details Updated Successfully");
            Response.Redirect("MasterGenerateInvoice_View?Package_Id=0&Invoice_Id=" + obj_tbl_PackageInvoice.PackageInvoice_Id.ToString());
            return;
        }
        else
        {
            MessageBox.Show("Error In Updation of Invoice Details...");
            return;
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
    protected void grdDeductions_PreRender(object sender, EventArgs e)
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
    private DataTable Filter_Deduction(DataSet ds, string Deduction_Mode)
    {
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "Deduction_Mode='" + Deduction_Mode + "'";
        return dv.ToTable();
    }
    private void get_tbl_Deduction(int Invoice_Id, int PackageEMB_Master_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction(Invoice_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = Filter_Deduction(ds, "-");
            if (AllClasses.CheckDt(dt))
            {
                grdDeductions.DataSource = dt;
                grdDeductions.DataBind();
                ViewState["grdDeductions"] = dt;

                decimal DeductionValue_Total = 0;
                TextBox txtDeductionAmount;
                for (int i = 0; i < grdDeductions.Rows.Count; i++)
                {
                    txtDeductionAmount = grdDeductions.Rows[i].FindControl("txtDeductionAmount") as TextBox;
                    try
                    {
                        DeductionValue_Total += Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                    }
                    catch
                    {
                        DeductionValue_Total += 0;
                    }
                }
                grdDeductions.FooterRow.Cells[10].Text = DeductionValue_Total.ToString();
            }
            else
            {
                grdDeductions.DataSource = null;
                grdDeductions.DataBind();
            }
            dt = new DataTable();
            dt = Filter_Deduction(ds, "+");
            if (AllClasses.CheckDt(dt))
            {
                grdDeductions2.DataSource = dt;
                grdDeductions2.DataBind();

                decimal DeductionValue_Total2 = 0;
                TextBox txtDeductionAmount2;
                for (int i = 0; i < grdDeductions2.Rows.Count; i++)
                {
                    txtDeductionAmount2 = grdDeductions2.Rows[i].FindControl("txtDeductionAmount2") as TextBox;
                    try
                    {
                        DeductionValue_Total2 += Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                    }
                    catch
                    {
                        DeductionValue_Total2 += 0;
                    }
                }
                grdDeductions2.FooterRow.Cells[10].Text = DeductionValue_Total2.ToString();
            }
            else
            {
                grdDeductions2.DataSource = null;
                grdDeductions2.DataBind();
            }
        }
        else
        {
            grdDeductions.DataSource = null;
            grdDeductions.DataBind();

            grdDeductions2.DataSource = null;
            grdDeductions2.DataBind();
        }

      
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelect = sender as CheckBox;
        GridViewRow gr = chkSelect.Parent.Parent as GridViewRow;
        TextBox txtDeductionAmount = gr.FindControl("txtDeductionAmount") as TextBox;
        TextBox txtDeductionValue = gr.FindControl("txtDeductionValue") as TextBox;
        CheckBox chkFlat = gr.FindControl("chkFlat") as CheckBox;
        if (chkSelect.Checked && !chkFlat.Checked)
        {
            decimal DeductionValue = 0;
            try
            {
                DeductionValue = Convert.ToDecimal(txtDeductionValue.Text.Trim());
            }
            catch
            {
                DeductionValue = 0;
            }
            try
            {
                txtDeductionAmount.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * DeductionValue) / 100), 2, MidpointRounding.AwayFromZero).ToString();
            }
            catch
            {
                txtDeductionAmount.Text = "0";
            }
        }
        else if (chkFlat.Checked && chkSelect.Checked)
        {
            decimal invoice_Total = 0;
            decimal DeductionAmount = 0;

            try
            {
                invoice_Total = Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim());
            }
            catch
            {
                invoice_Total = 0;
            }

            try
            {
                DeductionAmount = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
            }
            catch
            {
                DeductionAmount = 0;
            }
            if (DeductionAmount > 0)
            {
                txtDeductionValue.Text = decimal.Round((DeductionAmount * 100) / invoice_Total, 2, MidpointRounding.AwayFromZero).ToString();
            }
        }
        else
        {
            txtDeductionAmount.Text = "0";
        }
        if (e.GetType().Name == "GridViewRowEventArgs")
        { }
        else
        {
            decimal DeductionValue_Total = 0;
            for (int i = 0; i < grdDeductions.Rows.Count; i++)
            {
                txtDeductionAmount = grdDeductions.Rows[i].FindControl("txtDeductionAmount") as TextBox;
                try
                {
                    DeductionValue_Total += Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                }
                catch
                {
                    DeductionValue_Total += 0;
                }
            }
            grdDeductions.FooterRow.Cells[10].Text = DeductionValue_Total.ToString();
        }
        Calculate_Total();
    }

    protected void txtDeductionValue_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as TextBox).Parent.Parent as GridViewRow;
        CheckBox chkSelect = gr.FindControl("chkSelect") as CheckBox;
        if (chkSelect.Checked)
            chkSelect_CheckedChanged(chkSelect, e);
    }

    protected void grdDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
            CheckBox chkFlat = e.Row.FindControl("chkFlat") as CheckBox;
            int PackageInvoiceAdditional_Id = 0;
            try
            {
                PackageInvoiceAdditional_Id = Convert.ToInt32(e.Row.Cells[1].Text.Trim());
            }
            catch
            {
                PackageInvoiceAdditional_Id = 0;
            }
            string Deduction_Type = e.Row.Cells[3].Text.Trim();
            if (Deduction_Type == "Percentage")
            {
                chkFlat.Checked = false;
            }
            else
            {
                chkFlat.Checked = true;
            }

            int Deduction_isFlat = 0;
            try
            {
                Deduction_isFlat = Convert.ToInt32(e.Row.Cells[2].Text.Trim());
            }
            catch
            {
                Deduction_isFlat = 0;
            }
            if (Deduction_isFlat == 1)
            {
                chkFlat.Checked = true;
            }
            else
            {
                chkFlat.Checked = false;
            }

            if (PackageInvoiceAdditional_Id > 0)
            {
                chkSelect.Checked = true;
                chkSelect_CheckedChanged(chkSelect, e);
            }
            else
            {
                chkSelect.Checked = false;
            }
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

    protected void btnApproveInvoice_Click(object sender, EventArgs e)
    {
        decimal Total_Amount = 0;
        decimal Transfred_Amount = 0;
        if (divAmountTransfered.Visible)
        {
            try
            {
                Transfred_Amount = decimal.Parse(txtFudTransfered.Text.Trim());
            }
            catch
            {
                Transfred_Amount = 0;
            }
            if (Transfred_Amount == 0)
            {
                MessageBox.Show("Transfred Amount Should Be More Than Zero.");
                return;
            }

            try
            {
                Total_Amount = decimal.Parse(lblTotalAmount.Text.Trim());
            }
            catch
            {
                Total_Amount = 0;
            }
            if (Transfred_Amount > Total_Amount)
            {
                MessageBox.Show("Transfred Amount Should Be Less or Equal To Total Amount.");
                return;
            }
        }
        if (txtInvoiceNo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Invoice No");
            return;
        }
        if (ddlStatus.SelectedValue == "0" || ddlStatus.SelectedValue == "")
        {
            MessageBox.Show("Please Select Status");
            return;
        }
        if (ddlStatus.SelectedValue == "2" || ddlStatus.SelectedValue == "3")
        {
            if (txtComments.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Comments / Reason");
                return;
            }
        }

        if (Session["PersonJuridiction_DesignationId"].ToString() == "4")
        {
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_PackageInvoiceCover(Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim()), 0);
            if (!AllClasses.CheckDataSet(ds))
            {
                MessageBox.Show("Please Update Cover Letter Details before Approval Of Invoice.");
                return;
            }
        }
        tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
        obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_ApprovedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_Date = txtInvoiceDate.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_DBR_No = txtDBRNo.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
        obj_tbl_PackageInvoice.PackageInvoice_Narration = txtNarration.Text.Trim();
        obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
        obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = txtInvoiceNo.Text.Trim();

        List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
        for (int i = 0; i < grdBOQ.Rows.Count; i++)
        {
            TextBox txtRemarks = grdBOQ.Rows[i].FindControl("txtRemarks") as TextBox;
            tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[0].Text.Trim());
            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Remarks = txtRemarks.Text.Trim();
            List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
            GridView grdTaxes = grdBOQ.Rows[i].FindControl("grdTaxes") as GridView;
            for (int k = 0; k < grdTaxes.Rows.Count; k++)
            {
                tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_PackageInvoiceItem_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[0].Text.Trim());
                try
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = 0;
                }
                try
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = 0;
                }
                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status = 1;
                if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                {
                    if (obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        for (int j = 0; j < obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li.Count; j++)
                        {
                            if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_PackageInvoiceItem_Id == obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li[j].PackageInvoiceItem_Tax_PackageInvoiceItem_Id && obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id == obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li[j].PackageInvoiceItem_Tax_Deduction_Id)
                            {
                                MessageBox.Show("Deduction Already Addded. ");
                                return;
                            }
                        }
                    }
                }

                if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                {
                    obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                }
                if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                {
                    obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                }

            }
            obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);
        }

        List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li = new List<tbl_PackageInvoiceAdditional>();
        for (int i = 0; i < grdDeductions.Rows.Count; i++)
        {
            TextBox txtDeductionValue = grdDeductions.Rows[i].FindControl("txtDeductionValue") as TextBox;
            TextBox txtDeductionAmount = grdDeductions.Rows[i].FindControl("txtDeductionAmount") as TextBox;
            TextBox txtDeductionComments = grdDeductions.Rows[i].FindControl("txtDeductionComments") as TextBox;
            CheckBox chkSelect = grdDeductions.Rows[i].FindControl("chkSelect") as CheckBox;
            CheckBox chkFlat = grdDeductions.Rows[i].FindControl("chkFlat") as CheckBox;
            decimal DeductionValue = 0;
            if (chkSelect.Checked && !chkFlat.Checked)
            {
                try
                {
                    DeductionValue = Convert.ToDecimal(txtDeductionValue.Text.Trim());
                }
                catch
                {
                    DeductionValue = 0;
                }
                try
                {
                    txtDeductionAmount.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * DeductionValue) / 100), 2, MidpointRounding.AwayFromZero).ToString();
                }
                catch
                {
                    txtDeductionAmount.Text = "0";
                }

                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments.Text.Trim();
                if (chkFlat.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = DeductionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
            else if (chkFlat.Checked && chkSelect.Checked)
            {
                try
                {
                    DeductionValue = Convert.ToDecimal(txtDeductionValue.Text.Trim());
                }
                catch
                {
                    DeductionValue = 0;
                }
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments.Text.Trim();
                if (chkFlat.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = DeductionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }

        for (int i = 0; i < grdDeductions2.Rows.Count; i++)
        {
            TextBox txtDeductionValue2 = grdDeductions2.Rows[i].FindControl("txtDeductionValue2") as TextBox;
            TextBox txtDeductionAmount2 = grdDeductions2.Rows[i].FindControl("txtDeductionAmount2") as TextBox;
            TextBox txtDeductionComments2 = grdDeductions2.Rows[i].FindControl("txtDeductionComments2") as TextBox;
            CheckBox chkSelect2 = grdDeductions2.Rows[i].FindControl("chkSelect2") as CheckBox;
            CheckBox chkFlat2 = grdDeductions2.Rows[i].FindControl("chkFlat2") as CheckBox;
            decimal AdditionValue = 0;
            if (chkSelect2.Checked && !chkFlat2.Checked)
            {
                try
                {
                    AdditionValue = Convert.ToDecimal(txtDeductionValue2.Text.Trim());
                }
                catch
                {
                    AdditionValue = 0;
                }
                try
                {
                    txtDeductionAmount2.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * AdditionValue) / 100), 2, MidpointRounding.AwayFromZero).ToString();
                }
                catch
                {
                    txtDeductionAmount2.Text = "0";
                }

                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions2.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments2.Text.Trim();
                if (chkFlat2.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = AdditionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
            else if (chkFlat2.Checked && chkSelect2.Checked)
            {
                try
                {
                    AdditionValue = Convert.ToDecimal(txtDeductionValue2.Text.Trim());
                }
                catch
                {
                    AdditionValue = 0;
                }
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id = Convert.ToInt32(grdDeductions2.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = txtDeductionComments2.Text.Trim();
                if (chkFlat2.Checked)
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master = AdditionValue;
                }
                catch
                { }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }

        List<tbl_PackageInvoiceDocs> obj_tbl_PackageInvoiceDocs_Li = new List<tbl_PackageInvoiceDocs>();
        for (int i = 0; i < grdDocumentMaster.Rows.Count; i++)
        {
            FileUpload flUpload = grdDocumentMaster.Rows[i].FindControl("flUpload") as FileUpload;
            TextBox txtDocumentOrderNo = grdDocumentMaster.Rows[i].FindControl("txtDocumentOrderNo") as TextBox;
            TextBox txtDocumentComments = grdDocumentMaster.Rows[i].FindControl("txtDocumentComments") as TextBox;
            if (flUpload.HasFile)
            {
                tbl_PackageInvoiceDocs obj_tbl_PackageInvoiceDocs = new tbl_PackageInvoiceDocs();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileBytes = flUpload.FileBytes;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName = flUpload.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Status = 1;
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_OrderNo = txtDocumentOrderNo.Text.Trim();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Comments = txtDocumentComments.Text.Trim();
                obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Type = Convert.ToInt32(grdDocumentMaster.Rows[i].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceDocs_Li.Add(obj_tbl_PackageInvoiceDocs);
            }
            else
            {
                MessageBox.Show("Please Upload Document.");
                return;
            }
        }

        tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = txtComments.Text.Trim();
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = Convert.ToInt32(ddlStatus.SelectedValue);
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;

        tbl_FinancialTrans obj_tbl_FinancialTrans = null;
        if (divAmountTransfered.Visible && Transfred_Amount != 0)
        {
            obj_tbl_FinancialTrans = new tbl_FinancialTrans();
            obj_tbl_FinancialTrans.FinancialTrans_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_FinancialTrans.FinancialTrans_Amount = Total_Amount;
            obj_tbl_FinancialTrans.FinancialTrans_Comments = txtComments.Text.Trim();
            obj_tbl_FinancialTrans.FinancialTrans_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_FinancialTrans.FinancialTrans_EntryType = "C";
            obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = 0;
            obj_tbl_FinancialTrans.FinancialTrans_Invoice_Id = Convert.ToInt32(grdBOQ.Rows[0].Cells[1].Text.Trim());
            obj_tbl_FinancialTrans.FinancialTrans_Status = 1;
            obj_tbl_FinancialTrans.FinancialTrans_TransAmount = Transfred_Amount;
            obj_tbl_FinancialTrans.FinancialTrans_TransType = "C";
        }
        if (new DataLayer().Update_tbl_PackageInvoice(obj_tbl_PackageInvoice, obj_tbl_PackageInvoiceAdditional_Li, obj_tbl_PackageInvoiceDocs_Li, obj_tbl_PackageInvoiceItem_Li, obj_tbl_FinancialTrans, "A", obj_tbl_PackageInvoiceApproval, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString())))
        {
            MessageBox.Show("Invoice Details Approved Successfully");
            Response.Redirect("MasterGenerateInvoice_View?Package_Id=0&Invoice_Id=" + obj_tbl_PackageInvoice.PackageInvoice_Id.ToString());
            return;
        }
        else
        {
            MessageBox.Show("Error In Approval of Invoice Details...");
            return;
        }
    }

    protected void chkFlat_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void txtDeductionAmount_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as TextBox).Parent.Parent as GridViewRow;
        CheckBox chkFlat = gr.FindControl("chkFlat") as CheckBox;
        TextBox txtDeductionValue = gr.FindControl("txtDeductionValue") as TextBox;
        TextBox txtDeductionAmount = gr.FindControl("txtDeductionAmount") as TextBox;
        CheckBox chkSelect = gr.FindControl("chkSelect") as CheckBox;
        if (chkFlat.Checked && chkSelect.Checked)
        {
            decimal invoice_Total = 0;
            decimal DeductionAmount = 0;

            try
            {
                invoice_Total = Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim());
            }
            catch
            {
                invoice_Total = 0;
            }

            try
            {
                DeductionAmount = Convert.ToDecimal(txtDeductionAmount.Text.Trim());
            }
            catch
            {
                DeductionAmount = 0;
            }
            if (DeductionAmount > 0)
            {
                txtDeductionValue.Text = decimal.Round((DeductionAmount * 100) / invoice_Total, 2, MidpointRounding.AwayFromZero).ToString();
            }

            chkSelect_CheckedChanged(chkSelect, e);
        }

        decimal DeductionValue_Total = 0;
        for (int i = 0; i < grdDeductions.Rows.Count; i++)
        {
            txtDeductionAmount = grdDeductions.Rows[i].FindControl("txtDeductionAmount") as TextBox;
            try
            {
                DeductionValue_Total += Convert.ToDecimal(txtDeductionAmount.Text.Trim());
            }
            catch
            {
                DeductionValue_Total += 0;
            }
        }
        grdDeductions.FooterRow.Cells[10].Text = DeductionValue_Total.ToString();
    }

    protected void grdDocumentMaster_PreRender(object sender, EventArgs e)
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

        Cover_Letter obj_Cover_Letter = new Cover_Letter();
        DataSet ds = new DataSet();
        int Invoice_Id = Convert.ToInt32(hf_Invoice_Id.Value);
        ds = (new DataLayer()).get_Report_Cover_Details(Invoice_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            obj_Cover_Letter.Contractor_Name = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
            obj_Cover_Letter.Project_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();

            try
            {
                obj_Cover_Letter.Total_Centre_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["Central_Share"].ToString());
            }
            catch
            { }
            try
            {
                obj_Cover_Letter.Total_State_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["State_Share"].ToString());
            }
            catch
            { }
            try
            {
                obj_Cover_Letter.Total_ULB_Share = Convert.ToDecimal(ds.Tables[0].Rows[0]["ULB_Share"].ToString());
            }
            catch
            { }

            obj_Cover_Letter.Work_Name = ds.Tables[0].Rows[0]["ProjectWork_Name"].ToString();
            obj_Cover_Letter.Project_Id = ds.Tables[0].Rows[0]["ProjectWork_ProjectCode"].ToString();
            obj_Cover_Letter.Project_Type = ds.Tables[0].Rows[0]["ProjectType_Name"].ToString();

            obj_Cover_Letter.Financial_Year = "2020";
            obj_Cover_Letter.Account_Holder_Name = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
            try
            {
                obj_Cover_Letter.Amount_Received_To_Implementing_Agency_Including_Diversion = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Amount_Received_To_Implementing_Agency_Including_Diversion = 0;
            }

            try
            {
                obj_Cover_Letter.Amount_Released_To_Division = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Amount_Released_To_Division"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Amount_Released_To_Division = 0;
            }
            try
            {
                obj_Cover_Letter.Balance_Amount_As_In_Bank_Statement = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Balance_Amount_As_In_Bank_Statement = 0;
            }

            try
            {
                obj_Cover_Letter.Centage = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Centage"].ToString());
            }

            catch
            {
                obj_Cover_Letter.Centage = 0;
            }
            try
            {
                obj_Cover_Letter.Expenditure_Done_By_Implementing_Agency = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Expenditure_Done_By_Implementing_Agency = 0;
            }

            try
            {
                obj_Cover_Letter.Expenditure_By_Division = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Expenditure_By_Division"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Expenditure_By_Division = 0;
            }

            //obj_Cover_Letter.Financial_Year = "";

            try
            {
                obj_Cover_Letter.Find_Received = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_DiversionIn"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Find_Received = 0;
            }
            try
            {
                obj_Cover_Letter.Fund_Diverted = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_DiversionOut"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Fund_Diverted = 0;
            }
            obj_Cover_Letter.General_Manager = "";
            obj_Cover_Letter.Place = "";
            //obj_Cover_Letter.Project_Id = "";
            obj_Cover_Letter.Project_Manager = "";
            //obj_Cover_Letter.Project_Type = "";

            try
            {
                obj_Cover_Letter.Release_To_Implementing_Agency = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_ReleaseTillDate"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Release_To_Implementing_Agency = 0;
            }
            try
            {
                obj_Cover_Letter.Sanctioned_Amount_Without_Centage = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_SanctionedAmount"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Sanctioned_Amount_Without_Centage = 0;
            }
            obj_Cover_Letter.Scheme_Name = "";
            try
            {
                obj_Cover_Letter.Tendred_Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_TenderAmount"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Tendred_Amount = 0;
            }

            try
            {
                obj_Cover_Letter.Total_Amount_Paid_To_Contractor_Till_Date = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_PaymentTillDate"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Total_Amount_Paid_To_Contractor_Till_Date = 0;
            }
            try
            {
                obj_Cover_Letter.Total_Invoice_Value = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Invoice_Value"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Total_Invoice_Value = 0;
            }
            try
            {
                obj_Cover_Letter.Total_Mobelization_Advance = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_MoblizationAdvance"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Total_Mobelization_Advance = 0;
            }

            try
            {
                obj_Cover_Letter.Total_Mobelization_Advance_Adjustment_Before_Bill = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_MoblizationAdvanceAdjustment"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Total_Mobelization_Advance_Adjustment_Before_Bill = 0;
            }
            try
            {
                obj_Cover_Letter.Total_Mobelization_Advance_Adjustment_In_Current_Bill = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill"].ToString());
            }
            catch
            {
                obj_Cover_Letter.Total_Mobelization_Advance_Adjustment_In_Current_Bill = 0;
            }

            obj_Cover_Letter_Li.Add(obj_Cover_Letter);
            Session["Cover_Letter"] = obj_Cover_Letter_Li;
            mpViewCoverLetter.Show();
        }
        else
        {
            MessageBox.Show("Unable To View Report");
            return;
        }
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
                obj_ProjectSummery.Total_Payment_Earlier = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Payment_Earlier"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Total_Payment_Earlier = 0;
            }
            try
            {
                obj_ProjectSummery.Total_Proposed_Payment_Jal_Nigam = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Total_Proposed_Payment_Jal_Nigam = 0;
            }
            try
            {
                obj_ProjectSummery.Total_Release = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Release"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Total_Release = 0;
            }

            try
            {
                obj_ProjectSummery.Total_With_Held_Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_With_Held_Amount"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Total_With_Held_Amount = 0;
            }
            try
            {
                obj_ProjectSummery.Work_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Work_Amount"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Work_Cost = 0;
            }
            try
            {
                obj_ProjectSummery.Total_Balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Balance"].ToString());
            }
            catch
            {
                obj_ProjectSummery.Total_Balance = 0;
            }
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

    protected void grdDeductions2_PreRender(object sender, EventArgs e)
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

    protected void grdDeductions2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect2 = e.Row.FindControl("chkSelect2") as CheckBox;
            CheckBox chkFlat2 = e.Row.FindControl("chkFlat2") as CheckBox;
            int PackageInvoiceAdditional_Id = 0;
            try
            {
                PackageInvoiceAdditional_Id = Convert.ToInt32(e.Row.Cells[1].Text.Trim());
            }
            catch
            {
                PackageInvoiceAdditional_Id = 0;
            }
            string Deduction_Type = e.Row.Cells[3].Text.Trim();
            if (Deduction_Type == "Percentage")
            {
                chkFlat2.Checked = false;
            }
            else
            {
                chkFlat2.Checked = true;
            }
            int Deduction_isFlat = 0;
            try
            {
                Deduction_isFlat = Convert.ToInt32(e.Row.Cells[2].Text.Trim());
            }
            catch
            {
                Deduction_isFlat = 0;
            }
            if (Deduction_isFlat == 1)
            {
                chkFlat2.Checked = true;
            }
            else
            {
                chkFlat2.Checked = false;
            }

            if (PackageInvoiceAdditional_Id > 0)
            {
                chkSelect2.Checked = true;
                chkSelect2_CheckedChanged(chkSelect2, e);
            }
            else
            {
                chkSelect2.Checked = false;
            }
        }
    }

    protected void chkSelect2_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelect2 = sender as CheckBox;
        GridViewRow gr = chkSelect2.Parent.Parent as GridViewRow;
        TextBox txtDeductionAmount2 = gr.FindControl("txtDeductionAmount2") as TextBox;
        TextBox txtDeductionValue2 = gr.FindControl("txtDeductionValue2") as TextBox;
        CheckBox chkFlat2 = gr.FindControl("chkFlat2") as CheckBox;
        if (chkSelect2.Checked && !chkFlat2.Checked)
        {
            decimal DeductionValue2 = 0;
            try
            {
                DeductionValue2 = Convert.ToDecimal(txtDeductionValue2.Text.Trim());
            }
            catch
            {
                DeductionValue2 = 0;
            }
            try
            {
                txtDeductionAmount2.Text = decimal.Round(((Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim()) * DeductionValue2) / 100), 2, MidpointRounding.AwayFromZero).ToString();
            }
            catch
            {
                txtDeductionAmount2.Text = "0";
            }
        }
        else if (chkFlat2.Checked && chkSelect2.Checked)
        {
            decimal invoice_Total2 = 0;
            decimal DeductionAmount2 = 0;

            try
            {
                invoice_Total2 = Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim());
            }
            catch
            {
                invoice_Total2 = 0;
            }

            try
            {
                DeductionAmount2 = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
            }
            catch
            {
                DeductionAmount2 = 0;
            }
            if (DeductionAmount2 > 0)
            {
                txtDeductionValue2.Text = decimal.Round((DeductionAmount2 * 100) / invoice_Total2, 2, MidpointRounding.AwayFromZero).ToString();
            }
        }
        else
        {
            txtDeductionAmount2.Text = "0";
        }
        if (e.GetType().Name == "GridViewRowEventArgs")
        { }
        else
        {
            decimal DeductionValue_Total2 = 0;
            for (int i = 0; i < grdDeductions2.Rows.Count; i++)
            {
                txtDeductionAmount2 = grdDeductions2.Rows[i].FindControl("txtDeductionAmount2") as TextBox;
                try
                {
                    DeductionValue_Total2 += Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
                }
                catch
                {
                    DeductionValue_Total2 += 0;
                }
            }
            grdDeductions2.FooterRow.Cells[10].Text = DeductionValue_Total2.ToString();
        }
        Calculate_Total();
    }

    protected void chkFlat2_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void txtDeductionValue2_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as TextBox).Parent.Parent as GridViewRow;
        CheckBox chkSelect2 = gr.FindControl("chkSelect2") as CheckBox;
        if (chkSelect2.Checked)
            chkSelect2_CheckedChanged(chkSelect2, e);
    }

    protected void txtDeductionAmount2_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as TextBox).Parent.Parent as GridViewRow;
        CheckBox chkFlat2 = gr.FindControl("chkFlat2") as CheckBox;
        TextBox txtDeductionValue2 = gr.FindControl("txtDeductionValue2") as TextBox;
        TextBox txtDeductionAmount2 = gr.FindControl("txtDeductionAmount2") as TextBox;
        CheckBox chkSelect2 = gr.FindControl("chkSelect2") as CheckBox;
        if (chkFlat2.Checked && chkSelect2.Checked)
        {
            decimal invoice_Total = 0;
            decimal DeductionAmount2 = 0;

            try
            {
                invoice_Total = Convert.ToDecimal(grdBOQ.FooterRow.Cells[11].Text.Trim());
            }
            catch
            {
                invoice_Total = 0;
            }

            try
            {
                DeductionAmount2 = Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
            }
            catch
            {
                DeductionAmount2 = 0;
            }
            if (DeductionAmount2 > 0)
            {
                txtDeductionValue2.Text = decimal.Round((DeductionAmount2 * 100) / invoice_Total, 2, MidpointRounding.AwayFromZero).ToString();
            }

            chkSelect2_CheckedChanged(chkSelect2, e);
        }

        decimal DeductionValue_Total2 = 0;
        for (int i = 0; i < grdDeductions2.Rows.Count; i++)
        {
            txtDeductionAmount2 = grdDeductions2.Rows[i].FindControl("txtDeductionAmount2") as TextBox;
            try
            {
                DeductionValue_Total2 += Convert.ToDecimal(txtDeductionAmount2.Text.Trim());
            }
            catch
            {
                DeductionValue_Total2 += 0;
            }
        }
        grdDeductions2.FooterRow.Cells[10].Text = DeductionValue_Total2.ToString();
    }
    protected void grdMultipleFiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDownload = (e.Row.FindControl("lnkDownload") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload);
        }
    }
    protected void grdDocumentMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void grdTaxes_PreRender(object sender, EventArgs e)
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



    protected void btnViewEMB_Click(object sender, ImageClickEventArgs e)
    {
        try
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
            Response.Redirect("ViewEMBDetails?Invoice_Id=" + Invoice_Id);
        }
        catch
        {

        }
    }



    protected void tnViewBOQ_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
            int Package_Id = 0;
            try
            {
                Package_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
            }
            catch
            {
                Package_Id = 0;
            }
            Response.Redirect("View_BOQ_Details?Package_Id=" + Package_Id);
        }
        catch
        {

        }
    }

    protected void link_PaymentOrder_ServerClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Office_Order?Invoice_Id=" + hf_Invoice_Id.Value);
        }
        catch
        {

        }
    }
}
