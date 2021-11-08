using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for ReportClass
/// </summary>

[Serializable]
public class Bill_Info
{
    public string Organization_Name { get; set; }
    public string Organization_Address { get; set; }
    public string Organization_Mobile { get; set; }
    public string Organization_EmailID { get; set; }
    public string Organization_GSTIN { get; set; }
    public string Organization_TAN { get; set; }
    public string Organization_PAN { get; set; }
    public string Division_Name { get; set; }
    public string Circle_Name { get; set; }
    public string Zone_Name { get; set; }
    public string Division_Address { get; set; }
    public string Division_Mobile { get; set; }
    public string Division_EmailID { get; set; }
    public string Division_GSTIN { get; set; }
    public string Division_PAN { get; set; }
    public string Division_TAN { get; set; }
    public string Contractor_Name { get; set; }
    public string Contractor_Address { get; set; }
    public string Contractor_Mobile { get; set; }
    public string Contractor_EmailID { get; set; }
    public string Contractor_GSTIN { get; set; }
    public string Contractor_PAN { get; set; }
    public string Contractor_TAN { get; set; }
    public string Invoice_No { get; set; }
    public string Invoice_Date { get; set; }
    public string DBR_No { get; set; }
    public string SBR_No { get; set; }
    public string Narration { get; set; }
    public string EMB_Specification { get; set; }
    public string EMB_Unit_Name { get; set; }
    public decimal EMB_Qty { get; set; }
    public decimal EMB_Rate { get; set; }
    public decimal EMB_Amount { get; set; }
    public string EMB_Amount_In_Words { get; set; }
    public string EMB_Date { get; set; }
    public string EMB_Ref_No { get; set; }
    public string Scheme_Name { get; set; }
    public string Agreement_No { get; set; }
    public string Start_Date { get; set; }
    public string End_Date { get; set; }
    public decimal Invoie_Amount_Final { get; set; }
    public string Additional_Data_1 { get; set; }
    public string Additional_Data_2 { get; set; }
    public string Additional_Data_3 { get; set; }
    public string Additional_Data_4 { get; set; }
    public string Additional_Data_5 { get; set; }
    public string Additional_Data_6 { get; set; }
    public string Additional_Data_7 { get; set; }
    public string Additional_Data_8 { get; set; }
    public string Additional_Data_9 { get; set; }
    public string Additional_Data_10 { get; set; }
}

[Serializable]
public class PackageInvoiceAdditional
{
    public string Deduction_Name { get; set; }
    public decimal Deduction_Value { get; set; }
    public string Value_Type { get; set; }
}

[Serializable]
public class Invoice_View
{
    public List<Bill_Info> obj_Bill_Info_Li { get; set; }
    public List<PackageInvoiceAdditional> obj_PackageInvoiceAdditional_Li { get; set; }
}

[Serializable]
public class ProjectSummery
{
    public string Project_Name { get; set; }
    public string Contractor_Name { get; set; }
    public decimal Project_Cost { get; set; }
    public decimal Work_Cost { get; set; }
    public string Work_Name { get; set; }
    public string Scheme_Name { get; set; }
    public decimal Sactioned_Cost { get; set; }
    public decimal Tender_Cost { get; set; }
    public decimal Tender_Cost_Less { get; set; }
    public decimal Total_Release { get; set; }
    public decimal Total_Centre_Share { get; set; }
    public decimal Total_State_Share { get; set; }
    public decimal Total_ULB_Share { get; set; }
    public decimal Total_Calculated { get; set; }
    public decimal Total_Payment_Earlier { get; set; }
    public decimal Total_Balance { get; set; }
    public decimal Total_Bill_Raised { get; set; }
    public decimal Total_Proposed_Payment_Jal_Nigam { get; set; }
    public decimal Total_With_Held_Amount { get; set; }
    public string Tender_Cost_Less_With_Text { get; set; }
    public string Installment_Condition { get; set; }
    public string Extra_Item_Condition { get; set; }
    public string Additional_Text_1 { get; set; }
    public string Additional_Text_2 { get; set; }
    public string Additional_Text_3 { get; set; }
    public string Additional_Text_4 { get; set; }
    public decimal Additional_Value_1 { get; set; }
    public decimal Additional_Value_2 { get; set; }
    public decimal Additional_Value_3 { get; set; }
    public decimal Additional_Value_4 { get; set; }
}

[Serializable]
public class Cover_Letter
{
    public string Place { get; set; }
    public string Financial_Year { get; set; }
    public string Project_Type { get; set; }
    public string Project_Id { get; set; }
    public string Project_Name { get; set; }
    public string Scheme_Name { get; set; }
    public string Work_Name { get; set; }
    public decimal Sanctioned_Amount_Without_Centage { get; set; }
    public decimal Total_Centre_Share { get; set; }
    public decimal Total_State_Share { get; set; }
    public decimal Total_ULB_Share { get; set; }
    public decimal Centage { get; set; }
    public decimal Tendred_Amount { get; set; }
    public decimal Release_To_Implementing_Agency { get; set; }
    public decimal Fund_Diverted { get; set; }
    public decimal Find_Received { get; set; }
    public decimal Amount_Received_To_Implementing_Agency_Including_Diversion { get; set; }
    public decimal Expenditure_Done_By_Implementing_Agency { get; set; }
    public decimal Balance_Amount_As_In_Bank_Statement { get; set; }
    public decimal Amount_Released_To_Division { get; set; }
    public decimal Expenditure_By_Division { get; set; }
    public string Contractor_Type { get; set; }
    public string Contractor_Firm_Name { get; set; }
    public string Contractor_Name { get; set; }
    public string Contractor_Address { get; set; }
    public string Contractor_Mobile { get; set; }
    public string Contractor_EmailID { get; set; }
    public string Contractor_GSTIN { get; set; }
    public string Contractor_PAN { get; set; }
    public string Contractor_TAN { get; set; }
    public string Contractor_Service_Tax { get; set; }
    public string Bank_Name { get; set; }
    public string Account_Number { get; set; }
    public string Account_Holder_Name { get; set; }
    public decimal Total_Amount_Paid_To_Contractor_Till_Date { get; set; }
    public decimal Total_Mobelization_Advance { get; set; }
    public decimal Total_Mobelization_Advance_Adjustment_Before_Bill { get; set; }
    public decimal Total_Mobelization_Advance_Adjustment_In_Current_Bill { get; set; }
    public decimal Total_Invoice_Value { get; set; }
    public string Project_Manager { get; set; }
    public string General_Manager { get; set; }
    public string Additional_Text_1 { get; set; }
    public string Additional_Text_2 { get; set; }
    public string Additional_Text_3 { get; set; }
    public string Additional_Text_4 { get; set; }
    public decimal Additional_Value_1 { get; set; }
    public decimal Additional_Value_2 { get; set; }
    public decimal Additional_Value_3 { get; set; }
    public decimal Additional_Value_4 { get; set; }
}

[Serializable]
public class Decleration_Letter
{
    public string Subject { get; set; }
    public string Zone_Name { get; set; }
    public string Circle_Name { get; set; }
    public string Division_Name { get; set; }
    public string Project_Name { get; set; }
    public string Project_Code { get; set; }
    public string Package_Name { get; set; }
    public string Package_Code { get; set; }
    public string Content { get; set; }
    public int Last_RA_Bill_No { get; set; }
    public string Last_RA_Bill_Date { get; set; }
    public string Additional_1 { get; set; }
    public string Additional_2 { get; set; }
    public string Additional_3 { get; set; }
    public string Additional_4 { get; set; }
}
