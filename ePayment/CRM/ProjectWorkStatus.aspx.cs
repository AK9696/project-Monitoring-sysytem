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

public partial class ProjectWorkStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            AllClasses.Create_Directory_Session(1);
            AllClasses.Create_Directory_Session2();
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
        int Project_Id = Convert.ToInt32(gr.Cells[2].Text.Trim());
        int PackageWork_Id = 0;
        try
        {
            PackageWork_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            PackageWork_Id = 0;
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

        get_tbl_ProjectUC_ReleaseDetails(Package_Id, PackageWork_Id);
        get_tbl_ProjectQuestionnaire(Project_Id);
        get_tbl_ProjectUC(Package_Id, PackageWork_Id);
    }

    private void get_tbl_ProjectUC_ReleaseDetails(int Package_Id, int ProjectWork_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectUC_ReleaseDetails(Package_Id, ProjectWork_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            lblRealseTillPreFinYear.Text = ds.Tables[0].Rows[0]["ReleaseTillPrevFinYear"].ToString();
        }
        else
        {
            lblRealseTillPreFinYear.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            lblReleaseCurrentFinYears.Text = ds.Tables[1].Rows[0]["ReleaseTillPrevFinYear"].ToString();
        }
        else
        {
            lblReleaseCurrentFinYears.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
        {
            lblReleasePrevMonth.Text = ds.Tables[2].Rows[0]["ReleasePrevMonth"].ToString();
        }
        else
        {
            lblReleasePrevMonth.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
        {
            lblReleaseCurrentMonth.Text = ds.Tables[3].Rows[0]["ReleaseCurrentMonth"].ToString();
        }
        else
        {
            lblReleaseCurrentMonth.Text = "0.000";
        }
        lblReleaseTotal.Text = (Convert.ToDecimal(lblRealseTillPreFinYear.Text) + Convert.ToDecimal(lblReleaseCurrentFinYears.Text) + Convert.ToDecimal(lblReleasePrevMonth.Text) + Convert.ToDecimal(lblReleaseCurrentMonth.Text)).ToString("F3");

        if (ds != null && ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
        {
            lblWorkExpenditureAmtTillPrevFinYear.Text = ds.Tables[4].Rows[0]["ExpenditureAmtTllPrevFinYear"].ToString();
        }
        else
        {
            lblWorkExpenditureAmtTillPrevFinYear.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 5 && ds.Tables[5].Rows.Count > 0)
        {
            lblWorkExpenditureAmtCurrentFinYear.Text = ds.Tables[5].Rows[0]["ExpenditureAmtCurrentFinYear"].ToString();
        }
        else
        {
            lblWorkExpenditureAmtCurrentFinYear.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 6 && ds.Tables[6].Rows.Count > 0)
        {
            lblWorkExpenditureAmtPrevMonth.Text = ds.Tables[6].Rows[0]["ExpenditureAmtPrevMonth"].ToString();
        }
        else
        {
            lblWorkExpenditureAmtPrevMonth.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 7 && ds.Tables[7].Rows.Count > 0)
        {
            txtFundUtilized.Text = ds.Tables[7].Rows[0]["ExpenditureAmtCurrentMonth"].ToString();
            txtFundUtilized.Enabled = false;
        }
        else
        {
            txtFundUtilized.Text = "0.000";
            txtFundUtilized.Enabled = true;
        }
        lblWorkExpenditureAmtTotal.Text = (Convert.ToDecimal(lblWorkExpenditureAmtTillPrevFinYear.Text) + Convert.ToDecimal(lblWorkExpenditureAmtCurrentFinYear.Text) + Convert.ToDecimal(lblWorkExpenditureAmtPrevMonth.Text) + Convert.ToDecimal(txtFundUtilized.Text)).ToString("F3");


        if (ds != null && ds.Tables.Count > 8 && ds.Tables[8].Rows.Count > 0)
        {
            lblCentageTillPrevFinYear.Text = ds.Tables[8].Rows[0]["CentageTllPrevFinYear"].ToString();
        }
        else
        {
            lblCentageTillPrevFinYear.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 9 && ds.Tables[9].Rows.Count > 0)
        {
            lblCentageCurrentFinYear.Text = ds.Tables[9].Rows[0]["CentageCurrentFinYear"].ToString();
        }
        else
        {
            lblCentageCurrentFinYear.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 10 && ds.Tables[10].Rows.Count > 0)
        {
            lblCentagePrevMonth.Text = ds.Tables[10].Rows[0]["CentageAmtPrevMonth"].ToString();
        }
        else
        {
            lblCentagePrevMonth.Text = "0.000";
        }

        if (ds != null && ds.Tables.Count > 11 && ds.Tables[11].Rows.Count > 0)
        {
            txtFundCentage.Text = ds.Tables[11].Rows[0]["CentageAmtCurrentMonth"].ToString();
            txtFundCentage.Enabled = false;
        }
        else
        {
            txtFundCentage.Text = "0.000";
            txtFundCentage.Enabled = true;
        }
        lblCentageTotal.Text = (Convert.ToDecimal(lblCentageTillPrevFinYear.Text) + Convert.ToDecimal(lblCentageCurrentFinYear.Text) + Convert.ToDecimal(lblCentagePrevMonth.Text) + Convert.ToDecimal(txtFundCentage.Text)).ToString();
        lblTotalExpenditureTillPrevFinYear.Text = (Convert.ToDecimal(lblWorkExpenditureAmtTillPrevFinYear.Text) + Convert.ToDecimal(lblCentageTillPrevFinYear.Text)).ToString();
        lblTotalExpenditureCurrentinYear.Text = (Convert.ToDecimal(lblWorkExpenditureAmtCurrentFinYear.Text) + Convert.ToDecimal(lblCentageCurrentFinYear.Text)).ToString();
        lblTotalExpenditurePrevMonth.Text = (Convert.ToDecimal(lblWorkExpenditureAmtPrevMonth.Text) + Convert.ToDecimal(lblCentagePrevMonth.Text)).ToString();
        lblTotalExpenditureCurrentMonth.Text = (Convert.ToDecimal(txtFundUtilized.Text) + Convert.ToDecimal(txtFundCentage.Text)).ToString();
        lblTotalExpenditureTotal.Text = (Convert.ToDecimal(lblWorkExpenditureAmtTotal.Text) + Convert.ToDecimal(lblCentageTotal.Text)).ToString();

        if (ds != null && ds.Tables.Count > 12 && ds.Tables[12].Rows.Count > 0)
        {
            lblProgressPrevTotal.Text = ds.Tables[12].Rows[0]["PhysicalPrevTotal"].ToString();
        }
        else
        {
            lblProgressPrevTotal.Text = "0";
        }

        if (ds != null && ds.Tables.Count > 13 && ds.Tables[13].Rows.Count > 0)
        {
            lblProgressLastMonth.Text = ds.Tables[13].Rows[0]["PhysicalLastMonth"].ToString();
        }
        else
        {
            lblProgressLastMonth.Text = "0";
        }

        if (ds != null && ds.Tables.Count > 14 && ds.Tables[14].Rows.Count > 0)
        {
            txtPhysicalProgress.Text = ds.Tables[14].Rows[0]["PhysicalCurrentMonth"].ToString();
            txtPhysicalProgress.Enabled = false;
        }
        else
        {
            txtPhysicalProgress.Text = "0";
            txtPhysicalProgress.Enabled = true;
        }

        if (ds != null && ds.Tables.Count > 15 && ds.Tables[15].Rows.Count > 0)
        {
            lblProgressTotal.Text = ds.Tables[15].Rows[0]["PhysicalTotal"].ToString();
        }
        else
        {
            lblProgressTotal.Text = "0";
        }

        if (ds != null && ds.Tables.Count > 16 && ds.Tables[16].Rows.Count > 0)
        {
            grdPhysicalProgress.DataSource = ds.Tables[16];
            grdPhysicalProgress.DataBind();
        }
        else
        {
            grdPhysicalProgress.DataSource = null;
            grdPhysicalProgress.DataBind();
        }
        if (ds != null && ds.Tables.Count > 17 && ds.Tables[17].Rows.Count > 0)
        {
            grdDeliverables.DataSource = ds.Tables[17];
            grdDeliverables.DataBind();
        }
        else
        {
            grdDeliverables.DataSource = null;
            grdDeliverables.DataBind();
        }
        //divExtendedTracking.Visible = true;


    }
    private void get_tbl_ProjectUC(int Package_Id, int ProjectWork_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectUC(Package_Id, ProjectWork_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdUC.DataSource = ds.Tables[0];
            grdUC.DataBind();
        }
        else
        {
            grdUC.DataSource = null;
            grdUC.DataBind();
        }
    }

    private void get_tbl_ProjectQuestionnaire(int Project_Id)
    {
        DataSet ds = new DataLayer().get_tbl_ProjectQuestionnaire(Project_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            dgvQuestionnaire.DataSource = ds.Tables[0];
            dgvQuestionnaire.DataBind();
        }
        else
        {
            dgvQuestionnaire.DataSource = null;
            dgvQuestionnaire.DataBind();
        }
    }
    protected void dgvQuestionnaire_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int QuestionnaireId = 0;
            try
            {
                QuestionnaireId = Convert.ToInt32(e.Row.Cells[1].Text.Trim());
            }
            catch
            {
                QuestionnaireId = 0;
            }
            if (QuestionnaireId > 0)
            {
                DropDownList ddlQuestionnaireAnswer = (e.Row.FindControl("ddlQuestionnaireAnswer") as DropDownList);
                TextBox txtQuestionnaireAnswer = (e.Row.FindControl("txtQuestionnaireAnswer") as TextBox);
                DataSet ds = new DataLayer().get_tbl_ProjectAnswer(QuestionnaireId);
                if (AllClasses.CheckDataSet(ds))
                {
                    ddlQuestionnaireAnswer.Visible = true;
                    txtQuestionnaireAnswer.Visible = false;
                    AllClasses.FillDropDown(ds.Tables[0], ddlQuestionnaireAnswer, "ProjectAnswer_Name", "ProjectAnswer_Id");
                }
                else
                {
                    ddlQuestionnaireAnswer.Visible = false;
                    txtQuestionnaireAnswer.Visible = true;
                }
            }
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (txtFundUtilized.Text.Trim() == "")
        {
            MessageBox.Show("Please Fill Total Funds Utilized..");
            return;
        }
        if (txtPhysicalProgress.Text.Trim() == "")
        {
            MessageBox.Show("Please Fill Physical Progress..");
            return;
        }
        string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (_data.Length != 4)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }

        int Package_Id = 0;
        try
        {
            Package_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            Package_Id = 0;
        }
        if (Package_Id == 0)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
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

        decimal total_Allocated = 0;
        try
        {
            total_Allocated = Convert.ToDecimal(_data[2]);
        }
        catch
        {
            total_Allocated = 0;
        }
        decimal Total_Expenditure = 0;
        try
        {
            Total_Expenditure = Convert.ToDecimal(_data[3]);
        }
        catch
        {
            Total_Expenditure = 0;
        }

        int PhysicalCompenentCurrentmonth = 0;
        int PhysicalCompenentCount = grdPhysicalProgress.Rows.Count;
        List<tbl_ProjectUC_PhysicalProgress> obj_tbl_ProjectUC_PhysicalProgress = new List<tbl_ProjectUC_PhysicalProgress>();
        for (int i = 0; i < grdPhysicalProgress.Rows.Count; i++)
        {

            tbl_ProjectUC_PhysicalProgress obj_tbl_ProjectUC_PhysicalProgress1 = new tbl_ProjectUC_PhysicalProgress();
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id = Convert.ToInt32(grdPhysicalProgress.Rows[i].Cells[0].Text.Trim());
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_Status = 1;
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_ProjectWork_Id = Work_Id;
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_ProjectPkg_Id = Package_Id;
            obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_FinancialYear_Id = 0;
            TextBox textBox = grdPhysicalProgress.Rows[i].FindControl("txtPhysicalCompenentCurrentmonth") as TextBox;
            try
            {
                obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_PhysicalProgress = Convert.ToInt32(textBox.Text);
                PhysicalCompenentCurrentmonth = PhysicalCompenentCurrentmonth + Convert.ToInt32(textBox.Text);
            }
            catch
            {
                obj_tbl_ProjectUC_PhysicalProgress1.ProjectUC_PhysicalProgress_PhysicalProgress = 0;
                PhysicalCompenentCurrentmonth = 0;
            }
            obj_tbl_ProjectUC_PhysicalProgress.Add(obj_tbl_ProjectUC_PhysicalProgress1);

        }

        List<tbl_ProjectUC_Deliverables> obj_tbl_ProjectUC_Deliverables = new List<tbl_ProjectUC_Deliverables>();
        for (int i = 0; i < grdDeliverables.Rows.Count; i++)
        {

            tbl_ProjectUC_Deliverables obj_tbl_ProjectUC_Deliverables1 = new tbl_ProjectUC_Deliverables();
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_Deliverables_Id = Convert.ToInt32(grdDeliverables.Rows[i].Cells[0].Text.Trim());
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_Status = 1;
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_ProjectWork_Id = Work_Id;
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_ProjectPkg_Id = Package_Id;
            obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_FinancialYear_Id = 0;
            TextBox textBox = grdDeliverables.Rows[i].FindControl("txtDeliverablesCurrentmonth") as TextBox;
            try
            {
                obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_Deliverables = Convert.ToInt32(textBox.Text);
            }
            catch
            {
                obj_tbl_ProjectUC_Deliverables1.ProjectUC_Deliverables_Deliverables = 0;
            }
            obj_tbl_ProjectUC_Deliverables.Add(obj_tbl_ProjectUC_Deliverables1);

        }

        if (PhysicalCompenentCurrentmonth > 0)
        {
            txtPhysicalProgress.Text = (Convert.ToInt32(PhysicalCompenentCurrentmonth / PhysicalCompenentCount)).ToString();
        }

        decimal BudgetUtilized = 0;
        decimal ProjectUC_Centage = 0;
        try
        {
            BudgetUtilized = decimal.Parse(txtFundUtilized.Text.Trim());
        }
        catch
        {
            BudgetUtilized = 0;
        }
        try
        {
            ProjectUC_Centage = decimal.Parse(txtFundCentage.Text.Trim());
        }
        catch
        {
            ProjectUC_Centage = 0;
        }
        if (BudgetUtilized == 0)
        {
            MessageBox.Show("Total Fund Utilized Till Date Should Not be Zero");
            return;
        }
        if (BudgetUtilized > total_Allocated)
        {
            MessageBox.Show("Total Fund Utilized Till Date Should Not be More than Total Release");
            return;
        }
        if (BudgetUtilized <= Total_Expenditure)
        {
            MessageBox.Show("Total Fund Utilized Till Date Should be More than Previous Expenditure Filled.");
            return;
        }

        decimal Physical_Progress_Last = 0;
        //if (grdUC.Rows.Count > 0)
        //{
        //    try
        //    {
        //        Physical_Progress_Last = Convert.ToDecimal(grdUC.Rows[grdUC.Rows.Count - 1].Cells[14].Text.Trim());
        //    }
        //    catch
        //    {
        //        Physical_Progress_Last = 0;
        //    }
        //}

        tbl_ProjectUC obj_tbl_ProjectUC = new tbl_ProjectUC();
        List<tbl_ProjectPkgSitePics> obj_tbl_ProjectPkgSitePics_Li = new List<tbl_ProjectPkgSitePics>();
        obj_tbl_ProjectUC.ProjectUC_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProjectUC.ProjectUC_BudgetUtilized = BudgetUtilized * 100000;
        obj_tbl_ProjectUC.ProjectUC_Centage = ProjectUC_Centage * 100000;
        obj_tbl_ProjectUC.ProjectUC_Total_Allocated = total_Allocated * 100000;
        try
        {
            obj_tbl_ProjectUC.ProjectUC_Achivment = (obj_tbl_ProjectUC.ProjectUC_BudgetUtilized * 100) / obj_tbl_ProjectUC.ProjectUC_Total_Allocated;
        }
        catch
        {
            obj_tbl_ProjectUC.ProjectUC_Achivment = 0;
        }
        obj_tbl_ProjectUC.ProjectUC_FinancialYear_Id = 0;
        obj_tbl_ProjectUC.ProjectUC_Comments = txtComments.Text.Trim();
        obj_tbl_ProjectUC.ProjectUC_ProjectPkg_Id = Package_Id;
        obj_tbl_ProjectUC.ProjectUC_ProjectWork_Id = Work_Id;
        obj_tbl_ProjectUC.ProjectUC_Status = 1;
        obj_tbl_ProjectUC.ProjectUC_SubmitionDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        try
        {
            obj_tbl_ProjectUC.ProjectUC_PhysicalProgress = Convert.ToInt32(txtPhysicalProgress.Text.Trim());
        }
        catch
        {
            obj_tbl_ProjectUC.ProjectUC_PhysicalProgress = 0;
        }
        if (obj_tbl_ProjectUC.ProjectUC_PhysicalProgress > 100)
        {
            MessageBox.Show("Physical Progress Percentage Should Not Be More Than 100.");
            return;
        }
        if (obj_tbl_ProjectUC.ProjectUC_PhysicalProgress < Physical_Progress_Last)
        {
            MessageBox.Show("Physical Progress Percentage Should More Than Last Filled Physical Progress.");
            return;
        }
        decimal Fund_Utilized = 0;
        Fund_Utilized = obj_tbl_ProjectUC.ProjectUC_BudgetUtilized - Total_Expenditure * 100000;
        if (Fund_Utilized <= 0)
        {
            MessageBox.Show("Fund Utilized Amount is not Proper.");
            return;
        }

        tbl_FinancialTrans obj_tbl_FinancialTrans = new tbl_FinancialTrans();
        obj_tbl_FinancialTrans.FinancialTrans_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_FinancialTrans.FinancialTrans_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        obj_tbl_FinancialTrans.FinancialTrans_EntryType = "Fund Utilized";
        obj_tbl_FinancialTrans.FinancialTrans_Status = 1;
        obj_tbl_FinancialTrans.FinancialTrans_Comments = txtComments.Text.Trim();
        //  obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = Convert.ToInt32(Session["FinancialYear_Id"].ToString());
        obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = 0;
        obj_tbl_FinancialTrans.FinancialTrans_Package_Id = Package_Id;
        obj_tbl_FinancialTrans.FinancialTrans_TransAmount = Fund_Utilized;
        obj_tbl_FinancialTrans.FinancialTrans_TransType = "D";
        obj_tbl_FinancialTrans.FinancialTrans_Work_Id = Work_Id;
        obj_tbl_FinancialTrans.FinancialTrans_FilePath1 = "";
        obj_tbl_FinancialTrans.FinancialTrans_GO_Date = obj_tbl_FinancialTrans.FinancialTrans_Date;
        obj_tbl_FinancialTrans.FinancialTrans_GO_Number = "";

        List<tbl_ProjectUC_Concent> obj_tbl_ProjectUC_Concent_Li = new List<tbl_ProjectUC_Concent>();

        for (int i = 0; i < dgvQuestionnaire.Rows.Count; i++)
        {
            DropDownList ddlQuestionnaireAnswer = (dgvQuestionnaire.Rows[i].FindControl("ddlQuestionnaireAnswer") as DropDownList);
            TextBox txtQuestionnaireAnswer = (dgvQuestionnaire.Rows[i].FindControl("txtQuestionnaireAnswer") as TextBox);
            if (ddlQuestionnaireAnswer.Visible && ddlQuestionnaireAnswer.SelectedValue == "0")
            {
                MessageBox.Show("Please Select Answer For Selected Questionire");
                ddlQuestionnaireAnswer.Focus();
                return;
            }
            if (txtQuestionnaireAnswer.Visible && txtQuestionnaireAnswer.Text.Trim() == "")
            {
                MessageBox.Show("Please Fill Answer For Selected Questionire");
                txtQuestionnaireAnswer.Focus();
                return;
            }
            tbl_ProjectUC_Concent obj_tbl_ProjectUC_Concent = new tbl_ProjectUC_Concent();
            obj_tbl_ProjectUC_Concent.ProjectUC_Concent_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            try
            {
                obj_tbl_ProjectUC_Concent.ProjectUC_Concent_Answer_Id = Convert.ToInt32(ddlQuestionnaireAnswer.SelectedValue);
            }
            catch
            {

            }
            obj_tbl_ProjectUC_Concent.ProjectUC_Concent_Questionire_Id = Convert.ToInt32(dgvQuestionnaire.Rows[i].Cells[1].Text.Trim());
            obj_tbl_ProjectUC_Concent.ProjectUC_Concent_Status = 1;
            obj_tbl_ProjectUC_Concent_Li.Add(obj_tbl_ProjectUC_Concent);
        }

        Dictionary<string, byte[]> file_Upload_Array_Pre = new Dictionary<string, byte[]>();
        if (Session["FileUpload1"] != null)
        {
            file_Upload_Array_Pre = (Dictionary<string, byte[]>)Session["FileUpload1"];
            foreach (KeyValuePair<string, byte[]> item in file_Upload_Array_Pre)
            {
                if (item.Value != null)
                {
                    tbl_ProjectPkgSitePics obj_tbl_ProjectPkgSitePics = new tbl_ProjectPkgSitePics();
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectPkg_Id = Package_Id;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectWork_Id = Work_Id;
                    //obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ReportSubmitted = ddlAuthority.SelectedItem.Text.Replace(":", "");
                    //try
                    //{
                    //    obj_tbl_ProjectDPRSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = Convert.ToInt32(ddlAuthority.SelectedValue);
                    //}
                    //catch
                    //{
                    //    obj_tbl_ProjectDPRSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = 0;
                    //}
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_Status = 1;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Bytes1 = item.Value;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1 = item.Key;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Type = "B";
                    obj_tbl_ProjectPkgSitePics_Li.Add(obj_tbl_ProjectPkgSitePics);
                }
            }
        }
        else
        {
            file_Upload_Array_Pre = null;
        }

        Dictionary<string, byte[]> file_Upload_Array_Post = new Dictionary<string, byte[]>();
        if (Session["FileUpload2"] != null)
        {
            file_Upload_Array_Post = (Dictionary<string, byte[]>)Session["FileUpload2"];
            foreach (KeyValuePair<string, byte[]> item in file_Upload_Array_Post)
            {
                if (item.Value != null)
                {
                    tbl_ProjectPkgSitePics obj_tbl_ProjectPkgSitePics = new tbl_ProjectPkgSitePics();
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectPkg_Id = Package_Id;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectWork_Id = Work_Id;
                    //obj_tbl_ProjectDPRSitePics.ProjectDPRSitePics_ReportSubmitted = ddlAuthority.SelectedItem.Text.Replace(":", "");
                    //try
                    //{
                    //    obj_tbl_ProjectDPRSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = Convert.ToInt32(ddlAuthority.SelectedValue);
                    //}
                    //catch
                    //{
                    //    obj_tbl_ProjectDPRSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = 0;
                    //}
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_Status = 1;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Bytes1 = item.Value;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1 = item.Key;
                    obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Type = "A";
                    obj_tbl_ProjectPkgSitePics_Li.Add(obj_tbl_ProjectPkgSitePics);
                }
            }
        }
        else
        {
            file_Upload_Array_Post = null;
        }
        if (file_Upload_Array_Pre == null || file_Upload_Array_Post == null)
        {
            MessageBox.Show("Please Upload Pre / Post Site Photos.");
            return;
        }
        if (file_Upload_Array_Pre.Count + file_Upload_Array_Post.Count == 0)
        {
            MessageBox.Show("Please Upload Pre / Post Site Photos.");
            return;
        }
        if (new DataLayer().Update_tbl_ProjectDPR_WorkStatus(obj_tbl_ProjectUC, obj_tbl_ProjectUC_Concent_Li, obj_tbl_ProjectUC_PhysicalProgress, obj_tbl_ProjectUC_Deliverables, obj_tbl_ProjectPkgSitePics_Li, obj_tbl_FinancialTrans))
        {
            MessageBox.Show("Work Physical & Financial Status Updated Successfully");
            btnSearch_Click(null, null);
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Unable To Update Work Status");
            return;
        }
    }
    private void reset()
    {
        divDPRUpload.Visible = false;
        txtFundUtilized.Text = "";
        txtComments.Text = "";
        hf_ProjectWork_Id.Value = "0|0|0";
    }

    protected void btnUC_Click(object sender, ImageClickEventArgs e)
    {
        //int UC_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[4].Text.Trim());

        //DataSet ds = new DataSet();
        //ds = new DataLayer().get_tbl_ProjectDPR_UC_Report(UC_Id);
        //if (AllClasses.CheckDataSet(ds))
        //{
        //    List<Report_UC> obj_Report_UC_Li = new List<Report_UC>();

        //    Report_UC obj_Report_UC = new Report_UC();
        //    string ULB_Type = "";
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NN")
        //    {
        //        ULB_Type = "नगर निगम ";
        //    }
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NPP")
        //    {
        //        ULB_Type = "नगर पालिका परिषद् ";
        //    }
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NP")
        //    {
        //        ULB_Type = "नगर पंचायत ";
        //    }
        //    try
        //    {
        //        obj_Report_UC.Budget_Approved = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectDPR_BudgetAllocated"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Budget_Approved = "0";
        //    }
        //    obj_Report_UC.Comments = ds.Tables[0].Rows[0]["ProjectUC_PhysicalProgress"].ToString();
        //    obj_Report_UC.Designation_1 = "अवर अभियन्ता";
        //    obj_Report_UC.Designation_1_Sub_Text = "स्थानीय निकाय";
        //    obj_Report_UC.Designation_2 = "अधीशासी अधिकारी";
        //    obj_Report_UC.Designation_2_Sub_Text = ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.Designation_3 = "अध्यक्ष";
        //    obj_Report_UC.Designation_3_Sub_Text = ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.District_Name_With_Caption = "जिला " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC.Expenditure_Percentage = ds.Tables[0].Rows[0]["ProjectUC_Achivment"].ToString();
        //    obj_Report_UC.GO_Date = ds.Tables[0].Rows[0]["ProjectWork_GO_Date"].ToString();
        //    obj_Report_UC.GO_No = ds.Tables[0].Rows[0]["ProjectWork_GO_No"].ToString();
        //    obj_Report_UC.GO_No_And_Date = "शाशानादेश संख्या: " + ds.Tables[0].Rows[0]["ProjectWork_GO_No"].ToString() + ", दिनांक: " + ds.Tables[0].Rows[0]["ProjectWork_GO_Date"].ToString();
        //    obj_Report_UC.Header_Text_Main = "कार्यालय " + ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString() + ", " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC.Header_Text_Sub = "उपयोगिता प्रमाण - पत्र";
        //    obj_Report_UC.Note_Text = "प्रमाणित किया जाता है की सन्दर्भित योजना / कार्यक्रम के अंतर्गत स्वीकुत धनराशि से प्रस्तावित / स्वीकुत कार्य शाशनादेश की शर्तो के अनुसार कराये गए है तथा इसमें विचलन नहीं किया गया है |";
        //    obj_Report_UC.Project_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();
        //    try
        //    {
        //        obj_Report_UC.Total_Expenditure = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectUC_BudgetUtilized"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Total_Expenditure = "0";
        //    }
        //    try
        //    {
        //        obj_Report_UC.Total_Released = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectUC_Total_Allocated"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Total_Released = "0";
        //    }
        //    obj_Report_UC.ULB_Name = ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.UC_File_Date = ds.Tables[0].Rows[0]["ProjectUC_SubmitionDate"].ToString();
        //    obj_Report_UC.ULB_Name_With_District = ds.Tables[0].Rows[0]["ULB_Name"].ToString() + ", " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC_Li.Add(obj_Report_UC);

        //    string filePath = "\\Downloads\\UC\\";
        //    if (!Directory.Exists(Server.MapPath(".") + filePath))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(".") + filePath);
        //    }

        //    string fileName = UC_Id.ToString() + ".pdf";
        //    string webURI = "";
        //    if (Page.Request.Url.Query.Trim() == "")
        //    {
        //        webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "") + filePath + fileName).Replace("\\", "/");
        //    }
        //    else
        //    {
        //        webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace(Page.Request.Url.Query, "") + filePath + fileName).Replace("\\", "/");
        //    }

        //    ReportDocument crystalReport = new ReportDocument();
        //    crystalReport.Load(Server.MapPath("~/Crystal/Upyogita_Pramad.rpt"));
        //    crystalReport.SetDataSource(obj_Report_UC_Li);
        //    crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath(".") + filePath + fileName);

        //    FileInfo fi = new FileInfo(Server.MapPath(".") + filePath + fileName);
        //    if (fi.Exists)
        //    {
        //        new AllClasses().Render_PDF_Document(ltEmbed, filePath + fileName);
        //        mp1.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Unable To Download File.");
        //        return;
        //    }
        //}
        //else
        //{
        //    MessageBox.Show("Unable To Generate UC.");
        //    return;
        //}
    }

    protected void grdUC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnUC = (e.Row.FindControl("btnUC") as ImageButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUC);
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