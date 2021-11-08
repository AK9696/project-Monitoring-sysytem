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
    [RoutePrefix("api/ComiteeMember")]
    public class ComiteeMemberController : ApiController
    {
        public readonly string _connectionString;
        public readonly ComiteeMemberRepository _ComiteeMemberRepository;
        public ComiteeMemberController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _ComiteeMemberRepository = new ComiteeMemberRepository(_connectionString);
        }
        // GET: api/ComiteeMember
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/ComiteeMember/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/ComiteeMember
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_PersonDetail> obj_tbl_PersonDetail_Li = new List<tbl_PersonDetail>();

            obj_tbl_PersonDetail_Li = await _ComiteeMemberRepository.get_ComiteeMember(obj_tbl_Person);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_PersonDetail_Li);
            });            
        }

        // PUT: api/ComiteeMember/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ComiteeMember/5
        public void Delete(int id)
        {
        }
    }
}
