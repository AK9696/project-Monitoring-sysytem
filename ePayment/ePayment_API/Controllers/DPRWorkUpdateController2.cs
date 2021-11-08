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
    [RoutePrefix("api/DPRWorkUpdate2")]
    public class DPRWorkUpdate2Controller : ApiController
    {
        public readonly string _connectionString;
        public readonly DPRWorkUpdate2Repository _DPRWorkUpdate2Repository;
        public DPRWorkUpdate2Controller()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DPRWorkUpdate2Repository = new DPRWorkUpdate2Repository(_connectionString);
        }
        // GET: api/DPRWorkUpdate2
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DPRWorkUpdate2/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DPRWorkUpdate2
        public async Task<HttpResponseMessage> Post([FromBody]DPRWorkUpdate2 obj_DPRWorkUpdate2)
        {
            bool retVal = await _DPRWorkUpdate2Repository.update_ProjectDPRWorkStatus(obj_DPRWorkUpdate2);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, retVal);
            });
        }

        // PUT: api/DPRWorkUpdate2/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DPRWorkUpdate2/5
        public void Delete(int id)
        {
        }
    }
}
