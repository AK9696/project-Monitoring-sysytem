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
    [RoutePrefix("api/TicketCategory")]
    public class TicketCategoryController : ApiController
    {
        public readonly string _connectionString;
        public readonly TicketCategoryRepository _TicketCategoryRepository;
        public TicketCategoryController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _TicketCategoryRepository = new TicketCategoryRepository(_connectionString);
        }
        // GET: api/TicketCategory
        public async Task<HttpResponseMessage> Get()
        {
            List<tbl_TicketCategory> obj_tbl_TicketCategory_Li = new List<tbl_TicketCategory>();

            obj_tbl_TicketCategory_Li = await _TicketCategoryRepository.get_TicketCategory();

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_TicketCategory_Li);
            });
        }

        // GET: api/TicketCategory/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/TicketCategory
        public async Task<HttpResponseMessage> Post()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/TicketCategory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TicketCategory/5
        public void Delete(int id)
        {
        }
    }
}
