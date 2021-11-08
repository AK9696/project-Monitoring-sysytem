using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class DPRWorkUpdate2Repository : RepositoryAsyn
    {
        public DPRWorkUpdate2Repository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<bool> update_ProjectDPRWorkStatus(DPRWorkUpdate2 obj_DPRWorkUpdate2)
        {
            tbl_ProjectUC obj_tbl_ProjectUC = new tbl_ProjectUC();
            tbl_FinancialTrans obj_tbl_FinancialTrans = new tbl_FinancialTrans();
            List<tbl_ProjectUC_Concent> obj_tbl_ProjectUC_Concent_Li = new List<tbl_ProjectUC_Concent>();
            tbl_ProjectDPRSitePicsB1 obj_tbl_ProjectDPR_JalNigamSitePics_B1 = new tbl_ProjectDPRSitePicsB1();
            tbl_ProjectDPRSitePicsB2 obj_tbl_ProjectDPR_JalNigamSitePics_B2 = new tbl_ProjectDPRSitePicsB2();
            tbl_ProjectDPRSitePicsA1 obj_tbl_ProjectDPR_JalNigamSitePics_A1 = new tbl_ProjectDPRSitePicsA1();
            tbl_ProjectDPRSitePicsA2 obj_tbl_ProjectDPR_JalNigamSitePics_A2 = new tbl_ProjectDPRSitePicsA2();

            obj_tbl_ProjectUC.ProjectUC_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectUC.ProjectUC_BudgetUtilized = obj_DPRWorkUpdate2.BudgetUtilized * 100000;
            obj_tbl_ProjectUC.ProjectUC_Comments = obj_DPRWorkUpdate2.Comments;
            obj_tbl_ProjectUC.ProjectUC_FinancialYear_Id = obj_DPRWorkUpdate2.ProjectUC_FinancialYear_Id;
            obj_tbl_ProjectUC.ProjectUC_Latitude = obj_DPRWorkUpdate2.Latitude;
            obj_tbl_ProjectUC.ProjectUC_Longitude = obj_DPRWorkUpdate2.Longitude;
            obj_tbl_ProjectUC.ProjectUC_PhysicalProgress = obj_DPRWorkUpdate2.PhysicalProgress;
            obj_tbl_ProjectUC.ProjectUC_Centage = obj_DPRWorkUpdate2.Total_Centage;
            obj_tbl_ProjectUC.ProjectUC_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_ProjectUC.ProjectUC_ProjectWork_Id = obj_DPRWorkUpdate2.ProjectWork_Id;
            obj_tbl_ProjectUC.ProjectUC_Total_Allocated = obj_DPRWorkUpdate2.Total_Allocated * 100000;
            obj_tbl_ProjectUC.ProjectUC_Status = 1;
            try
            {
                obj_tbl_ProjectUC.ProjectUC_Achivment = (obj_tbl_ProjectUC.ProjectUC_BudgetUtilized * 100) / obj_tbl_ProjectUC.ProjectUC_Total_Allocated;
            }
            catch
            {
                obj_tbl_ProjectUC.ProjectUC_Achivment = 0;
            }
            obj_tbl_ProjectUC.ProjectUC_SubmitionDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_FinancialTrans.FinancialTrans_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_FinancialTrans.FinancialTrans_Comments = obj_DPRWorkUpdate2.Comments;
            obj_tbl_FinancialTrans.FinancialTrans_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_FinancialTrans.FinancialTrans_EntryType = "Fund Utilized";
            obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id = obj_DPRWorkUpdate2.ProjectUC_FinancialYear_Id;
            obj_tbl_FinancialTrans.FinancialTrans_Jurisdiction_Id = obj_DPRWorkUpdate2.NP_JurisdictionId;
            obj_tbl_FinancialTrans.FinancialTrans_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_FinancialTrans.FinancialTrans_SchemeId = obj_DPRWorkUpdate2.SchemeId;
            obj_tbl_FinancialTrans.FinancialTrans_Status = 1;
            obj_tbl_FinancialTrans.FinancialTrans_TransAmount = obj_DPRWorkUpdate2.BudgetUtilized * 100000;
            obj_tbl_FinancialTrans.FinancialTrans_TransType = "D";
            obj_tbl_FinancialTrans.FinancialTrans_WorkId = obj_DPRWorkUpdate2.ProjectWork_Id;

            obj_tbl_ProjectUC_Concent_Li = obj_DPRWorkUpdate2.obj_tbl_ProjectUC_Concent_Li;

            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_ProjectWork_Id = obj_DPRWorkUpdate2.ProjectWork_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_ReportSubmittedBy_PersonId = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Type = "B";
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Bytes1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_Base64_B1;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Path1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_PathB1;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_Status = 1;

            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_ProjectWork_Id = obj_DPRWorkUpdate2.ProjectWork_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Type = "B";
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_ReportSubmittedBy_PersonId = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Bytes1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_Base64_B2;
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Path1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_PathB2;
            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_Status = 1;

            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_ProjectWork_Id = obj_DPRWorkUpdate2.ProjectWork_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Type = "A";
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_ReportSubmittedBy_PersonId = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Bytes1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_Base64_A1;
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Path1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_PathA1;
            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_Status = 1;

            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_AddedBy = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_ProjectDPR_Id = obj_DPRWorkUpdate2.ProjectDPR_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_ProjectWork_Id = obj_DPRWorkUpdate2.ProjectWork_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Type = "A";
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_ReportSubmittedBy_PersonId = obj_DPRWorkUpdate2.Person_Id;
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Bytes1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_Base64_A2;
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Path1 = obj_DPRWorkUpdate2.ProjectDPRSitePics_SitePic_PathA2;
            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_Status = 1;

            return new DataLayer().Update_tbl_ProjectDPR_JalNigam_WorkStatus(obj_tbl_ProjectUC, obj_tbl_FinancialTrans, obj_tbl_ProjectUC_Concent_Li, obj_tbl_ProjectDPR_JalNigamSitePics_B1, obj_tbl_ProjectDPR_JalNigamSitePics_B2, obj_tbl_ProjectDPR_JalNigamSitePics_A1, obj_tbl_ProjectDPR_JalNigamSitePics_A2);
        }
    }
}
