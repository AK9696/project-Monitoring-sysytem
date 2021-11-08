using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class SitePicRepository : RepositoryAsyn
    {
        public SitePicRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectDPR_JalNigamSitePics>> get_SitePic(tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectDPR_JalNigamSitePics> obj_tbl_ProjectDPR_JalNigamSitePics_Li = get_tbl_ProjectDPR_JalNigamSitePics(obj_tbl_Person);
            return obj_tbl_ProjectDPR_JalNigamSitePics_Li;
        }
        private List<tbl_ProjectDPR_JalNigamSitePics> get_tbl_ProjectDPR_JalNigamSitePics(tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectDPR_JalNigamSitePics> obj_tbl_ProjectDPR_JalNigamSitePics_Li = new List<tbl_ProjectDPR_JalNigamSitePics>();
            try
            {
                DataSet ds = new DataLayer().get_Site_Pics(obj_tbl_Person.ProjectDPR_Id, obj_tbl_Person.ProjectWork_Id, obj_tbl_Person.ProjectUC_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectDPR_JalNigamSitePics obj_tbl_ProjectDPR_JalNigamSitePics = new tbl_ProjectDPR_JalNigamSitePics();
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPRSitePics_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_Id = 1;
                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPRSitePics_ProjectDPR_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectDPR_Id = 1;
                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPRSitePics_ProjectWork_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ProjectWork_Id = 1;
                        }
                        try
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectDPRSitePics_ReportSubmittedBy_PersonId"].ToString());
                        }
                        catch
                        {
                            obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmittedBy_PersonId = 1;
                        }
                        obj_tbl_ProjectDPR_JalNigamSitePics.UserType_Desc_E = ds.Tables[0].Rows[i]["UserType_Desc_E"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics.Person_Name = ds.Tables[0].Rows[i]["Person_Name"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_ReportSubmitted = ds.Tables[0].Rows[i]["ProjectDPRSitePics_ReportSubmitted"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Path1 = ds.Tables[0].Rows[i]["ProjectDPRSitePics_SitePic_Path1"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_SitePic_Type = ds.Tables[0].Rows[i]["ProjectDPRSitePics_SitePic_Type"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics.ProjectDPRSitePics_AddedOn = ds.Tables[0].Rows[i]["ProjectDPRSitePics_AddedOn"].ToString();
                        obj_tbl_ProjectDPR_JalNigamSitePics_Li.Add(obj_tbl_ProjectDPR_JalNigamSitePics);
                    }
                }
                else
                {
                    obj_tbl_ProjectDPR_JalNigamSitePics_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectDPR_JalNigamSitePics_Li = null;
            }
            return obj_tbl_ProjectDPR_JalNigamSitePics_Li;
        }
    }
}
