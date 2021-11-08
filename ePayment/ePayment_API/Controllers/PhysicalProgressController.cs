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
    [RoutePrefix("api/PhysicalProgress")]
    public class PhysicalProgressController : ApiController
    {
        public readonly string _connectionString;
        public readonly PhysicalProgressRepository _PhysicalProgressRepository;
        public PhysicalProgressController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _PhysicalProgressRepository = new PhysicalProgressRepository(_connectionString);
        }
        // GET: api/PhysicalProgress
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PhysicalProgress/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PhysicalProgress
        public async Task<HttpResponseMessage> Post([FromBody]tbl_Person obj_Person)
        {
            List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress_Li = await _PhysicalProgressRepository.get_ProjectPkg_PhysicalProgress(obj_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectPkg_PhysicalProgress_Li);
            });
        }

        // PUT: api/PhysicalProgress/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PhysicalProgress/5
        public void Delete(int id)
        {
        }
    }
}
