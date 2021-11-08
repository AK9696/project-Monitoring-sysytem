using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GoogleMaps.LocationServices;

public partial class Office_Order : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }

        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                int Invoice_Id = Convert.ToInt32(Request.QueryString["Invoice_Id"]);
                get_tbl_Deduction(Invoice_Id);
                get_Report_Cover_Details(Invoice_Id);
                get_OfficeOrder(Invoice_Id);
            }
            else
            {
                //MessageBox.Show("Work Details Not Found!!");
                //return;
            }
        }
    }

    protected void grdPost_PreRender(object sender, EventArgs e)
    {

    }

    protected void get_tbl_Deduction(int Invoice_Id)
    {
       
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_tbl_Deduction(Invoice_Id, 0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdDeductionHistory.DataSource = ds.Tables[0];
                grdDeductionHistory.DataBind();
            }
            else
            {
                grdDeductionHistory.DataSource = null;
                grdDeductionHistory.DataBind();
            }
        
    }

    protected void get_Report_Cover_Details(int Invoice_Id)
    {
        DataSet ds = new DataSet();
        
        ds = (new DataLayer()).get_Report_Cover_Details(Invoice_Id);
        if (AllClasses.CheckDataSet(ds))
        {
           
            try
            {
                lblCentralShare.Text = ds.Tables[0].Rows[0]["Central_Share"].ToString();
            }
            catch
            { }
            try
            {
                lblStateShare.Text = ds.Tables[0].Rows[0]["State_Share"].ToString();
            }
            catch
            { }
            try
            {
                lblULBShare.Text = ds.Tables[0].Rows[0]["ULB_Share"].ToString();
            }
            catch
            { }

            lblProjectName.Text = ds.Tables[0].Rows[0]["ProjectWork_Name"].ToString();
            lblProjectID.Text = ds.Tables[0].Rows[0]["ProjectWork_ProjectCode"].ToString();
            lblSector.Text = ds.Tables[0].Rows[0]["ProjectType_Name"].ToString();

            lblYear.Text = "2020";
            
            try
            {
                lblBalanceamount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement"].ToString();
            }
            catch
            {
                lblBalanceamount.Text = "0";
            }

            try
            {
                lblCentage.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Centage"].ToString();
            }

            catch
            {
                lblCentage.Text = "0";
            }
            lblActualCentageAmountreceived.Text = "0";
            try
            {
                lblExpendituredonebyImplementingAgency.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency"].ToString();
            }
            catch
            {
                lblExpendituredonebyImplementingAgency.Text = "0";
            }

            
            lblCity.Text = "";
            
            try
            {
                lblAmountReleasedToImplementingAgency.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_ReleaseTillDate"].ToString();
            }
            catch
            {
                lblAmountReleasedToImplementingAgency.Text = "0";
            }
            try
            {
                lblSanctionedAmountWithoutCentage.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_SanctionedAmount"].ToString();
            }
            catch
            {
                lblSanctionedAmountWithoutCentage.Text = "0";
            }
           
            try
            {
                lblTenderedAmount.Text = ds.Tables[0].Rows[0]["PackageInvoiceCover_TenderAmount"].ToString();
            }
            catch
            {
                lblTenderedAmount.Text = "0";
            }

           
        }
        else
        {
            MessageBox.Show("Unable To View Report");
            return;
        }
    }

    protected void get_OfficeOrder(int Invoice_Id)
    {

        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_OfficeOrder(Invoice_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                decimal Total = Convert.ToDecimal(ds.Tables[0].Rows[i]["CentralShare"].ToString()) + Convert.ToDecimal(ds.Tables[0].Rows[i]["StateShare"].ToString()) +
                    Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectWork_Centage"].ToString()) + Convert.ToDecimal(ds.Tables[0].Rows[i]["ULBShare"].ToString());
                ds.Tables[0].Rows[i]["Total"] = Total.ToString();
            }
            grdPostSecond.DataSource = ds.Tables[0];
            grdPostSecond.DataBind();
        }
        else
        {
            grdPostSecond.DataSource = null;
            grdPostSecond.DataBind();
        }
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[1];
            grdPost.DataBind();
        }
        else
        {
            grdPost.DataSource = null;
            grdPost.DataBind();
        }
    }
}