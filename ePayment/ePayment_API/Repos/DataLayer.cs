using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class DataLayer
    {
        private DataSet ExecuteSelectQuerywithTransaction(SqlConnection Con, string Sql, SqlTransaction trans)
        {
            DataSet set1 = new DataSet();
            SqlCommand command1 = new SqlCommand(Sql, Con, trans);
            command1.CommandTimeout = 7000;
            SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
            adapter1.Fill(set1);
            return set1;
        }

        private DataSet ExecuteSelectQuery(string Sql)
        {
            string ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            DataSet set1 = new DataSet();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                SqlCommand command1 = new SqlCommand(Sql, con);
                if (command1.Connection.State == ConnectionState.Closed)
                {
                    command1.Connection.Open();
                }
                command1.CommandTimeout = 7000;
                SqlDataAdapter adapter1 = new SqlDataAdapter(command1);

                adapter1.Fill(set1);
            }
            return set1;
        }

        public void Insert_tbl_SMS(tbl_SMS obj_tbl_SMS)
        {
            DataSet set1 = new DataSet();
            SqlCommand command1 = null;
            SqlDataAdapter adapter1 = null;
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_SMS (SMS_Mobile_No, SMS_Content, SMS_Response, SMS_AddedOn) values ('" + obj_tbl_SMS.SMS_Mobile_No + "', '" + obj_tbl_SMS.SMS_Content + "','" + obj_tbl_SMS.SMS_Response + "', getdate());Select @@Identity";
            try
            {
                string ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    command1 = new SqlCommand(strQuery, con);
                    if (command1.Connection.State == ConnectionState.Closed)
                    {
                        command1.Connection.Open();
                    }
                    command1.CommandTimeout = 7000;
                    adapter1 = new SqlDataAdapter(command1);
                    adapter1.Fill(set1);
                }
            }
            catch
            {

            }
        }

        public DataSet get_M_Jurisdiction(int M_Level_Id, int Parent_Jurisdiction_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = " set dateformat dmy; select M_Jurisdiction.M_Jurisdiction_Id, M_Jurisdiction.M_Level_Id, M_Jurisdiction.Jurisdiction_Name_Eng, Jurisdiction_Name_With_Code = M_Jurisdiction.Jurisdiction_Name_Eng + ' - ' + isnull(M_Jurisdiction.Jurisdiction_Code, ''), M_Jurisdiction.Parent_Jurisdiction_Id, M_Jurisdiction.Jurisdiction_Code, Jurisdiction_Name_Eng_With_Parent = (case when isnull(Parent_M_Jurisdiction.Jurisdiction_Name_Eng, '') = '' then '' else isnull(Parent_M_Jurisdiction.Jurisdiction_Name_Eng, '-NA-') + '- ' end) + M_Jurisdiction.Jurisdiction_Name_Eng, M_Jurisdiction.Created_By, M_Jurisdiction.Created_Date, M_Jurisdiction.Is_Active, IsNULL(Parent_M_Jurisdiction.Jurisdiction_Name_Eng,'-NA-') Parent_Jurisdiction_Name_Eng, isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, M_Jurisdiction.Created_Date,tbl_PersonDetail1.Person_Name as ModifyBy,M_Jurisdiction.M_Jurisdiction_ModifiedOn from M_Jurisdiction left join M_Jurisdiction Parent_M_Jurisdiction on Parent_M_Jurisdiction.M_Jurisdiction_Id = M_Jurisdiction.Parent_Jurisdiction_Id left join tbl_PersonDetail on Person_Id = M_Jurisdiction.Created_By left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = M_Jurisdiction.M_Jurisdiction_ModifiedBy where M_Jurisdiction.Is_Active = 1 ";
            if (M_Level_Id != 0)
            {
                strQuery += " and M_Jurisdiction.M_Level_Id = '" + M_Level_Id + "'";
            }
            if (Parent_Jurisdiction_Id != 0)
            {
                strQuery += " and M_Jurisdiction.Parent_Jurisdiction_Id = '" + Parent_Jurisdiction_Id + "'";
            }
            strQuery += " order by M_Jurisdiction.M_Level_Id, Parent_M_Jurisdiction.Jurisdiction_Name_Eng, M_Jurisdiction.Jurisdiction_Name_Eng";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet getLoginDetails(string Mobile_No)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                        select 
                            tbl_PersonDetail.Person_Id,
	                        tbl_PersonDetail.Person_Name,
	                        Person_Name_Full = isnull(tbl_PersonDetail.Person_Name, '') + ', ' + ', Mob: ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                        tbl_PersonDetail.Person_Mobile1,
	                        tbl_PersonJuridiction.M_Level_Id,
	                        tbl_PersonJuridiction.M_Jurisdiction_Id,
	                        tbl_PersonJuridiction.PersonJuridiction_UserTypeId,
	                        Level_Name,
	                        UserType_Desc_E,
                            CONVERT(char(10),getdate(),103) ServerDate, 
                            Zone_Id, 
                            Circle_Id, 
	                        Division_Id, 
	                        Zone_Name, 
	                        Circle_Name, 
	                        Division_Name
                        from tbl_PersonDetail
                        join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                        left join tbl_Department on Department_Id = PersonJuridiction_DepartmentId
                        left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                        left join M_Level on M_Level.M_Level_Id = tbl_PersonJuridiction.M_Level_Id
                        left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                        left join tbl_UserLogin on Login_PersonId = Person_Id
                        left join tbl_Division on Division_Id = tbl_PersonJuridiction.PersonJuridiction_DivisionId
                        left join tbl_Circle on Circle_Id = tbl_PersonJuridiction.PersonJuridiction_CircleId
                        left join tbl_Zone on Zone_Id = tbl_PersonJuridiction.PersonJuridiction_ZoneId
                        where tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and tbl_PersonJuridiction.PersonJuridiction_UserTypeId in (7) and (tbl_PersonDetail.Person_Mobile1 = '" + Mobile_No + "' or tbl_PersonDetail.Person_Mobile2 = '" + Mobile_No + "'); ";

            strQuery += Environment.NewLine;

            strQuery += @"set dateformat dmy; 
                        select 
                            tbl_PersonDetail.Person_Id,
	                        tbl_PersonDetail.Person_Name,
	                        Person_Name_Full = isnull(tbl_PersonDetail.Person_Name, '') + ', ' + ', Mob: ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                        tbl_PersonDetail.Person_Mobile1,
	                        tbl_PersonJuridiction.M_Level_Id,
	                        tbl_PersonJuridiction.M_Jurisdiction_Id,
	                        tbl_PersonJuridiction.PersonJuridiction_UserTypeId
                        from tbl_PersonDetail
                        join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                        join 
                        (
	                        select 
                                distinct 
		                        ProjectWorkPkg_Vendor_Id 
	                        from tbl_ProjectWorkPkg 
	                        where ProjectWorkPkg_Status = 1
                        ) ProjectDPRTenderInfo on ProjectDPRTenderInfo.ProjectWorkPkg_Vendor_Id = Person_Id
                        where tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and tbl_PersonJuridiction.PersonJuridiction_UserTypeId = 5 and (tbl_PersonDetail.Person_Mobile1 = '" + Mobile_No + "' or tbl_PersonDetail.Person_Mobile2 = '" + Mobile_No + "'); ";

            strQuery += Environment.NewLine;

            strQuery += @"set dateformat dmy; 
                        select 
                            tbl_PersonDetail.Person_Id,
	                        tbl_PersonDetail.Person_Name,
	                        Person_Name_Full = isnull(tbl_PersonDetail.Person_Name, '') + ', ' + ', Mob: ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                        tbl_PersonDetail.Person_Mobile1,
	                        tbl_PersonJuridiction.M_Level_Id,
	                        tbl_PersonJuridiction.M_Jurisdiction_Id,
	                        tbl_PersonJuridiction.PersonJuridiction_UserTypeId
                        from tbl_PersonDetail
                        join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                        join 
                        (
	                        select 
                                distinct 
		                        ProjectPKGInspectionInfo_InspectionPersonId
	                        from tbl_ProjectPKGInspectionInfo 
	                        where ProjectPKGInspectionInfo_Status = 1
                        ) ProjectDPRInspectionInfo on ProjectDPRInspectionInfo.ProjectPKGInspectionInfo_InspectionPersonId = Person_Id
                        where tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and (tbl_PersonDetail.Person_Mobile1 = '" + Mobile_No + "' or tbl_PersonDetail.Person_Mobile2 = '" + Mobile_No + "'); ";

            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch (Exception)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_Project()
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        Project_Id, 
                        Project_Name, 
                        Project_Time, 
                        Project_Budget, 
                        Project_AddedOn, 
                        Project_AddedBy, 
                        Project_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Project_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = Project_ModifiedOn, 
                        Project_GO_Path, 
                        Project_AllocationType
                      from tbl_Project
                      left join tbl_PersonDetail on Person_Id = Project_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Project_ModifiedBy
                      where Project_Status = 1 order by Project_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }
        public DataSet get_tbl_ProjectDPR_Upload(int Person_Id, string _Mode, int ULB_Id, int Scheme_Id, string _Status)
        {
            string sqlJoin = "";
            string strQuery = "";
            DataSet ds = new DataSet();

            strQuery = @"set dateformat dmy; 
                        select 
	                        ProjectDPR_Id,
	                        ProjectDPR_Project_Id,
	                        ProjectDPR_District_Jurisdiction_Id,
	                        ProjectDPR_NP_JurisdictionId,
	                        convert(char(10), ProjectDPR_SubmitionDate, 103) ProjectDPR_SubmitionDate,
	                        ProjectDPR_Comments,
	                        convert(char(10), ProjectDPR_AddedOn, 103) ProjectDPR_AddedOn, 
	                        ProjectDPR_AddedBy,
	                        ProjectDPR_FilePath1,
	                        ProjectDPR_FilePath2,
	                        convert(char(10), ProjectDPR_UploadedOn, 103) ProjectDPR_UploadedOn,
	                        ProjectDPR_UploadedBy,
	                        ProjectDPR_Verified_Comments,
	                        ProjectDPR_IsVerified,
	                        ProjectDPR_Upload_Comments,
	                        ProjectDPR_VerifiedBy,
	                        ProjectDPR_BudgetAllocated = convert(decimal(18,2), (isnull(ProjectDPR_BudgetAllocated, 0) / 100000)),
	                        ProjectDPR_BudgetAllocatedBy,
	                        convert(char(10), ProjectDPR_BudgetAllocatedOn, 103) ProjectDPR_BudgetAllocatedOn,
	                        ProjectDPR_BudgetAllocationComments,
	                        convert(char(10), ProjectDPR_VerifiedOn, 103) ProjectDPR_VerifiedOn,
	                        ProjectDPR_Work_Id,
	                        ProjectDPR_SubmitionDate1 = convert(char(10), ProjectDPR_SubmitionDate, 103), 
	                        ProjectDPR_BudgetAllocatedOn1 = convert(char(10), ProjectDPR_BudgetAllocatedOn, 103), 
	                        M_Jurisdiction_NP.ULB_Name NP_Jurisdiction_Name_Eng, 
                            M_Jurisdiction.Jurisdiction_Name_Eng Jurisdiction_Name_Eng, 
                            Fund_Allocated = convert(decimal(18,2), (isnull(tFinancialTrans.FinancialTrans_TransAmount, 0) / 100000)) , 
                            FinancialTrans_Id = convert(decimal(18,2), (isnull(tFinancialTrans.FinancialTrans_Id, 0) / 100000)) , 
                            Total_Release = convert(decimal(18,2), (isnull(tStatement.TransAmount_C, 0) / 100000)) , 
                            Total_Expenditure = convert(decimal(18,2), (isnull(tStatement.TransAmount_D, 0) / 100000)) ,
                            Total_Available = convert(decimal(18,2), (isnull(tStatement.TransAmount_C, 0) / 100000)) - convert(decimal(18,2), (isnull(tStatement.TransAmount_D, 0) / 100000)), 
                            Vendor_Name = tbl_ProjectDPRTenderInfo.Person_Name, 
	                        tbl_ProjectDPRTenderInfo.ProjectDPRTenderInfo_Id, 
                            tAdditional.ProjectDPRAdditionalInfo_Designation,
	                        tAdditional.ProjectDPRAdditionalInfo_RecomendatorMobile, 
	                        tAdditional.ProjectDPRAdditionalInfo_RecomendatorName, 
	                        ProjectWork_Id,
	                        ProjectWork_Project_Id,
	                        ProjectWork_Name,
	                        ProjectWork_Status,
	                        ProjectWork_AddedBy,
	                        ProjectWork_GO_Path,
	                        ProjectWork_Budget,
	                        ProjectWork_GO_Date = convert(char(10), ProjectWork_GO_Date, 103),
	                        ProjectWork_GO_Date1 = convert(char(10), ProjectWork_GO_Date, 103), 
	                        ProjectWork_GO_No,
	                        ProjectWork_ProjectType_Id,
	                        ProjectWork_Target_Date = convert(char(10), ProjectWork_Target_Date, 103),
	                        ProjectWork_IsVerified, 
	                        tVerificationStatus.ProjectDPRStatus_Date, 
	                        tVerificationStatus.ProjectDPRStatus_Comments, 
	                        tVerificationStatus.DPR_Status_DPR_StatusName, 
	                        tVerificationStatus.ProjectDPRStatus_DPR_StatusId, 
                            tbl_FinancialTransGO.List_FT_GO, 
                            Physical_Progress = isnull(TUCDetails.ProjectUC_PhysicalProgress, 0),
                            Financial_Progress = convert(decimal(18,2), isnull(TUCDetails.ProjectUC_BudgetUtilized, 0) / 100000),
                            Financial_Progress_Per = isnull(TUCDetails.ProjectUC_Achivment, 0), 
                            M_Jurisdiction.Jurisdiction_Name_Eng District_Name, 
                            M_Jurisdiction_NP.ULB_Name NP_Jurisdiction_Name_Eng, 
                            M_Jurisdiction_NP.ULB_Name ULB_Name, 
                            M_Jurisdiction.Jurisdiction_Name_Eng Jurisdiction_Name_Eng,                             
                            Vendor_Name = tbl_ProjectDPRTenderInfo.Person_Name, 
                            Vendor_Mobile = tbl_ProjectDPRTenderInfo.Person_Mobile1, 
	                        tbl_ProjectDPRTenderInfo.ProjectDPRTenderInfo_Id, 
                            tAdditional.ProjectDPRAdditionalInfo_Designation,
	                        tAdditional.ProjectDPRAdditionalInfo_RecomendatorMobile, 
	                        tAdditional.ProjectDPRAdditionalInfo_RecomendatorName, 
                            ProjectDPR_PhysicalProgressTrackingType
                        from tbl_ProjectWork 
                        join tbl_ProjectDPR on ProjectWork_Id = ProjectDPR_Work_Id and ProjectWork_Status = 1
                        join M_Jurisdiction on ProjectDPR_District_Jurisdiction_Id = M_Jurisdiction_Id 
                        join tbl_ULB M_Jurisdiction_NP on M_Jurisdiction_NP.ULB_Id = tbl_ProjectDPR.ProjectDPR_NP_JurisdictionId
                        
                        left join (select ROW_NUMBER() over (partition by ProjectDPRAdditionalInfo_ProjectDPR_Id order by ProjectDPRAdditionalInfo_Id desc) rrrr, ProjectDPRAdditionalInfo_Id, ProjectDPRAdditionalInfo_ProjectDPR_Id, ProjectDPRAdditionalInfo_RecomendatorMobile, ProjectDPRAdditionalInfo_RecomendatorName, ProjectDPRAdditionalInfo_Designation from tbl_ProjectDPRAdditionalInfo where ProjectDPRAdditionalInfo_Status = 1) tAdditional on ProjectDPRAdditionalInfo_ProjectDPR_Id = ProjectDPR_Id and tAdditional.rrrr = 1
                        left join (select ROW_NUMBER() over (partition by ProjectDPRStatus_ProjectDPR_Id order by ProjectDPRStatus_Id desc) rrrr, ProjectDPRStatus_DPR_StatusId, ProjectDPRStatus_ProjectDPR_Id, ProjectDPRStatus_Date = convert(char(10), ProjectDPRStatus_Date, 103), ProjectDPRStatus_Comments, DPR_Status_DPR_StatusName from tbl_ProjectDPRStatus join tbl_DPR_Status on DPR_Status_Id = ProjectDPRStatus_DPR_StatusId where ProjectDPRStatus_Status = 1) tVerificationStatus on tVerificationStatus.ProjectDPRStatus_ProjectDPR_Id = ProjectDPR_Id and tVerificationStatus.rrrr = 1
                        leftCondHere join (select ROW_NUMBER() over (partition by ProjectDPRTenderInfo_ProjectDPR_Id, ProjectDPRTenderInfo_ProjectWork_Id order by ProjectDPRTenderInfo_Id desc) rrrT, ProjectDPRTenderInfo_ProjectDPR_Id, ProjectDPRTenderInfo_ProjectWork_Id, ProjectDPRTenderInfo_Id, ProjectDPRTenderInfo_VendorPersonId, Person_Name, Person_Mobile1 from tbl_ProjectDPRTenderInfo join tbl_PersonDetail on Person_Id = ProjectDPRTenderInfo_VendorPersonId) tbl_ProjectDPRTenderInfo on tbl_ProjectDPRTenderInfo.ProjectDPRTenderInfo_ProjectDPR_Id = ProjectDPR_Id and tbl_ProjectDPRTenderInfo.ProjectDPRTenderInfo_ProjectWork_Id = ProjectWork_Id and tbl_ProjectDPRTenderInfo.rrrT = 1
                        left join (select ROW_NUMBER() over (partition by FinancialTrans_ProjectDPR_Id, FinancialTrans_Jurisdiction_Id, FinancialTrans_SchemeId, FinancialTrans_WorkId, FinancialTrans_TransType order by FinancialTrans_Id asc) rrr, FinancialTrans_Jurisdiction_Id, FinancialTrans_Id, FinancialTrans_TransAmount, FinancialTrans_SchemeId, FinancialTrans_WorkId, FinancialTrans_ProjectDPR_Id from tbl_FinancialTrans where FinancialTrans_Status = 1 and FinancialTrans_TransType = 'C') tFinancialTrans on tFinancialTrans.FinancialTrans_SchemeId = ProjectDPR_Project_Id and tFinancialTrans.FinancialTrans_Jurisdiction_Id = ProjectDPR_NP_JurisdictionId and FinancialTrans_ProjectDPR_Id = ProjectDPR_Id and FinancialTrans_WorkId = ProjectDPR_Work_Id and rrr = 1
                        left join (select FinancialTrans_Jurisdiction_Id, FinancialTrans_ProjectDPR_Id, FinancialTrans_SchemeId, FinancialTrans_WorkId, TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end), TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end) from tbl_FinancialTrans where FinancialTrans_Status = 1 group by FinancialTrans_Jurisdiction_Id, FinancialTrans_SchemeId, FinancialTrans_ProjectDPR_Id, FinancialTrans_WorkId) tStatement on tStatement.FinancialTrans_SchemeId = ProjectDPR_Project_Id and tStatement.FinancialTrans_Jurisdiction_Id = ProjectDPR_NP_JurisdictionId and tStatement.FinancialTrans_ProjectDPR_Id = ProjectDPR_Id and tStatement.FinancialTrans_WorkId = ProjectDPR_Work_Id
                        left join (select ROW_NUMBER() over (partition by ProjectUC_ProjectDPR_Id, ProjectUC_ProjectWork_Id order by ProjectUC_Id desc) rrUC, ProjectUC_ProjectDPR_Id, ProjectUC_ProjectWork_Id, ProjectUC_Achivment, convert(date, ProjectUC_SubmitionDate, 103) ProjectUC_SubmitionDate, ProjectUC_BudgetUtilized, ProjectUC_PhysicalProgress from tbl_ProjectUC where ProjectUC_Status = 1) TUCDetails on TUCDetails.ProjectUC_ProjectDPR_Id = ProjectDPR_Id  and TUCDetails.ProjectUC_ProjectWork_Id = ProjectDPR_Work_Id and rrUC = 1
                        left join 
                        (
	                        SELECT	FinancialTrans_ProjectDPR_Id, 
			                        FinancialTrans_WorkId,
			                        STUFF((SELECT ', ' + CAST(FinancialTrans_GO_Number AS NVARCHAR(100)) + ' Dt: ' + convert(char(10), FinancialTrans_GO_Date, 103) [text()]
                                    FROM tbl_FinancialTrans 
                                    WHERE FinancialTrans_ProjectDPR_Id = t.FinancialTrans_ProjectDPR_Id and FinancialTrans_WorkId = t.FinancialTrans_WorkId and tbl_FinancialTrans.FinancialTrans_Status = 1 and tbl_FinancialTrans.FinancialTrans_TransType = 'C'
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') List_FT_GO
	                        FROM tbl_FinancialTrans t
                            where t.FinancialTrans_Status = 1 and t.FinancialTrans_TransType = 'C'
	                        GROUP BY FinancialTrans_ProjectDPR_Id, FinancialTrans_WorkId
                        ) tbl_FinancialTransGO on tbl_FinancialTransGO.FinancialTrans_ProjectDPR_Id = ProjectDPR_Id and tbl_FinancialTransGO.FinancialTrans_WorkId = ProjectWork_Id
                        AddJoinCondition
                        where ProjectDPR_Status = 1 and isnull(tFinancialTrans.FinancialTrans_TransAmount, 0) > 0 ";

            if (_Mode == "ULB")
            {
                strQuery = strQuery.Replace("AddJoinCondition", "");
                strQuery = strQuery.Replace("leftCondHere", "left");
                strQuery += " and (ProjectDPR_NP_JurisdictionId in (select PersonJuridiction_ULB_Id from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + Person_Id.ToString() + "') or ProjectDPR_NP_JurisdictionId in (select PersonAdditionalULB_ULB_Id from tbl_PersonAdditionalULB where PersonAdditionalULB_Person_Id = '" + Person_Id.ToString() + "'))";
            }
            else if (_Mode == "Vendor")
            {
                strQuery = strQuery.Replace("leftCondHere", "");
                strQuery = strQuery.Replace("AddJoinCondition", "");
                strQuery += "and tbl_ProjectDPRTenderInfo.ProjectDPRTenderInfo_VendorPersonId = " + Person_Id.ToString();
            }
            else if (_Mode == "Inspection")
            {
                sqlJoin += Environment.NewLine;
                sqlJoin += "join tbl_ProjectDPRInspectionInfo on ProjectDPRInspectionInfo_ProjectDPR_Id = ProjectDPR_Id and ProjectDPRInspectionInfo_ProjectWork_Id = ProjectWork_Id";
                strQuery = strQuery.Replace("AddJoinCondition", sqlJoin);
                strQuery = strQuery.Replace("leftCondHere", "left");
                strQuery += "and ProjectDPRInspectionInfo_InspectionPersonId = " + Person_Id.ToString();
            }
            else
            {
                strQuery = strQuery.Replace("AddJoinCondition", "");
                strQuery = strQuery.Replace("leftCondHere", "left");
            }
            if (ULB_Id > 0)
            {
                // strQuery += " and PersonJuridiction_PersonId = '" + ULB_Id.ToString() + "'";
                strQuery += " and (ProjectDPR_NP_JurisdictionId in (select PersonJuridiction_ULB_Id from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + ULB_Id.ToString() + "') or ProjectDPR_NP_JurisdictionId in (select PersonAdditionalULB_ULB_Id from tbl_PersonAdditionalULB where PersonAdditionalULB_Person_Id = '" + ULB_Id.ToString() + "')) ";
            }
            if (Scheme_Id > 0)
            {
                strQuery += " and ProjectDPR_Project_Id = '" + Scheme_Id.ToString() + "'";
            }
            if (_Status == "Completed")
            {
                strQuery += " and isnull(TUCDetails.ProjectUC_PhysicalProgress, 0)>=100 and isnull(TUCDetails.ProjectUC_Achivment, 0)>=100 ";
            }
            else
            {
                strQuery += " and (isnull(TUCDetails.ProjectUC_PhysicalProgress, 0) < 100 or isnull(TUCDetails.ProjectUC_Achivment, 0) < 100) ";
            }
            strQuery += " order by M_Jurisdiction.Jurisdiction_Name_Eng, M_Jurisdiction_NP.ULB_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_ProjectDPR_JalNigam_Upload(int Person_Id, string _Mode, int ULB_Id, int Scheme_Id, string _Status)
        {
            string sqlJoin = "";
            string strQuery = "";
            DataSet ds = new DataSet();

            strQuery = @"set dateformat dmy; 
                        select 
	                        ProjectWorkPkg_Id,
							ProjectWorkPkg_Work_Id,
							ProjectWorkPkg_Code, 
							ProjectWorkPkg_Name,
							ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
							ProjectWorkPkg_AgreementAmount,
							ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
							ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
							ProjectWorkPkg_Agreement_No,
							ProjectWorkPkg_Indent_No,
							ProjectWorkPkg_Vendor_Id,
							ProjectWorkPkg_Staff_Id,
							Vendor_Name = Vendor.Person_Name,
							Vendor_Mobile = Vendor.Person_Mobile1,
							List_ReportingStaff_JEAPE_Name,
							List_ReportingStaff_AEPE_Name,
							ProjectWork_Id, 
							ProjectWork_Project_Id, 
							Project_Name, 
							ProjectWork_ProjectCode,
							ProjectWork_Name, 
							ProjectWork_Description, 
							ProjectWork_GO_Path,
							ProjectWork_Budget,
							ProjectWork_AddedOn, 
							ProjectWork_AddedBy, 
							ProjectWork_ModifiedOn, 
							ProjectWork_ModifiedBy, 
							ProjectWork_Status,  
							Division_Name, 
							Circle_Name, 
							Zone_Name,
							ProjectWork_DistrictId, 
							ProjectWork_ULB_Id, 
							ProjectWork_DivisionId, 
							Division_CircleId, 
							tbl_FinancialTransGO.List_FT_GO, 
                            Physical_Progress = isnull(TUCDetails.ProjectUC_PhysicalProgress, 0),
                            Financial_Progress = convert(decimal(18,2), isnull(TUCDetails.ProjectUC_BudgetUtilized, 0) / 100000),
                            Financial_Progress_Per = isnull(TUCDetails.ProjectUC_Achivment, 0), 
                            ProjectDPR_PhysicalProgressTrackingType
                        from tbl_ProjectWorkPkg
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
                        join tbl_ProjectDPR on ProjectWork_Id = ProjectDPR_ProjectWork_Id and ProjectDPR_ProjectWorkPkg_Id = ProjectWorkPkg_Id and ProjectWork_Status = 1
						join tbl_Project on Project_Id = ProjectWork_Project_Id
						left join tbl_Division on Division_Id = ProjectWork_DivisionId
                        left join tbl_Circle on Circle_Id = Division_CircleId
                        left join tbl_Zone on Zone_Id = Circle_ZoneId
                        left join tbl_PersonDetail Vendor on Vendor.Person_Id = ProjectWorkPkg_Vendor_Id
						left join tbl_PersonDetail Staff on Staff.Person_Id = ProjectWorkPkg_Staff_Id
                        left join (select ROW_NUMBER() over (partition by FinancialTrans_Work_Id, FinancialTrans_Package_Id, FinancialTrans_TransType order by FinancialTrans_Id asc) rrr, FinancialTrans_Work_Id, FinancialTrans_Package_Id, FinancialTrans_Id, FinancialTrans_TransAmount from tbl_FinancialTrans where FinancialTrans_Status = 1 and FinancialTrans_TransType = 'C') tFinancialTrans on FinancialTrans_Work_Id = ProjectWork_Id and FinancialTrans_Package_Id = ProjectWorkPkg_Id and rrr = 1
                        
						left join (select FinancialTrans_Work_Id, FinancialTrans_Package_Id, TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end), TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end) from tbl_FinancialTrans where FinancialTrans_Status = 1 group by FinancialTrans_Work_Id, FinancialTrans_Package_Id) tStatement on tStatement.FinancialTrans_Work_Id = ProjectWork_Id and tStatement.FinancialTrans_Package_Id = ProjectWorkPkg_Id
                        
						left join (select ROW_NUMBER() over (partition by ProjectUC_ProjectPkg_Id, ProjectUC_ProjectWork_Id order by ProjectUC_Id desc) rrUC, ProjectUC_ProjectPkg_Id, ProjectUC_ProjectWork_Id, ProjectUC_Achivment, convert(date, ProjectUC_SubmitionDate, 103) ProjectUC_SubmitionDate, ProjectUC_BudgetUtilized, ProjectUC_PhysicalProgress from tbl_ProjectUC where ProjectUC_Status = 1) TUCDetails on TUCDetails.ProjectUC_ProjectPkg_Id = ProjectWorkPkg_Id and TUCDetails.ProjectUC_ProjectWork_Id = ProjectWork_Id and rrUC = 1
                        
						left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                            where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
						left join 
                        (
	                        SELECT	FinancialTrans_Work_Id, 
									FinancialTrans_Package_Id,
			                        STUFF((SELECT ', ' + CAST(FinancialTrans_GO_Number AS NVARCHAR(100)) + ' Dt: ' + convert(char(10), FinancialTrans_GO_Date, 103) [text()]
                                    FROM tbl_FinancialTrans 
                                    WHERE FinancialTrans_Work_Id = t.FinancialTrans_Work_Id and FinancialTrans_Package_Id = t.FinancialTrans_Package_Id and tbl_FinancialTrans.FinancialTrans_Status = 1 and tbl_FinancialTrans.FinancialTrans_TransType = 'C'
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') List_FT_GO
	                        FROM tbl_FinancialTrans t
                            where t.FinancialTrans_Status = 1 and t.FinancialTrans_TransType = 'C'
	                        GROUP BY FinancialTrans_Work_Id, FinancialTrans_Package_Id
                        ) tbl_FinancialTransGO on tbl_FinancialTransGO.FinancialTrans_Work_Id = ProjectWork_Id and tbl_FinancialTransGO.FinancialTrans_Package_Id = ProjectWorkPkg_Id
                        
                        where ProjectWorkPkg_Status = 1 ";

            if (_Mode == "ULB")
            {
                strQuery += @" and (ProjectWork_DivisionId in (select PersonJuridiction_DivisionId from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + Person_Id.ToString() + "') or ProjectWork_DivisionId in (select ProjectAdditionalArea_DevisionId from tbl_ProjectAdditionalArea inner join tbl_PersonJuridiction on PersonJuridiction_DivisionId = ProjectAdditionalArea_DevisionId where PersonJuridiction_PersonId = '" + Person_Id.ToString() + "'))";
            }
            else if (_Mode == "Vendor")
            {
                strQuery = strQuery.Replace("leftCondHere", "");
                strQuery = strQuery.Replace("AddJoinCondition", "");
                strQuery += "and tbl_ProjectDPR_JalNigamTenderInfo.ProjectDPRTenderInfo_VendorPersonId = " + Person_Id.ToString();
            }
            else if (_Mode == "Inspection")
            {
                sqlJoin += Environment.NewLine;
                sqlJoin += "join tbl_ProjectDPR_JalNigamInspectionInfo on ProjectDPRInspectionInfo_ProjectDPR_Id = ProjectDPR_Id and ProjectDPRInspectionInfo_ProjectWork_Id = ProjectWork_Id";
                strQuery = strQuery.Replace("AddJoinCondition", sqlJoin);
                strQuery = strQuery.Replace("leftCondHere", "left");
                strQuery += "and ProjectDPRInspectionInfo_InspectionPersonId = " + Person_Id.ToString();
            }
            else
            {
                strQuery = strQuery.Replace("AddJoinCondition", "");
                strQuery = strQuery.Replace("leftCondHere", "left");
            }
            if (ULB_Id > 0)
            {
                // strQuery += " and PersonJuridiction_PersonId = '" + ULB_Id.ToString() + "'";
                strQuery += " and (ProjectDPR_NP_JurisdictionId in (select PersonJuridiction_ULB_Id from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + ULB_Id.ToString() + "') or ProjectDPR_NP_JurisdictionId in (select PersonAdditionalULB_ULB_Id from tbl_PersonAdditionalULB where PersonAdditionalULB_Person_Id = '" + ULB_Id.ToString() + "')) ";
            }
            if (Scheme_Id > 0)
            {
                strQuery += " and ProjectDPR_Project_Id = '" + Scheme_Id.ToString() + "'";
            }
            if (_Status == "Completed")
            {
                strQuery += " and isnull(TUCDetails.ProjectUC_PhysicalProgress, 0)>=100 and isnull(TUCDetails.ProjectUC_Achivment, 0)>=100 ";
            }
            else
            {
                strQuery += " and (isnull(TUCDetails.ProjectUC_PhysicalProgress, 0) < 100 or isnull(TUCDetails.ProjectUC_Achivment, 0) < 100) ";
            }
            strQuery += " order by Zone_Name,Circle_Name,Division_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_ProjectQuestionnaire(int Project_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                        select 
                            ProjectQuestionnaire_Id,
                            ProjectQuestionnaire_ProjectId,
                            ProjectQuestionnaire_Name,
                            ProjectQuestionnaire_AddedBy,
                            ProjectQuestionnaire_AddedOn,
                            ProjectQuestionnaire_Status
                        from tbl_ProjectQuestionnaire 
                        where ProjectQuestionnaire_Status = 1 ";
            if (Project_Id > 0)
            {
                strQuery += " and ProjectQuestionnaire_ProjectId = '" + Project_Id + "'";
            }
            strQuery += " order by ProjectQuestionnaire_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }
        public DataSet get_tbl_ProjectAnswer(int Questionnaire_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        ProjectAnswer_Id,
                        ProjectAnswer_ProjectQuestionnaireId,
                        ProjectAnswer_Name,
                        ProjectAnswer_AddedBy,
                        ProjectAnswer_AddedOn,
                        ProjectAnswer_Status
                    from tbl_ProjectAnswer 
                    where ProjectAnswer_Status = 1 ";
            if (Questionnaire_Id > 0)
            {
                strQuery += " and ProjectAnswer_ProjectQuestionnaireId = '" + Questionnaire_Id + "'";
            }
            strQuery += " order by ProjectAnswer_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        private bool saveImageData(string file_Name, string base_64_Data, string UploadPath_Full)
        {
            try
            {
                string ext = file_Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
                byte[] bytes = Convert.FromBase64String(base_64_Data);
                if (bytes != null && bytes.Length > 0)
                {
                    using (Image image = Image.FromStream(new MemoryStream(bytes)))
                    {
                        if (!Directory.Exists(UploadPath_Full))
                        {
                            Directory.CreateDirectory(UploadPath_Full);
                        }
                        if (ext.ToLower() == "jpg" || ext.ToLower() == "jpeg")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Jpeg);
                        }
                        else if (ext.ToLower() == "bmp")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Bmp);
                        }
                        else if (ext.ToLower() == "gif")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Gif);
                        }
                        else if (ext.ToLower() == "icon")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Icon);
                        }
                        else if (ext.ToLower() == "png")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Png);
                        }
                        else if (ext.ToLower() == "tiff")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Tiff);
                        }
                        else if (ext.ToLower() == "wmf")
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Wmf);
                        }
                        else
                        {
                            image.Save(UploadPath_Full + file_Name, ImageFormat.Jpeg);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool Update_tbl_ProjectDPR_JalNigam_WorkStatus(tbl_ProjectUC obj_tbl_ProjectUC, tbl_FinancialTrans obj_tbl_FinancialTrans, List<tbl_ProjectUC_Concent> obj_tbl_ProjectUC_Concent_Li, tbl_ProjectDPRSitePicsB1 obj_tbl_ProjectDPR_JalNigamSitePics_B1, tbl_ProjectDPRSitePicsB2 obj_tbl_ProjectDPR_JalNigamSitePics_B2, tbl_ProjectDPRSitePicsA1 obj_tbl_ProjectDPR_JalNigamSitePics_A1, tbl_ProjectDPRSitePicsA2 obj_tbl_ProjectDPR_JalNigamSitePics_A2)
        {
            DataSet ds = new DataSet();
            bool iResult = false;
            string ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    obj_tbl_ProjectUC.ProjectUC_Id = Insert_tbl_ProjectUC(obj_tbl_ProjectUC, trans, cn);

                    if (obj_tbl_ProjectUC_Concent_Li != null)
                    {
                        for (int i = 0; i < obj_tbl_ProjectUC_Concent_Li.Count; i++)
                        {
                            obj_tbl_ProjectUC_Concent_Li[i].ProjectUC_Concent_ProjectUC_Id = obj_tbl_ProjectUC.ProjectUC_Id;
                            Insert_tbl_ProjectUC_Concent(obj_tbl_ProjectUC_Concent_Li[i], trans, cn);
                        }
                    }

                    if (obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Bytes1 != null && obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Bytes1.Length > 0)
                    {
                        if (saveImageData(obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Path1, obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Bytes1, System.Configuration.ConfigurationManager.AppSettings["BasePath"] + "Work\\"))
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Path1 = "\\Downloads\\Work\\" + obj_tbl_ProjectDPR_JalNigamSitePics_B1.ProjectDPRSitePics_SitePic_Path1;
                            Insert_tblProjectDPRSitePicsB1(obj_tbl_ProjectDPR_JalNigamSitePics_B1, obj_tbl_ProjectUC.ProjectUC_Id, trans, cn);
                        }
                    }

                    if (obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Bytes1 != null && obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Bytes1.Length > 0)
                    {
                        if (saveImageData(obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Path1, obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Bytes1, System.Configuration.ConfigurationManager.AppSettings["BasePath"] + "Work\\"))
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Path1 = "\\Downloads\\Work\\" + obj_tbl_ProjectDPR_JalNigamSitePics_B2.ProjectDPRSitePics_SitePic_Path1;
                            Insert_tblProjectDPRSitePicsB2(obj_tbl_ProjectDPR_JalNigamSitePics_B2, obj_tbl_ProjectUC.ProjectUC_Id, trans, cn);
                        }
                    }

                    if (obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Bytes1 != null && obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Bytes1.Length > 0)
                    {
                        if (saveImageData(obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Path1, obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Bytes1, System.Configuration.ConfigurationManager.AppSettings["BasePath"] + "Work\\"))
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Path1 = "\\Downloads\\Work\\" + obj_tbl_ProjectDPR_JalNigamSitePics_A1.ProjectDPRSitePics_SitePic_Path1;
                            Insert_tblProjectDPRSitePicsA1(obj_tbl_ProjectDPR_JalNigamSitePics_A1, obj_tbl_ProjectUC.ProjectUC_Id, trans, cn);
                        }
                    }

                    if (obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Bytes1 != null && obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Bytes1.Length > 0)
                    {
                        if (saveImageData(obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Path1, obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Bytes1, System.Configuration.ConfigurationManager.AppSettings["BasePath"] + "Work\\"))
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Path1 = "\\Downloads\\Work\\" + obj_tbl_ProjectDPR_JalNigamSitePics_A2.ProjectDPRSitePics_SitePic_Path1;
                            Insert_tblProjectDPRSitePicsA2(obj_tbl_ProjectDPR_JalNigamSitePics_A2, obj_tbl_ProjectUC.ProjectUC_Id, trans, cn);
                        }
                    }

                    obj_tbl_FinancialTrans.FinancialTrans_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                    obj_tbl_FinancialTrans.FinancialTrans_GO_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                    Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans, trans, cn);

                    iResult = true;
                    trans.Commit();
                    cn.Close();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    cn.Close();
                    iResult = false;
                }
            }
            return iResult;
        }

        private void Insert_tblProjectDPRSitePicsA1(tbl_ProjectDPRSitePicsA1 obj_tbl_ProjectDPR_JalNigamSitePics, int UCDetails_Id, SqlTransaction trans, SqlConnection cn)
        {
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_JalNigamSitePics ( [ProjectDPRSitePics_AddedBy],[ProjectDPRSitePics_AddedOn],[ProjectDPRSitePics_ProjectDPR_Id],[ProjectDPRSitePics_ProjectWork_Id],[ProjectDPRSitePics_ReportSubmitted],[ProjectDPRSitePics_ReportSubmittedBy_PersonId],[ProjectDPRSitePics_SitePic_Path1],[ProjectDPRSitePics_SitePic_Type],[ProjectDPRSitePics_Status], [ProjectDPRSitePics_UCDetails_Id]) values ('" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_AddedBy + "',getdate(),'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmitted + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Path1 + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Type + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Status + "', '" + UCDetails_Id + "');Select @@Identity";
            if (trans == null)
            {
                try
                {
                    ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
        }

        private void Insert_tblProjectDPRSitePicsA2(tbl_ProjectDPRSitePicsA2 obj_tbl_ProjectDPR_JalNigamSitePics, int UCDetails_Id, SqlTransaction trans, SqlConnection cn)
        {
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_JalNigamSitePics ( [ProjectDPRSitePics_AddedBy],[ProjectDPRSitePics_AddedOn],[ProjectDPRSitePics_ProjectDPR_Id],[ProjectDPRSitePics_ProjectWork_Id],[ProjectDPRSitePics_ReportSubmitted],[ProjectDPRSitePics_ReportSubmittedBy_PersonId],[ProjectDPRSitePics_SitePic_Path1],[ProjectDPRSitePics_SitePic_Type],[ProjectDPRSitePics_Status], [ProjectDPRSitePics_UCDetails_Id]) values ('" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_AddedBy + "',getdate(),'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmitted + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Path1 + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Type + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Status + "', '" + UCDetails_Id + "');Select @@Identity";
            if (trans == null)
            {
                try
                {
                    ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
        }

        private void Insert_tblProjectDPRSitePicsB1(tbl_ProjectDPRSitePicsB1 obj_tbl_ProjectDPR_JalNigamSitePics, int UCDetails_Id, SqlTransaction trans, SqlConnection cn)
        {
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_JalNigamSitePics ( [ProjectDPRSitePics_AddedBy],[ProjectDPRSitePics_AddedOn],[ProjectDPRSitePics_ProjectDPR_Id],[ProjectDPRSitePics_ProjectWork_Id],[ProjectDPRSitePics_ReportSubmitted],[ProjectDPRSitePics_ReportSubmittedBy_PersonId],[ProjectDPRSitePics_SitePic_Path1],[ProjectDPRSitePics_SitePic_Type],[ProjectDPRSitePics_Status], [ProjectDPRSitePics_UCDetails_Id]) values ('" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_AddedBy + "',getdate(),'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmitted + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Path1 + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Type + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Status + "', '" + UCDetails_Id + "');Select @@Identity";
            if (trans == null)
            {
                try
                {
                    ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
        }

        private void Insert_tblProjectDPRSitePicsB2(tbl_ProjectDPRSitePicsB2 obj_tbl_ProjectDPR_JalNigamSitePics, int UCDetails_Id, SqlTransaction trans, SqlConnection cn)
        {
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_JalNigamSitePics ( [ProjectDPRSitePics_AddedBy],[ProjectDPRSitePics_AddedOn],[ProjectDPRSitePics_ProjectDPR_Id],[ProjectDPRSitePics_ProjectWork_Id],[ProjectDPRSitePics_ReportSubmitted],[ProjectDPRSitePics_ReportSubmittedBy_PersonId],[ProjectDPRSitePics_SitePic_Path1],[ProjectDPRSitePics_SitePic_Type],[ProjectDPRSitePics_Status], [ProjectDPRSitePics_UCDetails_Id]) values ('" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_AddedBy + "',getdate(),'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmitted + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Path1 + "',N'" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Type + "','" + obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Status + "', '" + UCDetails_Id + "');Select @@Identity";
            if (trans == null)
            {
                try
                {
                    ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
        }

        private string Insert_tbl_FinancialTrans(tbl_FinancialTrans obj_tbl_FinancialTrans, SqlTransaction trans, SqlConnection cn)
        {
            DataSet ds = new DataSet();
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_FinancialTrans ( [FinancialTrans_AddedBy],[FinancialTrans_Jurisdiction_Id],[FinancialTrans_Status],[FinancialTrans_TransAmount],[FinancialTrans_TransType],[FinancialTrans_SchemeId], [FinancialTrans_AddedOn], [FinancialTrans_Date], [FinancialTrans_EntryType], [FinancialTrans_Comments], [FinancialTrans_FinancialYear_Id], [FinancialTrans_ProjectDPR_Id], [FinancialTrans_WorkId], [FinancialTrans_FilePath1], [FinancialTrans_GO_Date], [FinancialTrans_GO_Number]) values ('" + obj_tbl_FinancialTrans.FinancialTrans_AddedBy + "','" + obj_tbl_FinancialTrans.FinancialTrans_Jurisdiction_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Status + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransAmount + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransType + "','" + obj_tbl_FinancialTrans.FinancialTrans_SchemeId + "', getdate(), convert(date, '" + obj_tbl_FinancialTrans.FinancialTrans_Date + "', 103), '" + obj_tbl_FinancialTrans.FinancialTrans_EntryType + "', '" + obj_tbl_FinancialTrans.FinancialTrans_Comments + "', '" + obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id + "', '" + obj_tbl_FinancialTrans.FinancialTrans_ProjectDPR_Id + "', '" + obj_tbl_FinancialTrans.FinancialTrans_WorkId + "', '" + obj_tbl_FinancialTrans.FinancialTrans_FilePath1 + "', convert(date, '" + obj_tbl_FinancialTrans.FinancialTrans_GO_Date + "', 103), N'" + obj_tbl_FinancialTrans.FinancialTrans_GO_Number + "'); Select @@Identity";
            if (trans == null)
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            else
            {
                ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
            return ds.Tables[0].Rows[0][0].ToString();
        }

        private int Insert_tbl_ProjectUC(tbl_ProjectUC obj_tbl_ProjectUC, SqlTransaction trans, SqlConnection cn)
        {
            DataSet ds = new DataSet();
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectUC ([ProjectUC_Achivment],[ProjectUC_AddedBy],[ProjectUC_AddedOn],[ProjectUC_BudgetUtilized],[ProjectUC_Comments],[ProjectUC_ProjectDPR_Id],[ProjectUC_ProjectWork_Id],[ProjectUC_Status],[ProjectUC_SubmitionDate], [ProjectUC_PhysicalProgress], [ProjectUC_Latitude], [ProjectUC_Longitude], [ProjectUC_FinancialYear_Id], [ProjectUC_Total_Allocated], [ProjectUC_Centage]) values ('" + obj_tbl_ProjectUC.ProjectUC_Achivment + "','" + obj_tbl_ProjectUC.ProjectUC_AddedBy + "', getdate(),'" + obj_tbl_ProjectUC.ProjectUC_BudgetUtilized + "',N'" + obj_tbl_ProjectUC.ProjectUC_Comments + "','" + obj_tbl_ProjectUC.ProjectUC_ProjectDPR_Id + "','" + obj_tbl_ProjectUC.ProjectUC_ProjectWork_Id + "','" + obj_tbl_ProjectUC.ProjectUC_Status + "', convert(date, '" + obj_tbl_ProjectUC.ProjectUC_SubmitionDate + "', 103), '" + obj_tbl_ProjectUC.ProjectUC_PhysicalProgress + "', '" + obj_tbl_ProjectUC.ProjectUC_Latitude + "', '" + obj_tbl_ProjectUC.ProjectUC_Longitude + "', '" + obj_tbl_ProjectUC.ProjectUC_FinancialYear_Id + "', '" + obj_tbl_ProjectUC.ProjectUC_Total_Allocated + "', '" + obj_tbl_ProjectUC.ProjectUC_Centage + "'); select @@IDENTITY";
            if (trans == null)
            {
                try
                {
                    ds = ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }

        private void Insert_tbl_ProjectUC_Concent(tbl_ProjectUC_Concent objtbl_ProjectUC_Concent, SqlTransaction trans, SqlConnection cn)
        {
            string strQuery = "";
            strQuery = " set dateformat dmy;insert into tbl_ProjectUC_Concent (ProjectUC_Concent_ProjectUC_Id, ProjectUC_Concent_Questionire_Id, ProjectUC_Concent_Answer_Id, ProjectUC_Concent_AddedOn, ProjectUC_Concent_AddedBy, ProjectUC_Concent_Status, ProjectUC_Concent_Comments, ProjectUC_Concent_Answer) values ('" + objtbl_ProjectUC_Concent.ProjectUC_Concent_ProjectUC_Id + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Questionire_Id + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Answer_Id + "', getdate(), '" + objtbl_ProjectUC_Concent.ProjectUC_Concent_AddedBy + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Status + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Comments + "', N'" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Answer + "')";
            if (trans == null)
            {
                try
                {
                    ExecuteSelectQuery(strQuery);
                }
                catch
                {
                }
            }
            else
            {
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            }
        }

        public DataSet get_Dashboard_Data(int Person_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                        select 
	                        count(distinct ProjectWorkPkg_Id) Total_Running_ULB
                        from tbl_ProjectWorkPkg
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
                        where ProjectWorkPkg_Status = 1 and ProjectWork_DivisionId in (select PersonJuridiction_DivisionId from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + Person_Id + "' and PersonJuridiction_Status = 1); ";

            strQuery += Environment.NewLine;

            strQuery += @"set dateformat dmy; 
                        select 
	                        count(distinct ProjectWorkPkg_Id) Total_Running_Vendor
                        from tbl_ProjectWorkPkg
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
                        where ProjectWorkPkg_Status = 1 and ProjectWorkPkg_Vendor_Id = '" + Person_Id + "'; ";

            strQuery += Environment.NewLine;

            strQuery += @"set dateformat dmy; 
                        select 
	                        count(distinct ProjectWorkPkg_Id) Total_Running_Inspection
                        from tbl_ProjectWorkPkg
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
                        join tbl_ProjectPKGInspectionInfo on ProjectPKGInspectionInfo_ProjectPkg_Id = ProjectWorkPkg_Id and ProjectPKGInspectionInfo_ProjectWork_Id = ProjectWork_Id
                        where ProjectWorkPkg_Status = 1 and ProjectPKGInspectionInfo_Status = 1 and ProjectPKGInspectionInfo_InspectionPersonId = '" + Person_Id + "'; ";

            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch (Exception)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_ComiteeMember(int ProjectDPR_Id, int ProjectWork_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                        select 
	                        Person_Id,
	                        UserType_Desc_E, 
	                        Person_Name, 
	                        Person_Mobile1, 
	                        Person_Mobile2,
	                        Designation_DesignationName
                        from tbl_ProjectDPR_JalNigamInspectionInfo
                        join tbl_PersonDetail on Person_Id = ProjectDPRInspectionInfo_InspectionPersonId
                        join tbl_PersonJuridiction on PersonJuridiction_PersonId = ProjectDPRInspectionInfo_InspectionPersonId
                        left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                        join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                        where ProjectDPRInspectionInfo_Status = 1 and ProjectDPRInspectionInfo_ProjectDPR_Id = '" + ProjectDPR_Id + "' and ProjectDPRInspectionInfo_ProjectWork_Id = '" + ProjectWork_Id + "' ";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_FinancialRelease_Breakup(int ProjectDPR_Id, int ProjectWork_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                        select 
                            FinancialTrans_Id,
                            FinancialTrans_Date = convert(char(10), FinancialTrans_Date, 103),
	                        FinancialTrans_EntryType,
	                        FinancialTrans_Comments,
	                        FinancialTrans_FilePath1,
	                        FinancialTrans_GO_Date = convert(char(10), FinancialTrans_GO_Date, 103),
	                        FinancialTrans_GO_Number,
	                        TransAmount_C = case when FinancialTrans_TransType = 'C' then convert(decimal(18,2), isnull(FinancialTrans_TransAmount, 0) / 100000) else 0 end, 
	                        TransAmount_D = case when FinancialTrans_TransType = 'D' then convert(decimal(18,2), isnull(FinancialTrans_TransAmount, 0) / 100000) else 0 end
                        from tbl_FinancialTrans
                        where FinancialTrans_Status = 1 and FinancialTrans_ProjectDPR_Id = '" + ProjectDPR_Id + "' ";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_Site_Pics(int ProjectDPR_Id, int ProjectWork_Id, int ProjectUC_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                        select 
	                        Person_Name,
	                        UserType_Desc_E,
	                        ProjectDPRSitePics_Id,
	                        ProjectDPRSitePics_ProjectDPR_Id,
	                        ProjectDPRSitePics_ProjectWork_Id,
	                        ProjectDPRSitePics_ReportSubmittedBy_PersonId,
	                        ProjectDPRSitePics_ReportSubmitted = isnull(UserType_Desc_E, '') + ' ' + isnull(Person_Name, '') + ', Mob: ' + isnull(Person_Mobile1, ''),
	                        ProjectDPRSitePics_SitePic_Path1,
	                        ProjectDPRSitePics_SitePic_Type = case when ProjectDPRSitePics_SitePic_Type = 'B' then 'Before' else 'After' end,
	                        ProjectDPRSitePics_AddedOn = convert(char(10), ProjectDPRSitePics_AddedOn, 103)
                        from tbl_ProjectDPR_JalNigamSitePics
                        join tbl_PersonDetail on Person_Id = ProjectDPRSitePics_ReportSubmittedBy_PersonId
                        join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                        left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                        where ProjectDPRSitePics_Status = 1  ";
            if (ProjectDPR_Id > 0)
            {
                strQuery += " and ProjectDPRSitePics_ProjectDPR_Id = '" + ProjectDPR_Id + "'";
            }
            if (ProjectUC_Id > 0)
            {
                strQuery += " and ProjectDPRSitePics_UCDetails_Id = '" + ProjectUC_Id + "'";
            }
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_DPR_UC_Details(int ProjectDPR_Id, int ProjectWork_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                        select 
	                        ProjectUC_Id,
	                        ProjectUC_ProjectDPR_Id,
	                        ProjectUC_ProjectWork_Id,
	                        ProjectUC_Achivment,
	                        ProjectUC_SubmitionDate = convert(char(10), ProjectUC_SubmitionDate, 103),
	                        ProjectUC_BudgetUtilized,
	                        ProjectUC_Comments,
	                        ProjectUC_PhysicalProgress,
	                        ProjectUC_Total_Allocated, 
	                        ProjectUC_Latitude,
	                        ProjectUC_Longitude, 
	                        Person_Name, 
	                        Person_Mobile1, 
	                        Level_Name = UserType_Desc_E, 
	                        ProjectUC_AddedBy
                        from tbl_ProjectUC 
                        left join tbl_PersonDetail on Person_Id = ProjectUC_AddedBy
                        join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                        left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                        where ProjectUC_Status = 1 and ProjectUC_ProjectDPR_Id = '" + ProjectDPR_Id + "' ";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_DPRQuestionnaire(int Project_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        DPRQuestionnaire_Id,
                        DPRQuestionnaire_ProjectId,
                        DPRQuestionnaire_Name,
                        DPRQuestionnaire_AddedBy,
                        DPRQuestionnaire_AddedOn,
                        DPRQuestionnaire_Status
                    from tbl_DPRQuestionnaire 
                    where DPRQuestionnaire_Status = 1 ";
            if (Project_Id > 0)
            {
                strQuery += " and DPRQuestionnaire_ProjectId = '" + Project_Id + "'";
            }
            strQuery += " order by DPRQuestionnaire_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_Scheme_Wise_Report(tbl_Person obj_tbl_Person)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                        select                     
	                        Project_Id,
                            Project_Name,
                            Project_Budget = convert(decimal(18, 3), Project_Scheme_Budget_Budget),  
                            tDPR.Total_ULB, 
                            Total_Work = isnull(tDPR.Total_Work, 0), 
                            ProjectDPR_BudgetAllocated = convert(decimal(18, 3), isnull(tDPR.ProjectDPR_BudgetAllocated, 0)),
                            Fund_Released = convert(decimal(18, 3), isnull(tStatement.TransAmount_C, 0) / 100000), 
                            Expenditure = convert(decimal(18, 3), isnull(tStatement.TransAmount_D, 0) / 100000), 
                            Balance = convert(decimal(18, 3), ((isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0)) / 100000)) 
                        from tbl_Project
                        left join tbl_Project_Scheme_Budget on Project_Scheme_Budget_Project_Id=Project_Id and  Project_Scheme_Budget_FinancialYear_Id = FinancialYear_IdCond
                        left join 
                        (
	                        select 
		                        FinancialTrans_SchemeId ProjectDPR_Project_Id, 
		                        count(distinct FinancialTrans_WorkId) Total_Work,
		                        count(distinct FinancialTrans_Jurisdiction_Id) Total_ULB, 
		                        sum(isnull(ProjectDPR_BudgetAllocated, 0)) / 100000 ProjectDPR_BudgetAllocated 
	                        from tbl_ProjectWork 
                            join tbl_ProjectDPR_JalNigam on ProjectDPR_Work_Id = ProjectWork_Id and ProjectDPR_Status = 1 
                            
                            full outer join (
		                        select 
			                        FinancialTrans_FinancialYear_Id,
			                        FinancialTrans_ProjectDPR_Id,
			                        FinancialTrans_SchemeId,
			                        FinancialTrans_WorkId, 
                                    FinancialTrans_Jurisdiction_Id, 
                                    ULB_District_Id
		                        from tbl_FinancialTrans 
                                join tbl_ULB on ULB_Id = FinancialTrans_Jurisdiction_Id 
                                where FinancialTrans_Status = 1 
		                        group by FinancialTrans_FinancialYear_Id, FinancialTrans_ProjectDPR_Id, FinancialTrans_WorkId, FinancialTrans_SchemeId, ULB_District_Id, FinancialTrans_Jurisdiction_Id 
                            ) tFYData on FinancialTrans_ProjectDPR_Id = ProjectDPR_Id and FinancialTrans_WorkId = ProjectWork_Id 
	                        where ProjectWork_Status = 1 and ProjectDPR_Status = 1 ULB_IdCond
	                        group by FinancialTrans_SchemeId
                        ) tDPR on tDPR.ProjectDPR_Project_Id = Project_Id 
                        left join 
                        (
	                        select 
		                        FinancialTrans_SchemeId, 
		                        TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then isnull(FinancialTrans_TransAmount, 0) else 0 end), 
		                        TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then isnull(FinancialTrans_TransAmount, 0) else 0 end) 
	                        from tbl_FinancialTrans 
                            join tbl_ProjectDPR on ProjectDPR_ProjectWork_Id = FinancialTrans_Work_Id and ProjectDPR_ProjectWorkPkg_Id = FinancialTrans_Package_Id and ProjectDPR_Status = 1
                            join tbl_ProjectWork on ProjectDPR_ProjectWork_Id = ProjectWork_Id and FinancialTrans_Work_Id = ProjectWork_Id and ProjectWork_Status = 1
	                        where FinancialTrans_Status= 1 ULB_IdCond 
	                        group by FinancialTrans_SchemeId
                        ) tStatement on tStatement.FinancialTrans_SchemeId = Project_Id 
                        where isnull(tDPR.Total_Work, 0) > 0 order by Project_Name";
            if (obj_tbl_Person.Person_Id > 0)
            {
                //strQuery = strQuery.Replace("ULB_IdCond", obj_tbl_Person.Person_Id.ToString());
                strQuery = strQuery.Replace("ULB_IdCond", " and (ProjectDPR_NP_JurisdictionId in (select PersonJuridiction_ULB_Id from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + obj_tbl_Person.Person_Id.ToString() + "')  or ProjectDPR_NP_JurisdictionId in (select PersonAdditionalULB_ULB_Id from tbl_PersonAdditionalULB where PersonAdditionalULB_Person_Id = '" + obj_tbl_Person.Person_Id.ToString() + "'))");
            }
            else
            {
                strQuery = strQuery.Replace("ULB_IdCond", "");
            }
            strQuery = strQuery.Replace("FinancialYear_IdCond", obj_tbl_Person.FinancialYear_Id.ToString());
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_TicketCategory()
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @"set dateformat dmy; 
                    select 
                        TicketCategory_Id, 
                        TicketCategory_Name, 
                        TicketCategory_AddedOn, 
                        TicketCategory_AddedBy, 
                        TicketCategory_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = TicketCategory_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = TicketCategory_ModifiedOn 
                      from tbl_TicketCategory
                      left join tbl_PersonDetail on Person_Id = TicketCategory_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = TicketCategory_ModifiedBy
                      where TicketCategory_Status = 1 order by TicketCategory_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet insert_tbl_DeviceInfo(tbl_DeviceInfo obj_tbl_DeviceInfo)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = " set dateformat dmy; insert into tbl_DeviceInfo ([BRAND],[CPU_ABI],[CPU_ABI2],[Created_Date],[DEVICE],[DEVICE_ID],[DISPLAY],[HARDWARE],[HOST],[ID],[Is_Active],[MANUFACTURER],[MODEL],[PRODUCT],[RELEASE],[SERIAL],[UNKNOWN],[USER], [Person_Id], [Mobile_No], [Latitude], [Longitude], [App_Version]) values ('" + obj_tbl_DeviceInfo.BRAND + "','" + obj_tbl_DeviceInfo.CPU_ABI + "','" + obj_tbl_DeviceInfo.CPU_ABI2 + "',getdate(),'" + obj_tbl_DeviceInfo.DEVICE + "','" + obj_tbl_DeviceInfo.DEVICE_ID + "','" + obj_tbl_DeviceInfo.DISPLAY + "','" + obj_tbl_DeviceInfo.HARDWARE + "','" + obj_tbl_DeviceInfo.HOST + "','" + obj_tbl_DeviceInfo.ID + "',1,'" + obj_tbl_DeviceInfo.MANUFACTURER + "','" + obj_tbl_DeviceInfo.MODEL + "','" + obj_tbl_DeviceInfo.PRODUCT + "','" + obj_tbl_DeviceInfo.RELEASE + "','" + obj_tbl_DeviceInfo.SERIAL + "','" + obj_tbl_DeviceInfo.UNKNOWN + "','" + obj_tbl_DeviceInfo.USER + "', '" + obj_tbl_DeviceInfo.Person_Id + "', '" + obj_tbl_DeviceInfo.Mobile_No + "', '" + obj_tbl_DeviceInfo.Latitude + "', '" + obj_tbl_DeviceInfo.Longitude + "', '" + obj_tbl_DeviceInfo.App_Version + "');Select @@Identity";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_Zone()
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        Zone_Id, 
                        Zone_Name, 
                        Zone_AddedOn, 
                        Zone_AddedBy, 
                        Zone_Status
                      from tbl_Zone
                      where Zone_Status = 1 order by Zone_Name";

            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_Circle(int Zone_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        Circle_Id, 
                        Circle_ZoneId, 
                        Zone_Name, 
                        Circle_Name, 
                        Circle_AddedOn, 
                        Circle_AddedBy, 
                        Circle_Status 
                    from tbl_Circle
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    where Circle_Status = 1";
            if (Zone_Id != 0)
            {
                strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
            }
            strQuery += " order by Zone_Name, Circle_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_Division(int Circle_Id)
        {
            string strQuery = "";
            DataSet ds = new DataSet();
            strQuery = @" set dateformat dmy; 
                    select 
                        Division_Id, 
                        Division_CircleId, 
                        Circle_Name, 
                        Division_Name, 
                        Division_AddedOn, 
                        Division_AddedBy, 
                        Division_Status 
                    from tbl_Division
                    join tbl_Circle on Circle_Id = Division_CircleId
                    where Division_Status = 1";
            if (Circle_Id != 0)
            {
                strQuery += " and Division_CircleId = '" + Circle_Id + "'";
            }
            strQuery += " order by Circle_Name, Division_Name";
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public DataSet get_tbl_Profile(int Person_Id)
        {
            DataSet dt = new DataSet();
            string strQuery = @"select 
	                                Person_Id,
	                                Person_Name,
	                                Person_FName,
	                                Person_Mobile1,
	                                Person_Mobile2,
	                                Person_TelePhone,
	                                Person_EmailId,
	                                Person_AddressLine1,
	                                Person_AddressLine2,
	                                Person_ProfilePIC 
                                from tbl_PersonDetail 
                                where Person_Id = " + Person_Id.ToString();
            dt = ExecuteSelectQuery(strQuery);

            return dt;
        }

        public bool Update_tbl_Profile(tbl_Profile obj_tbl_Profile)
        {
            string ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    string baseURL = ConfigurationManager.AppSettings.Get("BaseURL");
                    if (saveImageData(obj_tbl_Profile.Profile_Pic_File, obj_tbl_Profile.Profile_Base_64, baseURL + "\\Downloads\\" + obj_tbl_Profile.Profile_Pic_File))
                    {
                        obj_tbl_Profile.Profile_Pic_File = "\\Downloads\\" + obj_tbl_Profile.Profile_Pic_File;
                    }
                    else
                    {
                        obj_tbl_Profile.Profile_Pic_File = "";
                    }
                    string sql = "set dateformat dmy; update tbl_PersonDetail set Person_Name = '" + obj_tbl_Profile.Profile_Name + "', Person_Mobile1 = '" + obj_tbl_Profile.Profile_Mobile + "', Person_EmailId = '" + obj_tbl_Profile.Profile_Email + "', Person_AddressLine1 = '" + obj_tbl_Profile.Profile_Address + "', Person_ProfilePIC = '" + obj_tbl_Profile.Profile_Pic_File + "', Person_ModifiedBy = '" + obj_tbl_Profile.Person_Id + "', Person_ModifiedOn = getdate() where Person_Id = '" + obj_tbl_Profile.Person_Id + "'";
                    ExecuteSelectQuerywithTransaction(cn, sql, trans);

                    trans.Commit();
                    cn.Close();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    cn.Close();
                    return false;
                }
            }
        }

        public DataSet get_ProjectPkg_PhysicalProgress(int ProjectWork_Id, int Package_Id)
        {
            DataSet dt = new DataSet();
            string strQuery = @"set dateformat dmy; 
                                select 
	                                PhysicalProgressComponent_Id,
                                    PhysicalProgressComponent_Component,
                                    Unit_Name,
                                    PhysicalProgress_Total = isnull(tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_PhysicalProgress,0)
                                from tbl_ProjectPkg_PhysicalProgress
                                left join tbl_PhysicalProgressComponent on ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id=PhysicalProgressComponent_Id  
                                inner join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id

                                left join 
                                (
	                                select 
		                                ROW_NUMBER() Over(Partition by ProjectUC_PhysicalProgress_ProjectWork_Id,ProjectUC_PhysicalProgress_ProjectPkg_Id,ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id order by ProjectUC_PhysicalProgress_Id desc) as rr,* 
	                                from tbl_ProjectUC_PhysicalProgress 
	                                where ProjectUC_PhysicalProgress_Status=1
                                ) as tbl_ProjectUC_PhysicalProgressTotal on tbl_ProjectUC_PhysicalProgressTotal.rr=1 and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_ProjectWork_Id = ProjectPkg_PhysicalProgress_PrjectWork_Id and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_ProjectPkg_Id = ProjectPkg_PhysicalProgress_ProjectPkg_Id and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id = ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id
                                where ProjectPkg_PhysicalProgress_Status = 1 and ProjectPkg_PhysicalProgress_PrjectWork_Id = '" + ProjectWork_Id + "' and ProjectPkg_PhysicalProgress_ProjectPkg_Id='" + Package_Id + "' ";
            dt = ExecuteSelectQuery(strQuery);

            return dt;
        }

        public DataSet get_ProjectPkg_Deliverables(int ProjectWork_Id, int Package_Id)
        {
            DataSet dt = new DataSet();
            string strQuery = @"set dateformat dmy; 
                                select 
	                                Deliverables_Id,
                                    Deliverables_Deliverables,
                                    Unit_Name,
                                    Deliverables_Total = isnull(tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_Deliverables,0)
                                from tbl_ProjectPkg_Deliverables
                                left join tbl_Deliverables on ProjectPkg_Deliverables_Deliverables_Id=Deliverables_Id  
                                inner join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                                left join 
                                (
	                                select 
		                                ROW_NUMBER() Over(Partition by ProjectUC_Deliverables_ProjectWork_Id,ProjectUC_Deliverables_ProjectPkg_Id,ProjectUC_Deliverables_Deliverables_Id order by ProjectUC_Deliverables_Id desc) as rr,* 
	                                from tbl_ProjectUC_Deliverables 
	                                where ProjectUC_Deliverables_Status=1
                                ) as tbl_ProjectUC_DeliverablesTotal on tbl_ProjectUC_DeliverablesTotal.rr = 1 and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_ProjectWork_Id = ProjectPkg_Deliverables_ProjectWork_Id and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_ProjectPkg_Id = ProjectPkg_Deliverables_ProjectPkg_Id and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_Deliverables_Id = ProjectPkg_Deliverables_Deliverables_Id
                                where ProjectPkg_Deliverables_Status=1 and ProjectPkg_Deliverables_ProjectWork_Id='" + ProjectWork_Id + "' and ProjectPkg_Deliverables_ProjectPkg_Id='" + Package_Id + "' ";
            dt = ExecuteSelectQuery(strQuery);

            return dt;
        }
    }
}
