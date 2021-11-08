
using System;
using System.Collections.Generic;

namespace ePayment_API.Models
{
    public class DPRWorkUpdate2
    {
        public int User_Role_Id { get; set; }
        public int Person_Id { get; set; }
        public int ProjectUC_FinancialYear_Id { get; set; }
        public decimal BudgetUtilized { get; set; }
        public decimal PhysicalProgress { get; set; }
        public decimal Total_Centage { get; set; }
        public decimal Total_Allocated { get; set; }
        public string Comments { get; set; }
        public int ProjectDPR_Id { get; set; }
        public int ProjectWork_Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int NP_JurisdictionId { get; set; }
        public int SchemeId { get; set; }
        public List<tbl_ProjectUC_Concent> obj_tbl_ProjectUC_Concent_Li { get; set; }
        public List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress_Li { get; set; }
        public List<tbl_ProjectPkg_Deliverables> obj_tbl_ProjectPkg_Deliverables_Li { get; set; }
        public string ProjectDPRSitePics_SitePic_PathB1 { get; set; }
        public string ProjectDPRSitePics_SitePic_Base64_B1 { get; set; }
        public string ProjectDPRSitePics_SitePic_PathB2 { get; set; }
        public string ProjectDPRSitePics_SitePic_Base64_B2 { get; set; }
        public string ProjectDPRSitePics_SitePic_PathA1 { get; set; }
        public string ProjectDPRSitePics_SitePic_Base64_A1 { get; set; }
        public string ProjectDPRSitePics_SitePic_PathA2 { get; set; }
        public string ProjectDPRSitePics_SitePic_Base64_A2 { get; set; }
        public string  Person_AE { get; set; }
        public string  Person_JE { get; set; }
        public string Person_Vendor { get; set; }
        public string Project_Completion_Date { get; set; }
        public string Project_Status { get; set; }
    }
}
