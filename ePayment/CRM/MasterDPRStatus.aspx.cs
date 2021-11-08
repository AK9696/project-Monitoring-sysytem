using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterDPRStatus : System.Web.UI.Page
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
            get_tbl_DPR_Status();
        }
    }

    private void get_tbl_DPR_Status()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_DPR_Status();
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
        //tbl_DPR_Status obj_tbl_DPR_Status = tbl_DPR_StatusAssignment(ref Msg);
        tbl_DPR_Status obj_tbl_DPR_Status = new tbl_DPR_Status();
        if (hf_DPR_Status_Id.Value == "0" || hf_DPR_Status_Id.Value == "")
        {
            obj_tbl_DPR_Status.DPR_Status_Id = 0;
        }
        else
        {
            obj_tbl_DPR_Status.DPR_Status_Id = Convert.ToInt32(hf_DPR_Status_Id.Value);
        }
        obj_tbl_DPR_Status.DPR_Status_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (txtDPR_Status.Text.Trim() == string.Empty)
        {
            Msg = "Give DPR_Status";
            txtDPR_Status.Focus();
            return;
        }
        obj_tbl_DPR_Status.DPR_Status_DPR_StatusName = txtDPR_Status.Text.Trim();
        obj_tbl_DPR_Status.DPR_Status_Status = 1;

        if (new DataLayer().Insert_tbl_DPR_Status(obj_tbl_DPR_Status, obj_tbl_DPR_Status.DPR_Status_Id, ref Msg))
        {
            MessageBox.Show("DPR Status Created Successfully ! ");
            reset();
            get_tbl_DPR_Status();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This DPR Status Already Exist. Give another! ");
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
        txtDPR_Status.Text = "";
        hf_DPR_Status_Id.Value = "0";
        get_tbl_DPR_Status();
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
        string Level_Id = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[3].Text.Trim();
        string DPR_Status_Name = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[2].Text.Trim();
        hf_DPR_Status_Id.Value = DPR_Status_Id.ToString();
        
        txtDPR_Status.Text = DPR_Status_Name;
        mp1.Show();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int DPR_Status_Id = Convert.ToInt32(hf_DPR_Status_Id.Value);
        if (new DataLayer().Delete_DPR_Status(DPR_Status_Id, Person_Id))
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
        txtDPR_Status.Text = "";
        hf_DPR_Status_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}
