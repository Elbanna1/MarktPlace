using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Constants;
using Shared.DTOs.Common;
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

            if (argument is PaginationParams { IsPageIndexOutOfRange: true })
            {
                context.Result = Unprocessable([UserMessages.Errors.PageIndexOutOfRange]);
                return;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext);

            if (result.IsValid)
                continue;

            context.Result = Unprocessable(result.Errors.Select(e => e.ErrorMessage).ToList());
            return;
        }

        await next();
    }

    private static ObjectResult Unprocessable(List<string> errors) =>
        new(ApiResponse.Fail("فيه بيانات ناقصة أو مش صحيحة.", errors))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity
        };
}
