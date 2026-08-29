/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

namespace MeridianOps.Models;

public class Assignment
{
    public GroundStaff Staff         { get; set; }
    public Flight?     Flight        { get; set; }     
    public Gate?       Gate          { get; set; }         
    public double      DurationHours { get; set; }
}