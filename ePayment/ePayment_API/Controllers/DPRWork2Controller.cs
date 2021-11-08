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
    [RoutePrefix("api/DPRWork2")]
    public class DPRWork2Controller : ApiController
    {
        public readonly string _connectionString;
        public readonly DPRWork2Repository _DPRWork2Repository;
        public DPRWork2Controller()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DPRWork2Repository = new DPRWork2Repository(_connectionString);
        }
        // GET: api/DPRWork2
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DPRWork2/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DPRWork2
        public async Task<HttpResponseMessage> Post([FromBody]tbl_Person obj_Person)
        {
            List<tbl_ProjectDPR_JalNigam> obj_tbl_ProjectDPR_JalNigam_Li = await _DPRWork2Repository.get_ProjectDPR(obj_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectDPR_JalNigam_Li);
            });
        }

        // PUT: api/DPRWork2/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DPRWork2/5
        public void Delete(int id)
        {
        }
    }
}
