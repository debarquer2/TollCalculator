using PublicHoliday;
using TollCalculator.Vehicles;

namespace TollCalculator;

/// <summary>
/// Checks toll free status based on two factors:
/// If a vehicle is toll free based on data in the tollFreeVehicles array.
/// If the date is a weekend or holiday using the PublicHoliday package.
/// </summary>
public class TollFreeStatus
{
    private string[] tollFreeVehicles = [];
    public string TollFreeVehiclesPath { get; set; } = "TollFreeVehicles.txt";
    public string[] TollFreeVehicles { get => tollFreeVehicles; set => tollFreeVehicles = value; }

    /// <summary>
    /// Loads toll free vehicles from the tollFreeVehiclesPath text file.
    /// </summary>
    public void LoadTollFreeVehicles()
    {
        if (!File.Exists(TollFreeVehiclesPath))
        {
            Utilities.LogError($"InitializeCosts error: {TollFreeVehiclesPath} does not exist.");
            return;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(TollFreeVehiclesPath);
        }
        catch (Exception ex)
        {
            Utilities.LogError($"InitializeCosts error exception: {ex.Message}");
            return;
        }

        TollFreeVehicles = lines;
    }

    /// <summary>
    /// Returns the toll free status based on whether the current date is a weekend or national holiday.
    /// </summary>
    /// <param name="date">The date used to calculate the toll free status.</param>
    /// <returns>The toll free status based on whether the current date is a weekend or national holiday.</returns>
    public bool IsTollFreeDate(DateTime date)
    {
        return IsWeekend(date) || IsSwedishHoliday(date);
    }

    /// <summary>
    /// Returns whether or not the date parameter is saturday or sunday.
    /// </summary>
    /// <param name="date">The date used to calculate the toll free status.</param>
    /// <returns>Whether or not the date parameter is saturday or sunday.</returns>
    private bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Returns whether or not the date parameter is a swedish public holiday.
    /// </summary>
    /// <param name="date">The date used to calculate the toll free status.</param>
    /// <returns>Whether or not the date parameter is a swedish public holiday.</returns>
    private bool IsSwedishHoliday(DateTime date)
    {
        SwedenPublicHoliday publicHolidays = new();
        return publicHolidays.IsPublicHoliday(date);
    }

    /// <summary>
    /// Returns the toll free status of the vehicle based on the tollFreeVehicles list.
    /// </summary>
    /// <param name="vehicle">The vehicle type used to calculate the toll free status.</param>
    /// <returns>The toll free status of the vehicle based on the tollFreeVehicles list.</returns>
    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if(tollFreeVehicles == null)
        {
            Utilities.LogError("IsTollFreeVehicle error: Please load vehicles before calling this method.");
            return false;
        }
        if (vehicle == null)
        {
            Utilities.LogError("IsTollFreeVehicle error: vehicle is null");
            return false;
        }

        string vehicleType = vehicle.GetVehicleType();
        return TollFreeVehicles.Contains(vehicleType);
    }
}
