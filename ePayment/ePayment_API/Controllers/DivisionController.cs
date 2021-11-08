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
    [RoutePrefix("api/Division")]
    public class DivisionController : ApiController
    {
        public readonly string _connectionString;
        public readonly DivisionRepository _DivisionRepository;
        public DivisionController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DivisionRepository = new DivisionRepository(_connectionString);
        }
        // GET: api/Division
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/Division/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            List<tbl_Division> obj_tbl_Division_Li = new List<tbl_Division>();

            obj_tbl_Division_Li = await _DivisionRepository.get_Division(id);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Division_Li);
            });
        }

        // POST: api/Division
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Division/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Division/5
        public void Delete(int id)
        {
        }
    }
}
