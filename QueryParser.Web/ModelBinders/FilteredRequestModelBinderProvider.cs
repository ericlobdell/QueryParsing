using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using QueryParser.Web.Requests;

namespace QueryParser.Web.ModelBinders
{
    public class FilteredRequestModelBinderProvider: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if ( typeof(IFilterableRequest).IsAssignableFrom(context.Metadata.ModelType) )
                return new BinderTypeModelBinder(typeof(FilteredRequestModelBinder));

            return null;
        }
    }
}
