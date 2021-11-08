using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdditionalDepartmentPaymentItem : System.Web.UI.Page
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
            get_tbl_ADP_Category();
            get_tbl_Unit();
            get_tbl_ADP_Item();
        }
    }

    private void get_tbl_ADP_Category()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ADP_Category();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlCategory, "ADP_Category_Name", "ADP_Category_Id");
        }
        else
        {
            ddlCategory.Items.Clear();
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
    private void get_tbl_ADP_Item()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ADP_Item();
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

        tbl_ADP_Item obj_tbl_ADP_Item = new tbl_ADP_Item();
        if (hf_ADP_Category_Id.Value == "0" || hf_ADP_Category_Id.Value == "")
        {
            obj_tbl_ADP_Item.ADP_Item_Id = 0;
        }
        else
        {
            obj_tbl_ADP_Item.ADP_Item_Id = Convert.ToInt32(hf_ADP_Category_Id.Value);
        }
        obj_tbl_ADP_Item.ADP_Item_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (ddlCategory.SelectedValue == "0" || ddlCategory.SelectedValue == "")
        {
            Msg = "Give Category";
            ddlCategory.Focus();
            return;
        }
        if (txtSpecification.Text.Trim() == string.Empty)
        {
            Msg = "Give Specification";
            txtSpecification.Focus();
            return;
        }
        if (ddlUnit.SelectedValue == "0" || ddlUnit.SelectedValue == "")
        {
            Msg = "Give Unit";
            ddlUnit.Focus();
            return;
        }
        obj_tbl_ADP_Item.ADP_Item_Category_Id = Convert.ToInt32(ddlCategory.SelectedValue);
        obj_tbl_ADP_Item.ADP_Item_Unit_Id = Convert.ToInt32(ddlUnit.SelectedValue);
        try
        {
            obj_tbl_ADP_Item.ADP_Item_Rate = Convert.ToDecimal(txtRate.Text);
        }
        catch
        {
            obj_tbl_ADP_Item.ADP_Item_Rate = 0;
        }
        try
        {
            obj_tbl_ADP_Item.ADP_Item_Qty = Convert.ToDecimal(txtQty.Text);
        }
        catch
        {
            obj_tbl_ADP_Item.ADP_Item_Qty = 0;
        }
        obj_tbl_ADP_Item.ADP_Item_Specification = txtSpecification.Text.Trim();
        obj_tbl_ADP_Item.ADP_Item_Status = 1;

        if (new DataLayer().Insert_tbl_ADP_Item(obj_tbl_ADP_Item, obj_tbl_ADP_Item.ADP_Item_Id, ref Msg))
        {
            MessageBox.Show("Item Created Successfully ! ");
            reset();
            get_tbl_ADP_Item();
            return;
        }
        else
        {

            MessageBox.Show("Error ! ");

            return;
        }
    }

    private void reset()
    {
        txtSpecification.Text = "";
        txtRate.Text = "0";
        txtQty.Text = "0";
        ddlCategory.SelectedValue = "0";
        ddlUnit.SelectedValue = "0";
        hf_ADP_Category_Id.Value = "0";
        get_tbl_ADP_Category();
        mp1.Hide();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int DPR_Status_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        string ADP_Item_Category_Id = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[1].Text.Trim();
        string ADP_Item_Unit_Id = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[2].Text.Trim();
        string Specification = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[5].Text.Trim();
        string Rate = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[7].Text.Trim();
        string Qty = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[8].Text.Trim();
        hf_ADP_Category_Id.Value = DPR_Status_Id.ToString();
        ddlCategory.SelectedValue = ADP_Item_Category_Id;
        ddlUnit.SelectedValue = ADP_Item_Unit_Id;

        txtSpecification.Text = Specification;
        txtRate.Text = Rate;
        txtQty.Text = Qty;
        btnDelete.Visible = true;
        mp1.Show();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int DPR_Status_Id = Convert.ToInt32(hf_ADP_Category_Id.Value);
        if (new DataLayer().Delete_tbl_ADP_Item(DPR_Status_Id, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            reset();
            get_tbl_ADP_Item();
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
        txtSpecification.Text = "";
        txtRate.Text = "0";
        txtQty.Text = "0";
        ddlCategory.SelectedValue = "0";
        ddlUnit.SelectedValue = "0";
        hf_ADP_Category_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}