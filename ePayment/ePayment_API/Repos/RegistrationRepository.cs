using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class RegistrationRepository : RepositoryAsyn
    {
        public RegistrationRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<tbl_Person> User_Registration(tbl_Person obj_tbl_Person)
        {
            obj_tbl_Person = Registration(obj_tbl_Person);
            return obj_tbl_Person;
        }
        private tbl_Person Registration(tbl_Person obj_tbl_Person)
        {
            try
            {
                List<SMS_Objects> obj_SMS_Objects_Li = new List<SMS_Objects>();
                Random rnd = new Random();
                string OTP = rnd.Next(1121, 9979).ToString();
                DataSet ds = new DataSet();
                ds = new DataLayer().getLoginDetails(obj_tbl_Person.Person_Mobile);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        SMS_Objects obj_SMS_Objects = new SMS_Objects();
                        obj_SMS_Objects.MobileNum = ds.Tables[0].Rows[0]["Person_Mobile1"].ToString().Trim();
                        obj_SMS_Objects.SMS_Content = "Dear " + ds.Tables[0].Rows[0]["Person_Name"].ToString() + " OTP for Login To Project Monitoring System App is " + OTP + ". Please Use This OTP for Login. Thankyou";
                        obj_SMS_Objects_Li.Add(obj_SMS_Objects);
                        obj_tbl_Person.Base_URL = "http://www.jnupepayment.in/";
                        obj_tbl_Person.District_Id = 0;
                        obj_tbl_Person.FinancialYear_Id = 3;
                        obj_tbl_Person.OTP = OTP;
                        obj_tbl_Person.Person_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Person_Id"].ToString());
                        obj_tbl_Person.response = "success";
                        obj_tbl_Person.Role_ULB = 3;
                        obj_tbl_Person.Zone_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Zone_Id"].ToString());
                        obj_tbl_Person.Circle_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Circle_Id"].ToString());
                        obj_tbl_Person.Division_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Division_Id"].ToString());
                        Utility.SendSMS(obj_SMS_Objects_Li);
                    }

                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        SMS_Objects obj_SMS_Objects = new SMS_Objects();
                        obj_SMS_Objects.MobileNum = ds.Tables[1].Rows[0]["Person_Mobile1"].ToString().Trim();
                        obj_SMS_Objects.SMS_Content = "Dear " + ds.Tables[1].Rows[0]["Person_Name"].ToString() + " OTP for Login To Project Monitoring System App is " + OTP + ". Please Use This OTP for Login. Thankyou";
                        obj_SMS_Objects_Li.Add(obj_SMS_Objects);
                        obj_tbl_Person.Base_URL = "https://www.urbandevelopmentpms.in/";
                        obj_tbl_Person.District_Id = Convert.ToInt32(ds.Tables[1].Rows[0]["M_Jurisdiction_Id"].ToString());
                        obj_tbl_Person.FinancialYear_Id = 3;
                        obj_tbl_Person.OTP = OTP;
                        obj_tbl_Person.Person_Id = Convert.ToInt32(ds.Tables[1].Rows[0]["Person_Id"].ToString());
                        obj_tbl_Person.response = "success";
                        obj_tbl_Person.Role_Vendor = 5;
                        obj_tbl_Person.ULB_Id = 0;
                        Utility.SendSMS(obj_SMS_Objects_Li);
                    }

                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        SMS_Objects obj_SMS_Objects = new SMS_Objects();
                        obj_SMS_Objects.MobileNum = ds.Tables[2].Rows[0]["Person_Mobile1"].ToString().Trim();
                        obj_SMS_Objects.SMS_Content = "Dear " + ds.Tables[2].Rows[0]["Person_Name"].ToString() + " OTP for Login To Project Monitoring System App is " + OTP + ". Please Use This OTP for Login. Thankyou";
                        obj_SMS_Objects_Li.Add(obj_SMS_Objects);
                        obj_tbl_Person.Base_URL = "https://www.urbandevelopmentpms.in/";
                        obj_tbl_Person.District_Id = Convert.ToInt32(ds.Tables[2].Rows[0]["M_Jurisdiction_Id"].ToString());
                        obj_tbl_Person.FinancialYear_Id = 2;
                        obj_tbl_Person.OTP = OTP;
                        obj_tbl_Person.Person_Id = Convert.ToInt32(ds.Tables[2].Rows[0]["Person_Id"].ToString());
                        obj_tbl_Person.response = "success";
                        obj_tbl_Person.Role_Inspection = 6;
                        //obj_tbl_Person.ULB_Id = 0;
                        
                        Utility.SendSMS(obj_SMS_Objects_Li);
                    }
                }
                else
                {
                    obj_tbl_Person.Person_Id = 0;
                    obj_tbl_Person.response = "Error";
                    obj_tbl_Person.OTP = "";
                }
            }
            catch (Exception)
            {
                obj_tbl_Person.Person_Id = 0;
                obj_tbl_Person.response = "Error";
                obj_tbl_Person.OTP = "";
            }
            return obj_tbl_Person;
        }
    }
}
