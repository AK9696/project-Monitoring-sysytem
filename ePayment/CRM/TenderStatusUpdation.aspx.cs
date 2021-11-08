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
public partial class TenderStatusUpdation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            get_tbl_Project();
            get_M_Jurisdiction();
            get_tbl_Zone();
            get_Employee_Reporting_Manager();
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
    private void get_M_Jurisdiction()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(3, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlDistrict.Items.Clear();
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
        get_tbl_PackageEMB_Approve(Package_Id, EMB_Master_Id);
    }
    private void get_tbl_PackageEMB_Approve(int Package_Id, int EMB_Master_Id)
    {
        int Project_Id = 0;
        int District_Id = 0;
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", false, "IsApproved");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            divData.Visible = true;
            divDPRUpload.Visible = false;
        }
        else
        {
            divData.Visible = false;
            divDPRUpload.Visible = false;
            grdPost.DataSource = null;
            grdPost.DataBind();
            MessageBox.Show("No Records Found");
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
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divDPRUpload.Visible = true;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Package_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());

        int PackageWork_Id = 0;
        try
        {
            PackageWork_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            PackageWork_Id = 0;
        }
        int ProjectWorkPkg_Vendor_Id = 0;
        try
        {
            ProjectWorkPkg_Vendor_Id = 0;
        }
        catch
        {
            ProjectWorkPkg_Vendor_Id = 0;
        }
        decimal Budget = 0;

        try
        {
            Budget = decimal.Parse(gr.Cells[20].Text.Trim());
        }
        catch
        {
            Budget = 0;
        }
        decimal Release = 0;

        try
        {
            Release = decimal.Parse(gr.Cells[32].Text.Trim());
        }
        catch
        {
            Release = 0;
        }
        hf_ProjectWork_Id.Value = Package_Id.ToString() + "|" + PackageWork_Id.ToString() + "|" + Budget.ToString() + "|" + Release.ToString();
        gr.BackColor = Color.LightGreen;

        get_tbl_ProjectDPRTenderInfo(Package_Id.ToString(), PackageWork_Id.ToString(), ProjectWorkPkg_Vendor_Id);
    }

    private void get_tbl_ProjectDPRTenderInfo(string projectPkg_Id, string work_Id,int ProjectWorkPkg_Vendor_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectPkgTenderInfo(projectPkg_Id, work_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            rbtNIT.SelectedValue = "Y";
            rbtNIT_SelectedIndexChanged(rbtNIT, new EventArgs());
            rbtNIT.Enabled = false;
            try
            {
                ddlTenderStatus.SelectedValue = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderStatus"].ToString();
                ddlTenderStatus.Enabled = false;
            }
            catch
            {
                ddlTenderStatus.SelectedValue = "0";
            }

            txtNITDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_NITDate"].ToString();
            txtNITDate.Enabled = false;
            txtTenderCompletion.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_CompletionTime"].ToString();
            txtTenderCompletion.Enabled = false;
            txtTenderDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderDate"].ToString();
            txtTenderDate.Enabled = false;
            txtAmount.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderAmount"].ToString();
            txtAmount.Enabled = false;
            txtComments.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_Comments"].ToString();
            txtComments.Enabled = false;
            try
            {
                ddlPersonVendor.SelectedValue = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_VendorPersonId"].ToString();
                ddlPersonVendor.Enabled = false;
            }
            catch
            {
                ddlPersonVendor.SelectedValue = "0";
                ddlPersonVendor.Enabled = true;
            }

            try
            {
                txtCentage.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_Centage"].ToString();

            }
            catch
            {
                txtCentage.Text = "0.000";
            }
            txtCentage.Enabled = false;
            try
            {
                txtWorkCostIn.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_WorkCostIn"].ToString();

            }
            catch
            {
                txtWorkCostIn.Text = "0.000";
            }
            txtWorkCostIn.Enabled = false;

            try
            {
                txtWorkCostOut.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_WorkCostOut"].ToString();

            }
            catch
            {
                txtWorkCostOut.Text = "0.000";
            }
            txtWorkCostOut.Enabled = false;

            try
            {
                txtGSTNotIncludeWorkCost.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_GSTNotIncludeWorkCost"].ToString();

            }
            catch
            {
                txtGSTNotIncludeWorkCost.Text = "0.000";
            }
            txtGSTNotIncludeWorkCost.Enabled = false;

            txtPrebidMeetingDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_PrebidMeetingDate"].ToString();
            txtPrebidMeetingDate.Enabled = false;
            txtTenderOutDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderOutDate"].ToString();
            txtTenderOutDate.Enabled = false;
            txtTenderTechnicalDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderTechnicalDate"].ToString();
            txtTenderTechnicalDate.Enabled = false;
            txtTenderFinancialDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_TenderFinancialDate"].ToString();
            txtTenderFinancialDate.Enabled = false;
            txtContractSignDate.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_ContractSignDate"].ToString();
            txtContractSignDate.Enabled = false;
            txtContractBondNo.Text = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_ContractBondNo"].ToString();
            txtContractBondNo.Enabled = false;


            btnUpload.Visible = false;
        }
        else
        {
            try
            {
                ddlPersonVendor.SelectedValue = ds.Tables[0].Rows[0]["ProjectPkgTenderInfo_VendorPersonId"].ToString();
                ddlPersonVendor.Enabled = true;
            }
            catch
            {
                ddlPersonVendor.SelectedValue = "0";
                ddlPersonVendor.Enabled = true;
            }
        }
    }

    protected void rbtNIT_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtNIT.SelectedValue == "N")
        {
            divTenderInfo.Visible = false;
            divTenderInfo1.Visible = false;
            divTenderInfo2.Visible = false;
            divTenderInfo3.Visible = false;
        }
        else
        {
            divTenderInfo.Visible = true;
            divTenderInfo1.Visible = true;
            divTenderInfo2.Visible = true;
            divTenderInfo3.Visible = true;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (_data == null)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (_data.Length != 4)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (ddlTenderStatus.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Tender Status");
            return;
        }
        if (ddlDistrict.SelectedValue == "0")
        {
            MessageBox.Show("Please Select District");
            return;
        }
        if (txtNITDate.Text == "")
        {
            MessageBox.Show("Please Provide Tender NIT Date");
            return;
        }
        if (txtTenderDate.Text == "")
        {
            MessageBox.Show("Please Provide Tender Opening Date");
            return;
        }
        if (txtTenderCompletion.Text == "")
        {
            MessageBox.Show("Please Provide Tender Completion Time");
            return;
        }
        int ProjectPkg_Id = 0;
        try
        {
            ProjectPkg_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            ProjectPkg_Id = 0;
        }
        int Work_Id = 0;
        try
        {
            Work_Id = Convert.ToInt32(_data[1]);
        }
        catch
        {
            Work_Id = 0;
        }
        decimal Total_Budget = 0;
        try
        {
            Total_Budget = Convert.ToDecimal(_data[2]);
        }
        catch
        {
            Total_Budget = 0;
        }
        if (ProjectPkg_Id == 0)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (txtComments.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Comments");
            txtComments.Focus();
            return;
        }
        if (txtPrebidMeetingDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Pre-Bid Meeting Date");
            txtPrebidMeetingDate.Focus();
            return;
        }
        if (txtTenderOutDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Bid Submission Date (Closing)");
            txtTenderOutDate.Focus();
            return;
        }
        if (txtTenderTechnicalDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Tender Opening Date (Technical)");
            txtTenderTechnicalDate.Focus();
            return;
        }
        if (txtTenderFinancialDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Tender Opening Date (Financial)");
            txtTenderFinancialDate.Focus();
            return;
        }
        if (txtContractSignDate.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Contract Signing Date");
            txtContractSignDate.Focus();
            return;
        }
        decimal Allocated_Budget = 0;
        try
        {
            Allocated_Budget = Convert.ToDecimal(txtAmount.Text.Trim());
        }
        catch
        {
            Allocated_Budget = 0;
        }
        if (Allocated_Budget == 0)
        {
            MessageBox.Show("Plese Provide Tender Amount");
            txtAmount.Focus();
            return;
        }
        if (Allocated_Budget > Total_Budget)
        {
            MessageBox.Show("Tender Finalized Amount Should be Less or equal to Budget Allocated");
            txtAmount.Focus();
            return;
        }
       

        tbl_PersonDetail obj_PersonDetail = new tbl_PersonDetail();
        tbl_PersonJuridiction obj_PersonJuridiction = new tbl_PersonJuridiction();
        if (divAddVendor.Visible)
        {
            obj_PersonDetail = new tbl_PersonDetail();
            obj_PersonDetail.Person_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_PersonDetail.Person_Id = 0;
            obj_PersonDetail.Person_BranchOffice_Id = 1;
            obj_PersonDetail.Person_AddressLine1 = "";
            obj_PersonDetail.Person_EmailId = "";
            obj_PersonDetail.Person_Mobile1 = txtVendorMobile1.Text.Trim();
            obj_PersonDetail.Person_Mobile2 = "";
            obj_PersonDetail.Person_Name = txtVendorName.Text.Trim();
            obj_PersonDetail.Person_FName = "";
            obj_PersonDetail.Person_Status = 1;

            obj_PersonJuridiction = new tbl_PersonJuridiction();
            obj_PersonJuridiction.PersonJuridiction_Id = 0;
            obj_PersonJuridiction.PersonJuridiction_PersonId = 0;
            obj_PersonJuridiction.M_Level_Id = 3;
            obj_PersonJuridiction.M_Jurisdiction_Id = Convert.ToInt32(ddlDistrict.SelectedValue);
            obj_PersonJuridiction.PersonJuridiction_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_PersonJuridiction.PersonJuridiction_DepartmentId = 0;
            obj_PersonJuridiction.PersonJuridiction_GSTIN = txtVendorGSTIN.Text.Trim();
            obj_PersonJuridiction.PersonJuridiction_DesignationId = 0;
            obj_PersonJuridiction.PersonJuridiction_ParentPerson_Id = 0;
            obj_PersonJuridiction.PersonJuridiction_Status = 1;
            obj_PersonJuridiction.PersonJuridiction_UserTypeId = 5;
        }
        else
        {
            obj_PersonDetail = null;
            obj_PersonJuridiction = null;
        }
        tbl_ProjectPkgTenderInfo obj_tbl_ProjectPkgTenderInfo = new tbl_ProjectPkgTenderInfo();
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Comments = txtComments.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectPkg_Id = ProjectPkg_Id;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectWork_Id = Work_Id;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Status = 1;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderAmount = Allocated_Budget * 100000;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderDate = txtTenderDate.Text.Trim();
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_CompletionTime = txtTenderCompletion.Text.Trim();
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_NITDate = txtNITDate.Text.Trim();
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderStatus = ddlTenderStatus.SelectedValue;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_VendorPersonId = Convert.ToInt32(ddlPersonVendor.SelectedValue);
        try
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Centage = Convert.ToDecimal(txtCentage.Text);
        }
        catch
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Centage = 0;
        }
        try
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostIn = (Convert.ToDecimal(txtWorkCostIn.Text) * 100000);
        }
        catch
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostIn = 0;
        }
        try
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostOut = (Convert.ToDecimal(txtWorkCostOut.Text) * 100000);
        }
        catch
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostOut = 0;
        }
        try
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_GSTNotIncludeWorkCost = Convert.ToDecimal(txtGSTNotIncludeWorkCost.Text);
        }
        catch
        {
            obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_GSTNotIncludeWorkCost = 0;
        }
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_PrebidMeetingDate = txtPrebidMeetingDate.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderOutDate = txtTenderOutDate.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderTechnicalDate = txtTenderTechnicalDate.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderFinancialDate = txtTenderFinancialDate.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ContractSignDate = txtContractSignDate.Text;
        obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ContractBondNo = txtContractBondNo.Text;
        string msg = "";
        if (new DataLayer().Update_tbl_ProjectPkg_TenderStatus(obj_tbl_ProjectPkgTenderInfo, obj_PersonDetail, obj_PersonJuridiction, ref msg))
        {
            if (msg == "")
            {
                MessageBox.Show("Tender Status Updated Successfully");
                btnSearch_Click(null, null);
                reset();
                return;
            }
            else
            {
                MessageBox.Show(msg);
                return;
            }
        }
        else
        {
            MessageBox.Show("Unable To Update Tender Status");
            return;
        }
    }
    private void reset()
    {
        divDPRUpload.Visible = false;
        txtComments.Text = "";
        hf_ProjectWork_Id.Value = "0|0|0";
    }
    protected void ddlPersonVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPersonVendor.SelectedValue == "-1")
        {
            divAddVendor.Visible = true;
        }
        else
        {
            divAddVendor.Visible = false;
        }
    }
    private void get_Employee_Reporting_Manager()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Employee("5", 0, 0, 0, 0, 0, 0, 0,"","",0,0,0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].NewRow();

            dr["Person_Name_Mobile"] = "--Select--";
            dr["Person_Id"] = "0";
            ds.Tables[0].Rows.InsertAt(dr, 0);

            dr = ds.Tables[0].NewRow();

            dr["Person_Name_Mobile"] = "<< ADD NEW VENDOR>>";
            dr["Person_Id"] = "-1";
            ds.Tables[0].Rows.Add(dr);

            ddlPersonVendor.DataTextField = "Person_Name_Mobile";
            ddlPersonVendor.DataValueField = "Person_Id";
            ddlPersonVendor.DataSource = ds.Tables[0];
            ddlPersonVendor.DataBind();
        }
        else
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["Person_Name_Mobile"] = "<< ADD NEW VENDOR>>";
            dr["Person_Id"] = "-1";
            ds.Tables[0].Rows.Add(dr);

            ddlPersonVendor.DataTextField = "Person_Name_Mobile";
            ddlPersonVendor.DataValueField = "Person_Id";
            ddlPersonVendor.DataSource = ds.Tables[0];
            ddlPersonVendor.DataBind();

            divAddVendor.Visible = true;
        }
    }

    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDPRFile = (e.Row.FindControl("lnkDPRFile") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDPRFile);
            LinkButton lnkDocmentFile = (e.Row.FindControl("lnkDocmentFile") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDocmentFile);
            LinkButton lnkSitePic1 = (e.Row.FindControl("lnkSitePic1") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkSitePic1);
            LinkButton lnkSitePic2 = (e.Row.FindControl("lnkSitePic2") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkSitePic2);

            LinkButton lnkGO = (e.Row.FindControl("lnkGO") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkGO);
        }
    }
    protected void lnkDPRFile_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[6].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkDocmentFile_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[7].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkSitePic1_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[8].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkSitePic2_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[9].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }
    protected void lnkGO_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[10].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }
}