using Microsoft.EntityFrameworkCore;
using BadmintonHub.Data;
using BadmintonHub.Models;
using BadmintonHub.Models.Enums;
using BadmintonHub.ViewModels;

namespace BadmintonHub.Services;

public sealed class BookingService(AppDbContext db) : IBookingService
{
    public async Task<IReadOnlyList<BookingResponseDto>> GetAllAsync(
        string? customerId, CancellationToken ct)
    {
        var query = db.Bookings.AsNoTracking().Include(b => b.Court).AsQueryable();
        if (customerId is not null)
            query = query.Where(b => b.CustomerId == customerId);

        return await query
            .OrderByDescending(b => b.BookingDate)
            .Select(b => new BookingResponseDto(
                b.BookingId, b.CourtId, b.Court.CourtName, b.CustomerId,
                b.BookingDate, b.StartTime, b.EndTime,
                b.Status.ToString(), b.TotalPrice))
            .ToListAsync(ct);
    }

    public async Task<BookingResponseDto?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.Bookings.AsNoTracking().Include(b => b.Court)
            .Where(b => b.BookingId == id)
            .Select(b => new BookingResponseDto(
                b.BookingId, b.CourtId, b.Court.CourtName, b.CustomerId,
                b.BookingDate, b.StartTime, b.EndTime,
                b.Status.ToString(), b.TotalPrice))
            .SingleOrDefaultAsync(ct);

    public async Task<BookingResponseDto> CreateAsync(
        string customerId, BookingCreateDto dto, CancellationToken ct)
    {
        var court = await db.Courts.FindAsync([dto.CourtId], ct)
            ?? throw new KeyNotFoundException("Court does not exist.");

        if (dto.EndTime <= dto.StartTime)
            throw new InvalidOperationException("End time must be after start time.");

        // Check trùng lịch: cùng sân, cùng ngày, giờ giao nhau, đang Pending/Confirmed
        bool overlap = await db.Bookings.AnyAsync(b =>
            b.CourtId == dto.CourtId &&
            b.BookingDate == dto.BookingDate &&
            (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) &&
            dto.StartTime < b.EndTime && dto.EndTime > b.StartTime, ct);

        if (overlap)
            throw new InvalidOperationException("This court is already booked for the selected time.");

        var hours = (decimal)(dto.EndTime - dto.StartTime).TotalHours;

        var entity = new Booking
        {
            CourtId = dto.CourtId,
            CustomerId = customerId,
            BookingDate = dto.BookingDate,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = BookingStatus.Pending,
            TotalPrice = hours * court.PricePerHour,
            CreatedAt = DateTime.UtcNow
        };

        db.Bookings.Add(entity);
        await db.SaveChangesAsync(ct);

        return (await GetByIdAsync(entity.BookingId, ct))!;
    }

    public async Task<bool> CancelAsync(long id, string customerId, CancellationToken ct)
    {
        var entity = await db.Bookings.FindAsync([id], ct);
        if (entity is null || entity.CustomerId != customerId) return false;

        entity.Status = BookingStatus.Cancelled;
        await db.SaveChangesAsync(ct);
        return true;
    }
}