using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class CircleRepository : RepositoryAsyn
    {
        public CircleRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_Circle>> get_Circle(int Zone_Id)
        {
            List<tbl_Circle> obj_tbl_Circle_Li = get_tbl_Circle(Zone_Id);
            return obj_tbl_Circle_Li;
        }
        private List<tbl_Circle> get_tbl_Circle(int Zone_Id)
        {
            List<tbl_Circle> obj_tbl_Circle_Li = new List<tbl_Circle>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_Circle(Zone_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_Circle obj_tbl_Circle = new tbl_Circle();
                        
                        try
                        {
                            obj_tbl_Circle.Circle_Status = Convert.ToInt32(ds.Tables[0].Rows[i]["Circle_Status"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Circle.Circle_Status = 1;
                        }
                        obj_tbl_Circle.Circle_Name = ds.Tables[0].Rows[i]["Circle_Name"].ToString();
                        try
                        {
                            obj_tbl_Circle.Circle_ZoneId = Convert.ToInt32(ds.Tables[0].Rows[i]["Circle_ZoneId"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Circle.Circle_ZoneId = 0;
                        }
                        try
                        {
                            obj_tbl_Circle.Circle_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Circle_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_Circle.Circle_Id = 0;
                        }
                        obj_tbl_Circle_Li.Add(obj_tbl_Circle);
                    }
                }
                else
                {
                    obj_tbl_Circle_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_Circle_Li = null;
            }
            return obj_tbl_Circle_Li;
        }
    }
}
