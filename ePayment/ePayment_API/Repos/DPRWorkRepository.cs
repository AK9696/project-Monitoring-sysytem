using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class DPRWorkRepository : RepositoryAsyn
    {
        public DPRWorkRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectDPR>> get_ProjectDPR(tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR> obj_tbl_ProjectDPR_Li = get_tbl_ProjectDPR(obj_Person);
            return obj_tbl_ProjectDPR_Li;
        }
        private List<tbl_ProjectDPR> get_tbl_ProjectDPR(tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR> obj_tbl_ProjectDPR_Li = new List<tbl_ProjectDPR>();
            try
            {
                string _Mode = "";
                if (obj_Person.Role_ULB > 0)
                {
                    _Mode = "ULB";
                }
                else if (obj_Person.Role_Vendor > 0)
                {
                    _Mode = "Vendor";
                }
                else if (obj_Person.Role_Inspection > 0)
                {
                    _Mode = "Inspection";
                }
                else
                {
                    _Mode = "ULB";
                }
                DataSet ds = new DataLayer().get_tbl_ProjectDPR_Upload(obj_Person.Person_Id, _Mode, 0, obj_Person.Project_Id, obj_Person.Project_Status);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectDPR obj_tbl_ProjectDPR = new tbl_ProjectDPR();

                        obj_tbl_ProjectDPR.District_Name = ds.Tables[0].Rows[i]["District_Name"].ToString();
                        obj_tbl_ProjectDPR.PhysicalProgressTrackingType = ds.Tables[0].Rows[i]["ProjectDPR_PhysicalProgressTrackingType"].ToString();

                        try
                        {
                            obj_tbl_ProjectDPR.ProjectWork_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectWork_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPRTenderInfo_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPRTenderInfo_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.Fund_Allocated_Total = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Release"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.Fund_Expenditure_Total = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Expenditure"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.Fund_Remaining = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Available"].ToString());
                        }
                        catch
                        {

                        }
                        
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPR_BudgetAllocated = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectDPR_BudgetAllocated"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectWork_Budget = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectWork_Budget"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPR_District_Jurisdiction_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPR_District_Jurisdiction_Id"].ToString());
                        }
                        catch
                        {

                        }
                        obj_tbl_ProjectDPR.ProjectDPR_FilePath1 = ds.Tables[0].Rows[i]["ProjectDPR_FilePath1"].ToString();
                        obj_tbl_ProjectDPR.ProjectDPR_FilePath1 = ds.Tables[0].Rows[i]["ProjectDPR_FilePath1"].ToString();
                        obj_tbl_ProjectDPR.ProjectWork_GO_Path = ds.Tables[0].Rows[i]["ProjectWork_GO_Path"].ToString();
                        obj_tbl_ProjectDPR.ProjectWork_GO_Date = ds.Tables[0].Rows[i]["ProjectWork_GO_Date"].ToString();
                        obj_tbl_ProjectDPR.ProjectWork_GO_No = ds.Tables[0].Rows[i]["ProjectWork_GO_No"].ToString();
                        obj_tbl_ProjectDPR.Vendor_Name = ds.Tables[0].Rows[i]["Vendor_Name"].ToString();
                        obj_tbl_ProjectDPR.Vendor_Mobile = ds.Tables[0].Rows[i]["Vendor_Mobile"].ToString();
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPR_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPR_Id"].ToString());
                        }
                        catch
                        {

                        }
                        
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPR_NP_JurisdictionId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPR_NP_JurisdictionId"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR.ProjectDPR_Project_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPR_Project_Id"].ToString());
                        }
                        catch
                        {

                        }
                        
                        obj_tbl_ProjectDPR.ProjectDPR_SubmitionDate = ds.Tables[0].Rows[i]["ProjectDPR_SubmitionDate"].ToString();
                        obj_tbl_ProjectDPR.Physical_Progress = ds.Tables[0].Rows[i]["Physical_Progress"].ToString();
                        obj_tbl_ProjectDPR.Financial_Progress = ds.Tables[0].Rows[i]["Financial_Progress"].ToString();
                        obj_tbl_ProjectDPR.Financial_Progress_Per = ds.Tables[0].Rows[i]["Financial_Progress_Per"].ToString();

                        obj_tbl_ProjectDPR.Project_Name = ds.Tables[0].Rows[i]["ProjectWork_Name"].ToString();
                        obj_tbl_ProjectDPR.ULB_Name = ds.Tables[0].Rows[i]["ULB_Name"].ToString();
                        obj_tbl_ProjectDPR.District_Name = ds.Tables[0].Rows[i]["District_Name"].ToString();

                        obj_tbl_ProjectDPR_Li.Add(obj_tbl_ProjectDPR);
                    }
                }
                else
                {
                    obj_tbl_ProjectDPR_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectDPR_Li = null;
            }
            return obj_tbl_ProjectDPR_Li;
        }
    }
}
