using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class FinancialRelease_BreakupRepository : RepositoryAsyn
    {
        public FinancialRelease_BreakupRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_FinancialTrans>> get_FinancialRelease_Breakup(tbl_Person obj_tbl_Person)
        {
            List<tbl_FinancialTrans> obj_tbl_FinancialTrans_Li = get_tbl_FinancialTrans(obj_tbl_Person);
            return obj_tbl_FinancialTrans_Li;
        }
        private List<tbl_FinancialTrans> get_tbl_FinancialTrans(tbl_Person obj_tbl_Person)
        {
            List<tbl_FinancialTrans> obj_tbl_FinancialTrans_Li = new List<tbl_FinancialTrans>();
            try
            {
                DataSet ds = new DataLayer().get_FinancialRelease_Breakup(obj_tbl_Person.ProjectDPR_Id, obj_tbl_Person.ProjectWork_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_FinancialTrans obj_tbl_FinancialTrans = new tbl_FinancialTrans();
                        try
                        {
                            obj_tbl_FinancialTrans.FinancialTrans_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialTrans_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_FinancialTrans.FinancialTrans_Id = 0;
                        }
                        obj_tbl_FinancialTrans.FinancialTrans_Date = ds.Tables[0].Rows[i]["FinancialTrans_Date"].ToString();
                        obj_tbl_FinancialTrans.FinancialTrans_EntryType = ds.Tables[0].Rows[i]["FinancialTrans_EntryType"].ToString();
                        obj_tbl_FinancialTrans.FinancialTrans_Comments = ds.Tables[0].Rows[i]["FinancialTrans_Comments"].ToString();
                        obj_tbl_FinancialTrans.FinancialTrans_FilePath1 = ds.Tables[0].Rows[i]["FinancialTrans_FilePath1"].ToString();
                        obj_tbl_FinancialTrans.FinancialTrans_GO_Date = ds.Tables[0].Rows[i]["FinancialTrans_GO_Date"].ToString();
                        obj_tbl_FinancialTrans.FinancialTrans_GO_Number = ds.Tables[0].Rows[i]["FinancialTrans_GO_Number"].ToString();
                        try
                        {
                            obj_tbl_FinancialTrans.FinancialTrans_ReleaseAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransAmount_C"].ToString());
                        }
                        catch
                        {
                            obj_tbl_FinancialTrans.FinancialTrans_ReleaseAmount = 0;
                        }
                        try
                            {
                                obj_tbl_FinancialTrans.FinancialTrans_ExpenditureAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransAmount_D"].ToString());
                        }
                        catch
                        {
                            obj_tbl_FinancialTrans.FinancialTrans_ExpenditureAmount = 0;
                        }
                        obj_tbl_FinancialTrans_Li.Add(obj_tbl_FinancialTrans);
                    }
                }
                else
                {
                    obj_tbl_FinancialTrans_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_FinancialTrans_Li = null;
            }
            return obj_tbl_FinancialTrans_Li;
        }
    }
}
