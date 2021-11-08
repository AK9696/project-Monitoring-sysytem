using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketDetails : System.Web.UI.Page
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
            get_M_Jurisdiction();
            if (Session["UserType"].ToString() == "2" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {//District
                try
                {
                    ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    ddlDistrict.Enabled = false;
                }
                catch
                { }
            }
            btnSearch_Click(null, null);
        }
    }
    private void get_M_Jurisdiction()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_M_Jurisdiction(3, 0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtResult = new DataTable();
                if (Session["UserType"].ToString() == "3")
                {
                    if (Session["User_Access"] != null)
                    {
                        DataSet dsAccess = (DataSet)Session["User_Access"];
                        dtResult = AllClasses.FilterDT(ds.Tables[0], dsAccess.Tables[0], "M_Jurisdiction_Id", "District_Id", "Jurisdiction_Name_Eng");
                    }
                    else
                    {
                        dtResult = ds.Tables[0];
                    }
                }
                else
                {
                    dtResult = ds.Tables[0];
                }

                AllClasses.FillDropDown(dtResult, ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
                if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
                {
                    ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                    ddlDistrict_SelectedIndexChanged(ddlDistrict, null);
                }
            }
            else
            {
                ddlDistrict.Items.Clear();
            }
        }
        catch
        {

        }
    }
    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        int Person_Id = 0;
        if (Session["UserType"].ToString() == "4")
        {
            Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        }
        else
        {
            Person_Id = 0;
        }
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlProjectMaster, "Project_Name", "Project_Id");
        }
        else
        {
            ddlProjectMaster.Items.Clear();
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedValue == "0")
        {
            ddlULB.Items.Clear();
        }
        else
        {
            get_tbl_ULB(Convert.ToInt32(ddlDistrict.SelectedValue));
        }
    }

    private void get_tbl_ULB(int District_Id)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_ULB(District_Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtResult = new DataTable();
                if (Session["UserType"].ToString() == "3")
                {
                    if (Session["User_Access"] != null)
                    {
                        DataSet dsAccess = (DataSet)Session["User_Access"];
                        dtResult = AllClasses.FilterDT(ds.Tables[0], dsAccess.Tables[1], "ULB_Id", "ULB_Id", "ULB_Name");
                    }
                    else
                    {
                        dtResult = ds.Tables[0];
                    }
                }
                else
                {
                    dtResult = ds.Tables[0];
                }

                AllClasses.FillDropDown(dtResult, ddlULB, "ULB_Name", "ULB_Id");
                if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
                {
                    try
                    {
                        ddlULB.SelectedValue = Session["ULB_Id"].ToString();
                    }
                    catch
                    { }
                }
            }
            else
            {
                ddlULB.Items.Clear();
            }
        }
        catch
        {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int District_Id = 0;
        int ULB_Id = 0;
        int Project_Id = 0;
        int Person_Id = 0;
        if (Session["UserType"].ToString() != "1")
        {
            Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        }
        try
        {
            District_Id = Convert.ToInt32(ddlDistrict.SelectedValue);
        }
        catch
        { }

        try
        {
            ULB_Id = Convert.ToInt32(ddlULB.SelectedValue);
        }
        catch
        { }

        try
        {
            Project_Id = Convert.ToInt32(ddlProjectMaster.SelectedValue);
        }
        catch
        { }

        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_TicketDetails(District_Id, ULB_Id, Project_Id, Person_Id, txtDateFrom.Text, txtTillDate.Text);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
        }
        else
        {
            grdPost.DataSource = null;
            grdPost.DataBind();
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

    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTicketCloseDescription.Text.Trim() == string.Empty)
        {
            MessageBox.Show("Give Description");
            txtTicketCloseDescription.Focus();
            return;
        }

        int TicketCategory_Id = Convert.ToInt32(hf_TicketDetails_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Update_TicketDetails(TicketCategory_Id, Person_Id, txtTicketCloseDescription.Text))
        {
            MessageBox.Show("Update Successfully!!");
            btnSearch_Click(null, null);
            mp1.Hide();
            return;
        }
        else
        {
            MessageBox.Show("Error In Updation!!");
            mp1.Hide();
            return;
        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        mp1.Hide();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int TicketCategory_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_TicketDetails_Id.Value = TicketCategory_Id.ToString();
        txtTicketCloseDescription.Text = "";
        mp1.Show();
    }
}