using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Package_Import : System.Web.UI.Page
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
            Server.ScriptTimeout = 900;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!flUpload.HasFile)
        {
            MessageBox.Show("Please Upload A File!!");
            return;
        }
        string fileName = DateTime.Now.Ticks.ToString("x") + flUpload.FileName;
        string filePathFull = Server.MapPath(".") + "\\BOQ\\" + fileName;
        string filePath = "\\BOQ\\" + flUpload.FileName;
        string File = Path.GetFileNameWithoutExtension(flUpload.FileName);
        hdnFile.Value = File;
        hdnFileName.Value = filePathFull;
        flUpload.SaveAs(filePathFull);

        FileInfo fl = new FileInfo(filePathFull);
        if (fl.Exists)
        {
            OnPostImportFromExcel(fl, true, "");
        }
    }
    private DataTable process_Data(DataTable BOQData)
    {
        if (AllClasses.CheckDt(BOQData))
        {
            DataColumn dc = new DataColumn("Specification", typeof(string));
            if (!BOQData.Columns.Contains("Specification"))
            {
                BOQData.Columns.Add(dc);
            }
            int ProcessCounter = 0;
            for (int i = 0; i < BOQData.Rows.Count; i++)
            {
                string Project_Code = BOQData.Rows[i][1].ToString().Trim();
                if (Project_Code == "")
                {
                    BOQData.Rows[ProcessCounter]["Specification"] = BOQData.Rows[ProcessCounter]["Specification"] + Environment.NewLine + BOQData.Rows[i][3].ToString().Trim();
                    BOQData.Rows[i][3] = "";
                }
                else
                {
                    ProcessCounter = i;
                }
            }

        Restart:
            for (int i = 0; i < BOQData.Rows.Count; i++)
            {
                if (BOQData.Rows[i][3].ToString().Trim() == "")
                {
                    BOQData.Rows.RemoveAt(i);
                    goto Restart;
                }
            }
            grdProcessedData.AutoGenerateColumns = true;
            grdProcessedData.DataSource = BOQData;
            grdProcessedData.DataBind();

            List<string> _columnsList = new List<string>();

            for (int i = 0; i < grdProcessedData.Columns.Count; i++)
            {
                if (grdProcessedData.Columns[i].Visible == true)
                {
                    _columnsList.Add(null);
                }
            }
            _columnsList.Add(null);
            string[] columnsList = new string[_columnsList.Count];
            columnsList = _columnsList.ToArray();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            hf_dt_Options_Dynamic1.Value = jss.Serialize(columnsList);

        }
        return BOQData;
    }

    public void OnPostImportFromExcel(FileInfo flExcel, bool Bind_Sheet_DDL, string Sheet_Name)
    {
        string connString = "";
        if (flExcel.Extension.ToLower() == "xls")
        {
            //connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + flExcel.FullName + ";Extended Properties=Excel 8.0";
            connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + flExcel.FullName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
        }
        else
        {
            //connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + flExcel.FullName + ";Extended Properties=Excel 12.0";
            connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + flExcel.FullName + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
        }
        hdnFileName.Value = flExcel.FullName;
        // Create the connection object
        OleDbConnection oledbConn = new OleDbConnection(connString);
        try
        {
            // Open connection
            oledbConn.Open();

            //Get Sheet Name
            DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (Bind_Sheet_DDL)
            {
                AllClasses.FillDropDown_WithOutSelect(dt, ddlExcelSheet, "TABLE_NAME", "TABLE_NAME");
            }
            else
            {
                if (dt == null)
                {
                    grdRawData.DataSource = null;
                    grdRawData.DataBind();
                    return;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                OleDbCommand cmd = null;
                // Create OleDbCommand object and select data from worksheet Sheet1
                if (Sheet_Name == "")
                    cmd = new OleDbCommand("SELECT * FROM [" + excelSheets[0] + "]", oledbConn);
                else
                    cmd = new OleDbCommand("SELECT * FROM [" + Sheet_Name + "]", oledbConn);
                
                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Create a DataSet which will hold the data extracted from the worksheet.
                DataSet ds = new DataSet();

                // Fill the DataSet from the data extracted from the worksheet.
                oleda.Fill(ds);

                // Bind the data to the GridView
                grdRawData.AutoGenerateColumns = true;

                grdRawData.DataSource = ds.Tables[0];
                grdRawData.DataBind();

                List<string> _columnsList = new List<string>();

                for (int j = 0; j < grdRawData.Columns.Count; j++)
                {
                    if (grdRawData.Columns[j].Visible == true)
                    {
                        _columnsList.Add(null);
                    }
                }
                _columnsList.Add(null);
                string[] columnsList = new string[_columnsList.Count];
                columnsList = _columnsList.ToArray();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                hf_dt_Options_Dynamic.Value = jss.Serialize(columnsList);

                DataTable dtProcessedData = process_Data(ds.Tables[0]);

                btnUpload.Text = "Upload (Data: " + dtProcessedData.Rows.Count.ToString() + ")";

                ViewState["BOQData"] = dtProcessedData;
            }
        }
        catch (Exception ee)
        {
            ViewState["BOQData"] = null;
            MessageBox.Show(ee.Message);
        }
        finally
        {
            // Close connection
            oledbConn.Close();
        }
    }
    private List<string> get_Data_Colums(DataTable dataTable, bool bind_ddl, DropDownList ddl)
    {
        List<string> obj_Data_Colums = new List<string>();
        obj_Data_Colums.Add("-Select-");
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            obj_Data_Colums.Add(dataTable.Columns[i].Caption);
        }
        if (bind_ddl)
        {
            ddl.DataSource = obj_Data_Colums;
            ddl.DataBind();
            
            DataTable dtMapping = new DataTable();
            dtMapping.Columns.Add(new DataColumn("Sheet_Column_Name", typeof(string)));
            DataRow dr;
            for (int i = 1; i < obj_Data_Colums.Count; i++)
            {
                dr = dtMapping.NewRow();
                dr["Sheet_Column_Name"] = obj_Data_Colums[i];
                dtMapping.Rows.Add(dr);
            }
            //grdColums.DataSource = dtMapping;
            //grdColums.DataBind();
        }
        return obj_Data_Colums;
    }
    protected void btnMatchParty_Click(object sender, EventArgs e)
    {
        if (ViewState["BOQData"] != null)
        {
            DataTable dt_Tally_Data = (DataTable)ViewState["BOQData"];
            List<string> obj_Data_Colums = get_Data_Colums(dt_Tally_Data, false, null);
            DataSet dsFinal_Data = new DataLayer().Insert_BOQ_Data_And_Map(obj_Data_Colums, dt_Tally_Data, Convert.ToInt32(Session["Person_Id"].ToString()), "", "");
            if (dsFinal_Data != null)
            {
                if (dsFinal_Data != null && dsFinal_Data.Tables.Count > 0 && dsFinal_Data.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Data Matched Completed. Total Unmatched Records are:- " + dsFinal_Data.Tables[0].Rows.Count.ToString());
                    grdPost.AutoGenerateColumns = true;
                    grdPost.DataSource = dsFinal_Data.Tables[0];
                    grdPost.DataBind();
                }
                else
                {
                    MessageBox.Show("Data Matched Completed. Total Unmatched Records are:- 0");
                    grdPost.DataSource = null;
                    grdPost.DataBind();
                }
            }
            else
            {
                MessageBox.Show("Unable To Process and Map Data!!");
            }
        }
        else
        {
            MessageBox.Show("Data Not Available To Match..!!");
            return;
        }
    }

    private void reset()
    {
        btnUpload.Text = "Upload";
        ViewState["BOQData"] = null;
        hf_dt_Options_Dynamic.Value = "";
        hf_dt_Options_Dynamic1.Value = "";
        grdPost.DataSource = null;
        grdPost.DataBind();

        grdProcessedData.DataSource = null;
        grdProcessedData.DataBind();

        grdRawData.DataSource = null;
        grdRawData.DataBind();
    }

    protected void grdRawData_PreRender(object sender, EventArgs e)
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

    protected void grdProcessedData_PreRender(object sender, EventArgs e)
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

    protected void btnExtractData_Click(object sender, EventArgs e)
    {
        FileInfo fl = new FileInfo(hdnFileName.Value);
        if (fl.Exists)
        {
            OnPostImportFromExcel(fl, false, ddlExcelSheet.SelectedItem.Text);
        }
        else
        {
            MessageBox.Show("Please Upload A File First");
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

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string msg = "";
        DataTable dt_Tally_Data = (DataTable)ViewState["BOQData"];
        List<string> obj_Data_Colums = get_Data_Colums(dt_Tally_Data, false, null);
        if (new DataLayer().Insert_BOQ_Data(obj_Data_Colums, ref msg))
        {
            MessageBox.Show("Data Import Successful");
            reset();
            return;
        }
        else
        {
            MessageBox.Show("Error In Data Import..");
            return;
        }
    }
}
