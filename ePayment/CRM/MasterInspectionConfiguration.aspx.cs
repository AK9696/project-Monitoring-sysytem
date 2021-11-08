using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterInspectionConfiguration : System.Web.UI.Page
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
            get_tbl_Project();
        }
    }

    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        int Person_Id = 0;
        if (Session["UserType"].ToString() == "4")
        {
            Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        }
        else
        {
            Person_Id = 0;
        }
        ds = (new DataLayer()).get_tbl_Project();
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
        List<tbl_ProjectQuestionnaire> obj_tbl_ProjectQuestionnaire_Li = new List<tbl_ProjectQuestionnaire>();

        JavaScriptSerializer js = new JavaScriptSerializer();
        TextBox txtProjectQuestionnaire_Name;
        CheckBox chkAnswerDesc;
        for (int i = 0; i < dgvQuestionnaire.Rows.Count; i++)
        {
            txtProjectQuestionnaire_Name = dgvQuestionnaire.Rows[i].FindControl("txtProjectQuestionnaire_Name") as TextBox;
            chkAnswerDesc = dgvQuestionnaire.Rows[i].FindControl("chkAnswerDesc") as CheckBox;
            if (txtProjectQuestionnaire_Name.Text.Trim() == "")
            {
                MessageBox.Show("Please Fill Questionire!!");
                txtProjectQuestionnaire_Name.Focus();
                return;
            }
            tbl_ProjectQuestionnaire obj_tbl_ProjectQuestionnaire = new tbl_ProjectQuestionnaire();
            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_ProjectId = Convert.ToInt32(hf_Project_Id.Value);
            try
            {
                obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Id = Convert.ToInt32(dgvQuestionnaire.Rows[i].Cells[1].Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Id = 0;
            }
            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Name = txtProjectQuestionnaire_Name.Text.Trim();
            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Status = 1;

            List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
            if (chkAnswerDesc.Checked)
            {
                obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
            }
            else
            {
                js = new JavaScriptSerializer();
                HiddenField hdf_Answer = (dgvQuestionnaire.Rows[i].FindControl("hdf_Answer") as HiddenField);
                if (hdf_Answer.Value == "")
                {
                    MessageBox.Show("Questionire Answer has not been provided!!");
                    return;
                }
                else
                {
                    obj_tbl_ProjectAnswer_Li = js.Deserialize<List<tbl_ProjectAnswer>>(hdf_Answer.Value);
                }
            }
            obj_tbl_ProjectQuestionnaire.obj_tbl_ProjectAnswer_Li = obj_tbl_ProjectAnswer_Li;
            obj_tbl_ProjectQuestionnaire_Li.Add(obj_tbl_ProjectQuestionnaire);
        }
        
        if (new DataLayer().Insert_tbl_Project_Questionire(obj_tbl_ProjectQuestionnaire_Li))
        {
            MessageBox.Show("Project Inspection Configuration Created Successfully ! ");
            reset();
            get_tbl_Project();
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
        ViewState["dtQuestionnaire"] = null;
        hf_Project_Id.Value = "0";
        get_tbl_Project();
        dgvQuestionnaire.DataSource =null;
        dgvQuestionnaire.DataBind();

        ViewState["dtAnswer"] = null;
        gdvAnswer.DataSource = null;
        gdvAnswer.DataBind();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["dtQuestionnaire"] = null;
        dgvQuestionnaire.DataSource = null;
        dgvQuestionnaire.DataBind();

        ViewState["dtAnswer"] = null;
        gdvAnswer.DataSource = null;
        gdvAnswer.DataBind();
        
        GridViewRow gr = ((sender as ImageButton).Parent.Parent as GridViewRow);
        int Project_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());
        hf_Project_Id.Value = Project_Id.ToString();
        
        DataSet ds = new DataLayer().get_tbl_ProjectQuestionnaire(Project_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            ViewState["dtQuestionnaire"] = ds.Tables[0];
            dgvQuestionnaire.DataSource = ds.Tables[0];
            dgvQuestionnaire.DataBind();
        }
        else
        {
            Add_Questionire();
        }
        divhide.Visible = true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
        int Project_Id = Convert.ToInt32(hf_Project_Id.Value);
        if (new DataLayer().Delete_Project(Project_Id, Person_Id))
        {
            MessageBox.Show("Deleted Successfully!!");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Deletion!!");
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

    protected void imgAddAnswer_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (GridViewRow)((sender as ImageButton).Parent.Parent);

        CheckBox chkAnswerDesc = gr.FindControl("chkAnswerDesc") as CheckBox;
        if (chkAnswerDesc.Checked)
        {
            MessageBox.Show("Inspection Point is Marked For Descriptive Answer");
            return;
        }

        List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        
        HiddenField hdf_Answer = (gr.FindControl("hdf_Answer") as HiddenField);
        if (hdf_Answer.Value == "")
        {
            tbl_ProjectAnswer onj_tbl_ProjectAnswer = new tbl_ProjectAnswer();
            onj_tbl_ProjectAnswer.ProjectAnswer_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            onj_tbl_ProjectAnswer.ProjectAnswer_Name = "";
            onj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = 0;
            onj_tbl_ProjectAnswer.ProjectAnswer_Id = 0;
            onj_tbl_ProjectAnswer.ProjectAnswer_Status = 1;
            obj_tbl_ProjectAnswer_Li.Add(onj_tbl_ProjectAnswer);
        }
        else
        {
            obj_tbl_ProjectAnswer_Li = js.Deserialize<List<tbl_ProjectAnswer>>(hdf_Answer.Value);
        }

        gdvAnswer.DataSource = obj_tbl_ProjectAnswer_Li;
        gdvAnswer.DataBind();

        hdf_AnswerGlobal.Value = js.Serialize(obj_tbl_ProjectAnswer_Li) + "|" + gr.RowIndex.ToString();
        mp1.Show();
    }

    protected void imgdelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["dtQuestionnaire"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtQuestionnaire"];
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            dgvQuestionnaire.DataSource = dt;
            dgvQuestionnaire.DataBind();
            ViewState["dtQuestionnaire"] = dt;
        }
    }

    protected void btnQuestionnaire_Click(object sender, EventArgs e)
    {
        Add_Questionire();
    }

    private void Add_Questionire()
    {
        DataTable dtQuestionnaire;
        if (ViewState["dtQuestionnaire"] != null)
        {
            dtQuestionnaire = (DataTable)(ViewState["dtQuestionnaire"]);
            DataRow dr = dtQuestionnaire.NewRow();
            dtQuestionnaire.Rows.Add(dr);
            ViewState["dtQuestionnaire"] = dtQuestionnaire;

            dgvQuestionnaire.DataSource = dtQuestionnaire;
            dgvQuestionnaire.DataBind();
        }
        else
        {
            dtQuestionnaire = new DataTable();

            DataColumn dc_ProjectQuestionnaire_ProjectId = new DataColumn("ProjectQuestionnaire_ProjectId", typeof(int));
            DataColumn dc_ProjectQuestionnaire_Id = new DataColumn("ProjectQuestionnaire_Id", typeof(int));
            DataColumn dc_ProjectQuestionnaire_Name = new DataColumn("ProjectQuestionnaire_Name", typeof(string));


            dtQuestionnaire.Columns.AddRange(new DataColumn[] { dc_ProjectQuestionnaire_ProjectId, dc_ProjectQuestionnaire_Id, dc_ProjectQuestionnaire_Name });

            DataRow dr = dtQuestionnaire.NewRow();
            dtQuestionnaire.Rows.Add(dr);
            ViewState["dtQuestionnaire"] = dtQuestionnaire;

            dgvQuestionnaire.DataSource = dtQuestionnaire;
            dgvQuestionnaire.DataBind();

        }
    }

    protected void imgdeleteAnswer_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int index = gr.RowIndex;
        if (ViewState["dtAnswer"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtAnswer"];
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            gdvAnswer.DataSource = dt;
            gdvAnswer.DataBind();
            ViewState["dtAnswer"] = dt;
        }
        mp1.Show();
    }

    public void AddAnswer()
    {
        DataTable dtAnswer;
        if (ViewState["dtAnswer"] != null)
        {
            dtAnswer = (DataTable)(ViewState["dtAnswer"]);
            DataRow dr = dtAnswer.NewRow();
            dtAnswer.Rows.Add(dr);
            ViewState["dtAnswer"] = dtAnswer;

            gdvAnswer.DataSource = dtAnswer;
            gdvAnswer.DataBind();
        }
        else
        {
            dtAnswer = new DataTable();

            DataColumn dc_ProjectAnswer_ProjectQuestionnaireId = new DataColumn("ProjectAnswer_ProjectQuestionnaireId", typeof(int));
            DataColumn dc_ProjectAnswer_Id = new DataColumn("ProjectAnswer_Id", typeof(int));
            DataColumn dc_ProjectAnswer_Name = new DataColumn("ProjectAnswer_Name", typeof(string));
            
            dtAnswer.Columns.AddRange(new DataColumn[] { dc_ProjectAnswer_ProjectQuestionnaireId, dc_ProjectAnswer_Id, dc_ProjectAnswer_Name });

            DataRow dr = dtAnswer.NewRow();
            dtAnswer.Rows.Add(dr);
            ViewState["dtAnswer"] = dtAnswer;

            gdvAnswer.DataSource = dtAnswer;
            gdvAnswer.DataBind();
        }
    }

    protected void btnAddItemAnswer_Click(object sender, ImageClickEventArgs e)
    {
        AddAnswer();
        mp1.Show();
    }

    protected void btnSaveAnswer_Click(object sender, EventArgs e)
    {
        List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
        TextBox txtProjectAnswer_Name;
        for (int i = 0; i < gdvAnswer.Rows.Count; i++)
        {
            txtProjectAnswer_Name = gdvAnswer.Rows[i].FindControl("txtProjectAnswer_Name") as TextBox;
            if (txtProjectAnswer_Name.Text.Trim() == "")
            {
                MessageBox.Show("Please Input Answer!!");
                txtProjectAnswer_Name.Focus();
                mp1.Show();
                return;
            }
            tbl_ProjectAnswer obj_tbl_ProjectAnswer = new tbl_ProjectAnswer();
            obj_tbl_ProjectAnswer.ProjectAnswer_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            try
            {
                obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = Convert.ToInt32(gdvAnswer.Rows[i].Cells[0].Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = 0;
            }
            try
            {
                obj_tbl_ProjectAnswer.ProjectAnswer_Id = Convert.ToInt32(gdvAnswer.Rows[i].Cells[1].Text.Trim());
            }
            catch
            {
                obj_tbl_ProjectAnswer.ProjectAnswer_Id = 0;
            }
            obj_tbl_ProjectAnswer.ProjectAnswer_Name = txtProjectAnswer_Name.Text.Trim();
            obj_tbl_ProjectAnswer.ProjectAnswer_Status = 1;
            obj_tbl_ProjectAnswer_Li.Add(obj_tbl_ProjectAnswer);
        }
        if (obj_tbl_ProjectAnswer_Li.Count == 0)
        {
            MessageBox.Show("Please Provide Atlease One Answer");
            mp1.Show();
            return;
        }
        string[] _data = hdf_AnswerGlobal.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        int Indx = Convert.ToInt32(_data[1]);
        HiddenField hdf_Answer = (dgvQuestionnaire.Rows[Indx].FindControl("hdf_Answer") as HiddenField);
        JavaScriptSerializer js = new JavaScriptSerializer();
        hdf_Answer.Value = js.Serialize(obj_tbl_ProjectAnswer_Li);
        hdf_AnswerGlobal.Value = "";
    }

    protected void dgvQuestionnaire_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int QuestionnaireId = 0;
            try
            {
                QuestionnaireId = Convert.ToInt32(e.Row.Cells[1].Text.Trim());
            }
            catch
            {
                QuestionnaireId = 0;
            }
            if (QuestionnaireId > 0)
            {
                DataSet ds = new DataLayer().get_tbl_ProjectAnswer(QuestionnaireId);
                if (AllClasses.CheckDataSet(ds))
                {
                    HiddenField hdf_Answer = (e.Row.FindControl("hdf_Answer") as HiddenField);
                    List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectAnswer obj_tbl_ProjectAnswer = new tbl_ProjectAnswer();
                        obj_tbl_ProjectAnswer.ProjectAnswer_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_AddedBy"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_ProjectQuestionnaireId"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_Id"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_Name = ds.Tables[0].Rows[i]["ProjectAnswer_Name"].ToString();
                        obj_tbl_ProjectAnswer.ProjectAnswer_Status = 1;
                        obj_tbl_ProjectAnswer_Li.Add(obj_tbl_ProjectAnswer);
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    hdf_Answer.Value = js.Serialize(obj_tbl_ProjectAnswer_Li);
                }
                else
                {
                    CheckBox chkAnswerDesc = (e.Row.FindControl("chkAnswerDesc") as CheckBox);
                    chkAnswerDesc.Checked = true;
                }
            }
        }
    }

    protected void btnAddYes_No_Click(object sender, EventArgs e)
    {
        DataTable dtAnswer;
        dtAnswer = new DataTable();

        DataColumn dc_ProjectAnswer_ProjectQuestionnaireId = new DataColumn("ProjectAnswer_ProjectQuestionnaireId", typeof(int));
        DataColumn dc_ProjectAnswer_Id = new DataColumn("ProjectAnswer_Id", typeof(int));
        DataColumn dc_ProjectAnswer_Name = new DataColumn("ProjectAnswer_Name", typeof(string));

        dtAnswer.Columns.AddRange(new DataColumn[] { dc_ProjectAnswer_ProjectQuestionnaireId, dc_ProjectAnswer_Id, dc_ProjectAnswer_Name });

        DataRow dr = dtAnswer.NewRow();
        dr["ProjectAnswer_Id"] = 0;
        dr["ProjectAnswer_Name"] = "Yes";
        dtAnswer.Rows.Add(dr);

        dr = dtAnswer.NewRow();
        dr["ProjectAnswer_Id"] = 0;
        dr["ProjectAnswer_Name"] = "No";
        dtAnswer.Rows.Add(dr);

        ViewState["dtAnswer"] = dtAnswer;

        gdvAnswer.DataSource = dtAnswer;
        gdvAnswer.DataBind();
        mp1.Show();
    }
}
