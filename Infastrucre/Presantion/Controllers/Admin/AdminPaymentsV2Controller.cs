using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Payments;
using Shared.Enums;
using Shared.Responses;
using Shared.Authorization;

namespace Presentation.Controllers.Admin;

[Route(ApiVersions.AdminRoutePrefix + "/payments")]
[Tags("Admin Payments")]
[AdminPage(AdminPageCatalog.Keys.Payments)]
public class AdminPaymentsController : AdminApiController
{
    private readonly IPaymentService _payments;

    public AdminPaymentsController(IPaymentService payments)
    {
        _payments = payments;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<AdminPaymentListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<AdminPaymentListItemDto>>>> GetAll(
        [FromQuery] AdminPaymentFilterParams filter)
    {
        var result = await _payments.GetAllAsync(filter, RequestAborted);

        return Ok(ApiResponse<PaginatedResult<AdminPaymentListItemDto>>.Ok(result, "تم استرجاع المدفوعات."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminPaymentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminPaymentDetailsDto>>> GetById(Guid id)
    {
        var result = await _payments.GetForAdminAsync(id, RequestAborted);

        return Ok(ApiResponse<AdminPaymentDetailsDto>.Ok(result, "تم استرجاع عملية الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Approve)]
    [HttpPatch("{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<AdminPaymentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminPaymentDetailsDto>>> Approve(Guid id)
    {
        var result = await _payments.ApprovePaymentAsync(id, CurrentAdminId, RequestAborted);

        return Ok(ApiResponse<AdminPaymentDetailsDto>.Ok(result, "تم تأكيد الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Reject)]
    [HttpPatch("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<AdminPaymentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminPaymentDetailsDto>>> Reject(
        Guid id, [FromBody] RejectPaymentRequest request)
    {
        var result = await _payments.RejectPaymentAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminPaymentDetailsDto>.Ok(result, "تم رفض عملية الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:guid}/refund")]
    [ProducesResponseType(typeof(ApiResponse<AdminPaymentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminPaymentDetailsDto>>> Refund(
        Guid id, [FromBody] RefundPaymentRequest request)
    {
        var result = await _payments.RefundPaymentAsync(id, CurrentAdminId, request, RequestAborted);

        return Ok(ApiResponse<AdminPaymentDetailsDto>.Ok(result, "تم تسجيل استرداد المبلغ."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminOptionDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<AdminOptionDto>>> GetStatuses()
    {
        var statuses = PaymentCatalog.StatusNames
            .Select(entry => new AdminOptionDto { Id = (int)entry.Key, Name = entry.Value })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<AdminOptionDto>>.Ok(statuses, "تم استرجاع حالات الدفع."));
    }
}

[Route(ApiVersions.AdminRoutePrefix + "/payment-methods")]
[Tags("Admin Payment Methods")]
[AdminPage(AdminPageCatalog.Keys.Payments)]
public class AdminPaymentMethodsV2Controller : AdminApiController
{
    private readonly IPaymentService _payments;

    public AdminPaymentMethodsV2Controller(IPaymentService payments)
    {
        _payments = payments;
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentMethodDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentMethodDto>>>> GetAll()
    {
        var result = await _payments.GetAllMethodsAsync(RequestAborted);

        return Ok(ApiResponse<IReadOnlyList<PaymentMethodDto>>.Ok(result, "تم استرجاع وسائل الدفع."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> GetById(int id)
    {
        var result = await _payments.GetMethodByIdAsync(id, RequestAborted);

        return Ok(ApiResponse<PaymentMethodDto>.Ok(result, "تم استرجاع وسيلة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> Create(
        [FromBody] CreatePaymentMethodRequest request)
    {
        var result = await _payments.CreateMethodAsync(request, RequestAborted);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<PaymentMethodDto>.Ok(result, "تم إضافة وسيلة الدفع."));
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
        var result = await _payments.UpdateMethodAsync(id, request, RequestAborted);

        return Ok(ApiResponse<PaymentMethodDto>.Ok(result, "تم تحديث وسيلة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Manage)]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaymentMethodDto>>> SetStatus(
        int id, [FromBody] UpdateActiveStatusRequest request)
    {
        var current = await _payments.GetMethodByIdAsync(id, RequestAborted);

        var update = new UpdatePaymentMethodRequest
        {
            Name = current.Name,
            ArabicName = current.ArabicName,
            Type = current.Type,
            PhoneNumber = current.PhoneNumber,
            InstaPayIdentifier = current.InstaPayId,
            BankName = current.Bank?.BankName,
            AccountHolderName = current.Bank?.AccountHolderName,
            AccountNumber = current.Bank?.AccountNumber,
            Iban = current.Bank?.Iban,
            Instructions = current.Instructions,
            DisplayOrder = current.DisplayOrder,
            IsActive = request.IsActive
        };

        var result = await _payments.UpdateMethodAsync(id, update, RequestAborted);

        return Ok(ApiResponse<PaymentMethodDto>.Ok(
            result, request.IsActive ? "تم تفعيل وسيلة الدفع." : "تم تعطيل وسيلة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.Delete)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        await _payments.DeleteMethodAsync(id, RequestAborted);

        return Ok(ApiResponse.Ok("تم حذف وسيلة الدفع."));
    }

    [RequireAdminPermission(AdminPermission.View)]
    [HttpGet("types")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminOptionDto>>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<IReadOnlyList<AdminOptionDto>>> GetTypes()
    {
        var types = PaymentCatalog.MethodTypeNames
            .Select(entry => new AdminOptionDto { Id = (int)entry.Key, Name = entry.Value })
            .ToList();

        return Ok(ApiResponse<IReadOnlyList<AdminOptionDto>>.Ok(types, "تم استرجاع أنواع وسائل الدفع."));
    }
}
