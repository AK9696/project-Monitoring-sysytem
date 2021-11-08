using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class SchemeRepository : RepositoryAsyn
    {
        public SchemeRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_Project>> get_Scheme()
        {
            List<tbl_Project> obj_tbl_Project_Li = get_tbl_Project();
            return obj_tbl_Project_Li;
        }
        private List<tbl_Project> get_tbl_Project()
        {
            List<tbl_Project> obj_tbl_Project_Li = new List<tbl_Project>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_Project();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_Project obj_tbl_Project = new tbl_Project();
                        obj_tbl_Project.Project_AllocationType = ds.Tables[0].Rows[i]["Project_AllocationType"].ToString();
                        obj_tbl_Project.Project_Budget = ds.Tables[0].Rows[i]["Project_Budget"].ToString();
                        obj_tbl_Project.Project_GO_Path = ds.Tables[0].Rows[i]["Project_GO_Path"].ToString();
                        obj_tbl_Project.Project_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Project_Id"].ToString());
                        obj_tbl_Project.Project_Name = ds.Tables[0].Rows[i]["Project_Name"].ToString();
                        obj_tbl_Project.Project_Status = 1;
                        obj_tbl_Project.Project_Time = ds.Tables[0].Rows[i]["Project_Time"].ToString();

                        obj_tbl_Project_Li.Add(obj_tbl_Project);
                    }                    
                }
                else
                {
                    obj_tbl_Project_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_Project_Li = null;
            }
            return obj_tbl_Project_Li;
        }
    }
}
