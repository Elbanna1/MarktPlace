using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Validation;
using Shared.Constants;
using Shared.DTOs.Workshops;
using Shared.Enums;
using Shared.Exceptions;
using Xunit;
using static MarkatPlace.Tests.WorkshopCraftsmenRestructureTests;

namespace MarkatPlace.Tests;

public class AluminumWorkshopRetirementTests
{
    [Fact]
    public void The_standalone_aluminum_option_is_retired_and_the_kitchens_option_is_untouched()
    {
        Assert.Equal(3, (int)WorkshopType.Aluminum);
        Assert.Equal(7, (int)WorkshopType.Kitchens);
        Assert.False(WorkshopCraftsmenCatalog.IsActiveWorkshopType(WorkshopType.Aluminum));
        Assert.True(WorkshopCraftsmenCatalog.IsActiveWorkshopType(WorkshopType.Kitchens));
        Assert.Equal("ورشة ألوميتال", WorkshopCraftsmenCatalog.GetWorkshopTypeName(WorkshopType.Aluminum));
        Assert.Equal("ورشة ألوميتال ومطابخ", WorkshopCraftsmenCatalog.GetWorkshopTypeName(WorkshopType.Kitchens));
    }

    [Fact]
    public void The_retirement_needs_no_migration_because_the_seeded_lookup_rows_are_unchanged()
    {
        using var context = NewContext();

        var aluminum = context.WorkshopTypes.Single(w => w.Id == (int)WorkshopType.Aluminum);
        var kitchens = context.WorkshopTypes.Single(w => w.Id == (int)WorkshopType.Kitchens);

        Assert.True(aluminum.IsActive);
        Assert.Equal("ورشة ألوميتال", aluminum.Name);
        Assert.True(kitchens.IsActive);
        Assert.Equal("ورشة ألوميتال ومطابخ", kitchens.Name);
        Assert.Equal(new[] { 20, 24 },
            context.WorkshopTypes.Where(w => !w.IsActive).Select(w => w.Id).OrderBy(id => id));
        Assert.Subset(WorkshopCraftsmenCatalog.RetiredWorkshopTypes.ToHashSet(),
            WorkshopCraftsmenCatalog.InactiveWorkshopTypeRows.ToHashSet());
    }

    [Fact]
    public async Task The_types_lookup_hides_standalone_aluminum_and_keeps_aluminum_and_kitchens()
    {
        using var context = NewContext();

        var types = await Lookups(context).GetWorkshopTypesAsync();

        Assert.DoesNotContain(types, t => t.Id == (int)WorkshopType.Aluminum);
        Assert.Single(types, t => t.Id == (int)WorkshopType.Kitchens && t.Name == "ورشة ألوميتال ومطابخ");
        Assert.Equal(26, types.Count);
        Assert.Equal("أخرى", types[^1].Name);
    }

    [Fact]
    public void Create_validation_refuses_standalone_aluminum_and_accepts_aluminum_and_kitchens()
    {
        var refused = new CreateWorkshopRequestValidator().Validate(WorkshopRequest(WorkshopType.Aluminum)).Errors;
        Assert.Contains(refused, e => e.ErrorMessage == WorkshopCraftsmenCatalog.RetiredChoiceMessage);

        var accepted = new CreateWorkshopRequestValidator().Validate(WorkshopRequest(WorkshopType.Kitchens)).Errors;
        Assert.DoesNotContain(accepted, e => e.PropertyName == nameof(CreateWorkshopRequest.WorkshopType));
    }

    [Fact]
    public async Task A_historical_standalone_aluminum_workshop_stays_readable_filterable_and_editable()
    {
        using var context = NewContext();
        var owner = SeedUser(context);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Workshops(context).CreateAsync(owner, WorkshopRequest(WorkshopType.Aluminum), [Upload()]));

        var kitchens = await Workshops(context).CreateAsync(owner, WorkshopRequest(WorkshopType.Kitchens), [Upload()]);
        Assert.Equal("ورشة ألوميتال ومطابخ", kitchens.WorkshopTypeName);

        var legacy = new Workshop
        {
            Id = Guid.NewGuid(), UserId = owner, Name = "ورشة قديمة", WorkshopType = WorkshopType.Aluminum,
            Governorate = LocationConstants.Governorate, Center = "الفيوم", Address = "عنوان",
            PhoneNumber = "01012345678", WhatsApp = "01012345678", AdTitle = "ألوميتال", AdDescription = "وصف",
            ModerationStatus = ModerationStatus.Approved, PublishedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(30)
        };
        context.Workshops.Add(legacy);
        await context.SaveChangesAsync();

        Assert.Equal("ورشة ألوميتال", (await Workshops(context).GetByIdAsync(legacy.Id)).WorkshopTypeName);
        Assert.Equal(1, (await Workshops(context).GetListAsync(
            new WorkshopFilterParams { WorkshopType = WorkshopType.Aluminum })).TotalCount);

        var kept = await Workshops(context).UpdateAsync(owner, false, legacy.Id, WorkshopUpdate(WorkshopType.Aluminum), []);
        Assert.Equal(WorkshopType.Aluminum, kept.WorkshopType);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            Workshops(context).UpdateAsync(owner, false, kitchens.Id, WorkshopUpdate(WorkshopType.Aluminum), []));

        var moved = await Workshops(context).UpdateAsync(owner, false, legacy.Id, WorkshopUpdate(WorkshopType.Kitchens), []);
        Assert.Equal("ورشة ألوميتال ومطابخ", moved.WorkshopTypeName);
        Assert.Equal(2, await context.Workshops.IgnoreQueryFilters().CountAsync());
    }
}
