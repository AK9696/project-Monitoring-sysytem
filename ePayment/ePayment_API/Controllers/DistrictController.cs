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
    [RoutePrefix("api/District")]
    public class DistrictController : ApiController
    {
        public readonly string _connectionString;
        public readonly LocalityRepository _LocalityRepository;
        public DistrictController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _LocalityRepository = new LocalityRepository(_connectionString);
        }
        // GET: api/District
        public async Task<HttpResponseMessage> Get()
        {
            List<M_Jurisdiction> obj_M_Jurisdiction_Li = new List<M_Jurisdiction>();

            obj_M_Jurisdiction_Li = await _LocalityRepository.get_Locality(3, 0);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_M_Jurisdiction_Li);
            });
        }

        // GET: api/District/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/District
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/District/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/District/5
        public void Delete(int id)
        {
        }
    }
}
