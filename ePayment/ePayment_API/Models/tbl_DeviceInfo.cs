
namespace ePayment_API.Models
{
    public class tbl_DeviceInfo
    {
        public int Person_Id { get; set; }
        public int DeviceInfo_Id { get; set; }
        public string RELEASE { get; set; }
        public string DEVICE { get; set; }
        public string MODEL { get; set; }
        public string PRODUCT { get; set; }
        public string BRAND { get; set; }
        public string DISPLAY { get; set; }
        public string CPU_ABI { get; set; }
        public string CPU_ABI2 { get; set; }
        public string UNKNOWN { get; set; }
        public string HARDWARE { get; set; }
        public string ID { get; set; }
        public string MANUFACTURER { get; set; }
        public string SERIAL { get; set; }
        public string USER { get; set; }
        public string HOST { get; set; }
        public string DEVICE_ID { get; set; }
        public string Mobile_No { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string App_Version { get; set; }
    }

    public class tbl_DeviceInfoResponse
    {
        public int Person_Id { get; set; }
        public int Prompt_Update_Version { get; set; }
        public string Available_Version { get; set; }
    }

}
