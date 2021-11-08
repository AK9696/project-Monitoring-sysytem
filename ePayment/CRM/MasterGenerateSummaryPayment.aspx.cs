using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterGenerateSummaryPayment : System.Web.UI.Page
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
        for (int i = 0; i < grdInvoice.Rows.Count; i++)
        {
            grdInvoice.Rows[i].BackColor = Color.Transparent;
        }
        gr.BackColor = Color.LightGreen;
        getPackage_Details(Package_Id);
        get_PackageInvoiceCover(Package_Id, Invoice_Id);
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
    private void get_PackageInvoiceCover(int Package_Id, int Invoice_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_PackageInvoiceCover(Invoice_Id, Package_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtTotal_Payment_Earlier.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Payment_Earlier"].ToString();
            txtTotal_Proposed_Payment_Jal_Nigam.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam"].ToString();
            txtTotal_Release.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Release"].ToString();
            txtTotal_With_Held_Amount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_With_Held_Amount"].ToString();
            txtTotal_Balance.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Total_Balance"].ToString();
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
        obj_tbl_PackageInvoiceCover.PackageInvoiceCover_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Payment_Earlier = Convert.ToDecimal(txtTotal_Payment_Earlier.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Payment_Earlier = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam = Convert.ToDecimal(txtTotal_Proposed_Payment_Jal_Nigam.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Release = Convert.ToDecimal(txtTotal_Release.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Release = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_With_Held_Amount = Convert.ToDecimal(txtTotal_With_Held_Amount.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_With_Held_Amount = 0;
        }
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Balance = Convert.ToDecimal(txtTotal_Balance.Text.Trim());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Balance = 0;
        }

        obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Status = 1;
        try
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id = Convert.ToInt32(Request.QueryString["Invoice_Id"].ToString());
        }
        catch
        {
            obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id = 0;
        }



        if (new DataLayer().Insert_PackageInvoiceCover(obj_tbl_PackageInvoiceCover,null, Package_Id, "SummaryInvoice",null))
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
}