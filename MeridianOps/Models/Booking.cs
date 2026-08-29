using MeridianOps.Enums;

namespace MeridianOps.Models;

public class Booking
{
    public Passenger Passenger { get; set; }
    public Flight Flight { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime BookingTime { get; set; } =  DateTime.Now;
}