using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class SchemeWiseReportRepository : RepositoryAsyn
    {
        public SchemeWiseReportRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_Scheme_Wise_Report>> get_Scheme_Wise_Report(tbl_Person obj_tbl_Person)
        {
            List<tbl_Scheme_Wise_Report> obj_tbl_Scheme_Wise_Report_Li = get_tbl_Scheme_Wise_Report(obj_tbl_Person);
            return obj_tbl_Scheme_Wise_Report_Li;
        }
        private List<tbl_Scheme_Wise_Report> get_tbl_Scheme_Wise_Report(tbl_Person obj_tbl_Person)
        {
            List<tbl_Scheme_Wise_Report> obj_tbl_Scheme_Wise_Report_Li = new List<tbl_Scheme_Wise_Report>();
            try
            {
                DataSet ds = new DataLayer().get_Scheme_Wise_Report(obj_tbl_Person);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_Scheme_Wise_Report obj_tbl_Scheme_Wise_Report = new tbl_Scheme_Wise_Report();
                        obj_tbl_Scheme_Wise_Report.Project_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Project_Id"].ToString());
                        obj_tbl_Scheme_Wise_Report.Project_Name = ds.Tables[0].Rows[i]["Project_Name"].ToString();
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Total_ULB = Convert.ToInt32(ds.Tables[0].Rows[i]["Total_ULB"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Total_Work = Convert.ToInt32(ds.Tables[0].Rows[i]["Total_Work"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Balance = Convert.ToDecimal(ds.Tables[0].Rows[i]["Balance"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.BudgetAllocated = Convert.ToDecimal(ds.Tables[0].Rows[i]["Fund_Released"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Expenditure = Convert.ToDecimal(ds.Tables[0].Rows[i]["Expenditure"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Financial_Progress = Convert.ToDecimal(ds.Tables[0].Rows[i]["Financial_Progress"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Fund_Released = Convert.ToDecimal(ds.Tables[0].Rows[i]["Fund_Released"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Physical_Progress = Convert.ToDecimal(ds.Tables[0].Rows[i]["Physical_Progress"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            obj_tbl_Scheme_Wise_Report.Project_Budget = Convert.ToDecimal(ds.Tables[0].Rows[i]["Project_Budget"].ToString());
                        }
                        catch
                        { }
                        obj_tbl_Scheme_Wise_Report_Li.Add(obj_tbl_Scheme_Wise_Report);
                    }
                }
                else
                {
                    obj_tbl_Scheme_Wise_Report_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_Scheme_Wise_Report_Li = null;
            }
            return obj_tbl_Scheme_Wise_Report_Li;
        }
    }
}
