
namespace ePayment_API.Models
{
    public class tbl_ProjectPkg_PhysicalProgress
    {
        public int PhysicalProgressComponent_Id { get; set; }
        public string PhysicalProgressComponent_Component { get; set; }
        public string Unit_Name { get; set; }
        public int PhysicalProgress_Total { get; set; }
        public int PhysicalProgress_Current { get; set; }
    }
}
