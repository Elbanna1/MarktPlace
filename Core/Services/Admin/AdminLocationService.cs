using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.Enums;
using Shared.Exceptions;

namespace Services.Admin;

public class AdminLocationService : IAdminLocationService
{
    private readonly IAdminLocationRepository _repository;
    private readonly ILookupCache _lookupCache;
    private readonly IAdminAuditService _audit;

    public AdminLocationService(
        IAdminLocationRepository repository, ILookupCache lookupCache, IAdminAuditService audit)
    {
        _repository = repository;
        _lookupCache = lookupCache;
        _audit = audit;
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _repository.SaveChangesAsync(cancellationToken);
        _lookupCache.Invalidate();
    }

    public async Task<IReadOnlyList<AdminGovernorateDto>> GetGovernoratesAsync(
        CancellationToken cancellationToken = default)
    {
        var governorates = await _repository.GetGovernoratesAsync(cancellationToken);

        return governorates.Select(MapGovernorate).ToList();
    }

    public async Task<AdminGovernorateDto> CreateGovernorateAsync(
        SaveLocationRequest request, CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();

        if (await _repository.GovernorateNameExistsAsync(name, null, cancellationToken))
            throw new ConflictException("توجد محافظة بنفس الاسم بالفعل.");

        var governorate = new Governorate
        {
            Id = await _repository.NextGovernorateIdAsync(cancellationToken),
            Name = name,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _repository.AddGovernorate(governorate);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreateGovernorate, AdminAuditCatalog.Targets.Governorate,
            governorate.Id.ToString(), $"إضافة محافظة: {governorate.Name}",
            cancellationToken: cancellationToken);

        return MapGovernorate(governorate);
    }

    public async Task<AdminGovernorateDto> UpdateGovernorateAsync(
        int id, SaveLocationRequest request, CancellationToken cancellationToken = default)
    {
        var governorate = await _repository.FindGovernorateAsync(id, cancellationToken)
            ?? throw new NotFoundException("المحافظة غير موجودة.");

        var name = request.Name.Trim();

        if (await _repository.GovernorateNameExistsAsync(name, id, cancellationToken))
            throw new ConflictException("توجد محافظة بنفس الاسم بالفعل.");

        governorate.Name = name;
        governorate.IsActive = request.IsActive;
        governorate.SortOrder = request.SortOrder;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateGovernorate, AdminAuditCatalog.Targets.Governorate,
            governorate.Id.ToString(), $"تعديل محافظة: {governorate.Name}",
            cancellationToken: cancellationToken);

        return MapGovernorate(governorate);
    }

    public async Task DeleteGovernorateAsync(int id, CancellationToken cancellationToken = default)
    {
        var governorate = await _repository.FindGovernorateAsync(id, cancellationToken)
            ?? throw new NotFoundException("المحافظة غير موجودة.");

        if (governorate.Centers.Count > 0)
            throw new ConflictException("لا يمكن حذف محافظة تحتوي على مراكز.");

        var users = await _repository.CountUsersInGovernorateAsync(governorate.Name, cancellationToken);

        if (users > 0)
            throw new ConflictException(
                $"لا يمكن حذف محافظة مسجل بها {users} مستخدمًا. يمكنك تعطيلها بدلًا من ذلك.");

        _repository.RemoveGovernorate(governorate);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteGovernorate, AdminAuditCatalog.Targets.Governorate,
            governorate.Id.ToString(), $"حذف محافظة: {governorate.Name}",
            cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<AdminCenterDto>> GetCentersAsync(
        int governorateId, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindGovernorateAsync(governorateId, cancellationToken)
            ?? throw new NotFoundException("المحافظة غير موجودة.");

        var centers = await _repository.GetCentersAsync(governorateId, cancellationToken);

        return centers.Select(MapCenter).ToList();
    }

    public async Task<AdminCenterDto> CreateCenterAsync(
        int governorateId, SaveLocationRequest request, CancellationToken cancellationToken = default)
    {
        var governorate = await _repository.FindGovernorateAsync(governorateId, cancellationToken)
            ?? throw new NotFoundException("المحافظة غير موجودة.");

        var name = request.Name.Trim();

        if (await _repository.CenterNameExistsAsync(governorateId, name, null, cancellationToken))
            throw new ConflictException("يوجد مركز بنفس الاسم داخل هذه المحافظة.");

        var center = new Center
        {
            Id = await _repository.NextCenterIdAsync(cancellationToken),
            GovernorateId = governorateId,
            Governorate = governorate,
            Name = name,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _repository.AddCenter(center);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreateCenter, AdminAuditCatalog.Targets.Center,
            center.Id.ToString(), $"إضافة مركز: {center.Name}",
            cancellationToken: cancellationToken);

        return MapCenter(center);
    }

    public async Task<AdminCenterDto> UpdateCenterAsync(
        int id, SaveLocationRequest request, CancellationToken cancellationToken = default)
    {
        var center = await _repository.FindCenterAsync(id, cancellationToken)
            ?? throw new NotFoundException("المركز غير موجود.");

        var name = request.Name.Trim();

        if (await _repository.CenterNameExistsAsync(center.GovernorateId, name, id, cancellationToken))
            throw new ConflictException("يوجد مركز بنفس الاسم داخل هذه المحافظة.");

        center.Name = name;
        center.IsActive = request.IsActive;
        center.SortOrder = request.SortOrder;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateCenter, AdminAuditCatalog.Targets.Center,
            center.Id.ToString(), $"تعديل مركز: {center.Name}",
            cancellationToken: cancellationToken);

        return MapCenter(center);
    }

    public async Task DeleteCenterAsync(int id, CancellationToken cancellationToken = default)
    {
        var center = await _repository.FindCenterAsync(id, cancellationToken)
            ?? throw new NotFoundException("المركز غير موجود.");

        if (center.Projects.Count > 0)
            throw new ConflictException("لا يمكن حذف مركز يحتوي على مشاريع.");

        var users = await _repository.CountUsersInCenterAsync(center.Name, cancellationToken);

        if (users > 0)
            throw new ConflictException(
                $"لا يمكن حذف مركز مسجل به {users} مستخدمًا. يمكنك تعطيله بدلًا من ذلك.");

        _repository.RemoveCenter(center);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteCenter, AdminAuditCatalog.Targets.Center,
            center.Id.ToString(), $"حذف مركز: {center.Name}",
            cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<AdminProjectDto>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default)
    {
        var projects = await _repository.GetProjectsAsync(centerId, cancellationToken);

        return projects.Select(MapProject).ToList();
    }

    public async Task<AdminProjectDto> CreateProjectAsync(
        CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var center = await _repository.FindCenterAsync(request.CenterId, cancellationToken)
            ?? throw new NotFoundException("المركز غير موجود.");

        var name = request.Name.Trim();

        if (await _repository.ProjectNameExistsAsync(center.Id, name, null, cancellationToken))
            throw new ConflictException("يوجد مشروع بنفس الاسم داخل هذا المركز.");

        var project = new Project
        {
            Id = await _repository.NextProjectIdAsync(cancellationToken),
            CenterId = center.Id,
            Center = center,
            Name = name,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder
        };

        _repository.AddProject(project);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreateProject, AdminAuditCatalog.Targets.Project,
            project.Id.ToString(), $"إضافة مشروع: {project.Name}",
            cancellationToken: cancellationToken);

        return MapProject(project);
    }

    public async Task<AdminProjectDto> UpdateProjectAsync(
        int id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _repository.FindProjectAsync(id, cancellationToken)
            ?? throw new NotFoundException("المشروع غير موجود.");

        var targetCenterId = request.CenterId ?? project.CenterId;

        if (targetCenterId != project.CenterId)
        {
            var center = await _repository.FindCenterAsync(targetCenterId, cancellationToken)
                ?? throw new NotFoundException("المركز المستهدف غير موجود.");

            project.CenterId = center.Id;
            project.Center = center;
        }

        var name = request.Name.Trim();

        if (await _repository.ProjectNameExistsAsync(targetCenterId, name, id, cancellationToken))
            throw new ConflictException("يوجد مشروع بنفس الاسم داخل هذا المركز.");

        project.Name = name;
        project.IsActive = request.IsActive;
        project.SortOrder = request.SortOrder;

        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateProject, AdminAuditCatalog.Targets.Project,
            project.Id.ToString(), $"تعديل مشروع: {project.Name}",
            cancellationToken: cancellationToken);

        var refreshed = await _repository.FindProjectAsync(id, cancellationToken);

        return MapProject(refreshed ?? project);
    }

    public async Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _repository.FindProjectAsync(id, cancellationToken)
            ?? throw new NotFoundException("المشروع غير موجود.");

        _repository.RemoveProject(project);
        await SaveAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteProject, AdminAuditCatalog.Targets.Project,
            project.Id.ToString(), $"حذف مشروع: {project.Name}",
            cancellationToken: cancellationToken);
    }

    private static AdminGovernorateDto MapGovernorate(Governorate governorate) =>
        new()
        {
            Id = governorate.Id,
            Name = governorate.Name,
            IsActive = governorate.IsActive,
            SortOrder = governorate.SortOrder,
            CentersCount = governorate.Centers.Count,
            Centers = governorate.Centers
                .OrderBy(center => center.SortOrder)
                .ThenBy(center => center.Id)
                .Select(center => MapCenter(center, governorate))
                .ToList()
        };

    private static AdminCenterDto MapCenter(Center center) => MapCenter(center, center.Governorate);

    private static AdminCenterDto MapCenter(Center center, Governorate? governorate) =>
        new()
        {
            Id = center.Id,
            GovernorateId = center.GovernorateId,
            GovernorateName = governorate?.Name ?? string.Empty,
            Name = center.Name,
            IsActive = center.IsActive,
            SortOrder = center.SortOrder,
            ProjectsCount = center.Projects.Count,
            Projects = center.Projects
                .OrderBy(project => project.SortOrder)
                .ThenBy(project => project.Name)
                .Select(project => new AdminProjectDto
                {
                    Id = project.Id,
                    CenterId = center.Id,
                    CenterName = center.Name,
                    GovernorateId = center.GovernorateId,
                    GovernorateName = governorate?.Name ?? string.Empty,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    SortOrder = project.SortOrder
                })
                .ToList()
        };

    private static AdminProjectDto MapProject(Project project) =>
        new()
        {
            Id = project.Id,
            CenterId = project.CenterId,
            CenterName = project.Center?.Name ?? string.Empty,
            GovernorateId = project.Center?.GovernorateId ?? 0,
            GovernorateName = project.Center?.Governorate?.Name ?? string.Empty,
            Name = project.Name,
            IsActive = project.IsActive,
            SortOrder = project.SortOrder
        };
}
