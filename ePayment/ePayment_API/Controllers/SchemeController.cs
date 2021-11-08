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
    [RoutePrefix("api/Scheme")]
    public class SchemeController : ApiController
    {
        public readonly string _connectionString;
        public readonly SchemeRepository _SchemeRepository;
        public SchemeController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _SchemeRepository = new SchemeRepository(_connectionString);
        }
        // GET: api/Scheme
        public async Task<HttpResponseMessage> Get()
        {
            List<tbl_Project> obj_tbl_Project_Li = await _SchemeRepository.get_Scheme();
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Project_Li);
            });
        }

        // GET: api/Scheme/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Scheme
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Scheme/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Scheme/5
        public void Delete(int id)
        {
        }
    }
}
