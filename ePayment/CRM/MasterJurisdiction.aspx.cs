using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterJurisdiction : System.Web.UI.Page
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
            get_tbl_Level();
            if (Request.QueryString.Count > 0)
            {
                if (ddlLevel.SelectedValue == "1")
                {
                    dvPJ.Visible = false;
                }
                else
                {
                    dvPJ.Visible = true;
                }
                Set_Label(Request.QueryString[0].ToString());
            }
            else
            {
                dvPJ.Visible = true;
            }
        }
    }

    private void Set_Label(string LevelId)
    {
        if (LevelId == "1")
        {
            lblMainHeader.Text = "Master State";
            lblHeader.Text = "Create State";
            lblJurisdictionName.Text = "State Name";
            lblJurisdictionCode.Text = "State Code";
            lblParentJurisdiction.Text = "Not Applicable";
        }
        else if (LevelId == "2")
        {
            lblMainHeader.Text = "Master Mandal";
            lblHeader.Text = "Create Mandal";
            lblJurisdictionName.Text = "Mandal Name";
            lblJurisdictionCode.Text = "Mandal Code";
            lblParentJurisdiction.Text = "Select State";            
        }
        else if (LevelId == "3")
        {
            lblMainHeader.Text = "Master District";
            lblHeader.Text = "Create District";
            lblJurisdictionName.Text = "District Name";
            lblJurisdictionCode.Text = "District Code";
            lblParentJurisdiction.Text = "Select Mandal";
        }
        else if (LevelId == "4")
        {
            lblMainHeader.Text = "Master Block";
            lblHeader.Text = "Create Block";
            lblJurisdictionName.Text = "Block Name";
            lblJurisdictionCode.Text = "Block Code";
            lblParentJurisdiction.Text = "Select District";
        }
        else if (LevelId == "5")
        {
            lblMainHeader.Text = "Master Gram Panchayat";
            lblHeader.Text = "Create Gram Panchayat";
            lblJurisdictionName.Text = "Gram Panchayat Name";
            lblJurisdictionCode.Text = "Gram Panchayat Code";
            lblParentJurisdiction.Text = "Select Block";
        }
        else if (LevelId == "9")
        {
            lblMainHeader.Text = "Master Villages";
            lblHeader.Text = "Create Villages";
            lblJurisdictionName.Text = "Villages Name";
            lblJurisdictionCode.Text = "Villages Code";
            lblParentJurisdiction.Text = "Select Gram Panchayat";
        }
        else if (LevelId == "10")
        {
            lblMainHeader.Text = "Master Habitations";
            lblHeader.Text = "Create Habitations";
            lblJurisdictionName.Text = "Habitations Name";
            lblJurisdictionCode.Text = "Habitations Code";
            lblParentJurisdiction.Text = "Select Villages";
        }
        else
        {
            lblMainHeader.Text = "Master Jurisdiction";
            lblHeader.Text = "Create Jurisdiction";
            lblJurisdictionName.Text = "Jurisdiction Name";
            lblJurisdictionCode.Text = "Jurisdiction Code";
            lblParentJurisdiction.Text = "Parent Jurisdiction";
        }
    }

    private void get_tbl_Level()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Level(false);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlLevel, "Level_Name", "M_Level_Id");
            if (Request.QueryString.Count > 0)
            {
                ddlLevel.SelectedValue = Request.QueryString[0].ToString();
                ddlLevel.Enabled = false;
                ddlLevel_SelectedIndexChanged(ddlLevel, new EventArgs());
            }
            else
            {
                ddlLevel.SelectedValue = "1";
                ddlLevel_SelectedIndexChanged(ddlLevel, new EventArgs());
            }
        }
        else
        {
            ddlLevel.Items.Clear();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlLevel.SelectedValue == "0")
        {
            MessageBox.Show("Please Select Level");
            ddlLevel.Focus();
            return;
        }
        string Msg = "";
        M_Jurisdiction obj_tbl_ProductType = new M_Jurisdiction();
        obj_tbl_ProductType.Created_By = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_ProductType.Is_Active = 1;
        obj_tbl_ProductType.M_Level_Id = Convert.ToInt32(ddlLevel.SelectedValue);
        if (hf_M_Jurisdiction_Id.Value == "0" || hf_M_Jurisdiction_Id.Value == "")
        {
            obj_tbl_ProductType.M_Jurisdiction_Id = 0;
        }
        else
        {
            obj_tbl_ProductType.M_Jurisdiction_Id = Convert.ToInt32(hf_M_Jurisdiction_Id.Value);
        }
        if (txtJurisdictionName.Text.Trim() == string.Empty)
        {
            Msg = "Give Jurisdiction Name";
            txtJurisdictionName.Focus();
            return;
        }
        if (ddlParentJurisdiction.Items.Count > 0 && ddlParentJurisdiction.SelectedValue == "0")
        {
            Msg = "Select Parent Jurisdiction";
            ddlParentJurisdiction.Focus();
            return;
        }
        obj_tbl_ProductType.Jurisdiction_Name_Eng = txtJurisdictionName.Text.Trim();
        obj_tbl_ProductType.Jurisdiction_Code = txtJurisdictionCode.Text.Trim();
        try
        {
            obj_tbl_ProductType.Parent_Jurisdiction_Id = Convert.ToInt32(ddlParentJurisdiction.SelectedValue);
        }
        catch
        {
            obj_tbl_ProductType.Parent_Jurisdiction_Id = 0;
        }
        if (new DataLayer().Insert_M_Jurisdiction(obj_tbl_ProductType, obj_tbl_ProductType.M_Jurisdiction_Id, ref Msg))
        {
            MessageBox.Show("Jurisdiction Created Successfully ! ");
            reset();
            return;
        }
        else
        {
            if (Msg == "A")
            {
                MessageBox.Show("This Jurisdiction Already Exist. Give another Name ! ");
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
        hf_M_Jurisdiction_Id.Value = "0";
        ddlParentJurisdiction.Items.Clear();
        if (Request.QueryString.Count > 0)
        {
            ddlLevel.SelectedValue = Request.QueryString[0].ToString();
            ddlLevel.Enabled = false;
            ddlLevel_SelectedIndexChanged(ddlLevel, new EventArgs());
        }
        else
        {
            ddlLevel.SelectedValue = "1";
            ddlLevel_SelectedIndexChanged(ddlLevel, new EventArgs());
        }
        txtJurisdictionName.Text = "";
        txtJurisdictionCode.Text = "";
        int Parent_Jurisdiction_Id = 0;
        try
        {
            Parent_Jurisdiction_Id = Convert.ToInt32(ddlParentJurisdiction.SelectedValue);
        }
        catch
        {
            Parent_Jurisdiction_Id = 0;
        }
        get_tbl_Jurisdiction(Convert.ToInt32(ddlLevel.SelectedValue), Parent_Jurisdiction_Id);
        mp1.Hide();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtJurisdictionName.Text = "";
        txtJurisdictionCode.Text = "";
        hf_M_Jurisdiction_Id.Value = "0";
        if (ddlLevel.SelectedValue == "0")
        {
            grdPost.DataSource = null;
            grdPost.DataBind();
            hf_dt_Options_Dynamic.Value = "";
            ddlParentJurisdiction.Items.Clear();
        }
        else
        {
            int Parent_Level_Id = 0;
            if (Request.QueryString[0].ToString() == "9")
            {
                 Parent_Level_Id = 5;
            }
            else
            {
                 Parent_Level_Id = Convert.ToInt32(ddlLevel.SelectedValue) - 1;
            }
            if (Parent_Level_Id > 0)
            {
                get_tbl_Jurisdiction_Parent(Parent_Level_Id);
            }
            else
            {
                ddlParentJurisdiction.Items.Clear();
            }
            int Parent_Jurisdiction_Id = 0;
            try
            {
                Parent_Jurisdiction_Id = Convert.ToInt32(ddlParentJurisdiction.SelectedValue);
            }
            catch
            {
                Parent_Jurisdiction_Id = 0;
            }
            get_tbl_Jurisdiction(Convert.ToInt32(ddlLevel.SelectedValue), Parent_Jurisdiction_Id);
        }
        Set_Label(ddlLevel.SelectedValue);
    }

    private void get_tbl_Jurisdiction(int Level_Id, int Parent_Jurisdiction_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(Level_Id, Parent_Jurisdiction_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            
            List<string> _columnsList = new List<string>();

            for (int i = 0; i < grdPost.Columns.Count; i++)
            {
                if (grdPost.Columns[i].Visible == true)
                    _columnsList.Add(null);
            }
            //_columnsList.Add(null);
            //_columnsList.Add(null);
            string[] columnsList = new string[_columnsList.Count];
            columnsList = _columnsList.ToArray();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            hf_dt_Options_Dynamic.Value = jss.Serialize(columnsList);

        }
        else
        {
            hf_dt_Options_Dynamic.Value = "";
            grdPost.DataSource = null;
            grdPost.DataBind();
        }
    }

    private void get_tbl_Jurisdiction_Parent(int Level_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_M_Jurisdiction(Level_Id, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Level_Id >= 3)
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlParentJurisdiction, "Jurisdiction_Name_Eng_With_Parent", "M_Jurisdiction_Id");
            }
            else
            {
                AllClasses.FillDropDown(ds.Tables[0], ddlParentJurisdiction, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
            }
            ViewState["ParentJurisdiction"] = ds.Tables[0];
        }
        else
        {
            ddlParentJurisdiction.Items.Clear();
            ViewState["ParentJurisdiction"] = null;
        }
    }


    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlLevel.SelectedValue == "1")
            {
                e.Row.Cells[3].Text = "Not Applicable";
                e.Row.Cells[4].Text = "State Name";
                e.Row.Cells[5].Text = "State Code";
            }
            else if (ddlLevel.SelectedValue == "2")
            {
                e.Row.Cells[3].Text = "State";
                e.Row.Cells[4].Text = "Mandal Name";
                e.Row.Cells[5].Text = "Mandal Code";
            }
            else if (ddlLevel.SelectedValue == "3")
            {
                e.Row.Cells[3].Text = "Mandal";
                e.Row.Cells[4].Text = "District Name";
                e.Row.Cells[5].Text = "District Code";
            }
            else if (ddlLevel.SelectedValue == "4")
            {
                e.Row.Cells[3].Text = "District";
                e.Row.Cells[4].Text = "Block Name";
                e.Row.Cells[5].Text = "Block Code";
            }
            else if (ddlLevel.SelectedValue == "5")
            {
                e.Row.Cells[3].Text = "Block";
                e.Row.Cells[4].Text = "Gram Panchayat Name";
                e.Row.Cells[5].Text = "Gram Panchayat Code";
            }
            else
            {
                e.Row.Cells[3].Text = "Parent Jurisdiction";
                e.Row.Cells[4].Text = "Jurisdiction Name";
                e.Row.Cells[5].Text = "Jurisdiction Name";
            }

        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataTable dt = (DataTable)ViewState["ParentJurisdiction"];
        //    if (dt != null)
        //    {
        //        DropDownList ddlPJ = (e.Row.FindControl("ddlParentJurisdictionG") as DropDownList);
        //        AllClasses.FillDropDown_WithOutSelect(dt, ddlPJ, "Jurisdiction_Name_Eng", "M_Jurisdiction_Id");
        //        try
        //        {
        //            ddlPJ.SelectedValue = e.Row.Cells[1].Text.Trim();
        //        }
        //        catch
        //        {
        //            ddlPJ.SelectedValue = "0";
        //        }
        //        ddlPJ.Enabled = false;
        //        TextBox txtJrName = (e.Row.FindControl("txtJurisdictionNameG") as TextBox);
        //        txtJrName.Enabled = false;
        //        TextBox txtJrCode = (e.Row.FindControl("txtJurisdictionCodeG") as TextBox);
        //        txtJrCode.Enabled = false;
        //        Button btnUpdate = (e.Row.FindControl("btnUpdate") as Button);
        //        btnUpdate.Visible = false;
        //    }
        //}
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        btnSave_Click(btnSave, e);
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {

        int M_Jurisdiction_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());
        hf_M_Jurisdiction_Id.Value = M_Jurisdiction_Id.ToString();
        try
        {
            ddlParentJurisdiction.SelectedValue = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[1].Text.Replace("&nbsp;", "").Trim();
        }
        catch
        {
            ddlParentJurisdiction.SelectedValue = "0";
        }
        txtJurisdictionName.Text = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[4].Text.Replace("&nbsp;", "").Trim();
        txtJurisdictionCode.Text = ((sender as ImageButton).Parent.Parent as GridViewRow).Cells[5].Text.Replace("&nbsp;", "").Trim();

        mp1.Show();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int M_Jurisdiction_Id = Convert.ToInt32(hf_M_Jurisdiction_Id.Value);
        if (new DataLayer().Delete_Jurisdiction(M_Jurisdiction_Id, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            int Parent_Jurisdiction_Id = 0;
            try
            {
                Parent_Jurisdiction_Id = Convert.ToInt32(ddlParentJurisdiction.SelectedValue);
            }
            catch
            {
                Parent_Jurisdiction_Id = 0;
            }
            get_tbl_Jurisdiction(Convert.ToInt32(ddlLevel.SelectedValue), Parent_Jurisdiction_Id);
            Set_Label(ddlLevel.SelectedValue);
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
            return;
        }
    }

    protected void ddlParentJurisdiction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlLevel.SelectedValue) == 4)
        {
            if (ddlParentJurisdiction.SelectedValue == "0")
            {
                return;
            }
        }
        int Parent_Jurisdiction_Id = 0;
        try
        {
            Parent_Jurisdiction_Id = Convert.ToInt32(ddlParentJurisdiction.SelectedValue);
        }
        catch
        {
            Parent_Jurisdiction_Id = 0;
        }
        get_tbl_Jurisdiction(Convert.ToInt32(ddlLevel.SelectedValue), Parent_Jurisdiction_Id);
        mp1.Show();
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
        hf_M_Jurisdiction_Id.Value = "0";
        ddlParentJurisdiction.SelectedValue = "0";
        txtJurisdictionName.Text = "";
        txtJurisdictionCode.Text = "";
        mp1.Show();
    }
}
