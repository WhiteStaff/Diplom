using System.Threading.Tasks;
using System.Web.Http;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpPost, Route("create")]
        public async Task<string> Create()
        {
            return "123";
        }
    }
}