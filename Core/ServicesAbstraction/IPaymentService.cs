using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IPaymentService
{
    Task<IReadOnlyList<PaymentMethodDto>> GetMethodsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PaymentListItemDto>> GetMyPaymentsAsync(
        string userId, PaymentStatus? status = null, CancellationToken cancellationToken = default);

    Task<PaymentDetailsDto> GetByIdAsync(
        Guid id, string userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    Task<PaymentDetailsDto> SubmitAsync(
        string userId,
        SubmitPaymentRequest request,
        UploadImageModel? screenshot,
        CancellationToken cancellationToken = default);

    Task<Shared.Responses.PaginatedResult<AdminPaymentListItemDto>> GetAllAsync(
        AdminPaymentFilterParams filter, CancellationToken cancellationToken = default);

    Task<AdminPaymentDetailsDto> GetForAdminAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AdminPaymentDetailsDto> ApprovePaymentAsync(
        Guid id, string adminUserId, CancellationToken cancellationToken = default);

    Task<AdminPaymentDetailsDto> RejectPaymentAsync(
        Guid id, string adminUserId, RejectPaymentRequest request, CancellationToken cancellationToken = default);

    Task<AdminPaymentDetailsDto> RefundPaymentAsync(
        Guid id, string adminUserId, RefundPaymentRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PaymentMethodDto>> GetAllMethodsAsync(CancellationToken cancellationToken = default);

    Task<PaymentMethodDto> GetMethodByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PaymentMethodDto> CreateMethodAsync(
        CreatePaymentMethodRequest request, CancellationToken cancellationToken = default);

    Task<PaymentMethodDto> UpdateMethodAsync(
        int id, UpdatePaymentMethodRequest request, CancellationToken cancellationToken = default);

    Task DeleteMethodAsync(int id, CancellationToken cancellationToken = default);
}
