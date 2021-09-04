using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using ReviewService.Domain.Entities;

namespace ReviewService.Api.Helpers
{
    public static class EdmReview
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Review>("Reviews");
            return builder.GetEdmModel();
        }
    }
}