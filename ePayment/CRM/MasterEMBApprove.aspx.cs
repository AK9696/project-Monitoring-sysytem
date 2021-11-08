using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class MasterEMBApprove : System.Web.UI.Page
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
            get_tbl_Deduction();
            get_tbl_Deduction1();
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
            int EMB_Master_Id = 0;
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
                    EMB_Master_Id = Convert.ToInt32(Request.QueryString["EMB_Master_Id"].ToString());
                }
                catch
                {
                    EMB_Master_Id = 0;
                }

                get_tbl_PackageEMB_Approve(Package_Id, EMB_Master_Id);
            }
            else
            {
                EMB_Master_Id = 0;
                Package_Id = 0;
            }
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
    private DataTable Filter_Deduction(DataSet ds, string Deduction_Mode)
    {
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "Deduction_Mode='" + Deduction_Mode + "'";
        return dv.ToTable();
    }
    private void get_tbl_Deduction1()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction(0);
        DataTable dt = Filter_Deduction(ds, "-");
        if (AllClasses.CheckDt(dt))
        {
            grdDeductions.DataSource = dt;
            grdDeductions.DataBind();
        }
        else
        {
            grdDeductions.DataSource = null;
            grdDeductions.DataBind();
        }
    }

    private void get_ProcessConfigMaster_Last(int ProcessConfigMaster_Id_Current)
    {
        if (Session["UserType"].ToString() == "1")
        {
            btnGenerateBill.Visible = true;
            DivDeductionDetails.Visible = true;
            btnGenerateCombineInvoice.Visible = false;
            btnUpdate.Visible = true;
        }
        else
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

            ds = (new DataLayer()).get_ProcessConfigMaster_Last("EMB", _Loop, null, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ProcessConfigMaster_Id_Current == Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Id"].ToString()))
                {
                    btnGenerateBill.Visible = true;
                    btnGenerateCombineInvoice.Visible = true;
                    DivDeductionDetails.Visible = true;
                    btnUpdate.Visible = true;
                    btnApproveAll.Visible = false;
                }
                else
                {
                    btnGenerateBill.Visible = false;
                    DivDeductionDetails.Visible = false;
                    btnGenerateCombineInvoice.Visible = false;
                    btnUpdate.Visible = false;
                }
            }
            else
            {
                btnGenerateBill.Visible = false;
                DivDeductionDetails.Visible = false;
                btnGenerateCombineInvoice.Visible = false;
                btnUpdate.Visible = false;
            }
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
        int Package_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());

        int PackageEMB_Master_Id = 0;
        try
        {
            PackageEMB_Master_Id = Convert.ToInt32(gr.Cells[3].Text.Trim());
        }
        catch
        {
            PackageEMB_Master_Id = 0;
        }
        hf_ProjectWork_Id.Value = Package_Id.ToString() + "|" + PackageEMB_Master_Id.ToString();
        gr.BackColor = Color.LightGreen;
        get_tbl_Deduction();
        get_tbl_PackageEMB(Package_Id, PackageEMB_Master_Id);
        get_PackageEMB_ExtraItemApprove(PackageEMB_Master_Id);
    }

    private void get_tbl_PackageEMB(int Package_Id, int PackageEMB_Master_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageEMB(Package_Id, PackageEMB_Master_Id, "A", false);

        if (AllClasses.CheckDataSet(ds))
        {
            grdEMB.DataSource = ds.Tables[0];
            grdEMB.DataBind();

            if (ds.Tables[0].Rows[0]["PackageEMB_Master_Date"].ToString().Trim() != "")
            {
                txtMBDate.Text = ds.Tables[0].Rows[0]["PackageEMB_Master_Date"].ToString().Trim();
            }
            txtMB_No.Text = ds.Tables[0].Rows[0]["PackageEMB_Master_VoucherNo"].ToString().Trim();
            txtRABillNo.Text = ds.Tables[0].Rows[0]["PackageEMB_Master_RA_BillNo"].ToString().Trim();
            hf_PackageEMB_Master_Id.Value = ds.Tables[0].Rows[0]["PackageEMB_PackageEMB_Master_Id"].ToString().Trim();

        }
        else
        {
            MessageBox.Show("EMB Details Not Found For Approval...!!");
            return;
        }

        get_ProcessConfig_Current(PackageEMB_Master_Id);
    }
    private void get_PackageEMB_ExtraItemApprove(int PackageEMB_Master_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_PackageEMB_ExtraItemApprove(PackageEMB_Master_Id);

        if (AllClasses.CheckDataSet(ds))
        {
            grdExtraItemApprove.DataSource = ds.Tables[0];
            grdExtraItemApprove.DataBind();

        }
        else
        {
            grdExtraItemApprove.DataSource = null;
            grdExtraItemApprove.DataBind();
        }

    }
    protected void grdExtraItemApprove_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDownload1 = (e.Row.FindControl("lnkAgreementFile2") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload1);

        }
    }
    protected void lnkAgreementFile2_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[0].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                //Response.ClearContent();
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                //Response.AddHeader("Content-Length", fi.Length.ToString());
                //string CId = Request["__EVENTTARGET"];
                //Response.TransmitFile(fi.FullName);
                //Response.End();
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
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

        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            return;
        }
        int Package_Id = 0;
        int EMB_Master_Id = 0;
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
                EMB_Master_Id = Convert.ToInt32(Request.QueryString["EMB_Master_Id"].ToString());
            }
            catch
            {
                EMB_Master_Id = 0;
            }
        }
        else
        {
            EMB_Master_Id = 0;
            Package_Id = 0;
        }
        if (ddlSearchScheme.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            ddlSearchScheme.Focus();
            return;
        }
        get_tbl_PackageEMB_Approve(Package_Id, EMB_Master_Id);
    }

    private void get_tbl_PackageEMB_Approve(int Package_Id, int EMB_Master_Id)
    {
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
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, Package_Id, "", "", 0, 0, EMB_Master_Id, "", false);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, Package_Id, "", "", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), EMB_Master_Id, "", false);
        }

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
            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(e.Row.Cells[4].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }

            int InvoiceItem_Id = 0;
            try
            {
                InvoiceItem_Id = Convert.ToInt32(e.Row.Cells[20].Text.Trim());
            }
            catch
            {
                InvoiceItem_Id = 0;
            }

            DropDownList ddlUnit = e.Row.FindControl("ddlUnit") as DropDownList;
            int Unit_Id = 0;
            try
            {
                Unit_Id = Convert.ToInt32(e.Row.Cells[3].Text.Trim());
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
            int Unit_Length_Applicable = 0;
            int Unit_Bredth_Applicable = 0;
            int Unit_Height_Applicable = 0;
            try
            {
                Unit_Length_Applicable = Convert.ToInt32(e.Row.Cells[22].Text.Trim());
            }
            catch
            {
                Unit_Length_Applicable = 0;
            }
            try
            {
                Unit_Bredth_Applicable = Convert.ToInt32(e.Row.Cells[23].Text.Trim());
            }
            catch
            {
                Unit_Bredth_Applicable = 0;
            }
            try
            {
                Unit_Height_Applicable = Convert.ToInt32(e.Row.Cells[24].Text.Trim());
            }
            catch
            {
                Unit_Height_Applicable = 0;
            }
            TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
            TextBox txtLength = e.Row.FindControl("txtLength") as TextBox;
            TextBox txtBreadth = e.Row.FindControl("txtBreadth") as TextBox;
            TextBox txtHeight = e.Row.FindControl("txtHeight") as TextBox;
            TextBox txtContents = e.Row.FindControl("txtContents") as TextBox;

            Label lblSpecification = e.Row.FindControl("lblSpecification") as Label;

            lblSpecification.Text = lblSpecification.Text.Replace("\n", "<br />");

            Button btnApprove = e.Row.FindControl("btnApprove") as Button;
            System.Web.UI.WebControls.Image imgBilling = e.Row.FindControl("imgBilling") as System.Web.UI.WebControls.Image;

            if (InvoiceItem_Id > 0)
            {
                imgBilling.ImageUrl = "~/assets/images/OK.png";
            }
            else
            {
                imgBilling.ImageUrl = "~/assets/images/Not_OK.png";
            }
            if (is_Approved > 0)
            {
                txtLength.Enabled = false;
                txtQty.Enabled = false;
                txtBreadth.Enabled = false;
                txtHeight.Enabled = false;
                txtContents.Enabled = false;
                ddlUnit.Enabled = false;

                btnApprove.Visible = false;
            }

            if (Unit_Length_Applicable == 0)
            {
                txtLength.Enabled = false;
            }
            else
            {
                txtLength.Enabled = true;
            }
            if (Unit_Bredth_Applicable == 0)
            {
                txtBreadth.Enabled = false;
            }
            else
            {
                txtBreadth.Enabled = true;
            }
            if (Unit_Height_Applicable == 0)
            {
                txtHeight.Enabled = false;
            }
            else
            {
                txtHeight.Enabled = true;
            }
            int Package_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
            GridView grdTaxes = e.Row.FindControl("grdTaxes") as GridView;
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction_WithPackageTax(Package_Id);
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

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        GridViewRow gr = (sender as Button).Parent.Parent as GridViewRow;
        TextBox txtQty = gr.FindControl("txtQty") as TextBox;
        List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li = new List<tbl_PackageEMB_Approval>();
        tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval = new tbl_PackageEMB_Approval();
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty = Convert.ToDecimal(txtQty.Text);
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Comments = "";
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_No = "";
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Status = 1;
        obj_tbl_PackageEMB_Approval.PackageEMB_DocumentPath = "";
        obj_tbl_PackageEMB_Approval_Li.Add(obj_tbl_PackageEMB_Approval);

        if (new DataLayer().Insert_tbl_PackageEMB_Approval(obj_tbl_PackageEMB_Approval_Li, null))
        {
            MessageBox.Show("EMB Details Approved Successfully");
            string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            get_tbl_PackageEMB(Convert.ToInt32(_data[0]), Convert.ToInt32(_data[1]));
            return;
        }
        else
        {
            MessageBox.Show("Error In EMB Details Approval");
            return;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue == "" || ddlStatus.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Status");
            ddlStatus.Focus();
            return;
        }

        string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        bool flagExtraItem = false;
        decimal QtyExtra = 0;
        tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
        obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(_data[0]);
        obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
        obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = txtRABillNo.Text.Trim();
        List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
        List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li = new List<tbl_PackageEMB_Approval>();
        List<tbl_PackageEMB> obj_tbl_PackageEMB_Li = new List<tbl_PackageEMB>();
        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            Label lblQty = (grdEMB.Rows[i].FindControl("lblQty") as Label);
            Label lblQtyMax = (grdEMB.Rows[i].FindControl("lblQtyMax") as Label);
            decimal Total_Qty = 0;
            try
            {
                Total_Qty = Convert.ToDecimal(lblQty.Text.Trim());
            }
            catch
            {
                Total_Qty = 0;
            }
            decimal Total_Qty_Paid = 0;
            try
            {
                Total_Qty_Paid = Convert.ToDecimal(lblQtyMax.Text.Trim());
            }
            catch
            {
                Total_Qty_Paid = 0;
            }



            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(grdEMB.Rows[i].Cells[4].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }
            int is_Billed = 0;
            try
            {
                is_Billed = Convert.ToInt32(grdEMB.Rows[i].Cells[7].Text.Trim());
            }
            catch
            {
                is_Billed = 0;
            }
            if (is_Approved == 0)
            {
                TextBox txtQty = grdEMB.Rows[i].FindControl("txtQty") as TextBox;
                tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval = new tbl_PackageEMB_Approval();
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty = Convert.ToDecimal(grdEMB.Rows[i].Cells[28].Text.Trim());
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Comments = "";
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_No = "";
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Status = 1;
                obj_tbl_PackageEMB_Approval.PackageEMB_DocumentPath = "";
                obj_tbl_PackageEMB_Approval_Li.Add(obj_tbl_PackageEMB_Approval);

                tbl_PackageEMB obj_tbl_PackageEMB = new tbl_PackageEMB();
                obj_tbl_PackageEMB.PackageEMB_Qty = Convert.ToDecimal(txtQty.Text.Trim());
                obj_tbl_PackageEMB.PackageEMB_QtyExtra = Total_Qty - (obj_tbl_PackageEMB.PackageEMB_Qty + Total_Qty_Paid);
                if (obj_tbl_PackageEMB.PackageEMB_QtyExtra > 0)
                {
                    obj_tbl_PackageEMB.PackageEMB_QtyExtra = 0;
                }
                obj_tbl_PackageEMB.PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageEMB_Li.Add(obj_tbl_PackageEMB);
            }

            if (is_Billed == 0)
            {
                tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_GSTType = grdEMB.Rows[i].Cells[29].Text.Replace("&nbsp;", "").Trim();
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ = Convert.ToDecimal((grdEMB.Rows[i].FindControl("txtQty") as TextBox).Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty = Convert.ToDecimal((grdEMB.Rows[i].FindControl("lblQty") as Label).Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = Total_Qty - (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ + Total_Qty_Paid);
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageBOQ_OrderNo = Convert.ToInt32(grdEMB.Rows[i].Cells[31].Text.Trim());
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageBOQ_OrderNo = 0;
                }
                if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra > 0)
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = 0;
                }
                if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra < 0)
                {
                    flagExtraItem = true;
                }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = Convert.ToDecimal((grdEMB.Rows[i].FindControl("txtPercentageToBeReleased") as TextBox).Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated = Convert.ToDecimal(grdEMB.Rows[i].Cells[13].Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted = Convert.ToDecimal(grdEMB.Rows[i].Cells[14].Text.Trim());
                }
                catch
                { }
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status = 1;
                List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
                GridView grdTaxes = grdEMB.Rows[i].FindControl("grdTaxes") as GridView;
                for (int k = 0; k < grdTaxes.Rows.Count; k++)
                {
                    tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                    TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                    DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
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
                        obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                    }
                    if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                    }
                }
                obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);
            }
        }
        int ProjectWorkPkg_Id = 0;
        try
        {
            ProjectWorkPkg_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            ProjectWorkPkg_Id = 0;
        }

        tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = null;
        if (obj_tbl_PackageInvoiceItem_Li.Count > 0)
        {
            obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = txtContentsMaster.Text.Trim();
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = ProjectWorkPkg_Id;
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0;
        }

        tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = new tbl_PackageEMBApproval();
        obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments = txtContentsMaster.Text.Trim();
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
        try
        {
            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        }
        catch
        {
            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = 0;
        }
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id = Convert.ToInt32(_data[0]);
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status = 1;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id = 1;


        List<tbl_PackageInvoiceEMBMasterLink> obj_tbl_PackageInvoiceEMBMasterLink_Li = new List<tbl_PackageInvoiceEMBMasterLink>();
        tbl_PackageInvoiceEMBMasterLink obj_tbl_PackageInvoiceEMBMasterLink = new tbl_PackageInvoiceEMBMasterLink();
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_EMBMaster_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status = 1;
        obj_tbl_PackageInvoiceEMBMasterLink_Li.Add(obj_tbl_PackageInvoiceEMBMasterLink);

        List<tbl_PackageEMB_Tax> obj_tbl_PackageEMB_Tax_Li = new List<tbl_PackageEMB_Tax>();
        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            GridView grdTaxes = grdEMB.Rows[i].FindControl("grdTaxes") as GridView;
            for (int k = 0; k < grdTaxes.Rows.Count; k++)
            {
                tbl_PackageEMB_Tax obj_tbl_PackageEMB_Tax = new tbl_PackageEMB_Tax();
                TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Package_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                try
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
                }
                catch
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id = 0;
                }
                try
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
                }
                catch
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Value = 0;
                }
                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Status = 1;

                if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id > 0)
                {
                    for (int j = 0; j < obj_tbl_PackageEMB_Tax_Li.Count; j++)
                    {
                        if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Package_Id == obj_tbl_PackageEMB_Tax_Li[j].PackageEMB_Tax_Package_Id && obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id == obj_tbl_PackageEMB_Tax_Li[j].PackageEMB_Tax_Deduction_Id)
                        {
                            MessageBox.Show("Deduction Already Addded. ");
                            return;
                        }
                    }
                }
                if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id > 0)
                {
                    obj_tbl_PackageEMB_Tax_Li.Add(obj_tbl_PackageEMB_Tax);
                }
            }
        }

        tbl_PackageEMB_Master obj_tbl_PackageEMB_Master = new tbl_PackageEMB_Master();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Date = txtMBDate.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo = txtMB_No.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo = txtRABillNo.Text.Trim();

        List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li = new List<tbl_PackageInvoiceAdditional>();

        for (int i = 0; i < grdDeductions.Rows.Count; i++)
        {
            CheckBox chkSelect = grdDeductions.Rows[i].FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked == true)
            {
                TextBox txtDeductionValue = grdDeductions.Rows[i].FindControl("txtDeductionValue") as TextBox;
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                RadioButtonList rblDeductionType = grdDeductions.Rows[i].FindControl("rblDeductionType") as RadioButtonList;

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
                if (rblDeductionType.SelectedValue == "Flat")
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = "";
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }

        tbl_PackageEMB_ExtraItem obj_tbl_PackageEMB_ExtraItem = new tbl_PackageEMB_ExtraItem();
        //if (flagExtraItem == true && grdExtraItemApprove.Rows.Count == 0)
        //{
        if (txtApprovalNo.Text.Trim() != "")
        {
            //if (txtApprovalNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve No");
            //    txtApprovalNo.Focus();
            //    return;
            //}
            //if (txtApprovalDate.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve Date");
            //    txtApprovalDate.Focus();
            //    return;
            //}

            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveNo = txtApprovalNo.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveDate = txtApprovalDate.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Comment = txtContentsMaster.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Status = 1;
            try
            {
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = (Byte[])Session["FileBytes"];
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = Session["FileName"].ToString();
            }
            catch
            {

                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = null;
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = "";
            }
            //if (Session["FileBytes"] != null)
            //{
            //    obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = (Byte[])Session["FileBytes"];
            //    obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = Session["FileName"].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Please Approval File Uploaded");
            //    return;
            //}
        }
        else
        {
            obj_tbl_PackageEMB_ExtraItem = null;
        }

        //if (obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id == 0)
        //{
        //    MessageBox.Show("Please Select Deduction.");
        //    ddlDeduction.Focus();
        //    return;
        //}

        if (new DataLayer().Update_tbl_EMB_Set_Billing(obj_tbl_PackageEMB_Approval_Li, obj_tbl_PackageInvoice, obj_tbl_PackageInvoiceItem_Li, ProjectWorkPkg_Id, Convert.ToInt32(Session["Person_Id"].ToString()), obj_tbl_PackageInvoiceApproval, obj_tbl_PackageEMBApproval, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), obj_tbl_PackageEMB_Tax_Li, obj_tbl_PackageEMB_Li, obj_tbl_PackageEMB_Master, obj_tbl_PackageInvoiceAdditional_Li, obj_tbl_PackageInvoiceEMBMasterLink_Li, null, obj_tbl_PackageEMB_ExtraItem))
        {
            MessageBox.Show("Package EMB Marked For Billing");
            Approvereset();
            get_tbl_PackageEMB(Convert.ToInt32(_data[0]), Convert.ToInt32(_data[1]));
            return;
        }
        else
        {
            MessageBox.Show("Error In  Package EMB Billing Process.");
            return;
        }
    }

    protected void btnGenerateBill_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue == "" || ddlStatus.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Status");
            ddlStatus.Focus();
            return;
        }

        string[] _data = hf_PackageEMB_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        bool flagExtraItem = false;
        decimal QtyExtra = 0;
        tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
        obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoice.PackageInvoice_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(_data[0]);
        obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
        obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = txtRABillNo.Text.Trim();

        int is_Billed = 0;
        List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            Label lblQty = (grdEMB.Rows[i].FindControl("lblQty") as Label);
            Label lblQtyMax = (grdEMB.Rows[i].FindControl("lblQtyMax") as Label);
            Label lblQtyExtra = grdEMB.Rows[i].FindControl("lblQtyExtra") as Label;

            decimal Total_Qty = 0;
            try
            {
                Total_Qty = Convert.ToDecimal(lblQty.Text.Trim());
            }
            catch
            {
                Total_Qty = 0;
            }
            decimal Total_Qty_Paid = 0;
            try
            {
                Total_Qty_Paid = Convert.ToDecimal(lblQtyMax.Text.Trim());
            }
            catch
            {
                Total_Qty_Paid = 0;
            }



            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(grdEMB.Rows[i].Cells[4].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }

            try
            {
                is_Billed = Convert.ToInt32(grdEMB.Rows[i].Cells[7].Text.Trim());
            }
            catch
            {
                is_Billed = 0;
            }
            if (is_Billed == 0 && is_Approved > 0)
            {
                tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[1].Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_GSTType = grdEMB.Rows[i].Cells[29].Text.Replace("&nbsp;", "").Trim();
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ = Convert.ToDecimal((grdEMB.Rows[i].FindControl("txtQty") as TextBox).Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty = Convert.ToDecimal((grdEMB.Rows[i].FindControl("lblQty") as Label).Text.Trim());
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = Total_Qty - (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ + Total_Qty_Paid);
                if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra > 0)
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = 0;
                }
                if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra < 0)
                {
                    flagExtraItem = true;
                }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageBOQ_OrderNo = Convert.ToInt32(grdEMB.Rows[i].Cells[31].Text.Trim());
                }
                catch
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageBOQ_OrderNo = 0;
                }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = Convert.ToDecimal((grdEMB.Rows[i].FindControl("txtPercentageToBeReleased") as TextBox).Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated = Convert.ToDecimal(grdEMB.Rows[i].Cells[13].Text.Trim());
                }
                catch
                { }
                try
                {
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted = Convert.ToDecimal(grdEMB.Rows[i].Cells[14].Text.Trim());
                }
                catch
                { }
                obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status = 1;
                List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
                GridView grdTaxes = grdEMB.Rows[i].FindControl("grdTaxes") as GridView;
                for (int k = 0; k < grdTaxes.Rows.Count; k++)
                {
                    tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                    TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                    DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                    obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
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
                        obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                    }
                    if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                    }
                }
                obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);
            }
        }

        int ProjectWorkPkg_Id = 0;
        try
        {
            ProjectWorkPkg_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            ProjectWorkPkg_Id = 0;
        }
        if (obj_tbl_PackageInvoiceItem_Li.Count == 0)
        {
            MessageBox.Show("No Items Elegible For Billing..");
            return;
        }
        tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = null;
        if (obj_tbl_PackageInvoiceItem_Li.Count > 0)
        {
            obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = txtContentsMaster.Text.Trim();
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = ProjectWorkPkg_Id;
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0;
        }

        tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = new tbl_PackageEMBApproval();
        obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments = txtContentsMaster.Text.Trim();
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
        try
        {
            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        }
        catch
        {
            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = 0;
        }
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id = Convert.ToInt32(_data[0]);
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status = 1;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id = 1;

        List<tbl_PackageInvoiceEMBMasterLink> obj_tbl_PackageInvoiceEMBMasterLink_Li = new List<tbl_PackageInvoiceEMBMasterLink>();
        tbl_PackageInvoiceEMBMasterLink obj_tbl_PackageInvoiceEMBMasterLink = new tbl_PackageInvoiceEMBMasterLink();
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_EMBMaster_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status = 1;
        obj_tbl_PackageInvoiceEMBMasterLink_Li.Add(obj_tbl_PackageInvoiceEMBMasterLink);


        tbl_PackageEMB_Master obj_tbl_PackageEMB_Master = new tbl_PackageEMB_Master();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Date = txtMBDate.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo = txtMB_No.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo = txtRABillNo.Text.Trim();

        List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li = new List<tbl_PackageInvoiceAdditional>();

        for (int i = 0; i < grdDeductions.Rows.Count; i++)
        {
            CheckBox chkSelect = grdDeductions.Rows[i].FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked == true)
            {
                TextBox txtDeductionValue = grdDeductions.Rows[i].FindControl("txtDeductionValue") as TextBox;
                tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional = new tbl_PackageInvoiceAdditional();
                RadioButtonList rblDeductionType = grdDeductions.Rows[i].FindControl("rblDeductionType") as RadioButtonList;

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
                if (rblDeductionType.SelectedValue == "Flat")
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "1";
                }
                else
                {
                    obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat = "0";
                }
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments = "";
                obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status = 1;
                obj_tbl_PackageInvoiceAdditional_Li.Add(obj_tbl_PackageInvoiceAdditional);
            }
        }


        tbl_PackageEMB_ExtraItem obj_tbl_PackageEMB_ExtraItem = new tbl_PackageEMB_ExtraItem();
        //if (flagExtraItem == true && grdExtraItemApprove.Rows.Count == 0)
        //{
        if (txtApprovalNo.Text.Trim() != "")
        {
            //if (txtApprovalNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve No");
            //    txtApprovalNo.Focus();
            //    return;
            //}
            //if (txtApprovalDate.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve Date");
            //    txtApprovalDate.Focus();
            //    return;
            //}

            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveNo = txtApprovalNo.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveDate = txtApprovalDate.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Comment = txtContentsMaster.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Status = 1;
            try
            {
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = (Byte[])Session["FileBytes"];
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = Session["FileName"].ToString();
            }
            catch
            {

                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = null;
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = "";
            }
            //if (Session["FileBytes"] != null)
            //{
            //    obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = (Byte[])Session["FileBytes"];
            //    obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = Session["FileName"].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Please Approval File Uploaded");
            //    return;
            //}
        }
        else
        {
            obj_tbl_PackageEMB_ExtraItem = null;
        }

        if (new DataLayer().Update_tbl_EMB_Set_Billing(null, obj_tbl_PackageInvoice, obj_tbl_PackageInvoiceItem_Li, ProjectWorkPkg_Id, Convert.ToInt32(Session["Person_Id"].ToString()), obj_tbl_PackageInvoiceApproval, obj_tbl_PackageEMBApproval, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), null, null, obj_tbl_PackageEMB_Master, obj_tbl_PackageInvoiceAdditional_Li, obj_tbl_PackageInvoiceEMBMasterLink_Li, null, obj_tbl_PackageEMB_ExtraItem))
        {
            MessageBox.Show("Package EMB Marked For Billing");
            get_tbl_PackageEMB(Convert.ToInt32(_data[0]), Convert.ToInt32(_data[1]));
            Approvereset();
            return;
        }
        else
        {
            MessageBox.Show("Error In  Package EMB Billing Process.");
            return;
        }
    }

    protected void btnApproveAll_Click(object sender, EventArgs e)
    {

        if (ddlStatus.SelectedValue == "" || ddlStatus.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Status");
            ddlStatus.Focus();
            return;
        }

        int is_Billed = 0;
        bool flagExtraItem = false;
        decimal QtyExtra = 0;
        string[] _data = hf_PackageEMB_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li = new List<tbl_PackageEMB_Approval>();
        List<tbl_PackageEMB> obj_tbl_PackageEMB_Li = new List<tbl_PackageEMB>();
        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            int is_Approved = 0;
            try
            {
                is_Approved = Convert.ToInt32(grdEMB.Rows[i].Cells[4].Text.Trim());
            }
            catch
            {
                is_Approved = 0;
            }

            try
            {
                is_Billed = Convert.ToInt32(grdEMB.Rows[i].Cells[7].Text.Trim());
            }
            catch
            {
                is_Billed = 0;
            }
            //if (is_Approved == 0)
            //{
            TextBox txtQty = grdEMB.Rows[i].FindControl("txtQty") as TextBox;
            Label lblQty = (grdEMB.Rows[i].FindControl("lblQty") as Label);
            Label lblQtyMax = (grdEMB.Rows[i].FindControl("lblQtyMax") as Label);
            Label lblQtyExtra = grdEMB.Rows[i].FindControl("lblQtyExtra") as Label;


            decimal Total_Qty = 0;
            try
            {
                Total_Qty = Convert.ToDecimal(lblQty.Text.Trim());
            }
            catch
            {
                Total_Qty = 0;
            }
            decimal Total_Qty_Paid = 0;
            try
            {
                Total_Qty_Paid = Convert.ToDecimal(lblQtyMax.Text.Trim());
            }
            catch
            {
                Total_Qty_Paid = 0;
            }
            tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval = new tbl_PackageEMB_Approval();
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty = Convert.ToDecimal(grdEMB.Rows[i].Cells[28].Text.Trim());
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Comments = "";
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_No = "";
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Status = 1;
            obj_tbl_PackageEMB_Approval.PackageEMB_DocumentPath = "";
            obj_tbl_PackageEMB_Approval_Li.Add(obj_tbl_PackageEMB_Approval);

            tbl_PackageEMB obj_tbl_PackageEMB = new tbl_PackageEMB();
            obj_tbl_PackageEMB.PackageEMB_Qty = Convert.ToDecimal(txtQty.Text.Trim());
            obj_tbl_PackageEMB.PackageEMB_QtyExtra = Total_Qty - (obj_tbl_PackageEMB.PackageEMB_Qty + Total_Qty_Paid);
            if (obj_tbl_PackageEMB.PackageEMB_QtyExtra > 0)
            {
                obj_tbl_PackageEMB.PackageEMB_QtyExtra = 0;
            }
            obj_tbl_PackageEMB.PackageEMB_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
            obj_tbl_PackageEMB_Li.Add(obj_tbl_PackageEMB);
            //  }

            if (obj_tbl_PackageEMB.PackageEMB_QtyExtra < 0)
            {
                flagExtraItem = true;
            }
        }
        int ProjectWorkPkg_Id = 0;
        try
        {
            ProjectWorkPkg_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            ProjectWorkPkg_Id = 0;
        }

        //tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
        //obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        //obj_tbl_PackageInvoice.PackageInvoice_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        //obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(_data[0]);
        //obj_tbl_PackageInvoice.PackageInvoice_Status = 1;

        //List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
        //for (int i = 0; i < grdEMB.Rows.Count; i++)
        //{
        //    try
        //    {
        //        is_Billed = Convert.ToInt32(grdEMB.Rows[i].Cells[7].Text.Trim());
        //    }
        //    catch
        //    {
        //        is_Billed = 0;
        //    }
        //    if (is_Billed == 0)
        //    {
        //        tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
        //        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        //        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
        //        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ = Convert.ToDecimal((grdEMB.Rows[i].FindControl("txtQty") as TextBox).Text.Trim());
        //        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty = Convert.ToDecimal((grdEMB.Rows[i].FindControl("lblQty") as Label).Text.Trim());
        //        try
        //        {
        //            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated = Convert.ToDecimal(grdEMB.Rows[i].Cells[13].Text.Trim());
        //        }
        //        catch
        //        { }
        //        try
        //        {
        //            obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted = Convert.ToDecimal(grdEMB.Rows[i].Cells[14].Text.Trim());
        //        }
        //        catch
        //        { }
        //        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status = 1;
        //        List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
        //        GridView grdTaxes = grdEMB.Rows[i].FindControl("grdTaxes") as GridView;
        //        for (int k = 0; k < grdTaxes.Rows.Count; k++)
        //        {
        //            tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
        //            TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
        //            DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

        //            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        //            try
        //            {
        //                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
        //            }
        //            catch
        //            {
        //                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = 0;
        //            }
        //            try
        //            {
        //                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
        //            }
        //            catch
        //            {
        //                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = 0;
        //            }
        //            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status = 1;

        //            if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
        //            {
        //                obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
        //            }
        //            if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
        //            {
        //                obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
        //            }
        //        }

        //        obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);

        //    }
        //}

        //if (obj_tbl_PackageInvoiceItem_Li.Count == 0)
        //{
        //    MessageBox.Show("No Items Elegible For Billing..");
        //    return;
        //}
        //tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = null;
        //if (obj_tbl_PackageInvoiceItem_Li.Count > 0)
        //{
        //    obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments = txtContentsMaster.Text.Trim();
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = ProjectWorkPkg_Id;
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
        //    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0;
        //}

        tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = new tbl_PackageEMBApproval();
        obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments = "";
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id = ProjectWorkPkg_Id;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status = 1;
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id = Convert.ToInt32(ddlStatus.SelectedValue);
        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 1;

        List<tbl_PackageEMB_Tax> obj_tbl_PackageEMB_Tax_Li = new List<tbl_PackageEMB_Tax>();

        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            GridView grdTaxes = grdEMB.Rows[i].FindControl("grdTaxes") as GridView;
            for (int k = 0; k < grdTaxes.Rows.Count; k++)
            {
                tbl_PackageEMB_Tax obj_tbl_PackageEMB_Tax = new tbl_PackageEMB_Tax();

                TextBox txtTaxesP = grdTaxes.Rows[k].FindControl("txtTaxesP") as TextBox;
                DropDownList ddlTaxes = grdTaxes.Rows[k].FindControl("ddlTaxes") as DropDownList;

                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Package_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text.Trim());
                try
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id = Convert.ToInt32(ddlTaxes.SelectedValue);
                }
                catch
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id = 0;
                }
                try
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Value = Convert.ToDecimal(txtTaxesP.Text);
                }
                catch
                {
                    obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Value = 0;
                }
                obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Status = 1;


                if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id > 0)
                {
                    for (int j = 0; j < obj_tbl_PackageEMB_Tax_Li.Count; j++)
                    {
                        if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Package_Id == obj_tbl_PackageEMB_Tax_Li[j].PackageEMB_Tax_Package_Id && obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id == obj_tbl_PackageEMB_Tax_Li[j].PackageEMB_Tax_Deduction_Id)
                        {
                            MessageBox.Show("Deduction Already Addded. ");
                            return;
                        }
                    }
                }
                if (obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id > 0)
                {
                    obj_tbl_PackageEMB_Tax_Li.Add(obj_tbl_PackageEMB_Tax);

                }

            }
        }

        tbl_PackageEMB_Master obj_tbl_PackageEMB_Master = new tbl_PackageEMB_Master();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Date = txtMBDate.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
        obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo = txtMB_No.Text.Trim();
        obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo = txtRABillNo.Text.Trim();

        tbl_PackageEMB_ExtraItem obj_tbl_PackageEMB_ExtraItem = new tbl_PackageEMB_ExtraItem();
        //if (flagExtraItem == true && grdExtraItemApprove.Rows.Count==0)
        //{
        if (txtApprovalNo.Text.Trim() != "")
        {
            //    if (txtApprovalNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve No");
            //    txtApprovalNo.Focus();
            //    return;
            //}
            //if (txtApprovalDate.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Provide Approve Date");
            //    txtApprovalDate.Focus();
            //    return;
            //}

            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_PackageEMB_Master_Id = Convert.ToInt32(hf_PackageEMB_Master_Id.Value);
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveNo = txtApprovalNo.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveDate = txtApprovalDate.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Comment = txtContentsMaster.Text;
            obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Status = 1;
            //if (Session["FileBytes"] != null)
            //{
            try
            {
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = (Byte[])Session["FileBytes"];
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = Session["FileName"].ToString();
            }
            catch
            {
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes = null;
                obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention = "";
            }
            //}
            //else
            //{
            //    MessageBox.Show("Please Approval File Uploaded");
            //    return;
            //}
        }
        else
        {
            obj_tbl_PackageEMB_ExtraItem = null;
        }

        if (new DataLayer().Update_tbl_EMB_Set_Approval(obj_tbl_PackageEMB_Approval_Li, obj_tbl_PackageEMBApproval, null, null, Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), obj_tbl_PackageEMB_Tax_Li, obj_tbl_PackageEMB_Li, obj_tbl_PackageEMB_Master, obj_tbl_PackageEMB_ExtraItem))
        {
            MessageBox.Show("Package EMB Marked Approved");
            Approvereset();
            get_tbl_PackageEMB(ProjectWorkPkg_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id);
            return;
        }
        else
        {
            MessageBox.Show("Error In  Package EMB Approval Process.");
            return;
        }
    }
    public void Approvereset()
    {
        Session["FileBytes"] = null;
        Session["FileName"] = null;
        txtApprovalNo.Text = "";
        txtApprovalDate.Text = "";
        txtContentsMaster.Text = "";
    }

    private void get_tbl_InvoiceStatus(int ConfigMasterId)
    {
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(0, 0, 0, "EMB");
        }
        else
        {
            ds = (new DataLayer()).get_tbl_InvoiceStatus(Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), ConfigMasterId, "EMB");
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

    private void get_ProcessConfig_Current(int PackageEMB_Master_Id)
    {
        if (Session["UserType"].ToString() == "1")
        {
            btnApproveAll.Visible = true;
            get_tbl_InvoiceStatus(0);
        }
        else
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
                    get_tbl_InvoiceStatus(ConfigMaster_Id);

                    get_ProcessConfigMaster_Last(ConfigMaster_Id);
                }
                catch
                {
                    ConfigMaster_Id = 0;
                    get_tbl_InvoiceStatus(ConfigMaster_Id);
                }
                btnApproveAll.Visible = true;
            }
            else
            {
                btnApproveAll.Visible = false;
            }
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
            TextBox txtTaxesP = e.Row.FindControl("txtTaxesP") as TextBox;
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
            GridViewRow gr = e.Row.Parent.Parent.Parent.Parent as GridViewRow;
            int GSTPercenatge = 0;
            try
            {
                GSTPercenatge = Convert.ToInt16(gr.Cells[30].Text);
            }
            catch
            {
                GSTPercenatge = 0;
            }
            try
            {
                ddlTaxes.SelectedValue = e.Row.Cells[1].Text;
            }
            catch
            {

            }
            if (txtTaxesP.Text == "")
            {

                if (e.Row.RowIndex == 0)
                {
                    if (ddlTaxes.SelectedValue == "0")
                    {
                        try
                        {
                            ddlTaxes.SelectedValue = "1013";
                        }
                        catch
                        {
                            ddlTaxes.SelectedValue = "0";
                        }
                    }
                    if (GSTPercenatge > 0)
                    {
                        txtTaxesP.Text = (GSTPercenatge / 2).ToString();
                    }
                    else
                    {
                        txtTaxesP.Text = "0";
                    }

                }
                if (e.Row.RowIndex == 1)
                {
                    try
                    {
                        ddlTaxes.SelectedValue = "1014";
                    }
                    catch
                    {
                        ddlTaxes.SelectedValue = "0";
                    }
                    if (GSTPercenatge > 0)
                    {
                        txtTaxesP.Text = (GSTPercenatge / 2).ToString();
                    }
                    else
                    {
                        txtTaxesP.Text = "0";
                    }
                }
            }
        }
    }

    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gr = (sender as Button).Parent.Parent as GridViewRow;
            int PackageEMB_Id = Convert.ToInt32(gr.Cells[0].Text);
            int person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
            if (new DataLayer().Delete_tbl_PackageEMB(PackageEMB_Id, person_Id, "Rejected"))
            {
                MessageBox.Show("EMB Details Rejected Successfully");
                string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                get_tbl_PackageEMB(Convert.ToInt32(_data[0]), Convert.ToInt32(_data[1]));
                return;
            }
            else
            {
                MessageBox.Show("Error In EMB Details Rejected");
                return;
            }
        }
        catch
        {

        }

    }

    protected void btnGenerateCombineInvoice_Click(object sender, EventArgs e)
    {
        if (txtRABillNo.Text != "")
        {
            Response.Redirect("MasterCombineInvoice?RABillNo=" + txtRABillNo.Text);
        }
        else
        {
            MessageBox.Show("Please Enter RA Bill No.");
            return;
        }
    }
}