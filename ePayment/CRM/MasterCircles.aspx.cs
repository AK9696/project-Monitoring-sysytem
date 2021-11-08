using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterCircles : System.Web.UI.Page
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
            get_tbl_Zone();
            get_tbl_Circle();
        }
    }

    private void get_tbl_Zone()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlZoneMaster, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlZoneMaster.Items.Clear();
        }
    }

    private void get_tbl_Circle()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(0);
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        tbl_Circle obj_tbl_Circle = new tbl_Circle();
        try
        {
            obj_tbl_Circle.Circle_Id = Convert.ToInt32(hf_Circle_Id.Value);
        }
        catch
        {
            obj_tbl_Circle.Circle_Id = 0;
        }
        if (ddlZoneMaster.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            ddlZoneMaster.Focus();
            return;
        }
        if (txtCircleName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Circle Name");
            txtCircleName.Focus();
            return;
        }
        obj_tbl_Circle.Circle_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_tbl_Circle.Circle_Id = Convert.ToInt32(hf_Circle_Id.Value);
        }
        catch
        {
            obj_tbl_Circle.Circle_Id = 0;
        }
        obj_tbl_Circle.Circle_Name= txtCircleName.Text.Trim();
        obj_tbl_Circle.Circle_ZoneId= Convert.ToInt32(ddlZoneMaster.SelectedValue);        
        obj_tbl_Circle.Circle_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_Circle.Circle_Status = 1;
        string Msg = "";
        if ((new DataLayer()).Insert_tbl_Circle(obj_tbl_Circle, obj_tbl_Circle.Circle_Id, ref Msg))
        {
            MessageBox.Show("Vidhan Sabha Created Successfully!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Creating Vidhan Sabha!");
            return;
        }
    }

    private void reset()
    {
        hf_Circle_Id.Value = "0";
        txtCircleName.Text = "";
        ddlZoneMaster.SelectedValue = "0";
        get_tbl_Circle();
        divCreateNew.Visible = false;
    }
    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        ImageButton chk = sender as ImageButton;
        divCreateNew.Visible = true;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
         (chk.Parent.Parent as GridViewRow).BackColor = Color.LightGreen;
        hf_Circle_Id.Value = (chk.Parent.Parent as GridViewRow).Cells[0].Text.Trim();
        GridViewRow grd = chk.Parent.Parent as GridViewRow;
        ddlZoneMaster.SelectedValue = grd.Cells[1].Text.Replace("&nbsp;", "").Trim();
        txtCircleName.Text = grd.Cells[5].Text.Replace("&nbsp;", "").Trim();
        divCreateNew.Focus();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
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
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        reset();
        divCreateNew.Visible = true;
    }
}