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
    [RoutePrefix("api/Questionire")]
    public class QuestionireController : ApiController
    {
        public readonly string _connectionString;
        public readonly QuestionireRepository _QuestionireRepository;
        public QuestionireController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _QuestionireRepository = new QuestionireRepository(_connectionString);
        }
        // GET: api/Questionire
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Questionire/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            List<tbl_ProjectQuestionnaire> obj_tbl_ProjectQuestionnaire_Li = await _QuestionireRepository.get_Questionire(id);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectQuestionnaire_Li);
            });            
        }

        // POST: api/Questionire
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Questionire/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Questionire/5
        public void Delete(int id)
        {
        }
    }
}
