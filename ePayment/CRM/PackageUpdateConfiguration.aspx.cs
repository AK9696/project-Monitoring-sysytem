using CrystalDecisions.CrystalReports.Engine;
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
public partial class PackageUpdateConfiguration : System.Web.UI.Page
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
            Session["FileName"] = null;
            Session["FileBytes"] = null;
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
                    //ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
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
                    //ddlDistrict_SelectedIndexChanged(ddlDistrict, e);
                    //ddlDistrict.Enabled = false;
                    if (Session["UserType"].ToString() == "3" && Convert.ToInt32(Session["ULB_Id"].ToString()) > 0)
                    {//ULB
                        try
                        {
                            //ddlULB.SelectedValue = Session["ULB_Id"].ToString();
                            //ddlULB.Enabled = false;
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
        if (ddlSearchScheme.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Scheme");
            ddlSearchScheme.Focus();
            return;
        }
        if (ddlZone.SelectedValue == "0")
        {
            MessageBox.Show("Please Select A Zone");
            return;
        }
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
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, Project_Id, 0, Zone_Id, Circle_Id, Division_Id, 0, "", "", false,"");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPost.DataSource = ds.Tables[0];
            grdPost.DataBind();
            divData.Visible = true;
            divEntry.Visible = false;
        }
        else
        {
            divData.Visible = false;
            divEntry.Visible = false;
            grdPost.DataSource = null;
            grdPost.DataBind();
            MessageBox.Show("No Package Details Found");
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
        divEntry.Visible = true;
        for (int i = 0; i < grdPost.Rows.Count; i++)
        {
            grdPost.Rows[i].BackColor = Color.Transparent;
        }
        GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
        gr.BackColor = Color.LightGreen;

        hf_ProjectWorkPkg_Id.Value = gr.Cells[0].Text.Trim();
        divEntry.Focus();
    }


    protected void btnUpdatePackage_Click(object sender, EventArgs e)
    {
        try
        {
            tbl_ProjectWorkPkg obj_tbl_ProjectWork = new tbl_ProjectWorkPkg();
            try
            {
                obj_tbl_ProjectWork.ProjectWorkPkg_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
            }
            catch
            {
                obj_tbl_ProjectWork.ProjectWorkPkg_Id = 0;
            }

            if (Session["FileBytes"] != null)
            {
                obj_tbl_ProjectWork.ProjectWorkPkg_ApprovalFile_Path_Bytes = (Byte[])Session["FileBytes"];
                obj_tbl_ProjectWork.ProjectWorkPkg_ApprovalFile_Extention = Session["FileName"].ToString();
                obj_tbl_ProjectWork.ProjectWorkPkg__ApprovalRemark = txtRemark.Text;
            }
            else
            {
                MessageBox.Show("Please Approval File Uploaded");
                return;
            }
            if ((new DataLayer()).UpdateApproval_tbl_ProjectWorkPkg(obj_tbl_ProjectWork))
            {
                MessageBox.Show("Package Updated Successfully!");
                Session["FileName"] = null;
                Session["FileBytes"] = null;
                btnSearch_Click(null, null);
                return;
            }
            else
            {
                MessageBox.Show("Error In Creating Project Package!");
                return;
            }
        }
        catch
        {

        }
    }

    protected void btnDownload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gr = (sender as ImageButton).Parent.Parent as GridViewRow;
            List<Decleration_Letter> obj_Decleration_Letter_Li = new List<Decleration_Letter>();
            string ProjectWorkPkg_Id = gr.Cells[0].Text.Trim();
            Decleration_Letter obj_Decleration_Letter = new Decleration_Letter();
            obj_Decleration_Letter.Circle_Name = gr.Cells[8].Text.Trim();
            obj_Decleration_Letter.Division_Name = gr.Cells[9].Text.Trim();
            obj_Decleration_Letter.Package_Name = gr.Cells[15].Text.Trim();
            obj_Decleration_Letter.Package_Code = gr.Cells[14].Text.Trim();
            obj_Decleration_Letter.Project_Name = gr.Cells[12].Text.Trim();
            obj_Decleration_Letter.Project_Code = gr.Cells[11].Text.Trim();
            try
            {
                obj_Decleration_Letter.Last_RA_Bill_No = Convert.ToInt32(gr.Cells[24].Text.Trim());
            }
            catch
            { }
            obj_Decleration_Letter.Last_RA_Bill_Date = gr.Cells[25].Text.Trim();

            obj_Decleration_Letter.Subject = "अमृत कार्यक्रम के अंतर्गत " + obj_Decleration_Letter.Division_Name + " के " + obj_Decleration_Letter.Project_Code + " के पैकेज " + obj_Decleration_Letter.Package_Code + " के डेटा को Freeze करने के सम्बन्ध में |";
            string _content = "";
            _content += "उपरोक्त विषयक अवगत कराना है कि इस इकाई द्वारा अमृत कार्यक्रम के अंतर्गत " + obj_Decleration_Letter.Project_Name + " के पैकेज " + obj_Decleration_Letter.Package_Name + " से सम्बंधित निम्न सूचनाओं को पोर्टल http://www.jnupepayment.in पर अपडेट की जा चुकी है:-";
            _content += Environment.NewLine;
            _content += "Last RA Bill No: " + obj_Decleration_Letter.Last_RA_Bill_No.ToString();
            _content += Environment.NewLine;
            _content += "Last RA Bill Date: " + obj_Decleration_Letter.Last_RA_Bill_Date;
            _content += Environment.NewLine;
            _content += "Quantity Paid Till Date: Updated";
            _content += Environment.NewLine;
            _content += "Updation Of Percentage Payment: Updated (If Applicable)";
            _content += Environment.NewLine;
            _content += "Item Wise GST Applicability & Rate: Updated";
            _content += Environment.NewLine;
            _content += "अतः अनुरोध है की उक्त पैकेज की BOQ को Freeze करने की कृपा करें, जिससे की पैकेजों की पोर्टल पर ऑनलाइन eMB कर बीजकों को भुगतान हेतु अमृत निदेशालय प्रेषित किया जा सके |";

            obj_Decleration_Letter.Content = _content;

            obj_Decleration_Letter_Li.Add(obj_Decleration_Letter);

            string filePath = "\\Downloads\\Decleration\\";
            if (!Directory.Exists(Server.MapPath(".") + filePath))
            {
                Directory.CreateDirectory(Server.MapPath(".") + filePath);
            }

            string fileName = ProjectWorkPkg_Id + ".pdf";

            if (File.Exists(Server.MapPath(".") + filePath + fileName))
            {
                File.Delete(Server.MapPath(".") + filePath + fileName);
            }
            string webURI = "";
            if (Page.Request.Url.Query.Trim() == "")
            {
                webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "") + filePath + fileName).Replace("\\", "/");
            }
            else
            {
                webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace(Page.Request.Url.Query, "") + filePath + fileName).Replace("\\", "/");
            }

            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/Crystal/Decleration.rpt"));
            crystalReport.SetDataSource(obj_Decleration_Letter_Li);
            //crystalReport.ReportSource = crystalReport;
            //crystalReport.RefreshReport();
            crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath(".") + filePath + fileName);

            FileInfo fi = new FileInfo(Server.MapPath(".") + filePath + fileName);
            if (fi.Exists)
            {
                new AllClasses().Render_PDF_Document(ltEmbed, filePath + fileName);
                mp1.Show();
                #region For Open In Browser
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(webURI);
                //if (FileBuffer != null)
                //{
                //    Response.ContentType = "application/pdf";
                //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //    Response.BinaryWrite(FileBuffer);
                //}
                #endregion

                #region For Download File
                //Response.ClearContent();
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                //Response.AddHeader("Content-Length", fi.Length.ToString());
                //string CId = Request["__EVENTTARGET"];
                //Response.TransmitFile(fi.FullName);
                //Response.End(); 
                #endregion
            }
            else
            {
                MessageBox.Show("Unable To Download File.");
                return;
            }
        }
        catch(Exception ee)
        {
            MessageBox.Show(ee.Message);
        }
    }
}