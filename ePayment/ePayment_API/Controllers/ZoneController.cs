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
    [RoutePrefix("api/Zone")]
    public class ZoneController : ApiController
    {
        public readonly string _connectionString;
        public readonly ZoneRepository _ZoneRepository;
        public ZoneController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _ZoneRepository = new ZoneRepository(_connectionString);
        }
        // GET: api/Zone
        public async Task<HttpResponseMessage> Get()
        {
            List<tbl_Zone> obj_tbl_Zone_Li = await _ZoneRepository.get_Zone();
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Zone_Li);
            });
        }

        // GET: api/Zone/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Zone
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Zone/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Zone/5
        public void Delete(int id)
        {
        }
    }
}
