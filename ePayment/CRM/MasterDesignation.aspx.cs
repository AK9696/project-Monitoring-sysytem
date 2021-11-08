using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterDesignation : System.Web.UI.Page
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
            get_tbl_Designation();
        }
    }

    private void get_tbl_Designation()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Designation();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            //string[] control_Id = new string[] { "btnDelete|Updation|Image" };
            //AllClasses.set_Permisstion(control_Id, grdPost);
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
        //tbl_Designation obj_tbl_Designation = tbl_DesignationAssignment(ref Msg);
        tbl_Designation obj_tbl_Designation = new tbl_Designation();
        if (hf_Designation_Id.Value == "0" || hf_Designation_Id.Value == "")
        {
            obj_tbl_Designation.Designation_Id = 0;
        }
        else
        {
            obj_tbl_Designation.Designation_Id = Convert.ToInt32(hf_Designation_Id.Value);
        }
        obj_tbl_Designation.Designation_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (txtDesignation.Text.Trim() == string.Empty)
        {
            Msg = "Give Designation";
            txtDesignation.Focus();
            return;
        }
        if (ddlDesignationLevel.SelectedValue == "0")
        {
            Msg = "Give Designation Level";
            ddlDesignationLevel.Focus();
            return;
        }
        obj_tbl_Designation.Designation_DesignationName = txtDesignation.Text.Trim();
        obj_tbl_Designation.Designation_Level = Convert.ToInt32(ddlDesignationLevel.SelectedValue);
        obj_tbl_Designation.Designation_Status = 1;

        if (new DataLayer().Insert_tbl_Designation(obj_tbl_Designation, obj_tbl_Designation.Designation_Id, ref Msg))
        {
            MessageBox.Show("Designation Created Successfully ! ");
            reset();
            get_tbl_Designation();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Designation Already Exist. Give another! ");
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
        txtDesignation.Text = "";
        ddlDesignationLevel.SelectedValue = "0";
        hf_Designation_Id.Value = "0";
        get_tbl_Designation();
        mp1.Hide();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Designation_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        string Level_Id = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[3].Text.Trim();
        string Designation_Name = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[2].Text.Trim();
        hf_Designation_Id.Value = Designation_Id.ToString();
        try
        {
            ddlDesignationLevel.SelectedValue = Level_Id;
        }
        catch
        {
            ddlDesignationLevel.SelectedValue = "0";
        }

        txtDesignation.Text = Designation_Name;
        
        mp1.Show();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Designation_Id = Convert.ToInt32(hf_Designation_Id.Value);
        if (new DataLayer().Delete_Designation(Designation_Id, Person_Id))
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
        txtDesignation.Text = "";
        ddlDesignationLevel.SelectedValue = "0";
        hf_Designation_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}
