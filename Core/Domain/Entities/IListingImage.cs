namespace Domain.Entities;

public interface IListingImage
{
    Guid Id { get; set; }

    string FileName { get; set; }

    string ImagePath { get; set; }

    string ImageUrl { get; set; }

    bool IsPrimary { get; set; }

    DateTime CreatedAt { get; set; }
}
