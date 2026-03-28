using ConsoleApp1.Services;
using ConsoleApp1.UI;

var penaltyCalculator = new PenaltyCalculator();
var equipmentService = new EquipmentService();
var userService = new UserService();
var rentalService = new RentalService(penaltyCalculator);
var reportService = new ReportService(equipmentService, rentalService, userService);

var ui = new ConsoleUI(equipmentService, userService, rentalService, reportService);
ui.RunDemo();