
namespace ePayment_API.Models
{
    public class tbl_Person
    {
        public int Person_Id { get; set; }
        public string Person_Mobile { get; set; }
        public string Person_Email { get; set; }
        public string response { get; set; }
        public string OTP { get; set; }
        public int ULB_Id { get; set; }
        public int District_Id { get; set; }
        public int Role_ULB { get; set; }
        public int Role_Inspection { get; set; }
        public int Role_Vendor { get; set; }
        public int FinancialYear_Id { get; set; }
        public int ProjectDPR_Id { get; set; }
        public int ProjectWork_Id { get; set; }
        public int ProjectUC_Id { get; set; }
        public string Project_Status { get; set; }
        public int Project_Id { get; set; }
        public string Base_URL { get; set; }
        public int Zone_Id { get; set; }
        public int Circle_Id { get; set; }
        public int Division_Id { get; set; }
    }
}
