using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace QueryParser.Web.ModelBinders
{
    public class FilteredRequestModelBinder : IModelBinder
    {
        public Task BindModelAsync( ModelBindingContext bindingContext )
        {
            var model = bindingContext.Model as IFilterable;

            if ( model == null )
                return Task.CompletedTask;

            var query = bindingContext.HttpContext.Request.Query;
            model.SetQueryParams( query );

            bindingContext.Result = ModelBindingResult.Success( model );
            return Task.CompletedTask;
        }
    }
}
