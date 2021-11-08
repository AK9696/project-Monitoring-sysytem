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
    [RoutePrefix("api/SitePic")]
    public class SitePicController : ApiController
    {
        public readonly string _connectionString;
        public readonly SitePicRepository _SitePicRepository;
        public SitePicController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _SitePicRepository = new SitePicRepository(_connectionString);
        }
        // GET: api/SitePic
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/SitePic/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/SitePic
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectDPR_JalNigamSitePics> obj_tbl_ProjectDPR_JalNigamSitePics_Li = new List<tbl_ProjectDPR_JalNigamSitePics>();

            obj_tbl_ProjectDPR_JalNigamSitePics_Li = await _SitePicRepository.get_SitePic(obj_tbl_Person);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectDPR_JalNigamSitePics_Li);
            });            
        }

        // PUT: api/SitePic/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SitePic/5
        public void Delete(int id)
        {
        }
    }
}
