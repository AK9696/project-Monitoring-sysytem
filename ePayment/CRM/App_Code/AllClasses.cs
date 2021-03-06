using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Net.Sockets;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public class AllClasses : Page
{
    public AllClasses()
    {

    }
    public static void Create_Directory_Session(int _count)
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + _count.ToString() + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name" + _count.ToString()] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name" + _count.ToString()] = temp_folder;
        }
    }
    public static void Create_Directory_Session()
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name"] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name"] = temp_folder;
        }
    }
    public static void Create_Directory_Session3(int _count)
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + _count.ToString() + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name1"] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name1"] = temp_folder;
        }
    }
    public static void Create_Directory_Session1(int _count)
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + _count.ToString() + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name1" + _count.ToString()] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name1" + _count.ToString()] = temp_folder;
        }
    }
    public static void Create_Directory_Session2()
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name2"] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name2"] = temp_folder;
        }
    }
    public static void Create_Directory_Session2(int _count)
    {
        string temp_folder = DateTime.Now.Ticks.ToString("x") + _count.ToString() + "_" + HttpContext.Current.Session["Person_Id"].ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/"))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/K_Log/") + temp_folder + "/");
            HttpContext.Current.Session["Folder_Name2" + _count.ToString()] = temp_folder;
        }
        else
        {
            HttpContext.Current.Session["Folder_Name2" + _count.ToString()] = temp_folder;
        }
    }
    public void Render_PDF_Document(Literal _ltEmbed, string filePath)
    {
        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"900px\" height=\"600px\">";
        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
        embed += "</object>";
        _ltEmbed.Text = string.Format(embed, ResolveUrl(filePath.Replace("\\", "/")));
    }

    public static string ByteArr_To_ASCII(string string_ByteArr)
    {
        if (string_ByteArr == null)
        {
            return "";
        }
        else
        {
            byte[] uni = Convert.FromBase64String(string_ByteArr);
            return System.Text.UTF8Encoding.UTF8.GetString(uni);
        }
    }
    public static bool SendMail(string subject, string emailBody, string To_Email_Address, string From_Email_Address, string From_Email_Password)
    {
        bool strResult = false;
        try
        {
            var fromAddress = new MailAddress(From_Email_Address, From_Email_Address);
            var toAddress = new MailAddress(To_Email_Address, To_Email_Address);
            string fromPassword = From_Email_Password;
            string strsubject = subject;
            string body = emailBody;

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //};
            var smtp = new SmtpClient
            {
                Host = "smtpout.secureserver.net",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = strsubject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
            strResult = true;
        }
        catch (Exception ee)
        {
            strResult = false;
        }
        return strResult;
    }
    public static string getIPAddress()
    {
        try
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        catch
        {
            return "";
        }
    }
    public static string convert_To_Indian_No_Format(string data, string _DataType)
    {
        if (data == "")
        {
            return "0";
        }
        else
        {
            string retVal = "";
            retVal = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(data));
            if (_DataType == "Decimal")
            {

            }
            else
            {
                retVal = retVal.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            return retVal;
        }
    }
    public static string convert_To_Laks(string data)
    {
        if (data == "")
        {
            return "0";
        }
        if (data == "0")
        {
            return "0";
        }
        else
        {
            string retVal = "";
            retVal = decimal.Round((Convert.ToDecimal(data) / 100000), 2, MidpointRounding.AwayFromZero).ToString();
            return retVal;
        }
    }
    public static string getIPAddress2()
    {
        HttpContext context = HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    public static string getMACAddress()
    {
        try
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            if (macAddresses == "")
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            return macAddresses;
        }
        catch
        {
            return "";
        }
    }
    public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
    {
        DataTable dt = new DataTable();

        PropertyInfo[] columns = null;

        if (Linqlist == null) return dt;

        foreach (T Record in Linqlist)
        {

            if (columns == null)
            {
                columns = ((Type)Record.GetType()).GetProperties();
                foreach (PropertyInfo GetProperty in columns)
                {
                    Type colType = GetProperty.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                }
            }

            DataRow dr = dt.NewRow();

            foreach (PropertyInfo pinfo in columns)
            {
                dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                (Record, null);
            }

            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static string ConvertNumbertoWords(long number)
    {
        if (number == 0)
        {
            return "ZERO";
        }

        if (number < 0)
        {
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        }

        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " LAKES ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        //if ((number / 10) > 0)  
        //{  
        // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
        // number %= 10;  
        //}  
        if (number > 0)
        {
            if (words != "")
            {
                words += "AND ";
            }

            var unitsMap = new[]
            {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
            var tensMap = new[]
            {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
            if (number < 20)
            {
                words += unitsMap[number];
            }
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                {
                    words += " " + unitsMap[number % 10];
                }
            }
        }
        return words;
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
                sms_URL = "http://prioritysms.tulsitainfotech.com/api/mt/SendSMS?user=urbandevelopment&password=urban&senderid=MASTER&channel=Trans&DCS=0&flashsms=0&number=" + MobileNum + "&text=" + SMS_Content + "&route=15";
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
                new DataLayer().Insert_tbl_SMS(obj_SMS);

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
    public static string getAddress(string lat, string lng, string responseType)
    {
        string SMS_Response = "";
        string sms_URL = "https://maps.googleapis.com/maps/api/geocode/" + responseType + "?latlng=" + lat + "," + lng + "&key=AIzaSyD6sxtv-u4NTtcNychOw123dEULAPak1Fk";
        try
        {
            WebRequest request;
            request = WebRequest.Create(sms_URL);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response1.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            SMS_Response = responseFromServer.ToString().Trim();
            reader.Close();
            dataStream.Close();
            response1.Close();
        }
        catch
        {
            SMS_Response = "";
        }
        return SMS_Response;
    }
    public static string ConvertDTToJSONString(DataTable dt)
    {
        string[][] zaggedArray = new string[dt.Rows.Count][];
        int i = 0;
        string[] arrColVal = null;
        foreach (DataRow dr1 in dt.Rows)
        {
            arrColVal = new string[dt.Columns.Count];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                arrColVal[j] = dr1[j].ToString().Trim();
            }
            zaggedArray[i] = arrColVal;
            i = i + 1;
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string jsonString = js.Serialize(zaggedArray);
        return jsonString;
    }

    public static string ConvertListToJSONString(List<string> obj)
    {
        string[][] zaggedArray = new string[obj.Count][];
        JavaScriptSerializer js = new JavaScriptSerializer();
        string jsonString = js.Serialize(obj);
        return jsonString;
    }

    public static DataSet FillMonthCombo()
    {
        string Year = DateTime.Now.Year.ToString();
        string[] strText = { "---Select---", "January", "Feburary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        string[] strValue = { "0", "01-" + Year, "02-" + Year, "03-" + Year, "04-" + Year, "05-" + Year, "06-" + Year, "07-" + Year, "08-" + Year, "09-" + Year, "10-" + Year, "11-" + Year, "12-" + Year };
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();
        if (strText.Length == strValue.Length)
        {
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "Text";
            dt.Columns.Add(dc);
            DataColumn dc1 = new DataColumn();
            dc1.DataType = System.Type.GetType("System.String");
            dc1.ColumnName = "Value";
            dt.Columns.Add(dc1);
            for (int i = 0; i < strText.Length; i++)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr["Text"] = strText[i];
                dr["Value"] = strValue[i];
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
        }
        else
        {
            ds = null;
        }
        return ds;
    }
    public static DataTable FilterDT(DataTable dt, DataTable dsAccess, string CompareField1, string CompareField2, string TextShow)
    {
        DataTable dtResult = new DataTable();
        if (dt != null && dt.Rows.Count > 0)
        {
            var result = from dataRows1 in dt.AsEnumerable()
                         join dataRows2 in dsAccess.AsEnumerable()
                         on dataRows1.Field<int>(CompareField1) equals dataRows2.Field<int>(CompareField2)
                         select new { CompareField1 = dataRows1[CompareField1], TextShow = dataRows1[TextShow] };

            dtResult = AllClasses.LINQResultToDataTable(result);
        }
        dtResult.Columns["CompareField1"].ColumnName = CompareField1;
        dtResult.Columns["TextShow"].ColumnName = TextShow;
        return dtResult;
    }

    public bool getBarCode(string strData, int imageHeight, int imageWidth, string Forecolor, string Backcolor, bool bIncludeLabel, string strImageFormat, string strAlignment, string barcodeType, ref string savedPath)
    {
        bool flag = true;
        //BarCodeLib.Barcode b = new BarCodeLib.Barcode(BarCodeLib.TYPE.UPCA, "038000356216", Color.Black, Color.White, 300, 150);
        BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;

        switch (barcodeType)
        {
            case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
            case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
            case "UPC 2 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
            case "UPC 5 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
            case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
            case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
            case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
            case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
            case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
            case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
            case "Bookland-ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
            case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
            case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
            case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
            case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
            case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
            case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
            case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
            case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
            case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
            case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
            case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
            case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
            case "Telepen": type = BarcodeLib.TYPE.TELEPEN; break;
            case "FIM (Facing Identification Mark)": type = BarcodeLib.TYPE.FIM; break;
            default: break;
        }
        System.Drawing.Image barcodeImage = null;
        try
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            if (type != BarcodeLib.TYPE.UNSPECIFIED)
            {
                b.IncludeLabel = bIncludeLabel;

                //alignment
                switch (strAlignment.ToLower().Trim())
                {
                    case "c":
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        break;
                    case "r":
                        b.Alignment = BarcodeLib.AlignmentPositions.RIGHT;
                        break;
                    case "l":
                        b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                        break;
                    default:
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        break;
                }//switch

                if (Forecolor.Trim() == "" && Forecolor.Trim().Length != 6)
                {
                    Forecolor = "000000";
                }

                if (Backcolor.Trim() == "" && Backcolor.Trim().Length != 6)
                {
                    Backcolor = "FFFFFF";
                }
                //Forecolor = "FF0000";
                //===== Encoding performed here =====
                barcodeImage = b.Encode(type, strData.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);

                //===== Static Encoding performed here =====
                //barcodeImage = BarcodeLib.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.chkGenerateLabel.Checked, this.btnForeColor.BackColor, this.btnBackColor.BackColor);

                MemoryStream MemStream = new System.IO.MemoryStream();

                switch (strImageFormat)
                {
                    case "gif": barcodeImage.Save(MemStream, ImageFormat.Gif); break;
                    case "jpeg": barcodeImage.Save(MemStream, ImageFormat.Jpeg); break;
                    case "png": barcodeImage.Save(MemStream, ImageFormat.Png); break;
                    case "bmp": barcodeImage.Save(MemStream, ImageFormat.Bmp); break;
                    case "tiff": barcodeImage.Save(MemStream, ImageFormat.Tiff); break;
                    default: break;
                }
                //MemStream.WriteTo(Response.OutputStream);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(".") + "\\BarCode_Data\\" + strData + ".gif", MemStream.ToArray());
                //savedPath = HttpContext.Current.Server.MapPath(".") + "\\BarCode_Data\\" + strData + ".gif";

                savedPath = "BarCode_Data\\" + strData + ".gif";
            }
        }
        catch
        {
            flag = false;
        }
        finally
        {
            //Clean up / Dispose...
            //barcodeImage.Dispose();
        }

        return flag;
    }

    public bool getBarCodeInvoice(string strData, int imageHeight, int imageWidth, string Forecolor, string Backcolor, bool bIncludeLabel, string strImageFormat, string strAlignment, string barcodeType, ref string savedPath)
    {
        try
        {
            strData = strData.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[0].PadLeft(5, '0') + strData.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[1].PadLeft(5, '0');
        }
        catch
        {
            strData = strData.Replace("_", "").PadLeft(10, '0');
        }
        bool flag = true;
        //BarCodeLib.Barcode b = new BarCodeLib.Barcode(BarCodeLib.TYPE.UPCA, "038000356216", Color.Black, Color.White, 300, 150);
        BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;

        switch (barcodeType)
        {
            case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
            case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
            case "UPC 2 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
            case "UPC 5 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
            case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
            case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
            case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
            case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
            case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
            case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
            case "Bookland-ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
            case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
            case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
            case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
            case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
            case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
            case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
            case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
            case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
            case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
            case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
            case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
            case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
            case "Telepen": type = BarcodeLib.TYPE.TELEPEN; break;
            case "FIM (Facing Identification Mark)": type = BarcodeLib.TYPE.FIM; break;
            default: break;
        }
        System.Drawing.Image barcodeImage = null;
        try
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            if (type != BarcodeLib.TYPE.UNSPECIFIED)
            {
                b.IncludeLabel = bIncludeLabel;

                //alignment
                switch (strAlignment.ToLower().Trim())
                {
                    case "c":
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        break;
                    case "r":
                        b.Alignment = BarcodeLib.AlignmentPositions.RIGHT;
                        break;
                    case "l":
                        b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                        break;
                    default:
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        break;
                }//switch

                if (Forecolor.Trim() == "" && Forecolor.Trim().Length != 6)
                {
                    Forecolor = "000000";
                }

                if (Backcolor.Trim() == "" && Backcolor.Trim().Length != 6)
                {
                    Backcolor = "FFFFFF";
                }
                //Forecolor = "FF0000";
                //===== Encoding performed here =====
                barcodeImage = b.Encode(type, strData.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);

                //===== Static Encoding performed here =====
                //barcodeImage = BarcodeLib.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.chkGenerateLabel.Checked, this.btnForeColor.BackColor, this.btnBackColor.BackColor);

                MemoryStream MemStream = new System.IO.MemoryStream();

                switch (strImageFormat)
                {
                    case "gif": barcodeImage.Save(MemStream, ImageFormat.Gif); break;
                    case "jpeg": barcodeImage.Save(MemStream, ImageFormat.Jpeg); break;
                    case "png": barcodeImage.Save(MemStream, ImageFormat.Png); break;
                    case "bmp": barcodeImage.Save(MemStream, ImageFormat.Bmp); break;
                    case "tiff": barcodeImage.Save(MemStream, ImageFormat.Tiff); break;
                    default: break;
                }
                //MemStream.WriteTo(Response.OutputStream);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(".") + "\\BarCode_Invoice\\" + strData + ".gif", MemStream.ToArray());
                //savedPath = HttpContext.Current.Server.MapPath(".") + "\\BarCode_Data\\" + strData + ".gif";

                savedPath = "BarCode_Invoice\\" + strData + ".gif";
            }
        }
        catch
        {
            flag = false;
        }
        finally
        {
            //Clean up / Dispose...
            //barcodeImage.Dispose();
        }

        return flag;
    }

    public static string Get24HourTime(int hour, int minute, int seconds, string ToD, DateTime dt)
    {
        int year = dt.Year;
        int month = dt.Month;
        int day = dt.Day;
        if (ToD.ToUpper() == "PM")
        {
            hour = (hour % 12) + 12;
        }

        if (ToD.ToUpper() == "AM")
        {
            if (hour == 12)
            {
                hour = 0;
            }
        }

        return new DateTime(year, month, day, hour, minute, seconds).ToString("HH:mm:ss");
    }

    public static bool CheckDataSet(DataSet ds)
    {
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    public static void FillHour(ref DropDownList ddlHourdel)
    {
        ddlHourdel.Items.Clear();
        for (int i = 0; i < 24; i++)
        {
            ddlHourdel.Items.Add(i.ToString().PadLeft(2, '0'));
        }
    }

    public static void FillMinuteAndSecond(ref DropDownList ddlmindel)
    {
        ddlmindel.Items.Clear();
        for (int i = 0; i < 60; i++)
        {
            ddlmindel.Items.Add(i.ToString().PadLeft(2, '0'));
        }
    }

    public static void FillDropDown(DataTable dt, DropDownList ddl, string TextField, string ValueField)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.NewRow();

            dr[TextField] = "--Select--";
            dr[ValueField] = "0";
            dt.Rows.InsertAt(dr, 0);
            ddl.DataTextField = TextField;
            ddl.DataValueField = ValueField;
            ddl.DataSource = dt;
            ddl.DataBind();

            //dt.Rows.RemoveAt(0);
        }
    }

    public static void FillDropDown_WithOutSelect(DataTable dt, DropDownList ddl, string TextField, string ValueField)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            ddl.DataTextField = TextField;
            ddl.DataValueField = ValueField;

            ddl.DataSource = dt;
            ddl.DataBind();
        }
    }

    public static bool CheckDt(DataTable dt)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool chackDate(string _fromDate, string _tillDate)
    {
        DateTime dtCurrentDate = DateTime.ParseExact(HttpContext.Current.Session["ServerDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        DateTime dtFYStart = DateTime.ParseExact(HttpContext.Current.Session["FinancialYearStart"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime dtFYEnd = DateTime.ParseExact(HttpContext.Current.Session["FinancialYearEnd"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        DateTime fromDate = DateTime.ParseExact(_fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime tillDate = DateTime.ParseExact(_tillDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        TimeSpan tsFrom = fromDate - dtFYStart;
        TimeSpan tsTo = dtFYEnd - tillDate;

        TimeSpan tsCurr = dtCurrentDate - tillDate;

        if (tsCurr.TotalDays < 0)
        {
            return false;
        }
        if (tsFrom.TotalDays < 0)
        {
            return false;
        }
        if (tsTo.TotalDays < 0)
        {
            return false;
        }
        return true;
    }

    public static void Set_Dates(TextBox txtDateFrom, TextBox txtDateTill)
    {
        string[] FyEnd = HttpContext.Current.Session["FinancialYearEnd"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        string[] CurrDate = HttpContext.Current.Session["ServerDate"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        DateTime dtFyEnd = new DateTime(int.Parse(FyEnd[2]), int.Parse(FyEnd[1]), int.Parse(FyEnd[0]));
        DateTime dtCurrDate = new DateTime(int.Parse(CurrDate[2]), int.Parse(CurrDate[1]), int.Parse(CurrDate[0]));
        if (dtCurrDate.Subtract(dtFyEnd).TotalDays > 0)
        {
            txtDateFrom.Text = HttpContext.Current.Session["FinancialYearEnd"].ToString();
            txtDateTill.Text = HttpContext.Current.Session["FinancialYearEnd"].ToString();
        }
        else
        {
            txtDateFrom.Text = HttpContext.Current.Session["ServerDate"].ToString();
            txtDateTill.Text = HttpContext.Current.Session["ServerDate"].ToString();
        }
    }

    public static string getmonth(string mnth)
    {
        switch (mnth)
        {
            case "01":
                {
                    //  Label4.Text = "जनवरी";
                    return "January";

                }
            case "02":
                {
                    return "Febuary"; //Label9.Text = "फरवरी";

                }
            case "03":
                {
                    return "March"; //Label9.Text = "मार्च";


                }
            case "04":
                {
                    return "April"; //Label9.Text = "अप्रैल";

                }
            case "05":
                {
                    return "May"; //Label9.Text = "मई";

                }
            case "06":
                {
                    return "June"; //Label9.Text = "जून";

                }
            case "07":
                {
                    return "July"; //Label9.Text = "जुलाई";

                }
            case "08":
                {
                    return "August"; //Label9.Text = "अगस्त";

                }
            case "09":
                {
                    return "September"; //Label9.Text = "सितम्बर";

                }
            case "10":
                {
                    return "October"; //Label9.Text = "अक्टूबर";

                }
            case "11":
                {
                    return "November"; //Label9.Text = "नवम्बर";

                }
            case "12":
                {
                    return "December"; //Label9.Text = "दिसम्बर";

                }

        }
        return "";
    }

    public static string getmonthval(string mnth)
    {
        mnth = mnth.Substring(0, 3);
        switch (mnth)
        {
            case "Jan":
                {
                    return "1";
                }
            case "Feb":
                {
                    return "2";
                }
            case "Mar":
                {
                    return "3";
                }
            case "Apr":
                {
                    return "4";
                }
            case "May":
                {
                    return "5";
                }
            case "Jun":
                {
                    return "6";
                }
            case "Jul":
                {
                    return "7";
                }
            case "Aug":
                {
                    return "8";
                }
            case "Sep":
                {
                    return "9";
                }
            case "Oct":
                {
                    return "10";
                }
            case "Nov":
                {
                    return "11";
                }
            case "Dec":
                {
                    return "12";
                }
        }
        return "";
    }
    public static string re_Organize_GO_No(string GO_No, bool Replace_Special_Symbols)
    {
        string retVal = GO_No;
        retVal = GO_No.Replace("०", "0").Replace("१", "1").Replace("२", "2").Replace("३", "3").Replace("४", "4").Replace("५", "5").Replace("६", "6").Replace("७", "7").Replace("८", "8").Replace("९", "9").Replace(" ", "");
        if (Replace_Special_Symbols)
            retVal = retVal.Replace("-", "").Replace("/", "").Replace("\\", "").Replace("(", "").Replace(")", "");
        return retVal;
    }
}
public class SMS_Objects
{
    public string MobileNum { get; set; }
    public string SMS_Content { get; set; }
    public string SMS_Response { get; set; }
    public string Sid { get; set; }
}
public class tbl_SMS
{
    public int SMS_Id { get; set; }
    public string SMS_Mobile_No { get; set; }
    public string SMS_Content { get; set; }
    public string SMS_Response { get; set; }
    public string SMS_AddedOn { get; set; }
}

public class tbl_Zone
{
    public int Zone_AddedBy { get; set; }
    public string Zone_AddedOn { get; set; }
    public int Zone_ModifiedBy { get; set; }
    public string Zone_ModifiedOn { get; set; }
    public string Zone_Name { get; set; }
    public int Zone_Id { get; set; }
    public int Zone_Status { get; set; }
}

public class tbl_Circle
{
    public int Circle_AddedBy { get; set; }
    public string Circle_AddedOn { get; set; }
    public int Circle_ZoneId { get; set; }
    public string Circle_Name { get; set; }
    public int Circle_Id { get; set; }
    public int Circle_ModifiedBy { get; set; }
    public string Circle_ModifiedOn { get; set; }
    public int Circle_Status { get; set; }
}

public class tbl_Division
{
    public int Division_AddedBy { get; set; }
    public string Division_AddedOn { get; set; }
    public int Division_CircleId { get; set; }
    public string Division_Name { get; set; }
    public int Division_Id { get; set; }
    public int Division_ModifiedBy { get; set; }
    public string Division_ModifiedOn { get; set; }
    public int Division_Status { get; set; }
}

public class M_Jurisdiction
{
    public int M_Jurisdiction_Id { get; set; }
    public int M_Level_Id { get; set; }
    public string Jurisdiction_Name_Eng { get; set; }
    public int Jurisdiction_LokSabhaId { get; set; }
    public int Jurisdiction_VidhanSabhaId { get; set; }
    public int Parent_Jurisdiction_Id { get; set; }
    public string Jurisdiction_Code { get; set; }
    public int Created_By { get; set; }
    public string Created_Date { get; set; }
    public int M_Jurisdiction_ModifiedBy { get; set; }
    public string M_Jurisdiction_ModifiedOn { get; set; }
    public int Is_Active { get; set; }
}
public class tbl_Role
{
    public int Role_AddedBy { get; set; }
    public string Role_AddedOn { get; set; }
    public int Role_ModifiedBy { get; set; }
    public string Role_ModifiedOn { get; set; }
    public string Role_Name { get; set; }
    public int Role_Id { get; set; }
    public int Role_Status { get; set; }
}
public class tbl_Designation
{
    public int Designation_AddedBy { get; set; }
    public string Designation_AddedOn { get; set; }
    public int Designation_ModifiedBy { get; set; }
    public string Designation_ModifiedOn { get; set; }
    public string Designation_DesignationName { get; set; }
    public int Designation_Id { get; set; }
    public int Designation_Status { get; set; }
    public int Designation_Level { get; set; }
}
public class tbl_Department
{
    public int Department_AddedBy { get; set; }
    public string Department_AddedOn { get; set; }
    public int Department_ModifiedBy { get; set; }
    public string Department_ModifiedOn { get; set; }
    public string Department_Name { get; set; }
    public int Department_Id { get; set; }
    public int Department_Status { get; set; }
}
public class tbl_UserLogin
{
    public int Login_AddedBy { get; set; }
    public string Login_Addeddatetime { get; set; }
    public int Login_Id { get; set; }
    public string Login_password { get; set; }
    public int Login_PersonId { get; set; }
    public int Login_Status { get; set; }
    public string Login_UserName { get; set; }
}
public class tbl_OfficeBranch
{
    public int OfficeBranch_AddedBy { get; set; }
    public string OfficeBranch_AddedOn { get; set; }
    public string OfficeBranch_EmailId { get; set; }
    public string OfficeBranch_FullAddress { get; set; }
    public string OfficeBranch_GSTIN { get; set; }
    public int OfficeBranch_Id { get; set; }
    public string OfficeBranch_IsHO { get; set; }
    public int OfficeBranch_JurisdictionId { get; set; }
    public string OfficeBranch_LL { get; set; }
    public string OfficeBranch_Mobile { get; set; }
    public string OfficeBranch_Name { get; set; }
    public string OfficeBranch_PANNumber { get; set; }
    public string OfficeBranch_RegistrationNo { get; set; }
    public int OfficeBranch_Status { get; set; }
    public string OfficeBranch_WebSite { get; set; }
    public int OfficeBranch_ARV_Formula { get; set; }
    public string OfficeBranch_Logo { get; set; }
    public byte[] OfficeBranch_Logo_Bytes { get; set; }
}
public class tbl_PersonDetail
{
    public int Person_AddedBy { get; set; }
    public string Person_AddedOn { get; set; }
    public string Person_AddressLine1 { get; set; }
    public string Person_AddressLine2 { get; set; }
    public string Person_EmailId { get; set; }
    public int Person_Id { get; set; }
    public string Person_Mobile1 { get; set; }
    public string Person_Mobile2 { get; set; }
    public int Person_ModifiedBy { get; set; }
    public int Person_BranchOffice_Id { get; set; }
    public string Person_ModifiedOn { get; set; }
    public string Person_Name { get; set; }
    public string Person_FName { get; set; }
    public int Person_Status { get; set; }
    public string Person_TelePhone { get; set; }
    public string Person_ProfilePIC { get; set; }
}
public class tbl_PersonJuridiction
{
    public int M_Jurisdiction_Id { get; set; }
    public int M_Level_Id { get; set; }
    public int PersonJuridiction_AddedBy { get; set; }
    public int PersonJuridiction_ULB_Id { get; set; }
    public string PersonJuridiction_AddedOn { get; set; }
    public int PersonJuridiction_DepartmentId { get; set; }
    public int PersonJuridiction_ZoneId { get; set; }
    public int PersonJuridiction_CircleId { get; set; }
    public int PersonJuridiction_DivisionId { get; set; }
    public int PersonJuridiction_DesignationId { get; set; }
    public int PersonJuridiction_Id { get; set; }
    public int PersonJuridiction_ModifiedBy { get; set; }
    public string PersonJuridiction_ModifiedOn { get; set; }
    public int PersonJuridiction_ParentPerson_Id { get; set; }
    public int PersonJuridiction_PersonId { get; set; }
    public int PersonJuridiction_Status { get; set; }
    public int PersonJuridiction_UserTypeId { get; set; }
    public string PersonJuridiction_GSTIN { get; set; }
    public string PersonJuridiction_PAN { get; set; }
    public string PersonJuridiction_Firm_Name { get; set; }
    public string PersonJuridiction_EmpType { get; set; }
    public int PersonJuridiction_Project_Id { get; set; }
}
public class tbl_PersonRoleInfo
{
    public int PersonRoleInfo_PersonId { get; set; }
    public int PersonRoleInfo_AddedBy { get; set; }
    public string PersonRoleInfo_AddedOn { get; set; }
    public int PersonRoleInfo_Id { get; set; }
    public int PersonRoleInfo_RoleId { get; set; }
    public int PersonRoleInfo_ModifiedBy { get; set; }
    public string PersonRoleInfo_ModifiedOn { get; set; }
    public int PersonRoleInfo_Status { get; set; }
}
public class tbl_PersonAdditionalInfo
{
    public int PersonAdditionalInfo_PersonId { get; set; }
    public int PersonAdditionalInfo_AddedBy { get; set; }
    public string PersonAdditionalInfo_AddedOn { get; set; }
    public string PersonAdditionalInfo_Aniversery { get; set; }
    public string PersonAdditionalInfo_Birthday { get; set; }
    public string PersonAdditionalInfo_BloodGroup { get; set; }
    public string PersonAdditionalInfo_DOJ { get; set; }
    public string PersonAdditionalInfo_EmergencyContactPersonMobile { get; set; }
    public string PersonAdditionalInfo_EmergencyContactPersonName { get; set; }
    public int PersonAdditionalInfo_EnableBiometrics { get; set; }
    public int PersonAdditionalInfo_Id { get; set; }
    public int PersonAdditionalInfo_IdOnBiometrics { get; set; }
    public int PersonAdditionalInfo_ModifiedBy { get; set; }
    public string PersonAdditionalInfo_ModifiedOn { get; set; }
    public int PersonAdditionalInfo_Status { get; set; }
    public string PersonAdditionalInfo_ShiftIn { get; set; }
    public string PersonAdditionalInfo_ShiftOut { get; set; }
}
public class M_Jurisdiction_Detailed
{
    public int M_Level_Id { get; set; }
    public int State_Id { get; set; }
    public string State_Name { get; set; }
    public int District_Id { get; set; }
    public string District_Name { get; set; }
    public int Mandal_Id { get; set; }
    public string Mandal_Name { get; set; }
    public int Block_Id { get; set; }
    public string Block_Name { get; set; }
}
public class tbl_PersonAdditionalArea
{
    public int PersonAdditionalArea_AddedBy { get; set; }
    public string PersonAdditionalArea_AddedOn { get; set; }
    public int PersonAdditionalArea_ModifiedBy { get; set; }
    public string PersonAdditionalArea_ModifiedOn { get; set; }
    public int PersonAdditionalArea_Id { get; set; }
    public int PersonAdditionalArea_Person_Id { get; set; }
    public int PersonAdditionalArea_Designation_Id { get; set; }
    public int PersonAdditionalArea_ZoneId { get; set; }
    public int PersonAdditionalArea_CircleId { get; set; }
    public int PersonAdditionalArea_DivisionId { get; set; }
    public int PersonAdditionalArea_Status { get; set; }
}
public class tbl_LokSabha
{
    public int LokSabha_AddedBy { get; set; }
    public string LokSabha_AddedOn { get; set; }
    public int LokSabha_ModifiedBy { get; set; }
    public string LokSabha_ModifiedOn { get; set; }
    public string LokSabha_Name { get; set; }
    public int LokSabha_DistrictId { get; set; }
    public int LokSabha_Id { get; set; }
    public int LokSabha_Status { get; set; }
}
public class tbl_VidhanSabha
{
    public int VidhanSabha_AddedBy { get; set; }
    public string VidhanSabha_AddedOn { get; set; }
    public int VidhanSabha_LokSabhaId { get; set; }
    public string VidhanSabha_Name { get; set; }
    public int VidhanSabha_Id { get; set; }
    public int VidhanSabha_ModifiedBy { get; set; }
    public string VidhanSabha_ModifiedOn { get; set; }
    public int VidhanSabha_Status { get; set; }
}
public class tbl_ULB
{
    public int ULB_AddedBy { get; set; }
    public string ULB_AddedOn { get; set; }
    public int ULB_District_Id { get; set; }
    public int ULB_Id { get; set; }
    public int ULB_ModifiedBy { get; set; }
    public string ULB_ModifiedOn { get; set; }
    public string ULB_Name { get; set; }
    public int ULB_Old_District_Id { get; set; }
    public int ULB_Status { get; set; }
    public string ULB_Type { get; set; }
    public int ULB_VidhanSabha_Id { get; set; }
    public int ULB_LokSabha_Id { get; set; }
}
public class tbl_ProjectType
{
    public int ProjectType_AddedBy { get; set; }
    public string ProjectType_AddedOn { get; set; }
    public int ProjectType_ModifiedBy { get; set; }
    public string ProjectType_ModifiedOn { get; set; }
    public string ProjectType_Name { get; set; }
    public int ProjectType_Id { get; set; }
    public int ProjectType_Status { get; set; }
}
public class tbl_Bank
{
    public int Bank_AddedBy { get; set; }
    public string Bank_AddedOn { get; set; }
    public int Bank_ModifiedBy { get; set; }
    public string Bank_ModifiedOn { get; set; }
    public string Bank_Name { get; set; }
    public int Bank_Id { get; set; }
    public int Bank_Status { get; set; }
}
public class ProgramAddresses
{
    public string description { get; set; }
    public double lng { get; set; }
    public double lat { get; set; }
    public string EntityType { get; set; }
    /// <summary>
    /// 1: Log
    /// 0: Party
    /// </summary>
    public string History { get; set; }
    public string Dattime { get; set; }
}
public class tbl_ULBAccountDtls
{
    public int ULBAccount_ULB_Id { get; set; }
    public string ULBAccount_AccountNo { get; set; }
    public int ULBAccount_AddedBy { get; set; }
    public string ULBAccount_AddedOn { get; set; }
    public int ULBAccount_BankId { get; set; }
    public string ULBAccount_BranchAddress { get; set; }
    public string ULBAccount_BranchName { get; set; }
    public int ULBAccount_Id { get; set; }
    public string ULBAccount_IFSCCode { get; set; }
    public string ULBAccount_MICRCode { get; set; }
    public int ULBAccount_ModifiedBy { get; set; }
    public string ULBAccount_ModifiedOn { get; set; }
    public int ULBAccount_Status { get; set; }
    public int ULBAccount_SchemeId { get; set; }
    public int ULBAccount_Zone_Id      { get; set; }
    public int ULBAccount_Circle_Id    { get; set; }
    public int ULBAccount_Division_Id { get; set; }
}

public class tbl_Project
{
    public int Project_AddedBy { get; set; }
    public string Project_AddedOn { get; set; }
    public int Project_ModifiedBy { get; set; }
    public string Project_ModifiedOn { get; set; }
    public string Project_Name { get; set; }
    public string Project_Budget { get; set; }
    public string Project_GO_Path { get; set; }
    public byte[] Project_GO_Path_Bytes { get; set; }
    public string Project_Guideline_Path { get; set; }
    public byte[] Project_Guideline_Path_Bytes { get; set; }
    public int Project_Id { get; set; }
    public int Project_Status { get; set; }
}
public class tbl_ProjectWork
{
    public int ProjectWork_AddedBy { get; set; }
    public string ProjectWork_AddedOn { get; set; }
    public int ProjectWork_Project_Id { get; set; }
    public int ProjectWork_ProjectType_Id { get; set; }
    public string ProjectWork_Name { get; set; }
    public string ProjectWork_Description { get; set; }
    public string ProjectWork_ProjectCode { get; set; }
    public decimal ProjectWork_Budget { get; set; }
    public string ProjectWork_GO_Date { get; set; }
    public string ProjectWork_GO_No { get; set; }
    public string ProjectWork_GO_Path { get; set; }
    public byte[] ProjectWork_GO_Path_Bytes { get; set; }
    public int ProjectWork_Id { get; set; }
    public int ProjectWork_ModifiedBy { get; set; }
    public string ProjectWork_ModifiedOn { get; set; }
    public int ProjectWork_Status { get; set; }
    public int ProjectWork_DistrictId { get; set; }
    public int ProjectWork_ULB_Id { get; set; }
    public int ProjectWork_DivisionId { get; set; }
    public int ProjectWork_BlockId { get; set; }
    public decimal ProjectWork_Centage { get; set; }
}
public class tbl_ProjectAdditionalArea
{
    public int ProjectAdditionalArea_AddedBy { get; set; }
    public string ProjectAdditionalArea_AddedOn { get; set; }
    public int ProjectAdditionalArea_ModifiedBy { get; set; }
    public string ProjectAdditionalArea_ModifiedOn { get; set; }
    public int ProjectAdditionalArea_Id { get; set; }
    public int ProjectAdditionalArea_ProjectWork_Id { get; set; }
    public int ProjectAdditionalArea_ProjectWorkPkg_Id { get; set; }
    public int ProjectAdditionalArea_ZoneId { get; set; }
    public int ProjectAdditionalArea_CircleId { get; set; }
    public int ProjectAdditionalArea_DevisionId { get; set; }
    public int ProjectAdditionalArea_Status { get; set; }
}



public class tbl_Section
{
    public int Section_AddedBy { get; set; }
    public string Section_AddedOn { get; set; }
    public int Section_ModifiedBy { get; set; }
    public string Section_ModifiedOn { get; set; }
    public string Section_Name { get; set; }
    public int Section_Id { get; set; }
    public int Section_Status { get; set; }
}
public class tbl_Deduction
{
    public int Deduction_AddedBy { get; set; }
    public string Deduction_AddedOn { get; set; }
    public int Deduction_ModifiedBy { get; set; }
    public string Deduction_ModifiedOn { get; set; }
    public string Deduction_Name { get; set; }
    public string Deduction_Type { get; set; }
    public decimal Deduction_Value { get; set; }
    public int Deduction_Id { get; set; }
    public int Deduction_Status { get; set; }
    public string Deduction_Category { get; set; }
    public string Deduction_Mode { get; set; }
}
public class tbl_FundingPattern
{
    public int FundingPattern_AddedBy { get; set; }
    public string FundingPattern_AddedOn { get; set; }
    public int FundingPattern_ModifiedBy { get; set; }
    public string FundingPattern_ModifiedOn { get; set; }
    public string FundingPattern_Name { get; set; }
    public int FundingPattern_Id { get; set; }
    public int FundingPattern_Status { get; set; }
}

public class tbl_Unit
{
    public int Unit_AddedBy { get; set; }
    public string Unit_AddedOn { get; set; }
    public int Unit_ModifiedBy { get; set; }
    public string Unit_ModifiedOn { get; set; }
    public string Unit_Name { get; set; }
    public int Unit_Id { get; set; }
    public int Unit_Status { get; set; }
    public int Unit_Length_Applicable { get; set; }
    public int Unit_Bredth_Applicable { get; set; }
    public int Unit_Height_Applicable { get; set; }
}

public class tbl_ProjectWorkPkg
{
    public int ProjectWorkPkg_AddedBy { get; set; }
    public string ProjectWorkPkg_AddedOn { get; set; }
    public string ProjectWorkPkg_Agreement_Date { get; set; }
    public string ProjectWorkPkg_Due_Date { get; set; }
    public byte[] ProjectWorkPkg_AgreementPath_Bytes { get; set; }
    public string ProjectWorkPkg_Agreement_Extention { get; set; }
    public string ProjectWorkPkg_Agreement_No { get; set; }
    public int ProjectWorkPkg_LastRABillNo { get; set; }
    public string ProjectWorkPkg_LastRABillDate { get; set; }
    public string ProjectWorkPkg_Agreement_Path { get; set; }
    public string ProjectWorkPkg_BankGurantee_Path { get; set; }
    public string ProjectWorkPkg_Mobelization_Path { get; set; }
    public string ProjectWorkPkg_PerformanceSecurity_Path { get; set; }
    public Decimal ProjectWorkPkg_AgreementAmount { get; set; }
    public int ProjectWorkPkg_Id { get; set; }
    public string ProjectWorkPkg_Indent_No { get; set; }
    public int ProjectWorkPkg_ModifiedBy { get; set; }
    public string ProjectWorkPkg_ModifiedOn { get; set; }
    public string ProjectWorkPkg_Name { get; set; }
    public int ProjectWorkPkg_Staff_Id { get; set; }
    public int ProjectWorkPkg_Status { get; set; }
    public int ProjectWorkPkg_Vendor_Id { get; set; }
    public int ProjectWorkPkg_Work_Id { get; set; }
    public string ProjectWorkPkg_Code { get; set; }
    public int ProjectWorkPkg_Locked_By { get; set; }
    public string ProjectWorkPkg_LockedOn { get; set; }
    public byte[] ProjectWorkPkg_PerformanceSecurityPath_Bytes { get; set; }
    public string ProjectWorkPkg_PerformanceSecurity_Extention { get; set; }
    public byte[] ProjectWorkPkg_BankGuranteePath_Bytes { get; set; }
    public string ProjectWorkPkg_BankGurantee_Extention { get; set; }
    public byte[] ProjectWorkPkg_MobelizationPath_Bytes { get; set; }
    public string ProjectWorkPkg_Mobelization_Extention { get; set; }
    public byte[] ProjectWorkPkg_ApprovalFile_Path_Bytes { get; set; }
    public string ProjectWorkPkg_ApprovalFile_Extention { get; set; }
    public string ProjectWorkPkg_ApprovalFile_Path { get; set; }
    public string ProjectWorkPkg__ApprovalRemark { get; set; }
    public decimal ProjectWorkPkg_MobilizationAdvanceAmount { get; set; }
}
public class tbl_ProjectDPR
{
    public int ProjectDPR_AddedBy { get; set; }
    public string ProjectDPR_AddedOn { get; set; }
    public int ProjectDPR_Id { get; set; }
    public int ProjectDPR_Project_Id { get; set; }
    public int ProjectDPR_ProjectWork_Id { get; set; }
    public int ProjectDPR_ProjectWorkPkg_Id { get; set; }
    public byte[] ProjectDPR_DPRPDFPath_Bytes { get; set; }
    public string ProjectDPR_DPRPDFPath_Extention { get; set; }
    public string ProjectDPR_DPRPDFPath { get; set; }
    public byte[] ProjectDPR_DocumentDesignPath_Bytes { get; set; }
    public string ProjectDPR_DocumentDesignPath_Extention { get; set; }
    public string ProjectDPR_DocumentDesignPath { get; set; }
    public byte[] ProjectDPR_SitePic1Path_Bytes { get; set; }
    public string ProjectDPR_SitePic1Path_Extention { get; set; }
    public string ProjectDPR_SitePic1Path { get; set; }
    public byte[] ProjectDPR_SitePic2Path_Bytes { get; set; }
    public string ProjectDPR_SitePic2Path_Extention { get; set; }
    public string ProjectDPR_SitePic2Path { get; set; }
    public string ProjectDPR_Comments { get; set; }
    public int ProjectDPR_ModifiedBy { get; set; }
    public int ProjectDPR_Status { get; set; }
    public string ProjectDPR_ModifiedOn { get; set; }
   
}
public class tbl_PackageDivisionBOQItem
{
    public int PackageDivisionBOQItem_AddedBy { get; set; }
    public string PackageDivisionBOQItem_AddedOn { get; set; }
    public int PackageDivisionBOQItem_ProjectWorkPkg_Id { get; set; }
    public int PackageDivisionBOQItem_DevisionId { get; set; }
    public int PackageDivisionBOQItem_PackageBOQ_Id { get; set; }
    public int PackageDivisionBOQItem_Id { get; set; }
    public int PackageDivisionBOQItem_ModifiedBy { get; set; }
    public string PackageDivisionBOQItem_ModifiedOn { get; set; }
    public int PackageDivisionBOQItem_Status { get; set; }
}
public class tbl_Package_ExtraItem
{
    public int Package_ExtraItem_AddedBy { get; set; }
    public string Package_ExtraItem_AddedOn { get; set; }
    public int Package_ExtraItem_ProjectWorkPkg_Id { get; set; }
    public int Package_ExtraItem_Id { get; set; }
    public string Package_ExtraItem_ProcessStatus { get; set; }
    public int Package_ExtraItem_ModifiedBy { get; set; }
    public string Package_ExtraItem_ModifiedOn { get; set; }
    public int Package_ExtraItem_Status { get; set; }
    public string Package_ExtraItem_ProcessedOn { get; set; }
    public int Package_ExtraItem_ProcessedBy { get; set; }
    public string Package_ExtraItem_Comment { get; set; }
    public string Package_ExtraItem_ExtraItemFilePath { get; set; }
    public byte[] Package_ExtraItem_ExtraItemFilePath_Bytes { get; set; }
    public string Package_ExtraItem_ExtraItemFilePath_Extention { get; set; }
    public string Package_ExtraItem_ApprovalFilePath { get; set; }
    public byte[] Package_ExtraItem_ApprovalFilePath_Bytes { get; set; }
    public string Package_ExtraItem_ApprovalFilePath_Extention { get; set; }
}

public class tbl_ProjectWorkPkg_ReportingStaff_JE_APE
{
    public int ProjectWorkPkg_ReportingStaff_JE_APE_AddedBy { get; set; }
    public string ProjectWorkPkg_ReportingStaff_JE_APE_AddedOn { get; set; }
    public int ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_JE_APE_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedBy { get; set; }
    public string ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedOn { get; set; }
    public int ProjectWorkPkg_ReportingStaff_JE_APE_Status { get; set; }
    
}
public class tbl_ProjectWorkPkg_ReportingStaff_AE_PE
{
    public int ProjectWorkPkg_ReportingStaff_AE_PE_AddedBy { get; set; }
    public string ProjectWorkPkg_ReportingStaff_AE_PE_AddedOn { get; set; }
    public int ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_AE_PE_Id { get; set; }
    public int ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedBy { get; set; }
    public string ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedOn { get; set; }
    public int ProjectWorkPkg_ReportingStaff_AE_PE_Status { get; set; }

}
public class tbl_ProjectFundingPattern
{
    public int ProjectFundingPattern_AddedBy { get; set; }
    public string ProjectFundingPattern_AddedOn { get; set; }
    public int ProjectFundingPattern_FundingPattern_Id { get; set; }
    public int ProjectFundingPattern_Id { get; set; }
    public int ProjectFundingPattern_ModifiedBy { get; set; }
    public string ProjectFundingPattern_ModifiedOn { get; set; }
    public Decimal ProjectFundingPattern_Percentage { get; set; }
    public int ProjectFundingPattern_ProjectId { get; set; }
    public int ProjectFundingPattern_Status { get; set; }
    public Decimal ProjectFundingPattern_Value { get; set; }
}

public class tbl_ProjectWorkFundingPattern
{
    public int ProjectWorkFundingPattern_AddedBy { get; set; }
    public string ProjectWorkFundingPattern_AddedOn { get; set; }
    public int ProjectWorkFundingPattern_FundingPatternId { get; set; }
    public int ProjectWorkFundingPattern_Id { get; set; }
    public int ProjectWorkFundingPattern_ModifiedBy { get; set; }
    public string ProjectWorkFundingPattern_ModifiedOn { get; set; }
    public Decimal ProjectWorkFundingPattern_Percentage { get; set; }
    public int ProjectWorkFundingPattern_ProjectWorkId { get; set; }
    public int ProjectWorkFundingPattern_Status { get; set; }
    public Decimal ProjectWorkFundingPattern_Value { get; set; }
}
public class tbl_PackageBOQ
{
    public int PackageBOQ_AddedBy { get; set; }
    public string PackageBOQ_AddedOn { get; set; }
    public Decimal PackageBOQ_AmountEstimated { get; set; }
    public Decimal PackageBOQ_AmountQuoted { get; set; }
    public int PackageBOQ_Id { get; set; }
    public int PackageBOQ_ModifiedBy { get; set; }
    public string PackageBOQ_ModifiedOn { get; set; }
    public int PackageBOQ_Package_Id { get; set; }
    public Decimal PackageBOQ_Qty { get; set; }
    public Decimal PackageBOQ_QtyPaid { get; set; }
    public Decimal PackageBOQ_RateEstimated { get; set; }
    public Decimal PackageBOQ_RateQuoted { get; set; }
    public string PackageBOQ_Specification { get; set; }
    public int PackageBOQ_Status { get; set; }
    public int PackageBOQ_Is_Approved { get; set; }
    public int PackageBOQ_Unit_Id { get; set; }
    public decimal PackageBOQ_PercentageValuePaidTillDate { get; set; }
    public string GSTType { get; set; }
    public decimal GSTPercenatge { get; set; }
    public int PackageBOQ_OrderNo { get; set; }
}

public class tbl_PackageEMB
{
    public int PackageEMB_AddedBy { get; set; }
    public string PackageEMB_AddedOn { get; set; }
    public string PackageEMB_Length { get; set; }
    public string PackageEMB_Breadth { get; set; }
    public int PackageEMB_Id { get; set; }
    public int PackageEMB_PackageBOQ_Id { get; set; }
    public int PackageEMB_ModifiedBy { get; set; }
    public string PackageEMB_ModifiedOn { get; set; }
    public int PackageEMB_Package_Id { get; set; }
    public Decimal PackageEMB_Qty { get; set; }
    public string PackageEMB_Height { get; set; }
    public Decimal PackageEMB_QtyExtra { get; set; }
    public string PackageEMB_Contents { get; set; }
    public string PackageEMB_Specification { get; set; }
    public int PackageEMB_Status { get; set; }
    public int PackageEMB_Unit_Id { get; set; }
    public int PackageEMB_Is_Approved { get; set; }
    public int PackageEMB_PackageEMB_Master_Id { get; set; }
    public decimal PackageEMB_PercentageToBeReleased { get; set; }
    public string PackageEMB_GSTType { get; set; }
    public int PackageEMB_GSTPercenatge { get; set; }
    public int PackageEMB_PackageBOQ_OrderNo { get; set; }
}
public class tbl_PackageInvoice
{
    public int PackageInvoice_AddedBy { get; set; }
    public string PackageInvoice_AddedOn { get; set; }
    public string PackageInvoice_Date { get; set; }
    public string PackageInvoice_DBR_No { get; set; }
    public int PackageInvoice_Id { get; set; }
    public int PackageInvoice_ModifiedBy { get; set; }
    public string PackageInvoice_ModifiedOn { get; set; }
    public string PackageInvoice_Narration { get; set; }
    public int PackageInvoice_Package_Id { get; set; }
    public string PackageInvoice_SBR_No { get; set; }
    public int PackageInvoice_Status { get; set; }
    public string PackageInvoice_VoucherNo { get; set; }
    public int PackageInvoice_PackageEMBMaster_Id { get; set; }
    public string PackageInvoice_ProcessedOn { get; set; }
    public int PackageInvoice_ProcessedBy { get; set; }
    public string PackageInvoice_ApprovedOn { get; set; }
    public int PackageInvoice_ApprovedBy { get; set; }
}

public class tbl_PackageInvoiceAdditional
{
    public int PackageInvoiceAdditional_AddedBy { get; set; }
    public string PackageInvoiceAdditional_AddedOn { get; set; }
    public int PackageInvoiceAdditional_Deduction_Id { get; set; }
    public Char PackageInvoiceAdditional_Deduction_Type { get; set; }
    public decimal PackageInvoiceAdditional_Deduction_Value_Final { get; set; }
    public decimal PackageInvoiceAdditional_Deduction_Value_Master { get; set; }
    public int PackageInvoiceAdditional_Id { get; set; }
    public int PackageInvoiceAdditional_Invoice_Id { get; set; }
    public int PackageInvoiceAdditional_ModifiedBy { get; set; }
    public string PackageInvoiceAdditional_ModifiedOn { get; set; }
    public int PackageInvoiceAdditional_Status { get; set; }
    public string PackageInvoiceAdditional_Comments { get; set; }
    public string PackageInvoiceAdditional_Deduction_isFlat { get; set; }
}
public class tbl_PackageInvoiceEMBMasterLink
{
    public int PackageInvoiceEMBMasterLink_AddedBy { get; set; }
    public string PackageInvoiceEMBMasterLink_AddedOn { get; set; }
    public int PackageInvoiceEMBMasterLink_Invoice_Id { get; set; }
    public int PackageInvoiceEMBMasterLink_EMBMaster_Id { get; set; }
    public int PackageInvoiceEMBMasterLink_Id { get; set; }
    public int PackageInvoiceEMBMasterLink_ModifiedBy { get; set; }
    public string PackageInvoiceEMBMasterLink_ModifiedOn { get; set; }
    public int PackageInvoiceEMBMasterLink_Status { get; set; }
}



public class tbl_PackageInvoiceDocs
{
    public int PackageInvoiceDocs_AddedBy { get; set; }
    public string PackageInvoiceDocs_AddedOn { get; set; }
    public string PackageInvoiceDocs_FileName { get; set; }
    public byte[] PackageInvoiceDocs_FileBytes { get; set; }
    public int PackageInvoiceDocs_Id { get; set; }
    public int PackageInvoiceDocs_Invoice_Id { get; set; }
    public int PackageInvoiceDocs_ModifiedBy { get; set; }
    public string PackageInvoiceDocs_ModifiedOn { get; set; }
    public int PackageInvoiceDocs_Status { get; set; }
    public string PackageInvoiceDocs_OrderNo { get; set; }
    public string PackageInvoiceDocs_Comments { get; set; }
    public int PackageInvoiceDocs_Type { get; set; }
}


public class tbl_PackageInvoiceItem
{
    public int PackageInvoiceItem_AddedBy { get; set; }
    public string PackageInvoiceItem_AddedOn { get; set; }
    public Decimal PackageInvoiceItem_AmountQuoted { get; set; }
    public int PackageInvoiceItem_BOQ_Id { get; set; }
    public int PackageInvoiceItem_Id { get; set; }
    public int PackageInvoiceItem_Invoice_Id { get; set; }
    public int PackageInvoiceItem_ModifiedBy { get; set; }
    public string PackageInvoiceItem_ModifiedOn { get; set; }
    public Decimal PackageInvoiceItem_RateEstimated { get; set; }
    public Decimal PackageInvoiceItem_RateQuoted { get; set; }
    public string PackageInvoiceItem_Remarks { get; set; }
    public int PackageInvoiceItem_Status { get; set; }
    public Decimal PackageInvoiceItem_Total_Qty { get; set; }
    public Decimal PackageInvoiceItem_Total_Qty_Billed { get; set; }
    public Decimal PackageInvoiceItem_Total_Qty_BOQ { get; set; }
    public Decimal PackageInvoiceItem_QtyExtra { get; set; }
    public Decimal PackageInvoiceItem_PercentageToBeReleased { get; set; }
    public List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li { get; set; }
    public int PackageInvoiceItem_PackageEMB_Id { get; set; }
    public string PackageInvoiceItem_GSTType { get; set; }
    public int PackageInvoiceItem_PackageBOQ_OrderNo { get; set; }
}
public class tbl_PackageBOQ_Approval
{
    public int PackageBOQ_Approval_AddedBy { get; set; }
    public string PackageBOQ_Approval_AddedOn { get; set; }
    public Decimal PackageBOQ_Approval_Approved_Qty { get; set; }
    public string PackageBOQ_Approval_Comments { get; set; }
    public string PackageBOQ_Approval_Date { get; set; }
    public int PackageBOQ_Approval_Id { get; set; }
    public int PackageBOQ_Approval_ModifiedBy { get; set; }
    public string PackageBOQ_Approval_ModifiedOn { get; set; }
    public string PackageBOQ_Approval_No { get; set; }
    public int PackageBOQ_Approval_PackageBOQ_Id { get; set; }
    public int PackageBOQ_Approval_Person_Id { get; set; }
    public int PackageBOQ_Approval_Status { get; set; }
    public string PackageBOQ_DocumentPath { get; set; }
}

public class tbl_PackageEMB_ExtraItem
{
    public int PackageEMB_ExtraItem_AddedBy { get; set; }
    public string PackageEMB_ExtraItem_AddedOn { get; set; }
    public int PackageEMB_ExtraItem_Id { get; set; }
    public int PackageEMB_ExtraItem_PackageEMB_Master_Id { get; set; }
    public string PackageEMB_ExtraItem_ApproveNo { get; set; }
    public string PackageEMB_ExtraItem_ApproveDate { get; set; }
    public string PackageEMB_ExtraItem_Comment { get; set; }
    public int PackageEMB_ExtraItem_ModifiedBy { get; set; }
    public string PackageEMB_ExtraItem_ModifiedOn { get; set; }
    public int PackageEMB_ExtraItem_Status { get; set; }
    public string PackageEMB_ExtraItem_ApprovalFilePath { get; set; }
    public byte[] PackageEMB_ExtraItem_ApprovalFilePath_Bytes { get; set; }
    public string PackageEMB_ExtraItem_ApprovalFilePath_Extention { get; set; }
}

public class tbl_PackageEMB_Approval
{
    public int PackageEMB_Approval_AddedBy { get; set; }
    public string PackageEMB_Approval_AddedOn { get; set; }
    public Decimal PackageEMB_Approval_Approved_Qty { get; set; }
    public string PackageEMB_Approval_Comments { get; set; }
    public string PackageEMB_Approval_Date { get; set; }
    public int PackageEMB_Approval_Id { get; set; }
    public int PackageEMB_Approval_ModifiedBy { get; set; }
    public string PackageEMB_Approval_ModifiedOn { get; set; }
    public string PackageEMB_Approval_No { get; set; }
    public int PackageEMB_Approval_PackageEMB_Id { get; set; }
    public int PackageEMB_Approval_Person_Id { get; set; }
    public int PackageEMB_Approval_Status { get; set; }
    public string PackageEMB_DocumentPath { get; set; }
}

[Serializable]
public class tbl_ProcessConfigMaster
{
    public int ProcessConfigMaster_AddedBy { get; set; }
    public string ProcessConfigMaster_AddedOn { get; set; }
    public int ProcessConfigMaster_Department_Id { get; set; }
    public int ProcessConfigMaster_Designation_Id { get; set; }
    public int ProcessConfigMaster_Designation_Id1 { get; set; }
    public int ProcessConfigMaster_Loop { get; set; }
    public int ProcessConfigMaster_Id { get; set; }
    public int ProcessConfigMaster_ModifiedBy { get; set; }
    public string ProcessConfigMaster_ModifiedOn { get; set; }
    public int ProcessConfigMaster_OrgId { get; set; }
    public string ProcessConfigMaster_Process_Name { get; set; }
    public int ProcessConfigMaster_Status { get; set; }
    public string Organization_Name { get; set; }
    public string Department_Name { get; set; }
    public string Designation_Name { get; set; }
    public string Designation_Name1 { get; set; }
    public string ProcessConfigInvStatus_InvoiceStatus_Id { get; set; }
    public string InvoiceStatus_Name { get; set; }
    public int ProcessConfigMaster_Creation_Allowed { get; set; }
    public int ProcessConfigMaster_Updation_Allowed { get; set; }
    public int ProcessConfigMaster_Deduction_Allowed { get; set; }
    public int ProcessConfigMaster_Transfer_Allowed { get; set; }
    public string ProcessConfigDocumentLinking_DocumentMaster_Id { get; set; }
    public string TradeDocument_Name { get; set; }
}
public enum VoucherTypes
{
    EMB,
    Invoice
}

public class tbl_PackageInvoiceApproval
{
    public int PackageInvoiceApproval_AddedBy { get; set; }
    public string PackageInvoiceApproval_AddedOn { get; set; }
    public string PackageInvoiceApproval_Comments { get; set; }
    public string PackageInvoiceApproval_Date { get; set; }
    public int PackageInvoiceApproval_Id { get; set; }
    public int PackageInvoiceApproval_ModifiedBy { get; set; }
    public string PackageInvoiceApproval_ModifiedOn { get; set; }
    public int PackageInvoiceApproval_Next_Designation_Id { get; set; }
    public int PackageInvoiceApproval_Next_Organisation_Id { get; set; }
    public int PackageInvoiceApproval_Package_Id { get; set; }
    public int PackageInvoiceApproval_PackageInvoice_Id { get; set; }
    public int PackageInvoiceApproval_Status { get; set; }
    public int PackageInvoiceApproval_Status_Id { get; set; }
    public int PackageInvoiceApproval_Step_Count { get; set; }
}

public class tbl_FinancialTrans
{
    public int FinancialTrans_AddedBy { get; set; }
    public string FinancialTrans_AddedOn { get; set; }
    public string FinancialTrans_Comments { get; set; }
    public string FinancialTrans_Date { get; set; }
    public string FinancialTrans_EntryType { get; set; }
    public int FinancialTrans_FinancialYear_Id { get; set; }
    public int FinancialTrans_Id { get; set; }
    public int FinancialTrans_Invoice_Id { get; set; }
    public int FinancialTrans_ModifiedBy { get; set; }
    public string FinancialTrans_ModifiedOn { get; set; }
    public int FinancialTrans_Status { get; set; }
    public Decimal FinancialTrans_Amount { get; set; }
    public Decimal FinancialTrans_TransAmount { get; set; }
    public string FinancialTrans_TransType { get; set; }
    public int FinancialTrans_Work_Id { get; set; }
    public int FinancialTrans_Package_Id { get; set; }
    public byte[] FinancialTrans_FileBytes1 { get; set; }
    public string FinancialTrans_FilePath1 { get; set; }
    public string FinancialTrans_GO_Date { get; set; }
    public string FinancialTrans_GO_Number { get; set; }
}

public class tbl_ProjectWorkGO
{
    public int ProjectWorkGO_AddedBy { get; set; }
    public string ProjectWorkGO_AddedOn { get; set; }
    public int ProjectWorkGO_Id { get; set; }
    public string ProjectWorkGO_GO_Date { get; set; }
    public string ProjectWorkGO_GO_Number { get; set; }
    public int ProjectWorkGO_Work_Id { get; set; }
    public int ProjectWorkGO_ModifiedBy { get; set; }
    public string ProjectWorkGO_ModifiedOn { get; set; }
    public int ProjectWorkGO_Status { get; set; }
    public Decimal ProjectWorkGO_TotalRelease { get; set; }
    public Decimal ProjectWorkGO_CentralShare { get; set; }
    public Decimal ProjectWorkGO_StateShare { get; set; }
    public Decimal ProjectWorkGO_ULBShare { get; set; }
}
public class tbl_PackageEMB_Master
{
    public int PackageEMB_Master_AddedBy { get; set; }
    public string PackageEMB_Master_AddedOn { get; set; }
    public int PackageEMB_Master_ApprovedBy { get; set; }
    public string PackageEMB_Master_ApprovedOn { get; set; }
    public string PackageEMB_Master_Date { get; set; }
    public int PackageEMB_Master_Id { get; set; }
    public int PackageEMB_Master_ModifiedBy { get; set; }
    public string PackageEMB_Master_ModifiedOn { get; set; }
    public string PackageEMB_Master_Narration { get; set; }
    public int PackageEMB_Master_Package_Id { get; set; }
    public int PackageEMB_Master_ProcessedBy { get; set; }
    public string PackageEMB_Master_ProcessedOn { get; set; }
    public int PackageEMB_Master_Status { get; set; }
    public string PackageEMB_Master_VoucherNo { get; set; }
    public string PackageEMB_Master_RA_BillNo { get; set; }
}
public class File_Objects
{
    public int Package_Id { get; set; }
    public int Work_Id { get; set; }
    public int ProjectDPR_Id { get; set; }
    public int Added_By { get; set; }
    public string File_Path_1 { get; set; }
    public byte[] File_Path_Bytes_1 { get; set; }
    public string File_Path_2 { get; set; }
    public byte[] File_Path_Bytes_2 { get; set; }
    public string File_Path_3 { get; set; }
    public byte[] File_Path_Bytes_3 { get; set; }
    public string Comments { get; set; }
}

public class tbl_PackageEMB_Tax
{
    public int PackageEMB_Tax_AddedBy { get; set; }
    public string PackageEMB_Tax_AddedOn { get; set; }
    public int PackageEMB_Tax_ModifiedBy { get; set; }
    public string PackageEMB_Tax_ModifiedOn { get; set; }
    public int PackageEMB_Tax_Package_Id { get; set; }
    public int PackageEMB_Tax_Deduction_Id { get; set; }
    public decimal PackageEMB_Tax_Value { get; set; }
    public int PackageEMB_Tax_Id { get; set; }
    public int PackageEMB_Tax_Status { get; set; }
}
public class tbl_PackageInvoiceItem_Tax
{
    public int PackageInvoiceItem_Tax_AddedBy { get; set; }
    public string PackageInvoiceItem_Tax_AddedOn { get; set; }
    public int PackageInvoiceItem_Tax_ModifiedBy { get; set; }
    public string PackageInvoiceItem_Tax_ModifiedOn { get; set; }
    public int PackageInvoiceItem_Tax_PackageInvoiceItem_Id { get; set; }
    public int PackageInvoiceItem_Tax_Deduction_Id { get; set; }
    public decimal PackageInvoiceItem_Tax_Value { get; set; }
    public int PackageInvoiceItem_Tax_Id { get; set; }
    public int PackageInvoiceItem_Tax_Status { get; set; }
}
public class tbl_TradeDocument
{
    public int TradeDocument_AddedBy { get; set; }
    public string TradeDocument_AddedOn { get; set; }
    public int TradeDocument_ModifiedBy { get; set; }
    public string TradeDocument_ModifiedOn { get; set; }
    public string TradeDocument_Name { get; set; }
    public int TradeDocument_Id { get; set; }
    public int TradeDocument_Status { get; set; }
}
public class tbl_ProcessConfigDocumentLinking
{
    public int ProcessConfigDocumentLinking_AddedBy { get; set; }
    public string ProcessConfigDocumentLinking_AddedOn { get; set; }
    public int ProcessConfigDocumentLinking_ConfigMasterId { get; set; }
    public int ProcessConfigDocumentLinking_DocumentMaster_Id { get; set; }
    public int ProcessConfigDocumentLinking_Id { get; set; }
    public int ProcessConfigDocumentLinking_ModifiedBy { get; set; }
    public string ProcessConfigDocumentLinking_ModifiedOn { get; set; }
    public int ProcessConfigDocumentLinking_Status { get; set; }
}
public class tbl_PersonAdditionalCharge
{
    public int PersonAdditionalCharge_AddedBy { get; set; }
    public string PersonAdditionalCharge_AddedOn { get; set; }
    public int PersonAdditionalCharge_Id { get; set; }
    public int PersonAdditionalCharge_Jurisdiction_Id { get; set; }
    public int PersonAdditionalCharge_Level_Id { get; set; }
    public int PersonAdditionalCharge_ModifiedBy { get; set; }
    public string PersonAdditionalCharge_ModifiedOn { get; set; }
    public int PersonAdditionalCharge_PersonId { get; set; }
    public int PersonAdditionalCharge_Status { get; set; }
}
public class tbl_PackageEMBApproval
{
    public int PackageEMBApproval_AddedBy { get; set; }
    public string PackageEMBApproval_AddedOn { get; set; }
    public string PackageEMBApproval_Comments { get; set; }
    public string PackageEMBApproval_Date { get; set; }
    public int PackageEMBApproval_Id { get; set; }
    public int PackageEMBApproval_ModifiedBy { get; set; }
    public string PackageEMBApproval_ModifiedOn { get; set; }
    public int PackageEMBApproval_Next_Designation_Id { get; set; }
    public int PackageEMBApproval_Next_Organisation_Id { get; set; }
    public int PackageEMBApproval_Package_Id { get; set; }
    public int PackageEMBApproval_PackageEMBMaster_Id { get; set; }
    public int PackageEMBApproval_Status { get; set; }
    public int PackageEMBApproval_Status_Id { get; set; }
    public int PackageEMBApproval_Step_Count { get; set; }
}


public class tbl_TicketCategory
{
    public int TicketCategory_AddedBy { get; set; }
    public string TicketCategory_AddedOn { get; set; }
    public int TicketCategory_ModifiedBy { get; set; }
    public string TicketCategory_ModifiedOn { get; set; }
    public string TicketCategory_Name { get; set; }
    public int TicketCategory_Id { get; set; }
    public int TicketCategory_Status { get; set; }
}

public class tbl_ProjectPkgTenderInfo
{
    public int ProjectPkgTenderInfo_AddedBy { get; set; }
    public string ProjectPkgTenderInfo_AddedOn { get; set; }
    public string ProjectPkgTenderInfo_Comments { get; set; }
    public int ProjectPkgTenderInfo_Id { get; set; }
    public int ProjectPkgTenderInfo_ModifiedBy { get; set; }
    public string ProjectPkgTenderInfo_ModifiedOn { get; set; }
    public int ProjectPkgTenderInfo_ProjectPkg_Id { get; set; }
    public int ProjectPkgTenderInfo_ProjectWork_Id { get; set; }
    public int ProjectPkgTenderInfo_Status { get; set; }
    public Decimal ProjectPkgTenderInfo_TenderAmount { get; set; }
    public string ProjectPkgTenderInfo_TenderDate { get; set; }
    public string ProjectPkgTenderInfo_CompletionTime { get; set; }
    public string ProjectPkgTenderInfo_NITDate { get; set; }
    public string ProjectPkgTenderInfo_TenderStatus { get; set; }
    public int ProjectPkgTenderInfo_VendorPersonId { get; set; }
    public decimal ProjectPkgTenderInfo_Centage { get; set; }
    public decimal ProjectPkgTenderInfo_WorkCostIn { get; set; }
    public decimal ProjectPkgTenderInfo_WorkCostOut { get; set; }
    public decimal ProjectPkgTenderInfo_GSTNotIncludeWorkCost { get; set; }
    public string ProjectPkgTenderInfo_PrebidMeetingDate { get; set; }
    public string ProjectPkgTenderInfo_TenderOutDate { get; set; }
    public string ProjectPkgTenderInfo_TenderTechnicalDate { get; set; }
    public string ProjectPkgTenderInfo_TenderFinancialDate { get; set; }
    public string ProjectPkgTenderInfo_ContractSignDate { get; set; }
    public string ProjectPkgTenderInfo_ContractBondNo { get; set; }
}

public class tbl_ProjectPKGInspectionInfo
{
    public int ProjectPKGInspectionInfo_AddedBy { get; set; }
    public string ProjectPKGInspectionInfo_AddedOn { get; set; }
    public int ProjectPKGInspectionInfo_Id { get; set; }
    public int ProjectPKGInspectionInfo_ModifiedBy { get; set; }
    public string ProjectPKGInspectionInfo_ModifiedOn { get; set; }
    public int ProjectPKGInspectionInfo_ProjectPkg_Id { get; set; }
    public int ProjectPKGInspectionInfo_ProjectWork_Id { get; set; }
    public int ProjectPKGInspectionInfo_Status { get; set; }
    public int ProjectPKGInspectionInfo_InspectionPersonId { get; set; }
}
[Serializable]
public class tbl_PersonDetail_Temp
{
    public int Person_AddedBy { get; set; }
    public int Person_Id { get; set; }
    public string Person_Mobile1 { get; set; }
    public string Person_Mobile2 { get; set; }
    public int Person_BranchOffice_Id { get; set; }
    public string Person_Name { get; set; }
    public string Designation_Name { get; set; }
    public int Person_Status { get; set; }
    public int Designation_Id { get; set; }
    //public string District_Name { get; set; }
    //public int District_Id { get; set; }
    public string Zone_Name { get; set; }
    public int Zone_Id { get; set; }
    public string Circle_Name { get; set; }
    public int Circle_Id { get; set; }
    public string Division_Name { get; set; }
    public int Division_Id { get; set; }
}

public class tbl_PackageInvoiceCover
{
    public int PackageInvoiceCover_AddedBy { get; set; }
    public string PackageInvoiceCover_AddedOn { get; set; }
    public Decimal PackageInvoiceCover_SanctionedAmount { get; set; }
    public Decimal PackageInvoiceCover_Centage { get; set; }
    public Decimal PackageInvoiceCover_CentralShare { get; set; }
    public Decimal PackageInvoiceCover_DiversionIn { get; set; }
    public Decimal PackageInvoiceCover_DiversionOut { get; set; }
    public int PackageInvoiceCover_Id { get; set; }
    public int PackageInvoiceCover_Invoice_Id { get; set; }
    public Decimal PackageInvoiceCover_MoblizationAdvance { get; set; }
    public Decimal PackageInvoiceCover_MoblizationAdvanceAdjustment { get; set; }
    public int PackageInvoiceCover_ModifiedBy { get; set; }
    public string PackageInvoiceCover_ModifiedOn { get; set; }
    public Decimal PackageInvoiceCover_PaymentTillDate { get; set; }
    public Decimal PackageInvoiceCover_ReleaseTillDate { get; set; }
    public Decimal PackageInvoiceCover_StateShare { get; set; }
    public int PackageInvoiceCover_Status { get; set; }
    public Decimal PackageInvoiceCover_TenderAmount { get; set; }
    public Decimal PackageInvoiceCover_ULBShare { get; set; }
    public Decimal PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill { get; set; }
    public Decimal PackageInvoiceCover_Total_Invoice_Value { get; set; }
    public Decimal PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion { get; set; }
    public Decimal PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency { get; set; }
    public Decimal PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement { get; set; }
    public Decimal PackageInvoiceCover_Amount_Released_To_Division { get; set; }
    public Decimal PackageInvoiceCover_Expenditure_By_Division { get; set; }
    public Decimal PackageInvoiceCover_Total_Payment_Earlier { get; set; }
    public Decimal PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam { get; set; }
    public Decimal PackageInvoiceCover_Total_Release { get; set; }
    public Decimal PackageInvoiceCover_Total_With_Held_Amount { get; set; }
    public Decimal PackageInvoiceCover_Total_Balance { get; set; }
}


public class tbl_PhysicalProgressComponent
{
    public int PhysicalProgressComponent_AddedBy { get; set; }
    public string PhysicalProgressComponent_AddedOn { get; set; }
    public int PhysicalProgressComponent_ModifiedBy { get; set; }
    public string PhysicalProgressComponent_ModifiedOn { get; set; }
    public string PhysicalProgressComponent_Component { get; set; }
    public int PhysicalProgressComponent_Unit_Id { get; set; }
    public int PhysicalProgressComponent_Id { get; set; }
    public int PhysicalProgressComponent_Status { get; set; }
}
public class tbl_SetInspectionMaster
{
    public int SetInspectionMaster_AddedBy { get; set; }
    public string SetInspectionMaster_AddedOn { get; set; }
    public int SetInspectionMaster_ModifiedBy { get; set; }
    public string SetInspectionMaster_ModifiedOn { get; set; }
    public string SetInspectionMaster_Name { get; set; }
    public int SetInspectionMaster_Designation_Id { get; set; }
    public int SetInspectionMaster_MLevel_Id { get; set; }
    public int SetInspectionMaster_Id { get; set; }
    public int SetInspectionMaster_Status { get; set; }
}
public class tbl_Deliverables
{
    public int Deliverables_AddedBy { get; set; }
    public string Deliverables_AddedOn { get; set; }
    public int Deliverables_ModifiedBy { get; set; }
    public string Deliverables_ModifiedOn { get; set; }
    public string Deliverables_Deliverables { get; set; }
    public int Deliverables_Unit_Id { get; set; }
    public int Deliverables_Id { get; set; }
    public int Deliverables_Status { get; set; }
}

public class tbl_DPRQuestionnaire
{
    public int DPRQuestionnaire_AddedBy { get; set; }
    public string DPRQuestionnaire_AddedOn { get; set; }
    public int DPRQuestionnaire_ModifiedBy { get; set; }
    public string DPRQuestionnaire_ModifiedOn { get; set; }
    public string DPRQuestionnaire_Name { get; set; }
    public int DPRQuestionnaire_Id { get; set; }
    public int DPRQuestionnaire_ProjectId { get; set; }
    public int DPRQuestionnaire_Status { get; set; }
}
public class tbl_DPR_Status
{
    public int DPR_Status_AddedBy { get; set; }
    public string DPR_Status_AddedOn { get; set; }
    public int DPR_Status_ModifiedBy { get; set; }
    public string DPR_Status_ModifiedOn { get; set; }
    public string DPR_Status_DPR_StatusName { get; set; }
    public int DPR_Status_Id { get; set; }
    public int DPR_Status_Status { get; set; }
}
public class tbl_ADP_Category
{
    public int ADP_Category_AddedBy { get; set; }
    public string ADP_Category_AddedOn { get; set; }
    public int ADP_Category_ModifiedBy { get; set; }
    public string ADP_Category_ModifiedOn { get; set; }
    public string ADP_Category_Name { get; set; }
    public int ADP_Category_Id { get; set; }
    public int ADP_Category_Status { get; set; }
}
public class tbl_ADP_Item
{
    public int ADP_Item_AddedBy { get; set; }
    public string ADP_Item_AddedOn { get; set; }
    public int ADP_Item_ModifiedBy { get; set; }
    public string ADP_Item_ModifiedOn { get; set; }
    public int ADP_Item_Category_Id { get; set; }
    public string ADP_Item_Specification { get; set; }
    public int ADP_Item_Unit_Id { get; set; }
    public decimal ADP_Item_Rate { get; set; }
    public decimal ADP_Item_Qty { get; set; }
    public int ADP_Item_Id { get; set; }
    public int ADP_Item_Status { get; set; }
}
public class tbl_PackageEMB_ADP_Item
{
    public int PackageEMB_ADP_Item_AddedBy { get; set; }
    public string PackageEMB_ADP_Item_AddedOn { get; set; }
    public int PackageEMB_ADP_Item_ModifiedBy { get; set; }
    public string PackageEMB_ADP_Item_ModifiedOn { get; set; }
    public int PackageEMB_ADP_Item_Category_Id { get; set; }
    public int PackageEMB_ADP_Item_Id { get; set; }
    public int PackageEMB_ADP_Item_PackageEMB_Master_Id { get; set; }
    public string PackageEMB_ADP_Item_Specification { get; set; }
    public int PackageEMB_ADP_Item_Unit_Id { get; set; }
    public decimal PackageEMB_ADP_Item_Rate { get; set; }
    public decimal PackageEMB_ADP_Item_Qty { get; set; }
    public int PackageEMB_ADP_Id { get; set; }
    public int PackageEMB_ADP_Item_Status { get; set; }
}
public class tbl_PackageInvoice_ADP_Item
{
    public int PackageInvoice_ADP_Item_AddedBy { get; set; }
    public string PackageInvoice_ADP_Item_AddedOn { get; set; }
    public int PackageInvoice_ADP_Item_ModifiedBy { get; set; }
    public string PackageInvoice_ADP_Item_ModifiedOn { get; set; }
    public int PackageInvoice_ADP_Item_Category_Id { get; set; }
    public int PackageInvoice_ADP_Item_Id { get; set; }
    public int PackageInvoice_ADP_Item_PackageInvoice_Id { get; set; }
    public string PackageInvoice_ADP_Item_Specification { get; set; }
    public int PackageInvoice_ADP_Item_Unit_Id { get; set; }
    public decimal PackageInvoice_ADP_Item_Rate { get; set; }
    public decimal PackageInvoice_ADP_Item_Qty { get; set; }
    public int PackageInvoice_ADP_Id { get; set; }
    public int PackageInvoice_ADP_Item_Status { get; set; }
}
public class tbl_ProjectQuestionnaire
{
    public int ProjectQuestionnaire_AddedBy { get; set; }
    public string ProjectQuestionnaire_AddedOn { get; set; }
    public int ProjectQuestionnaire_ModifiedBy { get; set; }
    public string ProjectQuestionnaire_ModifiedOn { get; set; }
    public string ProjectQuestionnaire_Name { get; set; }
    public int ProjectQuestionnaire_Id { get; set; }
    public int ProjectQuestionnaire_ProjectId { get; set; }
    public int ProjectQuestionnaire_Status { get; set; }
    public List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li { get; set; }
}
public class tbl_ProjectAnswer
{
    public int ProjectAnswer_AddedBy { get; set; }
    public string ProjectAnswer_AddedOn { get; set; }
    public int ProjectAnswer_ModifiedBy { get; set; }
    public string ProjectAnswer_ModifiedOn { get; set; }
    public string ProjectAnswer_Name { get; set; }
    public int ProjectAnswer_Id { get; set; }
    public int ProjectAnswer_ProjectQuestionnaireId { get; set; }
    public int ProjectAnswer_Status { get; set; }
}

public class tbl_ProjectUC
{
    public Decimal ProjectUC_Achivment { get; set; }
    public int ProjectUC_AddedBy { get; set; }
    public int ProjectUC_FinancialYear_Id { get; set; }
    public string ProjectUC_AddedOn { get; set; }
    public Decimal ProjectUC_BudgetUtilized { get; set; }
    public Decimal ProjectUC_Centage { get; set; }
    public Decimal ProjectUC_Total_Allocated { get; set; }
    public int ProjectUC_PhysicalProgress { get; set; }
    public string ProjectUC_Comments { get; set; }
    public int ProjectUC_Id { get; set; }
    public int ProjectUC_ModifiedBy { get; set; }
    public string ProjectUC_ModifiedOn { get; set; }
    public int ProjectUC_ProjectPkg_Id { get; set; }
    public int ProjectUC_ProjectWork_Id { get; set; }
    public int ProjectUC_Status { get; set; }
    public string ProjectUC_SubmitionDate { get; set; }
    public decimal ProjectUC_Latitude { get; set; }
    public decimal ProjectUC_Longitude { get; set; }
}
public class tbl_ProjectUC_PhysicalProgress
{
    public int ProjectUC_PhysicalProgress_AddedBy { get; set; }
    public string ProjectUC_PhysicalProgress_AddedOn { get; set; }
    public int ProjectUC_PhysicalProgress_ModifiedBy { get; set; }
    public string ProjectUC_PhysicalProgress_ModifiedOn { get; set; }
    public int ProjectUC_PhysicalProgress_ProjectWork_Id { get; set; }
    public int ProjectUC_PhysicalProgress_ProjectPkg_Id { get; set; }
    public int ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id { get; set; }
    public int ProjectUC_PhysicalProgress_FinancialYear_Id { get; set; }
    public int ProjectUC_PhysicalProgress_PhysicalProgress { get; set; }
    public int ProjectUC_PhysicalProgress_Id { get; set; }
    public int ProjectUC_PhysicalProgress_Status { get; set; }
}
public class tbl_ProjectUC_Deliverables
{
    public int ProjectUC_Deliverables_AddedBy { get; set; }
    public string ProjectUC_Deliverables_AddedOn { get; set; }
    public int ProjectUC_Deliverables_ModifiedBy { get; set; }
    public string ProjectUC_Deliverables_ModifiedOn { get; set; }
    public int ProjectUC_Deliverables_ProjectWork_Id { get; set; }
    public int ProjectUC_Deliverables_ProjectPkg_Id { get; set; }
    public int ProjectUC_Deliverables_Deliverables_Id { get; set; }
    public int ProjectUC_Deliverables_FinancialYear_Id { get; set; }
    public int ProjectUC_Deliverables_Deliverables { get; set; }
    public int ProjectUC_Deliverables_Id { get; set; }
    public int ProjectUC_Deliverables_Status { get; set; }
}
public class tbl_ProjectUC_Concent
{
    public int ProjectUC_Concent_AddedBy { get; set; }
    public string ProjectUC_Concent_AddedOn { get; set; }
    public string ProjectUC_Concent_Comments { get; set; }
    public int ProjectUC_Concent_Id { get; set; }
    public int ProjectUC_Concent_ModifiedBy { get; set; }
    public string ProjectUC_Concent_ModifiedOn { get; set; }
    public int ProjectUC_Concent_ProjectUC_Id { get; set; }
    public int ProjectUC_Concent_Questionire_Id { get; set; }
    public int ProjectUC_Concent_Answer_Id { get; set; }
    public string ProjectUC_Concent_Answer { get; set; }
    public int ProjectUC_Concent_Status { get; set; }
}
public class tbl_ProjectPkgSitePics
{
    public int ProjectPkgSitePics_AddedBy { get; set; }
    public string ProjectPkgSitePics_AddedOn { get; set; }
    public int ProjectPkgSitePics_Id { get; set; }
    public int ProjectPkgSitePics_ModifiedBy { get; set; }
    public string ProjectPkgSitePics_ModifiedOn { get; set; }
    public int ProjectPkgSitePics_ProjectPkg_Id { get; set; }
    public int ProjectPkgSitePics_ProjectWork_Id { get; set; }
    public string ProjectPkgSitePics_ReportSubmitted { get; set; }
    public int ProjectPkgSitePics_ReportSubmittedBy_PersonId { get; set; }
    public string ProjectPkgSitePics_SitePic_Path1 { get; set; }
    public string ProjectPkgSitePics_SitePic_Type { get; set; }
    public byte[] ProjectPkgSitePics_SitePic_Bytes1 { get; set; }
    public int ProjectPkgSitePics_Status { get; set; }
}
public class tbl_ProjectPkg_PhysicalProgress
{
    public int ProjectPkg_PhysicalProgress_AddedBy { get; set; }
    public string ProjectPkg_PhysicalProgress_AddedOn { get; set; }
    public int ProjectPkg_PhysicalProgress_ModifiedBy { get; set; }
    public string ProjectPkg_PhysicalProgress_ModifiedOn { get; set; }
    public int ProjectPkg_PhysicalProgress_PrjectWork_Id { get; set; }
    public int ProjectPkg_PhysicalProgress_ProjectPkg_Id { get; set; }
    public int ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id { get; set; }
    public int ProjectPkg_PhysicalProgress_Id { get; set; }
    public int ProjectPkg_PhysicalProgress_Status { get; set; }
}
public class tbl_ProjectPkg_Deliverables
{
    public int ProjectPkg_Deliverables_AddedBy { get; set; }
    public string ProjectPkg_Deliverables_AddedOn { get; set; }
    public int ProjectPkg_Deliverables_ModifiedBy { get; set; }
    public string ProjectPkg_Deliverables_ModifiedOn { get; set; }
    public int ProjectPkg_Deliverables_ProjectWork_Id { get; set; }
    public int ProjectPkg_Deliverables_ProjectPkg_Id { get; set; }
    public int ProjectPkg_Deliverables_Deliverables_Id { get; set; }
    public int ProjectPkg_Deliverables_Id { get; set; }
    public int ProjectPkg_Deliverables_Status { get; set; }
}

public class tbl_ProjectDPRStatus
{
    public int ProjectDPRStatus_AddedBy { get; set; }
    public string ProjectDPRStatus_AddedOn { get; set; }
    public string ProjectDPRStatus_Comments { get; set; }
    public string ProjectDPRStatus_Date { get; set; }
    public int ProjectDPRStatus_DPR_StatusId { get; set; }
    public int ProjectDPRStatus_Id { get; set; }
    public int ProjectDPRStatus_ModifiedBy { get; set; }
    public string ProjectDPRStatus_ModifiedOn { get; set; }
    public int ProjectDPRStatus_ProjectDPR_Id { get; set; }
    public int ProjectDPRStatus_Status { get; set; }
    public int ProjectDPR_Work_Id { get; set; }
    public int ProjectDPRStatus_ProjectWorkPkg_Id { get; set; }
}

public class tbl_ProjectDPRQuestionire
{
    public int ProjectDPRQuestionire_AddedBy { get; set; }
    public string ProjectDPRQuestionire_AddedOn { get; set; }
    public string ProjectDPRQuestionire_Answer { get; set; }
    public string ProjectDPRQuestionire_Remark { get; set; }
    public int ProjectDPRQuestionire_Id { get; set; }
    public int ProjectDPRQuestionire_ModifiedBy { get; set; }
    public string ProjectDPRQuestionire_ModifiedOn { get; set; }
    public int ProjectDPRQuestionire_DPR_Id { get; set; }
    public int ProjectDPRQuestionire_Questionire_Id { get; set; }
    public int ProjectDPRQuestionire_Status { get; set; }
    public int ProjectDPRQuestionire_Work_Id { get; set; }
    public int ProjectDPRQuestionire_ProjectWorkPkg_Id { get; set; }
}
public class tbl_ProjectDPR_PhysicalProgress
{
    public int ProjectDPR_PhysicalProgress_AddedBy { get; set; }
    public string ProjectDPR_PhysicalProgress_AddedOn { get; set; }
    public int ProjectDPR_PhysicalProgress_ModifiedBy { get; set; }
    public string ProjectDPR_PhysicalProgress_ModifiedOn { get; set; }
    public int ProjectDPR_PhysicalProgress_PrjectWork_Id { get; set; }
    public int ProjectDPR_PhysicalProgress_ProjectWorkPkg_Id { get; set; }
    public int ProjectDPR_PhysicalProgress_ProjectDPR_Id { get; set; }
    public int ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id { get; set; }
    public int ProjectDPR_PhysicalProgress_Id { get; set; }
    public int ProjectDPR_PhysicalProgress_Status { get; set; }
}
public class tbl_ProjectDPR_Deliverables
{
    public int ProjectDPR_Deliverables_AddedBy { get; set; }
    public string ProjectDPR_Deliverables_AddedOn { get; set; }
    public int ProjectDPR_Deliverables_ModifiedBy { get; set; }
    public string ProjectDPR_Deliverables_ModifiedOn { get; set; }
    public int ProjectDPR_Deliverables_ProjectWork_Id { get; set; }
    public int ProjectDPR_Deliverables_ProjectWorkPkg_Id { get; set; }
    public int ProjectDPR_Deliverables_ProjectDPR_Id { get; set; }
    public int ProjectDPR_Deliverables_Deliverables_Id { get; set; }
    public int ProjectDPR_Deliverables_Id { get; set; }
    public int ProjectDPR_Deliverables_Status { get; set; }
}

public class tbl_Package_ADP_Item
{
    public int Package_ADP_Item_AddedBy { get; set; }
    public string Package_ADP_Item_AddedOn { get; set; }
    public int Package_ADP_Item_ModifiedBy { get; set; }
    public string Package_ADP_Item_ModifiedOn { get; set; }
    public int Package_ADP_Item_Id { get; set; }
    public int Package_ADP_Item_Item_Id { get; set; }
    public int Package_ADP_Item_Package_Id { get; set; }
    public int Package_ADP_Item_Category_Id { get; set; }
    public string Package_ADP_Item_Specification { get; set; }
    public int Package_ADP_Item_Unit_Id { get; set; }
    public decimal Package_ADP_Item_Rate { get; set; }
    public decimal Package_ADP_Item_Qty { get; set; }
    public int Package_ADP_Item_Status { get; set; }
    public List<tbl_Package_ADP_Item_Tax> tbl_Package_ADP_Item_Tax { get; set; }
}
public class tbl_Package_ADP_Item_Tax
{
    public int Package_ADP_Item_Tax_AddedBy { get; set; }
    public string Package_ADP_Item_Tax_AddedOn { get; set; }
    public int Package_ADP_Item_Tax_ModifiedBy { get; set; }
    public string Package_ADP_Item_Tax_ModifiedOn { get; set; }
    public int Package_ADP_Item_Tax_Id { get; set; }
    public int Package_ADP_Item_Tax_Package_ADP_Item_Id { get; set; }
    public int Package_ADP_Item_Tax_Package_Id { get; set; }
    public int Package_ADP_Item_Tax_Deduction_Id { get; set; }
    public decimal Package_ADP_Item_Tax_Value { get; set; }
    public int Package_ADP_Item_Tax_Status { get; set; }
}