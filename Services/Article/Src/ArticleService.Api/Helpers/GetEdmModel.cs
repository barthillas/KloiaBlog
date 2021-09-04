using ArticleService.Domain.Entities;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace ArticleService.Api.Helpers
{
    public static class Edm
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Article>("Articles");
            return builder.GetEdmModel();
        }
    }
}