using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class UCDetailsRepository : RepositoryAsyn
    {
        public UCDetailsRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectUC>> get_UCDetails(tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectUC> obj_tbl_ProjectUC_Li = get_tbl_ProjectUC(obj_tbl_Person);
            return obj_tbl_ProjectUC_Li;
        }
        private List<tbl_ProjectUC> get_tbl_ProjectUC(tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectUC> obj_tbl_ProjectUC_Li = new List<tbl_ProjectUC>();
            try
            {
                DataSet ds = new DataLayer().get_DPR_UC_Details(obj_tbl_Person.ProjectDPR_Id, obj_tbl_Person.ProjectWork_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectUC obj_tbl_ProjectUC = new tbl_ProjectUC();
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectUC_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_Id = 0;
                        }
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_Achivment = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_Achivment"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_Achivment = 0;
                        }
                        obj_tbl_ProjectUC.ProjectUC_SubmitionDate = ds.Tables[0].Rows[i]["ProjectUC_SubmitionDate"].ToString();
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_BudgetUtilized = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_BudgetUtilized"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_BudgetUtilized = 0;
                        }
                        obj_tbl_ProjectUC.ProjectUC_Comments = ds.Tables[0].Rows[i]["ProjectUC_Comments"].ToString();
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_Latitude = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_Latitude"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_Latitude = null;
                        }
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_Longitude = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_Longitude"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_Longitude = null;
                        }
                        obj_tbl_ProjectUC.ProjectUC_UC_Path = "";
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_PhysicalProgress = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_PhysicalProgress"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_PhysicalProgress = 0;
                        }
                        try
                        {
                            obj_tbl_ProjectUC.ProjectUC_Total_Allocated = Convert.ToDecimal(ds.Tables[0].Rows[i]["ProjectUC_Total_Allocated"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectUC.ProjectUC_Total_Allocated = 0;
                        }
                        obj_tbl_ProjectUC.Person_Mobile = ds.Tables[0].Rows[i]["Person_Mobile1"].ToString();
                        obj_tbl_ProjectUC.Person_Name = ds.Tables[0].Rows[i]["Person_Name"].ToString();
                        obj_tbl_ProjectUC.Level_Name = ds.Tables[0].Rows[i]["Level_Name"].ToString();
                        obj_tbl_ProjectUC_Li.Add(obj_tbl_ProjectUC);
                    }
                }
                else
                {
                    obj_tbl_ProjectUC_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectUC_Li = null;
            }
            return obj_tbl_ProjectUC_Li;
        }
    }
}
