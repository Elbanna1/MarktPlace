using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Admin;

public enum AdminReportAction
{
    None = 0,

    Suspend = 1,

    Delete = 2
}

public class AdminReportDecisionRequest
{
    [MaxLength(1000, ErrorMessage = "لا يمكن أن تتجاوز الملاحظات 1000 حرف.")]
    public string? Note { get; set; }
}

public class AdminReportActionRequest : AdminReportDecisionRequest
{
    [EnumDataType(typeof(AdminReportAction), ErrorMessage = "الإجراء غير صحيح.")]
    public AdminReportAction Action { get; set; } = AdminReportAction.None;
}
