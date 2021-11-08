using ePayment_API.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ePayment_API.Models;
using System.IO;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/ProjectWiseReport")]
    public class ProjectWiseReportController : ApiController
    {
        public readonly string _connectionString;
        public readonly ProjectWiseReportRepository _ProjectWiseReportRepository;
        public ProjectWiseReportController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _ProjectWiseReportRepository = new ProjectWiseReportRepository(_connectionString);
        }
        // GET: api/ProjectWiseReport
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/ProjectWiseReport
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_ProjectDPR_JalNigam> obj_tbl_ProjectDPR_JalNigam_Li = await _ProjectWiseReportRepository.get_ProjectDPR(obj_tbl_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_ProjectDPR_JalNigam_Li);
            });
        }

        // PUT: api/ProjectWiseReport/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/ProjectWiseReport/5
        public void Delete(int id)
        {
        }
    }
}
