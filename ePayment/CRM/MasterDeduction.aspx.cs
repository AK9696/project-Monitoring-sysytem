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

public partial class MasterDeduction : System.Web.UI.Page
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
            get_tbl_Deduction();
        }
    }

    private void get_tbl_Deduction()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction(0);
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
        if (txtDeduction.Text.Trim() == string.Empty)
        {
            MessageBox.Show("Give Deduction");
            txtDeduction.Focus();
            return;
        }
        if (ddldeductionCategory.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Deduction Category");
            ddldeductionCategory.Focus();
            return;
        }

        tbl_Deduction obj_tbl_Deduction = new tbl_Deduction();
        obj_tbl_Deduction.Deduction_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_Deduction.Deduction_Value = Convert.ToDecimal(txtValue.Text.Trim());
        obj_tbl_Deduction.Deduction_Name = txtDeduction.Text.Trim();
        obj_tbl_Deduction.Deduction_Category = ddldeductionCategory.SelectedValue;
        obj_tbl_Deduction.Deduction_Mode = ddlDeductionMode.SelectedValue;
        obj_tbl_Deduction.Deduction_Type = rbtStatusType.SelectedValue;
        obj_tbl_Deduction.Deduction_Status = 1;
        if (hf_Deduction_Id.Value == "0" || hf_Deduction_Id.Value == "")
        {
            obj_tbl_Deduction.Deduction_Id = 0;
        }
        else
        {
            obj_tbl_Deduction.Deduction_Id = Convert.ToInt32(hf_Deduction_Id.Value);
        }

        string Msg = "";

        if (new DataLayer().Insert_tbl_Deduction(obj_tbl_Deduction, int.Parse(hf_Deduction_Id.Value), ref Msg))
        {
            MessageBox.Show("Deduction Created Successfully ! ");
            reset();
            get_tbl_Deduction();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Deduction Already Exist. Give another! ");
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
        txtDeduction.Text = "";
        txtValue.Text = "";
        hf_Deduction_Id.Value = "0";
        get_tbl_Deduction();
        mp1.Hide();
    }


    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Deduction_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        hf_Deduction_Id.Value = Deduction_Id.ToString();
        txtDeduction.Text = gr.Cells[5].Text.Replace("&nbsp;", "").Trim();
        txtValue.Text = gr.Cells[6].Text.Replace("&nbsp;", "").Trim();
        try
        {
            rbtStatusType.SelectedValue = gr.Cells[4].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            rbtStatusType.SelectedIndex = 0;
        }
        try
        {
            ddldeductionCategory.SelectedValue =gr.Cells[3].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            rbtStatusType.SelectedIndex = 0;
        }
        try
        {
            ddlDeductionMode.SelectedValue = gr.Cells[2].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlDeductionMode.SelectedIndex = 0;
        }
        mp1.Show();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Deduction_Id = Convert.ToInt32(hf_Deduction_Id.Value);
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Delete_Deduction(Deduction_Id, Person_Id))
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

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        btnDelete.Visible = false;
        reset();
        mp1.Show();
    }
}
