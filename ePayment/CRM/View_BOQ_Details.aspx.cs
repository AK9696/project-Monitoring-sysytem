using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_BOQ_Details : System.Web.UI.Page
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
            int Package_Id = 0;
            try
            {
                Package_Id = Convert.ToInt32(Request.QueryString["Package_Id"].ToString());
                get_tbl_PackageBOQ(Package_Id);
            }
            catch (Exception ex)
            {
                Package_Id = 0;
            }
        }
    }
    private void get_tbl_PackageBOQ(int Package_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageBOQ(0, Package_Id);

        if (ds != null && ds.Tables.Count > 0)
        {
            grdBOQ.DataSource = ds.Tables[0];
            grdBOQ.DataBind();
        }
        else
        {
            grdBOQ.DataSource =null;
            grdBOQ.DataBind();
        }
    }

    protected void grdBOQ_PreRender(object sender, EventArgs e)
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

    protected void grdBOQ_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSpecification = e.Row.FindControl("lblSpecification") as Label;
            lblSpecification.Text = lblSpecification.Text.Replace("\n", "<br />");
        }
    }
}