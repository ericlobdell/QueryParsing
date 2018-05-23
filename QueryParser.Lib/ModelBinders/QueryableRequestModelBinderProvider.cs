using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace QueryableRequests.ModelBinders
{
    public class QueryableRequestModelBinderProvider: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if ( typeof(IQueryableRequest).IsAssignableFrom(context.Metadata.ModelType) )
                return new BinderTypeModelBinder(typeof(QueryableRequestModelBinder));

            return null;
        }
    }
}
