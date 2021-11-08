
using System.Collections.Generic;

namespace ePayment_API.Models
{
    public class tbl_ProjectQuestionnaire
    {
        public int ProjectQuestionnaire_AddedBy { get; set; }
        public string ProjectQuestionnaire_AddedOn { get; set; }
        public int ProjectQuestionnaire_ModifiedBy { get; set; }
        public string ProjectQuestionnaire_ModifiedOn { get; set; }
        public string ProjectQuestionnaire_Name { get; set; }
        public string ProjectQuestionnaire_Type { get; set; }
        public int ProjectQuestionnaire_Id { get; set; }
        public int ProjectQuestionnaire_ProjectId { get; set; }
        public int ProjectQuestionnaire_Status { get; set; }
        public List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li { get; set; }
    }
}
