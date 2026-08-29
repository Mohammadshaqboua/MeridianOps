/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Enums;

namespace MeridianOps.Common;

public class AppConfig
{
    public const int    MinConnectionMinutes = 45;
    public const double MaxDutyHours         = 8;
    public const int    StandbyCapacity      = 10;

    public static double BaggageAllowanceByCategory(PassengerCategory category)
    {
        return category switch
        {
            PassengerCategory.Standard => 30,
            PassengerCategory.VIP => 40,
            PassengerCategory.ReducedMobility => 35,
            _ => 30
        };
    }
}