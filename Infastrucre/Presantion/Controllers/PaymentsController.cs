using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Payments;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
[Produces("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private bool IsAdmin => User.IsInRole(AppRoles.Admin);

    [HttpGet("methods")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentMethodDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentMethodDto>>>> GetMethods()
    {
        var result = await _paymentService.GetMethodsAsync(HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<PaymentMethodDto>>.Ok(result, UserMessages.Payments.MethodsLoaded));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ImageConstants.MaxRequestBodySizeBytes)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDetailsDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<PaymentDetailsDto>>> Submit([FromForm] SubmitPaymentRequest request)
    {
        var screenshot = await request.Screenshot.ToUploadModelAsync(HttpContext.RequestAborted)
            ?? await Request.FormFilesOrEmpty().FirstOrDefault().ToUploadModelAsync(HttpContext.RequestAborted);

        var result = await _paymentService.SubmitAsync(
            CurrentUserId, request, screenshot, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<PaymentDetailsDto>.Ok(result, UserMessages.Payments.Submitted));
    }

    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentListItemDto>>>> GetMyPayments(
        [FromQuery] PaymentStatus? status)
    {
        var result = await _paymentService.GetMyPaymentsAsync(CurrentUserId, status, HttpContext.RequestAborted);
        return Ok(ApiResponse<IReadOnlyList<PaymentListItemDto>>.Ok(result, UserMessages.Payments.Loaded));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaymentDetailsDto>>> GetById(Guid id)
    {
        var result = await _paymentService.GetByIdAsync(id, CurrentUserId, IsAdmin, HttpContext.RequestAborted);
        return Ok(ApiResponse<PaymentDetailsDto>.Ok(result, UserMessages.Payments.DetailsLoaded));
    }
}
