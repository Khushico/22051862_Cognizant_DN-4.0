using System;
using System.Threading.Tasks; // For Parallel.Invoke

namespace SingletonPatternExample
{
    // Singleton Logger class
    public sealed class Logger
    {
        // Private static instance
        private static Logger _instance = null;

        // Lock object for thread safety
        private static readonly object _lock = new object();

        // Private constructor
        private Logger()
        {
            Console.WriteLine("Logger instance created");
        }

        // Public static property to access the instance
        public static Logger Instance
        {
            get
            {
                // Double-check locking
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        // Log method
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }

    // Test class
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Singleton Pattern Demo");
            Console.WriteLine("----------------------");

            // Get first instance
            Logger logger1 = Logger.Instance;
            logger1.Log("First log message");

            // Get second instance
            Logger logger2 = Logger.Instance;
            logger2.Log("Second log message");

            // Verify both references point to same instance
            Console.WriteLine($"\nAre logger1 and logger2 the same instance? {logger1 == logger2}");

            // Test thread safety
            Console.WriteLine("\nTesting thread safety...");

            Parallel.Invoke(
                () => {
                    Logger logger3 = Logger.Instance;
                    logger3.Log("Message from thread 1");
                },
                () => {
                    Logger logger4 = Logger.Instance;
                    logger4.Log("Message from thread 2");
                }
            );

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
