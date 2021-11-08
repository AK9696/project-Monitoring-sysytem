using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


public partial class Fund_Allocation : System.Web.UI.Page
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            get_tbl_Deduction();
            get_tbl_Project();
            get_tbl_Zone();
            if (Session["UserType"].ToString() != "1")
            {
                try
                {
                    if (Session["PersonJuridiction_Project_Id"].ToString() != "" && Session["PersonJuridiction_Project_Id"].ToString() != "0")
                    {
                        ddlSearchScheme.SelectedValue = Session["PersonJuridiction_Project_Id"].ToString();
                        ddlSearchScheme.Enabled = false;
                    }
                }
                catch
                {

                }

            }
            if (Session["UserType"].ToString() == "2" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {//District
                try
                {
                    //ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                   
                    //ddlDistrict.Enabled = false;
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_District_Id"].ToString()) > 0)
            {
                try
                {
                    //ddlDistrict.SelectedValue = Session["ULB_District_Id"].ToString();
                   
                    //ddlDistrict.Enabled = false;
                    if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
                    {//ULB
                        try
                        {
                            
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "4" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {//Zone
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "6" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                    if (Session["UserType"].ToString() == "6" && Convert.ToInt32(Session["PersonJuridiction_CircleId"].ToString()) > 0)
                    {//Circle
                        try
                        {
                            ddlCircle.SelectedValue = Session["PersonJuridiction_CircleId"].ToString();
                            ddlCircle_SelectedIndexChanged(ddlCircle, e);
                            ddlCircle.Enabled = false;
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }
            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_ZoneId"].ToString()) > 0)
            {
                try
                {
                    ddlZone.SelectedValue = Session["PersonJuridiction_ZoneId"].ToString();
                    ddlZone_SelectedIndexChanged(ddlZone, e);
                    ddlZone.Enabled = false;
                    if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_CircleId"].ToString()) > 0)
                    {//Circle
                        try
                        {
                            ddlCircle.SelectedValue = Session["PersonJuridiction_CircleId"].ToString();
                            ddlCircle_SelectedIndexChanged(ddlCircle, e);
                            ddlCircle.Enabled = false;
                            if (Session["UserType"].ToString() == "7" && Convert.ToInt32(Session["PersonJuridiction_DivisionId"].ToString()) > 0)
                            {//Circle
                                try
                                {
                                    ddlDivision.SelectedValue = Session["PersonJuridiction_DivisionId"].ToString();
                                    ddlDivision.Enabled = false;
                                }
                                catch
                                { }
                            }
                        }
                        catch
                        { }
                    }
                }
                catch
                { }
            }

            int Package_Id = 0;
            int EMB_Master_Id = 0;
            if (Request.QueryString.Count > 0)
            {
                try
                {
                    Package_Id = Convert.ToInt32(Request.QueryString["Package_Id"].ToString());
                }
                catch
                {
                    Package_Id = 0;
                }

                try
                {
                    EMB_Master_Id = Convert.ToInt32(Request.QueryString["EMB_Master_Id"].ToString());
                }
                catch
                {
                    EMB_Master_Id = 0;
                }

                get_tbl_PackageEMB_Approve(Package_Id, EMB_Master_Id);
            }
            else
            {
                EMB_Master_Id = 0;
                Package_Id = 0;
            }
        }
    }
    private void get_tbl_Deduction()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Deduction_Mode(0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Deduction"] = ds.Tables[0];
        }
        else
        {
            ViewState["Deduction"] = null;
        }
    }
    private void get_tbl_Zone()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Zone();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlZone, "Zone_Name", "Zone_Id");
        }
        else
        {
            ddlZone.Items.Clear();
        }
    }
    

    private void get_tbl_Circle(int Zone_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Circle(Zone_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlCircle, "Circle_Name", "Circle_Id");
        }
        else
        {
            ddlCircle.Items.Clear();
        }
    }
    private void get_tbl_Division(int Circle_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Division(Circle_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlDivision, "Division_Name", "Division_Id");
        }
        else
        {
            ddlDivision.Items.Clear();
        }
    }
    private void get_tbl_Project()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_Project();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            AllClasses.FillDropDown(ds.Tables[0], ddlSearchScheme, "Project_Name", "Project_Id");
        }
        else
        {
            ddlSearchScheme.Items.Clear();
        }
    }
    protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {
            ddlCircle.Items.Clear();
            ddlDivision.Items.Clear();
        }
        else
        {
            get_tbl_Circle(Convert.ToInt32(ddlZone.SelectedValue));
        }
    }

    protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCircle.SelectedValue == "0")
        {
            ddlDivision.Items.Clear();
        }
        else
        {
            get_tbl_Division(Convert.ToInt32(ddlCircle.SelectedValue));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            return;
        }
        int Package_Id = 0;
        int EMB_Master_Id = 0;
        if (Request.QueryString.Count > 0)
        {
            try
            {
                Package_Id = Convert.ToInt32(Request.QueryString["Package_Id"].ToString());
               
            }
            catch
            {
                Package_Id = 0;
            }

            try
            {
                EMB_Master_Id = Convert.ToInt32(Request.QueryString["EMB_Master_Id"].ToString());
            }
            catch
            {
                EMB_Master_Id = 0;
            }
        }
        else
        {
            EMB_Master_Id = 0;
            Package_Id = 0;
        }
        if (ddlSearchScheme.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            ddlSearchScheme.Focus();
            return;
        }
        get_tbl_PackageEMB_Approve(Package_Id, EMB_Master_Id);
    }
    private void get_tbl_PackageEMB_Approve(int Package_Id, int EMB_Master_Id)
    {
        int Project_Id = 0;
        int Zone_Id = 0;
        int Circle_Id = 0;
        int Division_Id = 0;

        try
        {
            Project_Id = Convert.ToInt32(ddlSearchScheme.SelectedValue);
        }
        catch
        {
            Project_Id = 0;
        }
        try
        {
            Zone_Id = Convert.ToInt32(ddlZone.SelectedValue);
        }
        catch
        {
            Zone_Id = 0;
        }
        try
        {
            Circle_Id = Convert.ToInt32(ddlCircle.SelectedValue);
        }
        catch
        {
            Circle_Id = 0;
        }
        try
        {
            Division_Id = Convert.ToInt32(ddlDivision.SelectedValue);
        }
        catch
        {
            Division_Id = 0;
        }
        DataSet ds = new DataSet();
        if (Session["UserType"].ToString() == "1")
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, Package_Id, "", "", 0, 0, EMB_Master_Id,"",false);
        }
        else
        {
            ds = (new DataLayer()).get_tbl_PackageEMB_Approve(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, Package_Id, "", "", Convert.ToInt32(Session["Person_BranchOffice_Id"].ToString()), Convert.ToInt32(Session["PersonJuridiction_DesignationId"].ToString()), EMB_Master_Id,"", false);
        }

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            divData.Visible = true;
            divDPRUpload.Visible = false;
        }
        else
        {
            divData.Visible = false;
            divDPRUpload.Visible = false;
            grdPost.DataSource = null;
            grdPost.DataBind();
            MessageBox.Show("No Records Found");
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
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divDPRUpload.Visible = true;
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        int Package_Id = Convert.ToInt32(gr.Cells[0].Text.Trim());

        int PackageWork_Id = 0;
        try
        {
            PackageWork_Id = Convert.ToInt32(gr.Cells[1].Text.Trim());
        }
        catch
        {
            PackageWork_Id = 0;
        }
        decimal Budget = 0;
       
        try
        {
            Budget = decimal.Parse(gr.Cells[19].Text.Trim());
        }
        catch
        {
            Budget = 0;
        }
        decimal Release = 0;

        try
        {
            Release = decimal.Parse(gr.Cells[20].Text.Trim());
        }
        catch
        {
            Release = 0;
        }
        hf_ProjectWork_Id.Value = Package_Id.ToString() + "|" + PackageWork_Id.ToString()+ "|" + Budget.ToString() + "|" + Release.ToString();
        gr.BackColor = Color.LightGreen;

        get_tbl_ProjectDPR_Transaction_Details(Package_Id, PackageWork_Id);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string[] _data = hf_ProjectWork_Id.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (_data == null)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (_data.Length != 4)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        int Package_Id = 0;
        decimal Total_Budget = 0;
       
        try
        {
            Package_Id = Convert.ToInt32(_data[0]);
        }
        catch
        {
            Package_Id = 0;
        }
        int Work_Id = 0;
        try
        {
            Work_Id = Convert.ToInt32(_data[1]);
        }
        catch
        {
            Work_Id = 0;
        }
        try
        {
            Total_Budget = Convert.ToDecimal(_data[2]);
        }
        catch
        {
            Total_Budget = 0;
        }
        decimal Total_Released = 0;
        try
        {
            Total_Released = Convert.ToDecimal(_data[3]);
        }
        catch
        {
            Total_Released = 0;
        }
        if (Package_Id == 0)
        {
            MessageBox.Show("Please Select A DPR Row.");
            return;
        }
        if (txtComments.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Comments");
            txtComments.Focus();
            return;
        }
        decimal Allocated_Budget = 0;
        try
        {
            Allocated_Budget = Convert.ToDecimal(txtAmount.Text.Trim());
        }
        catch
        {
            Allocated_Budget = 0;
        }
        if (Allocated_Budget == 0)
        {
            MessageBox.Show("Plese Provide Allocated Budget");
            txtAmount.Focus();
            return;
        }

        if (Allocated_Budget + Total_Released > Total_Budget)
        {
            MessageBox.Show("Total Release Amount Should be Less or Equal to Allocated Budget");
            txtAmount.Focus();
            return;
        }
        
        string filepath1 = "";
        Byte[] fileBytes1 = null;
        if (Session["FileBytes"] != null)
        {
            filepath1 = "\\Downloads\\GO\\" + DateTime.Now.ToString("MMddyyyyHHmmss") + "" + Session["FileName"].ToString();
            fileBytes1 = (Byte[])Session["FileBytes"];
        }
        else
        {
            fileBytes1 = null;
            filepath1 = "";
        }

        if (fileBytes1 == null)
        {
            MessageBox.Show("Please Upload File GO");
            return;
        }
        File_Objects obj_File_Objects = new File_Objects();
        obj_File_Objects.Package_Id = Package_Id;
        obj_File_Objects.Work_Id = Work_Id;
        obj_File_Objects.Added_By = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_File_Objects.File_Path_1 = filepath1;
        obj_File_Objects.File_Path_Bytes_1 = fileBytes1;
        obj_File_Objects.File_Path_2 = "";
        obj_File_Objects.File_Path_Bytes_2 = null;
        obj_File_Objects.Comments = txtComments.Text.Trim();

        tbl_FinancialTrans obj_tbl_FinancialTrans = new tbl_FinancialTrans();
        obj_tbl_FinancialTrans.FinancialTrans_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
        obj_tbl_FinancialTrans.FinancialTrans_Date = txtGODate.Text.Trim();
        obj_tbl_FinancialTrans.FinancialTrans_EntryType = "Fund Allocated";
        obj_tbl_FinancialTrans.FinancialTrans_Status = 1;
        obj_tbl_FinancialTrans.FinancialTrans_Comments = txtComments.Text.Trim();
        //obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = Convert.ToInt32(Session["FinancialYear_Id"].ToString());
        obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = 0;
        //int FY_Id = new DataLayer().get_FinancialYear_Id(txtGODate.Text.Trim());
        //if (FY_Id == 0)
        //    obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id2;
        //else
        //    obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = FY_Id;
        obj_tbl_FinancialTrans.FinancialTrans_Package_Id = Package_Id;
        obj_tbl_FinancialTrans.FinancialTrans_TransAmount = Allocated_Budget * 100000;
        obj_tbl_FinancialTrans.FinancialTrans_TransType = "C";
        obj_tbl_FinancialTrans.FinancialTrans_Work_Id = Work_Id;
        obj_tbl_FinancialTrans.FinancialTrans_FilePath1 = "";
        obj_tbl_FinancialTrans.FinancialTrans_GO_Date = txtGODate.Text.Trim();
        obj_tbl_FinancialTrans.FinancialTrans_GO_Number = AllClasses.re_Organize_GO_No(txtGONo.Text.Trim(), false);

        if (new DataLayer().Update_tbl_ProjectPackage_Fund_Allocation(obj_tbl_FinancialTrans, obj_File_Objects))
        {
            MessageBox.Show("Fund Allocated Successfully");
            btnSearch_Click(null, null);
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Unable To Allocate Fund");
            return;
        }
    }
    private void reset()
    {
        Session["FileName"] = null;
        Session["FileBytes"] = null;
        divDPRUpload.Visible = false;
        txtComments.Text = "";
        grdCallProductDtls.DataSource = null;
        grdCallProductDtls.DataBind();
        hf_ProjectWork_Id.Value = "0|0|0";
    }

    private void get_tbl_ProjectDPR_Transaction_Details(int Package_Id,int Work_Id)
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectPackage_Transaction_Details(Work_Id, Package_Id);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdCallProductDtls.DataSource = ds.Tables[0];
            grdCallProductDtls.DataBind();

            decimal TransAmount_C_Total = 0;
            decimal TransAmount_D_Total = 0;
            for (int i = 0; i < grdCallProductDtls.Rows.Count; i++)
            {
                decimal TransAmount_C = 0;
                decimal TransAmount_D = 0;
                try
                {
                    TransAmount_C = Convert.ToDecimal(grdCallProductDtls.Rows[i].Cells[8].Text.Trim());
                }
                catch
                {
                    TransAmount_C = 0;
                }

                try
                {
                    TransAmount_D = Convert.ToDecimal(grdCallProductDtls.Rows[i].Cells[9].Text.Trim());
                }
                catch
                {
                    TransAmount_D = 0;
                }
                if (i == 0)
                    grdCallProductDtls.Rows[i].Cells[10].Text = (TransAmount_C - TransAmount_D).ToString();
                else
                {
                    decimal Prev_Bal = 0;
                    try
                    {
                        Prev_Bal = Convert.ToDecimal(grdCallProductDtls.Rows[i - 1].Cells[10].Text.Trim());
                    }
                    catch
                    {
                        Prev_Bal = 0;
                    }
                    grdCallProductDtls.Rows[i].Cells[10].Text = (Prev_Bal + TransAmount_C - TransAmount_D).ToString();
                }
                TransAmount_C_Total += TransAmount_C;
                TransAmount_D_Total += TransAmount_D;
            }
            grdCallProductDtls.FooterRow.Cells[8].Text = TransAmount_C_Total.ToString();
            grdCallProductDtls.FooterRow.Cells[9].Text = TransAmount_D_Total.ToString();
            grdCallProductDtls.FooterRow.Cells[10].Text = grdCallProductDtls.Rows[grdCallProductDtls.Rows.Count - 1].Cells[10].Text;
        }
        else
        {
            grdCallProductDtls.DataSource = null;
            grdCallProductDtls.DataBind();
        }
    }

    protected void grdCallProductDtls_PreRender(object sender, EventArgs e)
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

    protected void grdCallProductDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkGO = (e.Row.FindControl("lnkGO") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkGO);
        }
    }

    protected void lnkGO_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[1].Text.Replace("&nbsp", "").ToString().Trim();
        if (File == "")
        {
            MessageBox.Show("File Not Find");
        }
        else
        {
            FileInfo fi = new FileInfo(Server.MapPath(".") + File);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, File);
                mp1.Show();
                //Response.ClearContent();
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                //Response.AddHeader("Content-Length", fi.Length.ToString());
                //string CId = Request["__EVENTTARGET"];
                //Response.TransmitFile(fi.FullName);
                //Response.End();
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }
}