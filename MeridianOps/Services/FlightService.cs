/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Enums;
using MeridianOps.Models;

namespace MeridianOps.Services;

public class FlightService
{
   private readonly List<Flight> _flights = new();
   private int _flightCounter = 1;

   public OperationResult RegisterFlight(FlightType type,
       DateTime arrivalTime,
       DateTime departureTime,
       int seatCapacity)
   {
       string flightNumber = $"FL-{_flightCounter++}";
        
       var flight = new Flight()
       {
           FlightNumber = flightNumber,
           Type = type,
           ArrivalTime = arrivalTime,
           DepartureTime = departureTime,
           SeatCapacity = seatCapacity
       };
        
       _flights.Add(flight);
       return OperationResult.Ok($"Flight registered successfully with number {flightNumber}.");
   }

   public OperationResult UpdateStatus(string flightNumber, FlightStatus newStatus)
   {
       var flight = FindFlight(flightNumber);
       
       if (flight == null)                  
       {
           return OperationResult.Fail("Flight not found.");
       }

       if (flight.Status == FlightStatus.Departed || flight.Status == FlightStatus.Cancelled)
       {
           return OperationResult.Fail($"Cannot update status: flight is already {flight.Status}.");
       }
       
       flight.Status = newStatus;  
       return OperationResult.Ok("Flight status updated successfully.");
   }
   
   public Flight? FindFlight(string flightNumber)
   {
       return _flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
   }
   
   public List<Flight> GetAllFlights() => _flights;
}
