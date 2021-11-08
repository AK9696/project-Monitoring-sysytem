using ePayment_API.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ePayment_API.Models;
using System.Web.ModelBinding;
using System.IO;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/SchemeWiseReport")]
    public class SchemeWiseReportController : ApiController
    {
        public readonly string _connectionString;
        public readonly SchemeWiseReportRepository _SchemeWiseReportRepository;
        public SchemeWiseReportController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _SchemeWiseReportRepository = new SchemeWiseReportRepository(_connectionString);
        }
        // GET: api/SchemeWiseReport
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/SchemeWiseReport
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_Scheme_Wise_Report> obj_tbl_Scheme_Wise_Report_Li = await _SchemeWiseReportRepository.get_Scheme_Wise_Report(obj_tbl_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Scheme_Wise_Report_Li);
            });
        }

        // PUT: api/SchemeWiseReport/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SchemeWiseReport/5
        public void Delete(int id)
        {
        }
    }
}
