﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace QueryableRequests.ModelBinders
{
    public class QueryableRequestModelBinder: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = Activator.CreateInstance(bindingContext.ModelMetadata.ModelType) as IQueryableRequest;

            if ( model == null )
                return Task.CompletedTask;

            var query = bindingContext.HttpContext.Request.Query;
            model.SetQueryParams(query);

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
