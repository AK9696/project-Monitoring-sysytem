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

public partial class MasterSupportTktCat : System.Web.UI.Page
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
            get_tbl_TicketCategory();
        }
    }

    private void get_tbl_TicketCategory()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_TicketCategory();
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
        if (txtTicketCategory.Text.Trim() == string.Empty)
        {
            Msg = "Give TicketCategory";
            txtTicketCategory.Focus();
            return ;
        }
        tbl_TicketCategory obj_tbl_TicketCategory = new tbl_TicketCategory();
        if (hf_TicketCategory_Id.Value == "0" || hf_TicketCategory_Id.Value == "")
        {
            obj_tbl_TicketCategory.TicketCategory_Id = 0;
        }
        else {
            obj_tbl_TicketCategory.TicketCategory_Id = Convert.ToInt32(hf_TicketCategory_Id.Value);
        }
        obj_tbl_TicketCategory.TicketCategory_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_TicketCategory.TicketCategory_Name = txtTicketCategory.Text.Trim();
        obj_tbl_TicketCategory.TicketCategory_Status = 1;

        if (obj_tbl_TicketCategory == null)
        {
            MessageBox.Show(Msg);
            return;
        }
        if (new DataLayer().Insert_tbl_TicketCategory(obj_tbl_TicketCategory, obj_tbl_TicketCategory.TicketCategory_Id, ref Msg))
        {
            MessageBox.Show("Ticket Category Created Successfully ! ");
            reset();
            get_tbl_TicketCategory();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Ticket Category Already Exist. Give another! ");
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
        txtTicketCategory.Text = "";
        hf_TicketCategory_Id.Value = "0";
        get_tbl_TicketCategory();
        
        mp1.Hide();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int TicketCategory_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_TicketCategory_Id.Value = TicketCategory_Id.ToString();
        DataSet ds = new DataSet();
        ds = (new DataLayer()).Edit_tbl_TicketCategory(TicketCategory_Id.ToString());
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtTicketCategory.Text = ds.Tables[0].Rows[0]["TicketCategory_Name"].ToString();

        }
        mp1.Show();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {

        int TicketCategory_Id = Convert.ToInt32(hf_TicketCategory_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Delete_TicketCategory(TicketCategory_Id, Person_Id))
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
        txtTicketCategory.Text = "";
        hf_TicketCategory_Id.Value = "0";
       
        mp1.Show();
    }
}
