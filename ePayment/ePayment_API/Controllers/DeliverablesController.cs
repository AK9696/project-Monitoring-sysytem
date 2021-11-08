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
    [RoutePrefix("api/Deliverables")]
    public class DeliverablesController : ApiController
    {
        public readonly string _connectionString;
        public readonly DeliverablesRepository _DeliverablesRepository;
        public DeliverablesController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DeliverablesRepository = new DeliverablesRepository(_connectionString);
        }
        // GET: api/Deliverables
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Deliverables/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Deliverables
        public async Task<HttpResponseMessage> Post([FromBody]tbl_Person obj_Person)
        {
            List<tbl_ProjectPkg_Deliverables> obj_tbl_ProjectPkg_Deliverables_Li = await _DeliverablesRepository.get_ProjectPkg_Deliverables(obj_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectPkg_Deliverables_Li);
            });
        }

        // PUT: api/Deliverables/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Deliverables/5
        public void Delete(int id)
        {
        }
    }
}
