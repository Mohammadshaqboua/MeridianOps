/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

namespace MeridianOps.Models;

public class GroundStaff
{
    public string           StaffId     { get; set; }
    public string           Name        { get; set; }
    public List<Assignment> Assignments { get; } = new();

    public double GetTotalHoursWorked()
    {
        return Assignments
            .Sum(assignment => assignment.DurationHours);
    }
}