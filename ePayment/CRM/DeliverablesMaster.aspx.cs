using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class DeliverablesMaster : System.Web.UI.Page
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
            get_tbl_Unit();
            get_tbl_Deliverables();
        }
    }

    private void get_tbl_Deliverables()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deliverables();
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

    private void get_tbl_Unit()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Unit();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlUnit, "Unit_Name", "Unit_Id");
        }
        else
        {
            ddlUnit.Items.Clear();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Msg = "";
        tbl_Deliverables obj_tbl_Deliverables = new tbl_Deliverables();
        if (hf_Deliverables_Id.Value == "0" || hf_Deliverables_Id.Value == "")
        {
            obj_tbl_Deliverables.Deliverables_Id = 0;
        }
        else
        {
            obj_tbl_Deliverables.Deliverables_Id = Convert.ToInt32(hf_Deliverables_Id.Value);
        }
        obj_tbl_Deliverables.Deliverables_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (txtDeliverables.Text.Trim() == string.Empty)
        {
            Msg = "Give Deliverables";
            txtDeliverables.Focus();
            return;
        }
        if (ddlUnit.SelectedValue == null || ddlUnit.SelectedValue == "0")
        {
            Msg = "Select Unit";
            ddlUnit.Focus();
            return;
        }
        obj_tbl_Deliverables.Deliverables_Deliverables = txtDeliverables.Text.Trim();
        obj_tbl_Deliverables.Deliverables_Status = 1;
        obj_tbl_Deliverables.Deliverables_Unit_Id = Convert.ToInt32(ddlUnit.SelectedValue);

        if (new DataLayer().Insert_tbl_Deliverables(obj_tbl_Deliverables, obj_tbl_Deliverables.Deliverables_Id, ref Msg))
        {
            MessageBox.Show("Deliverables Created Successfully ! ");
            reset();
            get_tbl_Deliverables();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Deliverables Already Exist. Give another! ");
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
        txtDeliverables.Text = "";
        hf_Deliverables_Id.Value = "0";
        ddlUnit.SelectedValue = "0";
        get_tbl_Deliverables();
        mp1.Hide();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {

        int Deliverables_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_Deliverables_Id.Value = Deliverables_Id.ToString();
        txtDeliverables.Text = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[3].Text.Trim();
        try
        {
            ddlUnit.SelectedValue = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[1].Text.Trim();
        }
        catch
        {
        }
        btnDelete.Visible = true;
        mp1.Show();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Deliverables_Id = Convert.ToInt32(hf_Deliverables_Id.Value);
        if (new DataLayer().Delete_Deliverables(Deliverables_Id, Person_Id))
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
        txtDeliverables.Text = "";
        hf_Deliverables_Id.Value = "0";
        ddlUnit.SelectedValue = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}