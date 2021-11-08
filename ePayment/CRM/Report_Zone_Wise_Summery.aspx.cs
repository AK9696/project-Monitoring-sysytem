using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_Zone_Wise_Summery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            get_tbl_Project();
            //get_Component_wise_Data();
        }
    }
    private void get_Zone_Wise_Analysis(int Scheme_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Zone_Wise_Analysis(Scheme_Id);
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

    protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlScheme.SelectedValue == "0")
        {
            grdPost.DataSource = null;
            grdPost.DataBind();
        }
        else
        {
            int Scheme_id = 0;
            Scheme_id = Convert.ToInt32(ddlScheme.SelectedValue);
            get_Zone_Wise_Analysis(Scheme_id);
        }
    }

    protected void lnkZone_Click(object sender, EventArgs e)
    {
        LinkButton lnkZone = sender as LinkButton;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        (lnkZone.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        GridViewRow gr = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Response.Redirect("Report_Circle_Wise_Summery.aspx?Zone_Id=" + gr.Cells[0].Text.Trim() + "&Scheme_Id=" + ddlScheme.SelectedValue);
    }
}