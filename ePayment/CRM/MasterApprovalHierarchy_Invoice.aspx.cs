using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterApprovalHierarchy_Invoice : System.Web.UI.Page
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
            List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = new List<tbl_ProcessConfigMaster>();
            ViewState["tbl_ProcessConfigMaster"] = obj_tbl_ProcessConfigMaster_Li;
            get_Branch_Office_Details();
            get_tbl_Department();
            get_tbl_Designation();
            get_tbl_ProcessConfigMaster();
            get_tbl_InvoiceStatus();
            get_tbl_TradeDocument();
        }
    }

    private void get_tbl_InvoiceStatus()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_InvoiceStatus(0, 0, 0, "");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            chkInvoiceStatus.DataTextField = "InvoiceStatus_Name";
            chkInvoiceStatus.DataValueField = "InvoiceStatus_Id";
            chkInvoiceStatus.DataSource = ds.Tables[0];
            chkInvoiceStatus.DataBind();
        }
        else
        {
            chkInvoiceStatus.Items.Clear();
        }
    }

    private void get_tbl_TradeDocument()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_TradeDocument();
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

    private void get_Branch_Office_Details()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Branch_Office_Details(0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlOrganization, "OfficeBranch_Name", "OfficeBranch_Id");
        }
        else
        {
            ddlOrganization.Items.Clear();
        }
    }

    private void get_tbl_ProcessConfigMaster()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProcessConfigMaster(Request.QueryString[0].ToString());
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = new List<tbl_ProcessConfigMaster>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tbl_ProcessConfigMaster obj_tbl_ProcessConfigMaster = new tbl_ProcessConfigMaster();
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Id"].ToString());
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_OrgId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_OrgId"].ToString());
                obj_tbl_ProcessConfigMaster.Organization_Name = ds.Tables[0].Rows[i]["Organization_Name"].ToString();
                obj_tbl_ProcessConfigMaster.Department_Name = ds.Tables[0].Rows[i]["Department_Name"].ToString();
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Department_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Department_Id"].ToString());
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Designation_Id"].ToString());
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id1 = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Designation_Id1"].ToString());
                try
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Loop = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Loop"].ToString());
                }
                catch
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Loop = 0;
                }
                obj_tbl_ProcessConfigMaster.Designation_Name = ds.Tables[0].Rows[i]["Designation_Name"].ToString();
                obj_tbl_ProcessConfigMaster.Designation_Name1 = ds.Tables[0].Rows[i]["Designation_Name1"].ToString();
                obj_tbl_ProcessConfigMaster.InvoiceStatus_Name = ds.Tables[0].Rows[i]["InvoiceStatus_Name"].ToString();
                obj_tbl_ProcessConfigMaster.ProcessConfigInvStatus_InvoiceStatus_Id = ds.Tables[0].Rows[i]["ProcessConfigInvStatus_InvoiceStatus_Id"].ToString();
                obj_tbl_ProcessConfigMaster.TradeDocument_Name = ds.Tables[0].Rows[i]["TradeDocument_Name"].ToString();
                obj_tbl_ProcessConfigMaster.ProcessConfigDocumentLinking_DocumentMaster_Id = ds.Tables[0].Rows[i]["ProcessConfigDocumentLinking_DocumentMaster_Id"].ToString();
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_AddedBy"].ToString());
                try
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Creation_Allowed = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Creation_Allowed"].ToString());
                }
                catch
                { }
                try
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Updation_Allowed = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Updation_Allowed"].ToString());
                }
                catch
                { }
                try
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Deduction_Allowed= Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Deduction_Allowed"].ToString());
                }
                catch
                { }
                try
                {
                    obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Transfer_Allowed = Convert.ToInt32(ds.Tables[0].Rows[i]["ProcessConfigMaster_Transfer_Allowed"].ToString());
                }
                catch
                { }
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Process_Name = Request.QueryString[0].ToString();
                obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Status = 1;

                obj_tbl_ProcessConfigMaster_Li.Add(obj_tbl_ProcessConfigMaster);

            }
            ViewState["tbl_ProcessConfigMaster"] = obj_tbl_ProcessConfigMaster_Li;

            grdComiteeMember.DataSource = obj_tbl_ProcessConfigMaster_Li;
            grdComiteeMember.DataBind();
        }
        else
        {
            List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = new List<tbl_ProcessConfigMaster>();
        }
    }

    private void reset()
    {
        ViewState["tbl_ProcessConfigMaster"] = null;
        grdComiteeMember.DataSource = null;
        grdComiteeMember.DataBind();
        txtComments.Text = "";
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

    private void get_tbl_Designation()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Designation();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDesignation, "Designation_DesignationName", "Designation_Id");
            if (ddlDesignation.Items.Count == 2)
            {
                ddlDesignation.SelectedIndex = 1;
            }
            AllClasses.FillDropDown_WithOutSelect(ds.Tables[0], ddlDesignation1, "Designation_DesignationName", "Designation_Id");
            if (ddlDesignation1.Items.Count == 2)
            {
                ddlDesignation1.SelectedIndex = 1;
            }
        }
        else
        {
            ddlDesignation.Items.Clear();
            ddlDesignation1.Items.Clear();
        }
    }

    private void get_tbl_Department()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Department();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDepartment, "Department_Name", "Department_Id");
        }
        else
        {
            ddlDepartment.Items.Clear();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (grdComiteeMember.Rows.Count == 0)
        {
            if (!chkCreate.Checked)
            {
                MessageBox.Show("It is mandatory that First Step Should be permitted for Creation / Addition..");
                return;
            }
            if (chkUpdate.Checked)
            {
                MessageBox.Show("It is mandatory that First Step Should not be permitted for Updation / Approval..");
                return;
            }
        }
        List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = (List<tbl_ProcessConfigMaster>)ViewState["tbl_ProcessConfigMaster"];

        if (ddlOrganization.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Organization");
            return;
        }
        if (ddlDepartment.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Department");
            return;
        }
        if (ddlDesignation.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Designation");
            return;
        }
        tbl_ProcessConfigMaster obj_tbl_ProcessConfigMaster = new tbl_ProcessConfigMaster();
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_OrgId = Convert.ToInt32(ddlOrganization.SelectedValue);
        obj_tbl_ProcessConfigMaster.Organization_Name = ddlOrganization.SelectedItem.Text;
        obj_tbl_ProcessConfigMaster.Department_Name = ddlDepartment.SelectedItem.Text;
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Department_Id = Convert.ToInt32(ddlDepartment.SelectedValue);
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id = Convert.ToInt32(ddlDesignation.SelectedValue);
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id1 = Convert.ToInt32(ddlDesignation1.SelectedValue);
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Loop = Convert.ToInt32(ddlLoop.SelectedValue);
        obj_tbl_ProcessConfigMaster.Designation_Name = ddlDesignation.SelectedItem.Text;
        if (ddlDesignation.SelectedValue == "0")
        {
            obj_tbl_ProcessConfigMaster.Designation_Name1 = "";
        }
        else {
            obj_tbl_ProcessConfigMaster.Designation_Name1 = ddlDesignation1.SelectedItem.Text;
        }
        
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Process_Name = Request.QueryString[0].ToString();
        obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Status = 1;
        if (chkCreate.Checked)
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Creation_Allowed = 1;
        }
        else
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Creation_Allowed = 0;
        }
        if (chkUpdate.Checked)
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Updation_Allowed = 1;
        }
        else
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Updation_Allowed = 0;
        }
        if (chkDeductionAllow.Checked)
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Deduction_Allowed = 1;
        }
        else
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Deduction_Allowed = 0;
        }
        if (chkAllowInputTransferAmount.Checked)
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Transfer_Allowed = 1;
        }
        else
        {
            obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Transfer_Allowed = 0;
        }
        string InvoiceStatus_Id = "";
        string InvoiceStatus_Name = "";
        for (int i = 0; i < chkInvoiceStatus.Items.Count; i++)
        {
            if (chkInvoiceStatus.Items[i].Selected)
            {
                InvoiceStatus_Id += chkInvoiceStatus.Items[i].Value.ToString() + ", ";
                InvoiceStatus_Name += chkInvoiceStatus.Items[i].Text.ToString() + ", ";
            }
        }
        if (InvoiceStatus_Id != "")
        {
            InvoiceStatus_Id = InvoiceStatus_Id.Trim().Substring(0, InvoiceStatus_Id.Trim().Length - 1);
        }
        if (InvoiceStatus_Name != "")
        {
            InvoiceStatus_Name = InvoiceStatus_Name.Trim().Substring(0, InvoiceStatus_Name.Trim().Length - 1);
        }
        obj_tbl_ProcessConfigMaster.ProcessConfigInvStatus_InvoiceStatus_Id = InvoiceStatus_Id;
        obj_tbl_ProcessConfigMaster.InvoiceStatus_Name = InvoiceStatus_Name;

        string DocumentMaster_Id = "";
        string DocumentMaster_Name = "";
        for (int i = 0; i < grdDocumentMaster.Rows.Count; i++)
        {
            CheckBox chkSelect = grdDocumentMaster.Rows[i].FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked)
            {
                DocumentMaster_Id += grdDocumentMaster.Rows[i].Cells[0].Text.Trim() + ", ";
                DocumentMaster_Name += grdDocumentMaster.Rows[i].Cells[3].Text.Trim() + ", ";
            }
        }
        if (DocumentMaster_Id != "")
        {
            DocumentMaster_Id = DocumentMaster_Id.Trim().Substring(0, DocumentMaster_Id.Trim().Length - 1);
        }
        if (DocumentMaster_Name != "")
        {
            DocumentMaster_Name = DocumentMaster_Name.Trim().Substring(0, DocumentMaster_Name.Trim().Length - 1);
        }
        obj_tbl_ProcessConfigMaster.ProcessConfigDocumentLinking_DocumentMaster_Id = DocumentMaster_Id;
        obj_tbl_ProcessConfigMaster.TradeDocument_Name = DocumentMaster_Name;

        obj_tbl_ProcessConfigMaster_Li.Add(obj_tbl_ProcessConfigMaster);
        ViewState["tbl_ProcessConfigMaster"] = obj_tbl_ProcessConfigMaster_Li;

        grdComiteeMember.DataSource = obj_tbl_ProcessConfigMaster_Li;
        grdComiteeMember.DataBind();
    }

    protected void grdComiteeMember_PreRender(object sender, EventArgs e)
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

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (grdComiteeMember.Rows.Count == 0)
        {
            MessageBox.Show("Please Add Atleast One Process To Save");
            return;
        }
        if (grdComiteeMember.Rows[grdComiteeMember.Rows.Count - 1].Cells[11].Text.Trim() == "0")
        {
            MessageBox.Show("Please Permit Updation / Approval Permission for Last Step of Process.");
            return;
        }
        List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = (List<tbl_ProcessConfigMaster>)ViewState["tbl_ProcessConfigMaster"];
        if (obj_tbl_ProcessConfigMaster_Li.Count > 0)
        {
            if (new DataLayer().Insert_tbl_ProcessConfigMaster(obj_tbl_ProcessConfigMaster_Li, Convert.ToInt32(Session["Person_Id"].ToString()), Request.QueryString[0].ToString()))
            {
                MessageBox.Show("Process Approval Configuration Updated");
                return;
            }
            else
            {
                MessageBox.Show("Error In Process Approval Configuration Updation");
                return;
            }
        }
        else
        {
            MessageBox.Show("Please Add Atleast One Configuration In The Order");
            return;
        }
    }

    protected void btnRemove_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = ((sender as ImageButton).Parent.Parent as GridViewRow);
        List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li = (List<tbl_ProcessConfigMaster>)ViewState["tbl_ProcessConfigMaster"];
        if (obj_tbl_ProcessConfigMaster_Li.Count > 0)
        {
            obj_tbl_ProcessConfigMaster_Li.RemoveAt(gr.RowIndex);

            ViewState["tbl_ProcessConfigMaster"] = obj_tbl_ProcessConfigMaster_Li;
            grdComiteeMember.DataSource = obj_tbl_ProcessConfigMaster_Li;
            grdComiteeMember.DataBind();
        }
        else
        {

        }
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
}
