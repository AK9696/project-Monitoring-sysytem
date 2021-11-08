using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterDivision : System.Web.UI.Page
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
            get_tbl_Circle();
            get_tbl_Division();
        }
    }

    private void get_tbl_Circle()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlZoneMaster, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlZoneMaster.Items.Clear();
        }
    }

    private void get_tbl_Division()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(0);
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
        tbl_Division obj_tbl_Division = new tbl_Division();
        try
        {
            obj_tbl_Division.Division_Id = Convert.ToInt32(hf_Division_Id.Value);
        }
        catch
        {
            obj_tbl_Division.Division_Id = 0;
        }
        if (ddlZoneMaster.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Circle");
            ddlZoneMaster.Focus();
            return;
        }
        if (txtDivisionName.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Division Name");
            txtDivisionName.Focus();
            return;
        }
        obj_tbl_Division.Division_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        try
        {
            obj_tbl_Division.Division_Id = Convert.ToInt32(hf_Division_Id.Value);
        }
        catch
        {
            obj_tbl_Division.Division_Id = 0;
        }
        obj_tbl_Division.Division_Name= txtDivisionName.Text.Trim();
        obj_tbl_Division.Division_CircleId= Convert.ToInt32(ddlZoneMaster.SelectedValue);        
        obj_tbl_Division.Division_ModifiedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_Division.Division_Status = 1;
        string Msg = "";
        if ((new DataLayer()).Insert_tbl_Division(obj_tbl_Division, obj_tbl_Division.Division_Id, ref Msg))
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
        hf_Division_Id.Value = "0";
        txtDivisionName.Text = "";
        ddlZoneMaster.SelectedValue = "0";
        get_tbl_Division();
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
        hf_Division_Id.Value = (chk.Parent.Parent as GridViewRow).Cells[0].Text.Trim();
        GridViewRow grd = chk.Parent.Parent as GridViewRow;
        ddlZoneMaster.SelectedValue = grd.Cells[1].Text.Replace("&nbsp;", "").Trim();
        txtDivisionName.Text = grd.Cells[5].Text.Replace("&nbsp;", "").Trim();
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

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Division_Id_Delete = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        if (new DataLayer().Delete_Division(Division_Id_Delete, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            get_tbl_Division();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            return;
        }
    }
}