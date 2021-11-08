using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class PhysicalProgressRepository : RepositoryAsyn
    {
        public PhysicalProgressRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectPkg_PhysicalProgress>> get_ProjectPkg_PhysicalProgress(tbl_Person obj_Person)
        {
            List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress_Li = get_tbl_ProjectPkg_PhysicalProgress(obj_Person);
            return obj_tbl_ProjectPkg_PhysicalProgress_Li;
        }
        private List<tbl_ProjectPkg_PhysicalProgress> get_tbl_ProjectPkg_PhysicalProgress(tbl_Person obj_Person)
        {
            List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress_Li = new List<tbl_ProjectPkg_PhysicalProgress>();
            try
            {
                DataSet ds = new DataLayer().get_ProjectPkg_PhysicalProgress(obj_Person.ProjectWork_Id, obj_Person.ProjectDPR_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectPkg_PhysicalProgress obj_tbl_ProjectPkg_PhysicalProgress = new tbl_ProjectPkg_PhysicalProgress();

                        obj_tbl_ProjectPkg_PhysicalProgress.PhysicalProgressComponent_Component = ds.Tables[0].Rows[i]["PhysicalProgressComponent_Component"].ToString();
                        obj_tbl_ProjectPkg_PhysicalProgress.Unit_Name = ds.Tables[0].Rows[i]["Unit_Name"].ToString();
                        
                        try
                        {
                            obj_tbl_ProjectPkg_PhysicalProgress.PhysicalProgressComponent_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PhysicalProgressComponent_Id"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            obj_tbl_ProjectPkg_PhysicalProgress.PhysicalProgress_Total = Convert.ToInt32(ds.Tables[0].Rows[i]["PhysicalProgress_Total"].ToString());
                        }
                        catch
                        {

                        }                        
                        obj_tbl_ProjectPkg_PhysicalProgress_Li.Add(obj_tbl_ProjectPkg_PhysicalProgress);
                    }
                }
                else
                {
                    obj_tbl_ProjectPkg_PhysicalProgress_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectPkg_PhysicalProgress_Li = null;
            }
            return obj_tbl_ProjectPkg_PhysicalProgress_Li;
        }
    }
}
