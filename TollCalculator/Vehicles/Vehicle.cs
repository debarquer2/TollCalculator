namespace TollCalculator.Vehicles;

/// <summary>
/// Stores the type of vehicle used to determine toll free status.
/// </summary>
public interface Vehicle
{
    string GetVehicleType();
}