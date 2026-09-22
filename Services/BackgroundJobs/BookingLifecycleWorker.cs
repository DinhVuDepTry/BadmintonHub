using Microsoft.EntityFrameworkCore;
using BadmintonHub.Data;
using BadmintonHub.Models.Enums;

namespace BadmintonHub.Services.BackgroundJobs;

public sealed class BookingLifecycleWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<BookingLifecycleWorker> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ProcessBookingsAsync(stoppingToken);

        using var timer = new PeriodicTimer(Interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await ProcessBookingsAsync(stoppingToken);
    }

    private async Task ProcessBookingsAsync(CancellationToken ct)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var now = DateTime.Now;

            var bookings = await db.Bookings
                .Where(b => b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed)
                .ToListAsync(ct);

            var changed = 0;
            foreach (var booking in bookings)
            {
                var end = booking.BookingDate.ToDateTime(booking.EndTime);
                if (end > now)
                    continue;

                booking.Status = booking.Status == BookingStatus.Confirmed
                    ? BookingStatus.Completed
                    : BookingStatus.Expired;
                changed++;
            }

            if (changed > 0)
            {
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Updated {BookingCount} completed or expired bookings.", changed);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Booking lifecycle processing failed.");
        }
    }
}
