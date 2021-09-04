using System.Net.Mime;
using ApiBase.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ApiControllerBase : ControllerBase
    {
        protected Response<TBody> ProduceResponse<TBody>(TBody body)
        {
            return Response<TBody>.Success(body);
        }
    }
}