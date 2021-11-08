using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class ProfileRepository : RepositoryAsyn
    {
        public ProfileRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<tbl_Profile> get_Profile(int Person_Id)
        {
            tbl_Profile obj_tbl_Profile = get_tbl_Profile(Person_Id);
            return obj_tbl_Profile;
        }
        private tbl_Profile get_tbl_Profile(int Person_Id)
        {
            tbl_Profile obj_tbl_Profile = new tbl_Profile();
            try
            {
                DataSet ds = new DataLayer().get_tbl_Profile(Person_Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    obj_tbl_Profile.Profile_Name = ds.Tables[0].Rows[0]["Person_Name"].ToString();
                    obj_tbl_Profile.Profile_Address = ds.Tables[0].Rows[0]["Person_AddressLine1"].ToString();
                    try
                    {
                        obj_tbl_Profile.Profile_Base_64 = Utility.Image_ToBase64(obj_tbl_Profile.Profile_Base_URL + ds.Tables[0].Rows[0]["Person_ProfilePIC"].ToString());
                    }
                    catch
                    {

                    }
                    obj_tbl_Profile.Profile_Email = ds.Tables[0].Rows[0]["Person_EmailId"].ToString();
                    obj_tbl_Profile.Profile_Mobile = ds.Tables[0].Rows[0]["Person_Mobile1"].ToString();
                    obj_tbl_Profile.Profile_Pic_File = ds.Tables[0].Rows[0]["Person_ProfilePIC"].ToString();
                }
            }
            catch (Exception)
            {

            }
            return obj_tbl_Profile;
        }
    }
}
