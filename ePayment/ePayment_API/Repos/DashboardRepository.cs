using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class DashboardRepository : RepositoryAsyn
    {
        public DashboardRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<tbl_Dashboard> get_Dashboard_Data(int Person_Id)
        {
            tbl_Dashboard obj_tbl_Dashboard = Dashboard_Data(Person_Id);
            return obj_tbl_Dashboard;
        }
        private tbl_Dashboard Dashboard_Data(int Person_Id)
        {
            tbl_Dashboard obj_tbl_Dashboard = new tbl_Dashboard();
            try
            {
                DataSet ds = new DataLayer().get_Dashboard_Data(Person_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        obj_tbl_Dashboard.Total_Runing_Projects = Convert.ToInt32(ds.Tables[0].Rows[0]["Total_Running_ULB"].ToString());
                    }
                    catch
                    {

                    }
                }
                else
                {
                    obj_tbl_Dashboard.Total_Runing_Projects = 0;
                }
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        obj_tbl_Dashboard.Total_Executing_Agency = Convert.ToInt32(ds.Tables[1].Rows[0]["Total_Running_Vendor"].ToString());
                    }
                    catch
                    {

                    }
                }
                else
                {
                    obj_tbl_Dashboard.Total_Executing_Agency = 0;
                }
                if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    try
                    {
                        obj_tbl_Dashboard.Total_Inspection = Convert.ToInt32(ds.Tables[2].Rows[0]["Total_Running_Inspection"].ToString());
                    }
                    catch
                    {

                    }
                }
                else
                {
                    obj_tbl_Dashboard.Total_Inspection = 0;
                }

                obj_tbl_Dashboard.Total_Budget = 1023;
                obj_tbl_Dashboard.Physical_Progress = 48;
                obj_tbl_Dashboard.Financial_Progress = 76;
                obj_tbl_Dashboard.Total_Funds_Released = 752;
            }
            catch (Exception)
            {
                obj_tbl_Dashboard = null;
            }
            return obj_tbl_Dashboard;
        }
    }
}
