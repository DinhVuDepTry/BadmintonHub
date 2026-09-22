namespace BadmintonHub.ViewModels;

public sealed record AdminDashboardViewModel(
    int ActiveCourtCount,
    int PendingBookingCount,
    int TodayBookingCount,
    decimal TodayRevenue,
    IReadOnlyList<BookingResponseDto> UpcomingBookings);
