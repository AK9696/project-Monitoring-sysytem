using System;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Report_Collection : System.Web.UI.Page
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
            int Zone_Id = 0;
            //if (Session["UserType"].ToString() == "2" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            //{//District
            //    District_Id = Convert.ToInt32(Session["ULB_District_Id"].ToString());
            //}
            //if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            //{
            //    District_Id = Convert.ToInt32(Session["ULB_District_Id"].ToString());
            //    if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
            //    {//ULB
            //        ULB_Id = Convert.ToInt32(Session["ULB_Id"].ToString());
            //    }
            //}
            get_Scheme_Wise_Details(Zone_Id);
        }
    }
    private void get_Scheme_Wise_Details(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Scheme_Wise_Details(Zone_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            
            grdPost.FooterRow.Cells[3].Text = ds.Tables[0].Compute("sum(Total_Work)", "").ToString();
            grdPost.FooterRow.Cells[4].Text = ds.Tables[0].Compute("sum(ProjectDPR_BudgetAllocated)", "").ToString();
            grdPost.FooterRow.Cells[5].Text = ds.Tables[0].Compute("sum(Fund_Released)", "").ToString();
            grdPost.FooterRow.Cells[6].Text = ds.Tables[0].Compute("sum(Expenditure)", "").ToString();
            grdPost.FooterRow.Cells[7].Text = ds.Tables[0].Compute("sum(Balance)", "").ToString();
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

    protected void lnkScheme_Click(object sender, EventArgs e)
    {
        int Scheme_Id = 0;
        Scheme_Id = Convert.ToInt32(((sender as LinkButton).Parent.Parent as GridViewRow).Cells[0].Text);
        Response.Redirect("Report_Collection_District.aspx?Scheme_Id=" + Scheme_Id.ToString());
    }

    protected void lnkSchemeF_Click(object sender, EventArgs e)
    {
        Response.Redirect("Report_Collection_District.aspx?Scheme_Id=0");
    }
}