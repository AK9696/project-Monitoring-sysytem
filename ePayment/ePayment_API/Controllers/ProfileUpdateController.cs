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
    [RoutePrefix("api/ProfileUpdate")]
    public class ProfileUpdateController : ApiController
    {
        public readonly string _connectionString;
        public readonly ProfileUpdateRepository _ProfileUpdateRepository;
        public ProfileUpdateController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _ProfileUpdateRepository = new ProfileUpdateRepository(_connectionString);
        }
        // GET: api/ProfileUpdate
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/ProfileUpdate
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Profile obj_tbl_Profile)
        {
            bool retVal = await _ProfileUpdateRepository.update_Profile(obj_tbl_Profile);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, retVal);
            });
        }

        // PUT: api/ProfileUpdate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ProfileUpdate/5
        public void Delete(int id)
        {
        }
    }
}
