using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class DashboardNew : System.Web.UI.Page
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
                    ddlZone.Enabled = false;
                }
                catch
                { }
            }


            get_Dashboard_Data();
        }
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        get_Dashboard_Data();
    }
    private void get_Dashboard_Data()
    {
        int Scheme_Id = 0;
        try
        {
            Scheme_Id = Convert.ToInt32(ddlScheme.SelectedValue);
        }
        catch
        {
            Scheme_Id = 0;
        }
        int Zone_Id = 0;
        try
        {
            Zone_Id = Convert.ToInt32(ddlZone.SelectedValue);
        }
        catch
        {
            Zone_Id = 0;
        }
        DataSet ds = new DataSet();
        if (rbtSearchBy.SelectedItem.Value == "1")
        {
            ds = (new DataLayer()).get_Dashboard_Data(Scheme_Id, Zone_Id, "", "");
        }
        else
        {
            ds = (new DataLayer()).get_Dashboard_Data(Scheme_Id, Zone_Id, txtDateFrom.Text, txtDateTill.Text);
        }

        if (ds != null)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblTotalULB.InnerHtml = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                lblTotalULB.InnerHtml = "0";
            }
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                lblTotalRunningProj.InnerHtml = ds.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                lblTotalRunningProj.InnerHtml = "0";
            }
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                lblBudgetAllocated.InnerHtml = ds.Tables[2].Rows[0][0].ToString() + " Cr.";
            }
            else
            {
                lblBudgetAllocated.InnerHtml = "0" + " Cr.";
            }
            if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                lblFundAllocated.InnerHtml = ds.Tables[3].Rows[0][0].ToString() + " Cr.";
                lblFundExpences.InnerHtml = ds.Tables[3].Rows[0][1].ToString() + " Cr.";
            }
            else
            {
                lblFundAllocated.InnerHtml = "0" + " Cr.";
                lblFundExpences.InnerHtml = "0" + " Cr.";
            }

            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                lblUC_Count.InnerHtml = ds.Tables[4].Rows[0]["Total_UC"].ToString();
                div_Phy_Pro.InnerHtml = "<div class='easy-pie-chart percentage' data-percent='" + ds.Tables[4].Rows[0]["Physical_Progress_Percentage"].ToString() + "' data-size='39' style='height: 39px; width: 39px; line-height: 38px;'><span class='percent'>" + ds.Tables[4].Rows[0]["Physical_Progress_Percentage"].ToString() + "</span> % <canvas height = '48' width='48' style='height: 39px; width: 39px;'></canvas></div>";
                decimal AVG_UC = 0;
                decimal Physical_Pro = 0;
                decimal Budget_UC = 0;
                try
                {
                    AVG_UC = decimal.Parse(ds.Tables[4].Rows[0]["AVG_UC_Percentage"].ToString());
                }
                catch
                {
                    AVG_UC = 0;
                }

                try
                {
                    Physical_Pro = decimal.Parse(ds.Tables[4].Rows[0]["Physical_Progress_Percentage"].ToString());
                }
                catch
                {
                    Physical_Pro = 0;
                }

                try
                {
                    Budget_UC = (decimal.Parse(ds.Tables[4].Rows[0]["BudgetUtilized"].ToString()) * 100) / (Convert.ToDecimal(lblBudgetAllocated.InnerHtml) * 100);
                }
                catch
                {
                    Budget_UC = 0;
                }
                //get_Graph_Data(AVG_UC, Physical_Pro, Budget_UC);
                //get_Graph_Data(20, 45, 60);
            }
            else
            {
                lblUC_Count.InnerHtml = "0";
                div_Phy_Pro.InnerHtml = "<div class='easy-pie-chart percentage' data-percent='0' data-size='39' style='height: 39px; width: 39px; line-height: 38px;'><span class='percent'>0</span> % <canvas height = '48' width='48' style='height: 39px; width: 39px;'></canvas></div>";
                //get_Graph_Data(0, 0, 0);
            }
        }
        else
        {

        }
        int ULB_Id = 0;
        string fromDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy").Replace("-", "/");
        string tillDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
        if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
        {
            //District_Id = Convert.ToInt32(Session["ULB_District_Id"].ToString());
            if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
            {//ULB
                ULB_Id = Convert.ToInt32(Session["ULB_Id"].ToString());
            }
        }
        // get_GO_Issued_Date_Wise(fromDate, tillDate, Scheme_Id, District_Id, ULB_Id, Convert.ToInt32(Session["FinancialYear_Id"].ToString()));
        //get_Budget_Detailed_Report(fromDate, tillDate, Scheme_Id, District_Id, ULB_Id, Convert.ToInt32(Session["FinancialYear_Id"].ToString()));
    }
    protected void rbtSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtSearchBy.SelectedItem.Value == "1")
        {
            divFromDate.Visible = false;
            divTillDate.Visible = false;
        }
        else if (rbtSearchBy.SelectedItem.Value == "3")
        {
            divFromDate.Visible = false;
            divTillDate.Visible = false;
            txtDateFrom.Text = Session["ServerDate"].ToString();
            txtDateTill.Text = Session["ServerDate"].ToString();
        }
        else
        {
            divFromDate.Visible = true;
            divTillDate.Visible = true;
        }
    }
}