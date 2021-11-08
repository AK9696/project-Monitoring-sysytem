using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterProjectWorkPackage : System.Web.UI.Page
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
            get_tbl_Zone_Addditional();
            get_tbl_PhysicalProgressComponent();
            get_tbl_Deliverables();
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
                            ddlULB.SelectedValue = Session["ULB_Id"].ToString();
                            ddlULB.Enabled = false;
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
    private void get_Employee_Vendor(int Division_Id)
    {
        string UserTypeId = "5";
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee(UserTypeId, 0, 0, 0, 0, 0, 0, 0, "", "", 0, 0,0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlVendor, "Person_Name_Mobile", "Person_Id");
        }
        else
        {
            ddlVendor.Items.Clear();
        }
    }
    private void get_tbl_PhysicalProgressComponent()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PhysicalProgressComponent();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPhysicalProgress.DataSource = ds.Tables[0];
            grdPhysicalProgress.DataBind();
        }
        else
        {
            grdPhysicalProgress.DataSource = null;
            grdPhysicalProgress.DataBind();
        }
    }
    private void get_tbl_Deliverables()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deliverables();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdDeliverables.DataSource = ds.Tables[0];
            grdDeliverables.DataBind();
        }
        else
        {
            grdDeliverables.DataSource = null;
            grdDeliverables.DataBind();
        }
    }
    private void get_tbl_Zone_Addditional()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionZone, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlAdditionZone.Items.Clear();
        }
    }
    private void get_Employee_Staff_JEAPE(int Division_Id)
    {
        string Designation_Id = "8,12";
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee("", 0, 0, 0, 0, 0, Division_Id, 0, Designation_Id, "", 0, 0,0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //AllClasses.FillDropDown(ds.Tables[0], ddlStaff, "Person_Name_Mobile", "Person_Id");
            lbReportingStaffJEAPE.DataTextField = "Person_Name_Mobile";
            lbReportingStaffJEAPE.DataValueField = "Person_Id";
            lbReportingStaffJEAPE.DataSource = ds.Tables[0];
            lbReportingStaffJEAPE.DataBind();
        }
        else
        {
            lbReportingStaffJEAPE.Items.Clear();
        }
    }
    private void get_Employee_Staff_AEPE(int Division_Id)
    {
        string Designation_Id = "10,5";
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee("", 0, 0, 0, 0, 0, Division_Id, 0, Designation_Id, "", 0, 0,0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //AllClasses.FillDropDown(ds.Tables[0], ddlStaff, "Person_Name_Mobile", "Person_Id");
            lbReportingStaffAEPE.DataTextField = "Person_Name_Mobile";
            lbReportingStaffAEPE.DataValueField = "Person_Id";
            lbReportingStaffAEPE.DataSource = ds.Tables[0];
            lbReportingStaffAEPE.DataBind();
        }
        else
        {
            lbReportingStaffAEPE.Items.Clear();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtProjectWorkName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Project Name");
            txtProjectWorkName.Focus();
            return;
        }
        if (txtAgreementAmount.Text.Trim() == "" || txtAgreementAmount.Text.Trim() == "0")
        {
            MessageBox.Show("Please Provide Project Agreement Amount");
            txtAgreementAmount.Focus();
            return;
        }
        if (txtAgreementDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Agreement Date");
            txtAgreementDate.Focus();
            return;
        }
        if (txtAgreementNo.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Agreement No");
            txtAgreementNo.Focus();
            return;
        }
        if (Session["UserType"].ToString() != "1")
        {
            if (txtLastRABillDate.Text.Trim() == "")
            {
                MessageBox.Show("Please Provide Last RA Bill Date. In  Case of New CB Please Fill Zero.");
                txtLastRABillDate.Focus();
                return;
            }
        }
        tbl_ProjectWorkPkg obj_tbl_ProjectWork = new tbl_ProjectWorkPkg();
        try
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Work_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Work_Id = 0;
        }
        try
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Id = 0;
        }
        obj_tbl_ProjectWork.ProjectWorkPkg_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());

        if (flUploadDPR.HasFile)
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_AgreementPath_Bytes = flUploadDPR.FileBytes;
            obj_tbl_ProjectWork.ProjectWorkPkg_Agreement_Extention = flUploadDPR.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Agreement_Extention = "-1";
        }
        if (flUploadBankGurantee.HasFile)
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_BankGuranteePath_Bytes = flUploadBankGurantee.FileBytes;
            obj_tbl_ProjectWork.ProjectWorkPkg_BankGurantee_Extention = flUploadBankGurantee.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_BankGurantee_Extention = "-1";
        }
        if (flUploadMobelization.HasFile)
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_MobelizationPath_Bytes = flUploadMobelization.FileBytes;
            obj_tbl_ProjectWork.ProjectWorkPkg_Mobelization_Extention = flUploadMobelization.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Mobelization_Extention = "-1";
        }
        if (flUploadPerformanceSec.HasFile)
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_PerformanceSecurityPath_Bytes = flUploadPerformanceSec.FileBytes;
            obj_tbl_ProjectWork.ProjectWorkPkg_PerformanceSecurity_Extention = flUploadPerformanceSec.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        else
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_PerformanceSecurity_Extention = "-1";
        }
        obj_tbl_ProjectWork.ProjectWorkPkg_Agreement_Date = txtAgreementDate.Text.Trim();
        obj_tbl_ProjectWork.ProjectWorkPkg_Due_Date = txtDueDate.Text.Trim();
        try
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_LastRABillNo = Convert.ToInt32(txtLastRABillNo.Text.Trim());
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_LastRABillNo = 0;
        }
        try
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_MobilizationAdvanceAmount = Convert.ToInt32(txtMobilizationAdvanceAmount.Text.Trim());
        }
        catch
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_MobilizationAdvanceAmount = 0;
        }
        obj_tbl_ProjectWork.ProjectWorkPkg_LastRABillDate = txtLastRABillDate.Text.Trim();
        obj_tbl_ProjectWork.ProjectWorkPkg_Agreement_No = txtAgreementNo.Text.Trim();
        obj_tbl_ProjectWork.ProjectWorkPkg_AgreementAmount = Convert.ToDecimal(txtAgreementAmount.Text.Trim());
        obj_tbl_ProjectWork.ProjectWorkPkg_Name = txtProjectWorkName.Text.Trim();
        obj_tbl_ProjectWork.ProjectWorkPkg_Code = txtPackageCode.Text.Trim();
        try
        {
            obj_tbl_ProjectWork.ProjectWorkPkg_Vendor_Id = Convert.ToInt32(ddlVendor.SelectedValue);
        }
        catch
        {

        }

        obj_tbl_ProjectWork.ProjectWorkPkg_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProjectWork.ProjectWorkPkg_Indent_No = txtEprocIndentNo.Text;
        obj_tbl_ProjectWork.ProjectWorkPkg_Status = 1;

        List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress = new List<tbl_ProjectPkg_PhysicalProgress>();
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {
            CheckBox checkBox = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            if (checkBox.Checked == true)
            {
                tbl_ProjectPkg_PhysicalProgress obj_tbl_ProjectPkg_PhysicalProgress1 = new tbl_ProjectPkg_PhysicalProgress();
                obj_tbl_ProjectPkg_PhysicalProgress1.ProjectPkg_PhysicalProgress_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectPkg_PhysicalProgress1.ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id = Convert.ToInt32(grdPhysicalProgress.Rows[i].Cells[0].Text.Trim());
                obj_tbl_ProjectPkg_PhysicalProgress1.ProjectPkg_PhysicalProgress_Status = 1;
                obj_tbl_ProjectPkg_PhysicalProgress.Add(obj_tbl_ProjectPkg_PhysicalProgress1);
            }

        }

        List<tbl_ProjectPkg_Deliverables> obj_tbl_ProjectPkg_Deliverables = new List<tbl_ProjectPkg_Deliverables>();
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox checkBox = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            if (checkBox.Checked == true)
            {
                tbl_ProjectPkg_Deliverables obj_tbl_ProjectPkg_Deliverables1 = new tbl_ProjectPkg_Deliverables();
                obj_tbl_ProjectPkg_Deliverables1.ProjectPkg_Deliverables_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectPkg_Deliverables1.ProjectPkg_Deliverables_Deliverables_Id = Convert.ToInt32(grdDeliverables.Rows[i].Cells[0].Text.Trim());
                obj_tbl_ProjectPkg_Deliverables1.ProjectPkg_Deliverables_Status = 1;
                obj_tbl_ProjectPkg_Deliverables.Add(obj_tbl_ProjectPkg_Deliverables1);
            }

        }
        if (Session["UserType"].ToString() != "1")
        {
            if (obj_tbl_ProjectPkg_PhysicalProgress == null)
            {
                MessageBox.Show("Please Check At Least One Physical Progress!");
                return;
            }
            if (obj_tbl_ProjectPkg_Deliverables == null)
            {
                MessageBox.Show("Please Check At Least One Deliverables!");
                return;
            }
        }
        List<tbl_ProjectWorkPkg_ReportingStaff_JE_APE> obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li = new List<tbl_ProjectWorkPkg_ReportingStaff_JE_APE>();

        foreach (ListItem listItem in lbReportingStaffJEAPE.Items)
        {
            if (listItem.Selected)
            {
                tbl_ProjectWorkPkg_ReportingStaff_JE_APE obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE = new tbl_ProjectWorkPkg_ReportingStaff_JE_APE();
                obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id = Convert.ToInt32(listItem.Value);
                obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1;
                obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li.Add(obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE);
            }
        }

        List<tbl_ProjectWorkPkg_ReportingStaff_AE_PE> obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li = new List<tbl_ProjectWorkPkg_ReportingStaff_AE_PE>();
        foreach (ListItem listItem in lbReportingStaffAEPE.Items)
        {
            if (listItem.Selected)
            {
                tbl_ProjectWorkPkg_ReportingStaff_AE_PE obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE = new tbl_ProjectWorkPkg_ReportingStaff_AE_PE();
                obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id = Convert.ToInt32(listItem.Value);
                obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1;
                obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li.Add(obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE);
            }
        }
        if (Session["UserType"].ToString() != "1")
        {
            if (obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li == null || obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li.Count == 0)
            {
                MessageBox.Show("Please Provide Reporting Staff JE / APE!");
                return;
            }
            if (obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li == null || obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li.Count == 0)
            {
                MessageBox.Show("Please Provide Reporting Staff AE / PE!");
                return;
            }
        }

        List<tbl_ProjectAdditionalArea> obj_tbl_ProjectAdditionalArea_Li = new List<tbl_ProjectAdditionalArea>();
        for (int i = 0; i < dgvAdditionalDivision.Rows.Count; i++)
        {
            tbl_ProjectAdditionalArea obj_tbl_ProjectAdditionalArea = new tbl_ProjectAdditionalArea();
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_ZoneId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[0].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_CircleId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[1].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_DevisionId = Convert.ToInt32(dgvAdditionalDivision.Rows[i].Cells[2].Text.Trim());
            obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_Status = 1;
            obj_tbl_ProjectAdditionalArea_Li.Add(obj_tbl_ProjectAdditionalArea);
        }

        if ((new DataLayer()).Insert_tbl_ProjectWorkPkg(obj_tbl_ProjectWork, obj_tbl_ProjectPkg_PhysicalProgress, obj_tbl_ProjectPkg_Deliverables, obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li, obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li, obj_tbl_ProjectAdditionalArea_Li))
        {
            MessageBox.Show("Project Package Created Successfully!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Creating Project Package!");
            return;
        }
    }

    private void reset()
    {
        //ddlDistrict.SelectedValue = "0";
        ddlULB.Items.Clear();
        hf_ProjectWork_Id.Value = "0";
        hf_ProjectWorkPkg_Id.Value = "0";
        txtProjectWorkName.Text = "";
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            chkSelectAllApprove.Checked = false;
            CheckBox chkSelectAllApproveH = grdDeliverables.HeaderRow.FindControl("chkSelectAllDeliverables") as CheckBox;
            chkSelectAllApproveH.Checked = false;
        }
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {

            CheckBox chkSelectAllApprove = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            chkSelectAllApprove.Checked = false;
            CheckBox chkSelectAllDeliverables = grdPhysicalProgress.HeaderRow.FindControl("chkSelectAllApproveH") as CheckBox;
            chkSelectAllDeliverables.Checked = false;
        }
        ViewState["AdditionalDivision"] = null;
        dgvAdditionalDivision.DataSource = null;
        dgvAdditionalDivision.DataBind();
        divEntry.Visible = false;
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
        ds = (new DataLayer()).get_tbl_ProjectWork(Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0,"");
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

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["UserType"].ToString() == "1" || Session["PersonJuridiction_DesignationId"].ToString() == "25")
        {
            divEntry.Visible = true;
            GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
            hf_ProjectWork_Id.Value = gr.Cells[0].Text.Trim();
            int Division_Id = Convert.ToInt32(gr.Cells[4].Text.Trim());
            get_Employee_Staff_JEAPE(Division_Id);
            get_Employee_Staff_AEPE(Division_Id);
            get_Employee_Vendor(Division_Id);
            ViewState["AdditionalDivision"] = null;
            dgvAdditionalDivision.DataSource = null;
            dgvAdditionalDivision.DataBind();
        }
    }

    protected void btnView_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Work_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(Work_Id, 0, 0, 0, 0, 0, 0, "", "", false,"");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPackageDetails.DataSource = ds.Tables[0];
            grdPackageDetails.DataBind();
            mp1.Show();
        }
        else
        {
            grdPackageDetails.DataSource = null;
            grdPackageDetails.DataBind();
            MessageBox.Show("Package Details Not Found");
        }
        hf_ProjectWork_Id.Value = gr.Cells[0].Text.Trim();
        int Division_Id = Convert.ToInt32(gr.Cells[4].Text.Trim());
        get_Employee_Staff_JEAPE(Division_Id);
        get_Employee_Staff_AEPE(Division_Id);
        get_Employee_Vendor(Division_Id);
    }

    protected void btnPackageEdit_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkUpdate = sender as ImageButton;
        hf_ProjectWorkPkg_Id.Value = (lnkUpdate.Parent.Parent as GridViewRow).Cells[0].Text.Trim();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).CheckPackageApproval(hf_ProjectWorkPkg_Id.Value);
        if (AllClasses.CheckDataSet(ds) || Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_ProjectWorkPkg_Edit(Convert.ToInt32(hf_ProjectWorkPkg_Id.Value));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    ddlVendor.SelectedValue = ds.Tables[0].Rows[0]["ProjectWorkPkg_Vendor_Id"].ToString();
                }
                catch
                {
                    ddlVendor.SelectedValue = "0";
                }

                txtPackageCode.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Code"].ToString();
                txtProjectWorkName.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Name"].ToString();
                txtAgreementAmount.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_AgreementAmount"].ToString();
                txtAgreementNo.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Agreement_No"].ToString();
                txtAgreementDate.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Agreement_Date"].ToString();
                txtDueDate.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Due_Date"].ToString();
                txtEprocIndentNo.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_Indent_No"].ToString();
                txtLastRABillDate.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_LastRABillDate"].ToString();
                txtLastRABillNo.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_LastRABillNo"].ToString();
                txtMobilizationAdvanceAmount.Text = ds.Tables[0].Rows[0]["ProjectWorkPkg_MobilizationAdvanceAmount"].ToString();

                string[] List_ReportingStaffJEAPE = ds.Tables[0].Rows[0]["List_ReportingStaff_JEAPE_Id"].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (List_ReportingStaffJEAPE.Length > 0)
                {
                    for (int i = 0; i < List_ReportingStaffJEAPE.Length; i++)
                    {
                        foreach (ListItem listItem in lbReportingStaffJEAPE.Items)
                        {
                            if (List_ReportingStaffJEAPE[i].Trim() == listItem.Value)
                            {
                                listItem.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (ListItem listItem in lbReportingStaffJEAPE.Items)
                    {
                        listItem.Selected = false;
                    }
                }

                string[] List_ReportingStaffAEPE = ds.Tables[0].Rows[0]["List_ReportingStaff_AEPE_Id"].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (List_ReportingStaffAEPE.Length > 0)
                {
                    for (int i = 0; i < List_ReportingStaffAEPE.Length; i++)
                    {
                        foreach (ListItem listItem in lbReportingStaffAEPE.Items)
                        {
                            if (List_ReportingStaffAEPE[i].Trim() == listItem.Value)
                            {
                                listItem.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (ListItem listItem in lbReportingStaffAEPE.Items)
                    {
                        listItem.Selected = false;
                    }
                }

                divEntry.Visible = true;
                divEntry.Focus();
            }
            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                grdPhysicalProgress.DataSource = ds.Tables[1];
                grdPhysicalProgress.DataBind();
            }
            else
            {
                grdPhysicalProgress.DataSource = null;
                grdPhysicalProgress.DataBind();
            }
            if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                grdDeliverables.DataSource = ds.Tables[2];
                grdDeliverables.DataBind();
            }
            else
            {
                grdDeliverables.DataSource = null;
                grdDeliverables.DataBind();
            }
            if (ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                ViewState["AdditionalDivision"] = ds.Tables[3];
                dgvAdditionalDivision.DataSource = ds.Tables[3];
                dgvAdditionalDivision.DataBind();
            }
            else
            {
                ViewState["AdditionalDivision"] = null;
                dgvAdditionalDivision.DataSource = null;
                dgvAdditionalDivision.DataBind();
            }
        }
        else
        {
            MessageBox.Show("Please Upload Approval File From Package Update!");
            return;
        }
    }

    protected void btnPackageDelete_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Person_Id_Delete = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        if (new DataLayer().Delete_tbl_ProjectWorkPkg(Person_Id_Delete, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            return;
        }
    }

    protected void chkSelectAllApproveH_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAllApproveH1 = (sender as CheckBox);
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdPhysicalProgress.Rows[i].FindControl("chkPostPhysicalProgress") as CheckBox;
            chkSelectAllApprove.Checked = chkSelectAllApproveH1.Checked;
        }
    }

    protected void chkSelectAllDeliverables_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAllApproveH = (sender as CheckBox);
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdDeliverables.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            chkSelectAllApprove.Checked = chkSelectAllApproveH.Checked;
        }
    }

    protected void grdPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkPostPhysicalProgress = (e.Row.FindControl("chkPostPhysicalProgress") as CheckBox);
            if (Convert.ToInt32(e.Row.Cells[1].Text.Trim().Replace("&nbsp;", "")) > 0)
            {
                chkPostPhysicalProgress.Checked = true;
            }
            else
            {
                chkPostPhysicalProgress.Checked = false;
            }

        }
    }
    protected void grdDeliverables_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkPostDeliverables = (e.Row.FindControl("chkPostDeliverables") as CheckBox);
            if (Convert.ToInt32(e.Row.Cells[1].Text.Trim().Replace("&nbsp;", "")) > 0)
            {
                chkPostDeliverables.Checked = true;
            }
            else
            {
                chkPostDeliverables.Checked = false;
            }

        }
    }


    protected void grdPackageDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDelete = e.Row.FindControl("btnPackageDelete") as ImageButton;
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
    protected void ddlAdditionZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdditionZone.SelectedValue == "0")
        {
            ddlAdditionalCircle.Items.Clear();
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            get_tbl_Circle_Additional(Convert.ToInt32(ddlAdditionZone.SelectedValue));
        }
    }

    protected void ddlAdditionalCircle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdditionalCircle.SelectedValue == "0")
        {
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            get_tbl_Division_Additional(Convert.ToInt32(ddlAdditionalCircle.SelectedValue));
        }
    }
    private void get_tbl_Circle_Additional(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(Zone_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionalCircle, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlAdditionalCircle.Items.Clear();
        }
    }
    private void get_tbl_Division_Additional(int Circle_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(Circle_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlAdditionalDivision, "Division_Name", "Division_Id");
        }
        else
        {
            ddlAdditionalDivision.Items.Clear();
        }
    }

    protected void btnAddAdditionalDivision_Click(object sender, EventArgs e)
    {
        if (ddlAdditionZone.SelectedValue == "0" || ddlAdditionZone.SelectedValue == "")
        {
            MessageBox.Show("Please Select Zone");
            ddlAdditionZone.Focus();
            return;
        }
        if (ddlAdditionalCircle.SelectedValue == "0" || ddlAdditionalCircle.SelectedValue == "")
        {
            MessageBox.Show("Please Select Circle");
            ddlAdditionalCircle.Focus();
            return;
        }
        if (ddlAdditionalDivision.SelectedValue == "0" || ddlAdditionalDivision.SelectedValue == "")
        {
            MessageBox.Show("Please Select Divison");
            ddlAdditionalDivision.Focus();
            return;
        }

        if (ViewState["AdditionalDivision"] != null)
        {
            DataTable dt = (DataTable)ViewState["AdditionalDivision"];

            DataRow dr = dt.NewRow();

            dr["ProjectAdditionalArea_ZoneId"] = ddlAdditionZone.SelectedValue;
            dr["ProjectAdditionalArea_CircleId"] = ddlAdditionalCircle.SelectedValue;
            dr["ProjectAdditionalArea_DevisionId"] = ddlAdditionalDivision.SelectedValue;
            dr["Zone_Name"] = ddlAdditionZone.SelectedItem.Text.Trim();
            dr["Circle_Name"] = ddlAdditionalCircle.SelectedItem.Text.Trim();
            dr["Division_Name"] = ddlAdditionalDivision.SelectedItem.Text.Trim();
            dt.Rows.Add(dr);

            ViewState["AdditionalDivision"] = dt;

            dgvAdditionalDivision.DataSource = dt;
            dgvAdditionalDivision.DataBind();
            ddlAdditionZone.SelectedValue = "0";
            ddlAdditionalCircle.Items.Clear();
            ddlAdditionalDivision.Items.Clear();
        }
        else
        {
            DataSet ds = (new DataLayer()).get_tbl_ProjectWork_Edit(0);
            try
            {
                ViewState["AdditionalDivision"] = ds.Tables[2];
            }
            catch
            {
                ViewState["AdditionalDivision"] = null;
            }
        }

    }

    protected void lnkDeleteAdditionalDivision_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["AdditionalDivision"] != null)
        {
            if (index >= 0)
            {
                DataTable dt = (DataTable)ViewState["AdditionalDivision"];
                dt.Rows.RemoveAt(index);
                dgvAdditionalDivision.DataSource = dt;
                dgvAdditionalDivision.DataBind();
                ViewState["AdditionalDivision"] = dt;
            }

        }
    }

    protected void btnAddBOQItem_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkUpdate = sender as ImageButton;
           string DivisionId= (lnkUpdate.Parent.Parent as GridViewRow).Cells[2].Text.Trim();
            DataSet ds = new DataSet();
            ds = (new DataLayer()).CheckPackageBOQ(hf_ProjectWorkPkg_Id.Value);
            if (AllClasses.CheckDataSet(ds))
            {
                Response.Redirect("MasterProjectWorkPackageAddBOQItemDivisionWise.aspx?ProjectWorkPkg_Id=" + hf_ProjectWorkPkg_Id.Value.Trim() + "&DivisionId=" + DivisionId);
            }
            else
            {
                MessageBox.Show("Please Add BOQ Items!");
                return;
            }
              
        }
        catch
        {


        }
    }
}