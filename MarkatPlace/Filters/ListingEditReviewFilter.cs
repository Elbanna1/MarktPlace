using Microsoft.AspNetCore.Mvc.Filters;
using ServicesAbstraction;

namespace MarkatPlace.Filters;

public class ListingEditReviewFilter : IAsyncActionFilter
{
    private readonly ILogger<ListingEditReviewFilter> _logger;

    public ListingEditReviewFilter(ILogger<ListingEditReviewFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();

        if (executed.Exception is not null && !executed.ExceptionHandled)
            return;

        try
        {
            var notifier = context.HttpContext.RequestServices
                .GetRequiredService<IListingEditReviewNotifier>();

            await notifier.AnnounceAsync(context.HttpContext.RequestAborted);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Announcing the advertisements returned to the review queue by this edit failed.");
        }
    }
}
