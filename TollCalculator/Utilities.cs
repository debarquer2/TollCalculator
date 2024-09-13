namespace TollCalculator
{
    internal static class Utilities
    {
        /// <summary>
        /// Prints the message parameter to the console and to log.txt 
        /// </summary>
        /// <param name="message">The error message.</param>
        public static void LogError(string message)
        {
            Console.WriteLine(message);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine($"{DateTime.Now.ToString()}: {message}");
            }
        }
    }
}
