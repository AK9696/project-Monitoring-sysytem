using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ePayment_API.Models;

namespace ePayment_API.Repos
{
    public class DeviceInfoRepository : RepositoryAsyn
    {
        public DeviceInfoRepository(string connectionString) : base(connectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public async Task<tbl_DeviceInfoResponse> User_DeviceInfo(tbl_DeviceInfo obj_tbl_DeviceInfo)
        {
            tbl_DeviceInfoResponse obj_tbl_DeviceInfoResponse = DeviceInfo(obj_tbl_DeviceInfo);
            return obj_tbl_DeviceInfoResponse;
        }
        private tbl_DeviceInfoResponse DeviceInfo(tbl_DeviceInfo obj_tbl_DeviceInfo)
        {
            tbl_DeviceInfoResponse obj_tbl_DeviceInfoResponse = new tbl_DeviceInfoResponse();
            new DataLayer().insert_tbl_DeviceInfo(obj_tbl_DeviceInfo);
            obj_tbl_DeviceInfoResponse.Person_Id = obj_tbl_DeviceInfo.Person_Id;
            obj_tbl_DeviceInfoResponse.Available_Version = "1.1.5";
            

            string [] currVersion = obj_tbl_DeviceInfo.App_Version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string [] serverVersion = obj_tbl_DeviceInfoResponse.Available_Version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            decimal _currVersion = 0;
            decimal _serverVersion = 0;

            for (int i = 0; i < currVersion.Length; i++)
            {
                try
                {
                    _currVersion += decimal.Parse(currVersion[i]);
                }
                catch
                {
                    _currVersion += 0;
                }
            }
            for (int i = 0; i < serverVersion.Length; i++)
            {
                try
                {
                    _serverVersion += decimal.Parse(serverVersion[i]);
                }
                catch
                {
                    _serverVersion += 0;
                }
            }

            if (_serverVersion != _currVersion)
            {
                obj_tbl_DeviceInfoResponse.Prompt_Update_Version = 1;
            }
            else
            {
                obj_tbl_DeviceInfoResponse.Prompt_Update_Version = 0;
            }
            return obj_tbl_DeviceInfoResponse;
        }
    }
}
