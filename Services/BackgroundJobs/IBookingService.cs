using BadmintonHub.ViewModels;

namespace BadmintonHub.Services;

public interface IBookingService
{
    Task<IReadOnlyList<BookingResponseDto>> GetAllAsync(string? customerId, CancellationToken ct);
    Task<BookingResponseDto?> GetByIdAsync(long id, CancellationToken ct);
    Task<BookingResponseDto> CreateAsync(string customerId, BookingCreateDto dto, CancellationToken ct);
    Task<bool> CancelAsync(long id, string customerId, CancellationToken ct);
}