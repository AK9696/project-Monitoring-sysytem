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
    [RoutePrefix("api/FinancialRelease_Breakup")]
    public class FinancialRelease_BreakupController : ApiController
    {
        public readonly string _connectionString;
        public readonly FinancialRelease_BreakupRepository _FinancialRelease_BreakupRepository;
        public FinancialRelease_BreakupController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _FinancialRelease_BreakupRepository = new FinancialRelease_BreakupRepository(_connectionString);
        }
        // GET: api/FinancialRelease_Breakup
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/FinancialRelease_Breakup/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // POST: api/FinancialRelease_Breakup
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            List<tbl_FinancialTrans> obj_tbl_FinancialTrans_Li = new List<tbl_FinancialTrans>();

            obj_tbl_FinancialTrans_Li = await _FinancialRelease_BreakupRepository.get_FinancialRelease_Breakup(obj_tbl_Person);

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_FinancialTrans_Li);
            });            
        }

        // PUT: api/FinancialRelease_Breakup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FinancialRelease_Breakup/5
        public void Delete(int id)
        {
        }
    }
}
