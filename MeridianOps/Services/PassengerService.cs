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

public class PassengerService
{
    private readonly List<Passenger> _passengers = new();
    private int _passengerCounter = 1;

    public OperationResult RegisterPassenger(string name,
        PassengerCategory category,
        Flight? connectingFlight)
    {
        string passengerId = $"P-{_passengerCounter++}";

        var passenger = new Passenger()
        {
            PassengerId =  passengerId,
            Name = name,
            Category = category,
            ConnectingFlight =  connectingFlight
        };
        
        _passengers.Add(passenger);
        return OperationResult.Ok($"Passenger registered successfully with ID {passengerId}.");
    }

    public OperationResult CheckBoardingEligibility(string passengerId,
        string nextFlightNumber,
        FlightService flightService)
    {
        var passenger = FindPassenger(passengerId);
        if (passenger == null)
        {
            return OperationResult.Fail("Passenger not found.");
        }

        var flight = flightService.FindFlight(nextFlightNumber);
        if (flight == null)
        {
            return OperationResult.Fail("Flight not found.");
        }
        
        if(flight.Status == FlightStatus.Cancelled || flight.Status == FlightStatus.Departed)
        {
            return OperationResult.Fail($"Cannot board: flight is {flight.Status}.");
        }
        
        if(passenger.IsConnectingPassenger())
        {
            double minutesRemaining = (flight.DepartureTime - passenger.ConnectingFlight.ArrivalTime).TotalMinutes;
            if (minutesRemaining < AppConfig.MinConnectionMinutes)
            {
                return OperationResult.Fail($"Only {(int)minutesRemaining} minutes remain since the connecting flight's arrival; the minimum connection time is {AppConfig.MinConnectionMinutes} minutes.");
            }
        }

        return OperationResult.Ok("Passenger is eligible to board.");
    }
    
    public Passenger? FindPassenger(string passengerId)
    {
        return _passengers.FirstOrDefault(passenger => passenger.PassengerId == passengerId);
    }
}