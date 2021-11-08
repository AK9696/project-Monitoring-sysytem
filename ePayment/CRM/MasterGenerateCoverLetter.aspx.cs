using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterGenerateCoverLetter : System.Web.UI.Page
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
            int Invoice_Id = 0;
            if (Request.QueryString.Count > 0)
            {
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
            }
            if (Invoice_Id > 0)
            {
                divEntry.Visible = true;
                hf_Invoice_Id.Value = Invoice_Id.ToString();
                get_tbl_Invoice_Details(Invoice_Id);
            }
        }
    }
    private void get_tbl_Invoice_Details(int Invoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Invoice_Details(Invoice_Id);
        if (ds != null)
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
        else
        {
            MessageBox.Show("Server Error!!");
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
        hf_Package_Id.Value = Package_Id.ToString();
        hf_Work_Id.Value = Work_Id.ToString();
        for (int i = 0; i < grdInvoice.Rows.Count; i++)
        {
            grdInvoice.Rows[i].BackColor = Color.Transparent;
        }
        gr.BackColor = Color.LightGreen;
        getPackage_Details(Package_Id);
        get_PackageInvoiceCover(Package_Id, Invoice_Id);
        if (Session["PersonJuridiction_DesignationId"].ToString() == "33")
        {
            divReleaseInstallment.Visible = true;
        }
        else
        {
            divReleaseInstallment.Visible = false;
        }
    }
    private void getPackage_Details(int Package_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, 0, 0, 0, 0, 0, Package_Id, "", "", false, "");
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
    private void get_PackageInvoiceCover(int Package_Id, int Invoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_PackageInvoiceCover(Invoice_Id, Package_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtSanctionedAmount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_SanctionedAmount"].ToString();
            txtCentage.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Centage"].ToString();
            txtTendredAmount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_TenderAmount"].ToString();
            txtCentralShare.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_CentralShare"].ToString();
            txtStateShare.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_StateShare"].ToString();
            txtULBShare.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_ULBShare"].ToString();
            txtReleaseTillDate.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_ReleaseTillDate"].ToString();
            txtDiversionOut.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_DiversionOut"].ToString();
            txtDiversionIn.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_DiversionIn"].ToString();
            txtPaidToContractorTillDate.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_PaymentTillDate"].ToString();
            txtMobilizationAdvance.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_MoblizationAdvance"].ToString();
            txtMobilizationAdvanceAdjustment.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_MoblizationAdvanceAdjustment"].ToString();
            txtMobelization_Advance_Adjustment_In_Current_Bill.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill"].ToString();
            txtTotal_Invoice_Value.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Invoice_Value"].ToString();
            txtAmount_Received_To_Implementing_Agency_Including_Diversion.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion"].ToString();
            txtExpenditure_Done_By_Implementing_Agency.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency"].ToString();
            txtBalance_Amount_As_In_Bank_Statement.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement"].ToString();
            txtAmount_Released_To_Division.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Amount_Released_To_Division"].ToString();
            txtExpenditure_By_Division.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Expenditure_By_Division"].ToString();
        }
        else
        {
           
            ds = (new DataLayer()).get_PackageDetailsFillCoverLetter(Convert.ToInt32(hf_Work_Id.Value));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtSanctionedAmount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_SanctionedAmount"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                txtCentage.Text = ds.Tables[1].Rows[0]["PackageInvoiceCover_Centage"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                txtTendredAmount.Text = ds.Tables[2].Rows[0]["PackageInvoiceCover_TenderAmount"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
            {
                txtCentralShare.Text = ds.Tables[3].Rows[0]["PackageInvoiceCover_CentralShare"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
            {
                txtStateShare.Text = ds.Tables[4].Rows[0]["PackageInvoiceCover_StateShare"].ToString();
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
            {
                txtULBShare.Text = ds.Tables[5].Rows[0]["PackageInvoiceCover_ULBShare"].ToString();
            }
        }

        ds = (new DataLayer()).get_tbl_ProjectWorkGO(Convert.ToInt32(hf_Work_Id.Value));

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ViewState["dtQuestionnaire"] = ds.Tables[0];
            grdCallProductDtls.DataSource = ds.Tables[0];
            grdCallProductDtls.DataBind();

        }
        else
        {
            try
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["ProjectWorkGO_Id"] = 0;
                dr["ProjectWorkGO_TotalRelease"] = 0;
                dr["ProjectWorkGO_CentralShare"] = 0;
                dr["ProjectWorkGO_StateShare"] = 0;
                dr["ProjectWorkGO_ULBShare"] = 0;
                dr["ProjectWorkGO_GO_Date"] = "";
                dr["ProjectWorkGO_GO_Number"] = "";
                ds.Tables[0].Rows.Add(dr);

                ViewState["dtQuestionnaire"] = ds.Tables[0];
                grdCallProductDtls.DataSource = ds.Tables[0];
                grdCallProductDtls.DataBind();
            }
            catch
            {
                grdCallProductDtls.DataSource = null;
                grdCallProductDtls.DataBind();
            }
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


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (hf_Package_Id.Value == "" || hf_Package_Id.Value == "0")
        {
            MessageBox.Show("Please Select A Package");
            return;
        }
        if (hf_Invoice_Id.Value == "" || hf_Invoice_Id.Value == "0")
        {
            MessageBox.Show("Please Select A Invoice");
            return;
        }
        tbl_PackageInvoiceCover obj_tbl_PackageInvoiceCover = new tbl_PackageInvoiceCover();

        int Package_Id = Convert.ToInt32(hf_Package_Id.Value);
        int Work_Id = Convert.ToInt32(hf_Work_Id.Value);
        int Invoice_Id = Convert.ToInt32(hf_Invoice_Id.Value);
        obj_tbl_PackageInvoiceCover.PackageInvoiceCover_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Centage = Convert.ToDecimal(txtCentage.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Centage = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_SanctionedAmount = Convert.ToDecimal(txtSanctionedAmount.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_SanctionedAmount = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_CentralShare = Convert.ToDecimal(txtCentralShare.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_CentralShare = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_StateShare = Convert.ToDecimal(txtStateShare.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_StateShare = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ULBShare = Convert.ToDecimal(txtULBShare.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ULBShare = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionIn = Convert.ToDecimal(txtDiversionIn.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionIn = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionOut = Convert.ToDecimal(txtDiversionOut.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionOut = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id = Convert.ToInt32(hf_Invoice_Id.Value);
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvance = Convert.ToDecimal(txtMobilizationAdvance.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvance = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvanceAdjustment = Convert.ToDecimal(txtMobilizationAdvanceAdjustment.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvanceAdjustment = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill = Convert.ToDecimal(txtMobelization_Advance_Adjustment_In_Current_Bill.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Invoice_Value = Convert.ToDecimal(txtTotal_Invoice_Value.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Invoice_Value = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion = Convert.ToDecimal(txtAmount_Received_To_Implementing_Agency_Including_Diversion.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency = Convert.ToDecimal(txtExpenditure_Done_By_Implementing_Agency.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement = Convert.ToDecimal(txtBalance_Amount_As_In_Bank_Statement.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Released_To_Division = Convert.ToDecimal(txtAmount_Released_To_Division.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Released_To_Division = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_By_Division = Convert.ToDecimal(txtExpenditure_By_Division.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_By_Division = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_PaymentTillDate = Convert.ToDecimal(txtPaidToContractorTillDate.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_PaymentTillDate = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ReleaseTillDate = Convert.ToDecimal(txtReleaseTillDate.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ReleaseTillDate = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_TenderAmount = Convert.ToDecimal(txtTendredAmount.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_TenderAmount = 0;
        }
        obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Status = 1;

        List<tbl_ProjectWorkGO> obj_tbl_ProjectWorkGO_Li = new List<tbl_ProjectWorkGO>();
        try
        {
            for (int i = 0; i < grdCallProductDtls.Rows.Count; i++)
            {
                decimal Allocated_Budget = 0;
                decimal CentralShare_Budget = 0;
                decimal StateShare_Budget = 0;
                decimal ULBShare_Budget = 0;

                TextBox txtGODate = grdCallProductDtls.Rows[i].FindControl("txtFinancialTrans_GO_Date") as TextBox;
                TextBox txtTransactionAmount = grdCallProductDtls.Rows[i].FindControl("txtTransactionAmount") as TextBox;
                TextBox txtCentralShare = grdCallProductDtls.Rows[i].FindControl("txtCentralShare") as TextBox;
                TextBox txtStateShare = grdCallProductDtls.Rows[i].FindControl("txtStateShare") as TextBox;
                TextBox txtULBShare = grdCallProductDtls.Rows[i].FindControl("txtULBShare") as TextBox;
                TextBox txtFinancialTrans_GO_Number = grdCallProductDtls.Rows[i].FindControl("txtFinancialTrans_GO_Number") as TextBox;
                try
                {
                    Allocated_Budget = Convert.ToDecimal(txtTransactionAmount.Text);
                }
                catch
                {
                    Allocated_Budget = 0;
                }
                try
                {
                    CentralShare_Budget = Convert.ToDecimal(txtCentralShare.Text);
                }
                catch
                {
                    CentralShare_Budget = 0;
                }
                try
                {
                    StateShare_Budget = Convert.ToDecimal(txtStateShare.Text);
                }
                catch
                {
                    StateShare_Budget = 0;
                }
                try
                {
                    ULBShare_Budget = Convert.ToDecimal(txtULBShare.Text);
                }
                catch
                {
                    ULBShare_Budget = 0;
                }
                tbl_ProjectWorkGO obj_tbl_ProjectWorkGO = new tbl_ProjectWorkGO();
                try
                {
                    obj_tbl_ProjectWorkGO.ProjectWorkGO_Id = Convert.ToInt32(grdCallProductDtls.Rows[i].Cells[0].Text);
                }
                catch
                {
                    obj_tbl_ProjectWorkGO.ProjectWorkGO_Id = 0;
                }
                obj_tbl_ProjectWorkGO.ProjectWorkGO_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Date = txtGODate.Text.Trim();
                obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Number = txtFinancialTrans_GO_Number.Text.Trim();
                obj_tbl_ProjectWorkGO.ProjectWorkGO_Work_Id = Work_Id;
                obj_tbl_ProjectWorkGO.ProjectWorkGO_Status = 1;
                obj_tbl_ProjectWorkGO.ProjectWorkGO_TotalRelease = Allocated_Budget * 100000;
                obj_tbl_ProjectWorkGO.ProjectWorkGO_CentralShare = CentralShare_Budget * 100000;
                obj_tbl_ProjectWorkGO.ProjectWorkGO_StateShare = StateShare_Budget * 100000;
                obj_tbl_ProjectWorkGO.ProjectWorkGO_ULBShare = ULBShare_Budget * 100000;

                obj_tbl_ProjectWorkGO_Li.Add(obj_tbl_ProjectWorkGO);


            }
        }
        catch
        {

        }
        if (new DataLayer().Insert_PackageInvoiceCover(obj_tbl_PackageInvoiceCover, null, Package_Id, "CoverLetter", obj_tbl_ProjectWorkGO_Li))
        {
            MessageBox.Show("Details Updated Successfully");
            return;
        }
        else
        {
            MessageBox.Show("Unable To Update Details");
            return;
        }
    }

    protected void btnQuestionnaire_Click(object sender, EventArgs e)
    {
        Add_Questionire();
    }

    private void Add_Questionire()
    {
        DataTable dtQuestionnaire;
        if (ViewState["dtQuestionnaire"] != null)
        {
            dtQuestionnaire = (DataTable)(ViewState["dtQuestionnaire"]);
            DataRow dr = dtQuestionnaire.NewRow();
            dtQuestionnaire.Rows.Add(dr);
            ViewState["dtQuestionnaire"] = dtQuestionnaire;

            grdCallProductDtls.DataSource = dtQuestionnaire;
            grdCallProductDtls.DataBind();
        }
        else
        {
            dtQuestionnaire = new DataTable();

            DataColumn dc_ProjectWorkGO_Id = new DataColumn("ProjectWorkGO_Id", typeof(int));
            DataColumn dc_ProjectWorkGO_GO_Date = new DataColumn("ProjectWorkGO_GO_Date", typeof(string));
            DataColumn dc_ProjectWorkGO_GO_Number = new DataColumn("ProjectWorkGO_GO_Number", typeof(string));
            DataColumn dc_ProjectWorkGO_TotalRelease = new DataColumn("ProjectWorkGO_TotalRelease", typeof(decimal));
            DataColumn dc_ProjectWorkGO_CentralShare = new DataColumn("ProjectWorkGO_CentralShare", typeof(decimal));
            DataColumn dc_ProjectWorkGO_StateShare = new DataColumn("ProjectWorkGO_StateShare", typeof(decimal));
            DataColumn dc_ProjectWorkGO_ULBShare = new DataColumn("ProjectWorkGO_ULBShare", typeof(decimal));


            dtQuestionnaire.Columns.AddRange(new DataColumn[] { dc_ProjectWorkGO_Id, dc_ProjectWorkGO_GO_Date, dc_ProjectWorkGO_GO_Number, dc_ProjectWorkGO_TotalRelease, dc_ProjectWorkGO_CentralShare, dc_ProjectWorkGO_StateShare, dc_ProjectWorkGO_ULBShare });

            DataRow dr = dtQuestionnaire.NewRow();
            dtQuestionnaire.Rows.Add(dr);
            ViewState["dtQuestionnaire"] = dtQuestionnaire;

            grdCallProductDtls.DataSource = dtQuestionnaire;
            grdCallProductDtls.DataBind();

        }
    }

    protected void imgdelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["dtQuestionnaire"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtQuestionnaire"];
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            grdCallProductDtls.DataSource = dt;
            grdCallProductDtls.DataBind();
            ViewState["dtQuestionnaire"] = dt;
        }
    }
}
