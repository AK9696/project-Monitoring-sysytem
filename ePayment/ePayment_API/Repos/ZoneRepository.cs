using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class ZoneRepository : RepositoryAsyn
    {
        public ZoneRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_Zone>> get_Zone()
        {
            List<tbl_Zone> obj_tbl_Zone_Li = get_tbl_Zone();
            return obj_tbl_Zone_Li;
        }
        private List<tbl_Zone> get_tbl_Zone()
        {
            List<tbl_Zone> obj_tbl_Zone_Li = new List<tbl_Zone>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_Zone();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_Zone obj_tbl_Zone = new tbl_Zone();
                        obj_tbl_Zone.Zone_Name = ds.Tables[0].Rows[i]["Zone_Name"].ToString();
                        obj_tbl_Zone.Zone_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Zone_Id"].ToString());
                        obj_tbl_Zone.Zone_Status = 1;
                        
                        obj_tbl_Zone_Li.Add(obj_tbl_Zone);
                    }                    
                }
                else
                {
                    obj_tbl_Zone_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_Zone_Li = null;
            }
            return obj_tbl_Zone_Li;
        }
    }
}
