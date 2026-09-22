using BadmintonHub.ViewModels;

namespace BadmintonHub.Services;

public interface ICourtService
{
    Task<IReadOnlyList<CourtResponseDto>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<CourtResponseDto>> GetAvailableAsync(DateOnly? date, TimeOnly? startTime, TimeOnly? endTime, CancellationToken ct);
    Task<CourtResponseDto?> GetByIdAsync(long id, CancellationToken ct);
    Task<CourtResponseDto> CreateAsync(CourtCreateDto dto, CancellationToken ct);
    Task<CourtResponseDto?> UpdateAsync(long id, CourtUpdateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(long id, CancellationToken ct);
}