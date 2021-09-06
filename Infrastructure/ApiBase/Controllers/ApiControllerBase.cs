using System.Net.Mime;
using ApiBase.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        protected Response<TBody> ProduceResponse<TBody>(TBody body)
        {
            return Response<TBody>.Success(body);
        }
    }
}