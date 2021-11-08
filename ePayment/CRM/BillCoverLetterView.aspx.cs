using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI;

public partial class BillCoverLetterView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Cover_Letter"] != null)
        {
            List<Cover_Letter> obj_Cover_Letter_Li = (List<Cover_Letter>)Session["Cover_Letter"];
            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/Crystal/Cover_Letter.rpt"));
            crystalReport.SetDataSource(obj_Cover_Letter_Li);
            crvBillView.ReportSource = crystalReport;
            crvBillView.RefreshReport();
        }
    }

    private string GetDefaultPrinter()
    {
        PrinterSettings settings = new PrinterSettings();
        foreach (string printer in PrinterSettings.InstalledPrinters)
        {
            settings.PrinterName = printer;
            if (settings.IsDefaultPrinter)
            {
                return printer;
            }
        }
        return string.Empty;
    }
}