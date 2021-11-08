
namespace ePayment_API.Models
{
    public class tbl_Dashboard
    {
        public int Total_Runing_Projects { get; set; }
        public int Total_Inspection { get; set; }
        public int Total_Executing_Agency { get; set; }
        public decimal Total_Budget { get; set; }
        public decimal Total_Funds_Released { get; set; }
        public decimal Total_Funds_Expenditure { get; set; }
        public decimal Physical_Progress { get; set; }
        public decimal Financial_Progress { get; set; }
    }
}
