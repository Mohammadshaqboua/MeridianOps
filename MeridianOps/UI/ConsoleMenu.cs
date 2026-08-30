/*
  ===============================================================
   MeridianOps - Airport Ground Operations Management System
   Author: Mohammad Shaqboua
   GITHUB: https://github.com/Mohammadshaqboua/MeridianOps.git
  ===============================================================
*/

using MeridianOps.Common;
using MeridianOps.Enums;
using MeridianOps.Models;
using MeridianOps.Services;

namespace MeridianOps.UI;

public class ConsoleMenu
{
    private readonly FlightService _flightService;
    private readonly GateService _gateService;
    private readonly PassengerService _passengerService;
    private readonly BaggageService _baggageService;
    private readonly BookingService _bookingService;
    private readonly StaffService _staffService;

    public ConsoleMenu(
        FlightService flightService,
        GateService gateService,
        PassengerService passengerService,
        BaggageService baggageService,
        BookingService bookingService,
        StaffService staffService)
    {
        _flightService = flightService;
        _gateService = gateService;
        _passengerService = passengerService;
        _baggageService = baggageService;
        _bookingService = bookingService;
        _staffService = staffService;
    }

    private const string Line = "========================================================";
    private const string SubLine = "--------------------------------------------------------";

    private void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(Line);
        Console.WriteLine($"                 {title}");
        Console.WriteLine(Line);
        Console.WriteLine();
    }

    private void PrintSuccess(string message)
    {
        Console.WriteLine(SubLine);
        Console.WriteLine($"[ SUCCESS ] {message}");
        Console.WriteLine(SubLine);
    }

    private void PrintError(string message)
    {
        Console.WriteLine(SubLine);
        Console.WriteLine($"[ ERROR   ] {message}");
        Console.WriteLine(SubLine);
    }

    private void PrintWarning(string message)
    {
        Console.WriteLine(SubLine);
        Console.WriteLine($"[ WARNING ] {message}");
        Console.WriteLine(SubLine);
    }

    private void PrintInfo(string message)
    {
        Console.WriteLine($"[ INFO    ] {message}");
    }

    private void PrintResult(OperationResult result, string successMessage, string errorMessage)
    {
        if (result.Success)
        {
            PrintSuccess(successMessage);
            PrintInfo(result.Message);
        }
        else
        {
            PrintError(errorMessage);
            Console.WriteLine($"[ REASON  ] {result.Message}");
            Console.WriteLine(SubLine);
        }
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(Line);
            Console.WriteLine("          MERIDIAN TERMINAL - GROUND OPERATIONS");
            Console.WriteLine(Line);
            Console.WriteLine();
            Console.WriteLine("  [1]  Register Flight");
            Console.WriteLine("  [2]  Assign Gate");
            Console.WriteLine("  [3]  Register Passenger");
            Console.WriteLine("  [4]  Check Boarding Eligibility");
            Console.WriteLine("  [5]  Baggage Management");
            Console.WriteLine("  [6]  Booking & Standby Management");
            Console.WriteLine("  [7]  Assign Staff");
            Console.WriteLine("  [8]  Register Gate");
            Console.WriteLine("  [9]  Register Staff");
            Console.WriteLine("  [10] Update Flight Status");
            Console.WriteLine("  [11] Exit");
            Console.WriteLine();
            Console.WriteLine(Line);

            int choice = InputValidator.ReadInt("Select an option: ");

            switch (choice)
            {
                case 1:
                    HandleRegisterFlight();
                    break;
                case 2:
                    HandleAssignGate();
                    break;
                case 3:
                    HandleRegisterPassenger();
                    break;
                case 4:
                    HandleCheckBoardingEligibility();
                    break;
                case 5:
                    HandleBaggageMenu();
                    break;
                case 6:
                    HandleBookingMenu();
                    break;
                case 7:
                    HandleAssignStaff();
                    break;
                case 8:
                    HandleRegisterGate();
                    break;
                case 9:
                    HandleRegisterStaff();
                    break;
                case 10:
                    HandleUpdateFlightStatus();
                    break;
                case 11:
                    Console.Clear();
                    Console.WriteLine(Line);
                    Console.WriteLine("              MERIDIANOPS SHUTTING DOWN");
                    Console.WriteLine(Line);
                    Console.WriteLine();
                    Console.WriteLine("Thank you for using MeridianOps.");
                    Console.WriteLine();
                    return;
                default:
                    PrintError("Invalid menu option. Please select a number from 1 to 11.");
                    Pause();
                    break;
            }

            if (choice >= 1 && choice <= 10)
                Pause();
        }
    }

    private void Pause()
    {
        Console.WriteLine();
        Console.Write("Press ENTER to return to the main menu...");
        Console.ReadLine();
    }

    // ---------------- Flights ----------------

    private void HandleRegisterFlight()
    {
        Console.Clear();
        
        PrintHeader("REGISTER FLIGHT");

        Console.WriteLine("Flight Type");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Domestic");
        Console.WriteLine("2. International");
        Console.WriteLine();

        int typeChoice = InputValidator.ReadInt("Select flight type: ");

        if (typeChoice != 1 && typeChoice != 2)
        {
            PrintError("Invalid flight type. Please select 1 or 2.");
            return;
        }

        FlightType type = typeChoice == 2
            ? FlightType.International
            : FlightType.Domestic;

        Console.WriteLine();

        DateTime arrivalTime = InputValidator.ReadDateTime(
            "Arrival time (YYYY-MM-DD HH:mm): ");

        DateTime departureTime = InputValidator.ReadDateTime(
            "Departure time (YYYY-MM-DD HH:mm): ");

        int seatCapacity = InputValidator.ReadInt("Seat capacity: ");

        Console.WriteLine();
        Console.WriteLine("Registering flight...");

        var result = _flightService.RegisterFlight(
            type,
            arrivalTime,
            departureTime,
            seatCapacity);

        PrintResult(
            result,
            "Flight registered successfully.",
            "Flight registration failed.");
    }

    private void HandleUpdateFlightStatus()
    {
        Console.Clear();
        
        PrintHeader("UPDATE FLIGHT STATUS");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        Console.WriteLine();
        Console.WriteLine("Available Statuses");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Scheduled");
        Console.WriteLine("2. Delayed");
        Console.WriteLine("3. Boarding");
        Console.WriteLine("4. Departed");
        Console.WriteLine("5. Cancelled");
        Console.WriteLine();

        int statusChoice = InputValidator.ReadInt("Select new status: ");

        FlightStatus newStatus;

        switch (statusChoice)
        {
            case 1:
                newStatus = FlightStatus.Scheduled;
                break;
            case 2:
                newStatus = FlightStatus.Delayed;
                break;
            case 3:
                newStatus = FlightStatus.Boarding;
                break;
            case 4:
                newStatus = FlightStatus.Departed;
                break;
            case 5:
                newStatus = FlightStatus.Cancelled;
                break;
            default:
                PrintError("Invalid status selection. Please select 1 to 5.");
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Updating flight status...");

        var result = _flightService.UpdateStatus(
            flightNumber,
            newStatus);

        PrintResult(
            result,
            "Flight status updated successfully.",
            "Failed to update flight status.");
    }

    // ---------------- Gates ----------------

    private void HandleRegisterGate()
    {
        Console.Clear();
        
        PrintHeader("REGISTER GATE");

        Console.WriteLine("International Flight Support");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");
        Console.WriteLine();

        int choice = InputValidator.ReadInt("Select option: ");

        if (choice != 1 && choice != 2)
        {
            PrintError("Invalid selection. Please select 1 or 2.");
            return;
        }

        bool supportsInternational = choice == 1;

        Console.WriteLine();
        Console.WriteLine("Registering gate...");

        var result = _gateService.RegisterGate(supportsInternational);

        PrintResult(
            result,
            "Gate registered successfully.",
            "Gate registration failed.");
    }

    private void HandleAssignGate()
    {
        Console.Clear();
        
        PrintHeader("ASSIGN GATE");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        string gateId = InputValidator.ReadNonEmptyString(
            "Gate ID: ");

        Console.WriteLine();
        Console.WriteLine("Assigning gate...");

        var result = _gateService.AssignGate(
            flightNumber,
            gateId,
            _flightService);

        PrintResult(
            result,
            "Gate assigned successfully.",
            "Gate assignment failed.");
    }

    // ---------------- Passengers ----------------

    private void HandleRegisterPassenger()
    {
        Console.Clear();
        
        PrintHeader("REGISTER PASSENGER");

        string name = InputValidator.ReadNonEmptyString(
            "Passenger name: ");

        Console.WriteLine();
        Console.WriteLine("Passenger Category");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Standard");
        Console.WriteLine("2. VIP");
        Console.WriteLine("3. Reduced Mobility");
        Console.WriteLine();

        int categoryChoice = InputValidator.ReadInt(
            "Select category: ");

        PassengerCategory category;

        switch (categoryChoice)
        {
            case 1:
                category = PassengerCategory.Standard;
                break;
            case 2:
                category = PassengerCategory.VIP;
                break;
            case 3:
                category = PassengerCategory.ReducedMobility;
                break;
            default:
                PrintError("Invalid passenger category. Please select 1 to 3.");
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Connecting Flight");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");
        Console.WriteLine();

        int connectingChoice = InputValidator.ReadInt(
            "Is passenger connecting from another flight? ");
        
        Console.WriteLine();

        if (connectingChoice != 1 && connectingChoice != 2)
        {
            PrintError("Invalid selection. Please select 1 or 2.");
            return;
        }

        Flight? connectingFlight = null;

        if (connectingChoice == 1)
        {
            string connectingFlightNumber =
                InputValidator.ReadNonEmptyString(
                    "Connecting flight number: ");

            connectingFlight =
                _flightService.FindFlight(connectingFlightNumber);

            if (connectingFlight == null)
            {
                PrintWarning("Connecting flight was not found.");
                PrintInfo("Passenger will be registered without a connection.");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Registering passenger...");

        var result = _passengerService.RegisterPassenger(
            name,
            category,
            connectingFlight);

        PrintResult(
            result,
            "Passenger registered successfully.",
            "Passenger registration failed.");
    }

    private void HandleCheckBoardingEligibility()
    {
        Console.Clear();
        
        PrintHeader("BOARDING ELIGIBILITY CHECK");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string nextFlightNumber = InputValidator.ReadNonEmptyString(
            "Next flight number: ");

        Console.WriteLine();
        Console.WriteLine("Checking boarding eligibility...");
        Console.WriteLine();

        var result = _passengerService.CheckBoardingEligibility(
            passengerId,
            nextFlightNumber,
            _flightService);

        if (result.Success)
        {
            Console.WriteLine(SubLine);
            Console.WriteLine("                 BOARDING ALLOWED");
            Console.WriteLine(SubLine);
            PrintInfo(result.Message);
            Console.WriteLine(SubLine);
        }
        else
        {
            Console.WriteLine(SubLine);
            Console.WriteLine("                  BOARDING DENIED");
            Console.WriteLine(SubLine);
            Console.WriteLine($"[ REASON  ] {result.Message}");
            Console.WriteLine(SubLine);
        }
    }

    // ---------------- Baggage ----------------

    private void HandleBaggageMenu()
    {
        Console.Clear();
        
        PrintHeader("BAGGAGE MANAGEMENT");

        Console.WriteLine("1. Register Baggage");
        Console.WriteLine("2. View Cumulative Baggage Weight");
        Console.WriteLine("3. Return to Main Menu");
        Console.WriteLine();

        int choice = InputValidator.ReadInt("Select an option: ");

        switch (choice)
        {
            case 1:
                HandleRegisterBaggage();
                break;
            case 2:
                HandleViewBaggageWeight();
                break;
            case 3:
                return;
            default:
                PrintError("Invalid baggage menu option. Please select 1 to 3.");
                break;
        }
    }

    private void HandleRegisterBaggage()
    {
        Console.Clear();
        
        PrintHeader("REGISTER BAGGAGE");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        double weightKg = InputValidator.ReadDouble(
            "Baggage weight (kg): ");

        Console.WriteLine();
        Console.WriteLine("Processing baggage...");

        var result = _baggageService.RegisterBaggage(
            passengerId,
            flightNumber,
            weightKg,
            _passengerService,
            _flightService);

        if (result.Success)
        {
            Console.WriteLine(SubLine);
            Console.WriteLine("                 BAGGAGE ACCEPTED");
            Console.WriteLine(SubLine);
            PrintInfo(result.Message);
            Console.WriteLine(SubLine);
        }
        else
        {
            Console.WriteLine(SubLine);
            Console.WriteLine("                 BAGGAGE REJECTED");
            Console.WriteLine(SubLine);
            Console.WriteLine($"[ REASON  ] {result.Message}");
            Console.WriteLine(SubLine);
        }
    }

    private void HandleViewBaggageWeight()
    {
        Console.Clear();
        
        PrintHeader("CUMULATIVE BAGGAGE WEIGHT");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        double total = _baggageService.GetTotalWeight(
            passengerId,
            flightNumber,
            _passengerService,
            _flightService);

        Console.WriteLine();
        Console.WriteLine(SubLine);
        Console.WriteLine($"Passenger ID : {passengerId}");
        Console.WriteLine($"Flight       : {flightNumber}");
        Console.WriteLine($"Total Weight : {total:F2} kg");
        Console.WriteLine(SubLine);
    }

    // ---------------- Bookings & Standby ----------------

    private void HandleBookingMenu()
    {
        Console.Clear();
        
        PrintHeader("BOOKING & STANDBY MANAGEMENT");

        Console.WriteLine("1. Book Passenger");
        Console.WriteLine("2. Cancel Booking");
        Console.WriteLine("3. View Standby List");
        Console.WriteLine("4. Process Boarding ");
        Console.WriteLine("5. Return to Main Menu");
        Console.WriteLine();

        int choice = InputValidator.ReadInt("Select an option: ");

        switch (choice)
        {
            case 1:
                HandleBookPassenger();
                break;
            case 2:
                HandleCancelBooking();
                break;
            case 3:
                HandleViewStandbyList();
                break;
            case 4:
                HandleProcessBoarding();
                break;
            case 5:
                return;
            default:
                PrintError("Invalid booking menu option. Please select 1 to 5.");
                break;
        }
    }
    
    private void HandleProcessBoarding()
    {
        Console.Clear();
    
        PrintHeader("PROCESS BOARDING");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        Console.WriteLine();
        Console.WriteLine("Processing boarding...");

        var result = _bookingService.ProcessBoarding(
            passengerId,
            flightNumber,
            _passengerService,
            _flightService);

        PrintResult(
            result,
            "Passenger boarded successfully.",
            "Boarding failed.");
    }

    private void HandleBookPassenger()
    {
        Console.Clear();
        
        PrintHeader("BOOK PASSENGER");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        Console.WriteLine();
        Console.WriteLine("Processing booking...");

        var result = _bookingService.BookPassenger(
            passengerId,
            flightNumber,
            _passengerService,
            _flightService);

        PrintResult(
            result,
            "Passenger booking completed successfully.",
            "Passenger booking failed.");
    }

    private void HandleCancelBooking()
    {
        Console.Clear();
        
        PrintHeader("CANCEL BOOKING");

        string passengerId = InputValidator.ReadNonEmptyString(
            "Passenger ID: ");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        Console.WriteLine();
        Console.WriteLine("Cancelling booking...");

        var result = _bookingService.CancelBooking(
            passengerId,
            flightNumber,
            _passengerService,
            _flightService);

        PrintResult(
            result,
            "Booking cancelled successfully.",
            "Booking cancellation failed.");
    }

    private void HandleViewStandbyList()
    {
        Console.Clear();
        
        PrintHeader("STANDBY LIST");

        string flightNumber = InputValidator.ReadNonEmptyString(
            "Flight number: ");

        var standbyList = _bookingService.GetStandbyList(
            flightNumber,
            _flightService);

        Console.WriteLine();

        if (standbyList.Count == 0)
        {
            Console.WriteLine(SubLine);
            Console.WriteLine("[ INFO ] No passengers are currently on the standby list.");
            Console.WriteLine(SubLine);
            return;
        }

        Console.WriteLine($"Flight: {flightNumber}");
        Console.WriteLine(SubLine);
        Console.WriteLine(
            $"{"Position",-10}{"Passenger",-25}{"Passenger ID",-18}{"Waiting Since"}");
        Console.WriteLine(SubLine);

        int position = 1;

        foreach (var booking in standbyList)
        {
            Console.WriteLine(
                $"{position,-10}" +
                $"{booking.Passenger.Name,-25}" +
                $"{booking.Passenger.PassengerId,-18}" +
                $"{booking.BookingTime}");

            position++;
        }

        Console.WriteLine(SubLine);
    }

    // ---------------- Staff ----------------

    private void HandleRegisterStaff()
    {
        Console.Clear();
        
        PrintHeader("REGISTER STAFF");

        string name = InputValidator.ReadNonEmptyString(
            "Staff name: ");

        Console.WriteLine();
        Console.WriteLine("Registering staff member...");

        var result = _staffService.RegisterStaff(name);

        PrintResult(
            result,
            "Staff member registered successfully.",
            "Staff registration failed.");
    }

    private void HandleAssignStaff()
    {
        Console.Clear();
        
        PrintHeader("ASSIGN STAFF");

        string staffId = InputValidator.ReadNonEmptyString(
            "Staff ID: ");

        Console.WriteLine();
        Console.WriteLine("Assignment Target");
        Console.WriteLine(SubLine);
        Console.WriteLine("1. Flight");
        Console.WriteLine("2. Gate");
        Console.WriteLine();

        int targetChoice = InputValidator.ReadInt(
            "Assign staff to: ");

        Flight? flight = null;
        Gate? gate = null;

        if (targetChoice == 1)
        {
            string flightNumber = InputValidator.ReadNonEmptyString(
                "Flight number: ");

            flight = _flightService.FindFlight(flightNumber);

            if (flight == null)
            {
                PrintError("Flight not found.");
                return;
            }
        }
        else if (targetChoice == 2)
        {
            string gateId = InputValidator.ReadNonEmptyString(
                "Gate ID: ");

            gate = _gateService.FindGate(gateId);

            if (gate == null)
            {
                PrintError("Gate not found.");
                return;
            }
        }
        else
        {
            PrintError("Invalid assignment target. Please select 1 or 2.");
            return;
        }

        Console.WriteLine();

        double durationHours = InputValidator.ReadDouble(
            "Assignment duration (hours): ");

        Console.WriteLine();
        Console.WriteLine("Assigning staff member...");

        var result = _staffService.AssignStaff(
            staffId,
            flight,
            gate,
            durationHours);

        if (result.Success)
        {
            PrintSuccess("Staff assignment completed successfully.");
            PrintInfo(result.Message);

            double totalHours =
                _staffService.GetCumulativeHours(staffId);

            Console.WriteLine();
            Console.WriteLine(
                $"[ SUMMARY ] Total duty hours: {totalHours:F2} hours");
            Console.WriteLine(SubLine);
        }
        else
        {
            PrintError("Staff assignment failed.");
            Console.WriteLine($"[ REASON  ] {result.Message}");
            Console.WriteLine(SubLine);
        }
    }
}