using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GoogleMaps.LocationServices;

public partial class Work_Detail : System.Web.UI.Page
{
    private void BindGMap(DataTable datatable)
    {
        try
        {
            List<ProgramAddresses> AddressList = new List<ProgramAddresses>();
            foreach (DataRow dr in datatable.Rows)
            {
                try
                {
                    ProgramAddresses MapAddress = new ProgramAddresses();
                    MapAddress.lat = double.Parse(dr["ProjectUC_Latitude"].ToString());
                    MapAddress.lng = double.Parse(dr["ProjectUC_Longitude"].ToString());
                    AddressList.Add(MapAddress);
                }
                catch
                { }
            }

            string jsonString = JsonConvert.SerializeObject(AddressList);
            hf_Map_Data.Value = jsonString;
        }
        catch (Exception)
        {
            hf_Map_Data.Value = "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                int ProjectWorkPkg_Id = Convert.ToInt32(Request.QueryString["ProjectWorkPkg_Id"]);
                DataSet ds1 = new DataSet();
                ds1 = (new DataLayer()).get_tbl_ProjectDPR_Details(ProjectWorkPkg_Id);
                if (ds1 != null)
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        lblScheme.Text = ds1.Tables[0].Rows[0]["Project_Name"].ToString();
                        grdPost.DataSource = ds1.Tables[0];
                        grdPost.DataBind();
                    }
                    else
                    {
                        grdPost.DataSource = null;
                        grdPost.DataBind();
                    }
                    if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0)
                    {
                        grdCallProductDtls.DataSource = ds1.Tables[1];
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
                    if (ds1.Tables.Count > 2 && ds1.Tables[2].Rows.Count > 0)
                    {
                        grdSaveVisit.DataSource = ds1.Tables[2];
                        grdSaveVisit.DataBind();

                        BindGMap(ds1.Tables[2]);
                    }
                    else
                    {
                        grdSaveVisit.DataSource = null;
                        grdSaveVisit.DataBind();
                    }

                    if (ds1.Tables.Count > 3 && ds1.Tables[3].Rows.Count > 0)
                    {
                        set_Gallery(ds1.Tables[3]);
                    }
                    else
                    {
                        divGallery.InnerHtml = "Site Photo Details Not Found";
                    }

                    if (ds1.Tables.Count > 4 && ds1.Tables[4].Rows.Count > 0)
                    {
                        grdComiteeDetails.DataSource = ds1.Tables[4];
                        grdComiteeDetails.DataBind();
                    }
                    else
                    {
                        grdComiteeDetails.DataSource = null;
                        grdComiteeDetails.DataBind();
                    }

                    if (ds1.Tables.Count > 5 && ds1.Tables[5].Rows.Count > 0)
                    {
                        grdTender.DataSource = ds1.Tables[5];
                        grdTender.DataBind();
                    }
                    else
                    {
                        grdTender.DataSource = null;
                        grdTender.DataBind();
                    }
                }
                else
                {
                    grdPost.DataSource = null;
                    grdPost.DataBind();

                    grdSaveVisit.DataSource = null;
                    grdSaveVisit.DataBind();

                    grdCallProductDtls.DataSource = null;
                    grdCallProductDtls.DataBind();

                    grdComiteeDetails.DataSource = null;
                    grdComiteeDetails.DataBind();

                    grdTender.DataSource = null;
                    grdTender.DataBind();
                }
            }
            else
            {
                MessageBox.Show("Work Details Not Found!!");
                return;
            }
        }
    }
    private void set_Gallery(DataTable ds)
    {
        string URL = Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace("\\", "/");
        if (Page.Request.Url.Query.Trim() == "")
        {
            URL = Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace("\\", "/");
        }
        else
        {
            URL = Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace(Page.Request.Url.Query, "").Replace("\\", "/");
        }

        string _inner = "";
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            _inner += "<ul class=\"ace-thumbnails clearfix\">";

            _inner += "    <li>";
            _inner += "        <a href = '" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "' data-rel=\"colorbox\" class=\"cboxElement\">";
            _inner += "            <img width = \"150\" height=\"150\" alt=\"150x150\" src='" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "'>";
            _inner += "            <div class=\"text\">";
            _inner += "                <div class=\"inner\">" + ds.Rows[i]["ProjectDPRSitePics_ReportSubmitted"].ToString() + ", " + ds.Rows[i]["ProjectDPRSitePics_SitePic_Type"].ToString() + "</div>";
            _inner += "            </div>";
            _inner += "        </a>";
            _inner += "    </li>";

            if (i <= ds.Rows.Count)
                i++;
            else
                break;
            if (i >= ds.Rows.Count)
            {
                break;
            }
            _inner += "    <li>";
            _inner += "        <a href = '" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "' data-rel=\"colorbox\" class=\"cboxElement\">";
            _inner += "            <img width = \"150\" height=\"150\" alt=\"150x150\" src='" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "'>";
            _inner += "            <div class=\"text\">";
            _inner += "                <div class=\"inner\">" + ds.Rows[i]["ProjectDPRSitePics_ReportSubmitted"].ToString() + ", " + ds.Rows[i]["ProjectDPRSitePics_SitePic_Type"].ToString() + "</div>";
            _inner += "            </div>";
            _inner += "        </a>";
            _inner += "    </li>";

            if (i <= ds.Rows.Count)
                i++;
            else
                break;
            if (i >= ds.Rows.Count)
            {
                break;
            }
            _inner += "    <li>";
            _inner += "        <a href = '" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "' data-rel=\"colorbox\" class=\"cboxElement\">";
            _inner += "            <img width = \"150\" height=\"150\" alt=\"150x150\" src='" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "'>";
            _inner += "            <div class=\"text\">";
            _inner += "                <div class=\"inner\">" + ds.Rows[i]["ProjectDPRSitePics_ReportSubmitted"].ToString() + ", " + ds.Rows[i]["ProjectDPRSitePics_SitePic_Type"].ToString() + "</div>";
            _inner += "            </div>";
            _inner += "        </a>";
            _inner += "    </li>";

            if (i <= ds.Rows.Count)
                i++;
            else
                break;
            if (i >= ds.Rows.Count)
            {
                break;
            }
            _inner += "    <li>";
            _inner += "        <a href = '" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "' data-rel=\"colorbox\" class=\"cboxElement\">";
            _inner += "            <img width = \"150\" height=\"150\" alt=\"150x150\" src='" + URL + ds.Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString().Replace("&nbsp;", "") + "'>";
            _inner += "            <div class=\"text\">";
            _inner += "                <div class=\"inner\">" + ds.Rows[i]["ProjectDPRSitePics_ReportSubmitted"].ToString() + ", " + ds.Rows[i]["ProjectDPRSitePics_SitePic_Type"].ToString() + "</div>";
            _inner += "            </div>";
            _inner += "        </a>";
            _inner += "    </li>";

            _inner += "</ul>";
        }
        divGallery.InnerHtml = _inner;
    }
    protected void grdPost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDownload = (e.Row.FindControl("lnkAgreementUpload2_FilePath") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload);
            LinkButton lnkDownload1 = (e.Row.FindControl("lnkAgreementFile2") as LinkButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkDownload1);
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

    protected void grdSaveVisit_PreRender(object sender, EventArgs e)
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
    protected void lnkAgreementUpload2_FilePath_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[2].Text.Replace("&nbsp", "").ToString().Trim();
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
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void lnkAgreementFile2_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((sender) as LinkButton).Parent.Parent as GridViewRow;
        string File = gr.Cells[3].Text.Replace("&nbsp", "").ToString().Trim();
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
            }
            else
            {
                MessageBox.Show("File Not Find");
            }
        }
    }

    protected void btnUC_Click(object sender, ImageClickEventArgs e)
    {
        //int UC_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[4].Text.Trim());

        //DataSet ds = new DataSet();
        //ds = new DataLayer().get_tbl_ProjectDPR_UC_Report(UC_Id);
        //if (AllClasses.CheckDataSet(ds))
        //{
        //    List<Report_UC> obj_Report_UC_Li = new List<Report_UC>();

        //    Report_UC obj_Report_UC = new Report_UC();
        //    string ULB_Type = "";
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NN")
        //    {
        //        ULB_Type = "नगर निगम ";
        //    }
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NPP")
        //    {
        //        ULB_Type = "नगर पालिका परिषद् ";
        //    }
        //    if (ds.Tables[0].Rows[0]["ULB_Type"].ToString() == "NP")
        //    {
        //        ULB_Type = "नगर पंचायत ";
        //    }
        //    try
        //    {
        //        obj_Report_UC.Budget_Approved = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectDPR_BudgetAllocated"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Budget_Approved = "0";
        //    }
        //    obj_Report_UC.Comments = ds.Tables[0].Rows[0]["ProjectUC_PhysicalProgress"].ToString();
        //    obj_Report_UC.Designation_1 = "अवर अभियन्ता";
        //    obj_Report_UC.Designation_1_Sub_Text = "स्थानीय निकाय";
        //    obj_Report_UC.Designation_2 = "अधीशासी अधिकारी";
        //    obj_Report_UC.Designation_2_Sub_Text = ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.Designation_3 = "अध्यक्ष";
        //    obj_Report_UC.Designation_3_Sub_Text = ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.District_Name_With_Caption = "जिला " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC.Expenditure_Percentage = ds.Tables[0].Rows[0]["ProjectUC_Achivment"].ToString();
        //    obj_Report_UC.GO_Date = ds.Tables[0].Rows[0]["ProjectWork_GO_Date"].ToString();
        //    obj_Report_UC.GO_No = ds.Tables[0].Rows[0]["ProjectWork_GO_No"].ToString();
        //    obj_Report_UC.GO_No_And_Date = "शाशानादेश संख्या: " + ds.Tables[0].Rows[0]["ProjectWork_GO_No"].ToString() + ", दिनांक: " + ds.Tables[0].Rows[0]["ProjectWork_GO_Date"].ToString();
        //    obj_Report_UC.Header_Text_Main = "कार्यालय " + ULB_Type + ds.Tables[0].Rows[0]["ULB_Name"].ToString() + ", " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC.Header_Text_Sub = "उपयोगिता प्रमाण - पत्र";
        //    obj_Report_UC.Note_Text = "प्रमाणित किया जाता है की सन्दर्भित योजना / कार्यक्रम के अंतर्गत स्वीकुत धनराशि से प्रस्तावित / स्वीकुत कार्य शाशनादेश की शर्तो के अनुसार कराये गए है तथा इसमें विचलन नहीं किया गया है |";
        //    obj_Report_UC.Project_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();
        //    try
        //    {
        //        obj_Report_UC.Total_Expenditure = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectUC_BudgetUtilized"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Total_Expenditure = "0";
        //    }
        //    try
        //    {
        //        obj_Report_UC.Total_Released = decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProjectUC_Total_Allocated"].ToString()) / 100000, 2, MidpointRounding.AwayFromZero).ToString();
        //    }
        //    catch
        //    {
        //        obj_Report_UC.Total_Released = "0";
        //    }
        //    obj_Report_UC.ULB_Name = ds.Tables[0].Rows[0]["ULB_Name"].ToString();
        //    obj_Report_UC.UC_File_Date = ds.Tables[0].Rows[0]["ProjectUC_SubmitionDate"].ToString();
        //    obj_Report_UC.ULB_Name_With_District = ds.Tables[0].Rows[0]["ULB_Name"].ToString() + ", " + ds.Tables[0].Rows[0]["District_Name"].ToString();
        //    obj_Report_UC_Li.Add(obj_Report_UC);

        //    string filePath = "\\Downloads\\UC\\";
        //    if (!Directory.Exists(Server.MapPath(".") + filePath))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(".") + filePath);
        //    }

        //    string fileName = UC_Id.ToString() + ".pdf";
        //    string webURI = "";
        //    if (Page.Request.Url.Query.Trim() == "")
        //    {
        //        webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "") + filePath + fileName).Replace("\\", "/");
        //    }
        //    else
        //    {
        //        webURI = (Page.Request.Url.AbsoluteUri.Replace(Page.Request.Url.AbsolutePath, "").Replace(Page.Request.Url.Query, "") + filePath + fileName).Replace("\\", "/");
        //    }

        //    ReportDocument crystalReport = new ReportDocument();
        //    crystalReport.Load(Server.MapPath("~/Crystal/Upyogita_Pramad.rpt"));
        //    crystalReport.SetDataSource(obj_Report_UC_Li);
        //    //crystalReport.ReportSource = crystalReport;
        //    //crystalReport.RefreshReport();
        //    crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath(".") + filePath + fileName);

        //    FileInfo fi = new FileInfo(Server.MapPath(".") + filePath + fileName);
        //    if (fi.Exists)
        //    {
        //        new AllClasses().Render_PDF_Document(ltEmbed, filePath + fileName);
        //        mp1.Show();
        //        #region For Open In Browser
        //        //WebClient User = new WebClient();
        //        //Byte[] FileBuffer = User.DownloadData(webURI);
        //        //if (FileBuffer != null)
        //        //{
        //        //    Response.ContentType = "application/pdf";
        //        //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
        //        //    Response.BinaryWrite(FileBuffer);
        //        //}
        //        #endregion

        //        #region For Download File
        //        //Response.ClearContent();
        //        //Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
        //        //Response.AddHeader("Content-Length", fi.Length.ToString());
        //        //string CId = Request["__EVENTTARGET"];
        //        //Response.TransmitFile(fi.FullName);
        //        //Response.End(); 
        //        #endregion
        //    }
        //    else
        //    {
        //        MessageBox.Show("Unable To Download File.");
        //        return;
        //    }
        //}
        //else
        //{
        //    MessageBox.Show("Unable To Generate UC.");
        //    return;
        //}
    }

    protected void grdSaveVisit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnUC = (e.Row.FindControl("btnUC") as ImageButton);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUC);

            LinkButton lnkPhysicalProgress= (e.Row.FindControl("lnkPhysicalProgress") as LinkButton);
            Label lblPhysicalProgress= (e.Row.FindControl("lblPhysicalProgress") as Label);
            lnkPhysicalProgress.Visible = false;
            lblPhysicalProgress.Visible = false;
            if (e.Row.Cells[11].Text.Trim() == "ExtendedTracking")
            {
                lnkPhysicalProgress.Visible = true;
            }
            else
            {
                lblPhysicalProgress.Visible = true;
            }
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
            FileInfo fi = new FileInfo(ConfigurationManager.AppSettings["BaseURL_GO"].ToString() + File);
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

    protected void grdCallProductDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkGO = (e.Row.FindControl("lnkGO") as LinkButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkGO);
        }
    }

    protected void grdComiteeDetails_PreRender(object sender, EventArgs e)
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

    protected void grdTender_PreRender(object sender, EventArgs e)
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

    protected void btnForm_Click(object sender, ImageClickEventArgs e)
    {
        int UC_Id = Convert.ToInt32(((sender as ImageButton).Parent.Parent as GridViewRow).Cells[4].Text.Trim());

        DataSet ds = new DataSet();
        ds = new DataLayer().get_tbl_ProjectDPR_UC_Questionire_Form(UC_Id);
        if (AllClasses.CheckDataSet(ds))
        {
            grdQuestionire.DataSource = ds.Tables[0];
            grdQuestionire.DataBind();
            mp2.Show();
        }
        else
        {
            grdQuestionire.DataSource = null;
            grdQuestionire.DataBind();
            MessageBox.Show("No Inspection Questionire Available.");
            return;
        }
    }

   

    protected void grdQuestionire_PreRender(object sender, EventArgs e)
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

    protected void lnkPhysicalProgress_Click(object sender, EventArgs e)
    {
        int UC_Id = Convert.ToInt32(((sender as LinkButton).Parent.Parent as GridViewRow).Cells[0].Text.Trim());

        DataSet ds = new DataSet();
        ds = new DataLayer().get_tbl_ProjectDPR_UC_PhysicalProgressComponentAndDeliverables_Form(UC_Id);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPhysicalProgress.DataSource = ds.Tables[0];
            grdPhysicalProgress.DataBind();
            mp2.Show();
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                grdDeliverables.DataSource = ds.Tables[1];
                grdDeliverables.DataBind();

            }
            else
            {
                grdDeliverables.DataSource = null;
                grdDeliverables.DataBind();

            }
        }
        else
        {
            grdPhysicalProgress.DataSource = null;
            grdPhysicalProgress.DataBind();
            MessageBox.Show("No Physical Progress Component Available.");
            return;
        }

    }
}