using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Admin;

public class AdminGovernorateDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public int CentersCount { get; set; }

    public IReadOnlyList<AdminCenterDto> Centers { get; set; } = Array.Empty<AdminCenterDto>();
}

public class AdminCenterDto
{
    public int Id { get; set; }

    public int GovernorateId { get; set; }

    public string GovernorateName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public int ProjectsCount { get; set; }

    public IReadOnlyList<AdminProjectDto> Projects { get; set; } = Array.Empty<AdminProjectDto>();
}

public class AdminProjectDto
{
    public int Id { get; set; }

    public int CenterId { get; set; }

    public string CenterName { get; set; } = default!;

    public int GovernorateId { get; set; }

    public string GovernorateName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}

public class SaveLocationRequest
{
    [Required(ErrorMessage = "الاسم مطلوب.")]
    [MaxLength(150, ErrorMessage = "لا يمكن أن يتجاوز الاسم 150 حرفًا.")]
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "الترتيب يجب أن يكون بين 0 و 9999.")]
    public int SortOrder { get; set; }
}

public class CreateProjectRequest : SaveLocationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "المركز مطلوب.")]
    public int CenterId { get; set; }
}

public class UpdateProjectRequest : SaveLocationRequest
{
    public int? CenterId { get; set; }
}
