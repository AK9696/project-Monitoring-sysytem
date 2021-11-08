using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class DivisionRepository : RepositoryAsyn
    {
        public DivisionRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_Division>> get_Division(int Circle_Id)
        {
            List<tbl_Division> obj_tbl_Division_Li = get_tbl_Division(Circle_Id);
            return obj_tbl_Division_Li;
        }
        private List<tbl_Division> get_tbl_Division(int Circle_Id)
        {
            List<tbl_Division> obj_tbl_Division_Li = new List<tbl_Division>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_Division(Circle_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_Division obj_tbl_Division = new tbl_Division();
                        
                        try
                        {
                            obj_tbl_Division.Division_Status = Convert.ToInt32(ds.Tables[0].Rows[i]["Division_Status"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Division.Division_Status = 1;
                        }
                        obj_tbl_Division.Division_Name = ds.Tables[0].Rows[i]["Division_Name"].ToString();
                        try
                        {
                            obj_tbl_Division.Division_CircleId = Convert.ToInt32(ds.Tables[0].Rows[i]["Division_CircleId"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Division.Division_CircleId = 0;
                        }
                        try
                        {
                            obj_tbl_Division.Division_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Division_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Division.Division_Id = 0;
                        }
                        obj_tbl_Division_Li.Add(obj_tbl_Division);
                    }
                }
                else
                {
                    obj_tbl_Division_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_Division_Li = null;
            }
            return obj_tbl_Division_Li;
        }
    }
}
