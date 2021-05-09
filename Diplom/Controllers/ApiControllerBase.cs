using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Diplom.Controllers
{
    public class ApiControllerBase : ApiController
    {
        protected HttpResponseMessage FileContent(string filename, byte[] buffer)
        {
            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);

            httpResponseMessage.Content = new ByteArrayContent(buffer);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filename));
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            return httpResponseMessage;
        }
    }
}