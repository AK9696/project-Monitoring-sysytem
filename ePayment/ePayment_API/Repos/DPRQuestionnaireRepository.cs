using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class DPRQuestionnaireRepository : RepositoryAsyn
    {
        public DPRQuestionnaireRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_DPRQuestionnaire>> get_DPRQuestionnaire(int Project_Id)
        {
            List<tbl_DPRQuestionnaire> obj_tbl_DPRQuestionnaire_Li = get_tbl_DPRQuestionnaire(Project_Id);
            return obj_tbl_DPRQuestionnaire_Li;
        }
        private List<tbl_DPRQuestionnaire> get_tbl_DPRQuestionnaire(int Project_Id)
        {
            List<tbl_DPRQuestionnaire> obj_tbl_DPRQuestionnaire_Li = new List<tbl_DPRQuestionnaire>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_DPRQuestionnaire(Project_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_DPRQuestionnaire obj_tbl_DPRQuestionnaire = new tbl_DPRQuestionnaire();
                        try
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["DPRQuestionnaire_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Id = 0;
                        }
                        obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Name = ds.Tables[0].Rows[i]["DPRQuestionnaire_Name"].ToString();
                        obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Status = Convert.ToInt32(ds.Tables[0].Rows[i]["DPRQuestionnaire_Status"].ToString());

                        try
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_ProjectId = Convert.ToInt32(ds.Tables[0].Rows[i]["DPRQuestionnaire_ProjectId"].ToString());
                        }
                        catch
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_ProjectId = 0;
                        }
                        try
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["DPRQuestionnaire_AddedBy"].ToString());
                        }
                        catch
                        {
                            obj_tbl_DPRQuestionnaire.DPRQuestionnaire_AddedBy = 0;
                        }
                        obj_tbl_DPRQuestionnaire_Li.Add(obj_tbl_DPRQuestionnaire);
                    }
                }
                else
                {
                    obj_tbl_DPRQuestionnaire_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_DPRQuestionnaire_Li = null;
            }
            return obj_tbl_DPRQuestionnaire_Li;
        }
    }
}
