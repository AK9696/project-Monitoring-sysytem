using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class ProjectWiseReportRepository : RepositoryAsyn
    {
        public ProjectWiseReportRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectDPR_JalNigam>> get_ProjectDPR(tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR_JalNigam> obj_tbl_ProjectDPR_JalNigam_Li = get_tbl_ProjectDPR_JalNigam(obj_Person);
            return obj_tbl_ProjectDPR_JalNigam_Li;
        }
        private List<tbl_ProjectDPR_JalNigam> get_tbl_ProjectDPR_JalNigam(tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR_JalNigam> obj_tbl_ProjectDPR_JalNigam_Li = new List<tbl_ProjectDPR_JalNigam>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_ProjectDPR_JalNigam_Upload(0, "", obj_Person.Person_Id, obj_Person.Project_Id,"");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectDPR_JalNigam obj_tbl_ProjectDPR_JalNigam = new tbl_ProjectDPR_JalNigam();

                        obj_tbl_ProjectDPR_JalNigam.Zone_Name = ds.Tables[0].Rows[i]["Zone_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Circle_Name = ds.Tables[0].Rows[i]["Circle_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Division_Name = ds.Tables[0].Rows[i]["Division_Name"].ToString();

                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Work_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectWorkPkg_Work_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectWorkPkg_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Fund_Allocated_Total = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Release"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Fund_Expenditure_Total = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Expenditure"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Fund_Remaining = Convert.ToDecimal(ds.Tables[0].Rows[i]["Total_Available"].ToString());
                        }
                        catch
                        {

                        }

                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_AgreementAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectWorkPkg_AgreementAmount"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.ProjectWork_Budget = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectWork_Budget"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Zone_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Zone_Id"].ToString());
                        }
                        catch
                        {

                        }
                        obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Agreement_Date = ds.Tables[0].Rows[i]["ProjectWorkPkg_Agreement_Date"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Due_Date = ds.Tables[0].Rows[i]["ProjectWorkPkg_Due_Date"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Agreement_No = ds.Tables[0].Rows[i]["ProjectWorkPkg_Agreement_No"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ReportingStaff_JEAPE_Name = ds.Tables[0].Rows[i]["List_ReportingStaff_JEAPE_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ReportingStaff_AEPE_Name = ds.Tables[0].Rows[i]["List_ReportingStaff_AEPE_Name"].ToString();

                        obj_tbl_ProjectDPR_JalNigam.ProjectWork_Name = ds.Tables[0].Rows[i]["ProjectWork_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWork_ProjectCode = ds.Tables[0].Rows[i]["ProjectWork_ProjectCode"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWork_GO_Path = ds.Tables[0].Rows[i]["ProjectWork_GO_Path"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Code = ds.Tables[0].Rows[i]["ProjectWorkPkg_Code"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.ProjectWorkPkg_Name = ds.Tables[0].Rows[i]["ProjectWorkPkg_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Vendor_Name = ds.Tables[0].Rows[i]["Vendor_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Vendor_Mobile = ds.Tables[0].Rows[i]["Vendor_Mobile"].ToString();

                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Circle_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Circle_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.Division_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Division_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigam.ProjectWork_Project_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectWork_Project_Id"].ToString());
                        }
                        catch
                        {

                        }

                        obj_tbl_ProjectDPR_JalNigam.Physical_Progress = ds.Tables[0].Rows[i]["Physical_Progress"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Financial_Progress = ds.Tables[0].Rows[i]["Financial_Progress"].ToString();
                        obj_tbl_ProjectDPR_JalNigam.Financial_Progress_Per = ds.Tables[0].Rows[i]["Financial_Progress_Per"].ToString();

                        obj_tbl_ProjectDPR_JalNigam.Project_Name = ds.Tables[0].Rows[i]["Project_Name"].ToString();

                        obj_tbl_ProjectDPR_JalNigam_Li.Add(obj_tbl_ProjectDPR_JalNigam);
                    }
                }
                else
                {
                    obj_tbl_ProjectDPR_JalNigam_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectDPR_JalNigam_Li = null;
            }
            return obj_tbl_ProjectDPR_JalNigam_Li;
        }
    }
}
