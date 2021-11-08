using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterDPR_Upload : System.Web.UI.Page
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
            Session["FileName"] = null;
            Session["FileBytes"] = null;
            Session["FileName1"] = null;
            Session["FileBytes1"] = null;
            Session["FileName3"] = null;
            Session["FileBytes3"] = null;
            Session["FileName4"] = null;
            Session["FileBytes4"] = null;
            //AllClasses.Create_Directory_Session3(1);
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
        try
        {
            tbl_ProjectDPR obj_tbl_ProjectDPR = new tbl_ProjectDPR();
            try
            {
                obj_tbl_ProjectDPR.ProjectDPR_ProjectWork_Id = Convert.ToInt32(hf_ProjectWork_Id.Value);
            }
            catch
            {
                obj_tbl_ProjectDPR.ProjectDPR_ProjectWork_Id = 0;
            }
            try
            {
                obj_tbl_ProjectDPR.ProjectDPR_Project_Id = Convert.ToInt32(hf_Project_Id.Value);
            }
            catch
            {
                obj_tbl_ProjectDPR.ProjectDPR_Project_Id = 0;
            }
            try
            {
                obj_tbl_ProjectDPR.ProjectDPR_ProjectWorkPkg_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
            }
            catch
            {
                obj_tbl_ProjectDPR.ProjectDPR_ProjectWorkPkg_Id = 0;
            }
            obj_tbl_ProjectDPR.ProjectDPR_Status = 1;
            obj_tbl_ProjectDPR.ProjectDPR_AddedBy= Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectDPR.ProjectDPR_Comments = txtComments.Text;
            if (Session["FileBytes"] != null)
            {
                obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Bytes = (Byte[])Session["FileBytes"];
                obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Extention = Session["FileName"].ToString();
                
            }
            else
            {
                MessageBox.Show("Please DPR file Uploaded");
                return;
            }
            if (Session["FileBytes1"] != null)
            {
                obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Bytes = (Byte[])Session["FileBytes1"];
                obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Extention = Session["FileName1"].ToString();

            }
            else
            {
                MessageBox.Show("Please Design Document Uploaded");
                return;
            }
            if (Session["FileBytes3"] != null)
            {
                obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Bytes = (Byte[])Session["FileBytes3"];
                obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Extention = Session["FileName3"].ToString();

            }
            //else
            //{
            //    MessageBox.Show("Please Design Document Uploaded");
            //    return;
            //}
            if (Session["FileBytes4"] != null)
            {
                obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Bytes = (Byte[])Session["FileBytes4"];
                obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Extention = Session["FileName4"].ToString();

            }
            //else
            //{
            //    MessageBox.Show("Please Design Document Uploaded");
            //    return;
            //}
            if ((new DataLayer()).UpdateApproval_tbl_ProjectDPR(obj_tbl_ProjectDPR))
            {
                MessageBox.Show("DPR Approved Successfully!");
                Session["FileName"] = null;
                Session["FileBytes"] = null;
                Session["FileName1"] = null;
                Session["FileBytes1"] = null;
                Session["FileName3"] = null;
                Session["FileBytes3"] = null;
                Session["FileName4"] = null;
                Session["FileBytes4"] = null;
                btnSearch_Click(null, null);
                return;
            }
            else
            {
                MessageBox.Show("Error In DPR Approved!");
                return;
            }
        }
        catch
        {

        }
    }

    private void reset()
    {
        //ddlDistrict.SelectedValue = "0";
        Session["FileName"] = null;
        Session["FileBytes"] = null;
        Session["FileName1"] = null;
        Session["FileBytes1"] = null;
        Session["FileName3"] = null;
        Session["FileBytes3"] = null;
        Session["FileName4"] = null;
        Session["FileBytes4"] = null;
        hf_ProjectWork_Id.Value = "0";
        hf_Project_Id.Value = "0";
        hf_ProjectWorkPkg_Id.Value = "0";
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0,Zone_Id, Circle_Id, Division_Id, 0,"","",false, "DPRUploadNew");
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
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Work_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        int Project_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        divEntry.Visible = true;
        hf_ProjectWork_Id.Value = gr.Cells[1].Text.Trim();
        hf_Project_Id.Value = gr.Cells[2].Text.Trim();
        hf_ProjectWorkPkg_Id.Value = gr.Cells[0].Text.Trim();
       // int Division_Id = Convert.ToInt32(gr.Cells[4].Text.Trim());
    }
}