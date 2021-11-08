using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class ComiteeMemberRepository : RepositoryAsyn
    {
        public ComiteeMemberRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_PersonDetail>> get_ComiteeMember(tbl_Person obj_tbl_Person)
        {
            List<tbl_PersonDetail> obj_tbl_PersonDetail_Li = get_tbl_PersonDetail(obj_tbl_Person);
            return obj_tbl_PersonDetail_Li;
        }
        private List<tbl_PersonDetail> get_tbl_PersonDetail(tbl_Person obj_tbl_Person)
        {
            List<tbl_PersonDetail> obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail>();
            try
            {
                DataSet ds = new DataLayer().get_ComiteeMember(obj_tbl_Person.ProjectDPR_Id, obj_tbl_Person.ProjectWork_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_PersonDetail obj_tbl_PersonDetail = new tbl_PersonDetail();
                        try
                        {
                            obj_tbl_PersonDetail.Person_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Person_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_PersonDetail.Person_Id = 1;
                        }
                        obj_tbl_PersonDetail.UserType_Desc = ds.Tables[0].Rows[i]["UserType_Desc_E"].ToString();
                        obj_tbl_PersonDetail.Person_Name = ds.Tables[0].Rows[i]["Person_Name"].ToString();
                        obj_tbl_PersonDetail.Designation = ds.Tables[0].Rows[i]["Designation_DesignationName"].ToString();
                        obj_tbl_PersonDetail.Person_Mobile1 = ds.Tables[0].Rows[i]["Person_Mobile1"].ToString();
                        obj_tbl_PersonDetail.Person_Mobile2 = ds.Tables[0].Rows[i]["Person_Mobile2"].ToString();
                        obj_tbl_PersonDetail_Li.Add(obj_tbl_PersonDetail);
                    }
                }
                else
                {
                    obj_tbl_PersonDetail_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_PersonDetail_Li = null;
            }
            return obj_tbl_PersonDetail_Li;
        }
    }
}
