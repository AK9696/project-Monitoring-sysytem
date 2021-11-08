
using System;

namespace ePayment_API.Models
{
    public class tbl_ProjectUC
    {
        public Decimal ProjectUC_Achivment { get; set; }
        public int ProjectUC_AddedBy { get; set; }
        public int ProjectUC_FinancialYear_Id { get; set; }
        public string ProjectUC_AddedOn { get; set; }
        public decimal ProjectUC_BudgetUtilized { get; set; }
        public decimal ProjectUC_Total_Allocated { get; set; }
        public decimal ProjectUC_PhysicalProgress { get; set; }
        public decimal ProjectUC_Centage { get; set; }
        public string ProjectUC_Comments { get; set; }
        public int ProjectUC_Id { get; set; }
        public int ProjectUC_ModifiedBy { get; set; }
        public string ProjectUC_ModifiedOn { get; set; }
        public int ProjectUC_ProjectDPR_Id { get; set; }
        public int ProjectUC_ProjectWork_Id { get; set; }
        public int ProjectUC_Status { get; set; }
        public string ProjectUC_SubmitionDate { get; set; }
        public decimal? ProjectUC_Latitude { get; set; }
        public decimal? ProjectUC_Longitude { get; set; }
        public string ProjectUC_UC_Path { get; set; }
        public string Person_Name { get; set; }
        public string Person_Mobile { get; set; }
        public string Level_Name { get; set; }
    }
}
