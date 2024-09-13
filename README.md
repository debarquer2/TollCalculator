# Toll Calculator
A toll calculator program based on https://github.com/afry-recruitment/toll-calculator.

## Vehicles
Vehicles classes describing toll free vehicles. Remains unchanged for compatibility reasons.

## TollCalculator
Some code moved to TollFreeStatus. Some internal data moved to external text files.

GetTollFee(Vehicle vehicle, DateTime[] dates) was reworked and optimized. It should now better handle many dates where the time between each date is inconsistently more than or less than one hour.
Toll pricing timetable has been moved to an external file to facilitate changes.
The new toll pricing time table system can be iterated through, simplifying GetTollFee(DateTime date, Vehicle vehicle) and reduces the risk of coding errors.

## TollFreeStatus
New class to handle the toll-free status of vehicles and dates.

Toll free vehicles has been moved to an external file and loaded into an array that can be iterated through. This significantly simplifies the IsTollFreeVehicle and reduces the risk of coding errors.

Holiday calculations have been replaced with the PublicHoliday package.
This reduces the risk of coding errors and reduces maintenance needs (we no longer need to update the holidays section every year).