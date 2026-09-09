using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadmintonHub.Models;

[Table("court")]
public class Court
{
    [Key, Column("court_id")]
    public long CourtId { get; set; }

    [Required, MaxLength(20), Column("court_code")]
    public string CourtCode { get; set; } = string.Empty;

    [Required, MaxLength(150), Column("court_name")]
    public string CourtName { get; set; } = string.Empty;

    [Required, MaxLength(20), Column("court_type")]
    public string CourtType { get; set; } = "SINGLE"; // SINGLE | DOUBLE

    [Column("price_per_hour")]
    public decimal PricePerHour { get; set; }

    [Required, MaxLength(20), Column("status")]
    public string Status { get; set; } = "ACTIVE"; // ACTIVE | MAINTENANCE | INACTIVE

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}