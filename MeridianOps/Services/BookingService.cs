/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/
using MeridianOps.Common;
using MeridianOps.Enums;
using MeridianOps.Models;

namespace MeridianOps.Services;

public class BookingService
{
    public OperationResult BookPassenger(string passengerId,
        string flightNumber,
        PassengerService passengerService,
        FlightService flightService)
    {
        var passenger = passengerService.FindPassenger(passengerId);
        if (passenger == null)
        {
            return OperationResult.Fail("Passenger not found.");
        }
        var flight = flightService.FindFlight(flightNumber);
        if (flight == null)
        {
            return OperationResult.Fail("Flight not found.");
        }
        if (flight.Status == FlightStatus.Cancelled || flight.Status == FlightStatus.Departed)
        {
            return OperationResult.Fail($"Cannot book: flight is {flight.Status}.");
        }
        
        BookingStatus statusToUse;
        
        if (flight.HasAvailableSeat())
        {
            statusToUse = BookingStatus.Confirmed;
        }
        else
        {
            if (flight.GetStandbyListOrdered().Count >= AppConfig.StandbyCapacity)
            {
                return OperationResult.Fail("Standby list is full.");
            }
            statusToUse = BookingStatus.Standby;
        }
        
        var newBooking = new Booking()
        {
            Passenger = passenger,
            Flight = flight,
            Status = statusToUse
        };
        
        flight.Bookings.Add(newBooking);
        return statusToUse == BookingStatus.Confirmed
            ? OperationResult.Ok("Seat confirmed.")
            : OperationResult.Ok("Flight is fully booked; passenger added to standby list.");
    }
    
    public OperationResult CancelBooking(string passengerId,
        string flightNumber,
        PassengerService passengerService,
        FlightService flightService)
    {
        var passenger = passengerService.FindPassenger(passengerId);
        if (passenger == null)
        {
            return OperationResult.Fail("Passenger not found.");
        }
        
        var flight = flightService.FindFlight(flightNumber);
        if (flight == null)
        {
            return OperationResult.Fail("Flight not found.");
        }
        
        var booking = flight.Bookings.FirstOrDefault(b =>
            b.Passenger.PassengerId == passengerId && b.Status == BookingStatus.Confirmed);
        if (booking == null)
        {
            return OperationResult.Fail("No confirmed booking found for this passenger.");
        }
        
        booking.Status = BookingStatus.Cancelled;
        
        PromoteFromStandby(flight);
        
        return OperationResult.Ok("Booking cancelled successfully.");
    }
    
    public List<Booking> GetStandbyList(string flightNumber,
        FlightService flightService)
    {
        var flight = flightService.FindFlight(flightNumber);
        return flight?.GetStandbyListOrdered().ToList() ?? new List<Booking>();
    }
    
    private void PromoteFromStandby(Flight flight)
    {
        var earliestStandby = flight.GetStandbyListOrdered().FirstOrDefault();
        if (earliestStandby != null)
        {
            earliestStandby.Status = BookingStatus.Confirmed;
        }
    }
}