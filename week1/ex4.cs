using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialForecasting
{
    class Program
    {
        // Function to calculate moving average forecast
        static List<double> MovingAverageForecast(List<double> data, int windowSize)
        {
            List<double> forecast = new List<double>();

            for (int i = 0; i <= data.Count - windowSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < windowSize; j++)
                {
                    sum += data[i + j];
                }
                forecast.Add(sum / windowSize);
            }

            return forecast;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Financial Forecasting (Using Moving Average)");
            Console.WriteLine("---------------------------------------------");

            // Sample monthly revenue data (e.g., last 12 months)
            List<double> revenueData = new List<double> { 
                12000, 13500, 12800, 14000, 15000, 16000, 
                15800, 17000, 16500, 17500, 18000, 19000 
            };

            Console.WriteLine("Monthly Revenue Data:");
            for (int i = 0; i < revenueData.Count; i++)
            {
                Console.WriteLine($"Month {i + 1}: ₹{revenueData[i]}");
            }

            Console.Write("\nEnter window size for Moving Average (e.g., 3): ");
            int windowSize = int.Parse(Console.ReadLine());

            var forecast = MovingAverageForecast(revenueData, windowSize);

            Console.WriteLine("\nForecasted Revenue:");
            for (int i = 0; i < forecast.Count; i++)
            {
                Console.WriteLine($"Month {i + windowSize}: ₹{forecast[i]:0.00}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
