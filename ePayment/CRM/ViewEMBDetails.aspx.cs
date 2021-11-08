using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class ViewEMBDetails : System.Web.UI.Page
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
            int Invoice_Id = 0;
            if (Request.QueryString.Count > 0)
            {
                try
                {
                    Invoice_Id = Convert.ToInt32(Request.QueryString["Invoice_Id"].ToString());
                }
                catch
                {
                    Invoice_Id = 0;
                }
            }
            else
            {
                Invoice_Id = 0;

            }
            if (Invoice_Id > 0)
            {
                get_EMBDetailsInvoiceWise(Invoice_Id);
            }
            else
            {
                Response.Redirect("Dashboard.aspx");
            }
        }
    }


    public void get_EMBDetailsInvoiceWise(int Invoice_Id)
    {
        try
        {

            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_EMBDetailsInvoiceWise(Invoice_Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdEMB.DataSource = ds.Tables[0];
                grdEMB.DataBind();
            }
            else
            {
                grdEMB.DataSource = null;
                grdEMB.DataBind();
            }

        }
        catch(Exception ex)
        {

        }
    }
    protected void grdEMB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int PackageEMB_Master_Id = 0;
            try
            {
                PackageEMB_Master_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
            }
            catch
            {
                PackageEMB_Master_Id = 0;
            }

            int Package_Id = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
            GridView grdEMBItems = e.Row.FindControl("grdEMBItems") as GridView;
            DataSet ds = new DataSet();
            ds = (new DataLayer()).get_EMBItemsDetailsInvoiceWise(PackageEMB_Master_Id);
            if (AllClasses.CheckDataSet(ds))
            {
                grdEMBItems.DataSource = ds.Tables[0];
                grdEMBItems.DataBind();
            }
            else
            {
                grdEMBItems.DataSource = null;
                grdEMBItems.DataBind();
            }
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

    protected void grdEMBItems_PreRender(object sender, EventArgs e)
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
}