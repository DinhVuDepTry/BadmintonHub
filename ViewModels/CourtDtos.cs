using System.ComponentModel.DataAnnotations;

namespace BadmintonHub.ViewModels;

public class CourtCreateDto
{
    [Required, MaxLength(20)]
    public string CourtCode { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string CourtName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string CourtType { get; set; } = "SINGLE";

    [Range(0, double.MaxValue)]
    public decimal PricePerHour { get; set; }

    public string Status { get; set; } = "ACTIVE";
}

public sealed class CourtUpdateDto : CourtCreateDto { }

public record CourtResponseDto(
    long CourtId,
    string CourtCode,
    string CourtName,
    string CourtType,
    decimal PricePerHour,
    string Status);