using TollCalculator;
using TollCalculator.Vehicles;

namespace TollCalculatorTesting;

[TestClass]
public class TollFreeStatusTests
{
    /// <summary>
    /// Tests all the days of the week, monday through sunday.
    /// Monday-sunday should return false, the rest return true.
    /// </summary>
    [TestMethod]
    public void TestIsWeekend()
    {
        TollFreeStatus tollFreeStatus = new ();

        DateTime monday = new DateTime(2024, 09, 09);
        DateTime tuesday = new DateTime(2024, 09, 10);
        DateTime wednesday = new DateTime(2024, 09, 11);
        DateTime thursday = new DateTime(2024, 09, 12);
        DateTime friday = new DateTime(2024, 09, 13);
        DateTime saturday = new DateTime(2024, 09, 14);
        DateTime sunday = new DateTime(2024, 09, 15);

        bool isWeekend = 
            !tollFreeStatus.IsTollFreeDate(monday) &&
            !tollFreeStatus.IsTollFreeDate(tuesday) &&
            !tollFreeStatus.IsTollFreeDate(wednesday) &&
            !tollFreeStatus.IsTollFreeDate(thursday) &&
            !tollFreeStatus.IsTollFreeDate(friday) &&
            tollFreeStatus.IsTollFreeDate(saturday) &&
            tollFreeStatus.IsTollFreeDate(sunday);

        Assert.IsTrue(isWeekend);
    }

    /// <summary>
    /// Tests a set of holidays.
    /// They should all evaluate to true.
    /// </summary>
    [TestMethod]
    public void TestHolidays()
    {
        TollFreeStatus tollFreeStatus = new();

        DateTime newYearsDay = new DateTime(2024, 1, 1);
        DateTime thirteenDay = new DateTime(2024, 1, 6);
        DateTime longFriday = new DateTime(2024, 3, 29);
        DateTime easterDay = new DateTime(2024, 3, 31);
        DateTime easterOther = new DateTime(2024, 4, 1);
        DateTime firstMay = new DateTime(2024, 5, 1);
        DateTime ascensionDay = new DateTime(2024, 5, 9);
        DateTime pingst = new DateTime(2024, 5, 19);
        DateTime nationalDay = new DateTime(2024, 6, 6);
        DateTime midsummerDay = new DateTime(2024, 6, 22);
        DateTime allHallowsEve = new DateTime(2024, 11, 2);
        DateTime christmasDay = new DateTime(2024, 12, 25);
        DateTime christmasOther = new DateTime(2024, 12, 26);

        bool isHoliday = (
            tollFreeStatus.IsTollFreeDate(newYearsDay) &&
            tollFreeStatus.IsTollFreeDate(thirteenDay) &&
            tollFreeStatus.IsTollFreeDate(longFriday) &&
            tollFreeStatus.IsTollFreeDate(easterDay) &&
            tollFreeStatus.IsTollFreeDate(easterOther) &&
            tollFreeStatus.IsTollFreeDate(firstMay) &&
            tollFreeStatus.IsTollFreeDate(ascensionDay) &&
            tollFreeStatus.IsTollFreeDate(pingst) &&
            tollFreeStatus.IsTollFreeDate(nationalDay) &&
            tollFreeStatus.IsTollFreeDate(midsummerDay) &&
            tollFreeStatus.IsTollFreeDate(allHallowsEve) &&
            tollFreeStatus.IsTollFreeDate(christmasDay) &&
            tollFreeStatus.IsTollFreeDate(christmasOther));

        Assert.IsTrue(isHoliday);
    }

    /// <summary>
    /// Tests a non-holiday day.
    /// Should evaluate to false.
    /// </summary>
    [TestMethod]
    public void TestHolidayFail()
    {
        TollFreeStatus tollFreeStatus = new();
        DateTime dateTime = new DateTime(2024, 12, 20);

        bool isHoliday = tollFreeStatus.IsTollFreeDate(dateTime);

        Assert.IsFalse(isHoliday);
    }

    /// <summary>
    /// Checks the IsTollFreeVehicle on all the toll free vehicle types.
    /// </summary>
    [TestMethod]
    public void TestIsTollFreeVehicle()
    {
        TollFreeStatus tollFreeStatus = new();
        tollFreeStatus.LoadTollFreeVehicles();

        Vehicle[] vehicles = {
            new Diplomat(),
            new Emergency(),
            new Foreign(),
            new Military(),
            new Tractor(),
            new Motorbike()
        };

        bool isTollFreeVehicle = true;
        foreach (Vehicle vehicle in vehicles)
        {
            if(!tollFreeStatus.IsTollFreeVehicle(vehicle))
            {
                isTollFreeVehicle = false;
                break;
            }
        }

        Assert.IsTrue(isTollFreeVehicle);
    }

    /// <summary>
    /// Checks the toll free status on a non-toll free vehicle type.
    /// Should evaluate to false.
    /// </summary>
    [TestMethod]
    public void TestIsTollFreeVehicleFail()
    {
        TollFreeStatus tollFreeStatus = new();
        tollFreeStatus.LoadTollFreeVehicles();

        var nonTollFreeVehicle = new Car();
        bool isTollFreeVehicle = tollFreeStatus.IsTollFreeVehicle(nonTollFreeVehicle);

        Assert.IsFalse(isTollFreeVehicle);
    }

    /// <summary>
    /// Attempts to load vehicles from file and checks if any are loaded in.
    /// </summary>
    [TestMethod]
    public void TestLoadVehicles()
    {
        TollFreeStatus tollFreeStatus = new();

        tollFreeStatus.LoadTollFreeVehicles();

        Assert.IsTrue(tollFreeStatus.TollFreeVehicles != null && tollFreeStatus.TollFreeVehicles.Length > 0);
    }

    /// <summary>
    /// Attempts to load vehicles from an invalid url and checks if any are loaded in.
    /// Should evaluate to false.
    /// </summary>
    [TestMethod]
    public void TestLoadVehiclesFail()
    {
        TollFreeStatus tollFreeStatus = new();

        tollFreeStatus.TollFreeVehiclesPath = "";
        tollFreeStatus.LoadTollFreeVehicles();

        Assert.IsFalse(tollFreeStatus.TollFreeVehicles != null && tollFreeStatus.TollFreeVehicles.Length > 0);
    }
}