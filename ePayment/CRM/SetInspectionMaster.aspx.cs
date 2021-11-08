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
using System.Collections.Generic;
using System.Xml.Linq;

public partial class SetInspectionMaster : System.Web.UI.Page
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
            get_tbl_Level();
            get_tbl_SetInspectionMaster();
        }
    }

    private void get_tbl_SetInspectionMaster()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_SetInspectionMaster();
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

    private void get_tbl_Level()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Level_SetInspection();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlLevel, "Level_Name", "M_Level_Id");

        }
        else
        {
            ddlLevel.Items.Clear();
        }
    }
    private void get_tbl_Designation()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Designation();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDesignation, "Designation_DesignationName", "Designation_Id");
            if (ddlDesignation.Items.Count == 2)
            {
                ddlDesignation.SelectedIndex = 1;
            }
        }
        else
        {
            ddlDesignation.Items.Clear();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Msg = "";
        if (txtSetName.Text.Trim() == string.Empty)
        {
            MessageBox.Show("Give Set Name");
            txtSetName.Focus();
            return;
        }
        List<tbl_SetInspectionMaster> obj_tbl_SetInspectionMaster_Li = new List<tbl_SetInspectionMaster>();
        for (int i = 0; i < dgvAddDesignation.Rows.Count; i++)
        {
            tbl_SetInspectionMaster obj_tbl_SetInspectionMaster = new tbl_SetInspectionMaster();
            if (hf_SetInspectionMaster_Id.Value == "0" || hf_SetInspectionMaster_Id.Value == "")
            {
                obj_tbl_SetInspectionMaster.SetInspectionMaster_Id = 0;
            }
            else
            {
                obj_tbl_SetInspectionMaster.SetInspectionMaster_Id = Convert.ToInt32(hf_SetInspectionMaster_Id.Value);
            }
            obj_tbl_SetInspectionMaster.SetInspectionMaster_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());


            obj_tbl_SetInspectionMaster.SetInspectionMaster_Name = txtSetName.Text.Trim();
            obj_tbl_SetInspectionMaster.SetInspectionMaster_Status = 1;
            obj_tbl_SetInspectionMaster.SetInspectionMaster_Designation_Id = Convert.ToInt32(dgvAddDesignation.Rows[i].Cells[0].Text.Trim());
            obj_tbl_SetInspectionMaster.SetInspectionMaster_MLevel_Id = Convert.ToInt32(dgvAddDesignation.Rows[i].Cells[1].Text.Trim());
            obj_tbl_SetInspectionMaster_Li.Add(obj_tbl_SetInspectionMaster);
        }
        if (obj_tbl_SetInspectionMaster_Li == null || obj_tbl_SetInspectionMaster_Li.Count==0)
        {
            MessageBox.Show("Please Addd At Least One Designation ! ");
        }

        if (new DataLayer().Insert_tbl_SetInspectionMaster(obj_tbl_SetInspectionMaster_Li, Convert.ToInt32(hf_SetInspectionMaster_Id.Value),txtSetName.Text, ref Msg))
        {
            MessageBox.Show("Set Inspection Created Successfully ! ");
            reset();
            get_tbl_SetInspectionMaster();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Set Inspection Already Exist. Give another! ");
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
        txtSetName.Text = "";
        hf_SetInspectionMaster_Id.Value = "0";
        ddlDesignation.SelectedValue = "0";
        ddlLevel.SelectedValue = "0";
        get_tbl_SetInspectionMaster();
        divCreateNew.Visible = false;
        ViewState["AdditionalDesigation"] = null;
        dgvAddDesignation.DataSource = null;
        dgvAddDesignation.DataBind();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {

        int SetInspectionMaster_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_SetInspectionMaster_Id.Value = SetInspectionMaster_Id.ToString();
        txtSetName.Text = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[2].Text.Trim();
        DataSet ds = (new DataLayer()).get_tbl_SetInspectionMaster_BySetName(txtSetName.Text);
        if (AllClasses.CheckDataSet(ds))
        {
            ViewState["AdditionalDesigation"] = ds.Tables[0];
            dgvAddDesignation.DataSource = ds.Tables[0];
            dgvAddDesignation.DataBind();
        }
        else
        {
            ViewState["AdditionalDesigation"] =null;
            dgvAddDesignation.DataSource = null;
            dgvAddDesignation.DataBind();
        }
        btnDelete.Visible = true;
        divCreateNew.Visible = true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        string SetInspectionMaster_Name = txtSetName.Text;
        if (new DataLayer().Delete_SetInspectionMaster(SetInspectionMaster_Name, Person_Id))
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
        txtSetName.Text = "";
        hf_SetInspectionMaster_Id.Value = "0";
        ddlDesignation.SelectedValue = "0";
        ddlLevel.SelectedValue = "0";
        ViewState["AdditionalDesigation"] = null;
        dgvAddDesignation.DataSource = null;
        dgvAddDesignation.DataBind();
        btnDelete.Visible = false;
        divCreateNew.Visible = true; 
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlDesignation.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Designation");
            ddlDesignation.Focus();
            return;
        }
        if (ddlLevel.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Level");
            ddlLevel.Focus();
            return;
        }

        if (ViewState["AdditionalDesigation"] != null)
        {
            DataTable dt = (DataTable)ViewState["AdditionalDesigation"];

            DataRow dr = dt.NewRow();

            dr["SetInspectionMaster_Designation_Id"] = ddlDesignation.SelectedValue;
            dr["SetInspectionMaster_MLevel_Id"] = ddlLevel.SelectedValue;
            dr["Designation_DesignationName"] = ddlDesignation.SelectedItem.Text.Trim();
            dr["Level_Name"] = ddlLevel.SelectedItem.Text.Trim();
            dt.Rows.Add(dr);

            ViewState["AdditionalDesigation"] = dt;

            dgvAddDesignation.DataSource = dt;
            dgvAddDesignation.DataBind();
            ddlDesignation.SelectedValue = "0";
            ddlLevel.SelectedValue = "0";
        }
        else
        {
            DataSet ds = (new DataLayer()).get_tbl_SetInspectionMaster_BySetName("");
            try
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.NewRow();

                dr["SetInspectionMaster_Designation_Id"] = ddlDesignation.SelectedValue;
                dr["SetInspectionMaster_MLevel_Id"] = ddlLevel.SelectedValue;
                dr["Designation_DesignationName"] = ddlDesignation.SelectedItem.Text.Trim();
                dr["Level_Name"] = ddlLevel.SelectedItem.Text.Trim();
                dt.Rows.Add(dr);

                ViewState["AdditionalDesigation"] = dt;

                dgvAddDesignation.DataSource = dt;
                dgvAddDesignation.DataBind();
                ddlDesignation.SelectedValue = "0";
                ddlLevel.SelectedValue = "0";

            }
            catch
            {
                ViewState["AdditionalDesigation"] = null;
            }
        }

    }

    protected void lnkDeleteAdditionalULB_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["AdditionalDesigation"] != null)
        {
            if (index >= 0)
            {
                DataTable dt = (DataTable)ViewState["AdditionalDesigation"];
                dt.Rows.RemoveAt(index);
                dgvAddDesignation.DataSource = dt;
                dgvAddDesignation.DataBind();
                ViewState["AdditionalDesigation"] = dt;
            }

        }
    }
}