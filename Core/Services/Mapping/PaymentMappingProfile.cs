using AutoMapper;
using Domain.Entities;
using Shared.Constants;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace Services.Mapping;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<PaymentMethod, PaymentMethodDto>()
            .ForMember(d => d.TypeName, o => o.MapFrom(s => PaymentCatalog.GetMethodTypeName(s.Type)))
            .ForMember(d => d.PhoneNumber,
                o => o.MapFrom(s => s.Type == PaymentMethodType.MobileWallet ? s.PhoneNumber : null))
            .ForMember(d => d.InstaPayId,
                o => o.MapFrom(s => s.Type == PaymentMethodType.InstaPay ? s.InstaPayIdentifier : null))
            .ForMember(d => d.Bank, o => o.MapFrom(s =>
                s.Type == PaymentMethodType.BankAccount
                    ? new BankAccountDto
                    {
                        BankName = s.BankName,
                        AccountHolderName = s.AccountHolderName,
                        AccountNumber = s.AccountNumber,
                        Iban = s.Iban
                    }
                    : null));

        CreateMap<PaymentMethod, PaymentMethodSummaryDto>()
            .ForMember(d => d.TypeName, o => o.MapFrom(s => PaymentCatalog.GetMethodTypeName(s.Type)));

        CreateMap<Payment, PaymentListItemDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => PaymentCatalog.GetStatusName(s.Status)));

        CreateMap<Payment, PaymentDetailsDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => PaymentCatalog.GetStatusName(s.Status)));
    }
}
