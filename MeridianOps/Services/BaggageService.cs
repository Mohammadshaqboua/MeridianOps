/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Common;
using MeridianOps.Models;
using MeridianOps.Enums;

namespace MeridianOps.Services;

public class BaggageService
{
    private int _baggageCounter = 1;
    public OperationResult RegisterBaggage(string passengerId,
        string flightNumber,
        double weightKg,
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
        
        if (flight.Status == FlightStatus.Departed || flight.Status == FlightStatus.Cancelled)
        {
            return OperationResult.Fail($"Cannot register baggage: flight is {flight.Status}.");
        }

        double currentTotal = passenger.GetTotalBaggageWeight(flight);
        double allowance = AppConfig.BaggageAllowanceByCategory(passenger.Category);
        if (allowance < (currentTotal + weightKg))
        {
            return OperationResult
                .Fail($"This bag would bring the passenger's total checked baggage to {currentTotal + weightKg}kg, exceeding the {allowance}kg allowance for {passenger.Category} passengers.");
        }

        string baggageId = $"B-{_baggageCounter++}";

        var newBaggage = new Baggage()
        {
            BaggageId = baggageId,
            WeightKg = weightKg,
            Owner =  passenger,
            Flight = flight
        };
        
        passenger.BaggageItems.Add(newBaggage);
        return OperationResult.Ok($"Baggage registered successfully with ID {baggageId}.");
    }

    public double GetTotalWeight(string passengerId,
        string flightNumber,
        PassengerService passengerService,
        FlightService flightService)
    {
        var passenger = passengerService.FindPassenger(passengerId);
        if (passenger == null)
        {
            return 0.0;
        }
        
        var  flight = flightService.FindFlight(flightNumber);
        if (flight == null)
        {
            return 0.0;
        }
        
        return passenger.GetTotalBaggageWeight(flight);
    }
}