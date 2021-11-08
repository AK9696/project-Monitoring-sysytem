using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI;

public partial class BillView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Invoice_View"] != null)
        {
            Invoice_View obj_Invoice_View = (Invoice_View)Session["Invoice_View"];
            List<Bill_Info> obj_Bill_Info_Li = obj_Invoice_View.obj_Bill_Info_Li;
            List<PackageInvoiceAdditional> obj_PackageInvoiceAdditional_Li = obj_Invoice_View.obj_PackageInvoiceAdditional_Li;
            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/Crystal/Invoice_Print.rpt"));
            crystalReport.SetDataSource(obj_Bill_Info_Li);
            crystalReport.Subreports[0].SetDataSource(obj_PackageInvoiceAdditional_Li);
            crvBillView.EnableDrillDown = false;
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