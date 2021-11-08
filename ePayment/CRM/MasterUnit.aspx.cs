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

public partial class MasterUnit : System.Web.UI.Page
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
        }
    }

    private void get_tbl_Unit()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Unit();
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
        tbl_Unit obj_tbl_Unit = new tbl_Unit();
        if (hf_Unit_Id.Value == "0" || hf_Unit_Id.Value == "")
        {
            obj_tbl_Unit.Unit_Id = 0;
        }
        else
        {
            obj_tbl_Unit.Unit_Id = Convert.ToInt32(hf_Unit_Id.Value);
        }
        obj_tbl_Unit.Unit_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        if (chkLengthApplicable.Checked)
        {
            obj_tbl_Unit.Unit_Length_Applicable = 1;
        }
        else
        {
            obj_tbl_Unit.Unit_Length_Applicable = 0;
        }
        if (chkBredthApplicable.Checked)
        {
            obj_tbl_Unit.Unit_Bredth_Applicable= 1;
        }
        else
        {
            obj_tbl_Unit.Unit_Bredth_Applicable = 0;
        }
        if (chkHeightApplicable.Checked)
        {
            obj_tbl_Unit.Unit_Height_Applicable= 1;
        }
        else
        {
            obj_tbl_Unit.Unit_Height_Applicable = 0;
        }
        if (txtUnit.Text.Trim() == string.Empty)
        {
            Msg = "Give Unit";
            txtUnit.Focus();
            return ;
        }
        obj_tbl_Unit.Unit_Name = txtUnit.Text.Trim();
        obj_tbl_Unit.Unit_Status = 1;

        if (new DataLayer().Insert_tbl_Unit(obj_tbl_Unit, obj_tbl_Unit.Unit_Id, ref Msg))
        {
            MessageBox.Show("Unit Created Successfully ! ");
            reset();
            get_tbl_Unit();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Unit Already Exist. Give another! ");
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
        txtUnit.Text = "";
        hf_Unit_Id.Value = "0";
        get_tbl_Unit();
        mp1.Hide();
    }
   
    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = ((sender as ImageButton).Parent.Parent as GridViewRow);
        int Unit_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        hf_Unit_Id.Value = Unit_Id.ToString();
        txtUnit.Text = gr.Cells[2].Text.Trim();
        if (gr.Cells[3].Text.Trim() == "1")
        {
            chkLengthApplicable.Checked = true;
        }
        else
        {
            chkLengthApplicable.Checked = false;
        }
        if (gr.Cells[4].Text.Trim() == "1")
        {
            chkBredthApplicable.Checked = true;
        }
        else
        {
            chkBredthApplicable.Checked = false;
        }
        if (gr.Cells[5].Text.Trim() == "1")
        {
            chkHeightApplicable.Checked = true;
        }
        else
        {
            chkHeightApplicable.Checked = false;
        }
        mp1.Show(); 
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Unit_Id = Convert.ToInt32(hf_Unit_Id.Value);
        if (new DataLayer().Delete_Unit(Unit_Id, Person_Id))
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
        txtUnit.Text = "";
        hf_Unit_Id.Value = "0";
        btnDelete.Visible = false;
        mp1.Show();
    }
}
