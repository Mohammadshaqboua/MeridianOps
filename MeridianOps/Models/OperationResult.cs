/*
 ===============================================================
  MeridianOps - Airport Ground Operations Management System
  Author: Mohammad Shaqboua
  GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
 ===============================================================
*/

namespace MeridianOps.Models;

public class OperationResult
{
    public bool   Success { get; }
    public string Message { get; }

    private OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    public static OperationResult Ok(string message)
    {
        return new OperationResult(true, message);
    }

    public static OperationResult Fail(string reason)
    {
        return new OperationResult(false, reason);
    }
}