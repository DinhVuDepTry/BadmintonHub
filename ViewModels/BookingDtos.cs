using System.ComponentModel.DataAnnotations;

namespace BadmintonHub.ViewModels;

public class BookingCreateDto
{
    [Required]
    public long CourtId { get; set; }

    [Required]
    public DateOnly BookingDate { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }
}

public record BookingResponseDto(
    long BookingId,
    long CourtId,
    string CourtName,
    string CustomerId,
    DateOnly BookingDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Status,
    decimal TotalPrice);