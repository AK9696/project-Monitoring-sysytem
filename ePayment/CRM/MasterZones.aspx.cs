using Obout.Ajax.UI.HTMLEditor;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterZones : System.Web.UI.Page
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
        }
    }

    private void get_tbl_Zone()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
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
        string Msg = "";
        if (txtZone.Text.Trim() == string.Empty)
        {
            Msg = "Give Zone";
            txtZone.Focus();
            return;
        }
        tbl_Zone obj_tbl_Zone = new tbl_Zone();
        if (hf_Zone_Id.Value == "0" || hf_Zone_Id.Value == "")
        {
            obj_tbl_Zone.Zone_Id = 0;
        }
        else
        {
            obj_tbl_Zone.Zone_Id = Convert.ToInt32(hf_Zone_Id.Value);
        }
        obj_tbl_Zone.Zone_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_Zone.Zone_Name = txtZone.Text.Trim();
        obj_tbl_Zone.Zone_Status = 1;

        if (obj_tbl_Zone == null)
        {
            MessageBox.Show(Msg);
            return;
        }
        if (new DataLayer().Insert_tbl_Zone(obj_tbl_Zone, obj_tbl_Zone.Zone_Id, ref Msg))
        {
            MessageBox.Show("Zone Created Successfully ! ");
            reset();
            get_tbl_Zone();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Zone Already Exist. Give another! ");
            }
            else
            {
                MessageBox.Show("Error ! ");
            }
            return;
        }
    }

    private void reset()
    {
        txtZone.Text = "";
        hf_Zone_Id.Value = "0";
        get_tbl_Zone();
        divCreateNew.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int Zone_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_Zone_Id.Value = Zone_Id.ToString();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).Edit_tbl_Zone(Zone_Id.ToString());
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtZone.Text = ds.Tables[0].Rows[0]["Zone_Name"].ToString();
        }
        divCreateNew.Visible = true;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Zone_Id = Convert.ToInt32(hf_Zone_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Delete_Zone(Zone_Id, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            reset();
            return;
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
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        btnDelete.Visible = false;
        txtZone.Text = "";
        hf_Zone_Id.Value = "0";
        divCreateNew.Visible = true;
    }
}
