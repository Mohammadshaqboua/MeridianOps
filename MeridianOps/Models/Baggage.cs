namespace MeridianOps.Models;

public class Baggage
{
    public string BaggageId { get; set; }
    public double WeightKg { get; set; }
    public Passenger Owner { get; set; }
    public Flight Flight { get; set; }
}