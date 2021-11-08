using ePayment_API.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ePayment_API.Models;
using System.IO;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/DPRWork")]
    public class DPRWorkController : ApiController
    {
        public readonly string _connectionString;
        public readonly DPRWorkRepository _DPRWorkRepository;
        public DPRWorkController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DPRWorkRepository = new DPRWorkRepository(_connectionString);
        }
        // GET: api/DPRWork
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DPRWork/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DPRWork
        public async Task<HttpResponseMessage> Post([FromBody]tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR> obj_tbl_ProjectDPR_Li = await _DPRWorkRepository.get_ProjectDPR(obj_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectDPR_Li);
            });
        }

        // PUT: api/DPRWork/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DPRWork/5
        public void Delete(int id)
        {
        }
    }
}
