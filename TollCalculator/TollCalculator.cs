using TollCalculator.Vehicles;

namespace TollCalculator;

/// <summary>
/// Calculates toll fees based on dates and vehicle types.
/// </summary>
public class TollCalculator
{
    /// <summary>
    /// Stores the price between two timespans.
    /// For example: 8:30 to 15:00 Price:8
    /// </summary>
    private struct TollTimeSpan()
    {
        public TimeSpan Start;
        public TimeSpan End;
        public int Price = 0;
    }

    private readonly string tollPricesPath = "TollPrices.txt";
    private List<TollTimeSpan> tollTimeSpans = [];
    private TollFreeStatus tollFreeStatus = new();
    private const int minutesInADay = 24 * 60;

    /// <summary>
    /// Default constructor
    /// </summary>
    public TollCalculator()
    {
        tollFreeStatus.LoadTollFreeVehicles();
        InitializePrices();
    }

    /// <summary>
    /// Constructor with parameter for the path to the toll prices text file.
    /// </summary>
    /// <param name="tollPricesPath">The path to the toll prices text file.</param>
    public TollCalculator(string tollPricesPath)
    {
        this.tollPricesPath = tollPricesPath;

        tollFreeStatus.LoadTollFreeVehicles();
        InitializePrices();
    }

    /// <summary>
    /// Constructor with parameter for the path to the toll prices text file and the path to the toll free vehicles text file.
    /// </summary>
    /// <param name="tollPricesPath">The path to the toll prices text file.</param>
    /// <param name="tollFreeVehiclesPath">The path to the toll free vehicles text file.</param>
    public TollCalculator(string tollPricesPath, string tollFreeVehiclesPath)
    {
        this.tollPricesPath = tollPricesPath;
        tollFreeStatus.TollFreeVehiclesPath = tollFreeVehiclesPath;

        tollFreeStatus.LoadTollFreeVehicles();
        InitializePrices();
    }

    /// <summary>
    /// Constructor with parameter for the path to the toll prices text file and an array of the toll free vehicles.
    /// </summary>
    /// <param name="tollPricesPath">The path to the toll prices text file.</param>
    /// <param name="tollFreeVehicles">An array of the toll free vehicles.</param>
    public TollCalculator(string tollPricesPath, string[] tollFreeVehicles)
    {
        this.tollPricesPath = tollPricesPath;
        tollFreeStatus.TollFreeVehicles = tollFreeVehicles;

        InitializePrices();
    }

    /// <summary>
    /// Initializes the various (surge) prices throughout the day based on the data from the tollPricesPath text file.
    /// Validates the loaded data to verify that all 1440 minutes are accounted for.
    /// </summary>
    private void InitializePrices()
    {
        if (!File.Exists(tollPricesPath))
        {
            Utilities.LogError($"InitializeCosts error: {tollPricesPath} does not exist.");
            return;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(tollPricesPath);
        }
        catch (Exception ex)
        {
            Utilities.LogError($"InitializeCosts error exception: {ex.Message}");
            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            // Check for comments
            if (lines[i][0] == '/' && lines[i][1] == '/') continue;

            string[] split = lines[i].Split(" ");

            int startHours = int.Parse(split[0]);
            int startMinutes = int.Parse(split[1]);

            int endHours = int.Parse(split[2]);
            int endMinutes = int.Parse(split[3]);

            int price = int.Parse(split[4]);

            TimeSpan start = new TimeSpan(startHours, startMinutes, 0);
            TimeSpan end = new TimeSpan(endHours, endMinutes, 0);

            tollTimeSpans.Add(new TollTimeSpan() { Start = start, End = end, Price = price });
        }

        // Toll prices validation. The total minutes have to add up to 1440 (the total amount of minutes in a day).
        double minutes = 0;
        foreach (TollTimeSpan t in tollTimeSpans)
        {
            minutes += (t.End - t.Start).TotalMinutes;
        }
        Console.WriteLine($"Total minutes: {minutes} out of {minutesInADay}");

        if (minutes == minutesInADay)
        {
            Console.WriteLine("TollPrices validated successfully.");
        }
        else
        {
            Utilities.LogError("InitializeCosts error: TollPrices failed vaildation");
        }
    }

    /// <summary>
    /// Returns the total toll fee for a specific date and time.
    /// Checks toll free status and applies per hour fee rules.    
    /// </summary>
    /// <param name="date">The date used to calculate the toll fee.</param>
    /// <param name="vehicle">The vehicle type used to calculate the toll fee.</param>
    /// <returns>The total toll fee for the selected date and time.</returns>
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (tollFreeStatus.IsTollFreeDate(date) || tollFreeStatus.IsTollFreeVehicle(vehicle)) return 0;

        TimeSpan timespan = new TimeSpan(date.Hour, date.Minute, 0);

        for (int i = 0; i < tollTimeSpans.Count; i++)
        {
            if ((timespan > tollTimeSpans[i].Start) && (timespan < tollTimeSpans[i].End))
            {
                return tollTimeSpans[i].Price;
            }
        }

        Utilities.LogError($"GetTollFee error: Unable to find price at time {date.Hour}:{date.Minute}");
        return 0;
    }

    /// <summary>
    /// Returns the total toll fee from a number of dates.
    /// Checks toll free status and applies per hour fee rules.
    /// </summary>
    /// <param name="vehicle">The vehicle type used to calculate the toll fee.</param>
    /// <param name="datesSorted">The dates used to calculate the toll fee.</param>
    /// <returns>The total toll fee from a number of dates.</returns>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (tollFreeStatus.IsTollFreeVehicle(vehicle)) return 0;

        DateTime[] datesSorted = dates.Order().ToArray();

        DateTime lastDate = datesSorted[0];
        int totalFee = 0;
        int lastFee = GetTollFee(lastDate, vehicle);

        // Skip the first since it is already handled above.
        for (int i = 1; i < datesSorted.Length; i++)
        {
            DateTime date = datesSorted[i];

            var minutes = (date - lastDate).TotalMinutes;
            if (minutes <= 60)
            {
                lastFee = Math.Max(lastFee, GetTollFee(date, vehicle));
            }
            else
            {
                totalFee += lastFee;
                lastFee = GetTollFee(date, vehicle);
                lastDate = date;
            }
        }

        totalFee += lastFee;

        return Math.Min(totalFee, 60);
    }
}