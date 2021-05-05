using System.Threading.Tasks;
using System.Web.Http;

namespace Diplom.Controllers
{
    [RoutePrefix("api/test")]
    public class testController : ApiController
    {
        [HttpGet, Route("shit")]
        public async Task<string> GetTest()
        {
            return "shit";
        }
    }
}