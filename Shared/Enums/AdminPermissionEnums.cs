namespace Shared.Enums;

[Flags]
public enum AdminPermission
{
    None = 0,

    View = 1,

    Create = 2,

    Edit = 4,

    Delete = 8,

    Approve = 16,

    Reject = 32,

    Manage = 64
}
