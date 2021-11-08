using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class TicketCategoryRepository : RepositoryAsyn
    {
        public TicketCategoryRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<List<tbl_TicketCategory>> get_TicketCategory()
        {
            List<tbl_TicketCategory> obj_tbl_TicketCategory_Li = get_tbl_TicketCategory();
            return obj_tbl_TicketCategory_Li;
        }
        private List<tbl_TicketCategory> get_tbl_TicketCategory()
        {
            List<tbl_TicketCategory> obj_tbl_TicketCategory_Li = new List<tbl_TicketCategory>();
            try
            {
                DataSet ds = new DataLayer().get_tbl_TicketCategory();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_TicketCategory obj_tbl_TicketCategory = new tbl_TicketCategory();
                        try
                        {
                            obj_tbl_TicketCategory.TicketCategory_AddedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["TicketCategory_AddedBy"].ToString());
                        }
                        catch
                        {
                            obj_tbl_TicketCategory.TicketCategory_AddedBy = 0;
                        }
                        try
                        {
                            obj_tbl_TicketCategory.TicketCategory_Status = Convert.ToInt32(ds.Tables[0].Rows[i]["TicketCategory_Status"].ToString());
                        }
                        catch
                        {
                            obj_tbl_TicketCategory.TicketCategory_Status = 1;
                        }
                        obj_tbl_TicketCategory.TicketCategory_Name = ds.Tables[0].Rows[i]["TicketCategory_Name"].ToString();
                        try
                        {
                            obj_tbl_TicketCategory.TicketCategory_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["TicketCategory_Id"].ToString());
                        }
                        catch
                        {
                            obj_tbl_TicketCategory.TicketCategory_Id = 0;
                        }
                        
                        obj_tbl_TicketCategory.TicketCategory_AddedOn = ds.Tables[0].Rows[i]["TicketCategory_AddedOn"].ToString();
                        obj_tbl_TicketCategory_Li.Add(obj_tbl_TicketCategory);
                    }
                }
                else
                {
                    obj_tbl_TicketCategory_Li = null;
                }
            }
            catch (Exception)
            {
                obj_tbl_TicketCategory_Li = null;
            }
            return obj_tbl_TicketCategory_Li;
        }
    }
}
