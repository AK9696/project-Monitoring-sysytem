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
    [RoutePrefix("api/QuestionireAnswer")]
    public class QuestionireAnswerController : ApiController
    {
        public readonly string _connectionString;
        public readonly QuestionireAnswerRepository _QuestionireAnswerRepository;
        public QuestionireAnswerController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _QuestionireAnswerRepository = new QuestionireAnswerRepository(_connectionString);
        }
        // GET: api/QuestionireAnswer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/QuestionireAnswer/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            List<tbl_ProjectAnswer> obj_tbl_ProjectAnswer_Li = await _QuestionireAnswerRepository.get_QuestionireAnswer(id);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectAnswer_Li);
            });            
        }

        // POST: api/QuestionireAnswer
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/QuestionireAnswer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/QuestionireAnswer/5
        public void Delete(int id)
        {
        }
    }
}
