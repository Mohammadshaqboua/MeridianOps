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

public class GateService
{
    private readonly List<Gate> _gates = new();
    private int _gateCounter = 1;

    public OperationResult RegisterGate(bool supportsInternational)
    {
        string gateId = $"G-{_gateCounter++}";

        var gate = new Gate()
        {
            GateId = gateId,
            SupportsInternational = supportsInternational
        };
       
        _gates.Add(gate);
        return OperationResult.Ok($"Gate registered successfully with ID {gateId}.");
    }

    public OperationResult AssignGate(string flightNumber,
        string gateId,
        FlightService flightService)
    {
        var flight = flightService.FindFlight(flightNumber);
        if (flight == null)
        {
            return OperationResult.Fail("Flight not found.");
        }

        var gate = FindGate(gateId);
        if (gate == null)
        {
            return OperationResult.Fail("Gate not found.");
        }
        
        if(flight.Type == FlightType.International && !gate.SupportsInternational)
        {
            return OperationResult.Fail($"Gate {gate.GateId} does not support international flights.");
        }

        if (!gate.IsAvailable(flight.ArrivalTime, flight.DepartureTime))
        {
            return OperationResult.Fail($"Gate {gate.GateId} is already occupied during this time window.");
        }
        
        gate.AssignedFlights.Add(flight);
        flight.AssignedGate =  gate;
        
        return OperationResult.Ok($"Flight {flight.FlightNumber} assigned to gate {gate.GateId} successfully.");
    }

    public Gate? FindGate(string gateId)
    {
        return _gates.FirstOrDefault(g => g.GateId == gateId);
    }
}