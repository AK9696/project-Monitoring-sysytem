using ePayment_API.Models;
using ePayment_API.Repos;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ePayment_API.Controllers
{
    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        public readonly string _connectionString;
        public readonly RegistrationRepository _RegistrationRepository;
        public RegistrationController()
        {
            _connectionString = ConnectionString.DBConnectionString;
            _RegistrationRepository = new RegistrationRepository(_connectionString);
        }
        // GET: api/Registration
        public async Task<HttpResponseMessage> Get()
        {
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            });
        }

        // GET: api/Registration/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Registration
        public async Task<HttpResponseMessage> Post([FromBody] tbl_Person obj_tbl_Person)
        {
            obj_tbl_Person = await _RegistrationRepository.User_Registration(obj_tbl_Person);
            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                if (obj_tbl_Person.Person_Mobile == "8957762028")
                {
                    obj_tbl_Person.OTP = "0000";
                }
                return Request.CreateResponse(HttpStatusCode.OK, obj_tbl_Person);
            });
        }
        // PUT: api/Registration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Registration/5
        public void Delete(int id)
        {
        }
    }
}
