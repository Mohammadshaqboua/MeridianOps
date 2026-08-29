/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Enums;

namespace MeridianOps.Models;

public class Passenger
{
    public string            PassengerId      { get; set; }
    public string            Name             { get; set; }
    public PassengerCategory Category         { get; set; }
    public Flight?           ConnectingFlight { get; set; } 
    public List<Baggage>     BaggageItems     { get; }      = new();

    public double GetTotalBaggageWeight(Flight flight)
    {
        return BaggageItems
            .Where(baggage => baggage.Flight == flight)
            .Sum(baggage => baggage.WeightKg);
    }

    public bool IsConnectingPassenger()
    {
        return  ConnectingFlight != null;
    }
}