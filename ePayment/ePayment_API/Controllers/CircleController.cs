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
    [RoutePrefix("api/Circle")]
    public class CircleController : ApiController
    {
        public readonly string _connectionString;
        public readonly CircleRepository _CircleRepository;
        public CircleController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _CircleRepository = new CircleRepository(_connectionString);
        }
        // GET: api/Circle
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/Circle/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            List<tbl_Circle> obj_tbl_Circle_Li = new List<tbl_Circle>();

            obj_tbl_Circle_Li = await _CircleRepository.get_Circle(id);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Circle_Li);
            });
        }

        // POST: api/Circle
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Circle/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Circle/5
        public void Delete(int id)
        {
        }
    }
}
