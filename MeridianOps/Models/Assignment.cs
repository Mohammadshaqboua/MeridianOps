namespace MeridianOps.Models;

public class Assignment
{
    public GroundStaff Staff { get; set; }
    public Flight? Flight { get; set; }     //Nullable
    public Gate? Gate { get; set; }         //Nullable
    public double DurationHours { get; set; }
}