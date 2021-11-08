using ePayment_API.Models;
using ePayment_API.Repos;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/DeviceInfo")]
    public class DeviceInfoController : ApiController
    {
        public readonly string _connectionString;
        public readonly DeviceInfoRepository _DeviceInfoRepository;
        public DeviceInfoController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _DeviceInfoRepository = new DeviceInfoRepository(_connectionString);
        }
        // GET: api/DeviceInfo
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/DeviceInfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DeviceInfo
        public async Task<HttpResponseMessage> Post([FromBody] tbl_DeviceInfo obj_tbl_DeviceInfo)
        {

            tbl_DeviceInfoResponse obj_tbl_DeviceInfoResponse = await _DeviceInfoRepository.User_DeviceInfo(obj_tbl_DeviceInfo);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_DeviceInfoResponse);
            });
        }
        // PUT: api/DeviceInfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DeviceInfo/5
        public void Delete(int id)
        {
        }
    }
}
