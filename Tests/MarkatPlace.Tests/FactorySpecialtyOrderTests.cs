using Shared.Constants;
using Shared.Enums;
using Xunit;

namespace MarkatPlace.Tests;

public class FactorySpecialtyOrderTests
{
    private const string AutoParts = "قطع غيار السيارات";
    private const string Other = "أخرى";

    private static List<string> DisplayedOrder() =>
        BusinessCatalog.ProductionSpecialtyNames
            .Select(entry => new { Id = (int)entry.Key, entry.Value })
            .OrderBy(row => row.Id == BusinessCatalog.ProductionSpecialtyLastId ? 1 : 0)
            .ThenBy(row => row.Id)
            .Select(row => row.Value)
            .ToList();

    [Fact]
    public void Auto_parts_is_offered_immediately_before_other()
    {
        var order = DisplayedOrder();

        var autoParts = order.IndexOf(AutoParts);
        var other = order.IndexOf(Other);

        Assert.True(autoParts >= 0, $"'{AutoParts}' is no longer offered to factory owners");
        Assert.True(other >= 0, $"'{Other}' is no longer offered to factory owners");
        Assert.Equal(other - 1, autoParts);
    }

    [Fact]
    public void The_catalog_declares_auto_parts_immediately_before_other()
    {
        var declared = BusinessCatalog.ProductionSpecialtyNames.Keys.ToList();

        Assert.Equal(ProductionSpecialty.Other, declared[^1]);
        Assert.Equal(ProductionSpecialty.CarSpareParts, declared[^2]);
    }

    [Fact]
    public void Other_is_the_last_choice_a_factory_owner_sees()
    {
        var order = DisplayedOrder();
        Assert.Equal(Other, order[^1]);
    }

    [Fact]
    public void Every_other_specialty_keeps_the_position_it_had()
    {
        var expected = BusinessCatalog.ProductionSpecialtyNames
            .Where(entry => entry.Key is not (ProductionSpecialty.Other or ProductionSpecialty.CarSpareParts))
            .OrderBy(entry => (int)entry.Key)
            .Select(entry => entry.Value)
            .ToList();

        var actual = DisplayedOrder()
            .Where(name => name != Other && name != AutoParts)
            .ToList();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void The_reorder_did_not_renumber_a_single_specialty()
    {
        Assert.Equal(38, (int)ProductionSpecialty.Other);
        Assert.Equal(39, (int)ProductionSpecialty.CarSpareParts);
        Assert.Equal((int)ProductionSpecialty.Other, BusinessCatalog.ProductionSpecialtyLastId);
    }

    [Fact]
    public void No_specialty_was_added_renamed_or_dropped()
    {
        var enumValues = Enum.GetValues<ProductionSpecialty>().ToHashSet();
        var catalogued = BusinessCatalog.ProductionSpecialtyNames.Keys.ToHashSet();

        Assert.Equal(enumValues, catalogued);
        Assert.Equal(39, catalogued.Count);
        Assert.All(BusinessCatalog.ProductionSpecialtyNames.Values,
            name => Assert.True(ArabicText.IsArabic(name), $"'{name}' is not Arabic"));
    }

    [Fact]
    public void Both_read_paths_sort_other_last_rather_than_by_raw_id()
    {
        var repositories = new[]
        {
            "Infastrucre/Presitance/Repositories/FactoryRepository.cs",
            "Infastrucre/Presitance/Repositories/LookupRepository.cs",
        };

        foreach (var relative in repositories)
        {
            var path = Path.Combine(RepositoryRoot.Path, relative);
            var source = File.ReadAllText(path);

            var index = source.IndexOf("_context.ProductionSpecialties", StringComparison.Ordinal);
            Assert.True(index >= 0, $"{relative} no longer reads ProductionSpecialties");

            var query = source[index..source.IndexOf(';', index)];

            Assert.Contains("ProductionSpecialtyLastId", query);
            Assert.DoesNotContain(".OrderBy(p => p.Id)", query);
        }
    }
}
