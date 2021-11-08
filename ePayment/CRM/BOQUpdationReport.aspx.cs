using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BOQUpdationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            get_tbl_PackageBOQ_UodationReport_EmployeeWise();
        }
    }
    private void get_tbl_PackageBOQ_UodationReport_EmployeeWise()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageBOQ_UodationReport_EmployeeWise();
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

    private void get_tbl_PackageBOQ_UodationReport_ItemWise(int UpdatedBy)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_PackageBOQ_UodationReport_ItemWise(UpdatedBy);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPostItem.DataSource = ds.Tables[0];
            grdPostItem.DataBind();
            divdetails.Visible = true;
        }
        else
        {
            grdPostItem.DataSource = null;
            grdPostItem.DataBind();
            divdetails.Visible = false;
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
    protected void lnkZone_Click(object sender, EventArgs e)
    {
        LinkButton lnkZone = sender as LinkButton;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        (lnkZone.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        GridViewRow gr = ((sender as LinkButton).Parent.Parent as GridViewRow);
        int UpdateBy = Convert.ToInt32(gr.Cells[0].Text);
        get_tbl_PackageBOQ_UodationReport_ItemWise(UpdateBy);
    }

    protected void grdPostItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int Specification_Change = 0;
        int Unit_Change = 0;
        int QtyPaid_Change = 0;
        int QtyPaid_Percentage_Change = 0;
        try
        {
            Specification_Change = Convert.ToInt32(e.Row.Cells[0].Text.Trim());
        }
        catch
        {
            Specification_Change = 0;
        }
        try
        {
            Unit_Change = Convert.ToInt32(e.Row.Cells[1].Text.Trim());
        }
        catch
        {
            Unit_Change = 0;
        }
        try
        {
            QtyPaid_Change = Convert.ToInt32(e.Row.Cells[2].Text.Trim());
        }
        catch
        {
            QtyPaid_Change = 0;
        }
        try
        {
            QtyPaid_Percentage_Change = Convert.ToInt32(e.Row.Cells[3].Text.Trim());
        }
        catch
        {
            QtyPaid_Percentage_Change = 0;
        }

        if (Specification_Change == 1)
        {
            e.Row.Cells[5].BackColor = Color.LightSalmon;
        }
        if (Unit_Change == 1)
        {
            e.Row.Cells[6].BackColor = Color.LightSalmon;
        }
        if (QtyPaid_Change == 1)
        {
            e.Row.Cells[12].BackColor = Color.LightSalmon;
        }
        if (QtyPaid_Percentage_Change == 1)
        {
            e.Row.Cells[13].BackColor = Color.LightSalmon;
        }
    }
}