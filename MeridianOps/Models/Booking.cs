/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

using MeridianOps.Enums;

namespace MeridianOps.Models;

public class Booking
{
    public Passenger     Passenger   { get; set; }
    public Flight        Flight      { get; set; }
    public BookingStatus Status      { get; set; }
    public DateTime      BookingTime { get; set; } =  DateTime.Now;
}