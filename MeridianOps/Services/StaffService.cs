/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Common;
using MeridianOps.Models;

namespace MeridianOps.Services;

public class StaffService
{
      private readonly List<GroundStaff> _groundStaff = new();
      private int _staffCounter = 1;
         
      public OperationResult RegisterStaff(string name)
      {
          string staffId = $"S-{_staffCounter++}";

          var newGroundStaff = new GroundStaff()
          {
              StaffId = staffId,
              Name = name
          };
          
          _groundStaff.Add(newGroundStaff);
          return OperationResult.Ok($"Staff member registered successfully with ID {staffId}.");
      }

      public OperationResult AssignStaff(string staffId,
          Flight? flight,
          Gate? gate,
          double durationHours)
      {
          var staff = _groundStaff.FirstOrDefault(staff => staff.StaffId == staffId);
          if (staff == null)
          {
              return OperationResult.Fail("Staff member not found.");
          }

          double totalHoursWorked = staff.GetTotalHoursWorked() + durationHours;
          if (totalHoursWorked > AppConfig.MaxDutyHours)
          {
              return OperationResult.Fail($"Cannot assign: this would bring {staff.Name}'s total duty hours to {totalHoursWorked}, exceeding the maximum of {AppConfig.MaxDutyHours} hours.");
          }
          
          var newAssignment = new Assignment()
          {
              Staff =  staff,
              Flight = flight,
              Gate = gate,
              DurationHours = durationHours
          };
          
          staff.Assignments.Add(newAssignment);
          return OperationResult.Ok("Staff assigned successfully.");
      }

      public double GetCumulativeHours(string staffId)
      {
          var staff = _groundStaff.FirstOrDefault(staff => staff.StaffId == staffId);
          if (staff == null)
          {
              return 0;
          }

          return staff.GetTotalHoursWorked();
      }
}