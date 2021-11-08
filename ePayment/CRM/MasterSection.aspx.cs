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

public partial class MasterSection : System.Web.UI.Page
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
            get_tbl_Section();
        }
    }

    private void get_tbl_Section()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Section();
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
        tbl_Section obj_tbl_Section = new tbl_Section();
        if (hf_Section_Id.Value == "0" || hf_Section_Id.Value == "")
        {
            obj_tbl_Section.Section_Id = 0;
        }
        else
        {
            obj_tbl_Section.Section_Id = Convert.ToInt32(hf_Section_Id.Value);
        }
        obj_tbl_Section.Section_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (txtSection.Text.Trim() == string.Empty)
        {
            Msg = "Give Section";
            txtSection.Focus();
            return ;
        }
        obj_tbl_Section.Section_Name = txtSection.Text.Trim();
        obj_tbl_Section.Section_Status = 1;

        if (new DataLayer().Insert_tbl_Section(obj_tbl_Section, obj_tbl_Section.Section_Id, ref Msg))
        {
            MessageBox.Show("Section Created Successfully ! ");
            reset();
            get_tbl_Section();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Section Already Exist. Give another! ");
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
        txtSection.Text = "";
        hf_Section_Id.Value = "0";
        get_tbl_Section();
        mp1.Hide();
    }

   
    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        
        int Section_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_Section_Id.Value = Section_Id.ToString();
        txtSection.Text = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[2].Text.Trim();
        mp1.Show(); 
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Section_Id = Convert.ToInt32(hf_Section_Id.Value);
        if (new DataLayer().Delete_Section(Section_Id, Person_Id))
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
        txtSection.Text = "";
        hf_Section_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}
