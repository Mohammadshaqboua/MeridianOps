/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

namespace MeridianOps.Common;

public class InputValidator
{
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                return result;
            }

            Console.WriteLine("Invalid input. Please enter a whole number.");
        }
    }

  public static double ReadDouble(string prompt)
  {
      while (true)
      {
          Console.Write(prompt);
          string? input = Console.ReadLine();

          if (double.TryParse(input, out double result))
          {
              return result;
          }

          Console.WriteLine("Invalid input. Please enter a number (e.g. 12.5).");
      }
  }
  
  public static DateTime ReadDateTime(string prompt)
  {
      while (true)
      {
          Console.Write(prompt);
          string? input = Console.ReadLine();

          if (DateTime.TryParse(input, out DateTime result))
          {
              return result;
          }

          Console.WriteLine("Invalid input. Please enter a valid date and time (e.g. 2026-08-30 14:00).");
      }
  }

  public static string ReadNonEmptyString(string prompt)
  {
      while (true)
      {
          Console.Write(prompt);
          string? input = Console.ReadLine();

          if (!string.IsNullOrEmpty(input))
          {
              return input;
          }

          Console.WriteLine("Invalid input. This field cannot be empty.");
      }
  }
}