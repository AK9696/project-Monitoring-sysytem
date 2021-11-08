using ePayment_API.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ePayment_API.Models;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/DPRQuestionnaire")]
    public class DPRQuestionnaireController : ApiController
    {
        public readonly string _connectionString;
        public readonly DPRQuestionnaireRepository _DPRQuestionnaireRepository;
        public DPRQuestionnaireController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DPRQuestionnaireRepository = new DPRQuestionnaireRepository(_connectionString);
        }
        // GET: api/DPRQuestionnaire
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/DPRQuestionnaire/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            List<tbl_DPRQuestionnaire> obj_tbl_DPRQuestionnaire_Li = new List<tbl_DPRQuestionnaire>();

            obj_tbl_DPRQuestionnaire_Li = await _DPRQuestionnaireRepository.get_DPRQuestionnaire(id);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_DPRQuestionnaire_Li);
            });
        }

        // POST: api/DPRQuestionnaire
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });            
        }

        // PUT: api/DPRQuestionnaire/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DPRQuestionnaire/5
        public void Delete(int id)
        {
        }
    }
}
