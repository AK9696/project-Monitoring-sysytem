
using System.Collections.Generic;

namespace ePayment_API.Models
{
    public class tbl_ProjectDPR
    {
        public string ProjectDPR_Comments { get; set; }
        public int ProjectDPR_District_Jurisdiction_Id { get; set; }
        public string District_Name { get; set; }
        public int ProjectDPR_Id { get; set; }
        public int ProjectWork_Id { get; set; }
        public int ProjectDPR_NP_JurisdictionId { get; set; }
        public string ULB_Name { get; set; }
        public int ProjectDPR_Project_Id { get; set; }
        public string Project_Name { get; set; }
        public string ProjectDPR_SubmitionDate { get; set; }
        public string ProjectDPR_UploadedOn { get; set; }
        public string ProjectDPR_Upload_Comments { get; set; }
        public int ProjectDPR_UploadedBy { get; set; }
        public decimal ProjectDPR_BudgetAllocated { get; set; }
        public string ProjectDPR_BudgetAllocatedOn { get; set; }
        public string ProjectDPR_BudgetAllocationComments { get; set; }
        public decimal Fund_Allocated_Total { get; set; }
        public decimal Fund_Expenditure_Total { get; set; }
        public decimal Fund_Remaining { get; set; }
        public string Vendor_Name { get; set; }
        public int Vendor_Person_Id { get; set; }
        public string ProjectDPR_FilePath1 { get; set; }
        public string ProjectDPR_FilePath2 { get; set; }
        public string ProjectWork_GO_Path { get; set; }
        public int ProjectDPRTenderInfo_Id { get; set; }
        public decimal ProjectWork_Budget { get; set; }
        public string ProjectWork_GO_Date { get; set; }
        public string ProjectWork_GO_No { get; set; }
        public string Vendor_Mobile { get; set; }
        public string Physical_Progress { get; set; }
        public string Financial_Progress { get; set; }
        public string Financial_Progress_Per { get; set; }
        public string PhysicalProgressTrackingType { get; set; }
    }
}
