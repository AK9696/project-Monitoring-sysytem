
namespace ePayment_API.Models
{
    public class M_Jurisdiction
    {
        public int M_Jurisdiction_Id { get; set; }
        public int M_Level_Id { get; set; }
        public string Jurisdiction_Name { get; set; }
        public int Parent_Jurisdiction_Id { get; set; }
        public string Jurisdiction_Code { get; set; }
        public int Created_By { get; set; }
        public string Created_Date { get; set; }
        public string Parent_Jurisdiction_Name { get; set; }
        public int Is_Active { get; set; }
    }
}
