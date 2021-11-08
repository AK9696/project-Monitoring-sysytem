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
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        public readonly string _connectionString;
        public readonly DashboardRepository _DashboardRepository;
        public DashboardController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DashboardRepository = new DashboardRepository(_connectionString);
        }
        // GET: api/Dashboard
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Dashboard/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            tbl_Dashboard obj_tbl_Dashboard = await _DashboardRepository.get_Dashboard_Data(id);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Dashboard);
            });
        }

        // POST: api/Dashboard
        public async Task<HttpResponseMessage> Post([FromBody]tbl_Person obj_Person)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // PUT: api/Dashboard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dashboard/5
        public void Delete(int id)
        {
        }
    }
}
