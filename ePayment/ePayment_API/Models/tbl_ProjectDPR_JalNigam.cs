
using System.Collections.Generic;

namespace ePayment_API.Models
{
    public class tbl_ProjectDPR_JalNigam
    {
        public int Zone_Id { get; set; }
        public int Circle_Id { get; set; }
        public int Division_Id { get; set; }
        public string Zone_Name { get; set; }
        public string Circle_Name { get; set; }
        public string Division_Name { get; set; }
        public int ProjectWorkPkg_Work_Id { get; set; }
        public int ProjectWorkPkg_Id { get; set; }
        public int ProjectWork_Project_Id { get; set; }
        public string Project_Name { get; set; }
        public string ProjectWork_Name { get; set; }
        public string ProjectWork_ProjectCode { get; set; }
        public string ProjectWorkPkg_Code { get; set; }
        public string ProjectWorkPkg_Name { get; set; }
        public decimal ProjectWorkPkg_AgreementAmount { get; set; }
        public string ProjectWorkPkg_Agreement_Date { get; set; }
        public string ProjectWorkPkg_Due_Date { get; set; }
        public string ProjectWorkPkg_Agreement_No { get; set; }
        public decimal Fund_Allocated_Total { get; set; }
        public decimal Fund_Expenditure_Total { get; set; }
        public decimal Fund_Remaining { get; set; }
        public string Vendor_Name { get; set; }
        public int Vendor_Person_Id { get; set; }
        public string Vendor_Mobile { get; set; }
        public string ReportingStaff_JEAPE_Name { get; set; }
        public string ReportingStaff_AEPE_Name { get; set; }
        public string ProjectWork_GO_Path { get; set; }
        public decimal ProjectWork_Budget { get; set; }
        public string Physical_Progress { get; set; }
        public string Financial_Progress { get; set; }
        public string Financial_Progress_Per { get; set; }
        public string PhysicalProgressTrackingType { get; set; }
    }
}
