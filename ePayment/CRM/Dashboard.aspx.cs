using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
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
            ChkShowEMB.Checked = true;
            load_dashboard();
        }
    }
    private void Set_Mark_Status(int PackageInvoice_Id, CheckBox chkMark)
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
                if (get_tbl_InvoiceStatus(ConfigMaster_Id, "Invoice"))
                {
                    chkMark.Visible = true;
                }
                else
                {
                    chkMark.Visible = false;
                }
            }
            catch
            { }
        }
    }

    private bool get_tbl_InvoiceStatus(int ConfigMasterId, string Process_Name)
    {
        bool rVal = false;

        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(0, 0, 0, Process_Name);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), ConfigMasterId, Process_Name);
        }
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["InvoiceStatus_Id"].ToString() == "4")
                {
                    rVal = true;
                    break;
                }
            }
        }
        else
        {
            rVal = false;
        }
        return rVal;
    }
    private void load_dashboard()
    {
        get_PackageInvoice_Summery();
        get_tbl_PackageEMB();
        get_tbl_PackageInvoice();
    }

    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlScheme, "Project_Name", "Project_Id");
        }
        else
        {
            ddlScheme.Items.Clear();
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

    private void get_PackageInvoice_Summery()
    {
        int Scheme_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            Scheme_Id = Convert.ToInt32(ddlScheme.SelectedValue);
        }
        catch
        {
            Scheme_Id = 0;
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
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(0, Zone_Id, Circle_Id, Division_Id, Scheme_Id, 0, 0, false);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(0, Zone_Id, Circle_Id, Division_Id, Scheme_Id, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), false);
        }
        int Total = 0;
        int Pending = 0;
        int Approved = 0;
        int Rejected = 0;
        int De_Escelated = 0;
        if (AllClasses.CheckDataSet(ds))
        {
            Total = ds.Tables[0].Rows.Count;
            int Status_Id = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    Status_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageInvoiceApproval_Status_Id"].ToString());
                }
                catch
                {
                    Status_Id = 0;
                }

                if (Status_Id == 0)
                {
                    Pending++;
                }
                if (Status_Id == 1)
                {
                    Approved++;
                }
                if (Status_Id == 2)
                {
                    De_Escelated++;
                }
                if (Status_Id == 3)
                {
                    Rejected++;
                }
            }

            lnkTotal.Text = Total.ToString();
            lnkPending.Text = Pending.ToString();
            lnkDe_Escelated.Text = De_Escelated.ToString();
            lnkApproved.Text = Approved.ToString();
        }
        else
        {
            lnkTotal.Text = Total.ToString();
            lnkPending.Text = Pending.ToString();
            lnkDe_Escelated.Text = De_Escelated.ToString();
            lnkApproved.Text = Approved.ToString();
        }
    }

    private void get_tbl_PackageInvoice()
    {
        int Scheme_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            Scheme_Id = Convert.ToInt32(ddlScheme.SelectedValue);
        }
        catch
        {
            Scheme_Id = 0;
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
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(0, Zone_Id, Circle_Id, Division_Id, Scheme_Id, 0, 0, false);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageInvoice(0, Zone_Id, Circle_Id, Division_Id, Scheme_Id, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), false);
        }

        if (AllClasses.CheckDataSet(ds))
        {
            btnMark.Visible = false;
            grdInvoice.DataSource = ds.Tables[0];
            grdInvoice.DataBind();

            for (int i = 0; i < grdInvoice.Rows.Count; i++)
            {
                CheckBox chkMark = grdInvoice.Rows[i].FindControl("chkMark") as CheckBox;
                if (chkMark.Visible)
                {
                    btnMark.Visible = true;
                }
            }
        }
        else
        {
            btnMark.Visible = false;
            grdInvoice.DataSource = null;
            grdInvoice.DataBind();
        }
    }

    private void get_tbl_PackageEMB()
    {
        int Scheme_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            Scheme_Id = Convert.ToInt32(ddlScheme.SelectedValue);
        }
        catch
        {
            Scheme_Id = 0;
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
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Scheme_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", 0, 0, 0, "", ChkShowEMB.Checked);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Scheme_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), 0, "", ChkShowEMB.Checked);
        }

        if (AllClasses.CheckDataSet(ds))
        {
            grdEMB.DataSource = ds.Tables[0];
            grdEMB.DataBind();
            //btnMarkMB.Visible = false;
            //btnCombineEMB.Visible = true;
            //for (int i = 0; i < grdEMB.Rows.Count; i++)
            //{
            //    CheckBox chkMarkMB = grdEMB.Rows[i].FindControl("chkMarkMB") as CheckBox;
            //    if (chkMarkMB.Visible)
            //    {
            //        btnMarkMB.Visible = true;
            //    }
            //}
        }
        else
        {
            //btnCombineEMB.Visible = false;
            //btnMarkMB.Visible = false;
            grdEMB.DataSource = null;
            grdEMB.DataBind();
        }
    }

    protected void btnOpenInvoice_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        string Invoice_Id = gr.Cells[0].Text.Trim();
        string Package_Id = gr.Cells[1].Text.Trim();
        string ProcessedBy = gr.Cells[2].Text.Trim().Replace("&nbsp;", "");
        if (ProcessedBy == "")
        {
            if (Session["Invoice_C"] == null)
            {
                Response.Redirect("MasterGenerateInvoice_Detail?Package_Id=" + Package_Id + "&Invoice_Id=" + Invoice_Id);
            }
            else if (Session["Invoice_C"].ToString() == "1")
            {
                //Response.Redirect("MasterGenerateInvoice.aspx?Package_Id="+ Package_Id);
                Response.Redirect("MasterGenerateInvoice_Detail?Package_Id=" + Package_Id + "&Invoice_Id=0");
            }
            else
            {
                MessageBox.Show("Invoice Not Generated. Please Generate First.");
                return;
            }
        }
        else
        {
            Response.Redirect("MasterGenerateInvoice_Detail.aspx?Package_Id=0&Invoice_Id=" + Invoice_Id);
        }
    }

    protected void grdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnCover = e.Row.FindControl("btnCover") as ImageButton;
            ImageButton btnOpenInvoice = e.Row.FindControl("btnOpenInvoice") as ImageButton;
            CheckBox chkMark = e.Row.FindControl("chkMark") as CheckBox;
            int PackageInvoiceCover_Id = 0;
           

            int Designation_Id = 0;
            int Organization_Id = 0;
            int PackageInvoice_Id = 0;
            try
            {
                PackageInvoice_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                PackageInvoice_Id = 0;
            }
            try
            {
                Organization_Id = Convert.ToInt32(e.Row.Cells[3].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                Organization_Id = 0;
            }
            try
            {
                Designation_Id = Convert.ToInt32(e.Row.Cells[4].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                Designation_Id = 0;
            }
            if (Session["UserType"].ToString() == "1")
            {
                btnOpenInvoice.Visible = true;
            }
            else
            {
                if (Session["Person_BranchOffice_Id"].ToString() == Organization_Id.ToString() && Session["PersonJuridiction_DesignationId"].ToString() == Designation_Id.ToString())
                {
                    btnOpenInvoice.Visible = true;
                }
                else
                {
                    btnOpenInvoice.Visible = false;
                }
            }

            if (Session["PersonJuridiction_DesignationId"].ToString() == "4" || Session["PersonJuridiction_DesignationId"].ToString() == "33" || Session["PersonJuridiction_DesignationId"].ToString() == "9")
            {
                try
                {
                    PackageInvoiceCover_Id = Convert.ToInt32(e.Row.Cells[5].Text.Trim().Replace("&nbsp;", ""));
                }
                catch
                {
                    PackageInvoiceCover_Id = 0;
                }
                if (PackageInvoiceCover_Id > 0)
                {
                    btnOpenInvoice.Visible = true;
                }
                else
                {
                    btnOpenInvoice.Visible = false;
                }
                btnCover.Visible = true;
            }
            else
            {
                btnCover.Visible = false;
            }


            Set_Mark_Status(PackageInvoice_Id, chkMark);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        load_dashboard();
    }

    protected void btnMark_Click(object sender, EventArgs e)
    {
        List<int> obj_PackageInvoice_Id_Li = new List<int>();
        for (int i = 0; i < grdInvoice.Rows.Count; i++)
        {
            int PackageInvoice_Id = 0;
            try
            {
                PackageInvoice_Id = Convert.ToInt32(grdInvoice.Rows[i].Cells[0].Text.Trim());
            }
            catch
            {
                PackageInvoice_Id = 0;
            }
            CheckBox chkMark = grdInvoice.Rows[i].FindControl("chkMark") as CheckBox;
            if (chkMark.Visible && chkMark.Checked)
            {
                if (PackageInvoice_Id > 0)
                    obj_PackageInvoice_Id_Li.Add(PackageInvoice_Id);
            }
        }
        if (obj_PackageInvoice_Id_Li.Count == 0)
        {
            MessageBox.Show("Please Select Atleast One Invoice To Mark");
            return;
        }
        else
        {
            List<tbl_PackageInvoiceApproval> obj_tbl_PackageInvoiceApproval_Li = new List<tbl_PackageInvoiceApproval>();
            for (int i = 0; i < obj_PackageInvoice_Id_Li.Count; i++)
            {
                tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = "";
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 4;
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_PackageInvoice_Id_Li[i];
                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
                obj_tbl_PackageInvoiceApproval_Li.Add(obj_tbl_PackageInvoiceApproval);
            }

            if (new DataLayer().Update_tbl_PackageInvoice_Mark(obj_tbl_PackageInvoiceApproval_Li, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString())))
            {
                MessageBox.Show("Invoice Marked Successfully");
                load_dashboard();
                return;
            }
            else
            {
                MessageBox.Show("Unable To Mark Invoice");
                return;
            }
        }
    }

    protected void grdEMB_PreRender(object sender, EventArgs e)
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

    protected void grdEMB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnOpenEMB = e.Row.FindControl("btnOpenEMB") as ImageButton;
            CheckBox chkMarkMB = e.Row.FindControl("chkMarkMB") as CheckBox;
            int Designation_Id = 0;
            int Organization_Id = 0;
            int PackageEMB_Master_Id = 0;
            try
            {
                PackageEMB_Master_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                PackageEMB_Master_Id = 0;
            }
            try
            {
                Organization_Id = Convert.ToInt32(e.Row.Cells[2].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                Organization_Id = 0;
            }
            try
            {
                Designation_Id = Convert.ToInt32(e.Row.Cells[3].Text.Trim().Replace("&nbsp;", ""));
            }
            catch
            {
                Designation_Id = 0;
            }
            if (Session["UserType"].ToString() == "1")
            {
                btnOpenEMB.Visible = true;
            }
            else
            {
                if (Session["Person_BranchOffice_Id"].ToString() == Organization_Id.ToString() && Session["PersonJuridiction_DesignationId"].ToString() == Designation_Id.ToString())
                {
                    btnOpenEMB.Visible = true;
                }
                else
                {
                    btnOpenEMB.Visible = false;
                }
            }

            Set_Mark_Status_MB(PackageEMB_Master_Id, chkMarkMB);
        }
    }
    private void Set_Mark_Status_MB(int PackageEMB_Master_Id, CheckBox chkMark)
    {
        DataSet ds = new DataSet();
        int _Loop = 0;
        if (Session["UserType"].ToString() == "1")
        {
            _Loop = (new DataLayer()).get_Loop("EMB", 0, 0, null, null);
        }
        else
        {
            _Loop = (new DataLayer()).get_Loop("EMB", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), null, null);
        }
        ds = (new DataLayer()).get_ProcessConfig_Current("EMB", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), _Loop, PackageEMB_Master_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int ConfigMaster_Id = 0;
            try
            {
                ConfigMaster_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Id"].ToString());
                if (get_tbl_InvoiceStatus(ConfigMaster_Id, "EMB"))
                {
                    chkMark.Visible = true;
                }
                else
                {
                    chkMark.Visible = false;
                }
            }
            catch
            { }
        }
    }

    //protected void btnMarkMB_Click(object sender, EventArgs e)
    //{
    //    List<int> obj_PackageEMB_Master_Id_Li = new List<int>();
    //    for (int i = 0; i < grdEMB.Rows.Count; i++)
    //    {
    //        int PackageEMB_Master_Id = 0;
    //        try
    //        {
    //            PackageEMB_Master_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
    //        }
    //        catch
    //        {
    //            PackageEMB_Master_Id = 0;
    //        }
    //        CheckBox chkMarkMB = grdEMB.Rows[i].FindControl("chkMarkMB") as CheckBox;
    //        if (chkMarkMB.Visible && chkMarkMB.Checked)
    //        {
    //            if (PackageEMB_Master_Id > 0)
    //                obj_PackageEMB_Master_Id_Li.Add(PackageEMB_Master_Id);
    //        }
    //    }
    //    if (obj_PackageEMB_Master_Id_Li.Count == 0)
    //    {
    //        MessageBox.Show("Please Select Atleast One EMB To Mark");
    //        return;
    //    }
    //    else
    //    {
    //        List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li = new List<tbl_PackageEMBApproval>();
    //        for (int i = 0; i < obj_PackageEMB_Master_Id_Li.Count; i++)
    //        {
    //            tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = new tbl_PackageEMBApproval();
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments = "";
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id = 4;
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_PackageEMB_Master_Id_Li[i];
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Status = 1;
    //            obj_tbl_PackageEMBApproval_Li.Add(obj_tbl_PackageEMBApproval);
    //        }

    //        if (new DataLayer().Update_tbl_PackageEMB_Mark(obj_tbl_PackageEMBApproval_Li, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString())))
    //        {
    //            MessageBox.Show("EMB Marked Successfully");
    //            load_dashboard();
    //            return;
    //        }
    //        else
    //        {
    //            MessageBox.Show("Unable To Mark EMB");
    //            return;
    //        }
    //    }
    //}

    protected void btnOpenEMB_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        string PackageEMB_Master_Id = gr.Cells[0].Text.Trim();
        string Package_Id = gr.Cells[1].Text.Trim();
        Response.Redirect("MasterEMBApprove.aspx?Package_Id=" + Package_Id + "&EMB_Master_Id=" + PackageEMB_Master_Id);
    }

    protected void lnkTotal_Click(object sender, EventArgs e)
    {

    }

    protected void lnkPending_Click(object sender, EventArgs e)
    {

    }

    protected void lnkApproved_Click(object sender, EventArgs e)
    {

    }

    protected void lnkDe_Escelated_Click(object sender, EventArgs e)
    {

    }

    //protected void btnCombineEMB_Click(object sender, EventArgs e)
    //{
    //    List<int> obj_PackageEMB_Master_Id_Li = new List<int>();
    //    string RA_Bill_NO = "";
    //    for (int i = 0; i < grdEMB.Rows.Count; i++)
    //    {
    //        CheckBox chkMarkMB = grdEMB.Rows[i].FindControl("chkMarkMB") as CheckBox;
    //        int PackageEMB_Master_Id = 0;
    //        try
    //        {
    //            PackageEMB_Master_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
    //        }
    //        catch
    //        {
    //            PackageEMB_Master_Id = 0;
    //        }

    //        if (chkMarkMB.Visible && chkMarkMB.Checked)
    //        {
    //            if (RA_Bill_NO == "")
    //            {
    //                RA_Bill_NO = grdEMB.Rows[0].Cells[9].Text.Trim();
    //            }
    //            else
    //            {
    //                if (RA_Bill_NO != grdEMB.Rows[0].Cells[9].Text.Trim())
    //                {
    //                    MessageBox.Show("RA Bill No Always Same.");
    //                    return;
    //                }
    //            }
    //            if (PackageEMB_Master_Id > 0)
    //                obj_PackageEMB_Master_Id_Li.Add(PackageEMB_Master_Id);
    //        }
    //    }

    //    if (obj_PackageEMB_Master_Id_Li.Count == 0)
    //    {
    //        MessageBox.Show("Please Select Atleast One EMB To Mark");
    //        return;
    //    }
    //    else
    //    {
    //        List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li = new List<tbl_PackageEMBApproval>();
    //        for (int i = 0; i < obj_PackageEMB_Master_Id_Li.Count; i++)
    //        {
    //            tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = new tbl_PackageEMBApproval();
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments = "";
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id = 4;
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_PackageEMB_Master_Id_Li[i];
    //            obj_tbl_PackageEMBApproval.PackageEMBApproval_Status = 1;
    //            obj_tbl_PackageEMBApproval_Li.Add(obj_tbl_PackageEMBApproval);
    //        }
    //        if (new DataLayer().Update_CombinedInvoiceGenerate(obj_tbl_PackageEMBApproval_Li, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString())))
    //        {
    //            MessageBox.Show("EMB Marked Successfully");
    //            load_dashboard();
    //            return;
    //        }
    //        else
    //        {
    //            MessageBox.Show("Unable To Mark EMB");
    //            return;
    //        }
    //    }        
    //}

    protected void btnCover_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        string Invoice_Id = gr.Cells[0].Text.Trim();
        string Package_Id = gr.Cells[1].Text.Trim();
        string ProcessedBy = gr.Cells[2].Text.Trim().Replace("&nbsp;", "");
        Response.Redirect("MasterGenerateCoverLetter?Invoice_Id=" + Invoice_Id);
    }

    protected void ChkShowEMB_CheckedChanged(object sender, EventArgs e)
    {
        get_tbl_PackageEMB();
    }
}