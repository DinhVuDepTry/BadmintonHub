using Microsoft.EntityFrameworkCore;
using BadmintonHub.Data;
using BadmintonHub.Models;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Services;

public sealed class CourtService(AppDbContext db) : ICourtService
{
    public async Task<IReadOnlyList<CourtResponseDto>> GetAllAsync(CancellationToken ct) =>
        await db.Courts.AsNoTracking()
            .OrderBy(c => c.CourtCode)
            .Select(c => new CourtResponseDto(
                c.CourtId, c.CourtCode, c.CourtName, c.CourtType, c.PricePerHour, c.Status))
            .ToListAsync(ct);

    public async Task<CourtResponseDto?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.Courts.AsNoTracking()
            .Where(c => c.CourtId == id)
            .Select(c => new CourtResponseDto(
                c.CourtId, c.CourtCode, c.CourtName, c.CourtType, c.PricePerHour, c.Status))
            .SingleOrDefaultAsync(ct);

    public async Task<CourtResponseDto> CreateAsync(CourtCreateDto dto, CancellationToken ct)
    {
        var code = dto.CourtCode.Trim().ToUpperInvariant();
        if (await db.Courts.AnyAsync(c => c.CourtCode == code, ct))
            throw new InvalidOperationException("Court code already exists.");

        var entity = new Court
        {
            CourtCode = code,
            CourtName = dto.CourtName.Trim(),
            CourtType = dto.CourtType.Trim().ToUpperInvariant(),
            PricePerHour = dto.PricePerHour,
            Status = dto.Status.Trim().ToUpperInvariant()
        };

        db.Courts.Add(entity);
        await db.SaveChangesAsync(ct);
        return new CourtResponseDto(entity.CourtId, entity.CourtCode, entity.CourtName,
            entity.CourtType, entity.PricePerHour, entity.Status);
    }

    public async Task<CourtResponseDto?> UpdateAsync(long id, CourtUpdateDto dto, CancellationToken ct)
    {
        var entity = await db.Courts.FindAsync([id], ct);
        if (entity is null) return null;

        var code = dto.CourtCode.Trim().ToUpperInvariant();
        if (await db.Courts.AnyAsync(c => c.CourtId != id && c.CourtCode == code, ct))
            throw new InvalidOperationException("Court code already exists.");

        entity.CourtCode = code;
        entity.CourtName = dto.CourtName.Trim();
        entity.CourtType = dto.CourtType.Trim().ToUpperInvariant();
        entity.PricePerHour = dto.PricePerHour;
        entity.Status = dto.Status.Trim().ToUpperInvariant();

        await db.SaveChangesAsync(ct);
        return new CourtResponseDto(entity.CourtId, entity.CourtCode, entity.CourtName,
            entity.CourtType, entity.PricePerHour, entity.Status);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct)
    {
        var entity = await db.Courts.FindAsync([id], ct);
        if (entity is null) return false;

        if (await db.Bookings.AnyAsync(b => b.CourtId == id, ct))
            throw new InvalidOperationException("Cannot delete a court that has bookings.");

        db.Courts.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}