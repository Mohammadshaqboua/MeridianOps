/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Enums;

namespace MeridianOps.Models;

public class Flight
{
    public string        FlightNumber  { get; set; }
    public FlightType    Type          { get; set; }
    public DateTime      ArrivalTime   { get; set; }
    public DateTime      DepartureTime { get; set; }
    public int           SeatCapacity  { get; set; }
    public FlightStatus  Status        { get; set; } = FlightStatus.Scheduled;
    public Gate?         AssignedGate  { get; set; } 
    public List<Booking> Bookings      { get; }      = new();

    public int GetConfirmedCount()
    {
        return Bookings
            .Count(booking => booking.Status == BookingStatus.Confirmed);
    }

    public bool HasAvailableSeat()
    {
        return SeatCapacity > GetConfirmedCount();
    }

    public List<Booking> GetStandbyListOrdered()
    {
        return Bookings
            .Where(booking => booking.Status == BookingStatus.Standby)
            .OrderBy(booking => booking.BookingTime)
            .ToList();
    }

    public (DateTime start, DateTime end) GetGateOccupancyWindow()
    {
        return (ArrivalTime, DepartureTime);
    }
}