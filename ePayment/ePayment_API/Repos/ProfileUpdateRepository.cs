using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class ProfileUpdateRepository : RepositoryAsyn
    {
        public ProfileUpdateRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        
        public async Task<bool> update_Profile(tbl_Profile obj_tbl_Profile)
        {
            return updateProfile(obj_tbl_Profile);
        }

        private bool updateProfile(tbl_Profile obj_tbl_Profile)
        {
            try
            {
                return new DataLayer().Update_tbl_Profile(obj_tbl_Profile);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
