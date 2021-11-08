using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class QuestionireRepository : RepositoryAsyn
    {
        public QuestionireRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_ProjectQuestionnaire>> get_Questionire(int Project_Id)
        {
            List<tbl_ProjectQuestionnaire> obj_tbl_ProjectQuestionnaire_Li = get_tbl_ProjectQuestionnaire(Project_Id);
            return obj_tbl_ProjectQuestionnaire_Li;
        }
        private List<tbl_ProjectQuestionnaire> get_tbl_ProjectQuestionnaire(int Project_Id)
        {
            List<tbl_ProjectQuestionnaire> obj_tbl_ProjectQuestionnaire_Li = new List<tbl_ProjectQuestionnaire>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_ProjectQuestionnaire(Project_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectQuestionnaire obj_tbl_ProjectQuestionnaire = new tbl_ProjectQuestionnaire();
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectQuestionnaire_AddedBy"].ToString());
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_AddedOn = ds.Tables[0].Rows[i]["ProjectQuestionnaire_AddedOn"].ToString();
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectQuestionnaire_Id"].ToString());
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Name= ds.Tables[0].Rows[i]["ProjectQuestionnaire_Name"].ToString();
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_ProjectId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectQuestionnaire_ProjectId"].ToString());
                        obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Status = 1;

                        List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
                        obj_tbl_ProjectAnswer_Li = get_tbl_ProjectAnswer(obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Id);
                        if (obj_tbl_ProjectAnswer_Li.Count == 1)
                        {
                            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Type = "Subjective";
                        }
                        else
                        {
                            obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Type = "Objective";
                        }
                        obj_tbl_ProjectQuestionnaire.obj_tbl_ProjectAnswer_Li = obj_tbl_ProjectAnswer_Li;
                        obj_tbl_ProjectQuestionnaire_Li.Add(obj_tbl_ProjectQuestionnaire);
                    }                    
                }
                else
                {
                    obj_tbl_ProjectQuestionnaire_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectQuestionnaire_Li = null;
            }
            return obj_tbl_ProjectQuestionnaire_Li;
        }

        private List<tbl_ProjectAnswer> get_tbl_ProjectAnswer(int Questionnaire_Id)
        {
            List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = new List<tbl_ProjectAnswer>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_ProjectAnswer(Questionnaire_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_ProjectAnswer obj_tbl_ProjectAnswer = new tbl_ProjectAnswer();
                        obj_tbl_ProjectAnswer.ProjectAnswer_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_AddedBy"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_AddedOn = ds.Tables[0].Rows[i]["ProjectAnswer_AddedOn"].ToString();
                        obj_tbl_ProjectAnswer.ProjectAnswer_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_Id"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_Name = ds.Tables[0].Rows[i]["ProjectAnswer_Name"].ToString();
                        obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectAnswer_ProjectQuestionnaireId"].ToString());
                        obj_tbl_ProjectAnswer.ProjectAnswer_Status = 1;

                        obj_tbl_ProjectAnswer_Li.Add(obj_tbl_ProjectAnswer);
                    }
                }
                else
                {
                    tbl_ProjectAnswer obj_tbl_ProjectAnswer = new tbl_ProjectAnswer();
                    obj_tbl_ProjectAnswer.ProjectAnswer_AddedBy = 1;
                    obj_tbl_ProjectAnswer.ProjectAnswer_AddedOn = "";
                    obj_tbl_ProjectAnswer.ProjectAnswer_Id = 0;
                    obj_tbl_ProjectAnswer.ProjectAnswer_Name = "Subjective";
                    obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId = Questionnaire_Id;
                    obj_tbl_ProjectAnswer.ProjectAnswer_Status = 1;

                    obj_tbl_ProjectAnswer_Li.Add(obj_tbl_ProjectAnswer);
                }
            }
            catch (Exception)
            {
                obj_tbl_ProjectAnswer_Li = null;
            }
            return obj_tbl_ProjectAnswer_Li;
        }
    }
}
