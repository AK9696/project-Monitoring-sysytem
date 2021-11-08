
namespace ePayment_API.Models
{
    public class tbl_FinancialTrans
    {
        public int FinancialTrans_AddedBy { get; set; }
        public string FinancialTrans_AddedOn { get; set; }
        public int FinancialTrans_Id { get; set; }
        public int FinancialTrans_Jurisdiction_Id { get; set; }
        public int FinancialTrans_Status { get; set; }
        public string FinancialTrans_Comments { get; set; }
        public decimal FinancialTrans_TransAmount { get; set; }
        public string FinancialTrans_EntryType { get; set; }
        public string FinancialTrans_TransType { get; set; }
        public int FinancialTrans_SchemeId { get; set; }
        public int FinancialTrans_WorkId { get; set; }
        public string FinancialTrans_Date { get; set; }
        public int FinancialTrans_FinancialYear_Id { get; set; }
        public int FinancialTrans_ProjectDPR_Id { get; set; }
        public string FinancialTrans_FilePath1 { get; set; }
        public string FinancialTrans_GO_Date { get; set; }
        public string FinancialTrans_GO_Number { get; set; }
        public decimal FinancialTrans_ReleaseAmount { get; set; }
        public decimal FinancialTrans_ExpenditureAmount { get; set; }
    }
}
