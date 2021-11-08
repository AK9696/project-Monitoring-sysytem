using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class LocalityRepository : RepositoryAsyn
    {
        public LocalityRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<M_Jurisdiction>> get_Locality(int M_Level_Id, int Parent_Jurisdiction_Id)
        {
            List<M_Jurisdiction> obj_M_Jurisdiction_Li = get_M_Jurisdiction(M_Level_Id, Parent_Jurisdiction_Id);
            return obj_M_Jurisdiction_Li;
        }
        private List<M_Jurisdiction> get_M_Jurisdiction(int M_Level_Id, int Parent_Jurisdiction_Id)
        {
            List<M_Jurisdiction> obj_M_Jurisdiction_Li = new List<M_Jurisdiction>();
            try
            {
                DataSet ds = new DataLayer().get_M_Jurisdiction(M_Level_Id, Parent_Jurisdiction_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        M_Jurisdiction obj_M_Jurisdiction = new M_Jurisdiction();
                        try
                        {
                            obj_M_Jurisdiction.Created_By = Convert.ToInt32(ds.Tables[0].Rows[i]["Created_By"].ToString());
                        }
                        catch
                        {
                            obj_M_Jurisdiction.Created_By = 0;
                        }
                        try
                        {
                            obj_M_Jurisdiction.Is_Active = Convert.ToInt32(ds.Tables[0].Rows[i]["Is_Active"].ToString());
                        }
                        catch
                        {
                            obj_M_Jurisdiction.Is_Active = 1;
                        }
                        obj_M_Jurisdiction.Jurisdiction_Code = ds.Tables[0].Rows[i]["Jurisdiction_Code"].ToString();
                        obj_M_Jurisdiction.Jurisdiction_Name = ds.Tables[0].Rows[i]["Jurisdiction_Name_Eng"].ToString();
                        try
                        {
                            obj_M_Jurisdiction.M_Jurisdiction_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["M_Jurisdiction_Id"].ToString());
                        }
                        catch
                        {
                            obj_M_Jurisdiction.M_Jurisdiction_Id = 0;
                        }
                        try
                        {
                            obj_M_Jurisdiction.M_Level_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["M_Level_Id"].ToString());
                        }
                        catch
                        {
                            obj_M_Jurisdiction.M_Level_Id = 0;
                        }
                        try
                        {
                            obj_M_Jurisdiction.Parent_Jurisdiction_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Parent_Jurisdiction_Id"].ToString());
                        }
                        catch
                        {
                            obj_M_Jurisdiction.Parent_Jurisdiction_Id = 0;
                        }
                        obj_M_Jurisdiction.Parent_Jurisdiction_Name = ds.Tables[0].Rows[i]["Parent_Jurisdiction_Name_Eng"].ToString();
                        obj_M_Jurisdiction.Created_Date = ds.Tables[0].Rows[i]["Created_Date"].ToString();
                        obj_M_Jurisdiction_Li.Add(obj_M_Jurisdiction);
                    }
                }
                else
                {
                    obj_M_Jurisdiction_Li = null;
                }
            }
            catch (Exception)
            {
                obj_M_Jurisdiction_Li = null;
            }
            return obj_M_Jurisdiction_Li;
        }
    }
}
