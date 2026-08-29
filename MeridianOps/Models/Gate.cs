/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

namespace MeridianOps.Models;

public class Gate
{
    public string       GateId                { get; set; }
    public bool         SupportsInternational { get; set; }
    public List<Flight> AssignedFlights       { get; } = new();

    public bool IsAvailable(DateTime start, DateTime end)
    {
        return !AssignedFlights.Any(flight =>
            flight.ArrivalTime < end && start < flight.DepartureTime);
    }
    
}