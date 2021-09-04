using System.Net;
using System.Net.Mime;
using ApiBase.Response;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
namespace ApiBase.Controllers
{
    public abstract class ODataControllerBase : ODataController
    {
        protected Response<TBody> ProduceResponse<TBody>(TBody body)
        {
            return Response<TBody>.Success(body);
        }

    }
}