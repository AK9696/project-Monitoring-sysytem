using Obout.Ajax.UI.HTMLEditor;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterLokSabha : System.Web.UI.Page
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
            get_tbl_LokSabha(0);
        }
    }
    private void get_tbl_Jurisdiction(int Level_Id, int Parent_Jurisdiction_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(Level_Id, Parent_Jurisdiction_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDistrict, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        }
        else
        {
            ddlDistrict.Items.Clear();
        }
    }
    private void get_tbl_LokSabha(int District_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_LokSabha(District_Id);
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
        if (txtLokSabha.Text.Trim() == string.Empty)
        {
            Msg = "Give Lok Sabha";
            txtLokSabha.Focus();
            return;
        }
        tbl_LokSabha obj_tbl_LokSabha = new tbl_LokSabha();
        if (hf_LokSabha_Id.Value == "0" || hf_LokSabha_Id.Value == "")
        {
            obj_tbl_LokSabha.LokSabha_Id = 0;
        }
        else
        {
            obj_tbl_LokSabha.LokSabha_Id = Convert.ToInt32(hf_LokSabha_Id.Value);
        }
        obj_tbl_LokSabha.LokSabha_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_LokSabha.LokSabha_Name = txtLokSabha.Text.Trim();
        obj_tbl_LokSabha.LokSabha_DistrictId = Convert.ToInt32(ddlDistrict.SelectedValue);
        obj_tbl_LokSabha.LokSabha_Status = 1;

        if (obj_tbl_LokSabha == null)
        {
            MessageBox.Show(Msg);
            return;
        }
        if (new DataLayer().Insert_tbl_LokSabha(obj_tbl_LokSabha, obj_tbl_LokSabha.LokSabha_Id, ref Msg))
        {
            MessageBox.Show("Lok Sabha Created Successfully ! ");
            reset();
            get_tbl_LokSabha(0);
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Lok Sabha Already Exist. Give another! ");
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
        txtLokSabha.Text = "";
        ddlDistrict.SelectedValue= "0";
        hf_LokSabha_Id.Value = "0";
        get_tbl_LokSabha(0);
        divCreateNew.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int LokSabha_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_LokSabha_Id.Value = LokSabha_Id.ToString();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).Edit_tbl_LokSabha(LokSabha_Id.ToString());
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtLokSabha.Text = ds.Tables[0].Rows[0]["LokSabha_Name"].ToString();
            ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["LokSabha_DistrictId"].ToString();
        }
        divCreateNew.Visible = true;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int LokSabha_Id = Convert.ToInt32(hf_LokSabha_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Delete_LokSabha(LokSabha_Id, Person_Id))
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
        txtLokSabha.Text = "";
        ddlDistrict.SelectedValue = "0";
        hf_LokSabha_Id.Value = "0";
        divCreateNew.Visible = true;
    }
}
