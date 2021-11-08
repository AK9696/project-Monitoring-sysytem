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

public partial class InspectionTeamCreation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            get_tbl_Zone();
            get_tbl_ZoneS();
            get_tbl_Project();
            get_tbl_SetInspectionMaster();
            get_tbl_Designation();
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
            get_Employee();
        }
    }

    private void get_Employee()
    {
        DataSet ds = new DataSet();
        int Zone_Id = 0;
        try
        {
            Zone_Id = Convert.ToInt32(ddlZoneS.SelectedValue);
        }
        catch
        {
            Zone_Id = 0;
        }
        int Circle_Id = 0;
        try
        {
            Circle_Id = Convert.ToInt32(ddlCircleS.SelectedValue);
        }
        catch
        {
            Circle_Id = 0;
        }
        int Division_Id = 0;
        try
        {
            Division_Id = Convert.ToInt32(ddlDivisionS.SelectedValue);
        }
        catch
        {
            Division_Id = 0;
        }
        int Scheme_Id = 0;
        try
        {
            Scheme_Id = Convert.ToInt32(ddlSearchScheme.SelectedValue);
        }
        catch
        {
            Scheme_Id = 0;
        }
        string UserTypeId = "1, 2, 4, 6, 7, 8";
        ds = (new DataLayer()).get_Employee(UserTypeId, 0, 0, 0, Zone_Id, Circle_Id, Division_Id, 0, ddlDesignation.SelectedValue, "", 0, 0, Scheme_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].NewRow();

            dr["Person_Name_Mobile"] = "--Select--";
            dr["Person_Id"] = "0";
            ds.Tables[0].Rows.InsertAt(dr, 0);

            dr = ds.Tables[0].NewRow();

            dr["Person_Name_Mobile"] = "<< ADD NEW INSPECTION MEMBER >>";
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
            dr["Person_Name_Mobile"] = "<< ADD NEW INSPECTION MEMBER >>";
            dr["Person_Id"] = "-1";
            ds.Tables[0].Rows.Add(dr);

            ddlPersonVendor.DataTextField = "Person_Name_Mobile";
            ddlPersonVendor.DataValueField = "Person_Id";
            ddlPersonVendor.DataSource = ds.Tables[0];
            ddlPersonVendor.DataBind();

            divAddVendor.Visible = true;
        }
    }
    private void get_tbl_SetInspectionMaster()
    {
        DataSet ds = new DataSet();

        ds = (new DataLayer()).get_tbl_SetInspectionMaster();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlSetName, "SetInspectionMaster_Name", "SetInspectionMaster_Id");
        }
        else
        {
            ddlSetName.Items.Clear();
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
        }
        else
        {
            ddlDesignation.Items.Clear();
        }
    }

    private void get_tbl_ZoneS()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlZoneS, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlZoneS.Items.Clear();
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
    private void get_tbl_CircleS(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(Zone_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlCircleS, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlCircleS.Items.Clear();
        }
    }
    private void get_tbl_DivisionS(int Circle_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(Circle_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDivisionS, "Division_Name", "Division_Id");
        }
        else
        {
            ddlDivisionS.Items.Clear();
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
            ddlZoneS.SelectedValue = ddlZone.SelectedValue;
            ddlZoneS_SelectedIndexChanged(null, null);
            get_tbl_Circle(Convert.ToInt32(ddlZone.SelectedValue));
        }
    }
    protected void ddlZoneS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlZoneS.SelectedValue == "0")
        {
            ddlCircleS.Items.Clear();
            ddlDivisionS.Items.Clear();
        }
        else
        {
            get_tbl_CircleS(Convert.ToInt32(ddlZoneS.SelectedValue));
            get_Employee();
        }
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        get_Employee();
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
    protected void ddlCircleS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCircleS.SelectedValue == "0")
        {
            ddlDivisionS.Items.Clear();
        }
        else
        {
            get_tbl_DivisionS(Convert.ToInt32(ddlCircleS.SelectedValue));
            get_Employee();
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

        get_tbl_ProjectDPRInspectionInfo(Package_Id.ToString(), PackageWork_Id.ToString());
    }
    private void get_tbl_ProjectDPRInspectionInfo(string projectDPR_Id, string work_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectDPRInspectionInfo(projectDPR_Id, work_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            grdComiteeMember.DataSource = ds.Tables[0];
            grdComiteeMember.DataBind();
            btnAdd.Visible = false;
            btnUpload.Visible = false;
        }
        else
        {
            grdComiteeMember.DataSource = null;
            grdComiteeMember.DataBind();
            btnAdd.Visible = false;
            btnUpload.Visible = false;
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


        List<tbl_PersonDetail_Temp> obj_tbl_PersonDetail_Li = (List<tbl_PersonDetail_Temp>)ViewState["tbl_PersonDetail"];
        if (obj_tbl_PersonDetail_Li == null)
        {
            obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail_Temp>();
        }
        if (obj_tbl_PersonDetail_Li.Count == 0)
        {
            MessageBox.Show("Please Add Atleast One Comitee Member..");
            return;
        }
        string msg = "";
        if (new DataLayer().Update_tbl_ProjectDPR_InspectionStatus(obj_tbl_PersonDetail_Li, ProjectPkg_Id, Work_Id, ref msg))
        {
            if (msg == "")
            {
                MessageBox.Show("Comitee Member Details Updated Successfully");
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
            MessageBox.Show("Unable To Update Comitee Member Details");
            return;
        }
    }
    private void reset()
    {
        divDPRUpload.Visible = false;
        txtComments.Text = "";
        hf_ProjectWork_Id.Value = "0|0|0";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        List<tbl_PersonDetail_Temp> obj_tbl_PersonDetail_Li = (List<tbl_PersonDetail_Temp>)ViewState["tbl_PersonDetail"];
        if (obj_tbl_PersonDetail_Li == null)
        {
            obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail_Temp>();
        }
        if (ddlPersonVendor.SelectedValue == "-1")
        {
            if (txtComiteeName.Text.Trim() == "")
            {
                MessageBox.Show("Please Give Comitee Member Name");
                return;
            }
            if (txtComiteeMobile1.Text.Trim() == "")
            {
                MessageBox.Show("Please Give Comitee Member Mobile No");
                return;
            }
            if (txtDesignation.Text.Trim() == "")
            {
                MessageBox.Show("Please Give Comitee Member Designation");
                return;
            }
            tbl_PersonDetail_Temp obj_tbl_PersonDetail = new tbl_PersonDetail_Temp();
            obj_tbl_PersonDetail.Person_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PersonDetail.Person_BranchOffice_Id = 1;
            obj_tbl_PersonDetail.Person_Mobile1 = txtComiteeMobile1.Text.Trim();
            obj_tbl_PersonDetail.Person_Mobile2 = txtComiteeMobile2.Text.Trim();
            obj_tbl_PersonDetail.Person_Name = txtComiteeName.Text.Trim();
            obj_tbl_PersonDetail.Person_Status = 1;
            obj_tbl_PersonDetail.Designation_Id = 0;
            obj_tbl_PersonDetail.Designation_Name = txtDesignation.Text;
            obj_tbl_PersonDetail.Zone_Id = Convert.ToInt32(ddlZoneS.SelectedValue);
            obj_tbl_PersonDetail.Zone_Name = ddlZoneS.SelectedItem.Text;
            obj_tbl_PersonDetail.Circle_Id = Convert.ToInt32(ddlCircleS.SelectedValue);
            obj_tbl_PersonDetail.Circle_Name = ddlCircleS.SelectedItem.Text;
            obj_tbl_PersonDetail.Division_Id = Convert.ToInt32(ddlDivisionS.SelectedValue);
            obj_tbl_PersonDetail.Division_Name = ddlDivisionS.SelectedItem.Text;
            obj_tbl_PersonDetail_Li.Add(obj_tbl_PersonDetail);
            ViewState["tbl_PersonDetail"] = obj_tbl_PersonDetail_Li;
            txtComiteeMobile1.Text = "";
            txtComiteeMobile2.Text = "";
            txtComiteeName.Text = "";
            txtDesignation.Text = "";
        }
        else
        {
            if (ddlPersonVendor.SelectedValue == "0")
            {
                MessageBox.Show("Please Select Comitee Member");
                return;
            }
            if (ddlZoneS.SelectedValue == "0")
            {
                MessageBox.Show("Please Select Zone");
                return;
            }
            tbl_PersonDetail_Temp obj_tbl_PersonDetail = new tbl_PersonDetail_Temp();
            obj_tbl_PersonDetail.Person_Name = ddlPersonVendor.SelectedItem.Text;
            obj_tbl_PersonDetail.Person_Id = Convert.ToInt32(ddlPersonVendor.SelectedValue);
            try
            {
                obj_tbl_PersonDetail.Designation_Id = Convert.ToInt32(ddlDesignation.SelectedValue);
            }
            catch
            {
                obj_tbl_PersonDetail.Designation_Id = 0;

            }
            try
            {
                if (ddlDesignation.SelectedValue == "0")
                {
                    obj_tbl_PersonDetail.Designation_Name = "";
                }
                else
                {

                    obj_tbl_PersonDetail.Designation_Name = ddlDesignation.SelectedItem.Text;
                }

            }
            catch
            {
                obj_tbl_PersonDetail.Designation_Name = "";

            }
            try
            {
                obj_tbl_PersonDetail.Zone_Id = Convert.ToInt32(ddlZoneS.SelectedValue);
            }
            catch
            {
                obj_tbl_PersonDetail.Zone_Id = 0;

            }
            try
            {
                if (ddlZoneS.SelectedValue == "0")
                {
                    obj_tbl_PersonDetail.Zone_Name = "";
                }
                else
                {

                    obj_tbl_PersonDetail.Zone_Name = ddlZoneS.SelectedItem.Text;
                }

            }
            catch
            {
                obj_tbl_PersonDetail.Zone_Name = "";

            }
            try
            {
                obj_tbl_PersonDetail.Circle_Id = Convert.ToInt32(ddlCircleS.SelectedValue);
            }
            catch
            {
                obj_tbl_PersonDetail.Circle_Id = 0;

            }
            try
            {
                if (ddlCircleS.SelectedValue == "0")
                {
                    obj_tbl_PersonDetail.Circle_Name = "";
                }
                else
                {

                    obj_tbl_PersonDetail.Circle_Name = ddlCircleS.SelectedItem.Text;
                }


            }
            catch
            {
                obj_tbl_PersonDetail.Circle_Name = "";

            }
            try
            {
                obj_tbl_PersonDetail.Division_Id = Convert.ToInt32(ddlDivisionS.SelectedValue);
            }
            catch
            {
                obj_tbl_PersonDetail.Division_Id = 0;

            }
            try
            {
                if (ddlDivisionS.SelectedValue == "0")
                {
                    obj_tbl_PersonDetail.Division_Name = "";
                }
                else
                {

                    obj_tbl_PersonDetail.Division_Name = ddlDivisionS.SelectedItem.Text;
                }

            }
            catch
            {
                obj_tbl_PersonDetail.Division_Name = "";

            }

            obj_tbl_PersonDetail_Li.Add(obj_tbl_PersonDetail);
            ViewState["tbl_PersonDetail"] = obj_tbl_PersonDetail_Li;
            ddlPersonVendor.SelectedValue = "0";
        }
        grdComiteeMember.DataSource = obj_tbl_PersonDetail_Li;
        grdComiteeMember.DataBind();
    }
    protected void btnRemove_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = ((sender as ImageButton).Parent.Parent as GridViewRow);
        List<tbl_PersonDetail_Temp> obj_tbl_PersonDetail_Li = (List<tbl_PersonDetail_Temp>)ViewState["tbl_PersonDetail"];
        if (obj_tbl_PersonDetail_Li == null)
        {
            obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail_Temp>();
        }
        if (obj_tbl_PersonDetail_Li.Count > 0)
        {
            obj_tbl_PersonDetail_Li.RemoveAt(gr.RowIndex);

            ViewState["tbl_PersonDetail"] = obj_tbl_PersonDetail_Li;
            grdComiteeMember.DataSource = obj_tbl_PersonDetail_Li;
            grdComiteeMember.DataBind();
        }
        else
        {

        }
    }
    protected void btnChooseSet_Click(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {

            MessageBox.Show("Please Select Zone");
            return;
        }
        ddlSetName.SelectedValue = "0";
        gvSetDetails.DataSource = null;
        gvSetDetails.DataBind();
        mp3.Show();
    }
    protected void ddlSetName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = (new DataLayer()).get_tbl_SetInspectionMaster_BySetName(ddlSetName.SelectedItem.Text);
        if (AllClasses.CheckDataSet(ds))
        {
            gvSetDetails.DataSource = ds.Tables[0];
            gvSetDetails.DataBind();
        }
        else
        {
            gvSetDetails.DataSource = null;
            gvSetDetails.DataBind();
        }
        mp3.Show();
    }
    protected void btnAddSet_Click(object sender, EventArgs e)
    {
        if (ddlSetName.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Set Name");
            mp3.Show();
            return;
        }

        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;


        DataSet ds = new DataSet();

        ds = (new DataLayer()).get_tbl_SetInspectionMaster_BySetName(ddlSetName.SelectedItem.Text);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["Level_Name"].ToString() == "Zone")
                {
                    try
                    {
                        Zone_Id = Convert.ToInt32(ddlZone.SelectedValue);
                    }
                    catch
                    {
                        Zone_Id = 0;
                    }
                }
                else if (ds.Tables[0].Rows[i]["Level_Name"].ToString() == "Circle")
                {
                    try
                    {
                        Circle_Id = Convert.ToInt32(ddlCircle.SelectedValue);
                    }
                    catch
                    {
                        Circle_Id = 0;
                    }
                }
                else if (ds.Tables[0].Rows[i]["Level_Name"].ToString() == "Division")
                {
                    try
                    {
                        Division_Id = Convert.ToInt32(ddlDivision.SelectedValue);
                    }
                    catch
                    {
                        Division_Id = 0;
                    }
                }

            }
        }
        List<tbl_PersonDetail_Temp> obj_tbl_PersonDetail_Li = (List<tbl_PersonDetail_Temp>)ViewState["tbl_PersonDetail"];
        if (obj_tbl_PersonDetail_Li == null)
        {
            obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail_Temp>();
        }
        string UserTypeId = "1, 2, 4, 6, 7, 8";
        ds = (new DataLayer()).get_Employee_BySetInspection(UserTypeId, Zone_Id, Circle_Id, Division_Id, ddlSetName.SelectedItem.Text);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tbl_PersonDetail_Temp obj_tbl_PersonDetail = new tbl_PersonDetail_Temp();
                obj_tbl_PersonDetail.Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                try
                {
                    obj_tbl_PersonDetail.Person_BranchOffice_Id = Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString());
                }
                catch
                { }
                obj_tbl_PersonDetail.Person_Mobile1 = ds.Tables[0].Rows[i]["Person_Mobile1"].ToString();
                obj_tbl_PersonDetail.Person_Mobile2 = ds.Tables[0].Rows[i]["Person_Mobile2"].ToString();
                obj_tbl_PersonDetail.Person_Name = ds.Tables[0].Rows[i]["Person_Name"].ToString();
                obj_tbl_PersonDetail.Person_Status = 1;
                try
                {
                    obj_tbl_PersonDetail.Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Designation_Id"].ToString());
                }
                catch
                { }
                obj_tbl_PersonDetail.Designation_Name = ds.Tables[0].Rows[i]["Designation_Name"].ToString();
                try
                {
                    obj_tbl_PersonDetail.Zone_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Zone_Id"].ToString());
                }
                catch
                { }
                obj_tbl_PersonDetail.Zone_Name = ds.Tables[0].Rows[i]["Zone_Name"].ToString();
                try
                {
                    obj_tbl_PersonDetail.Circle_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Circle_Id"].ToString());
                }
                catch
                { }
                obj_tbl_PersonDetail.Circle_Name = ds.Tables[0].Rows[i]["Circle_Name"].ToString();
                try
                {
                    obj_tbl_PersonDetail.Division_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Division_Id"].ToString());
                }
                catch
                { }
                obj_tbl_PersonDetail.Division_Name = ds.Tables[0].Rows[i]["Division_Name"].ToString();
                obj_tbl_PersonDetail_Li.Add(obj_tbl_PersonDetail);
            }

            ViewState["tbl_PersonDetail"] = obj_tbl_PersonDetail_Li;

            grdComiteeMember.DataSource = obj_tbl_PersonDetail_Li;
            grdComiteeMember.DataBind();
        }
        mp3.Hide();

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