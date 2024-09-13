using TollCalculator.Vehicles;

namespace TollCalculatorTesting;

[TestClass]
public class TollCalculatorTests
{
    // The price file for reference
    // Start hr Start minute End hour End minute Price
    //0 0 6 0 0
    //6 0 6 30 8
    //6 30 7 0 13
    //7 0 8 0 18
    //8 0 8 30 13
    //8 30 15 0 8
    //15 0 15 30 13
    //15 30 17 0 18
    //17 0 18 0 13
    //18 0 18 30 8
    //18 30 24 0 0

    /// <summary>
    /// Checks if tollCalculator.GetTollFee (one date) returns the correct fee.
    /// </summary>
    [TestMethod]
    public void TestGetTollFeeForDate()
    {
        TollCalculator.TollCalculator tollCalculator = new ();

        Vehicle vehicle = new Car();
        DateTime date = new DateTime(2024, 9, 11, 8, 15, 0);

        int tollFee = tollCalculator.GetTollFee(date, vehicle);

        Assert.IsTrue (tollFee == 13);
    }

    /// <summary>
    /// Checks if tollCalculator.GetTollFee (multiple dates) returns the correct fee.
    /// </summary>
    [TestMethod]
    public void TestGetTollFeeForDates()
    {
        TollCalculator.TollCalculator tollCalculator = new();

        Vehicle vehicle = new Car();
        DateTime[] dates = {
            new DateTime(2024, 9, 11, 15, 45, 0),
            new DateTime(2024, 9, 11, 14, 45, 0),
            new DateTime(2024, 9, 11, 15, 15, 0),
            new DateTime(2024, 9, 11, 14, 30, 0),
        };

        int tollFee = tollCalculator.GetTollFee(vehicle, dates);

        Assert.IsTrue(tollFee == 13 + 18);
    }
}

