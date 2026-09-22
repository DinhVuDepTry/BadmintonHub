using BadmintonHub.Data;
using BadmintonHub.Models;
using BadmintonHub.Models.Enums;
using BadmintonHub.Services;
using BadmintonHub.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BadmintonHub.Tests;

public sealed class CourtServiceTests
{
    [Fact]
    public async Task GetAvailableAsync_ExcludesInactiveAndOverlappingCourts()
    {
        await using var db = CreateDbContext();
        db.Courts.AddRange(
            new Court { CourtCode = "C01", CourtName = "Court One", CourtType = "DOUBLE", PricePerHour = 100000, Status = CourtStatus.Active },
            new Court { CourtCode = "C02", CourtName = "Court Two", CourtType = "DOUBLE", PricePerHour = 100000, Status = CourtStatus.Maintenance },
            new Court { CourtCode = "C03", CourtName = "Court Three", CourtType = "SINGLE", PricePerHour = 100000, Status = CourtStatus.Active });
        await db.SaveChangesAsync();

        db.Bookings.Add(new Booking
        {
            CourtId = 1,
            CustomerId = "customer",
            BookingDate = new DateOnly(2026, 10, 1),
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(20, 0),
            Status = BookingStatus.Confirmed,
            TotalPrice = 200000
        });
        await db.SaveChangesAsync();

        var service = new CourtService(db);
        var available = await service.GetAvailableAsync(
            new DateOnly(2026, 10, 1), new TimeOnly(19, 0), new TimeOnly(21, 0), CancellationToken.None);

        Assert.Equal(["C03"], available.Select(c => c.CourtCode));
    }

    [Fact]
    public async Task GetAvailableAsync_IncludesCourtWhenBookingDoesNotOverlap()
    {
        await using var db = CreateDbContext();
        db.Courts.Add(new Court
        {
            CourtCode = "C01",
            CourtName = "Court One",
            CourtType = "DOUBLE",
            PricePerHour = 100000,
            Status = CourtStatus.Active
        });
        await db.SaveChangesAsync();

        db.Bookings.Add(new Booking
        {
            CourtId = 1,
            CustomerId = "customer",
            BookingDate = new DateOnly(2026, 10, 1),
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(20, 0),
            Status = BookingStatus.Confirmed,
            TotalPrice = 200000
        });
        await db.SaveChangesAsync();

        var service = new CourtService(db);
        var available = await service.GetAvailableAsync(
            new DateOnly(2026, 10, 1), new TimeOnly(20, 0), new TimeOnly(21, 0), CancellationToken.None);

        Assert.Single(available);
    }

    [Fact]
    public async Task ConfirmAsync_ChangesPendingBookingToConfirmed()
    {
        await using var db = CreateDbContext();
        db.Courts.Add(new Court
        {
            CourtCode = "C01",
            CourtName = "Court One",
            CourtType = "DOUBLE",
            PricePerHour = 100000,
            Status = CourtStatus.Active
        });
        await db.SaveChangesAsync();

        db.Bookings.Add(new Booking
        {
            CourtId = 1,
            CustomerId = "customer",
            BookingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(20, 0),
            Status = BookingStatus.Pending,
            TotalPrice = 200000
        });
        await db.SaveChangesAsync();

        var service = new BookingService(db);
        var confirmed = await service.ConfirmAsync(1, CancellationToken.None);

        Assert.True(confirmed);
        Assert.Equal(BookingStatus.Confirmed, (await db.Bookings.SingleAsync()).Status);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}
