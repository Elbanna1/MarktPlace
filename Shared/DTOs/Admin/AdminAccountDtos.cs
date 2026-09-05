using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminPermissionOptionDto
{
    public string Key { get; set; } = default!;

    public int Value { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;
}

public class AdminPageDefinitionDto
{
    public string Key { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string Group { get; set; } = default!;

    public IReadOnlyList<AdminPermissionOptionDto> Permissions { get; set; } =
        Array.Empty<AdminPermissionOptionDto>();
}

public class AdminPermissionCatalogDto
{
    public IReadOnlyList<AdminPageDefinitionDto> Pages { get; set; } = Array.Empty<AdminPageDefinitionDto>();

    public IReadOnlyList<AdminPermissionOptionDto> Permissions { get; set; } =
        Array.Empty<AdminPermissionOptionDto>();
}

public class AdminGrantedPageDto
{
    public string Key { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string Group { get; set; } = default!;

    public IReadOnlyList<string> Permissions { get; set; } = Array.Empty<string>();
}

public class AdminIdentityDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Image { get; set; }

    public bool IsActive { get; set; }

    public bool IsSuperAdmin { get; set; }
}

public class AdminPermissionsDto
{
    public AdminIdentityDto Admin { get; set; } = new();

    public IReadOnlyList<AdminGrantedPageDto> Pages { get; set; } = Array.Empty<AdminGrantedPageDto>();
}

public class AdminAccountListItemDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Image { get; set; }

    public bool IsActive { get; set; }

    public bool IsSuperAdmin { get; set; }

    public UserAccountStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public int PagesCount { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class AdminAccountDetailsDto : AdminAccountListItemDto
{
    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string? StatusReason { get; set; }

    public DateTime? StatusChangedAt { get; set; }

    public IReadOnlyList<AdminGrantedPageDto> Pages { get; set; } = Array.Empty<AdminGrantedPageDto>();
}

public class AdminPageAssignmentRequest
{
    public string PageKey { get; set; } = default!;

    public IReadOnlyList<string> Permissions { get; set; } = Array.Empty<string>();
}

public class AdminCandidateSearchParams
{
    public const int MinimumTermLength = 2;

    private const int MaxLimit = 25;

    private int _limit = 10;

    public string? Q { get; set; }

    public bool ExcludeAdmins { get; set; }

    public int Limit
    {
        get => _limit;
        set => _limit = value switch
        {
            < 1 => 1,
            > MaxLimit => MaxLimit,
            _ => value
        };
    }
}

public class AdminCandidateUserDto
{
    public string UserId { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Image { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsSuperAdmin { get; set; }

    public UserAccountStatus Status { get; set; }

    public string StatusName { get; set; } = default!;
}

public class ConfirmAdminRequest
{
    public string UserId { get; set; } = default!;

    public IReadOnlyList<AdminPageAssignmentRequest> Pages { get; set; } =
        Array.Empty<AdminPageAssignmentRequest>();
}

public class UpdateAdminRequest
{
    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? Password { get; set; }
}

public class UpdateAdminStatusRequest
{
    public bool IsActive { get; set; }

    public string? Reason { get; set; }
}

public class UpdateAdminPermissionsRequest
{
    public IReadOnlyList<AdminPageAssignmentRequest> Pages { get; set; } =
        Array.Empty<AdminPageAssignmentRequest>();
}

public class AdminAccountFilterParams
{
    private const int MaxPageSize = 100;

    private int _pageSize = 20;
    private int _pageIndex = 1;

    public string? Search { get; set; }

    public bool? IsActive { get; set; }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 1,
            > MaxPageSize => MaxPageSize,
            _ => value
        };
    }
}
