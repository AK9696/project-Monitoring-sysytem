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

public partial class MasterULB : System.Web.UI.Page
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
            get_tbl_Jurisdiction(3, 0);
            get_tbl_ULB();
        }
    }
    private void get_tbl_Jurisdiction(int Level_Id, int Parent_Jurisdiction_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(Level_Id, Parent_Jurisdiction_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Level_Id == 1)
            {
                //AllClasses.FillDropDown(ds.Tables[0], ddlState, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
            else if (Level_Id == 2)
            {
                //AllClasses.FillDropDown(ds.Tables[0], ddlMandal, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
            else if (Level_Id == 3)
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
        }
        else
        {
            if (Level_Id == 1)
            {
                //ddlState.Items.Clear();
            }
            if (Level_Id == 2)
            {
                //ddlMandal.Items.Clear();
            }
            else if (Level_Id == 3)
            {
                ddlDistrict.Items.Clear();
            }
        }
    }
    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDistrict.SelectedValue == "0")
        {
            ddlLokSabha.Items.Clear();
            ddlVidhanSabha.Items.Clear();
        }
        else
        {
            get_tbl_LokSabha(Convert.ToInt32(ddlDistrict.SelectedValue));
        }
        mp1.Show();
    }
    private void get_tbl_LokSabha(int District_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_LokSabha(District_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlLokSabha, "LokSabha_Name", "LokSabha_Id");
        }
        else
        {
            ddlLokSabha.Items.Clear();
        }
    }
    private void get_tbl_ULB()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ULB(0);
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
        tbl_ULB obj_tbl_ULB = new tbl_ULB();
        if (hf_ULB_Id.Value == "0" || hf_ULB_Id.Value == "")
        {
            obj_tbl_ULB.ULB_Id = 0;
        }
        else
        {
            obj_tbl_ULB.ULB_Id = Convert.ToInt32(hf_ULB_Id.Value);
        }
        obj_tbl_ULB.ULB_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (ddlDistrict.SelectedValue == "0")
        {
            Msg = "Select District";
            ddlDistrict.Focus();
            return;
        }
        if (txtULBName.Text.Trim() == string.Empty)
        {
            Msg = "Give ULB";
            txtULBName.Focus();
            return;
        }
        if (ddlULBCode.SelectedValue == "0")
        {
            Msg = "Select ULB Type";
            ddlULBCode.Focus();
            return;
        }

        obj_tbl_ULB.ULB_Name = txtULBName.Text.Trim();
        obj_tbl_ULB.ULB_District_Id = Convert.ToInt32(ddlDistrict.SelectedValue);
        try
        {
            obj_tbl_ULB.ULB_LokSabha_Id = Convert.ToInt32(ddlLokSabha.SelectedValue);
        }
        catch
        {

        }
        try
        {
            obj_tbl_ULB.ULB_VidhanSabha_Id = Convert.ToInt32(ddlVidhanSabha.SelectedValue);
        }
        catch
        {

        }
        obj_tbl_ULB.ULB_Status = 1;
        obj_tbl_ULB.ULB_Type = ddlULBCode.SelectedValue;

        if (new DataLayer().Insert_tbl_ULB(obj_tbl_ULB))
        {
            MessageBox.Show("ULB Created Successfully ! ");
            reset();
            get_tbl_ULB();
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
        txtULBName.Text = "";
        hf_ULB_Id.Value = "0";
        get_tbl_ULB();
        mp1.Hide();
    }
    protected void ddlLokSabha_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLokSabha.SelectedValue == "0")
        {
            ddlVidhanSabha.Items.Clear();
        }
        else
        {
            get_tbl_VidhanSabha(Convert.ToInt32(ddlLokSabha.SelectedValue));
        }
        mp1.Show();
    }

    private void get_tbl_VidhanSabha(int LokSabha_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_VidhanSabha(LokSabha_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlVidhanSabha, "VidhanSabha_Name", "VidhanSabha_Id");
        }
        else
        {
            ddlVidhanSabha.Items.Clear();
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int ULB_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_ULB_Id.Value = ULB_Id.ToString();
        GridViewRow gr = ((sender as ImageButton).Parent.Parent as GridViewRow);

        try
        {
            ddlDistrict.SelectedValue = gr.Cells[1].Text.Trim();
            ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
        }
        catch
        { }
        try
        {
            ddlLokSabha.SelectedValue = gr.Cells[2].Text.Trim();
            ddlLokSabha_SelectedIndexChanged(ddlLokSabha, e);
        }
        catch
        { }
        try
        {
            ddlVidhanSabha.SelectedValue = gr.Cells[3].Text.Trim();
        }
        catch
        { }
        ddlULBCode.SelectedValue = gr.Cells[6].Text.Trim();
        txtULBName.Text = gr.Cells[7].Text.Trim();
        mp1.Show();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int ULB_Id = Convert.ToInt32(hf_ULB_Id.Value);
        if (new DataLayer().Delete_ULB(ULB_Id, Person_Id))
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
        txtULBName.Text = "";
        hf_ULB_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}
