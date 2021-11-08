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
    [RoutePrefix("api/UCDetails")]
    public class UCDetailsController : ApiController
    {
        public readonly string _connectionString;
        public readonly UCDetailsRepository _UCDetailsRepository;
        public UCDetailsController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _UCDetailsRepository = new UCDetailsRepository(_connectionString);
        }
        // GET: api/UCDetails
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/UCDetails/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/UCDetails
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectUC> obj_tbl_ProjectUC_Li = new List<tbl_ProjectUC>();

            obj_tbl_ProjectUC_Li = await _UCDetailsRepository.get_UCDetails(obj_tbl_Person);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectUC_Li);
            });            
        }

        // PUT: api/UCDetails/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UCDetails/5
        public void Delete(int id)
        {
        }
    }
}
