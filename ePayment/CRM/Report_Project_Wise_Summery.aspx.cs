using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_Project_Wise_Summery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            get_tbl_Project();
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["Scheme_Id"] != null)
                {
                    ddlScheme.SelectedValue = Request.QueryString["Scheme_Id"].ToString();
                }

                if (Request.QueryString["Division_Id"] != null)
                {
                    get_Project_Wise_Analysis(Convert.ToInt32(Request.QueryString["Scheme_Id"].ToString()), Convert.ToInt32(Request.QueryString["Division_Id"].ToString()));
                }
            }
        }
    }
    private void get_Project_Wise_Analysis(int Scheme_Id, int Division_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_Project_Wise_Analysis(Scheme_Id, Division_Id);
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
            return;
        }
        else
        {
            int Scheme_id = 0;
            Scheme_id = Convert.ToInt32(ddlScheme.SelectedValue);
            int Division_Id = 0;
            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString()) > 0)
            {//Division
                Division_Id = Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString());
            }
            if (Request.QueryString["Division_Id"] != null)
            {
                Division_Id = Convert.ToInt32(Request.QueryString["Division_Id"].ToString());
            }
            get_Project_Wise_Analysis(Scheme_id, Division_Id);
        }
    }

    protected void lnkProject_Click(object sender, EventArgs e)
    {
        LinkButton lnkZone = sender as LinkButton;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        (lnkZone.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        GridViewRow gr = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Response.Redirect("Report_Package_Wise_Summery.aspx?ProjectWork_Id=" + gr.Cells[0].Text.Trim() + "&Scheme_Id=" + ddlScheme.SelectedValue);
    }
}