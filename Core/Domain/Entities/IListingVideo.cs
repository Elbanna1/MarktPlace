namespace Domain.Entities;

public interface IListingVideo
{
    Guid Id { get; set; }

    string FileName { get; set; }

    string VideoPath { get; set; }

    string VideoUrl { get; set; }

    DateTime CreatedAt { get; set; }
}
