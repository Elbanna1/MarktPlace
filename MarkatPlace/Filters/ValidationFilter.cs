using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Responses;

namespace MarkatPlace.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            context.ActionArguments.TryGetValue(parameter.Name, out var argument);

            if (argument is null)
            {
                if (parameter.BindingInfo?.BindingSource != BindingSource.Body)
                    continue;

                if (parameter.BindingInfo?.EmptyBodyBehavior == EmptyBodyBehavior.Allow)
                    continue;

                context.Result = new ObjectResult(
                    ApiResponse.Fail("البيانات المرسلة ناقصة أو مش صحيحة."))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext);

            if (result.IsValid)
                continue;

            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            var response = ApiResponse.Fail("فيه بيانات ناقصة أو مش صحيحة.", errors);

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
            return;
        }

        await next();
    }
}
