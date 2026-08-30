using MeridianOps.Services;
using MeridianOps.UI;

var flightService    = new FlightService();
var gateService      = new GateService();
var passengerService = new PassengerService();
var baggageService   = new BaggageService();
var bookingService   = new BookingService();
var staffService     = new StaffService();

var menu = new ConsoleMenu(flightService,
    gateService,
    passengerService,
    baggageService,
    bookingService,
    staffService);

menu.Run();