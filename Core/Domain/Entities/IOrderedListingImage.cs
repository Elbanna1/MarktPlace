namespace Domain.Entities;

public interface IOrderedListingImage : IListingImage
{
    int SortOrder { get; set; }
}
