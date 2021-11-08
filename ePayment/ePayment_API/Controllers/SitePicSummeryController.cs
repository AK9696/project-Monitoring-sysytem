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
    [RoutePrefix("api/SitePicSummery")]
    public class SitePicSummeryController : ApiController
    {
        public readonly string _connectionString;
        public readonly SitePicSummeryRepository _SitePicSummeryRepository;
        public SitePicSummeryController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _SitePicSummeryRepository = new SitePicSummeryRepository(_connectionString);
        }
        // GET: api/SitePicSummery
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/SitePicSummery/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/SitePicSummery
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectUC> obj_tbl_ProjectUC_Li = new List<tbl_ProjectUC>();

            obj_tbl_ProjectUC_Li = await _SitePicSummeryRepository.get_SitePic(obj_tbl_Person);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectUC_Li);
            });            
        }

        // PUT: api/SitePicSummery/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SitePicSummery/5
        public void Delete(int id)
        {
        }
    }
}
