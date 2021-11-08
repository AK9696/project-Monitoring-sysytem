using ePayment_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ePayment_API.Repos
{
    public class Utility
    {
        public static bool CheckDataSet(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string Image_ToBase64(string Path)
        {
            using (Image image = Image.FromFile(Path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public static byte[] generateSecureKey()
        {
            Aes KEYGEN = Aes.Create();
            byte[] secretKey = KEYGEN.Key;
            return secretKey;
        }

        public static void SendSMS(List<SMS_Objects> obj_SMS_Objects)
        {
            for (int i = 0; i < obj_SMS_Objects.Count; i++)
            {
                string MobileNum = obj_SMS_Objects[i].MobileNum;
                string SMS_Content = obj_SMS_Objects[i].SMS_Content;
                string SMS_Response = obj_SMS_Objects[i].SMS_Response;
                string Sid = obj_SMS_Objects[i].Sid;
                string sms_URL = "";

                if (Sid == "" || string.IsNullOrEmpty(Sid))
                {
                    sms_URL = "http://prioritysms.tulsitainfotech.com/api/mt/SendSMS?user=urbandevelopment&password=urban&senderid=EURBAN&channel=Trans&DCS=0&flashsms=0&number=" + MobileNum + "&text=" + SMS_Content + "&route=15";
                }
                else
                {
                    sms_URL = "http://prioritysms.tulsitainfotech.com/api/mt/SendSMS?user=urbandevelopment&password=urban&senderid=" + Sid + "&channel=Trans&DCS=0&flashsms=0&number=" + MobileNum + "&text=" + SMS_Content + "&route=15";
                }

                try
                {
                    WebRequest request;
                    string MobileNo = MobileNum.Trim();
                    string sms = SMS_Content.Trim();
                    request = WebRequest.Create(sms_URL);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response1.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    String responseFromServer = reader.ReadToEnd();
                    SMS_Response = responseFromServer.ToString().Trim();

                    tbl_SMS obj_SMS = new tbl_SMS();
                    obj_SMS.SMS_Content = sms;
                    obj_SMS.SMS_Mobile_No = MobileNo;
                    obj_SMS.SMS_Response = SMS_Response;
                    //new DataLayer().Insert_tbl_SMS(obj_SMS);

                    reader.Close();
                    dataStream.Close();
                    response1.Close();
                }
                catch
                {
                    SMS_Response = "Error";
                }
            }
        }
    }
}
