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
    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController
    {
        public readonly string _connectionString;
        public readonly ProfileRepository _ProfileRepository;
        public ProfileController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _ProfileRepository = new ProfileRepository(_connectionString);
        }

        // GET: api/Profile/1
        public async Task<HttpResponseMessage> Get(int id)
        {
            tbl_Profile obj_tbl_Profile = await _ProfileRepository.get_Profile(id);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Profile);
            });
        }
               
        // PUT: api/Profile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Profile/5
        public void Delete(int id)
        {
        }
    }
}
