using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI;

public partial class BillPaymentSummeryView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ProjectSummery"] != null)
        {
            List<ProjectSummery> obj_ProjectSummery_Li = (List<ProjectSummery>)Session["ProjectSummery"];
            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/Crystal/Summery.rpt"));
            crystalReport.SetDataSource(obj_ProjectSummery_Li);
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