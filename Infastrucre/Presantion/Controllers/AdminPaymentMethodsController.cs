using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Payments;
using Shared.Responses;
using Shared.Authorization;
using Shared.Enums;

namespace Presentation.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/admin/payment-methods")]
[Authorize(Roles = AppRoles.Admin)]
[Produces("application/json")]
[AdminPage(AdminPageCatalog.Keys.Payments)]
public class AdminPaymentMethodsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public AdminPaymentMethodsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentMethodDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentMethodDto>>>> GetAll()
    {
        var result = await _paymentService.GetAllMethodsAsync(HttpContext.RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PaymentMethodDto>>.Ok(result, "تم استرجاع طرق الدفع."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> GetById(int id)
    {
        var result = await _paymentService.GetMethodByIdAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaymentMethodDto>.Ok(result, "تم استرجاع طريقة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> Create(
        [FromBody] CreatePaymentMethodRequest request)
    {
        var result = await _paymentService.CreateMethodAsync(request, HttpContext.RequestAborted);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<PaymentMethodDto>.Ok(result, "تم إضافة طريقة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Edit)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> Update(
        int id, [FromBody] UpdatePaymentMethodRequest request)
    {
        var result = await _paymentService.UpdateMethodAsync(id, request, HttpContext.RequestAborted);

        return Ok(ApiResponse<PaymentMethodDto>.Ok(result, "تم تحديث طريقة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        await _paymentService.DeleteMethodAsync(id, HttpContext.RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف طريقة الدفع."));
    }
}
