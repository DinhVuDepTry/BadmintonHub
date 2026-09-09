using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BadmintonHub.Models.Enums;

namespace BadmintonHub.Models;

[Table("booking")]
public class Booking
{
    [Key, Column("booking_id")]
    public long BookingId { get; set; }

    [Required, Column("court_id")]
    public long CourtId { get; set; }

    [Required, Column("customer_id")]
    public string CustomerId { get; set; } = string.Empty; // FK -> ApplicationUser.Id

    [Column("booking_date", TypeName = "date")]
    public DateOnly BookingDate { get; set; }

    [Column("start_time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    [Column("status")]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    [Column("total_price")]
    public decimal TotalPrice { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CourtId))]
    public Court Court { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    public ApplicationUser Customer { get; set; } = null!;
}